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

public class C30C31OrchestrationRetailUseCasePagesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C30C31OrchestrationRetailUseCasePagesTests(WebApplicationFactory<Program> factory)
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
    [InlineData("/kullanim-senaryolari/omnichannel-orkestrasyon")]
    [InlineData("/en/use-cases/omnichannel-orchestration")]
    [InlineData("/kullanim-senaryolari/perakende-e-ticaret")]
    [InlineData("/en/use-cases/retail-ecommerce")]
    public async Task OrchestrationAndRetailPages_Return200OK(string path)
    {
        using var client = CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ─────────────────────────────────────────────────────────────
    // 02 SEO: Verify exact SeoHelper metadata
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void C30_OmnichannelOrchestrationSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "OmnichannelOrchestrationUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/omnichannel-orkestrasyon", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/omnichannel-orchestration", meta.AlternatePathEn);
        Assert.Equal("Omnichannel Orkestrasyon | Email, SMS ve WhatsApp | Pika", meta.TitleTr);
        Assert.Equal("Omnichannel Orchestration | Email, SMS & WhatsApp | Pika", meta.TitleEn);
        Assert.Equal("Pika ile Audience, Campaign ve Journey bağlamını Email, SMS ve WhatsApp kanallarıyla; izin, gönderim kontrolü ve ölçüm katmanları içinde kontrollü biçimde yönetin.", meta.DescriptionTr);
        Assert.Equal("Use Pika to coordinate Audience, Campaign and Journey context across Email, SMS and WhatsApp within controlled permission, delivery and measurement layers.", meta.DescriptionEn);
        Assert.Equal("Omnichannel Orkestrasyon", meta.BreadcrumbTitleTr);
        Assert.Equal("Omnichannel Orchestration", meta.BreadcrumbTitleEn);
    }

    [Fact]
    public void C31_RetailEcommerceSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "RetailEcommerceUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/perakende-e-ticaret", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/retail-ecommerce", meta.AlternatePathEn);
        Assert.Equal("Perakende ve E-ticaret Müşteri Zekâsı | Pika", meta.TitleTr);
        Assert.Equal("Retail & E-commerce Customer Intelligence | Pika", meta.TitleEn);
        Assert.Equal("Pika ile perakende ve e-ticaret müşteri, ürün ve işlem verisini anlamlandırın; tekrar satın alma, çapraz satış, geri kazanım, segmentasyon ve kontrollü omnichannel aksiyon fırsatlarını değerlendirin.", meta.DescriptionTr);
        Assert.Equal("Use Pika to connect retail and e-commerce customer, product and transaction data to repeat purchase, cross-sell, win-back, segmentation and controlled omnichannel action.", meta.DescriptionEn);
        Assert.Equal("Perakende ve E-ticaret", meta.BreadcrumbTitleTr);
        Assert.Equal("Retail & E-commerce", meta.BreadcrumbTitleEn);
    }

    // ─────────────────────────────────────────────────────────────
    // 03 H1 & SEMANTIC: Exactly one H1 with exact supplied text
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/omnichannel-orkestrasyon", "Email, SMS ve WhatsApp'ı tek bir karar zincirinin kontrollü aksiyon kanalları olarak yönetin.")]
    [InlineData("/en/use-cases/omnichannel-orchestration", "Manage Email, SMS and WhatsApp as controlled action channels inside one decision chain.")]
    [InlineData("/kullanim-senaryolari/perakende-e-ticaret", "Müşteri, ürün ve işlem verisini aynı ticari karar bağlamında bir araya getirin.")]
    [InlineData("/en/use-cases/retail-ecommerce", "Bring customer, product and transaction data into one commercial decision context.")]
    public async Task OrchestrationAndRetailPages_HaveExactlyOneCorrectH1(string path, string expectedH1)
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
    [InlineData("/kullanim-senaryolari/omnichannel-orkestrasyon")]
    [InlineData("/en/use-cases/omnichannel-orchestration")]
    [InlineData("/kullanim-senaryolari/perakende-e-ticaret")]
    [InlineData("/en/use-cases/retail-ecommerce")]
    public async Task OrchestrationAndRetailPages_ComplyWithGlobalGuardrails(string path)
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
    // 05 C30 SPECIFIC ASSERTIONS: Omnichannel Orchestration
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/omnichannel-orkestrasyon", true)]
    [InlineData("/en/use-cases/omnichannel-orchestration", false)]
    public async Task C30_OmnichannelOrchestration_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        // Core confirmed channels
        Assert.Contains("Email", main);
        Assert.Contains("SMS", main);
        Assert.Contains("WhatsApp", main);

        // Core layers
        Assert.Contains("Audience Manager", main);
        Assert.Contains("Campaign Manager", main);
        Assert.Contains("Journey Manager", main);
        Assert.Contains("Consent Management", main);

        // No positive push capability claims
        Assert.DoesNotMatch(@"\bPush\s+(?:notification|bildirimi|kanalı|channel)\b", main);

        if (isTr)
        {
            Assert.Contains("Kanal orchestration, hedef kitleyi veya iletişim iznini kendi başına belirlemez", main);
            Assert.Contains("Önce aksiyon bağlamını kurun. Sonra kanalı yönetin", main);
            Assert.Contains("Omnichannel ≠ herkese her kanaldan mesaj", main);
            Assert.Contains("Omnichannel ≠ AI'ın otomatik kanal seçmesi", main);
            Assert.Contains("Omnichannel ≠ otonom audience seçimi", main);
            Assert.Contains("Omnichannel ≠ consent bypass", main);
            Assert.Contains("Omnichannel ≠ Push capability", main);
            Assert.Contains("Omnichannel ≠ delivery, conversion veya revenue garantisi", main);
        }
        else
        {
            Assert.Contains("Channel orchestration does not independently determine the audience or communication permission", main);
            Assert.Contains("Build the action context first. Then manage the channel", main);
            Assert.Contains("Omnichannel ≠ messaging everyone on every channel", main);
            Assert.Contains("Omnichannel ≠ automatic AI channel selection", main);
            Assert.Contains("Omnichannel ≠ autonomous audience selection", main);
            Assert.Contains("Omnichannel ≠ consent bypass", main);
            Assert.Contains("Omnichannel ≠ Push capability", main);
            Assert.Contains("Omnichannel ≠ delivery, conversion or revenue guarantee", main);
        }

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 06 C31 SPECIFIC ASSERTIONS: Retail & E-commerce
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/perakende-e-ticaret", true)]
    [InlineData("/en/use-cases/retail-ecommerce", false)]
    public async Task C31_RetailEcommerce_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        // Core entities and layers
        Assert.Contains("Customer Intelligence", main);
        Assert.Contains("Product Intelligence", main);
        Assert.Contains("Customer Value", main);
        Assert.Contains("REST", main);
        Assert.Contains("Excel", main);
        Assert.Contains("CSV", main);

        // Explicit negative boundaries
        if (isTr)
        {
            Assert.Contains("Pika bir e-ticaret platformu, POS, ERP veya sipariş yönetim sistemi değildir", main);
            Assert.Contains("Pika ≠ e-ticaret altyapısı", main);
            Assert.Contains("Pika ≠ POS veya ERP", main);
            Assert.Contains("Pika ≠ sipariş yönetim sistemi", main);
            Assert.Contains("Pika ≠ stok veya fiyat optimizasyon motoru", main);
            Assert.Contains("Pika ≠ her commerce sistemi için hazır native connector garantisi", main);
            Assert.Contains("Pika ≠ satış, conversion veya revenue garantisi", main);
        }
        else
        {
            Assert.Contains("Pika is not an e-commerce platform, POS, ERP or order-management system", main);
            Assert.Contains("Pika ≠ e-commerce platform", main);
            Assert.Contains("Pika ≠ POS or ERP", main);
            Assert.Contains("Pika ≠ order-management system", main);
            Assert.Contains("Pika ≠ inventory or price-optimization engine", main);
            Assert.Contains("Pika ≠ guaranteed native connector for every commerce system", main);
            Assert.Contains("Pika ≠ sales, conversion or revenue guarantee", main);
        }

        // Must not contain unverified commerce native platform connector claims
        Assert.DoesNotContain("Shopify connector", main, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Magento connector", main, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("WooCommerce connector", main, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Trendyol connector", main, StringComparison.OrdinalIgnoreCase);

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

        Assert.Contains("https://pika.tr/kullanim-senaryolari/omnichannel-orkestrasyon", locs);
        Assert.Contains("https://pika.tr/en/use-cases/omnichannel-orchestration", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/perakende-e-ticaret", locs);
        Assert.Contains("https://pika.tr/en/use-cases/retail-ecommerce", locs);
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
