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

public class C08JourneyManagerPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C08JourneyManagerPageTests(WebApplicationFactory<Program> factory)
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
        var response = await client.GetAsync("/cozumler/journey-manager");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEnglishPage_ReturnsSuccess200()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 3: Exact SeoHelper Title & Meta Description for TR & EN
    [Theory]
    [InlineData("/cozumler/journey-manager", "tr")]
    [InlineData("/en/solutions/journey-manager", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Solutions", "JourneyManager");
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
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
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
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Tek bir mesajdan, kontrollü bir müşteri yolculuğuna geçin.", h1Text);
    }

    // 6: Exact EN H1 normalized
    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Move from one message to a controlled customer journey.", h1Text);
    }

    // 7: Direct-answer question present in TR: Journey Manager nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Journey Manager nedir?", decoded);
    }

    // 8: Direct-answer question present in EN: What is Journey Manager?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Journey Manager?", decoded);
    }

    // 9: Canonical product terms present in both languages
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
    public async Task RenderedPage_ContainsCanonicalProductTerms(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Journey Manager", html);
        Assert.Contains("Audience Manager", html);
        Assert.Contains("Campaign Manager", html);
        Assert.Contains("Content Studio", html);
    }

    // 10: Günün Fırsatları present in TR
    [Fact]
    public async Task TurkishPage_ContainsGununFirsatlari()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Günün Fırsatları", decoded);
    }

    // 11: Daily Opportunities present in EN
    [Fact]
    public async Task EnglishPage_ContainsDailyOpportunities()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Daily Opportunities", decoded);
    }

    // 12: Confirmed public channels present in both languages: Email, SMS, WhatsApp
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
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
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
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

    // 14: Canonical building blocks present in both languages
    [Fact]
    public async Task TurkishPage_ContainsCanonicalBuildingBlocks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Başlangıç", decoded);
        Assert.Contains("Koşul", decoded);
        Assert.Contains("Dallanma", decoded);
        Assert.Contains("Bekleme", decoded);
        Assert.Contains("Kanal Aksiyonu", decoded);
        Assert.Contains("Bitiş", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsCanonicalBuildingBlocks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Start", decoded);
        Assert.Contains("Condition", decoded);
        Assert.Contains("Branch", decoded);
        Assert.Contains("Wait", decoded);
        Assert.Contains("Channel Action", decoded);
        Assert.Contains("End", decoded);
    }

    // 15: View file contains NO legacy tags: ViewData["Title"], ViewData["MetaDescription"], ViewData["MetaKeywords"], ViewData["CanonicalUrl"], @section JsonLd, /wiki/, <img
    [Fact]
    public void JourneyManagerView_ContainsNoForbiddenLegacyElements()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "JourneyManager.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("wikiBase", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/wiki/", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("pika-story-image", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("@section JsonLd", viewContent);
        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 16: No product screenshots in view or rendered page
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
    public async Task RenderedPage_And_View_ContainNoScreenshotReferences(string path)
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "JourneyManager.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("img_journey-tasarim-tuvali", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", viewContent, StringComparison.OrdinalIgnoreCase);

        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("img_journey-tasarim-tuvali", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", body, StringComparison.OrdinalIgnoreCase);
    }

    // 17: No fake performance bars or pseudo-dashboard copy
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
    public async Task RenderedPage_ContainsNoFakePerformanceDashboardCopy(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Flow Performance", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Akış Performansı", body, StringComparison.OrdinalIgnoreCase);
    }

    // 18: Rendered body contains no prohibited claims
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
    public async Task RenderedPage_ContainsNoProhibitedClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("real-time", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real time", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("anlık", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("instant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("millisecond", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("self-optimizing", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed engagement", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed revenue", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("KVKK compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("IYS compliant", body, StringComparison.OrdinalIgnoreCase);
    }

    // 19: Rendered page clearly explains: Audience Manager = WHO?
    [Fact]
    public async Task TurkishPage_ExplainsAudienceManagerAnswersWho()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Bu bağlama uyan müşteriler kimler?", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsAudienceManagerAnswersWho()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Which customers match this context?", decoded);
    }

    // 20: Rendered page clearly distinguishes: Campaign Manager vs Journey Manager
    [Fact]
    public async Task TurkishPage_DistinguishesCampaignManagerVsJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager ve Journey Manager, aynı gönderim problemini çözmez.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesCampaignManagerVsJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Campaign Manager and Journey Manager do not solve the same delivery problem.", decoded);
    }

    // 21: Rendered page says journey structure is user-defined
    [Fact]
    public async Task TurkishPage_StatesJourneyStructureIsUserDefined()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Journey yapısı kullanıcı tarafından tanımlanır ve etkinleştirilir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesJourneyStructureIsUserDefined()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("The journey structure is defined and activated by the user.", decoded);
    }

    // 22: Rendered page says configured active journeys may progress according to rules
    [Fact]
    public async Task TurkishPage_StatesConfiguredActiveJourneysProgressByRules()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Etkin journey içindeki ilerleme, yapılandırılmış koşul, bekleme ve aksiyon kurallarına göre yürütülebilir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesConfiguredActiveJourneysProgressByRules()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Progress inside an active journey can then follow configured condition, wait and action rules.", decoded);
    }

    // 23: Rendered page says opportunity does not automatically launch a journey
    [Fact]
    public async Task TurkishPage_StatesOpportunityDoesNotAutomaticallyLaunchJourney()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Bu ilişki, fırsatın otomatik olarak journey başlattığı anlamına gelmez.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesOpportunityDoesNotAutomaticallyLaunchJourney()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("This relationship does not mean that an opportunity automatically launches a journey.", decoded);
    }

    // 24: Rendered page says journey channel action does not bypass consent
    [Fact]
    public async Task TurkishPage_StatesJourneyActionDoesNotBypassConsent()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Bir müşterinin journey içinde bir kanal aksiyonuna ulaşması, seçilen kanalda iletişimin hiçbir kontrol olmadan gönderileceği anlamına gelmez.", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesJourneyActionDoesNotBypassConsent()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("A customer reaching a channel-action step in a journey does not mean communication is sent through that channel without controls.", decoded);
    }

    // 25: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Journey Manager nedir?",
            "Journey Manager ile Campaign Manager arasındaki fark nedir?",
            "Journey Manager hangi iletişim kanallarını kullanabilir?",
            "Journey Manager'da koşul ve dallanma ne işe yarar?",
            "Journey Manager'da bekleme adımı ne işe yarar?",
            "Journey Manager tamamen otomatik mi çalışır?",
            "Journey Manager iletişim izinlerini ortadan kaldırır mı?",
            "Journey Manager journey'leri yapay zekâ ile kendi kendine oluşturur mu?"
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

    // 26: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Journey Manager?",
            "What is the difference between Journey Manager and Campaign Manager?",
            "Which communication channels can Journey Manager use?",
            "What do conditions and branching do in Journey Manager?",
            "What does a wait step do in Journey Manager?",
            "Is Journey Manager fully automatic?",
            "Does Journey Manager bypass communication consent?",
            "Does Journey Manager create journeys autonomously with AI?"
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

    // 27: Rendered page contains NO page-level SoftwareApplication, FAQPage, or Offer JSON-LD
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
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

    // 28: All required internal links present in TR and EN
    [Fact]
    public async Task TurkishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/content-studio\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/content-studio\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
    }

    // 29: Exact pricing copy in TR and EN
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/journey-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 30: Rendered page contains NO public pricing numbers: "₺", "$", "€", "free trial", "ücretsiz deneme"
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
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

    // 31: Rendered page contains NO Wiki links
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
    public async Task RenderedPage_ContainsNoWikiLinks(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", body, StringComparison.OrdinalIgnoreCase);
    }

    // 32: Rendered page contains NO fake journey metrics
    [Theory]
    [InlineData("/cozumler/journey-manager")]
    [InlineData("/en/solutions/journey-manager")]
    public async Task RenderedPage_ContainsNoFakeJourneyMetrics(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("%99", body);
        Assert.DoesNotContain("99%", body);
        Assert.DoesNotContain("4.2x", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("10x", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("84/100", body);
        Assert.DoesNotContain("Deniz Kaya", body, StringComparison.OrdinalIgnoreCase);
    }
}
