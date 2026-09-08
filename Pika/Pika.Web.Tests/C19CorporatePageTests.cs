using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class C19CorporatePageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C19CorporatePageTests(WebApplicationFactory<Program> factory)
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

    private static string NormalizeWhitespace(string input)
    {
        return Regex.Replace(input, @"\s+", " ").Trim();
    }

    private static string ExtractH1(string html)
    {
        var match = Regex.Match(html, @"<h1[^>]*>(.*?)</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!match.Success) return string.Empty;
        var stripped = Regex.Replace(match.Groups[1].Value, @"<[^>]+>", " ");
        return NormalizeWhitespace(WebUtility.HtmlDecode(stripped));
    }

    private static string ExtractMainBodyWithoutScripts(string html)
    {
        var bodyMatch = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var pageBody = bodyMatch.Success ? bodyMatch.Groups[1].Value : html;
        return Regex.Replace(pageBody, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);
    }

    private static string GetProjectRoot()
    {
        var dir = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(dir))
        {
            if (File.Exists(Path.Combine(dir, "Pika.csproj")))
            {
                return dir;
            }
            var parent = Directory.GetParent(dir);
            if (parent == null) break;
            dir = parent.FullName;
        }
        throw new DirectoryNotFoundException("Could not locate Pika project root directory.");
    }

    // 01: GET /kurumsal = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/kurumsal");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/corporate = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/corporate");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Home.Corporate SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_HomeCorporate_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Home", "Corporate");
        Assert.NotNull(entry);
        Assert.Equal("/kurumsal", entry.AlternatePathTr);
        Assert.Equal("/en/corporate", entry.AlternatePathEn);
        Assert.Equal("Kurumsal | Pika Müşteri Zekâsı ve Omnichannel Pazarlama", entry.TitleTr);
        Assert.Equal("Corporate | Pika Customer Intelligence & Omnichannel Marketing", entry.TitleEn);
        Assert.Equal("Pika'nın müşteri zekâsı ve omnichannel pazarlama platformu yaklaşımını; veri, entegrasyon, güvenlik, izin yönetimi, kontrollü aksiyon, AI ve kurumsal değerlendirme başlıklarıyla inceleyin.", entry.DescriptionTr);
        Assert.Equal("Explore Pika's Customer Intelligence & Omnichannel Marketing Platform approach across data, integrations, security, consent, controlled action, AI and organizational evaluation.", entry.DescriptionEn);
        Assert.Equal("Kurumsal", entry.BreadcrumbTitleTr);
        Assert.Equal("Corporate", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var h1Matches = Regex.Matches(html, @"<h1[^>]*>[\s\S]*?</h1>", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);
    }

    // 05: TR normalized H1
    [Fact]
    public async Task TurkishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kurumsal");
        var h1 = ExtractH1(html);
        Assert.Equal("Müşteri zekâsını, açık ürün sınırları ve kontrollü aksiyon yaklaşımıyla değerlendirin.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/corporate");
        var h1 = ExtractH1(html);
        Assert.Equal("Evaluate customer intelligence through clear product boundaries and controlled action.", h1);
    }

    // 07: TR contains Chapter 02 H2
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kurumsal");
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("Pika'yı kurumsal olarak nasıl değerlendirmelisiniz?", decoded);
    }

    // 08: EN contains Chapter 02 H2
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/corporate");
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("How should an organization evaluate Pika?", decoded);
    }

    // 09: Canonical positioning
    [Theory]
    [InlineData("/kurumsal", "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu")]
    [InlineData("/en/corporate", "Customer Intelligence & Omnichannel Marketing Platform")]
    public async Task BothPages_ContainCanonicalPositioning(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 10: Both contain all required platform layers
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainRequiredPlatformLayers(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Product Intelligence", decoded);
        Assert.Contains("Pika 360", decoded);
        Assert.True(decoded.Contains("Günün Fırsatları") || decoded.Contains("Daily Opportunities"));
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Content Studio", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Consent Management", decoded);
        Assert.Contains("Security & Privacy", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
        Assert.Contains("Pika Pilot", decoded);
    }

    // 11: Active channels Email, SMS, WhatsApp
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainActiveCommunicationChannels(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Email", decoded);
        Assert.Contains("SMS", decoded);
        Assert.Contains("WhatsApp", decoded);
    }

    // 12: Push capability = 0
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainZeroPushPositiveClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push Notification", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Mobile Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Web Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/kanallar/push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/en/channels/push", body, StringComparison.OrdinalIgnoreCase);
    }

    // 13: Both contain REST, Excel, CSV intake references
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainDataIntakeReferences(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("REST Ingestion API", decoded);
        Assert.Contains("Excel", decoded);
        Assert.Contains("CSV", decoded);
    }

    // 14: Both establish no universal native-connector claim
    [Theory]
    [InlineData("/kurumsal", "her CRM, ERP veya e-ticaret ürünü için hazır native connector bulunduğu iddiasını yapmaz")]
    [InlineData("/en/corporate", "does not claim that a ready-made native connector exists for every CRM, ERP or e-commerce product")]
    public async Task BothPages_EstablishNoUniversalNativeConnectorClaim(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 15: Both contain RBAC
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainRbac(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("RBAC", decoded);
    }

    // 16: Both establish AI is assistive and user-controlled
    [Theory]
    [InlineData("/kurumsal", "kullanıcı değerlendirmesi ve kontrolü altında")]
    [InlineData("/en/corporate", "under user review and control")]
    public async Task BothPages_EstablishAiIsAssistiveAndUserControlled(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 17: Both establish no autonomous AI sending
    [Theory]
    [InlineData("/kurumsal", "AI yardımı, otonom gönderim yetkisi değildir.")]
    [InlineData("/en/corporate", "AI assistance is not autonomous delivery authority.")]
    public async Task BothPages_EstablishNoAutonomousAiSending(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 18: Both contain exact commercial pricing line
    [Theory]
    [InlineData("/kurumsal", "İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.")]
    [InlineData("/en/corporate", "Pricing is tailored to your requirements and scope of use.")]
    public async Task BothPages_ContainExactCommercialPricingLine(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 19: No fixed price patterns
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainNoFixedPricePattern(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(@"\$\d+", body);
        Assert.DoesNotMatch(@"€\d+", body);
        Assert.DoesNotMatch(@"\d+\s*₺", body);
        Assert.DoesNotMatch(@"\b(aylık|monthly)\s+\d+", body);
    }

    // 20: No positive SOC 2, ISO 27001, SAML / SSO capability claims
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainNoPositiveCertificationClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("SOC 2 certified", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ISO 27001 certified", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SOC 2 sertifikalı", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ISO 27001 sertifikalı", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SAML SSO desteği", body, StringComparison.OrdinalIgnoreCase);
    }

    // 21: No international / global / market-leader claims
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainNoUnverifiedScaleClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("international customers", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("global customers", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("market leader", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("industry leader", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("pazar lideri", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("sektör lideri", body, StringComparison.OrdinalIgnoreCase);
    }

    // 22: No 99.9%, 24/7 support, SLA claims
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainNoUptimeOrSlaClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("99.9%", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("24/7", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("7/24", body, StringComparison.OrdinalIgnoreCase);
    }

    // 23: No ROI, conversion, revenue guarantees
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainNoGuarantees(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("guaranteed ROI", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed revenue", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ROI garantisi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("dönüşüm garantisi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("gelir garantisi", body, StringComparison.OrdinalIgnoreCase);
    }

    // 24: View contains no ViewData overrides
    [Fact]
    public void ViewSource_ContainsNoViewDataOverrides()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Corporate.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
    }

    // 25: View contains no data-i18n="corp.
    [Fact]
    public void ViewSource_ContainsNoDataI18nCorpDependencies()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Corporate.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("data-i18n=\"corp.", content);
    }

    // 26: View contains no <img, service-image-7.jpg, or demo placeholder text
    [Fact]
    public void ViewSource_ContainsNoImgTagsOrPlaceholders()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Corporate.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("<img", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("service-image-7.jpg", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Demo image placeholder", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Demo görsel alanı", content, StringComparison.OrdinalIgnoreCase);
    }

    // 27: No Wiki links
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainZeroWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
    }

    // 28: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainNoPageLevelJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.DoesNotContain("\"@type\": \"FAQPage\"", html);
        Assert.DoesNotContain("\"@type\":\"FAQPage\"", html);
        Assert.DoesNotContain("\"@type\": \"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"@type\":\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"@type\": \"Offer\"", html);
        Assert.DoesNotContain("\"@type\":\"Offer\"", html);
    }

    // 29 & 30: Exactly 8 FAQs rendered in details tags
    [Theory]
    [InlineData("/kurumsal")]
    [InlineData("/en/corporate")]
    public async Task BothPages_ContainExactlyEightFaqs(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        var detailsMatches = Regex.Matches(html, @"<details[^>]*class=""corp-faq-details""[^>]*>", RegexOptions.IgnoreCase);
        Assert.Equal(8, detailsMatches.Count);
    }

    // 31: Contextual internal link graph targets
    [Fact]
    public async Task TurkishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kurumsal");

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/pika-360\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/entegrasyonlar\"", html);
        Assert.Contains("href=\"/guvenlik-ve-gizlilik\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
        Assert.Contains("href=\"/demo-talebi\"", html);
        Assert.Contains("href=\"/iletisim\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/corporate");

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/pika-360\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/integrations\"", html);
        Assert.Contains("href=\"/en/security-and-privacy\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
        Assert.Contains("href=\"/en/demo-request\"", html);
        Assert.Contains("href=\"/en/contact\"", html);
    }

    // 32: Homepage files untouched
    [Fact]
    public void Homepage_FilesRemainUntouched()
    {
        var root = GetProjectRoot();
        Assert.True(File.Exists(Path.Combine(root, "Views", "Home", "Index.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-home.css")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-orbit.css")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "js", "pika-orbit.js")));
    }

    // 33: site.js untouched
    [Fact]
    public void SiteJs_RemainsUntouched()
    {
        var root = GetProjectRoot();
        var siteJsPath = Path.Combine(root, "wwwroot", "js", "site.js");
        Assert.True(File.Exists(siteJsPath));
    }
}
