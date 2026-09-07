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

public class C07CampaignManagerPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C07CampaignManagerPageTests(WebApplicationFactory<Program> factory)
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

    // 1 & 2: HTTP 200 for TR and EN routes
    [Fact]
    public async Task GetTurkishPage_ReturnsSuccess200()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEnglishPage_ReturnsSuccess200()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 3: Exact SeoHelper Title & Meta Description for TR & EN
    [Theory]
    [InlineData("/cozumler/campaign-manager", "tr")]
    [InlineData("/en/solutions/campaign-manager", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Solutions", "CampaignManager");
        Assert.NotNull(meta);

        var expectedTitle = lang == "tr" ? meta.TitleTr : meta.TitleEn;
        var expectedDesc = lang == "tr" ? meta.DescriptionTr : meta.DescriptionEn;

        var actualTitle = ExtractTitle(html);
        var actualDesc = ExtractMetaDescription(html);

        Assert.Equal(expectedTitle, actualTitle);
        Assert.Equal(expectedDesc, actualDesc);
    }

    // 4: Exactly one H1
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_HasExactlyOneH1(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var h1Matches = Regex.Matches(html, @"<h1(?:\s|>)", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);
    }

    // 5: Exact TR H1 normalized
    [Fact]
    public async Task TurkishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Kampanyayı tek yerde hazırlayın. Gönderimi kontrol altında tutun.", h1Text);
    }

    // 6: Exact EN H1 normalized
    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Prepare the campaign in one place. Keep delivery under control.", h1Text);
    }

    // 7: Direct-answer question present in TR: Campaign Manager nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager nedir?", decoded);
    }

    // 8: Direct-answer question present in EN: What is Campaign Manager?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Campaign Manager?", decoded);
    }

    // 9: Canonical product terms present in both languages
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsCanonicalProductTerms(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Campaign Manager", html);
        Assert.Contains("Audience Manager", html);
        Assert.Contains("Content Studio", html);
        Assert.Contains("Journey Manager", html);
    }

    // 10: Günün Fırsatları present in TR
    [Fact]
    public async Task TurkishPage_ContainsGununFirsatlari()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Günün Fırsatları", decoded);
    }

    // 11: Daily Opportunities present in EN
    [Fact]
    public async Task EnglishPage_ContainsDailyOpportunities()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Daily Opportunities", decoded);
    }

    // 12: Confirmed public channels present in both languages: Email, SMS, WhatsApp
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsConfirmedChannels(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Email", html);
        Assert.Contains("SMS", html);
        Assert.Contains("WhatsApp", html);
    }

    // 13: ZERO instances of forbidden push terms in public rendered HTML
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsZeroPushTerms(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Mobile Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Web Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.False(Regex.IsMatch(body, @"\bpush\b", RegexOptions.IgnoreCase), "Rendered body should contain zero instances of word 'push'.");
    }

    // 14: View file contains NO: wikiBase, /wiki/, <img, @section JsonLd, ViewData["Title"], ViewData["MetaDescription"], ViewData["MetaKeywords"], ViewData["CanonicalUrl"]
    [Fact]
    public void CampaignManagerView_ContainsNoForbiddenLegacyElements()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "CampaignManager.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("wikiBase", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/wiki/", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<img", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("@section JsonLd", viewContent);
        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 15: View file and rendered page contain NO screenshot references
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_And_View_ContainNoScreenshotReferences(string path)
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "CampaignManager.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("img_kampanya-yonetimi", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", viewContent, StringComparison.OrdinalIgnoreCase);

        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("img_kampanya-yonetimi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", body, StringComparison.OrdinalIgnoreCase);
    }

    // 16: Rendered page contains NO prohibited claims
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsNoProhibitedClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Sıfır Hata", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Zero Error", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ceza riski ortadan kalkar", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("100% compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("KVKK compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("IYS compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real-time", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real time", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("anlık gönderim", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("instant dispatch", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("live consent", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("canlı izin", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed revenue", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("one-click deploy", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("tek tıkla gönder", body, StringComparison.OrdinalIgnoreCase);
    }

    // 17: Canonical question: Audience Manager = WHO? / KİM?
    [Fact]
    public async Task TurkishPage_ExplainsAudienceManagerAnswersWho()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager “KİM?” sorusunu cevaplar.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsAudienceManagerAnswersWho()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager answers “WHO?”", decoded);
    }

    // 18: Canonical question: Campaign Manager = WHAT CAMPAIGN / CONTROLLED SEND?
    [Fact]
    public async Task TurkishPage_ExplainsCampaignManagerFocusQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("hangi kampanya", decoded, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("kontrollü", decoded, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task EnglishPage_ExplainsCampaignManagerFocusQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("which campaign", decoded, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("controlled", decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 19: Boundary clearly stated: audience membership != permission to send
    [Fact]
    public async Task TurkishPage_ExplainsAudienceDoesNotEqualPermissionToSend()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Hedef kitleye dahil olmak, gönderime uygun olmakla aynı şey değildir.", decoded);
        Assert.Contains("anlamına tek başına gelmez", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsAudienceDoesNotEqualPermissionToSend()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("is not the same as being eligible for delivery", decoded);
        Assert.Contains("does not by itself mean", decoded);
    }

    // 20: Boundary clearly stated: Campaign Manager != Journey Manager
    [Fact]
    public async Task TurkishPage_ExplainsCampaignManagerNotEqualJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager, Journey Manager değildir.", decoded);
        Assert.Contains("müşteri yolculuğu", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsCampaignManagerNotEqualJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager is not Journey Manager.", decoded);
        Assert.Contains("customer-journey", decoded);
    }

    // 21: Boundary clearly stated: Campaign Manager != Content Studio
    [Fact]
    public async Task TurkishPage_ExplainsCampaignManagerNotEqualContentStudio()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager, Content Studio değildir.", decoded);
        Assert.Contains("Content Studio", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsCampaignManagerNotEqualContentStudio()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager is not Content Studio.", decoded);
        Assert.Contains("Content Studio", decoded);
    }

    // 22: Boundary clearly stated: Campaign Manager != Consent Management
    [Fact]
    public async Task TurkishPage_ExplainsCampaignManagerNotEqualConsentManagement()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager, Consent Management değildir.", decoded);
        Assert.Contains("kontrol", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsCampaignManagerNotEqualConsentManagement()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager is not Consent Management.", decoded);
        Assert.Contains("controls", decoded);
    }

    // 23: Scheduling context present: future date planning
    [Fact]
    public async Task TurkishPage_ContainsSchedulingContext()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Zamanlama:", decoded);
        Assert.Contains("ileri bir tarihte gönderim sürecine alınmasını", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsSchedulingContext()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Scheduling asks:", decoded);
        Assert.Contains("schedule a campaign to enter the delivery process at a future time", decoded);
    }

    // 24: Pacing context present: rate limiting / load management
    [Fact]
    public async Task TurkishPage_ContainsPacingContext()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pacing:", decoded);
        Assert.Contains("pacing / hız yönetimi", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsPacingContext()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pacing asks:", decoded);
        Assert.Contains("defined pacing or delivery-rate management", decoded);
    }

    // 25: Attribution boundary present: Campaign Manager reports metrics; does not guarantee sole causality for revenue / sales
    [Fact]
    public async Task TurkishPage_ContainsAttributionBoundary()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Attribution:", decoded);
        Assert.Contains("ilişki kurmaya yardımcı olur.", decoded);
        Assert.Contains("Kampanya tek başına bu satışın kesin nedenidir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsAttributionBoundary()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Attribution:", decoded);
        Assert.Contains("helps establish an association.", decoded);
        Assert.Contains("The campaign alone was the definitive cause of the sale.", decoded);
    }

    // 26: All 8 TR FAQ questions present in rendered page
    [Fact]
    public async Task TurkishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Campaign Manager nedir?",
            "Campaign Manager hangi iletişim kanallarını destekler?",
            "Campaign Manager ile Audience Manager arasındaki fark nedir?",
            "Campaign Manager ile Journey Manager arasındaki fark nedir?",
            "Campaign Manager gönderim öncesi izin kontrolü yapar mı?",
            "Campaign Manager kampanyaları zamanlayabilir mi?",
            "Campaign Manager kampanya gelirini ölçebilir mi?",
            "Campaign Manager kampanyaları yapay zekâ ile otomatik gönderir mi?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var faqMatch = Regex.Match(html, @"<section[^>]*id=""faq""[^>]*>(.*?)</section>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.True(faqMatch.Success);
        var detailsCount = Regex.Matches(faqMatch.Groups[1].Value, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(8, detailsCount);
    }

    // 27: All 8 EN FAQ questions present in rendered page
    [Fact]
    public async Task EnglishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Campaign Manager?",
            "Which communication channels does Campaign Manager support?",
            "What is the difference between Campaign Manager and Audience Manager?",
            "What is the difference between Campaign Manager and Journey Manager?",
            "Does Campaign Manager include pre-send consent controls?",
            "Can Campaign Manager schedule campaigns?",
            "Can Campaign Manager measure campaign revenue?",
            "Does Campaign Manager automatically send campaigns with AI?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var faqMatch = Regex.Match(html, @"<section[^>]*id=""faq""[^>]*>(.*?)</section>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.True(faqMatch.Success);
        var detailsCount = Regex.Matches(faqMatch.Groups[1].Value, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(8, detailsCount);
    }

    // 28: Rendered page contains NO page-level SoftwareApplication, FAQPage, or Offer JSON-LD
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsNoForbiddenStructuredData(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("SoftwareApplication", html);
        Assert.DoesNotContain("FAQPage", html);
        Assert.DoesNotContain("\"@type\": \"Offer\"", html);
        Assert.DoesNotContain("\"@type\":\"Offer\"", html);
    }

    // 29: All 11 required contextual internal links present in TR
    [Fact]
    public async Task TurkishPage_ContainsAllElevenRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/content-studio\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
    }

    // 30: All 11 required contextual internal links present in EN
    [Fact]
    public async Task EnglishPage_ContainsAllElevenRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/content-studio\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
    }

    // 31: Exact commercial/pricing copy present in TR & EN
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/campaign-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 32: Rendered page contains NO public pricing numbers: "₺", "$", "€", "free trial", "ücretsiz deneme"
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsNoFixedPublicPricingPatterns(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("₺", body);
        Assert.DoesNotContain("$", body);
        Assert.DoesNotContain("€", body);
        Assert.DoesNotContain("free trial", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ücretsiz deneme", body, StringComparison.OrdinalIgnoreCase);
    }

    // 33: Rendered page contains NO fabricated/fake campaign metrics or Wiki links
    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedPage_ContainsNoFabricatedMetricsOrWikiLinks(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("%99.8", body);
        Assert.DoesNotContain("99.8%", body);
        Assert.DoesNotContain("4.2x", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("10x", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", body, StringComparison.OrdinalIgnoreCase);
    }
}
