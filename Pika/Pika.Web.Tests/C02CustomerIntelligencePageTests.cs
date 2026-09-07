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

public class C02CustomerIntelligencePageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C02CustomerIntelligencePageTests(WebApplicationFactory<Program> factory)
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

    // 1 & 2: HTTP 200
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task GetCustomerIntelligence_ReturnsSuccess200(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 3 & 4: Title & Meta Description match SeoHelper
    [Theory]
    [InlineData("/platform/customer-intelligence", "tr")]
    [InlineData("/en/platform/customer-intelligence", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Platform", "CustomerIntelligence");
        Assert.NotNull(meta);

        var expectedTitle = lang == "tr" ? meta.TitleTr : meta.TitleEn;
        var expectedDesc = lang == "tr" ? meta.DescriptionTr : meta.DescriptionEn;

        var actualTitle = ExtractTitle(html);
        var actualDesc = ExtractMetaDescription(html);

        Assert.Equal(expectedTitle, actualTitle);
        Assert.Equal(expectedDesc, actualDesc);
    }

    // 5: Exactly one H1
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_HasExactlyOneH1(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var h1Matches = Regex.Matches(html, @"<h1(?:\s|>)", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);
    }

    // 6: TR H1 equals normalized text
    [Fact]
    public async Task TurkishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Müşterinizin kim olduğunu değil, nasıl değiştiğini anlayın.", h1Text);
    }

    // 7: EN H1 equals normalized text
    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Understand not only who your customer is, but how they are changing.", h1Text);
    }

    // 8 & 9: Direct answer questions
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence nedir?", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Customer Intelligence?", decoded);
    }

    // 10: Canonical terms
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_ContainsCanonicalTerms(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Pika 360", decoded);
        Assert.Contains("Product Intelligence", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
    }

    // 11 & 12: Günün Fırsatları / Daily Opportunities
    [Fact]
    public async Task TurkishPage_ContainsGununFirsatlari()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Günün Fırsatları", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsDailyOpportunities()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Daily Opportunities", decoded);
    }

    // 13: Email check
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_ContainsEmailThroughSharedLayout(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Email", html);
    }

    // 14: No Push references in page body
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_ContainsNoPushReferences(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var bodyMatch = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var pageBody = bodyMatch.Success ? bodyMatch.Groups[1].Value : html;
        var contentWithoutScripts = Regex.Replace(pageBody, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);

        Assert.DoesNotContain("push", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("mobile push", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("web push", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
    }

    // 15: No forbidden claims in page body
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_ContainsNoForbiddenClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var bodyMatch = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var pageBody = bodyMatch.Success ? bodyMatch.Groups[1].Value : html;
        var contentWithoutScripts = Regex.Replace(pageBody, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);

        Assert.DoesNotContain("real-time", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real time", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("milisaniye", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("milliseconds", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("%100", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
    }

    // 16: View contains no forbidden elements
    [Fact]
    public void CustomerIntelligenceView_ContainsNoForbiddenElements()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Platform", "CustomerIntelligence.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("/wiki/assets/images/", viewContent);
        Assert.DoesNotContain("<img", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("@section JsonLd", viewContent);
        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 17: No forbidden page-level JSON-LD
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
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

    // 18: Turkish FAQ contains exactly the 8 supplied questions
    [Fact]
    public async Task TurkishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Customer Intelligence nedir?",
            "Pika Customer Intelligence hangi verileri kullanır?",
            "Customer Intelligence hangi metrikleri değerlendirir?",
            "Customer Intelligence ile Pika 360 arasındaki fark nedir?",
            "Customer Intelligence segmentasyon yapar mı?",
            "Customer Intelligence tekrar satın alma zamanını anlamaya yardımcı olur mu?",
            "Customer Intelligence yapay zekâ ile mi çalışır?",
            "Customer Intelligence neden pazarlama için önemlidir?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }
    }

    // 19: English FAQ contains exactly the 8 supplied questions
    [Fact]
    public async Task EnglishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Customer Intelligence?",
            "What data does Pika Customer Intelligence use?",
            "Which metrics does Customer Intelligence evaluate?",
            "What is the difference between Customer Intelligence and Pika 360?",
            "Does Customer Intelligence perform segmentation?",
            "Can Customer Intelligence help understand repeat-purchase timing?",
            "Does Customer Intelligence rely on generative AI?",
            "Why is Customer Intelligence important for marketing?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }
    }

    // 20: Required internal links for TR
    [Fact]
    public async Task TurkishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/pika-360\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
    }

    // 20: Required internal links for EN
    [Fact]
    public async Task EnglishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/pika-360\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
    }

    // 21: No href points to legacy /platform/firsatlar
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_ContainsNoLegacyFirsatlarLinks(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("/platform/firsatlar", html);
    }

    // 22: No Wiki href in Customer Intelligence View
    [Fact]
    public void CustomerIntelligenceView_ContainsNoWikiLinks()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Platform", "CustomerIntelligence.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("/wiki/", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", viewContent, StringComparison.OrdinalIgnoreCase);
    }

    // 23: No fixed pricing patterns
    [Theory]
    [InlineData("/platform/customer-intelligence")]
    [InlineData("/en/platform/customer-intelligence")]
    public async Task RenderedPage_ContainsNoFixedPublicPricingPatterns(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var contentWithoutScripts = Regex.Replace(html, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);

        Assert.DoesNotContain("₺", contentWithoutScripts);
        Assert.DoesNotContain("$", contentWithoutScripts);
        Assert.DoesNotContain("€", contentWithoutScripts);
        Assert.DoesNotContain("free trial", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ücretsiz deneme", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
    }

    // 24: Exact pricing copy in final CTA
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/customer-intelligence");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }
}
