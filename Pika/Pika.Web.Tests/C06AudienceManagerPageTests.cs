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

public class C06AudienceManagerPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C06AudienceManagerPageTests(WebApplicationFactory<Program> factory)
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
        var response = await client.GetAsync("/cozumler/audience-manager");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEnglishPage_ReturnsSuccess200()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 3: Exact SeoHelper Title & Description for TR & EN
    [Theory]
    [InlineData("/cozumler/audience-manager", "tr")]
    [InlineData("/en/solutions/audience-manager", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Solutions", "AudienceManager");
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
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
    public async Task RenderedPage_HasExactlyOneH1(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var h1Matches = Regex.Matches(html, @"<h1(?:\s|>)", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);
    }

    // 5: Exact TR H1
    [Fact]
    public async Task TurkishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Doğru kitleyi, müşteri bağlamından oluşturun.", h1Text);
    }

    // 6: Exact EN H1
    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Build the right audience from customer context.", h1Text);
    }

    // 7: TR Direct answer question
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager nedir?", decoded);
    }

    // 8: EN Direct answer question
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Audience Manager?", decoded);
    }

    // 9: Canonical product terms in both
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
    public async Task RenderedPage_ContainsCanonicalProductTerms(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Audience Manager", html);
        Assert.Contains("Customer Intelligence", html);
        Assert.Contains("Product Intelligence", html);
        Assert.Contains("Campaign Manager", html);
        Assert.Contains("Journey Manager", html);
    }

    // 10: TR contains Günün Fırsatları
    [Fact]
    public async Task TurkishPage_ContainsGununFirsatlari()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Günün Fırsatları", decoded);
    }

    // 11: EN contains Daily Opportunities
    [Fact]
    public async Task EnglishPage_ContainsDailyOpportunities()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Daily Opportunities", decoded);
    }

    // 12: Rule logic explicit AND / OR in both, VE / VEYA in TR
    [Fact]
    public async Task TurkishPage_ContainsExplicitRuleLogic()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("VE", decoded);
        Assert.Contains("VEYA", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExplicitRuleLogic()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("AND", decoded);
        Assert.Contains("OR", decoded);
    }

    // 13: View contains no forbidden elements
    [Fact]
    public void AudienceManagerView_ContainsNoForbiddenLegacyElements()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "AudienceManager.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("wikiBase", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/wiki/", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("@section JsonLd", viewContent);
        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 14: Editorial photography is allowed; real product screenshots remain excluded.
    [Fact]
    public void AudienceManagerView_UsesEditorialImagesWithoutScreenshots()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "AudienceManager.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.Contains("pika-story-image", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", viewContent, StringComparison.OrdinalIgnoreCase);
    }

    // 15: Rendered page contains no prohibited claims
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
    public async Task RenderedPage_ContainsNoProhibitedClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("KVKK uyumlu", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("KVKK compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real-time preview", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real time preview", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("anlık önizleme", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("instant preview", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("millisecond", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed engagement", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("automatic campaign launch", body, StringComparison.OrdinalIgnoreCase);
    }

    // 16: The word dynamic is allowed and present
    [Fact]
    public async Task DynamicTerm_IsAllowedAndPresent()
    {
        var client = CreateNoRedirectClient();

        var trResponse = await client.GetAsync("/cozumler/audience-manager");
        var trHtml = await trResponse.Content.ReadAsStringAsync();
        Assert.Contains("dinamik", trHtml, StringComparison.OrdinalIgnoreCase);

        var enResponse = await client.GetAsync("/en/solutions/audience-manager");
        var enHtml = await enResponse.Content.ReadAsStringAsync();
        Assert.Contains("dynamic", enHtml, StringComparison.OrdinalIgnoreCase);
    }

    // 17: Canonical question distinction WHO? / KİM?
    [Fact]
    public async Task TurkishPage_ContainsCanonicalWhoDistinction()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("KİM?", decoded);
        Assert.Contains("Audience Manager", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsCanonicalWhoDistinction()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("WHO?", decoded);
        Assert.Contains("Audience Manager", decoded);
    }

    // 18: Audience membership != permission to send
    [Fact]
    public async Task TurkishPage_ExplainsAudienceDoesNotEqualPermissionToSend()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("KİTLE ≠ GÖNDERİM İZNİ", decoded);
        Assert.Contains("anlamına gelmez", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsAudienceDoesNotEqualPermissionToSend()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("AUDIENCE ≠ PERMISSION TO SEND", decoded);
        Assert.Contains("does not mean", decoded);
    }

    // 19: Audience Manager does not send campaigns
    [Fact]
    public async Task TurkishPage_ExplainsAudienceManagerDoesNotSendCampaigns()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager aksiyonu göndermez.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsAudienceManagerDoesNotSendCampaigns()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Audience Manager does not send the action.", decoded);
    }

    // 20: Exactly 8 Turkish FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Audience Manager nedir?",
            "Audience Manager hangi verileri segmentasyonda kullanabilir?",
            "Dinamik hedef kitle nedir?",
            "Audience Manager ile Customer Intelligence arasındaki fark nedir?",
            "Audience Manager ile Günün Fırsatları arasındaki fark nedir?",
            "Audience Manager kampanya gönderir mi?",
            "Audience Manager iletişim iznini garanti eder mi?",
            "Audience Manager yapay zekâ ile mi segment oluşturur?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        // Count details tags in FAQ section
        var faqMatch = Regex.Match(html, @"<section[^>]*id=""faq""[^>]*>(.*?)</section>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.True(faqMatch.Success);
        var detailsCount = Regex.Matches(faqMatch.Groups[1].Value, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(8, detailsCount);
    }

    // 21: Exactly 8 English FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Audience Manager?",
            "What data can Audience Manager use for segmentation?",
            "What is a dynamic audience?",
            "What is the difference between Audience Manager and Customer Intelligence?",
            "What is the difference between Audience Manager and Daily Opportunities?",
            "Does Audience Manager send campaigns?",
            "Does Audience Manager guarantee communication permission?",
            "Does Audience Manager use AI to create segments?"
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

    // 22: No forbidden structured data
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
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

    // 23: Required contextual internal links
    [Fact]
    public async Task TurkishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/content-studio\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/content-studio\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
    }

    // 24: Exact pricing lines in CTA
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/audience-manager");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 25: No fixed price patterns
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
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

    // 26: No fabricated audience-size numbers
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
    public async Task RenderedPage_ContainsNoFabricatedAudienceSizeNumbers(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("12.450", body);
        Assert.DoesNotContain("12,450", body);
        Assert.DoesNotContain("45.200", body);
        Assert.DoesNotContain("45,200", body);
    }

    // 27: No old canned examples
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
    public async Task RenderedPage_ContainsNoOldCannedExamples(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("last 30 days", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("son 30 gün", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Deniz Kaya", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("84/100", body);
    }

    // 28: No Wiki links in page content
    [Theory]
    [InlineData("/cozumler/audience-manager")]
    [InlineData("/en/solutions/audience-manager")]
    public async Task RenderedPage_ContainsNoWikiLinks(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", body, StringComparison.OrdinalIgnoreCase);
    }
}
