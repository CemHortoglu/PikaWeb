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

public class C13IntegrationsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C13IntegrationsPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /entegrasyonlar = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/entegrasyonlar");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/integrations = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/integrations");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.Integrations SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsIntegrations_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "Integrations");
        Assert.NotNull(entry);
        Assert.Equal("/entegrasyonlar", entry.AlternatePathTr);
        Assert.Equal("/en/integrations", entry.AlternatePathEn);
        Assert.Equal("Entegrasyonlar | REST API, Excel/CSV ve Veri Aktarımı | Pika", entry.TitleTr);
        Assert.Equal("Integrations | REST API, Excel/CSV & Data Ingestion | Pika", entry.TitleEn);
        Assert.Equal("Pika Entegrasyonlar; müşteri, ürün ve işlem verilerini asenkron REST Ingestion API veya Excel/CSV içe aktarma akışlarıyla Pika'ya taşımanıza yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Integrations helps bring customer, product and transaction data into Pika through an asynchronous REST Ingestion API and Excel/CSV import flows.", entry.DescriptionEn);
        Assert.Equal("Entegrasyonlar", entry.BreadcrumbTitleTr);
        Assert.Equal("Integrations", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per language
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task Page_HasExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var matches = Regex.Matches(html, @"<h1\b[^>]*>", RegexOptions.IgnoreCase);
        Assert.Single(matches);
    }

    // 05: TR normalized H1
    [Fact]
    public async Task TurkishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");
        var h1 = ExtractH1(html);
        Assert.Equal("Müşteri, ürün ve işlem verisini Pika'nın karar zincirine bağlayın.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");
        var h1 = ExtractH1(html);
        Assert.Equal("Connect customer, product and transaction data to Pika's decision chain.", h1);
    }

    // 07: TR contains: Pika Entegrasyonlar nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");
        Assert.Contains("Pika Entegrasyonlar nedir?", html);
    }

    // 08: EN contains: What are Pika Integrations?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");
        Assert.Contains("What are Pika Integrations?", html);
    }

    // 09: Both contain Customer Intelligence, Product Intelligence, Pika 360
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_ContainPlatformDownstreamEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("Customer Intelligence", html);
        Assert.Contains("Product Intelligence", html);
        Assert.Contains("Pika 360", html);
    }

    // 10: TR contains: Günün Fırsatları
    [Fact]
    public async Task TurkishPage_ContainsOpportunitiesTurkishEntity()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");
        Assert.Contains("Günün Fırsatları", html);
    }

    // 11: EN contains: Daily Opportunities
    [Fact]
    public async Task EnglishPage_ContainsOpportunitiesEnglishEntity()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");
        Assert.Contains("Daily Opportunities", html);
    }

    // 12: Both contain Excel, CSV, REST, /api/v1/ingest/
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_ContainCoreIngestionMechanisms(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("Excel", html);
        Assert.Contains("CSV", html);
        Assert.Contains("REST", html);
        Assert.Contains("/api/v1/ingest/", html);
    }

    // 13: Both contain X-API-Key, X-Idempotency-Key
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_ContainDocumentedHeaders(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("X-API-Key", html);
        Assert.Contains("X-Idempotency-Key", html);
    }

    // 14: Both contain canonical domains: customer / müşteri, product / ürün, transaction / işlem
    [Fact]
    public async Task TurkishPage_ContainsCanonicalDomainsTurkish()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");
        Assert.Contains("Müşteri", html);
        Assert.Contains("Ürün", html);
        Assert.Contains("İşlem", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsCanonicalDomainsEnglish()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");
        Assert.Contains("Customer", html);
        Assert.Contains("Product", html);
        Assert.Contains("Transaction", html);
    }

    // 15: Page states REST ingestion is asynchronous
    [Theory]
    [InlineData("/entegrasyonlar", "asenkron")]
    [InlineData("/en/integrations", "asynchronous")]
    public async Task BothPages_StateRestIngestionIsAsynchronous(string url, string keyword)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains(keyword, html, StringComparison.OrdinalIgnoreCase);
    }

    // 16: Page contains no positive real-time claim (while permitting valid negative boundarycopy)
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoPositiveRealTimeClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\breal-time integration\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\blive data sync\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\binstant synchronization\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bmillisecond ingestion\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bzero-latency\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bcontinuous streaming\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bgerçek zamanlı entegrasyon\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\banlık senkronizasyon\b", RegexOptions.IgnoreCase), body);
    }

    // 17: Page states native connector availability is NOT universal
    [Theory]
    [InlineData("/entegrasyonlar", "native connector")]
    [InlineData("/en/integrations", "native connector")]
    public async Task BothPages_StateNativeConnectorNotUniversal(string url, string phrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains(phrase, html, StringComparison.OrdinalIgnoreCase);
    }

    // 18: Page does not contain named connector claims
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_DoNotContainNamedConnectorBrands(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        string[] forbiddenBrands = { "Salesforce", "HubSpot", "Shopify", "SAP", "Dynamics", "Magento", "WooCommerce", "Nebim" };
        foreach (var brand in forbiddenBrands)
        {
            Assert.DoesNotMatch(new Regex($@"\b{brand}\b", RegexOptions.IgnoreCase), body);
        }
    }

    // 19: Page states ingestion does not automatically launch campaigns
    [Theory]
    [InlineData("/entegrasyonlar", "otomatik olarak kampanya")]
    [InlineData("/en/integrations", "automatically launched")]
    public async Task BothPages_StateIngestionDoesNotAutomaticallyLaunchCampaigns(string url, string phrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains(phrase, html, StringComparison.OrdinalIgnoreCase);
    }

    // 20: Page distinguishes request idempotency vs data resolution
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_DistinguishIdempotencyFromDataResolution(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("Request idempotency", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Data resolution", html, StringComparison.OrdinalIgnoreCase);
    }

    // 21: View contains no ViewData["Title"], etc.
    [Fact]
    public void IntegrationsView_ContainsNoLegacyDirectives()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "Integrations.cshtml");
        var text = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", text);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", text);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", text);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", text);
        Assert.DoesNotContain("@section JsonLd", text);
        Assert.DoesNotContain("/wiki/", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<img", text, StringComparison.OrdinalIgnoreCase);
    }

    // 22: No product screenshot
    [Fact]
    public void IntegrationsView_ContainsNoScreenshotReferences()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "Integrations.cshtml");
        var text = File.ReadAllText(viewPath);

        Assert.DoesNotContain("screenshot", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(".png", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(".jpg", text, StringComparison.OrdinalIgnoreCase);
    }

    // 23: No positive Push capability
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoPositivePushClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\bMobile Push\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bWeb Push\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bPush Notification\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bPush: ON\b", RegexOptions.IgnoreCase), body);
    }

    // 24: No fake API metrics
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoFakeApiMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\b99\.99%\b"), body);
        Assert.DoesNotMatch(new Regex(@"\breq/sec\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\brps\b", RegexOptions.IgnoreCase), body);
    }

    // 25: No fake throughput / latency values
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoFakeThroughputOrLatency(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\b\d+\s*ms\b"), body);
        Assert.DoesNotMatch(new Regex(@"\b\d+\s*milliseconds\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\b\d+\s*milisaniye\b", RegexOptions.IgnoreCase), body);
    }

    // 26: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_HasExactly8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");
        var matches = Regex.Matches(html, @"<summary class=""intg-faq-summary"">([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, matches.Count);

        string[] expectedTrQuestions = {
            "Pika Entegrasyonlar nedir?",
            "Pika'ya Excel veya CSV ile veri aktarılabilir mi?",
            "Pika'nın veri aktarımı için REST API'si var mı?",
            "REST Ingestion API gerçek zamanlı mı çalışır?",
            "X-Idempotency-Key ne işe yarar?",
            "Pika'nın her CRM veya ERP için hazır entegrasyonu var mı?",
            "Pika'ya veri gelmesi kampanyayı otomatik olarak başlatır mı?",
            "Veri Pika'ya girdikten sonra ne olur?"
        };

        for (int i = 0; i < expectedTrQuestions.Length; i++)
        {
            var summaryText = NormalizeWhitespace(WebUtility.HtmlDecode(matches[i].Groups[1].Value));
            Assert.Equal(expectedTrQuestions[i], summaryText);
        }
    }

    // 27: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_HasExactly8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");
        var matches = Regex.Matches(html, @"<summary class=""intg-faq-summary"">([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, matches.Count);

        string[] expectedEnQuestions = {
            "What are Pika Integrations?",
            "Can data be imported into Pika through Excel or CSV?",
            "Does Pika provide a REST API for data ingestion?",
            "Does the REST Ingestion API operate in real time?",
            "What is X-Idempotency-Key used for?",
            "Does Pika have a ready-made integration for every CRM or ERP?",
            "Does data arriving in Pika automatically launch a campaign?",
            "What happens after data enters Pika?"
        };

        for (int i = 0; i < expectedEnQuestions.Length; i++)
        {
            var summaryText = NormalizeWhitespace(WebUtility.HtmlDecode(matches[i].Groups[1].Value));
            Assert.Equal(expectedEnQuestions[i], summaryText);
        }
    }

    // 28: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoPageLevelSchemaTypes(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.DoesNotContain("\"FAQPage\"", html);
        Assert.DoesNotContain("\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"Offer\"", html);
    }

    // 29: Correct internal links
    [Fact]
    public async Task TurkishPage_HasCorrectInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");

        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/pika-360\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/guvenlik-ve-gizlilik\"", html);
    }

    [Fact]
    public async Task EnglishPage_HasCorrectInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");

        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/pika-360\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/security-and-privacy\"", html);
    }

    // 30: Exact pricing lines
    [Fact]
    public async Task TurkishPage_ContainsExactPricingLine()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/entegrasyonlar");
        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingLine()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/integrations");
        Assert.Contains("Pricing is tailored to your requirements and scope of use.", html);
    }

    // 31: No fixed public price patterns
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoFixedPricePatterns(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"₺\s*\d+"), body);
        Assert.DoesNotMatch(new Regex(@"\$\s*\d+"), body);
        Assert.DoesNotMatch(new Regex(@"\b\d+\s*₺"), body);
        Assert.DoesNotMatch(new Regex(@"\b\d+\s*\$"), body);
        Assert.DoesNotMatch(new Regex(@"/\s*ay\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"/\s*month\b", RegexOptions.IgnoreCase), body);
    }

    // 32: No Wiki links in page content
    [Theory]
    [InlineData("/entegrasyonlar")]
    [InlineData("/en/integrations")]
    public async Task BothPages_HaveNoWikiLinksInPageContent(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotContain("/wiki", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wiki.pika.tr", body, StringComparison.OrdinalIgnoreCase);
    }
}
