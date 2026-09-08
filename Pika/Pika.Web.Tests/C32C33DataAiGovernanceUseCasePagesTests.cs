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

public class C32C33DataAiGovernanceUseCasePagesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C32C33DataAiGovernanceUseCasePagesTests(WebApplicationFactory<Program> factory)
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
    [InlineData("/kullanim-senaryolari/veri-aktarimi")]
    [InlineData("/en/use-cases/data-onboarding")]
    [InlineData("/kullanim-senaryolari/pazarlama-ai-yonetisimi")]
    [InlineData("/en/use-cases/marketing-ai-governance")]
    public async Task DataAndAiGovernancePages_Return200OK(string path)
    {
        using var client = CreateClient();
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ─────────────────────────────────────────────────────────────
    // 02 SEO: Verify exact SeoHelper metadata
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void C32_DataOnboardingSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "DataOnboardingUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/veri-aktarimi", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/data-onboarding", meta.AlternatePathEn);
        Assert.Equal("Veri Aktarımı ve Onboarding | REST API, Excel ve CSV | Pika", meta.TitleTr);
        Assert.Equal("Data Onboarding | REST API, Excel & CSV | Pika", meta.TitleEn);
        Assert.Equal("Müşteri, ürün ve işlem verisini Pika'ya Excel, CSV veya asenkron REST Ingestion API ile taşıyın; veri girişini sonraki Customer Intelligence ve Product Intelligence katmanlarına hazırlayın.", meta.DescriptionTr);
        Assert.Equal("Bring customer, product and transaction data into Pika through Excel, CSV or the asynchronous REST Ingestion API and prepare it for downstream Customer Intelligence and Product Intelligence.", meta.DescriptionEn);
        Assert.Equal("Veri Aktarımı", meta.BreadcrumbTitleTr);
        Assert.Equal("Data Onboarding", meta.BreadcrumbTitleEn);
    }

    [Fact]
    public void C33_MarketingAiGovernanceSeoMetadata_HasExactValues()
    {
        var meta = SeoHelper.GetMetadata("Solutions", "MarketingAiGovernanceUseCase");
        Assert.NotNull(meta);
        Assert.Equal("/kullanim-senaryolari/pazarlama-ai-yonetisimi", meta.AlternatePathTr);
        Assert.Equal("/en/use-cases/marketing-ai-governance", meta.AlternatePathEn);
        Assert.Equal("Pazarlamada AI Yönetişimi | İnsan Kontrollü Pika Pilot | Pika", meta.TitleTr);
        Assert.Equal("Marketing AI Governance | Human-Controlled Pika Pilot | Pika", meta.TitleEn);
        Assert.Equal("Pika Pilot'ın hesaplanmış müşteri, ürün ve fırsat bağlamını AI destekli kampanya taslaklarında nasıl kullandığını; kaynak gerçek, insan kararı ve kontrollü execution sınırlarıyla değerlendirin.", meta.DescriptionTr);
        Assert.Equal("Evaluate how Pika Pilot uses calculated customer, product and opportunity context for AI-assisted campaign drafts while preserving source truth, human decision and controlled execution boundaries.", meta.DescriptionEn);
        Assert.Equal("Pazarlamada AI Yönetişimi", meta.BreadcrumbTitleTr);
        Assert.Equal("Marketing AI Governance", meta.BreadcrumbTitleEn);
    }

    // ─────────────────────────────────────────────────────────────
    // 03 H1 & SEMANTIC: Exactly one H1 with exact supplied text
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/veri-aktarimi", "Müşteri, ürün ve işlem verisini Pika'nın karar zincirine kontrollü biçimde taşıyın.")]
    [InlineData("/en/use-cases/data-onboarding", "Bring customer, product and transaction data into Pika's decision chain under control.")]
    [InlineData("/kullanim-senaryolari/pazarlama-ai-yonetisimi", "AI'ı kaynak gerçeğin ve insan kararının yerine koymadan kullanın.")]
    [InlineData("/en/use-cases/marketing-ai-governance", "Use AI without replacing source truth or human decision.")]
    public async Task DataAndAiGovernancePages_HaveExactlyOneCorrectH1(string path, string expectedH1)
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
    [InlineData("/kullanim-senaryolari/veri-aktarimi")]
    [InlineData("/en/use-cases/data-onboarding")]
    [InlineData("/kullanim-senaryolari/pazarlama-ai-yonetisimi")]
    [InlineData("/en/use-cases/marketing-ai-governance")]
    public async Task DataAndAiGovernancePages_ComplyWithGlobalGuardrails(string path)
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
    // 05 C32 SPECIFIC ASSERTIONS: Data Onboarding
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/veri-aktarimi", true)]
    [InlineData("/en/use-cases/data-onboarding", false)]
    public async Task C32_DataOnboarding_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        // Core data domains and ingestion mechanisms
        Assert.Contains("Excel", main);
        Assert.Contains(".xlsx", main);
        Assert.Contains("CSV", main);
        Assert.Contains("REST", main);
        Assert.Contains("/api/v1/ingest/", main);
        Assert.Contains("202 Accepted", main);
        Assert.Contains("X-Idempotency-Key", main);
        Assert.Contains("Customer Intelligence", main);
        Assert.Contains("Product Intelligence", main);

        if (isTr)
        {
            Assert.Contains("müşteri, ürün ve işlem verisi", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("asenkron", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("column-mapping", main);
            Assert.Contains("Entegrasyon katmanı müşteriyi analiz etmez", main);
            Assert.Contains("Data Onboarding ≠ her sistem için hazır native connector", main);
            Assert.Contains("Data Onboarding ≠ real-time streaming garantisi", main);
            Assert.Contains("Data Onboarding ≠ ERP veya CRM yerine geçen ürün", main);
            Assert.Contains("Data Onboarding ≠ otomatik intelligence sonucu", main);
            Assert.Contains("Data Onboarding ≠ otomatik campaign kararı", main);
        }
        else
        {
            Assert.Contains("customer, product and transaction", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("asynchronous", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("column-mapping", main);
            Assert.Contains("The integration layer does not analyze the customer", main);
            Assert.Contains("Data Onboarding ≠ ready-made native connector for every system", main);
            Assert.Contains("Data Onboarding ≠ real-time streaming guarantee", main);
            Assert.Contains("Data Onboarding ≠ replacement ERP or CRM", main);
            Assert.Contains("Data Onboarding ≠ automatic intelligence result", main);
            Assert.Contains("Data Onboarding ≠ automatic campaign decision", main);
        }

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 06 C33 SPECIFIC ASSERTIONS: Marketing AI Governance
    // ─────────────────────────────────────────────────────────────
    [Theory]
    [InlineData("/kullanim-senaryolari/pazarlama-ai-yonetisimi", true)]
    [InlineData("/en/use-cases/marketing-ai-governance", false)]
    public async Task C33_MarketingAiGovernance_EstablishesCoreArchitectureAndFaq(string path, bool isTr)
    {
        using var client = CreateClient();
        var html = await client.GetStringAsync(path);
        var main = ExtractMain(html);

        Assert.Contains("Pika Pilot", main);
        Assert.Contains("Customer Intelligence", main);
        Assert.Contains("Product Intelligence", main);
        Assert.Contains("Audience Manager", main);
        Assert.Contains("Campaign Manager", main);
        Assert.Contains("Journey Manager", main);
        Assert.Contains("Consent Management", main);

        if (isTr)
        {
            Assert.Contains("hesaplanmış müşteri, ürün ve fırsat bağlamı", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AI destekli taslak", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("son karar kullanıcıda kalır", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Pika Pilot ≠ müşteri intelligence motoru", main);
            Assert.Contains("Pika Pilot ≠ ticari fırsatın kaynak gerçeği", main);
            Assert.Contains("Pika Pilot ≠ autonomous marketing agent", main);
            Assert.Contains("Pika Pilot ≠ bağımsız campaign sender", main);
            Assert.Contains("Pika Pilot ≠ genel amaçlı enterprise AI governance platformu", main);
            Assert.Contains("Dahili ürün dokümantasyonu, doğrudan kişisel tanımlayıcıların LLM prompt bağlamından hariç tutulmasının hedeflendiği anonymized-parameter AI sınırını tarif eder; C33 bunu mutlak bir Zero-PII garantisi olarak sunmaz.", main);
        }
        else
        {
            Assert.Contains("calculated customer, product and opportunity context", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AI-assisted draft", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("final decision remains with the user", main, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Pika Pilot ≠ customer-intelligence engine", main);
            Assert.Contains("Pika Pilot ≠ source truth for the commercial opportunity", main);
            Assert.Contains("Pika Pilot ≠ autonomous marketing agent", main);
            Assert.Contains("Pika Pilot ≠ independent campaign sender", main);
            Assert.Contains("Pika Pilot ≠ general-purpose enterprise AI-governance platform", main);
            Assert.Contains("Internal product documentation describes an anonymized-parameter AI boundary where direct personal identifiers are intended to be excluded from LLM prompt context; C33 does not present this as an absolute Zero-PII guarantee.", main);
        }

        // Negative absolute claims forbidden
        Assert.DoesNotMatch(new Regex(@"\b100%\s+anonymized\b", RegexOptions.IgnoreCase), main);
        Assert.DoesNotMatch(new Regex(@"\bno\s+PII\s+ever\b", RegexOptions.IgnoreCase), main);
        Assert.DoesNotMatch(new Regex(@"\bGDPR\s+compliant\s+AI\b", RegexOptions.IgnoreCase), main);
        Assert.DoesNotMatch(new Regex(@"\bKVKK\s+compliant\s+AI\b", RegexOptions.IgnoreCase), main);

        var faqCount = Regex.Matches(main, @"<details\b", RegexOptions.IgnoreCase).Count;
        Assert.Equal(6, faqCount);
    }

    // ─────────────────────────────────────────────────────────────
    // 07 SITEMAP ASSERTIONS: All C25-C33 Growth URLs
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void Sitemap_ContainsAllGrowthCanonicalUrls()
    {
        var root = GetProjectRoot();
        var sitemapPath = Path.Combine(root, "wwwroot", "sitemap.xml");
        Assert.True(File.Exists(sitemapPath));

        var doc = XDocument.Load(sitemapPath);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locs = doc.Descendants(ns + "loc").Select(x => x.Value).ToList();

        // C25-C33 TR URLs
        Assert.Contains("https://pika.tr/kullanim-senaryolari/tekrar-satin-alma", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/capraz-satis", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/geri-kazanim", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/musteri-segmentasyonu", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/musteri-degeri", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/omnichannel-orkestrasyon", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/perakende-e-ticaret", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/veri-aktarimi", locs);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/pazarlama-ai-yonetisimi", locs);

        // C25-C33 EN URLs
        Assert.Contains("https://pika.tr/en/use-cases/repeat-purchase", locs);
        Assert.Contains("https://pika.tr/en/use-cases/cross-sell", locs);
        Assert.Contains("https://pika.tr/en/use-cases/win-back", locs);
        Assert.Contains("https://pika.tr/en/use-cases/customer-segmentation", locs);
        Assert.Contains("https://pika.tr/en/use-cases/customer-value", locs);
        Assert.Contains("https://pika.tr/en/use-cases/omnichannel-orchestration", locs);
        Assert.Contains("https://pika.tr/en/use-cases/retail-ecommerce", locs);
        Assert.Contains("https://pika.tr/en/use-cases/data-onboarding", locs);
        Assert.Contains("https://pika.tr/en/use-cases/marketing-ai-governance", locs);
    }

    // ─────────────────────────────────────────────────────────────
    // 08 LLMS CONVERGENCE TESTS
    // ─────────────────────────────────────────────────────────────
    [Fact]
    public void LlmsTxt_ContainsAllNineTrGrowthUrlsAndRequiredContext()
    {
        var root = GetProjectRoot();
        var llmsPath = Path.Combine(root, "wwwroot", "llms.txt");
        Assert.True(File.Exists(llmsPath));

        var text = File.ReadAllText(llmsPath);

        // All 9 TR URLs
        Assert.Contains("https://pika.tr/kullanim-senaryolari/tekrar-satin-alma", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/capraz-satis", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/geri-kazanim", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/musteri-segmentasyonu", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/musteri-degeri", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/omnichannel-orkestrasyon", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/perakende-e-ticaret", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/veri-aktarimi", text);
        Assert.Contains("https://pika.tr/kullanim-senaryolari/pazarlama-ai-yonetisimi", text);

        // Key channels and boundaries
        Assert.Contains("Email", text);
        Assert.Contains("SMS", text);
        Assert.Contains("WhatsApp", text);
        Assert.Contains("Pika Pilot", text);
        Assert.Contains("nihai karar kullanıcıda kalır", text);

        // Positive Push absent
        Assert.DoesNotMatch(@"\bPush\b", text);
        Assert.DoesNotContain("Next Best Action", text);
    }

    [Fact]
    public void LlmsFullTxt_ContainsAllNineEnGrowthUrlsAndRequiredContext()
    {
        var root = GetProjectRoot();
        var llmsFullPath = Path.Combine(root, "wwwroot", "llms-full.txt");
        Assert.True(File.Exists(llmsFullPath));

        var text = File.ReadAllText(llmsFullPath);

        // All 9 EN URLs
        Assert.Contains("https://pika.tr/en/use-cases/repeat-purchase", text);
        Assert.Contains("https://pika.tr/en/use-cases/cross-sell", text);
        Assert.Contains("https://pika.tr/en/use-cases/win-back", text);
        Assert.Contains("https://pika.tr/en/use-cases/customer-segmentation", text);
        Assert.Contains("https://pika.tr/en/use-cases/customer-value", text);
        Assert.Contains("https://pika.tr/en/use-cases/omnichannel-orchestration", text);
        Assert.Contains("https://pika.tr/en/use-cases/retail-ecommerce", text);
        Assert.Contains("https://pika.tr/en/use-cases/data-onboarding", text);
        Assert.Contains("https://pika.tr/en/use-cases/marketing-ai-governance", text);

        // Careers text exact match
        Assert.Contains("General career-application page explaining Pika's product and problem context without publishing an unverified list of open positions.", text);

        // Contact text exact match
        Assert.Contains("General contact page for product, data, integration, commercial and collaboration inquiries. Structured product evaluation is handled through Demo Request.", text);

        // Corporate text exact match
        Assert.Contains("Corporate evaluation page covering Pika's product architecture, integrations, security, consent, AI and commercial-evaluation approach.", text);

        // Demo Request text exact match
        Assert.Contains("Structured product and usage-scope evaluation request for organizations evaluating Pika.", text);

        // Qualified AI privacy text
        Assert.Contains("Internal product documentation describes an anonymized-parameter AI boundary where direct personal identifiers are intended to be excluded from LLM prompt context; this is not presented as an absolute Zero-PII guarantee.", text);

        // Exclusions
        Assert.DoesNotContain("Career opportunities and engineering culture at Pika", text);
        Assert.DoesNotMatch(new Regex(@"\buniversal\s+connector\b", RegexOptions.IgnoreCase), text);
    }

    // ─────────────────────────────────────────────────────────────
    // 09 BOUNDARY CHECK: Frozen files remain untouched
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
