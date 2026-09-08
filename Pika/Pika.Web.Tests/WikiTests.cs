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

            // 3. Contains robots meta
            var expectedRobots = (slug == "excel-csv-aktarimi" || slug == "iletisim-listeleri-ve-opt-out" || slug == "segment-yonetimi-ve-filtreler")
                ? "name=\"robots\" content=\"noindex, follow\""
                : "name=\"robots\" content=\"index, follow\"";
            Assert.Contains(expectedRobots, html);

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

            // Must include /wiki/ root and exactly 58 indexable public articles (59 total)
            Assert.Equal(59, wikiUrls.Count);
            Assert.Contains("https://pika.tr/wiki/", wikiUrls);
            Assert.Contains("https://pika.tr/wiki/pika-nedir", wikiUrls);
            Assert.Contains("https://pika.tr/wiki/pika-360", wikiUrls);

            // NOINDEX pages must NOT be present in sitemap
            Assert.DoesNotContain("https://pika.tr/wiki/excel-csv-aktarimi", wikiUrls);
            Assert.DoesNotContain("https://pika.tr/wiki/journey-tasarim-tuvali", wikiUrls);
            Assert.DoesNotContain("https://pika.tr/wiki/teslimat-konsolu", wikiUrls);
            Assert.DoesNotContain("https://pika.tr/wiki/gonderim-son-katman", wikiUrls);

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

        // ============================================================
        // 4. CANONICAL TRUTH, SAFETY & INDEXABILITY CONVERGENCE TESTS
        // ============================================================

        [Fact]
        public void PublicInternalBoundary_ContainsNoInternalArticlesOrNav()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();

            var allPages = wikiService.GetAllPages();
            var internalSlugs = allPages.Where(p => p.Slug.StartsWith("internal-", StringComparison.OrdinalIgnoreCase)).ToList();
            Assert.Empty(internalSlugs);

            var homeModel = wikiService.GetHomeViewModel();
            foreach (var group in homeModel.Categories)
            {
                Assert.DoesNotContain("Mühendislik", group.Title, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("Internal", group.Title, StringComparison.OrdinalIgnoreCase);
                foreach (var page in group.Pages)
                {
                    Assert.False(page.Slug.StartsWith("internal-", StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        [Fact]
        public void NavIntegrity_AllSlugsResolve_NoBrokenRelated()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();

            var allPages = wikiService.GetAllPages();
            var pageDict = allPages.ToDictionary(p => p.Slug, StringComparer.OrdinalIgnoreCase);

            // Exactly 88 public articles
            Assert.Equal(88, allPages.Count);

            // Check home nav groups (9 categories, 87 slugs)
            var homeModel = wikiService.GetHomeViewModel();
            Assert.Equal(9, homeModel.Categories.Count);
            var navSlugCount = homeModel.Categories.Sum(g => g.Pages.Count);
            Assert.Equal(87, navSlugCount);

            // Every nav slug must resolve
            foreach (var group in homeModel.Categories)
            {
                foreach (var summary in group.Pages)
                {
                    Assert.True(pageDict.ContainsKey(summary.Slug), $"Nav slug '{summary.Slug}' not found in public wiki pages");
                    var fullPage = wikiService.GetPage(summary.Slug);
                    Assert.NotNull(fullPage);
                    Assert.False(string.IsNullOrWhiteSpace(fullPage.Title));
                    Assert.False(string.IsNullOrWhiteSpace(fullPage.Summary));
                    Assert.False(string.IsNullOrWhiteSpace(fullPage.Html));
                }
            }

            // All related links must resolve within public wiki
            foreach (var summary in allPages)
            {
                var full = wikiService.GetPage(summary.Slug);
                if (full?.Related != null)
                {
                    foreach (var rel in full.Related)
                    {
                        Assert.True(pageDict.ContainsKey(rel), $"Article '{full.Slug}' has broken related link: '{rel}'");
                    }
                }
            }
        }

        [Fact]
        public void CustomerValueScore_ReflectsCanonicalFourFactorFormula_OmitsRhythmFromScore()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();
            var allPages = wikiService.GetAllPages();

            // 1. Absence of obsolete "%60 ciro + %40 sıklık" or "%60 + %40"
            foreach (var summary in allPages)
            {
                var full = wikiService.GetPage(summary.Slug)!;
                Assert.DoesNotContain("%60", full.Html);
                Assert.DoesNotContain("%60", full.Summary);
            }

            // 2. musteri-deger-skoru must feature the canonical 4-factor formula
            var mds = wikiService.GetPage("musteri-deger-skoru");
            Assert.NotNull(mds);
            Assert.Contains("0.40", mds.Html);
            Assert.Contains("0.25", mds.Html);
            Assert.Contains("0.20", mds.Html);
            Assert.Contains("0.15", mds.Html);
            Assert.Contains("Monetary", mds.Html);
            Assert.Contains("Frequency", mds.Html);
            Assert.Contains("Recency", mds.Html);
            Assert.Contains("Loyalty", mds.Html);

            // 3. Rhythm must be defined as independent behavioral context, NOT a score factor
            Assert.Contains("Alışveriş Ritmi (Rhythm)", mds.Html);
            Assert.Contains("bağımsız bir davranışsal bağlam sinyalidir", mds.Html);

            // 4. musteri-degeri-sadakat must also reflect canonical formula
            var mdsad = wikiService.GetPage("musteri-degeri-sadakat");
            Assert.NotNull(mdsad);
            Assert.Contains("0.40", mdsad.Html);
            Assert.Contains("0.25", mdsad.Html);
            Assert.Contains("0.20", mdsad.Html);
            Assert.Contains("0.15", mdsad.Html);
        }

        [Fact]
        public void Channels_DoNotMarketPushAsActiveChannel_EmailSmsWhatsAppOnly()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();
            var allPages = wikiService.GetAllPages();

            foreach (var summary in allPages)
            {
                var full = wikiService.GetPage(summary.Slug)!;
                Assert.DoesNotContain("Push bildirim", full.Html, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("Push notification", full.Html, StringComparison.OrdinalIgnoreCase);
            }

            // Verify active channel trio in kampanya-kanallari-ve-rol-dagilimi
            var pageKanal = wikiService.GetPage("kampanya-kanallari-ve-rol-dagilimi");
            Assert.NotNull(pageKanal);
            Assert.Contains("E-posta", pageKanal.Html);
            Assert.Contains("SMS", pageKanal.Html);
            Assert.Contains("WhatsApp", pageKanal.Html);
            Assert.DoesNotContain("Push", pageKanal.Html);
        }

        [Fact]
        public void RepeatPurchase_ExcludesUnsupportedForecastsAndProbabilities_UsesCycleWindow()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();

            var page = wikiService.GetPage("tekrar-satin-alma-analizi");
            Assert.NotNull(page);

            // Must use canonical cycle window framing
            Assert.Contains("%80–%120", page.Summary);
            Assert.Contains("%80–%120", page.Html);
            Assert.Contains("döngü penceresi", page.Html, StringComparison.OrdinalIgnoreCase);

            // Must NOT contain unsupported predictive claims or fake probabilities
            Assert.DoesNotContain("tahmin güveni", page.Summary, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("olası alışveriş tutarları", page.Html, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("tahmin güveni", page.Html, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("%87 olasılıkla", page.Html);
        }

        [Fact]
        public void AiRole_AssertsHumanInTheLoop_NoAutonomousSending()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();

            var aiPage = wikiService.GetPage("ai-rolu-guven-siniri");
            Assert.NotNull(aiPage);
            Assert.Contains("İnsan &amp; Yönetici", aiPage.Html);
            Assert.Contains("Son karar, onay", aiPage.Html);

            var nbaPage = wikiService.GetPage("next-best-action");
            Assert.NotNull(nbaPage);
            Assert.Contains("deterministik kurallarla", nbaPage.Html);
            Assert.Contains("otonom bir yapay zekâ göndericisi değildir", nbaPage.Html);
        }

        [Fact]
        public void OpportunityConfidence_FramesAsEvidenceQuality_NotStandaloneScoreEngine()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();

            var page = wikiService.GetPage("firsat-guveni-kanit");
            Assert.NotNull(page);
            Assert.Contains("Kanıt Yeterliliği Seviyesi", page.Html);
            Assert.Contains("Sınırlı Kanıt", page.Html);
            Assert.Contains("Gelişen Kanıt", page.Html);
            Assert.Contains("Güçlü Kanıt", page.Html);
            Assert.Contains("bağımsız bir 'güven skoru motoru' veya yapay satın alma olasılığı çalıştırmaz", page.Html);
        }

        [Fact]
        public void HighRiskClaims_AbsenceOfSoc2Iso27001SamlSsoAndGuarantees()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();
            var allPages = wikiService.GetAllPages();

            var prohibitedTerms = new[]
            {
                "SOC 2", "SOC2", "ISO 27001", "ISO27001",
                "SAML SSO", "99.99%", "garantili ciro",
                "kesin ciro artışı", "kesin satış garantisi"
            };

            foreach (var summary in allPages)
            {
                var full = wikiService.GetPage(summary.Slug)!;
                foreach (var term in prohibitedTerms)
                {
                    Assert.DoesNotContain(term, full.Html, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(term, full.Summary, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        [Fact]
        public async Task RobotsMeta_EmitsSingleTag_IndexFollowOrNoindexFollow()
        {
            var client = CreateNoRedirectClient();

            // 1. Check INDEX page (e.g. pika-nedir)
            var indexResp = await client.GetAsync("/wiki/pika-nedir");
            Assert.Equal(HttpStatusCode.OK, indexResp.StatusCode);
            var indexHtml = await indexResp.Content.ReadAsStringAsync();
            Assert.Contains("<meta name=\"robots\" content=\"index, follow\">", indexHtml);
            Assert.DoesNotContain("noindex", indexHtml);

            // 2. Check NOINDEX page (e.g. excel-csv-aktarimi)
            var noindexResp = await client.GetAsync("/wiki/excel-csv-aktarimi");
            Assert.Equal(HttpStatusCode.OK, noindexResp.StatusCode);
            var noindexHtml = await noindexResp.Content.ReadAsStringAsync();
            Assert.Contains("<meta name=\"robots\" content=\"noindex, follow\">", noindexHtml);
            Assert.DoesNotContain("<meta name=\"robots\" content=\"index, follow\">", noindexHtml);

            // Count occurrences of meta robots in each
            var indexMatches = System.Text.RegularExpressions.Regex.Matches(indexHtml, @"<meta\s+name=""robots""", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Assert.Single(indexMatches);

            var noindexMatches = System.Text.RegularExpressions.Regex.Matches(noindexHtml, @"<meta\s+name=""robots""", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Assert.Single(noindexMatches);
        }

        [Fact]
        public void AssetIntegrity_AllReferencedImagesExistOnDisk()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();
            var allPages = wikiService.GetAllPages();

            var webRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "wwwroot"));
            var imageRegex = new System.Text.RegularExpressions.Regex(@"src=""(/wiki/assets/images/[^""]+)""", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var foundImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var summary in allPages)
            {
                var full = wikiService.GetPage(summary.Slug)!;
                var matches = imageRegex.Matches(full.Html);
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    var relativeUrl = match.Groups[1].Value;
                    foundImages.Add(relativeUrl);

                    var localPath = Path.Combine(webRoot, relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    Assert.True(File.Exists(localPath), $"Image '{relativeUrl}' referenced in article '{full.Slug}' does not exist on disk at '{localPath}'");
                }
            }

            // Exactly 29 unique screenshots referenced across public Wiki
            Assert.Equal(29, foundImages.Count);
        }

        [Fact]
        public void WikiGovernance_GovernanceCountsMatch58Index30Noindex()
        {
            using var scope = _factory.Services.CreateScope();
            var wikiService = scope.ServiceProvider.GetRequiredService<IWikiService>();
            var allPages = wikiService.GetAllPages();

            var indexableCount = allPages.Count(p => p.Indexable);
            var noindexCount = allPages.Count(p => !p.Indexable);

            Assert.Equal(88, allPages.Count);
            Assert.Equal(58, indexableCount);
            Assert.Equal(30, noindexCount);
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
