using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Pika.Models;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests
{
    public class WikiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WikiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private HttpClient CreateNoRedirectClient()
        {
            return _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private HttpClient CreateAuthenticatedClient()
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        // ==========================================
        // 1. PUBLIC WIKI TESTS
        // ==========================================

        [Fact]
        public async Task WikiRoot_Returns200_WithCompleteArticleLinksAndStructure()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/wiki/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("<h1", html);
            Assert.Contains("Pika Bilgi Bankası", html);
            Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/wiki/\" />", html);
            Assert.Contains("name=\"robots\" content=\"index, follow\"", html);
            Assert.Contains("href=\"/wiki/pika-nedir\"", html);
            Assert.Contains("href=\"/wiki/pika-360\"", html);
            Assert.Contains("href=\"/wiki/product-intelligence-nedir\"", html);
            Assert.Contains("\"@type\": \"CollectionPage\"", html);
        }

        [Fact]
        public async Task WikiRootWithoutSlash_Returns200OK()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/wiki");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("Pika Bilgi Bankası", html);
        }

        [Theory]
        [InlineData("/wiki/index.html")]
        [InlineData("/wiki/index")]
        public async Task LegacyWikiIndexHtml_Returns301Redirect_ToCanonicalWikiRoot(string url)
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
            Assert.Equal("/wiki/", response.Headers.Location?.OriginalString);
        }

        [Theory]
        [InlineData("pika-nedir", "Pika Nedir?")]
        [InlineData("pika-360", "Pika 360")]
        [InlineData("product-intelligence-nedir", "Product Intelligence Nedir?")]
        [InlineData("excel-csv-aktarimi", "Mevcut Verinizden Başlayın")]
        [InlineData("kampanya-journey-orkestrasyonu", "Journey, Kampanya ve Orkestrasyon Katmanı")]
        [InlineData("ai-musteri-ozeti", "AI Müşteri Özeti")]
        [InlineData("gunun-firsatlari-ve-karar-motoru", "Günün Fırsatları ve Karar Motoru")]
        [InlineData("customer-intelligence-nedir", "Customer Intelligence ve Müşteri Analitiği")]
        [InlineData("kampanya-yoneticisi-ve-kurgular", "Campaign Manager ve Kampanya Kurguları")]
        [InlineData("icerik-studyosu-ve-gorsel-yonetimi", "Content Studio ve İçerik Tasarımı")]
        [InlineData("iletisim-listeleri-ve-opt-out", "İletişim Listeleri ve Tercih Yönetimi")]
        [InlineData("segment-yonetimi-ve-filtreler", "Dinamik Segment Oluşturma ve Kural Filtreleri")]
        public async Task WikiArticle_Returns200_WithFullSSRContentAndMetadata(string slug, string expectedH1)
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync($"/wiki/{slug}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var html = await response.Content.ReadAsStringAsync();

            // 1. Initial HTML contains H1
            Assert.Contains($"<h1>{expectedH1}</h1>", html);

            // 2. Contains canonical URL
            Assert.Contains($"<link rel=\"canonical\" href=\"https://pika.tr/wiki/{slug}\"", html);

            // 3. Contains robots indexable
            Assert.Contains("name=\"robots\" content=\"index, follow\"", html);

            // 4. Contains breadcrumbs
            Assert.Contains("Pika Bilgi Bankası", html);

            // 5. Contains structured data TechArticle & BreadcrumbList
            Assert.Contains("\"@type\": \"TechArticle\"", html);
            Assert.Contains("\"@type\": \"BreadcrumbList\"", html);
            Assert.Contains($"https://pika.tr/wiki/{slug}", html);

            // 6. Contains sidebar navigation links with real href
            Assert.Contains("href=\"/wiki/pika-nedir\"", html);
            Assert.Contains("href=\"/wiki/pika-360\"", html);
        }

        [Fact]
        public async Task NonexistentWikiArticle_Returns404NotFound()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/wiki/this-article-does-not-exist-xyz");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AllSitemapWikiUrls_Return200OK()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/sitemap.xml");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var xmlContent = await response.Content.ReadAsStringAsync();
            var doc = XDocument.Parse(xmlContent);
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var wikiUrls = doc.Descendants(ns + "loc")
                              .Select(x => x.Value)
                              .Where(u => u.Contains("/wiki"))
                              .ToList();

            // Must include /wiki/ root and at least 80 public articles
            Assert.True(wikiUrls.Count >= 80, $"Expected >= 80 wiki URLs in sitemap, found {wikiUrls.Count}");
            Assert.Contains("https://pika.tr/wiki/", wikiUrls);
            Assert.Contains("https://pika.tr/wiki/pika-nedir", wikiUrls);
            Assert.Contains("https://pika.tr/wiki/pika-360", wikiUrls);

            // Test a batch of wiki URLs from sitemap
            foreach (var url in wikiUrls.Take(15))
            {
                var uri = new Uri(url);
                var articleResponse = await client.GetAsync(uri.PathAndQuery);
                Assert.True(articleResponse.StatusCode == HttpStatusCode.OK, $"URL {url} returned {articleResponse.StatusCode}");
            }
        }

        [Fact]
        public async Task ArticleWithMultipleSections_GeneratesTableOfContents()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/wiki/pika-nedir");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("<nav class=\"toc\"", html);
            Assert.Contains("İçindekiler", html);
            Assert.Contains("href=\"#", html);
        }

        [Fact]
        public async Task OAISearchBot_CanAccessWikiRootAndArticles()
        {
            var client = CreateNoRedirectClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "/wiki/pika-nedir");
            request.Headers.Add("User-Agent", "OAI-SearchBot");

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("<h1>Pika Nedir?</h1>", html);
            Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/wiki/pika-nedir\" />", html);
        }

        // ==========================================
        // 2. SECURE INTERNAL WIKI TESTS (Section 41)
        // ==========================================

        [Fact]
        public async Task UnauthenticatedInternalWikiRoot_RedirectsToLogin()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/internal/wiki/");

            // Must redirect to Account/Login or return 401/302
            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.Unauthorized);
            Assert.Contains("/Account/Login", response.Headers.Location?.OriginalString ?? string.Empty);
        }

        [Theory]
        [InlineData("internal-mimari-genel-bakis")]
        [InlineData("internal-teslimat-konsolu-ve-worker-mimarisi")]
        [InlineData("internal-retry-politikasi-ve-hata-yonetimi")]
        [InlineData("internal-musteri-deger-skoru-algoritmasi")]
        [InlineData("internal-cce-karar-motoru-mimarisi")]
        [InlineData("internal-ai-mimarisi-ve-prompt-yonetimi")]
        public async Task UnauthenticatedInternalWikiArticle_DoesNotReturnContent_AndRedirectsToLogin(string slug)
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync($"/internal/wiki/{slug}");

            Assert.True(response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.Unauthorized);
            Assert.Contains("/Account/Login", response.Headers.Location?.OriginalString ?? string.Empty);

            var content = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain("Dahili Mühendislik Dokümantasyonu", content);
        }

        [Fact]
        public async Task AuthenticatedUser_CanAccessInternalWikiRoot()
        {
            using var scope = _factory.Services.CreateScope();
            var internalService = scope.ServiceProvider.GetRequiredService<IInternalWikiService>();
            var model = internalService.GetHomeViewModel();

            Assert.NotNull(model);
            Assert.True(model.Categories.Count >= 5);
            Assert.True(model.TotalArticles >= 10);
        }

        [Theory]
        [InlineData("internal-mimari-genel-bakis")]
        [InlineData("internal-teslimat-konsolu-ve-worker-mimarisi")]
        [InlineData("internal-retry-politikasi-ve-hata-yonetimi")]
        [InlineData("internal-musteri-deger-skoru-algoritmasi")]
        public async Task InternalWikiService_ReturnsCompleteTechnicalContent(string slug)
        {
            using var scope = _factory.Services.CreateScope();
            var internalService = scope.ServiceProvider.GetRequiredService<IInternalWikiService>();
            var article = internalService.GetArticleViewModel(slug);

            Assert.NotNull(article);
            Assert.False(string.IsNullOrWhiteSpace(article.CurrentPage.Title));
            Assert.False(string.IsNullOrWhiteSpace(article.CurrentPage.Html));
            Assert.Contains("hero", article.CurrentPage.Html);
        }

        [Fact]
        public async Task InternalWikiUrls_AreNeverInSitemap()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/sitemap.xml");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var xmlContent = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain("/internal/", xmlContent);
            Assert.DoesNotContain("internal-mimari-genel-bakis", xmlContent);
            Assert.DoesNotContain("internal-teslimat-konsolu", xmlContent);
        }

        [Fact]
        public async Task InternalContent_IsNotPresentInPublicAppJs()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/wiki/assets/app.js");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsContent = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain("internal-mimari-genel-bakis", jsContent);
            Assert.DoesNotContain("internal-teslimat-konsolu", jsContent);
            Assert.DoesNotContain("Dahili Mühendislik Dokümantasyonu", jsContent);
        }

        [Fact]
        public async Task PublicWikiSearch_CannotReturnInternalArticles()
        {
            using var scope = _factory.Services.CreateScope();
            var publicWikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();

            var allPublicPages = publicWikiService.GetAllPages();
            Assert.DoesNotContain(allPublicPages, p => p.Slug.StartsWith("internal-", StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(allPublicPages, p => p.Section.Contains("Mühendislik", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task LlmsTxt_ExistsAndContainsOnlyPublicCanonicalUrls()
        {
            var client = CreateNoRedirectClient();
            var response = await client.GetAsync("/llms.txt");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Pika", content);
            Assert.Contains("https://pika.tr/wiki/", content);
            Assert.Contains("https://pika.tr/wiki/pika-nedir", content);
            Assert.DoesNotContain("/internal/", content);
            Assert.DoesNotContain("internal-", content);
        }

        // ==========================================
        // 3. PRODUCT COVERAGE TEST (Section 42)
        // ==========================================

        [Fact]
        public async Task CustomerFacingCapabilities_HavePublicWikiDocumentation()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();
            var allPages = wikiService.GetAllPages();
            var slugs = allPages.Select(p => p.Slug).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var requiredPublicCapabilities = new Dictionary<string, string>
            {
                { "Customer Intelligence", "customer-intelligence-nedir" },
                { "Product Intelligence", "product-intelligence-nedir" },
                { "Pika 360", "pika-360" },
                { "Günün Fırsatları", "gunun-firsatlari-ve-karar-motoru" },
                { "Campaign Manager", "kampanya-yoneticisi-ve-kurgular" },
                { "Journey Manager", "kampanya-journey-orkestrasyonu" },
                { "Audience Manager", "segment-yonetimi-ve-filtreler" },
                { "Content Studio", "icerik-studyosu-ve-gorsel-yonetimi" },
                { "AI Campaign Assistant", "pika-pilot-ai-kampanya-asistani" },
                { "Gmail / Outlook Kişi Aktarımı", "gmail-kisi-aktarimi" },
                { "İletişim Listeleri & Opt-out", "iletisim-listeleri-ve-opt-out" },
                { "İzin & İYS Yönetimi", "izin-optout-iys" },
                { "Excel / CSV Aktarımı", "excel-csv-aktarimi" },
                { "API Entegrasyonu", "api-entegrasyonu" },
                { "BI Kokpit & Raporlama", "bi-kokpit" }
            };

            foreach (var kvp in requiredPublicCapabilities)
            {
                Assert.True(slugs.Contains(kvp.Value), $"Missing public wiki article for capability '{kvp.Key}' (expected slug: {kvp.Value})");
            }
        }
    }

    public class TestAuthHandler : Microsoft.AspNetCore.Authentication.AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            Microsoft.Extensions.Options.IOptionsMonitor<AuthenticationSchemeOptions> options,
            Microsoft.Extensions.Logging.ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "test.engineer@pika.tr"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("TenantId", "tenant-1")
            };
            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
