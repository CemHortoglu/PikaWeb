using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class P06HomepageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public P06HomepageTests(WebApplicationFactory<Program> factory)
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

    private static string GetWebRoot()
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
        throw new InvalidOperationException("Could not locate Pika project root directory.");
    }

    private static string ExtractTitle(string html)
    {
        var match = Regex.Match(html, @"<title>([^<]*)</title>", RegexOptions.IgnoreCase);
        return match.Success ? WebUtility.HtmlDecode(match.Groups[1].Value.Trim()) : string.Empty;
    }

    private static string ExtractMetaDescription(string html)
    {
        var match = Regex.Match(html, @"<meta\s+name=""description""\s+content=""([^""]*)""", RegexOptions.IgnoreCase);
        return match.Success ? WebUtility.HtmlDecode(match.Groups[1].Value.Trim()) : string.Empty;
    }

    private static List<string> ExtractAllJsonLdBlocks(string html)
    {
        var matches = Regex.Matches(html, @"<script[^>]*type=[""']application/ld(?:&#x2B;|\+)json[""'][^>]*>([\s\S]*?)</script>", RegexOptions.IgnoreCase);
        return matches.Select(m => m.Groups[1].Value).ToList();
    }

    // =========================================================================
    // 1 & 2: 200 OK on TR (/) and EN (/en/)
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_Returns200Ok(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // =========================================================================
    // 3 & 4: Canonical Title and Meta Description (via SeoHelper authority)
    // =========================================================================

    [Fact]
    public async Task TurkishHomepage_RendersCanonicalTitleAndMetaDescriptionFromSeoHelper()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        var seoMeta = SeoHelper.GetMetadata("Home", "Index");
        Assert.NotNull(seoMeta);

        var title = ExtractTitle(html);
        var metaDesc = ExtractMetaDescription(html);

        var expectedTitle = SeoHelper.FormatPageTitle(seoMeta.TitleTr);
        Assert.Equal(expectedTitle, title);
        Assert.Equal(seoMeta.DescriptionTr, metaDesc);
        Assert.Contains("Müşteri Zekâsı ve Omnichannel Pazarlama Platformu", title);
    }

    [Fact]
    public async Task EnglishHomepage_RendersCanonicalTitleAndMetaDescriptionFromSeoHelper()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/");
        var html = await response.Content.ReadAsStringAsync();

        var seoMeta = SeoHelper.GetMetadata("Home", "Index");
        Assert.NotNull(seoMeta);

        var title = ExtractTitle(html);
        var metaDesc = ExtractMetaDescription(html);

        var expectedTitle = SeoHelper.FormatPageTitle(seoMeta.TitleEn);
        Assert.Equal(expectedTitle, title);
        Assert.Equal(seoMeta.DescriptionEn, metaDesc);
        Assert.Contains("Customer Intelligence & Omnichannel Marketing Platform", title);
    }

    // =========================================================================
    // 5: Absense of Push Notifications in Visible Marketing Copy & Grids
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_ContainsNoPushNotificationClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        // Ensure push notification is not advertised anywhere on homepage
        Assert.DoesNotContain("push notification", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("web push", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("mobil push", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("mobile push", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("anlık bildirim", html, StringComparison.OrdinalIgnoreCase);
    }

    // =========================================================================
    // 6: Absense of "tüm kanallar" or "all channels"
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_DoesNotContainUnconstrainedChannelClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("tüm kanallar", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("all channels", html, StringComparison.OrdinalIgnoreCase);
    }

    // =========================================================================
    // 7 & 8: Schema Hygiene: No SoftwareApplication, No FAQPage, No Offer
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_DoesNotRenderSoftwareApplicationOrFAQPageSchema(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var jsonLdBlocks = ExtractAllJsonLdBlocks(html);
        Assert.NotEmpty(jsonLdBlocks);

        foreach (var block in jsonLdBlocks)
        {
            Assert.DoesNotContain("\"SoftwareApplication\"", block, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("\"FAQPage\"", block, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("\"Offer\"", block, StringComparison.OrdinalIgnoreCase);
        }
    }

    // =========================================================================
    // 9: Absence of Old Fake Synthetic Numbers (12.4K, %68, %24)
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_DoesNotContainOldSyntheticMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("12.4K", html);
        Assert.DoesNotContain("%68", html);
        Assert.DoesNotContain("%24", html);
        Assert.DoesNotContain("ph-mock", html);
    }

    // =========================================================================
    // 10: Absence of Old Legacy Category Name and "Enterprise-Grade"
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_DoesNotContainOldCategoryNameOrEnterpriseGrade(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("Omnichannel Pazarlama Otomasyonu ve Müşteri Yolculuğu Platformu", html);
        Assert.DoesNotContain("enterprise-grade", html, StringComparison.OrdinalIgnoreCase);
    }

    // =========================================================================
    // 11: 9 Chapter Landmark Sections Present
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_ContainsAllNineChapterLandmarkSections(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var expectedSectionIds = new[]
        {
            "hero",
            "firsatlar",
            "calisma-modeli",
            "zeka-katmani",
            "pika-360",
            "aksiyon-ve-icra",
            "pika-pilot",
            "yonetisim-ve-olcumleme",
            "teklif-ve-demo"
        };

        foreach (var id in expectedSectionIds)
        {
            Assert.Contains($"id=\"{id}\"", html);
        }
    }

    // =========================================================================
    // 12: Real Product Screenshots Exist on Disk
    // =========================================================================

    [Fact]
    public void Homepage_AllReferencedScreenshotFilesExistOnDisk()
    {
        var webRoot = GetWebRoot();
        var screenshotsDir = Path.Combine(webRoot, "wwwroot", "wiki", "assets", "images");

        var expectedScreenshots = new[]
        {
            "img_pika-360_7.png",
            "img_gunun-firsatlari.png",
            "img_excel-csv-aktarimi_1.png",
            "img_tekrar-satin-alma-analizi_6.png",
            "img_urun-siniflandirma-workbench_13.png",
            "img_journey-tasarim-tuvali_18.png",
            "img_kisi-listesi-ve-segmentler_3.png",
            "img_yayinlama-sablon-ve-yonetim_27.png",
            "img_pika-pilot-ai-kampanya-asistani_16.png",
            "img_bi-kokpit_5.png"
        };

        foreach (var file in expectedScreenshots)
        {
            var filePath = Path.Combine(screenshotsDir, file);
            Assert.True(File.Exists(filePath), $"Referenced screenshot '{file}' does not exist at {filePath}");
        }
    }

    // =========================================================================
    // 13: Insider One-Style Fluid Storytelling & Animated Mockups
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_RendersInsiderOneFluidCompositionsAndMockups(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        // Atmospheric Dark Hero & Floating Vignettes
        Assert.Contains("pw2-hero-insider", html);
        Assert.Contains("pw2-hero-console-card", html);
        Assert.Contains("pw2-float-badge", html);

        // Trust & Channel Ribbon
        Assert.Contains("pw2-trust-ribbon", html);

        // CCE Bento Grid
        Assert.Contains("pw2-cce-grid", html);
        Assert.Contains("pw2-cce-card", html);

        // Operating Model Pipeline
        Assert.Contains("pw2-pipeline-stage", html);

        // Twin Intelligence & CVS Dial
        Assert.Contains("pw2-intel-grid", html);
        Assert.Contains("pw2-cvs-circle", html);

        // Pika 360 Console Stage
        Assert.Contains("pw2-360-stage", html);

        // Omnichannel Bento with Smartphone WhatsApp Mockup
        Assert.Contains("pw2-omni-bento", html);
        Assert.Contains("pw2-phone-mockup", html);
        Assert.Contains("pw2-wa-bubble", html);

        // Grounded AI Studio Terminal
        Assert.Contains("pw2-ai-terminal", html);
        Assert.Contains("pw2-ai-prompt-box", html);

        // Telemetry & Attributed Revenue SVG Area Chart
        Assert.Contains("pw2-telemetry-stage", html);
        Assert.Contains("<svg", html);

        // Atmospheric Horizon CTA & Giant Watermark
        Assert.Contains("pw2-cta-horizon", html);
        Assert.Contains("pw2-brand-watermark", html);
    }

    // =========================================================================
    // 14: Commercial Copy Governance (No Pricing Tables, Custom Quote Copy)
    // =========================================================================

    [Fact]
    public async Task TurkishHomepage_RendersCanonicalCommercialAndGovernanceCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        // Canonical pricing statement
        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", html);

        // Revenue attribution terminology governance (ilişkilendirilen ciro / ciro atfı)
        Assert.Contains("ilişkilendirilen ciro", html, StringComparison.OrdinalIgnoreCase);

        // Dispatch windows governance
        Assert.Contains("gönderim zaman pencereleri", html, StringComparison.OrdinalIgnoreCase);

        // Absence of prohibited phrases
        Assert.DoesNotContain("KVKK quiet hours", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("sales caused by", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task EnglishHomepage_RendersCanonicalCommercialAndGovernanceCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/");
        var html = await response.Content.ReadAsStringAsync();

        // Canonical pricing statement
        Assert.Contains("Custom quote based on your specific needs and usage scope.", html);

        // Revenue attribution terminology governance
        Assert.Contains("attributed revenue", html, StringComparison.OrdinalIgnoreCase);

        // Dispatch windows governance
        Assert.Contains("quiet-hours dispatch window", html, StringComparison.OrdinalIgnoreCase);
    }

    // =========================================================================
    // 15: Accessibility & Semantic Structure
    // =========================================================================

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_CompliesWithAccessibilityAndSemanticStandards(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        // Exactly one H1 per page
        var h1Matches = Regex.Matches(html, @"<h1[^>]*>([\s\S]*?)</h1>", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);

        // All img tags inside main content have non-empty alt attributes
        var imgMatches = Regex.Matches(html, @"<img\s+[^>]*>", RegexOptions.IgnoreCase);
        Assert.NotEmpty(imgMatches);
        foreach (Match img in imgMatches)
        {
            var altMatch = Regex.Match(img.Value, @"alt=[""']([^""']*)[""']", RegexOptions.IgnoreCase);
            Assert.True(altMatch.Success, $"Image tag missing alt attribute: {img.Value}");
            Assert.False(string.IsNullOrWhiteSpace(altMatch.Groups[1].Value), $"Image tag has empty alt attribute: {img.Value}");
        }

        // Check for decorative icons inside main content having aria-hidden
        var mainMatch = Regex.Match(html, @"<main[^>]*>([\s\S]*?)</main>", RegexOptions.IgnoreCase);
        Assert.True(mainMatch.Success, "Main content container not found");
        var mainHtml = mainMatch.Groups[1].Value;

        var decorativeIconMatches = Regex.Matches(mainHtml, @"<i\s+class=[""'][^""']*ri-[^""']*[""'][^>]*>", RegexOptions.IgnoreCase);
        Assert.NotEmpty(decorativeIconMatches);
        foreach (Match icon in decorativeIconMatches)
        {
            Assert.Contains("aria-hidden=\"true\"", icon.Value);
        }
    }
}

