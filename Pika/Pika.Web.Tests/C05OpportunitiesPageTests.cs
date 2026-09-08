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

public class C05OpportunitiesPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C05OpportunitiesPageTests(WebApplicationFactory<Program> factory)
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
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
    public async Task GetOpportunities_ReturnsSuccess200(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 3: Exact SeoHelper Title & Description for TR & EN
    [Theory]
    [InlineData("/platform/gunun-firsatlari", "tr")]
    [InlineData("/en/platform/opportunities", "en")]
    public async Task RenderedSeo_MatchesSeoHelper_ForTrAndEn(string path, string lang)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        var meta = SeoHelper.GetMetadata("Platform", "Opportunities");
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
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
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
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Bugün hangi müşteride hangi fırsat var?", h1Text);
    }

    // 6: Exact EN H1
    [Fact]
    public async Task EnglishH1_MatchesExactCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/opportunities");
        var html = await response.Content.ReadAsStringAsync();

        var h1Text = ExtractH1(html);
        Assert.Equal("Which customer has which opportunity today?", h1Text);
    }

    // 7: TR Direct answer question
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Günün Fırsatları nedir?", decoded);
    }

    // 8: EN Direct answer question
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/opportunities");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What are Daily Opportunities?", decoded);
    }

    // 9: Core opportunity types
    [Fact]
    public async Task TurkishPage_ContainsCoreOpportunityTypes()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Tekrar Satın Alma", decoded);
        Assert.Contains("Cross-sell", decoded);
        Assert.Contains("Geri Kazanım", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsCoreOpportunityTypes()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/opportunities");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Repeat Purchase", decoded);
        Assert.Contains("Cross-sell", decoded);
        Assert.Contains("Win-back", decoded);
    }

    // 10: View contains no forbidden legacy elements
    [Fact]
    public void OpportunitiesView_ContainsNoForbiddenElements()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Platform", "Opportunities.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("wikiBase", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/wiki/", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("@section JsonLd", viewContent);
        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 11: Rendered page contains no canonical opportunity cards/headings named Upsell, Loyalty, Value Retention, Opportunity Confidence
    [Theory]
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
    public async Task RenderedPage_ContainsNoRemovedOpportunityBucketsInHeadingsOrCards(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        // Check headings h1-h4
        var headingMatches = Regex.Matches(html, @"<h[1-4][^>]*>(.*?)</h[1-4]>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        foreach (Match match in headingMatches)
        {
            var text = Regex.Replace(match.Groups[1].Value, @"<[^>]+>", " ").Trim();
            Assert.DoesNotContain("Upsell", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Loyalty", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Value Retention", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Opportunity Confidence", text, StringComparison.OrdinalIgnoreCase);
        }
    }

    // 12: No links to /platform/firsatlar
    [Theory]
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
    public async Task RenderedPage_ContainsNoLegacyFirsatlarLinks(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("/platform/firsatlar", html);
    }

    // 13: View contains no <img> or screenshot references
    [Fact]
    public void OpportunitiesView_ContainsNoImageTagsOrScreenshots()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Platform", "Opportunities.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("<img", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("screenshot", viewContent, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekran görüntüsü", viewContent, StringComparison.OrdinalIgnoreCase);
    }

    // 14: No fabricated customer names or metrics
    [Theory]
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
    public async Task RenderedPage_ContainsNoFabricatedMetricsOrCustomerNames(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("Deniz Kaya", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("84/100", html);
    }

    // 15: No forbidden claims in page body
    [Theory]
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
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
        Assert.DoesNotContain("ROAS", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("next best action", contentWithoutScripts, StringComparison.OrdinalIgnoreCase);

        // Stripping allowed explicit negation "not guaranteed" / "satış garantisi vermez" / "garanti edilmez"
        var withoutAllowedNegation = contentWithoutScripts
            .Replace("not guaranteed", "", StringComparison.OrdinalIgnoreCase)
            .Replace("does not guarantee", "", StringComparison.OrdinalIgnoreCase)
            .Replace("garantisi vermez", "", StringComparison.OrdinalIgnoreCase)
            .Replace("garanti edilmez", "", StringComparison.OrdinalIgnoreCase);

        Assert.DoesNotContain("guaranteed", withoutAllowedNegation, StringComparison.OrdinalIgnoreCase);
    }

    // 16: No page-level SoftwareApplication, FAQPage, Offer JSON-LD
    [Theory]
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
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

    // 17: Exactly 8 Turkish FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Günün Fırsatları nedir?",
            "Günün Fırsatları hangi fırsat türlerini gösterir?",
            "Tekrar satın alma fırsatı nasıl oluşur?",
            "Cross-sell fırsatı nasıl oluşur?",
            "Geri kazanım fırsatı nasıl oluşur?",
            "Günün Fırsatları otomatik kampanya gönderir mi?",
            "Günün Fırsatları ile Pika 360 arasındaki fark nedir?",
            "Günün Fırsatları yapay zekâ tarafından mı oluşturulur?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }
    }

    // 18: Exactly 8 English FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsAllEightFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/opportunities");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What are Daily Opportunities?",
            "Which opportunity types does Daily Opportunities show?",
            "How is a repeat-purchase opportunity formed?",
            "How is a cross-sell opportunity formed?",
            "How is a win-back opportunity formed?",
            "Does Daily Opportunities automatically send campaigns?",
            "What is the difference between Daily Opportunities and Pika 360?",
            "Are Daily Opportunities generated by AI?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }
    }

    // 19: Required internal links for TR
    [Fact]
    public async Task TurkishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/pika-360\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
    }

    // 20: Required internal links for EN
    [Fact]
    public async Task EnglishPage_ContainsRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/opportunities");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/pika-360\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
    }

    // 21: Exact pricing copy in final CTA
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/platform/opportunities");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 22: No fixed public pricing patterns in page body
    [Theory]
    [InlineData("/platform/gunun-firsatlari")]
    [InlineData("/en/platform/opportunities")]
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

    // 23: Homepage frozen files unchanged
    [Fact]
    public void HomepageFrozenFiles_AreUnchanged()
    {
        var root = GetProjectRoot();
        var frozenFiles = new[]
        {
            "Views/Home/Index.cshtml",
            "wwwroot/css/pika-home.css",
            "wwwroot/css/pika-orbit.css",
            "wwwroot/js/pika-orbit.js"
        };

        foreach (var file in frozenFiles)
        {
            // Site-wide editorial cleanup permits only this homepage border change.
            if (file == "wwwroot/css/pika-orbit.css")
            {
                var baseline = RunGitCommand(root, "show HEAD:./wwwroot/css/pika-orbit.css")
                    .Replace("border-left:2px solid #c5d59e", "border: 1px solid rgba(128, 148, 139, .24)")
                    .Replace("\r\n", "\n");
                Assert.NotEmpty(baseline);
                Assert.Equal(baseline, File.ReadAllText(Path.Combine(root, file)).Replace("\r\n", "\n").Trim());
                continue;
            }
            var diff = RunGitCommand(root, $"diff --name-only HEAD -- {file}");
            Assert.True(string.IsNullOrWhiteSpace(diff), $"Frozen file was modified: {file}");
        }
    }

    // 24: _MarketingHero.cshtml unchanged
    [Fact]
    public void MarketingHero_IsUnchanged()
    {
        var root = GetProjectRoot();
        var file = "Views/Shared/_MarketingHero.cshtml";
        var diff = RunGitCommand(root, $"diff --name-only HEAD -- {file}");
        Assert.True(string.IsNullOrWhiteSpace(diff), $"Shared marketing hero was modified: {file}");
    }

    // 25: SeoHelper has only Platform.Opportunities modified in this stage
    [Fact]
    public void SeoHelper_OnlyOpportunitiesWasModified()
    {
        var root = GetProjectRoot();
        var diff = RunGitCommand(root, "diff 7f9ea1c2da3e3fcc684f5a363f460330ad893fbf -- Services/SeoHelper.cs");
        Assert.NotEmpty(diff);
        Assert.Contains("Platform.Opportunities", diff);

        // Ensure no other keys in SeoHelper were touched in C05
        var keyMatches = Regex.Matches(diff, @"^\+\s*\[""([^""]+)""\]", RegexOptions.Multiline);
        foreach (Match match in keyMatches)
        {
            Assert.Equal("Platform.Opportunities", match.Groups[1].Value);
        }
    }
}
