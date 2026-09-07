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

public class C11ReportingPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C11ReportingPageTests(WebApplicationFactory<Program> factory)
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

    private static string ExtractTitle(string html)
    {
        var match = Regex.Match(html, @"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        return match.Success ? WebUtility.HtmlDecode(match.Groups[1].Value.Trim()) : string.Empty;
    }

    private static string ExtractMetaDescription(string html)
    {
        var match = Regex.Match(html, @"<meta\s+name=""description""\s+content=""([^""]*)""", RegexOptions.IgnoreCase);
        return match.Success ? WebUtility.HtmlDecode(match.Groups[1].Value.Trim()) : string.Empty;
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

    // 01: GET /cozumler/analytics-reporting = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/solutions/analytics-reporting = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.Reporting SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsReporting_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "Reporting");
        Assert.NotNull(entry);

        Assert.Equal("Analytics & Reporting | Kampanya, Kanal ve Ciro Atfı | Pika", entry.TitleTr);
        Assert.Equal("Analytics & Reporting | Campaign, Channel & Revenue Attribution | Pika", entry.TitleEn);
        Assert.Equal("Pika Analytics & Reporting; kampanya, kanal, kitle ve teslimat sonuçlarını görünür hale getirir, etkileşim verilerini ve ilişkilendirilebilen satış veya ciro sonuçlarını attribution bağlamında değerlendirmenize yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Analytics & Reporting makes campaign, channel, audience and delivery results visible and helps evaluate engagement plus attributable sales or revenue outcomes.", entry.DescriptionEn);
        Assert.Equal("Analytics & Reporting", entry.BreadcrumbTitleTr);
        Assert.Equal("Analytics & Reporting", entry.BreadcrumbTitleEn);
    }

    [Fact]
    public async Task TurkishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Analytics & Reporting | Kampanya, Kanal ve Ciro Atfı | Pika", ExtractTitle(html));
        Assert.Equal("Pika Analytics & Reporting; kampanya, kanal, kitle ve teslimat sonuçlarını görünür hale getirir, etkileşim verilerini ve ilişkilendirilebilen satış veya ciro sonuçlarını attribution bağlamında değerlendirmenize yardımcı olur.", ExtractMetaDescription(html));
    }

    [Fact]
    public async Task EnglishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Analytics & Reporting | Campaign, Channel & Revenue Attribution | Pika", ExtractTitle(html));
        Assert.Equal("Pika Analytics & Reporting makes campaign, channel, audience and delivery results visible and helps evaluate engagement plus attributable sales or revenue outcomes.", ExtractMetaDescription(html));
    }

    // 04: Exactly one H1
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_HaveExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var matches = Regex.Matches(html, @"<h1[^>]*>.*?</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Single(matches);
    }

    // 05: TR normalized H1: Gönderimin ötesini görün. Kampanya sonucunu bağlamıyla ölçün.
    [Fact]
    public async Task TurkishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Gönderimin ötesini görün. Kampanya sonucunu bağlamıyla ölçün.", ExtractH1(html));
    }

    // 06: EN normalized H1: See beyond delivery. Measure campaign outcomes in context.
    [Fact]
    public async Task EnglishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("See beyond delivery. Measure campaign outcomes in context.", ExtractH1(html));
    }

    // 07: TR contains: Analytics & Reporting nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting nedir?", decoded);
    }

    // 08: EN contains: What is Analytics & Reporting?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Analytics & Reporting?", decoded);
    }

    // 09: Both contain: Campaign Manager, Journey Manager, Audience Manager
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainCanonicalEcosystemEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Audience Manager", decoded);
    }

    // 10: Both contain: Email, SMS, WhatsApp
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainConfirmedChannels(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Email", html);
        Assert.Contains("SMS", html);
        Assert.Contains("WhatsApp", html);
    }

    // 11: Rendered public body contains ZERO: Push / Mobile Push / Web Push
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_RenderedBodyContainsZeroPushMentions(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Mobile Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Web Push", body, StringComparison.OrdinalIgnoreCase);
    }

    // 12: View contains no legacy tags, wikiBase, /wiki/, <img
    [Fact]
    public void ViewFile_ContainsNoLegacyTagsOrWikiLinksOrImg()
    {
        var solutionDir = GetProjectRoot();
        var viewPath = Path.Combine(solutionDir, "Views", "Solutions", "Reporting.cshtml");
        Assert.True(File.Exists(viewPath), $"View file should exist at {viewPath}");

        var content = File.ReadAllText(viewPath);
        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("wikiBase", content);
        Assert.DoesNotContain("<img", content);
    }

    // 13: No fake dashboard metric bars: pika-sol-showcase-mock-bar, w80, w60, w70, w45
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoFakeDashboardMetricBars(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("pika-sol-showcase-mock-bar", html);
        Assert.DoesNotContain("w80", html);
        Assert.DoesNotContain("w60", html);
        Assert.DoesNotContain("w70", html);
        Assert.DoesNotContain("w45", html);
    }

    // 14: No positive real-time claims
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoPositiveRealTimeClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Anlık veri", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Real-time data", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Live analytics", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Instant analytics", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Instant reporting", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("live dashboard", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("always up to date", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("immediate updates", body, StringComparison.OrdinalIgnoreCase);

        // millisecond reporting only permitted within negative disclaimer context
        if (body.Contains("millisecond reporting", StringComparison.OrdinalIgnoreCase))
        {
            Assert.True(
                body.Contains("taahhüdünde bulunmaz", StringComparison.OrdinalIgnoreCase) ||
                body.Contains("makes no commitment", StringComparison.OrdinalIgnoreCase),
                "millisecond reporting must only appear in negative boundary copy.");
        }
    }

    // 15: Page clearly states: Attribution != causality
    [Fact]
    public async Task TurkishPage_StatesAttributionIsNotCausality()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("attribution ile causality aynı şey değildir", decoded, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task EnglishPage_StatesAttributionIsNotCausality()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("attribution and causality are not the same thing", decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 16: Page states: association does not prove sole cause
    [Fact]
    public async Task TurkishPage_StatesAssociationDoesNotProveSoleCause()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("kampanyanın satışın tek ve kesin nedeni olduğunu kanıtlamaz", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesAssociationDoesNotProveSoleCause()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("does not prove that the campaign was the sole and definitive cause of that sale", decoded);
    }

    // 17: Page clearly distinguishes: Analytics & Reporting vs Campaign Manager
    [Fact]
    public async Task TurkishPage_DistinguishesAnalyticsReportingVsCampaignManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting, Campaign Manager değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesAnalyticsReportingVsCampaignManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting is not Campaign Manager.", decoded);
    }

    // 18: Page clearly distinguishes: Analytics & Reporting vs Journey Manager
    [Fact]
    public async Task TurkishPage_DistinguishesAnalyticsReportingVsJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting, Journey Manager değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesAnalyticsReportingVsJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting is not Journey Manager.", decoded);
    }

    // 19: Page clearly distinguishes: Analytics & Reporting vs Audience Manager
    [Fact]
    public async Task TurkishPage_DistinguishesAnalyticsReportingVsAudienceManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting, Audience Manager değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesAnalyticsReportingVsAudienceManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting is not Audience Manager.", decoded);
    }

    // 20: Page clearly states: Analytics & Reporting is not an opportunity engine
    [Fact]
    public async Task TurkishPage_StatesAnalyticsReportingIsNotAnOpportunityEngine()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting bir fırsat motoru değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesAnalyticsReportingIsNotAnOpportunityEngine()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Analytics & Reporting is not an opportunity engine.", decoded);
    }

    // 21: Page contains: delivery telemetry / delivery telemetrisi
    [Fact]
    public async Task TurkishPage_ContainsDeliveryTelemetrisi()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("delivery telemetrisi", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task EnglishPage_ContainsDeliveryTelemetry()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("delivery telemetry", html, StringComparison.OrdinalIgnoreCase);
    }

    // 22: Page contains: engagement context / etkileşim
    [Fact]
    public async Task TurkishPage_ContainsEtkilesim()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("etkileşim", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task EnglishPage_ContainsEngagement()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("engagement", html, StringComparison.OrdinalIgnoreCase);
    }

    // 23: TR contains: açılma, tıklama
    [Fact]
    public async Task TurkishPage_ContainsAcilmaTiklama()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("açılma", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("tıklama", html, StringComparison.OrdinalIgnoreCase);
    }

    // 24: EN contains: opens, clicks
    [Fact]
    public async Task EnglishPage_ContainsOpensClicks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("opens", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("clicks", html, StringComparison.OrdinalIgnoreCase);
    }

    // 25: Page explains metrics differ by channel
    [Fact]
    public async Task TurkishPage_ExplainsMetricsDifferByChannel()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Kanalların iletişim ve telemetri yapıları farklıdır.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsMetricsDifferByChannel()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Communication and telemetry structures differ by channel.", decoded);
    }

    // 26: No positive best segment, best channel claims
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoPositiveBestSegmentOrChannelClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Hangi segmentin en iyi performans gösterdiğini belirleyin", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Determine which segment performs best", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Hangi kanalın en etkili olduğunu belirleyin", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Determine which channel is most effective", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("winning segment", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("winning channel", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("perfect attribution", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("100% attribution", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed ROI", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed ROAS", body, StringComparison.OrdinalIgnoreCase);
    }

    // 27: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_RendersExactlyEightCanonicalFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Analytics & Reporting nedir?",
            "Analytics & Reporting hangi kampanya sonuçlarını ölçebilir?",
            "Email, SMS ve WhatsApp için aynı metrikler mi kullanılır?",
            "Analytics & Reporting segment performansını gösterebilir mi?",
            "Analytics & Reporting kampanya cirosunu ölçebilir mi?",
            "Analytics & Reporting gerçek zamanlı mı çalışır?",
            "Analytics & Reporting ile Campaign Manager arasındaki fark nedir?",
            "Analytics & Reporting sonraki kampanyayı otomatik olarak optimize eder mi?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var summaryMatches = Regex.Matches(html, @"<summary[^>]*class=""report-faq-summary""", RegexOptions.IgnoreCase);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 28: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_RendersExactlyEightCanonicalFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Analytics & Reporting?",
            "Which campaign outcomes can Analytics & Reporting measure?",
            "Are the same metrics used for Email, SMS and WhatsApp?",
            "Can Analytics & Reporting show segment performance?",
            "Can Analytics & Reporting measure campaign revenue?",
            "Does Analytics & Reporting operate in real time?",
            "What is the difference between Analytics & Reporting and Campaign Manager?",
            "Does Analytics & Reporting automatically optimize the next campaign?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var summaryMatches = Regex.Matches(html, @"<summary[^>]*class=""report-faq-summary""", RegexOptions.IgnoreCase);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 29: No page-level prohibited JSON-LD
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoPageLevelProhibitedJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("\"FAQPage\"", html);
        Assert.DoesNotContain("\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"Offer\"", html);
    }

    // 30: Correct contextual internal links
    [Fact]
    public async Task TurkishPage_ContainsAllContextualInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllContextualInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
    }

    // 31: Exact pricing copy
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/analytics-reporting");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 32: No fixed price patterns
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoFixedPriceOrFreeTrialClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("₺", body);
        Assert.DoesNotContain("$", body);
        Assert.DoesNotContain("€", body);
        Assert.DoesNotContain("free trial", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ücretsiz deneme", body, StringComparison.OrdinalIgnoreCase);
    }

    // 33: No fabricated metrics / percentages
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoFabricatedMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("84.250", body);
        Assert.DoesNotContain("84/100", body);
        Assert.DoesNotContain("%100", body);
    }

    // 34: No Wiki links
    [Theory]
    [InlineData("/cozumler/analytics-reporting")]
    [InlineData("/en/solutions/analytics-reporting")]
    public async Task BothPages_ContainNoWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", body, StringComparison.OrdinalIgnoreCase);
    }
}
