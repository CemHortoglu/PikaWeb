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

public class C10AiCampaignAssistantPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C10AiCampaignAssistantPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /urunler/ai-kampanya-asistani returns 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/products/ai-campaign-assistant returns 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.AiCampaignAssistant SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsAiCampaignAssistant_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "AiCampaignAssistant");
        Assert.NotNull(entry);

        Assert.Equal("Pika Pilot | AI Kampanya Asistanı ve İçerik Taslağı | Pika", entry.TitleTr);
        Assert.Equal("Pika Pilot | AI Campaign Assistant & Content Drafting | Pika", entry.TitleEn);
        Assert.Equal("Pika Pilot; Pika'nın hesapladığı müşteri, ürün ve fırsat bağlamını kullanarak hedef kitle kriterleri, kanal kurgusu ve kampanya içeriği taslaklarını hazırlamaya yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Pilot uses customer, product and opportunity context calculated by Pika to help draft audience criteria, channel plans and campaign content.", entry.DescriptionEn);
        Assert.Equal("Pika Pilot", entry.BreadcrumbTitleTr);
        Assert.Equal("Pika Pilot", entry.BreadcrumbTitleEn);
    }

    [Fact]
    public async Task TurkishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Pika Pilot | AI Kampanya Asistanı ve İçerik Taslağı | Pika", ExtractTitle(html));
        Assert.Equal("Pika Pilot; Pika'nın hesapladığı müşteri, ürün ve fırsat bağlamını kullanarak hedef kitle kriterleri, kanal kurgusu ve kampanya içeriği taslaklarını hazırlamaya yardımcı olur.", ExtractMetaDescription(html));
    }

    [Fact]
    public async Task EnglishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Pika Pilot | AI Campaign Assistant & Content Drafting | Pika", ExtractTitle(html));
        Assert.Equal("Pika Pilot uses customer, product and opportunity context calculated by Pika to help draft audience criteria, channel plans and campaign content.", ExtractMetaDescription(html));
    }

    // 04: Both pages have exactly one H1
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_HaveExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var matches = Regex.Matches(html, @"<h1[^>]*>.*?</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Single(matches);
    }

    // 05: TR normalized H1: Kampanya fikrini, hesaplanmış müşteri bağlamıyla taslağa dönüştürün.
    [Fact]
    public async Task TurkishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Kampanya fikrini, hesaplanmış müşteri bağlamıyla taslağa dönüştürün.", ExtractH1(html));
    }

    // 06: EN normalized H1: Turn a campaign idea into a draft grounded in calculated customer context.
    [Fact]
    public async Task EnglishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Turn a campaign idea into a draft grounded in calculated customer context.", ExtractH1(html));
    }

    // 07: TR contains direct answer heading: Pika Pilot nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Pika Pilot nedir?", html);
    }

    // 08: EN contains direct answer heading: What is Pika Pilot?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("What is Pika Pilot?", html);
    }

    // 09: Both contain canonical ecosystem entities
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainCanonicalEcosystemEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot", decoded);
        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Product Intelligence", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Content Studio", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
    }

    // 10: TR contains: Günün Fırsatları
    [Fact]
    public async Task TurkishPage_ContainsGununFirsatlari()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Günün Fırsatları", html);
    }

    // 11: EN contains: Daily Opportunities
    [Fact]
    public async Task EnglishPage_ContainsDailyOpportunities()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Daily Opportunities", html);
    }

    // 12: Both contain active channels: Email, SMS, WhatsApp
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainActiveChannels(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Email", html);
        Assert.Contains("SMS", html);
        Assert.Contains("WhatsApp", html);
    }

    // 13: Rendered public body contains ZERO: Push / Mobile Push / Web Push
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
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

    // 14: Both contain exact canonical AI statement
    [Fact]
    public async Task TurkishPage_ContainsExactCanonicalAiStatement()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika AI, müşteriyi veya fırsatı hayal ederek oluşturmaz. Pika'nın hesapladığı müşteri, ürün ve fırsat bağlamını kullanarak kampanya hazırlığını ve içerik üretimini hızlandırır; son karar kullanıcıda kalır.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactCanonicalAiStatement()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika AI does not invent the customer or the opportunity. It uses customer, product and opportunity context calculated by Pika to accelerate campaign preparation and content creation, while the final decision remains with the user.", decoded);
    }

    // 15: Both visibly establish: final decision remains with user
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani", "son karar kullanıcıda kalır")]
    [InlineData("/en/products/ai-campaign-assistant", "final decision remains with the user")]
    public async Task BothPages_EstablishFinalDecisionRemainsWithUser(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains(expectedPhrase, decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 16: Page clearly establishes: Pika Pilot = assistive co-pilot
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani", "yardımcı AI co-pilot")]
    [InlineData("/en/products/ai-campaign-assistant", "assistive AI co-pilot")]
    public async Task BothPages_EstablishPikaPilotIsAssistiveCoPilot(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains(expectedPhrase, decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 17: Page clearly establishes: Pika Pilot does not independently send campaigns
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani", "kampanyayı kendi başına gönderen bir sistem değildir")]
    [InlineData("/en/products/ai-campaign-assistant", "does not independently send the campaign")]
    public async Task BothPages_EstablishPikaPilotDoesNotIndependentlySendCampaigns(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains(expectedPhrase, decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 18: Page clearly distinguishes: Pika Pilot vs Audience Manager
    [Fact]
    public async Task TurkishPage_DistinguishesPikaPilotVsAudienceManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot, Audience Manager değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesPikaPilotVsAudienceManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot is not Audience Manager.", decoded);
    }

    // 19: Page clearly distinguishes: Pika Pilot vs Content Studio
    [Fact]
    public async Task TurkishPage_DistinguishesPikaPilotVsContentStudio()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot, Content Studio değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesPikaPilotVsContentStudio()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot is not Content Studio.", decoded);
    }

    // 20: Page clearly distinguishes: Pika Pilot vs Campaign Manager / Journey Manager
    [Fact]
    public async Task TurkishPage_DistinguishesPikaPilotVsCampaignAndJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot, Campaign Manager veya Journey Manager değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesPikaPilotVsCampaignAndJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika Pilot is not Campaign Manager or Journey Manager.", decoded);
    }

    // 21: Page clearly states: AI draft != actual audience definition
    [Fact]
    public async Task TurkishPage_StatesAiDraftIsNotActualAudienceDefinition()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("AI önerisi, audience gerçeğinin yerine geçmez.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesAiDraftIsNotActualAudienceDefinition()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("An AI suggestion does not replace audience truth.", decoded);
    }

    // 22: Page clearly states: AI draft != published/sent content
    [Fact]
    public async Task TurkishPage_StatesAiDraftIsNotPublishedContent()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Üretilen taslak, yayınlanmış içerik değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesAiDraftIsNotPublishedContent()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Generated draft content is not published content.", decoded);
    }

    // 23: View contains no legacy tags, wikiBase, /wiki/, <img
    [Fact]
    public void ViewFile_ContainsNoLegacyTagsOrWikiLinksOrImg()
    {
        var solutionDir = GetProjectRoot();
        var viewPath = Path.Combine(solutionDir, "Views", "Solutions", "AiCampaignAssistant.cshtml");
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

    // 24: No real product screenshot reference
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoRealScreenshots(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("img_pika-pilot-ai-kampanya-asistani_16.png", html);
        Assert.DoesNotContain("/wiki/assets/images/", html);
        Assert.DoesNotContain("<img", html);
    }

    // 25: No speed claims
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoProhibitedSpeedClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("10 kat", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("10x", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("saniyeler içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("in seconds", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("dakikalar içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("in minutes", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45 saniye", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45 seconds", body, StringComparison.OrdinalIgnoreCase);
    }

    // 26: No absolute error/compliance claims
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoAbsoluteErrorOrComplianceClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Sıfır Otonom Hata Riski", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Zero Autonomous Error Risk", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("%100 İYS", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("100% IYS", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("IYS compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("KVKK compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("100% compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("yasal uyum garantisi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("legal compliance guarantee", body, StringComparison.OrdinalIgnoreCase);
    }

    // 27: No positive autonomous/automatic decision claims
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoPositiveAutonomousClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Auto Cohort & Channel Mapping", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Autonomous Campaign Engine", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Otonom Kampanya Motoru", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("otonom gönderim yapar", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("autonomously sends", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("automatic audience selection", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("automatic channel selection", body, StringComparison.OrdinalIgnoreCase);
    }

    // 28: No autonomous opportunity detection claims
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoAutonomousOpportunityDetectionClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("fırsatları kendisi uydurur", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("invents opportunities", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("fırsatları kendi başına kararlaştırır", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("autonomously decides opportunities", body, StringComparison.OrdinalIgnoreCase);
    }

    // 29: No fake metrics
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoFakeMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("10x daha hızlı kampanya", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("%100 hatasız içerik", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("100% error-free content", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("garantili açılma", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed open rate", body, StringComparison.OrdinalIgnoreCase);
    }

    // 30: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_RendersExactlyEightCanonicalFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Pika Pilot nedir?",
            "Pika Pilot müşteri veya fırsatı yapay zekâ ile mi oluşturur?",
            "Pika Pilot hedef kitleyi otomatik olarak seçer mi?",
            "Pika Pilot hangi içerikleri hazırlamaya yardımcı olabilir?",
            "Pika Pilot ile Content Studio arasındaki fark nedir?",
            "Pika Pilot kampanyayı kendi başına gönderir mi?",
            "Pika Pilot yapay zekâ önerilerinin doğru olduğunu garanti eder mi?",
            "Pika Pilot hangi Pika ürünleriyle birlikte çalışır?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var summaryMatches = Regex.Matches(html, @"<summary[^>]*>(.*?)</summary>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 31: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_RendersExactlyEightCanonicalFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Pika Pilot?",
            "Does Pika Pilot create the customer or opportunity with AI?",
            "Does Pika Pilot automatically select the audience?",
            "What content can Pika Pilot help prepare?",
            "What is the difference between Pika Pilot and Content Studio?",
            "Does Pika Pilot send campaigns by itself?",
            "Does Pika Pilot guarantee that AI suggestions are correct?",
            "Which Pika products does Pika Pilot work with?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var summaryMatches = Regex.Matches(html, @"<summary[^>]*>(.*?)</summary>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 32: Both pages use native <details>/<summary> for FAQ
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_UseNativeDetailsSummaryForFaq(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var detailsMatches = Regex.Matches(html, @"<details[^>]*class=""pilot-faq-item""", RegexOptions.IgnoreCase);
        var summaryMatches = Regex.Matches(html, @"<summary[^>]*class=""pilot-faq-summary""", RegexOptions.IgnoreCase);

        Assert.Equal(8, detailsMatches.Count);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 33: Both pages contain NO page-level prohibited JSON-LD
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task BothPages_ContainNoPageLevelProhibitedJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("\"FAQPage\"", html);
        Assert.DoesNotContain("\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"Offer\"", html);
    }

    // 34: TR page contains all contextual internal links
    [Fact]
    public async Task TurkishPage_ContainsAllContextualInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/cozumler/content-studio\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
    }

    // 35: EN page contains all contextual internal links
    [Fact]
    public async Task EnglishPage_ContainsAllContextualInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/solutions/content-studio\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
    }

    // 36: Both pages contain exact pricing copy
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/urunler/ai-kampanya-asistani");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/products/ai-campaign-assistant");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 37: Both pages contain NO fixed public price patterns
    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
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
}
