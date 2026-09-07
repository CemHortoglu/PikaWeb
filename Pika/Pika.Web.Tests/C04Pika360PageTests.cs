using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class C04Pika360PageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C04Pika360PageTests(WebApplicationFactory<Program> factory)
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

    private static string RunGitCommand(string workingDir, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = workingDir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);
        if (process == null) return string.Empty;
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output.Trim();
    }

    // 1 & 2: HTTP 200 for TR and EN routes
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task GetPika360_ReturnsSuccess200(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 3: Exact SeoHelper Title & Description for TR & EN
    [Theory]
    [InlineData("/platform/pika-360", "tr")]
    [InlineData("/en/platform/pika-360", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Platform", "Pika360");
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
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
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
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Müşteriyi yalnızca kimliğiyle değil, bütün karar bağlamıyla görün.", h1Text);
    }

    // 6: Exact EN H1
    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("See more than customer identity. See the complete decision context.", h1Text);
    }

    // 7: TR Direct answer question
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika 360 nedir?", decoded);
    }

    // 8: EN Direct answer question
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Pika 360?", decoded);
    }

    // 9: Canonical terms
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task RenderedPage_ContainsCanonicalTerms(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika 360", decoded);
        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Product Intelligence", decoded);
    }

    // 10: TR Günün Fırsatları
    [Fact]
    public async Task TurkishPage_ContainsGununFirsatlari()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Günün Fırsatları", decoded);
    }

    // 11: EN Daily Opportunities
    [Fact]
    public async Task EnglishPage_ContainsDailyOpportunities()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Daily Opportunities", decoded);
    }

    // 12: Five core semantic contexts
    [Fact]
    public async Task TurkishPage_ContainsFiveSemanticContexts()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İşlem Geçmişi", decoded);
        Assert.Contains("Müşteri Değeri", decoded);
        Assert.Contains("Davranış Bağlamı", decoded);
        Assert.Contains("İletişim Erişilebilirliği", decoded);
        Assert.Contains("Açık Fırsatlar", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsFiveSemanticContexts()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Transaction History", decoded);
        Assert.Contains("Customer Value", decoded);
        Assert.Contains("Behavioral Context", decoded);
        Assert.Contains("Communication Reachability", decoded);
        Assert.Contains("Open Opportunities", decoded);
    }

    // 13: View contains no wiki, no jsonld section, no viewdata overrides
    [Fact]
    public void Pika360View_ContainsNoForbiddenElements()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Platform", "Pika360.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("/wiki/", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("@section JsonLd", viewContent);
        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 14: View contains no <img> or screenshot references
    [Fact]
    public void Pika360View_ContainsNoImageTagsOrScreenshots()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Platform", "Pika360.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("<img", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", viewContent, StringComparison.OrdinalIgnoreCase);
    }

    // 15: No push references in page body
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
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

    // 16: No forbidden claims in page body
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task RenderedPage_ContainsNoForbiddenClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var bodyMatch = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var pageBody = bodyMatch.Success ? bodyMatch.Groups[1].Value : html;
        var contentWithoutScripts = Regex.Replace(pageBody, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);

        Assert.DoesNotContain("single-customer truth", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("single source of truth", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("instantly", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("empirical churn", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("active trigger", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real-time", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real time", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("milisaniye", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("milliseconds", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
    }

    // 17: No fabricated customer name: Deniz Kaya
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task RenderedPage_ContainsNoFabricatedCustomerName(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("Deniz Kaya", html, StringComparison.OrdinalIgnoreCase);
    }

    // 18: No fabricated metrics: 84/100, 30 gün, 30 days
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task RenderedPage_ContainsNoFabricatedMetrics(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("84/100", html);
        Assert.DoesNotContain("30 gün", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("30 days", html, StringComparison.OrdinalIgnoreCase);
    }

    // 19: No page-level SoftwareApplication, FAQPage, Offer JSON-LD
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
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

    // 20: Exactly 8 Turkish FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Pika 360 nedir?",
            "Pika 360 hangi bilgileri bir araya getirir?",
            "Pika 360 ile Customer Intelligence arasındaki fark nedir?",
            "Pika 360 ile Günün Fırsatları arasındaki fark nedir?",
            "Pika 360 bir CRM midir?",
            "Pika 360 kampanya gönderir mi?",
            "Pika 360'taki müşteri bağlamını yapay zekâ mı oluşturur?",
            "Pika 360 ile genel Customer 360 yaklaşımı arasındaki fark nedir?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }
    }

    // 21: Exactly 8 English FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Pika 360?",
            "What information does Pika 360 bring together?",
            "What is the difference between Pika 360 and Customer Intelligence?",
            "What is the difference between Pika 360 and Daily Opportunities?",
            "Is Pika 360 a CRM?",
            "Does Pika 360 send campaigns?",
            "Is the customer context in Pika 360 generated by AI?",
            "How is Pika 360 different from a generic Customer 360 approach?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }
    }

    // 22: Required internal links for TR
    [Fact]
    public async Task TurkishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
    }

    // 23: Required internal links for EN
    [Fact]
    public async Task EnglishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
    }

    // 24: No legacy /platform/firsatlar links
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task RenderedPage_ContainsNoLegacyFirsatlarLinks(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("/platform/firsatlar", html);
    }

    // 25: No fixed public pricing patterns in page body
    [Theory]
    [InlineData("/platform/pika-360")]
    [InlineData("/en/platform/pika-360")]
    public async Task RenderedPage_ContainsNoFixedPublicPricingPatterns(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var bodyMatch = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var pageBody = bodyMatch.Success ? bodyMatch.Groups[1].Value : html;
        var contentWithoutScripts = Regex.Replace(pageBody, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);

        Assert.DoesNotContain("₺", contentWithoutScripts);
        Assert.DoesNotContain("$", contentWithoutScripts);
        Assert.DoesNotContain("€", contentWithoutScripts);
        Assert.DoesNotContain("free trial", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ücretsiz deneme", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
    }

    // 26: Exact pricing copy in final CTA
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/pika-360");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 27: Homepage frozen files unchanged
    [Fact]
    public void HomepageFrozenFiles_AreUnchanged()
    {
        var root = GetProjectRoot();
        var frozenFiles = new[]
        {
            "Views/Home/Index.cshtml",
            "wwwroot/css/pika-home.css",
            "wwwroot/css/pika-orbit.css",
            "wwwroot/js/pika-orbit.js",
            "Views/Shared/_Layout.cshtml"
        };

        foreach (var file in frozenFiles)
        {
            var diff = RunGitCommand(root, $"diff --name-only HEAD -- {file}");
            Assert.True(string.IsNullOrWhiteSpace(diff), $"Frozen file was modified: {file}");
        }
    }

    // 28: _MarketingHero.cshtml unchanged
    [Fact]
    public void MarketingHero_IsUnchanged()
    {
        var root = GetProjectRoot();
        var file = "Views/Shared/_MarketingHero.cshtml";
        var diff = RunGitCommand(root, $"diff --name-only HEAD -- {file}");
        Assert.True(string.IsNullOrWhiteSpace(diff), $"Shared marketing hero was modified: {file}");
    }

    // 29: SeoHelper has only Platform.Pika360 modified
    [Fact]
    public void SeoHelper_OnlyPika360WasModified()
    {
        var root = GetProjectRoot();
        var diff = RunGitCommand(root, "diff HEAD -- Services/SeoHelper.cs");
        Assert.NotEmpty(diff);
        Assert.Contains("Platform.Pika360", diff);

        // Ensure no other keys in SeoHelper were touched in this branch working tree
        var keyMatches = Regex.Matches(diff, @"^\+\s*\[""([^""]+)""\]", RegexOptions.Multiline);
        foreach (Match match in keyMatches)
        {
            Assert.Equal("Platform.Pika360", match.Groups[1].Value);
        }
    }
}
