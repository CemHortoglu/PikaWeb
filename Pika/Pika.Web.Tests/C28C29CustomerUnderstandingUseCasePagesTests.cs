using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class C28C29CustomerUnderstandingUseCasePagesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C28C29CustomerUnderstandingUseCasePagesTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient() => _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static string NormalizeWhitespace(string input) => Regex.Replace(input, @"\s+", " ").Trim();

    private static string ExtractH1(string html)
    {
        var match = Regex.Match(html, @"<h1[^>]*>(.*?)</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (!match.Success) return string.Empty;
        var stripped = Regex.Replace(match.Groups[1].Value, @"<[^>]+>", " ");
        return NormalizeWhitespace(WebUtility.HtmlDecode(stripped));
    }

    private static string ExtractMain(string html)
    {
        var match = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var body = match.Success ? match.Groups[1].Value : html;
        var withoutScripts = Regex.Replace(body, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);
        return WebUtility.HtmlDecode(withoutScripts);
    }

    private static string GetProjectRoot()
    {
        var dir = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(dir))
        {
            if (File.Exists(Path.Combine(dir, "Pika.csproj"))) return dir;
            var parent = Directory.GetParent(dir);
            if (parent == null) break;
            dir = parent.FullName;
        }
        throw new DirectoryNotFoundException("Could not locate Pika project root directory.");
    }

    // ─────────────────────────────────────────────────────────────
    // 01 ROUTES: Verify 200 OK for all 4 endpoints
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/musteri-segmentasyonu")]
    [InlineData("/en/use-cases/customer-segmentation")]
    [InlineData("/kullanim-senaryolari/musteri-degeri")]
    [InlineData("/en/use-cases/customer-value")]
    public async Task CustomerUnderstandingPages_Return200OK(string path)
    {
        using var client = CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ─────────────────────────────────────────────────────────────
    // 02 SEO: Verify exact SeoHelper metadata
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void C28_CustomerSegmentationSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "CustomerSegmentationUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/musteri-segmentasyonu", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/customer-segmentation", meta.AlternatePathEn);
        Assert.Equal("Müşteri Segmentasyonu | Kural Tabanlı Hedef Kitle | Pika", meta.TitleTr);
        Assert.Equal("Customer Segmentation | Rule-Based Audiences | Pika", meta.TitleEn);
        Assert.Equal("Pika ile müşteri özellikleri, işlem geçmişi ve davranış bağlamını açık kurallarla birleştirerek yeniden değerlendirilebilir müşteri segmentleri oluşturun ve kontrollü aksiyona bağlayın.", meta.DescriptionTr);
        Assert.Equal("Use Pika to combine customer attributes, transaction history and behavioral context through explicit rules to create reusable customer segments for controlled action.", meta.DescriptionEn);
        Assert.Equal("Müşteri Segmentasyonu", meta.BreadcrumbTitleTr);
        Assert.Equal("Customer Segmentation", meta.BreadcrumbTitleEn);
    }

    [Fact]
    public void C29_CustomerValueSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "CustomerValueUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/musteri-degeri", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/customer-value", meta.AlternatePathEn);
        Assert.Equal("Müşteri Değeri Analizi | Customer Value Score | Pika", meta.TitleTr);
        Assert.Equal("Customer Value Analysis | Customer Value Score | Pika", meta.TitleEn);
        Assert.Equal("Pika Customer Intelligence ile Monetary, Frequency, Recency ve Loyalty bağlamını deterministic Customer Value Score içinde değerlendirerek müşteri değerini görünür hale getirin.", meta.DescriptionTr);
        Assert.Equal("Use Pika Customer Intelligence to evaluate Monetary, Frequency, Recency and Loyalty context through a deterministic Customer Value Score and make customer value visible.", meta.DescriptionEn);
        Assert.Equal("Müşteri Değeri", meta.BreadcrumbTitleTr);
        Assert.Equal("Customer Value", meta.BreadcrumbTitleEn);
    }

    // ─────────────────────────────────────────────────────────────
    // 03 H1 & SEMANTIC: Exactly one H1 with exact supplied text
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/musteri-segmentasyonu", "Müşterileri yalnızca listelemeyin. Hangi bağlamda hedef kitleye dahil olduklarını tanımlayın.")]
    [InlineData("/en/use-cases/customer-segmentation", "Do not only list customers. Define why they belong in an audience.")]
    [InlineData("/kullanim-senaryolari/musteri-degeri", "Müşteri değerini, tek bir harcama tutarından daha geniş bağlamda değerlendirin.")]
    [InlineData("/en/use-cases/customer-value", "Evaluate customer value through more than a single spend amount.")]
    public async Task CustomerUnderstandingPages_HaveExactlyOneCorrectH1(string path, string expectedH1)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);

        var h1Matches = Regex.Matches(html, @"<h1(?:>|\s)", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);

        var actualH1 = ExtractH1(html);
        Assert.Equal(expectedH1, actualH1);
    }

    // ─────────────────────────────────────────────────────────────
    // 04 GLOBAL GUARDRAILS: Zero img, zero wiki, zero push, zero JSON-LD
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/musteri-segmentasyonu")]
    [InlineData("/en/use-cases/customer-segmentation")]
    [InlineData("/kullanim-senaryolari/musteri-degeri")]
    [InlineData("/en/use-cases/customer-value")]
    public async Task CustomerUnderstandingPages_ComplyWithGlobalGuardrails(string path)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.DoesNotMatch(@"<img\b", main);
        Assert.DoesNotContain("/wiki/", main);
        Assert.DoesNotContain("kanallar/push", main);
        Assert.DoesNotContain("channels/push", main);
        Assert.DoesNotContain("FAQPage", html);
        Assert.DoesNotContain("SoftwareApplication", html);
        Assert.DoesNotContain("data-i18n", main);
    }

    // ─────────────────────────────────────────────────────────────
    // 05 C28 SPECIFIC ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/musteri-segmentasyonu", true)]
    [InlineData("/en/use-cases/customer-segmentation", false)]
    public async Task C28_CustomerSegmentation_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.Contains("Customer Intelligence", main);
        Assert.Contains("Audience Manager", main);
        Assert.Contains("AND / OR", main);

        if (isTr)
        {
            Assert.Contains("işlem geçmişi", main);
            Assert.Contains("davranış bağlamı", main);
            Assert.Contains("kural mantığı", main);
            Assert.Contains("Audience Manager müşteri davranışını hesaplayan intelligence motoru değildir", main);
            Assert.Contains("Statik liste", main);
            Assert.Contains("Kural tabanlı audience", main);
            Assert.Contains("iletişim izninin", main);
            Assert.Contains("generative AI müşteri seçimi", main);
            Assert.Contains("otonom kampanya kararı", main);
        }
        else
        {
            Assert.Contains("transaction history", main);
            Assert.Contains("behavioral context", main);
            Assert.Contains("rule logic", main);
            Assert.Contains("Audience Manager is not the intelligence engine that calculates customer behavior", main);
            Assert.Contains("static list", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("rule-based audience", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("communication permission", main);
            Assert.Contains("generative-AI customer selection", main);
            Assert.Contains("autonomous campaign decision", main);
        }

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 06 C29 SPECIFIC ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/musteri-degeri", true)]
    [InlineData("/en/use-cases/customer-value", false)]
    public async Task C29_CustomerValue_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.Contains("Customer Value Score", main);
        Assert.Contains("CVS", main);
        Assert.Contains("0–100", main);
        Assert.Contains("Monetary", main);
        Assert.Contains("Frequency", main);
        Assert.Contains("Recency", main);
        Assert.Contains("Loyalty", main);
        Assert.Contains("0.40", main);
        Assert.Contains("0.25", main);
        Assert.Contains("0.20", main);
        Assert.Contains("0.15", main);
        Assert.Contains("deterministic", main, StringComparison.OrdinalIgnoreCase);

        // Negative boundaries: asserts presence of explicit negative boundaries
        if (isTr)
        {
            Assert.Contains("gelecekteki CLV tahmini", main);
            Assert.Contains("gelecekteki revenue tahmini", main);
            Assert.Contains("AI müşteri puanlama", main);
            Assert.Contains("müşteri kârlılığı garantisi", main);
            Assert.Contains("otomatik audience seçimi", main);
            Assert.Contains("kampanya sonucu garantisi", main);
        }
        else
        {
            Assert.Contains("future CLV prediction", main);
            Assert.Contains("future revenue prediction", main);
            Assert.Contains("AI customer scoring", main);
            Assert.Contains("customer-profitability guarantee", main);
            Assert.Contains("automatic audience selection", main);
            Assert.Contains("campaign-outcome guarantee", main);
        }

        // No invented tier names
        Assert.DoesNotContain("Gold", main);
        Assert.DoesNotContain("Silver", main);
        Assert.DoesNotContain("Bronze", main);
        Assert.DoesNotContain("Champions", main);
        Assert.DoesNotContain("At Risk", main);
        Assert.DoesNotContain("Loyal Customers", main);

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 07 SITEMAP ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void Sitemap_ContainsAllFourCanonicalUrls()
    {
        var root = GetProjectRoot();
        var sitemapPath = Path.Combine(root, "wwwroot", "sitemap.xml");
        Assert.True(File.Exists(sitemapPath));

        var doc = XDocument.Load(sitemapPath);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locs = doc.Descendants(ns + "loc").Select(x => x.Value).ToList();

        Assert.Contains("https://pika.tr/kullanim-senaryolari/musteri-segmentasyonu", locs);
        Assert.Contains("https://pika.tr/en/use-cases/customer-segmentation", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/musteri-degeri", locs);
        Assert.Contains("https://pika.tr/en/use-cases/customer-value", locs);
    }

    // ─────────────────────────────────────────────────────────────
    // 08 BOUNDARY CHECK: Frozen files remain untouched
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void FrozenFiles_RemainUntouched()
    {
        var root = GetProjectRoot();
        Assert.True(File.Exists(Path.Combine(root, "Views", "Home", "Index.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-home.css")));
        Assert.True(File.Exists(Path.Combine(root, "Views", "Shared", "_Layout.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "Views", "Solutions", "UseCases.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "Views", "Platform", "CustomerIntelligence.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "Views", "Solutions", "AudienceManager.cshtml")));
    }
}
