using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class SeoGovernanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public SeoGovernanceTests(WebApplicationFactory<Program> factory)
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

    private static string ExtractMetaDescription(string html)
    {
        var match = Regex.Match(html, @"<meta\s+name=""description""\s+content=""([^""]*)""", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    private static string ExtractMetaRobots(string html)
    {
        var match = Regex.Match(html, @"<meta\s+name=""robots""\s+content=""([^""]*)""", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    private static List<string> ExtractAllJsonLdBlocks(string html)
    {
        var matches = Regex.Matches(html, @"<script[^>]*type=[""']application/ld(?:&#x2B;|\+)json[""'][^>]*>([\s\S]*?)</script>", RegexOptions.IgnoreCase);
        return matches.Select(m => m.Groups[1].Value).ToList();
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
    public void Layout_RobotsMeta_RespectsSeoHelperNoIndexAsRuntimeFallback()
    {
        var webRoot = GetWebRoot();
        var layoutPath = Path.Combine(webRoot, "Views", "Shared", "_Layout.cshtml");
        var layoutContent = File.ReadAllText(layoutPath);

        // Asserts runtime precedence: explicit ViewData["RobotsMeta"] ?? (seoMeta?.NoIndex == true ? "noindex, follow" : "index, follow")
        Assert.Contains("seoMeta?.NoIndex == true ? \"noindex, follow\" : \"index, follow\"", layoutContent);
    }

    [Fact]
    public void HomepageMetadata_DoesNotContainAllChannelsOrTumKanallar()
    {
        var homeMeta = SeoHelper.GetMetadata("Home", "Index");
        Assert.NotNull(homeMeta);

        Assert.DoesNotContain("tüm kanallar", homeMeta.DescriptionTr, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("all channels", homeMeta.DescriptionEn, StringComparison.OrdinalIgnoreCase);

        // Asserts explicitly confirmed channels are listed instead
        Assert.Contains("E-posta, SMS ve WhatsApp", homeMeta.DescriptionTr);
        Assert.Contains("Email, SMS, and WhatsApp", homeMeta.DescriptionEn);
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

    [Fact]
    public void P05Documentation_DoesNotReferenceNonExistentClaimIds()
    {
        var webRoot = GetWebRoot();
        var p05DocPath = Path.Combine(webRoot, "docs", "marketing", "P05_SEO_LLM_ENTITY_ARCHITECTURE.md");
        Assert.True(File.Exists(p05DocPath), $"P05 doc not found at {p05DocPath}");

        var content = File.ReadAllText(p05DocPath);

        // Must not reference CLM-021 or higher
        var invalidClaimMatch = Regex.Match(content, @"\bCLM-02[1-9]\b|\bCLM-0[3-9][0-9]\b|\bCLM-[1-9][0-9]{2,}\b");
        Assert.False(invalidClaimMatch.Success,
            $"P05 documentation references invalid nonexistent claim ID: '{invalidClaimMatch.Value}'");
    }

    [Fact]
    public void EntityRegistry_DoesNotContainUngovernedEvidenceLabels()
    {
        var webRoot = GetWebRoot();
        var entityRegistryPath = Path.Combine(webRoot, "docs", "marketing", "ENTITY_REGISTRY.md");
        Assert.True(File.Exists(entityRegistryPath), $"Entity registry not found at {entityRegistryPath}");

        var content = File.ReadAllText(entityRegistryPath);

        // Established evidence vocabulary: A. CODE_VERIFIED, B. DOCUMENTED, C. MARKETING_ONLY, D. CONTRADICTORY, E. UNVERIFIED
        Assert.DoesNotContain("CODEBASE_VERIFIED", content);
        Assert.DoesNotContain("UI_VERIFIED", content);
        Assert.DoesNotContain("UNCONFIRMED_CAPABILITY", content);
    }

    [Theory]
    [InlineData("/kurumsal", "Home", "Corporate", true)]
    [InlineData("/en/corporate", "Home", "Corporate", false)]
    [InlineData("/demo-talebi", "Home", "DemoRequest", true)]
    [InlineData("/en/demo-request", "Home", "DemoRequest", false)]
    public async Task RenderedMetadata_CanonicalRoutes_UseSeoHelperMetadata_AndOmitLegacyStrings(string path, string controller, string action, bool isTr)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var renderedDesc = ExtractMetaDescription(html);
        Assert.False(string.IsNullOrWhiteSpace(renderedDesc), $"Rendered meta description was empty for {path}");

        var expectedMeta = SeoHelper.GetMetadata(controller, action);
        Assert.NotNull(expectedMeta);
        var expectedDesc = isTr ? expectedMeta.DescriptionTr : expectedMeta.DescriptionEn;
        Assert.Equal(expectedDesc, renderedDesc);

        // Assert that rendered metadata does NOT contain legacy strings
        Assert.DoesNotContain("15 dakikalık demo", renderedDesc, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("15-minute tailored demo", renderedDesc, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("yüksek güvenlik standartları", renderedDesc, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("high security standards", renderedDesc, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("tüm kanallar", renderedDesc, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("all channels", renderedDesc, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/kanallar/push")]
    [InlineData("/en/channels/push")]
    public async Task RenderedMetadata_PushQuarantine_EmitsNoindexAndNoHreflangs(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var renderedRobots = ExtractMetaRobots(html);
        Assert.Equal("noindex, follow", renderedRobots);

        // Asserts no hreflang alternates are emitted for quarantined route
        Assert.DoesNotContain("<link rel=\"alternate\" hreflang=", html, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/cozumler/consent-management", true)]
    [InlineData("/en/solutions/consent-management", false)]
    public async Task RenderedMetadata_ConsentManagement_UsesMechanismBasedDescription(string path, bool isTr)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var renderedDesc = ExtractMetaDescription(html);
        Assert.False(string.IsNullOrWhiteSpace(renderedDesc));

        var expectedMeta = SeoHelper.GetMetadata("Solutions", "ConsentManagement");
        Assert.NotNull(expectedMeta);
        var expectedDesc = isTr ? expectedMeta.DescriptionTr : expectedMeta.DescriptionEn;
        Assert.Equal(expectedDesc, renderedDesc);

        Assert.DoesNotContain("İYS ve KVKK uyumlu ticari", renderedDesc, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RenderedStructuredData_Homepage_OmitsDeferredSchemasAndDuplicateWebSite()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var jsonLdBlocks = ExtractAllJsonLdBlocks(html);
        var allJsonLd = string.Join("\n", jsonLdBlocks);

        Assert.DoesNotContain("SoftwareApplication", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("FAQPage", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Push Notifications", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("WhatsApp, SMS, Email, Push", allJsonLd, StringComparison.OrdinalIgnoreCase);

        // Asserts single global WebSite schema (#website) without duplicate page-level WebSite
        var websiteOccurrences = Regex.Matches(html, @"https://pika\.tr/#website").Count;
        Assert.Equal(1, websiteOccurrences);
    }

    [Theory]
    [InlineData("/cozumler/campaign-manager")]
    [InlineData("/en/solutions/campaign-manager")]
    public async Task RenderedStructuredData_CampaignManager_OmitsPushAttributionAndPunitiveClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var jsonLdBlocks = ExtractAllJsonLdBlocks(html);
        var allJsonLd = string.Join("\n", jsonLdBlocks);

        Assert.DoesNotContain("Push", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Mobile Push", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("directly attributed", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("doğrudan kampanyaya", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ceza riskinden korur", allJsonLd, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/urunler/ai-kampanya-asistani")]
    [InlineData("/en/products/ai-campaign-assistant")]
    public async Task RenderedStructuredData_AiCampaignAssistant_OmitsPushAndUnsupportedQualityClaims(string path)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var jsonLdBlocks = ExtractAllJsonLdBlocks(html);
        var allJsonLd = string.Join("\n", jsonLdBlocks);

        Assert.DoesNotContain("mobile push", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Mobil Push", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("fully compatible", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("tam uyumlu", allJsonLd, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("client-tested", allJsonLd, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task RenderedStructuredData_Opportunities_UsesCanonicalUrlAndOmitsLegacyFirsatlar()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/platform/gunun-firsatlari");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        var jsonLdBlocks = ExtractAllJsonLdBlocks(html);
        Assert.NotEmpty(jsonLdBlocks);
        var allJsonLd = string.Join("\n", jsonLdBlocks);

        Assert.Contains("https://pika.tr/platform/gunun-firsatlari", html);
        Assert.Contains("https://pika.tr/platform/gunun-firsatlari", allJsonLd);
        Assert.DoesNotContain("https://pika.tr/platform/firsatlar", html);
        Assert.DoesNotContain("https://pika.tr/platform/firsatlar", allJsonLd);
        Assert.DoesNotContain("/platform/firsatlar", allJsonLd);
    }

    [Fact]
    public void ViewTemplates_DoNotContainPageLevelJsonLdSections()
    {
        var webRoot = GetWebRoot();
        var viewsDir = Path.Combine(webRoot, "Views");
        var cshtmlFiles = Directory.GetFiles(viewsDir, "*.cshtml", SearchOption.AllDirectories);

        foreach (var file in cshtmlFiles)
        {
            var content = File.ReadAllText(file);
            Assert.False(Regex.IsMatch(content, @"@section\s+JsonLd\b", RegexOptions.IgnoreCase),
                $"File '{file}' unexpectedly contains an @section JsonLd block.");
        }
    }
}
