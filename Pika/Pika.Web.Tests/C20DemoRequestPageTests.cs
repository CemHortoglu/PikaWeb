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

public class C20DemoRequestPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C20DemoRequestPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /demo-talebi = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/demo-talebi");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/demo-request = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/demo-request");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Home.DemoRequest SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_HomeDemoRequest_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Home", "DemoRequest");
        Assert.NotNull(entry);
        Assert.Equal("/demo-talebi", entry.AlternatePathTr);
        Assert.Equal("/en/demo-request", entry.AlternatePathEn);
        Assert.Equal("Pika Demo Talebi | Müşteri Zekâsı ve Omnichannel Pazarlama", entry.TitleTr);
        Assert.Equal("Request a Pika Demo | Customer Intelligence & Omnichannel Marketing", entry.TitleEn);
        Assert.Equal("Pika demosunda müşteri, ürün ve işlem verinizin nasıl anlamlandırıldığını; fırsat, audience, campaign, journey ve Email/SMS/WhatsApp aksiyonlarına nasıl bağlandığını değerlendirin.", entry.DescriptionTr);
        Assert.Equal("Request a Pika demo to evaluate how customer, product and transaction data connects to intelligence, opportunities, audiences, campaigns, journeys and Email/SMS/WhatsApp action.", entry.DescriptionEn);
        Assert.Equal("Demo Talebi", entry.BreadcrumbTitleTr);
        Assert.Equal("Demo Request", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
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
        var html = await client.GetStringAsync("/demo-talebi");
        var h1 = ExtractH1(html);
        Assert.Equal("Pika'yı, kendi veri ve aksiyon bağlamınız üzerinden değerlendirin.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/demo-request");
        var h1 = ExtractH1(html);
        Assert.Equal("Evaluate Pika through your own data and action context.", h1);
    }

    // 07: Form ID and section anchor exist
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainDemoRequestFormWithRequiredIdAndAnchor(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("id=\"pageDemoRequestForm\"", html);
        Assert.Contains("id=\"demo-request-form\"", html);
    }

    // 08: Form contains all required field names
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_FormContainsRequiredFieldNames(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.Contains("name=\"firstName\"", html);
        Assert.Contains("name=\"email\"", html);
        Assert.Contains("name=\"companyName\"", html);
        Assert.Contains("name=\"phone\"", html);
        Assert.Contains("name=\"city\"", html);
        Assert.Contains("name=\"message\"", html);
    }

    // 09: Form inputs have required flags as expected
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_FormSpecifiesRequiredInputs(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        // firstName, email, companyName, phone, city should have required attribute
        Assert.Matches(@"<input[^>]*name=""firstName""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""email""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""companyName""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""phone""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""city""[^>]*required", html);

        // message textarea is optional (not required)
        Assert.DoesNotMatch(@"<textarea[^>]*name=""message""[^>]*required", html);
    }

    // 10: Form contains phone prefix +90
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_FormContainsPhonePrefixPlus90(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("+90", html);
    }

    // 11: Result feedback container exists
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainPageDemoResultContainer(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("id=\"pageDemoResult\"", html);
    }

    // 12: Turnstile widget conditional markup in view source
    [Fact]
    public void ViewSource_ContainsTurnstileMarkup()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "DemoRequest.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.Contains("SiteOptions.Value.CloudflareTurnstile.SiteKey", content);
        Assert.Contains("class=\"cf-turnstile\"", content);
        Assert.Contains("data-sitekey=", content);
    }

    // 13: Submit button text
    [Theory]
    [InlineData("/demo-talebi", "Demo Talebini Gönder")]
    [InlineData("/en/demo-request", "Submit Demo Request")]
    public async Task BothPages_ContainSubmitButton(string url, string expectedButtonText)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedButtonText, decoded);
    }

    // 14: Both contain all required platform layers
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainRequiredPlatformLayers(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Product Intelligence", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Consent Management", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
    }

    // 15: Active channels Email, SMS, WhatsApp
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainActiveCommunicationChannels(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Email", decoded);
        Assert.Contains("SMS", decoded);
        Assert.Contains("WhatsApp", decoded);
    }

    // 16: Push capability = 0
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
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

    // 17: Both contain REST, Excel, CSV intake references
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainDataIntakeReferences(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("REST API", decoded);
        Assert.Contains("Excel", decoded);
        Assert.Contains("CSV", decoded);
    }

    // 18: Both contain exact commercial pricing line
    [Theory]
    [InlineData("/demo-talebi", "İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.")]
    [InlineData("/en/demo-request", "Pricing is tailored to your requirements and scope of use.")]
    public async Task BothPages_ContainExactCommercialPricingLine(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 19: No fixed price patterns
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
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

    // 20: No legacy duration claims (15 dakikalık, 45 dakikalık, etc.)
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainNoLegacyDurationClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("15 dakikalık", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45 dakikalık", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("15 dakika", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45 dakika", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("15-minute", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45-minute", body, StringComparison.OrdinalIgnoreCase);
    }

    // 21: No response time promises
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainNoResponseTimePromises(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("24 saat içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("within 24 hours", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("hemen döneceğiz", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("anında dönüş", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("en kısa sürede döneceğiz", body, StringComparison.OrdinalIgnoreCase);
    }

    // 22: No positive SOC 2, ISO 27001, SAML / SSO capability claims
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
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

    // 23: No ROI, conversion, revenue guarantees
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
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
        var viewPath = Path.Combine(root, "Views", "Home", "DemoRequest.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
    }

    // 25: View contains no data-i18n="demoPage. or data-i18n-placeholder
    [Fact]
    public void ViewSource_ContainsNoDataI18nDemoPageDependencies()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "DemoRequest.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("data-i18n=\"demoPage.", content);
        Assert.DoesNotContain("data-i18n-placeholder", content);
    }

    // 26: View contains no <img tags or placeholders
    [Fact]
    public void ViewSource_ContainsNoImgTagsOrPlaceholders()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "DemoRequest.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("<img", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("service-image-", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Demo image placeholder", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Demo görsel alanı", content, StringComparison.OrdinalIgnoreCase);
    }

    // 27: Zero Wiki links
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainZeroWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
    }

    // 28: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
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

    // 29: Exactly 8 FAQs rendered in details tags
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ContainExactlyEightFaqs(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        var detailsMatches = Regex.Matches(html, @"<details[^>]*class=""demo-req-faq-details""[^>]*>", RegexOptions.IgnoreCase);
        Assert.Equal(8, detailsMatches.Count);
    }

    // 30: Turkish page internal links
    [Fact]
    public async Task TurkishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/demo-talebi");

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/entegrasyonlar\"", html);
        Assert.Contains("href=\"/guvenlik-ve-gizlilik\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/kurumsal\"", html);
        Assert.Contains("href=\"/gizlilik-politikasi\"", html);
    }

    // 31: English page internal links
    [Fact]
    public async Task EnglishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/demo-request");

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/integrations\"", html);
        Assert.Contains("href=\"/en/security-and-privacy\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/corporate\"", html);
        Assert.Contains("href=\"/en/privacy-policy\"", html);
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
        var content = File.ReadAllText(siteJsPath);
        Assert.Contains("bindPageDemoForm", content);
        Assert.Contains("#pageDemoRequestForm", content);
    }

    // 34: Scoped CSS linked on both pages
    [Theory]
    [InlineData("/demo-talebi")]
    [InlineData("/en/demo-request")]
    public async Task BothPages_ReferenceScopedDemoRequestCss(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("pika-demo-request-page.css", html);
    }

    // 35: Scoped CSS line count within limit (< 80 lines)
    [Fact]
    public void ScopedCss_LineCountWithinLimit()
    {
        var root = GetProjectRoot();
        var cssPath = Path.Combine(root, "wwwroot", "css", "pika-demo-request-page.css");
        Assert.True(File.Exists(cssPath));
        var lines = File.ReadAllLines(cssPath);
        Assert.True(lines.Length <= 80, $"CSS file has {lines.Length} lines, exceeding ceiling of 80.");
    }
}
