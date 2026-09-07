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

public class C01PikaPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C01PikaPageTests(WebApplicationFactory<Program> factory)
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

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task GetPika_ReturnsSuccess200(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("/pika", "tr")]
    [InlineData("/en/pika", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Home", "Pika");
        Assert.NotNull(meta);

        var expectedTitle = lang == "tr" ? meta.TitleTr : meta.TitleEn;
        var expectedDesc = lang == "tr" ? meta.DescriptionTr : meta.DescriptionEn;

        var actualTitle = ExtractTitle(html);
        var actualDesc = ExtractMetaDescription(html);

        Assert.Equal(expectedTitle, actualTitle);
        Assert.Equal(expectedDesc, actualDesc);
    }

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task RenderedPage_HasExactlyOneH1(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var h1Matches = Regex.Matches(html, @"<h1(?:\s|>)", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);
    }

    [Fact]
    public async Task TurkishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/pika");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Müşterinizi anlamadan pazarlamaya başlamayın.", h1Text);
    }

    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/pika");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Understand your customer before you start marketing.", h1Text);
    }

    [Fact]
    public async Task TurkishPage_ContainsCanonicalCategoryAndDirectAnswer()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/pika");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Müşteri Zekâsı ve Omnichannel Pazarlama Platformu", decoded);
        Assert.Contains("Pika nedir?", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsCanonicalCategoryAndDirectAnswer()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/pika");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence & Omnichannel Marketing Platform", decoded);
        Assert.Contains("What is Pika?", decoded);
    }

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task RenderedPage_ContainsAllCanonicalEntities(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        // 10 Canonical entities
        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Product Intelligence", decoded);
        Assert.Contains("Pika 360", decoded);
        Assert.True(decoded.Contains("Günün Fırsatları") || decoded.Contains("Daily Opportunities"));
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Content Studio", decoded);
        Assert.Contains("Pika Pilot", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
    }

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task RenderedPage_ContainsOnlyConfirmedExecutionChannels(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Email", html);
        Assert.Contains("SMS", html);
        Assert.Contains("WhatsApp", html);
    }

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task RenderedPage_ContainsNoPushReferences(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        // Must not contain Push, Mobile Push, Web Push in rendered content (stripping layout analytics script tags)
        var contentWithoutScripts = Regex.Replace(html, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);
        Assert.DoesNotContain("push", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("mobile push", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("web push", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task RenderedPage_ContainsNoPageLevelForbiddenJsonLd(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        // No SoftwareApplication, Offer, or FAQPage JSON-LD
        Assert.DoesNotContain("SoftwareApplication", html);
        Assert.DoesNotContain("FAQPage", html);
        Assert.DoesNotContain("\"@type\": \"Offer\"", html);
        Assert.DoesNotContain("\"@type\":\"Offer\"", html);
    }

    [Fact]
    public void PikaView_ContainsNoScreenshotReferencesOrWikiImages()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Pika.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("/wiki/assets/images/", viewContent);
        Assert.DoesNotContain("<img", viewContent, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/pika")]
    [InlineData("/en/pika")]
    public async Task RenderedPage_ContainsNoFixedPublicPricingPatterns(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("fixed price", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("starting from", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("monthly package", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("free trial", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("99.99%", html);
    }

    [Fact]
    public async Task TurkishPage_ContainsExactCommercialPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/pika");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactCommercialPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/pika");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", html);
    }

    [Fact]
    public async Task TurkishFaq_ContainsExactEightQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/pika");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var questions = new[]
        {
            "Pika nedir?",
            "Pika nasıl çalışır?",
            "Pika hangi verileri kullanır?",
            "Pika hangi iletişim kanallarını destekler?",
            "Pika CRM veya CDP midir?",
            "Pika'da yapay zekâ ne yapar?",
            "Günün Fırsatları nedir?",
            "Pika fiyatlandırması nasıl belirlenir?"
        };

        foreach (var q in questions)
        {
            Assert.Contains(q, decoded);
        }
    }

    [Fact]
    public async Task EnglishFaq_ContainsExactEightQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/pika");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var questions = new[]
        {
            "What is Pika?",
            "How does Pika work?",
            "What data does Pika use?",
            "Which communication channels does Pika support?",
            "Is Pika a CRM or CDP?",
            "What does AI do in Pika?",
            "What are Daily Opportunities?",
            "How is Pika priced?"
        };

        foreach (var q in questions)
        {
            Assert.Contains(q, decoded);
        }
    }

    [Fact]
    public async Task TurkishPage_ContainsInternalCanonicalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/pika");
        var html = await response.Content.ReadAsStringAsync();

        var expectedLinks = new[]
        {
            "/platform/customer-intelligence",
            "/platform/product-intelligence",
            "/platform/pika-360",
            "/platform/gunun-firsatlari",
            "/cozumler/audience-manager",
            "/cozumler/campaign-manager",
            "/cozumler/journey-manager",
            "/cozumler/content-studio",
            "/urunler/ai-kampanya-asistani",
            "/cozumler/analytics-reporting"
        };

        foreach (var link in expectedLinks)
        {
            Assert.Contains($"href=\"{link}\"", html);
        }
    }

    [Fact]
    public async Task EnglishPage_ContainsInternalCanonicalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/pika");
        var html = await response.Content.ReadAsStringAsync();

        var expectedLinks = new[]
        {
            "/en/platform/customer-intelligence",
            "/en/platform/product-intelligence",
            "/en/platform/pika-360",
            "/en/platform/opportunities",
            "/en/solutions/audience-manager",
            "/en/solutions/campaign-manager",
            "/en/solutions/journey-manager",
            "/en/solutions/content-studio",
            "/en/products/ai-campaign-assistant",
            "/en/solutions/analytics-reporting"
        };

        foreach (var link in expectedLinks)
        {
            Assert.Contains($"href=\"{link}\"", html);
        }
    }

    [Fact]
    public void PikaView_DoesNotDefineViewDataSeoMetadata()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Pika.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }
}
