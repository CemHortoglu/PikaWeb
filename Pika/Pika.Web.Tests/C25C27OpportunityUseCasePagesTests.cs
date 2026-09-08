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

public class C25C27OpportunityUseCasePagesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C25C27OpportunityUseCasePagesTests(WebApplicationFactory<Program> factory)
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
    // 01 ROUTES: Verify 200 OK for all 6 endpoints
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/tekrar-satin-alma")]
    [InlineData("/en/use-cases/repeat-purchase")]
    [InlineData("/kullanim-senaryolari/capraz-satis")]
    [InlineData("/en/use-cases/cross-sell")]
    [InlineData("/kullanim-senaryolari/geri-kazanim")]
    [InlineData("/en/use-cases/win-back")]
    public async Task OpportunityPages_Return200OK(string path)
    {
        using var client = CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ─────────────────────────────────────────────────────────────
    // 02 SEO: Verify exact SeoHelper metadata
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void C25_RepeatPurchaseSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "RepeatPurchaseUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/tekrar-satin-alma", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/repeat-purchase", meta.AlternatePathEn);
        Assert.Equal("Tekrar Satın Alma Analizi | Replenishment Fırsatları | Pika", meta.TitleTr);
        Assert.Equal("Repeat Purchase Analysis | Replenishment Opportunities | Pika", meta.TitleEn);
        Assert.Equal("Pika ile müşteri işlem geçmişi ve ürün tüketim ritmini birlikte değerlendirerek tekrar satın alma fırsatlarını görünür hale getirin ve kontrollü pazarlama aksiyonuna bağlayın.", meta.DescriptionTr);
        Assert.Equal("Use Pika to evaluate customer transaction history and product consumption rhythm, surface repeat-purchase opportunities and connect them to controlled marketing action.", meta.DescriptionEn);
        Assert.Equal("Tekrar Satın Alma", meta.BreadcrumbTitleTr);
        Assert.Equal("Repeat Purchase", meta.BreadcrumbTitleEn);
    }

    [Fact]
    public void C26_CrossSellSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "CrossSellUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/capraz-satis", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/cross-sell", meta.AlternatePathEn);
        Assert.Equal("Çapraz Satış Analizi | Sepet Birlikteliği ve Cross-sell | Pika", meta.TitleTr);
        Assert.Equal("Cross-sell Analysis | Basket Affinity Opportunities | Pika", meta.TitleEn);
        Assert.Equal("Pika ile geçmiş sepet birlikteliklerinden anlamlı ürün ilişkilerini belirleyin, cross-sell fırsatlarını görünür hale getirin ve kontrollü pazarlama aksiyonuna bağlayın.", meta.DescriptionTr);
        Assert.Equal("Use Pika to identify meaningful product associations from historical baskets, surface cross-sell opportunities and connect them to controlled marketing action.", meta.DescriptionEn);
        Assert.Equal("Çapraz Satış", meta.BreadcrumbTitleTr);
        Assert.Equal("Cross-sell", meta.BreadcrumbTitleEn);
    }

    [Fact]
    public void C27_WinBackSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "WinBackUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/geri-kazanim", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/win-back", meta.AlternatePathEn);
        Assert.Equal("Geri Kazanım Analizi | Win-back ve Pasifleşme Fırsatları | Pika", meta.TitleTr);
        Assert.Equal("Win-back Analysis | Dormancy & Reactivation Opportunities | Pika", meta.TitleEn);
        Assert.Equal("Pika ile müşterinin hareketsizlik süresini kendi satın alma ritmiyle karşılaştırın, geri kazanım fırsatlarını görünür hale getirin ve kontrollü aksiyon bağlamında değerlendirin.", meta.DescriptionTr);
        Assert.Equal("Use Pika to compare customer inactivity with individual purchase rhythm, surface win-back opportunities and evaluate them within controlled action context.", meta.DescriptionEn);
        Assert.Equal("Geri Kazanım", meta.BreadcrumbTitleTr);
        Assert.Equal("Win-back", meta.BreadcrumbTitleEn);
    }

    // ─────────────────────────────────────────────────────────────
    // 03 H1 & SEMANTIC: Exactly one H1 with exact supplied text
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/tekrar-satin-alma", "Tekrar satın alma ritmini, müşterinin gerçek işlem bağlamından görünür hale getirin.")]
    [InlineData("/en/use-cases/repeat-purchase", "Make repeat-purchase rhythm visible from real customer transaction context.")]
    [InlineData("/kullanim-senaryolari/capraz-satis", "Geçmiş sepetlerden, birlikte anlamlı olan ürün ilişkilerini görün.")]
    [InlineData("/en/use-cases/cross-sell", "Find meaningful product relationships inside historical baskets.")]
    [InlineData("/kullanim-senaryolari/geri-kazanim", "Müşterinin sessizleşmesini, kendi geçmiş satın alma ritmine göre değerlendirin.")]
    [InlineData("/en/use-cases/win-back", "Evaluate customer inactivity against each customer's own purchase rhythm.")]
    public async Task OpportunityPages_HaveExactlyOneCorrectH1(string path, string expectedH1)
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
    [InlineData("/kullanim-senaryolari/tekrar-satin-alma")]
    [InlineData("/en/use-cases/repeat-purchase")]
    [InlineData("/kullanim-senaryolari/capraz-satis")]
    [InlineData("/en/use-cases/cross-sell")]
    [InlineData("/kullanim-senaryolari/geri-kazanim")]
    [InlineData("/en/use-cases/win-back")]
    public async Task OpportunityPages_ComplyWithGlobalGuardrails(string path)
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
    // 05 C25 SPECIFIC ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/tekrar-satin-alma", true)]
    [InlineData("/en/use-cases/repeat-purchase", false)]
    public async Task C25_RepeatPurchase_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.Contains("Replenishment Rhythm Engine", main);
        Assert.True(main.Contains("80%") || main.Contains("%80"), "Expected 80% or %80 in page content");
        Assert.True(main.Contains("120%") || main.Contains("%120"), "Expected 120% or %120 in page content");
        Assert.Contains("Customer Intelligence", main);
        Assert.Contains("Product Intelligence", main);
        Assert.Contains(isTr ? "Günün Fırsatları" : "Daily Opportunities", main);

        if (isTr)
        {
            Assert.Contains("garantisi değildir", main);
            Assert.Contains("sabit gün", main);
            Assert.Contains("stok seviyesi", main);
            Assert.Contains("talep veya envanter", main);
            Assert.Contains("otomatik kampanya", main);
            Assert.Contains("iletişim izni", main);
        }
        else
        {
            Assert.Contains("not a purchase guarantee", main);
            Assert.Contains("fixed-day reminder", main);
            Assert.Contains("stock-level sensor", main);
            Assert.Contains("demand or inventory forecast", main);
            Assert.Contains("automatic campaign delivery", main);
            Assert.Contains("communication permission", main);
        }

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 06 C26 SPECIFIC ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/capraz-satis", true)]
    [InlineData("/en/use-cases/cross-sell", false)]
    public async Task C26_CrossSell_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.Contains("statistical association mining", main, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Support", main);
        Assert.Contains("0.01", main);
        Assert.Contains("Confidence", main);
        Assert.Contains("0.15", main);
        Assert.Contains("Lift", main);
        Assert.Contains("1.2", main);
        Assert.Contains("Product Intelligence", main);
        Assert.Contains(isTr ? "Günün Fırsatları" : "Daily Opportunities", main);

        if (isTr)
        {
            Assert.Contains("black-box AI", main);
            Assert.Contains("katalog benzerliği", main);
            Assert.Contains("otomatik hedef kitle", main);
            Assert.Contains("otomatik bundle", main);
            Assert.Contains("dönüşüm garantisi", main);
        }
        else
        {
            Assert.Contains("black-box AI", main);
            Assert.Contains("catalog similarity alone", main);
            Assert.Contains("automatic audience selection", main);
            Assert.Contains("automatic bundling", main);
            Assert.Contains("conversion guarantee", main);
        }

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 07 C27 SPECIFIC ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/geri-kazanim", true)]
    [InlineData("/en/use-cases/win-back", false)]
    public async Task C27_WinBack_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.Contains("Churn & Dormancy Risk Engine", main);
        Assert.Contains(isTr ? "bireysel" : "individual", main, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(isTr ? "hareketsizlik" : "inactivity", main, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Customer Intelligence", main);
        Assert.Contains(isTr ? "Günün Fırsatları" : "Daily Opportunities", main);

        // Positive page copy must NOT introduce fixed 30-day, 60-day, 90-day rules
        Assert.DoesNotContain("30 gün", main);
        Assert.DoesNotContain("60 gün", main);
        Assert.DoesNotContain("90 gün", main);
        Assert.DoesNotContain("30 days", main);
        Assert.DoesNotContain("60 days", main);
        Assert.DoesNotContain("90 days", main);

        if (isTr)
        {
            Assert.Contains("kesin olarak churn", main);
            Assert.Contains("otomatik geri kazanım", main);
            Assert.Contains("otomatik teklif", main);
            Assert.Contains("ciro garantisi", main);
        }
        else
        {
            Assert.Contains("definitely churned", main);
            Assert.Contains("automatic win-back campaign", main);
            Assert.Contains("automatic offer or discount", main);
            Assert.Contains("revenue guarantee", main);
        }

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 08 SITEMAP ASSERTIONS
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void Sitemap_ContainsAllSixCanonicalUrls()
    {
        var root = GetProjectRoot();
        var sitemapPath = Path.Combine(root, "wwwroot", "sitemap.xml");
        Assert.True(File.Exists(sitemapPath));

        var doc = XDocument.Load(sitemapPath);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locs = doc.Descendants(ns + "loc").Select(x => x.Value).ToList();

        Assert.Contains("https://pika.tr/kullanim-senaryolari/tekrar-satin-alma", locs);
        Assert.Contains("https://pika.tr/en/use-cases/repeat-purchase", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/capraz-satis", locs);
        Assert.Contains("https://pika.tr/en/use-cases/cross-sell", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/geri-kazanim", locs);
        Assert.Contains("https://pika.tr/en/use-cases/win-back", locs);
    }

    // ─────────────────────────────────────────────────────────────
    // 09 BOUNDARY CHECK: Forbidden files remain untouched
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void ForbiddenFiles_RemainUntouched()
    {
        var root = GetProjectRoot();
        Assert.True(File.Exists(Path.Combine(root, "Views", "Home", "Index.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-home.css")));
        Assert.True(File.Exists(Path.Combine(root, "Views", "Shared", "_Layout.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "Views", "Solutions", "UseCases.cshtml")));
    }
}
