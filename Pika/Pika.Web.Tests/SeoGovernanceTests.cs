using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class SeoGovernanceTests
{
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
        throw new DirectoryNotFoundException("Could not locate Pika project root directory.");
    }

    [Fact]
    public void CanonicalRouteMetadata_MaintainsSymmetricTrEnHreflangPairs()
    {
        var indexedPages = SeoHelper.AllPages.Where(p => !p.NoIndex).ToList();
        Assert.NotEmpty(indexedPages);

        foreach (var page in indexedPages)
        {
            Assert.False(string.IsNullOrWhiteSpace(page.AlternatePathTr), $"Page with title '{page.TitleTr}' missing AlternatePathTr");
            Assert.False(string.IsNullOrWhiteSpace(page.AlternatePathEn), $"Page with title '{page.TitleEn}' missing AlternatePathEn");
            Assert.StartsWith("/", page.AlternatePathTr);
            Assert.StartsWith("/", page.AlternatePathEn);

            var alternates = SeoHelper.GetHreflangAlternates(page);
            Assert.Equal(3, alternates.Count);

            var trAlt = alternates.FirstOrDefault(a => a.Lang == "tr");
            var enAlt = alternates.FirstOrDefault(a => a.Lang == "en");
            var defaultAlt = alternates.FirstOrDefault(a => a.Lang == "x-default");

            Assert.NotNull(trAlt);
            Assert.NotNull(enAlt);
            Assert.NotNull(defaultAlt);

            Assert.Equal($"https://pika.tr{page.AlternatePathTr}", trAlt.Url);
            Assert.Equal($"https://pika.tr{page.AlternatePathEn}", enAlt.Url);
            Assert.Equal($"https://pika.tr{page.AlternatePathTr}", defaultAlt.Url);
        }
    }

    [Fact]
    public void Solutions_PushNotifications_IsMarkedNoindexAndExcludedFromActiveMarketing()
    {
        var push = SeoHelper.GetMetadata("Solutions", "PushNotifications");
        Assert.NotNull(push);
        Assert.True(push.NoIndex, "PushNotifications must be flagged as NoIndex in SeoHelper");
        Assert.DoesNotContain(push, SeoHelper.AllPages.Where(p => !p.NoIndex));

        var webRoot = GetWebRoot();
        var pushViewPath = Path.Combine(webRoot, "Views", "Solutions", "PushNotifications.cshtml");
        Assert.True(File.Exists(pushViewPath), "PushNotifications.cshtml must exist on disk");
        var pushView = File.ReadAllText(pushViewPath);
        Assert.Contains("ViewData[\"RobotsMeta\"] = \"noindex, follow\"", pushView);
    }

    [Fact]
    public void Layout_OrganizationJsonLd_OmitsFoundingLocation()
    {
        var webRoot = GetWebRoot();
        var layoutPath = Path.Combine(webRoot, "Views", "Shared", "_Layout.cshtml");
        Assert.True(File.Exists(layoutPath), $"Layout file not found at {layoutPath}");

        var layoutContent = File.ReadAllText(layoutPath);

        // Assert foundingLocation property is entirely omitted
        Assert.DoesNotContain("foundingLocation", layoutContent, StringComparison.OrdinalIgnoreCase);

        // Find the Organization JSON-LD script block and ensure neither İstanbul nor Ankara are present in it
        var orgScriptMatch = Regex.Match(layoutContent, @"<script\s+type=""@Html\.Raw\([^)]+\)""\s*>\s*\{[^{}]*""@*type""\s*:\s*""Organization""[\s\S]*?</script>", RegexOptions.IgnoreCase);
        Assert.True(orgScriptMatch.Success, "Organization JSON-LD script block not found in _Layout.cshtml");

        var orgScript = orgScriptMatch.Value;
        Assert.DoesNotContain("İstanbul", orgScript, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Istanbul", orgScript, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Ankara", orgScript, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Layout_OrganizationJsonLd_MatchesCanonicalP04BrandDefinition()
    {
        var webRoot = GetWebRoot();
        var layoutPath = Path.Combine(webRoot, "Views", "Shared", "_Layout.cshtml");
        var layoutContent = File.ReadAllText(layoutPath);

        Assert.Contains("\"alternateName\": \"Pika Müşteri Zekâsı ve Omnichannel Pazarlama Platformu\"", layoutContent);
        Assert.Contains("Pika; müşteri, ürün ve satış verisini birlikte anlamlandırarak işletmenin bugün hangi müşteride hangi ticari fırsatın oluştuğunu görmesini ve bu fırsatı kontrollü, ölçülebilir çok kanallı aksiyona dönüştürmesini sağlayan müşteri zekâsı ve pazarlama platformudur.", layoutContent);
    }

    [Fact]
    public void Layout_WebSiteJsonLd_ExistsAndOmitsSearchAction()
    {
        var webRoot = GetWebRoot();
        var layoutPath = Path.Combine(webRoot, "Views", "Shared", "_Layout.cshtml");
        var layoutContent = File.ReadAllText(layoutPath);

        Assert.Contains("\"WebSite\"", layoutContent);
        Assert.Contains("\"https://pika.tr/#website\"", layoutContent);
        Assert.Contains("\"https://pika.tr/#organization\"", layoutContent);
        Assert.DoesNotContain("SearchAction", layoutContent, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LlmsFiles_DoNotMarketPushAsCurrentOrRoadmap()
    {
        var webRoot = GetWebRoot();
        var llmsTxtPath = Path.Combine(webRoot, "wwwroot", "llms.txt");
        var llmsFullPath = Path.Combine(webRoot, "wwwroot", "llms-full.txt");

        Assert.True(File.Exists(llmsTxtPath), "llms.txt not found");
        Assert.True(File.Exists(llmsFullPath), "llms-full.txt not found");

        var llmsTxt = File.ReadAllText(llmsTxtPath);
        var llmsFull = File.ReadAllText(llmsFullPath);

        // llms.txt must not contain Push at all
        Assert.DoesNotContain("push", llmsTxt, StringComparison.OrdinalIgnoreCase);

        // llms-full.txt: Push must NOT be in the active execution channels line
        Assert.Contains("- **Execution Channels**: Email, SMS, Official Meta WhatsApp Business API", llmsFull);
        Assert.DoesNotContain("Execution Channels: Email, SMS, WhatsApp, Push", llmsFull, StringComparison.OrdinalIgnoreCase);

        // Any occurrence of Push in llms-full.txt must explicitly be qualified as CONTRADICTORY / UNCONFIRMED
        if (llmsFull.Contains("push", StringComparison.OrdinalIgnoreCase))
        {
            Assert.Contains("Push is CONTRADICTORY / UNCONFIRMED", llmsFull);
            Assert.Contains("must not be inferred as a live capability", llmsFull);
        }
    }

    [Fact]
    public void LlmsFiles_OmitAllStrictlyForbiddenClaims()
    {
        var webRoot = GetWebRoot();
        var llmsTxt = File.ReadAllText(Path.Combine(webRoot, "wwwroot", "llms.txt"));
        var llmsFull = File.ReadAllText(Path.Combine(webRoot, "wwwroot", "llms-full.txt"));

        var forbiddenPatterns = new[]
        {
            @"\bSOC-compliant\b",
            @"\bSOC\s*2\b",
            @"\bSOC-2\b",
            @"\bSAML\b",
            @"\bSSO\b",
            @"\bchatbot\b",
            @"\bTL/ay\b",
            @"\$/ay\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(llmsTxt, pattern, RegexOptions.IgnoreCase),
                $"llms.txt contains forbidden term matching pattern '{pattern}'");
            Assert.False(Regex.IsMatch(llmsFull, pattern, RegexOptions.IgnoreCase),
                $"llms-full.txt contains forbidden term matching pattern '{pattern}'");
        }
    }

    [Fact]
    public void SeoHelper_TitlesAndDescriptions_DoNotContainForbiddenClaims()
    {
        var forbiddenPatterns = new[]
        {
            @"\b15 dakikalık demo\b",
            @"\b15-minute demo\b",
            @"\bmilisaniyeler içinde\b",
            @"\bin milliseconds\b",
            @"\byüksek teslimat\b",
            @"\bhigh deliverability\b",
            @"\bSOC-compliant\b",
            @"\bSOC\s*2\b",
            @"\bSAML\b",
            @"\bSSO\b",
            @"\bchatbot\b",
            @"\bdoğrudan ciro atfı\b",
            @"\bdoğrudan ciro artışı\b"
        };

        foreach (var page in SeoHelper.AllPages)
        {
            foreach (var pattern in forbiddenPatterns)
            {
                if (!string.IsNullOrEmpty(page.TitleTr))
                    Assert.False(Regex.IsMatch(page.TitleTr, pattern, RegexOptions.IgnoreCase),
                        $"Page '{page.TitleTr}' TitleTr matches forbidden pattern: '{pattern}'");
                if (!string.IsNullOrEmpty(page.TitleEn))
                    Assert.False(Regex.IsMatch(page.TitleEn, pattern, RegexOptions.IgnoreCase),
                        $"Page '{page.TitleEn}' TitleEn matches forbidden pattern: '{pattern}'");
                if (!string.IsNullOrEmpty(page.DescriptionTr))
                    Assert.False(Regex.IsMatch(page.DescriptionTr, pattern, RegexOptions.IgnoreCase),
                        $"Page '{page.TitleTr}' DescriptionTr matches forbidden pattern: '{pattern}'");
                if (!string.IsNullOrEmpty(page.DescriptionEn))
                    Assert.False(Regex.IsMatch(page.DescriptionEn, pattern, RegexOptions.IgnoreCase),
                        $"Page '{page.TitleEn}' DescriptionEn matches forbidden pattern: '{pattern}'");
            }
        }
    }

    [Fact]
    public void SeoHelper_FormatPageTitle_AppliesCanonicalCategoryFallback()
    {
        var fallbackEmpty = SeoHelper.FormatPageTitle("");
        Assert.Equal("Pika | Müşteri Zekâsı ve Omnichannel Pazarlama Platformu", fallbackEmpty);

        var fallbackBrandOnly = SeoHelper.FormatPageTitle("Pika");
        Assert.Equal("Pika | Müşteri Zekâsı ve Omnichannel Pazarlama Platformu", fallbackBrandOnly);

        var formattedTitle = SeoHelper.FormatPageTitle("Müşteri Zekâsı");
        Assert.Equal("Müşteri Zekâsı | Pika", formattedTitle);
    }
}
