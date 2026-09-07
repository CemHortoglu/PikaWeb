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

public class C12ConsentManagementPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C12ConsentManagementPageTests(WebApplicationFactory<Program> factory)
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

    private static string ExtractMainBodyWithoutScripts(string html)
    {
        var bodyMatch = Regex.Match(html, @"<main[^>]*>(.*?)</main>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        var pageBody = bodyMatch.Success ? bodyMatch.Groups[1].Value : html;
        return Regex.Replace(pageBody, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "", RegexOptions.IgnoreCase);
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

    // 01: GET /cozumler/consent-management = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/solutions/consent-management = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.ConsentManagement SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsConsentManagement_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "ConsentManagement");
        Assert.NotNull(entry);

        Assert.Equal("Consent Management | İYS, Opt-Out ve Tercih Yönetimi | Pika", entry.TitleTr);
        Assert.Equal("Consent Management | Consent, Opt-Out & Preference Management | Pika", entry.TitleEn);
        Assert.Equal("Pika Consent Management; gönderim öncesi IYS durum kontrolü, merkezi opt-out yönetimi ve Email/SMS tercih güncelleme akışıyla iletişim uygunluğunu kontrol etmeye yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Consent Management helps govern communication eligibility through pre-dispatch IYS status checks, centralized opt-out handling and an Email/SMS preference update flow.", entry.DescriptionEn);
        Assert.Equal("Consent Management", entry.BreadcrumbTitleTr);
        Assert.Equal("Consent Management", entry.BreadcrumbTitleEn);
    }

    [Fact]
    public async Task TurkishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Consent Management | İYS, Opt-Out ve Tercih Yönetimi | Pika", ExtractTitle(html));
        Assert.Equal("Pika Consent Management; gönderim öncesi IYS durum kontrolü, merkezi opt-out yönetimi ve Email/SMS tercih güncelleme akışıyla iletişim uygunluğunu kontrol etmeye yardımcı olur.", ExtractMetaDescription(html));
    }

    [Fact]
    public async Task EnglishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Consent Management | Consent, Opt-Out & Preference Management | Pika", ExtractTitle(html));
        Assert.Equal("Pika Consent Management helps govern communication eligibility through pre-dispatch IYS status checks, centralized opt-out handling and an Email/SMS preference update flow.", ExtractMetaDescription(html));
    }

    // 04: Exactly one H1
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_HaveExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var matches = Regex.Matches(html, @"<h1[^>]*>.*?</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Single(matches);
    }

    // 05: TR normalized H1: İzin durumunu, gönderim kararının doğal parçası haline getirin.
    [Fact]
    public async Task TurkishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("İzin durumunu, gönderim kararının doğal parçası haline getirin.", ExtractH1(html));
    }

    // 06: EN normalized H1: Make consent state a natural part of the delivery decision.
    [Fact]
    public async Task EnglishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Make consent state a natural part of the delivery decision.", ExtractH1(html));
    }

    // 07: TR contains: Consent Management nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Consent Management nedir?", decoded);
    }

    // 08: EN contains: What is Consent Management?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Consent Management?", decoded);
    }

    // 09: Both contain: IYS, Opt-Out, Audience Manager, Campaign Manager, Journey Manager
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainCanonicalEcosystemEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("IYS", decoded);
        Assert.Contains("Opt-Out", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
    }

    // 10: Both contain: Email, SMS, WhatsApp
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainCanonicalChannels(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Email", decoded);
        Assert.Contains("SMS", decoded);
        Assert.Contains("WhatsApp", decoded);
    }

    // 11: Page explicitly states public preference flow is: Email + SMS only
    [Fact]
    public async Task TurkishPage_StatesPublicPreferenceIsEmailAndSmsOnly()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Bu public akış yalnızca Email ve SMS tercih alanlarını kapsar", decoded);
        Assert.Contains("EMAIL + SMS", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesPublicPreferenceIsEmailAndSmsOnly()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("That public flow contains Email and SMS preference fields only", decoded);
        Assert.Contains("EMAIL + SMS", decoded);
    }

    // 12: Page explicitly states WhatsApp is NOT managed by same public form
    [Fact]
    public async Task TurkishPage_StatesWhatsAppNotManagedByPublicForm()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("WhatsApp opt-in bilgisi aynı Email/SMS public formundan yönetilmez", decoded);
        Assert.Contains("WhatsApp izin yönetim ekranı değildir", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesWhatsAppNotManagedByPublicForm()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("WhatsApp opt-in is not managed through the same Email/SMS public form", decoded);
        Assert.Contains("is not the WhatsApp consent-management screen", decoded);
    }

    // 13: Page states WhatsApp opt-in context comes from: webhook or import flows
    [Fact]
    public async Task TurkishPage_StatesWhatsAppOptInComesFromWebhookOrImport()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("ayrı webhook veya import akışlarından gelen bağlam kullanılır", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesWhatsAppOptInComesFromWebhookOrImport()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("context comes through separate webhook or import flows", decoded);
    }

    // 14: Page states Push consent does not exist
    [Fact]
    public async Task TurkishPage_StatesPushConsentDoesNotExist()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Push consent mevcut değildir", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesPushConsentDoesNotExist()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Push consent does not exist", decoded);
    }

    // 15: Zero positive Push capability claims
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainZeroPositivePushClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push: ON", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Push preference", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Push consent management supported", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("manage Push consent", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Email SMS WhatsApp Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SMS, Email, WhatsApp ve Push için ayrı izin yönetimi", body, StringComparison.OrdinalIgnoreCase);
    }

    // 16: View contains no: ViewData["Title"], ViewData["MetaDescription"], ViewData["MetaKeywords"], ViewData["CanonicalUrl"], @section JsonLd, /wiki/, <img
    [Fact]
    public void ViewFile_ContainsNoForbiddenDirectives()
    {
        var projectRoot = GetProjectRoot();
        var viewPath = Path.Combine(projectRoot, "Views", "Solutions", "ConsentManagement.cshtml");
        Assert.True(File.Exists(viewPath), $"View file not found at {viewPath}");

        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("wikiBase", content);
        Assert.DoesNotContain("<img", content);
    }

    // 17: No fake preference dashboard: pika-sol-showcase-mock-bar, w80, w60, w70, w45, Email: ON, SMS: ON, WhatsApp: OFF, Push: ON
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainNoFakeDashboard(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("pika-sol-showcase-mock-bar", html);
        Assert.DoesNotContain("w80", html);
        Assert.DoesNotContain("w60", html);
        Assert.DoesNotContain("w70", html);
        Assert.DoesNotContain("w45", html);
        Assert.DoesNotContain("Email: ON", html);
        Assert.DoesNotContain("SMS: ON", html);
        Assert.DoesNotContain("WhatsApp: OFF", html);
        Assert.DoesNotContain("Push: ON", html);
    }

    // 18: No positive absolute legal-compliance claims: KVKK compliant, GDPR compliant, IYS compliant, fully compliant, 100% compliant, automatic compliance
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainNoPositiveAbsoluteLegalComplianceClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("KVKK compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("GDPR compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("IYS compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("fully compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("100% compliant", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("automatic compliance", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("compliance guaranteed", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("zero consent risk", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("zero compliance risk", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed lawful", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed consent", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("automatically compliant", body, StringComparison.OrdinalIgnoreCase);

        if (body.Contains("automatic legal compliance", StringComparison.OrdinalIgnoreCase))
        {
            Assert.Contains("Do not position", body, StringComparison.OrdinalIgnoreCase);
        }
    }

    // 19: Page clearly states: Audience != Consent
    [Fact]
    public async Task TurkishPage_StatesAudienceNotEqualConsent()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("HEDEF KİTLE ≠ İLETİŞİM İZNİ", decoded);
        Assert.Contains("Doğru müşteri, izinli iletişim anlamına gelmez", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesAudienceNotEqualConsent()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("AUDIENCE ≠ CONSENT", decoded);
        Assert.Contains("The right customer does not automatically mean permitted communication", decoded);
    }

    // 20: Page clearly states: Consent != Reachability
    [Fact]
    public async Task TurkishPage_StatesConsentNotEqualReachability()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Consent Management, reachability değildir", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesConsentNotEqualReachability()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Consent Management is not reachability", decoded);
    }

    // 21: Page clearly states: Consent != Delivery Guarantee
    [Fact]
    public async Task TurkishPage_StatesConsentNotEqualDeliveryGuarantee()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İZİN ≠ TESLİMAT GARANTİSİ", decoded);
        Assert.Contains("İletişim izni, mesajın mutlaka teslim edileceği anlamına gelmez", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesConsentNotEqualDeliveryGuarantee()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("CONSENT ≠ DELIVERY GUARANTEE", decoded);
        Assert.Contains("Permission to communicate does not mean the message will necessarily be delivered", decoded);
    }

    // 22: Page clearly states: Pika does not replace IYS
    [Fact]
    public async Task TurkishPage_StatesPikaDoesNotReplaceIys()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika IYS'nin yerine geçmez", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesPikaDoesNotReplaceIys()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika does not replace IYS", decoded);
    }

    // 23: Page clearly states: Opt-out is a suppression signal
    [Fact]
    public async Task TurkishPage_StatesOptOutIsSuppressionSignal()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("önemli bir suppression sinyalidir", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesOptOutIsSuppressionSignal()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("important suppression signal", decoded);
    }

    // 24: Page clearly states: Audience / opportunity / journey does not remove opt-out
    [Fact]
    public async Task TurkishPage_StatesAudienceOpportunityJourneyDoesNotRemoveOptOut()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("opt-out durumunu ortadan kaldırmaz", decoded);
    }

    [Fact]
    public async Task EnglishPage_StatesAudienceOpportunityJourneyDoesNotRemoveOptOut()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("does not remove their opt-out state", decoded);
    }

    // 25: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_HasExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var questions = new[]
        {
            "Consent Management nedir?",
            "Pika gönderimden önce IYS durumunu kontrol eder mi?",
            "Opt-out durumunda ne olur?",
            "Müşteri Email ve SMS tercihlerini kendisi güncelleyebilir mi?",
            "Aynı preference sayfasından WhatsApp izni de yönetilir mi?",
            "İletişim izni mesajın kesin teslim edileceği anlamına gelir mi?",
            "Consent Management hukuki uyumluluğu otomatik olarak garanti eder mi?",
            "Consent Management Campaign Manager ve Journey Manager ile nasıl ilişkilidir?"
        };

        foreach (var q in questions)
        {
            Assert.Contains(q, decoded);
        }

        var matches = Regex.Matches(html, @"<summary class=""consent-faq-summary"">", RegexOptions.IgnoreCase);
        Assert.Equal(8, matches.Count);
    }

    // 26: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_HasExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var questions = new[]
        {
            "What is Consent Management?",
            "Does Pika check IYS status before delivery?",
            "What happens when a customer has opted out?",
            "Can customers update their own Email and SMS preferences?",
            "Is WhatsApp consent managed through the same preference page?",
            "Does communication consent guarantee message delivery?",
            "Does Consent Management automatically guarantee legal compliance?",
            "How does Consent Management relate to Campaign Manager and Journey Manager?"
        };

        foreach (var q in questions)
        {
            Assert.Contains(q, decoded);
        }

        var matches = Regex.Matches(html, @"<summary class=""consent-faq-summary"">", RegexOptions.IgnoreCase);
        Assert.Equal(8, matches.Count);
    }

    // 27: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainNoPageLevelJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        // Ensure page does not define explicit Offer, FAQPage, or SoftwareApplication JSON-LD
        Assert.DoesNotContain("\"@type\": \"Offer\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("\"@type\":\"Offer\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("\"@type\": \"FAQPage\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("\"@type\":\"FAQPage\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("\"@type\": \"SoftwareApplication\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("\"@type\":\"SoftwareApplication\"", html, StringComparison.OrdinalIgnoreCase);
    }

    // 28: Correct internal links
    [Theory]
    [InlineData("/cozumler/consent-management", true)]
    [InlineData("/en/solutions/consent-management", false)]
    public async Task BothPages_ContainRequiredInternalLinks(string url, bool isTr)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var expectedLinks = isTr
            ? new[]
            {
                "/pika",
                "/cozumler/audience-manager",
                "/cozumler/campaign-manager",
                "/cozumler/journey-manager",
                "/cozumler/analytics-reporting",
                "/kanallar/email",
                "/kanallar/sms",
                "/kanallar/whatsapp",
                "/iletisim"
            }
            : new[]
            {
                "/en/pika",
                "/en/solutions/audience-manager",
                "/en/solutions/campaign-manager",
                "/en/solutions/journey-manager",
                "/en/solutions/analytics-reporting",
                "/en/channels/email",
                "/en/channels/sms",
                "/en/channels/whatsapp",
                "/en/contact"
            };

        foreach (var link in expectedLinks)
        {
            Assert.Contains($"href=\"{link}\"", html);
        }
    }

    // 29: Exact pricing copy
    [Fact]
    public async Task TurkishPage_HasExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_HasExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/consent-management");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 30: No fixed pricing
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainNoFixedPricing(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/ay", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/month", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("$", body);
        Assert.DoesNotContain("€", body);
        Assert.DoesNotContain("₺", body);
    }

    // 31: No fake consent metrics
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainNoFakeConsentMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("%99", body);
        Assert.DoesNotContain("99%", body);
        Assert.DoesNotContain("%100", body);
        Assert.DoesNotContain("100%", body);
        Assert.DoesNotContain("Deniz Kaya", body);
    }

    // 32: No Wiki links
    [Theory]
    [InlineData("/cozumler/consent-management")]
    [InlineData("/en/solutions/consent-management")]
    public async Task BothPages_ContainNoWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", body, StringComparison.OrdinalIgnoreCase);
    }

    // 33: Absolute Homepage Freeze audit
    [Fact]
    public void HomepageAndSharedFiles_RemainUntouched()
    {
        var projectRoot = GetProjectRoot();

        var homeIndex = Path.Combine(projectRoot, "Views", "Home", "Index.cshtml");
        var homeCss = Path.Combine(projectRoot, "wwwroot", "css", "pika-home.css");
        var orbitCss = Path.Combine(projectRoot, "wwwroot", "css", "pika-orbit.css");
        var orbitJs = Path.Combine(projectRoot, "wwwroot", "js", "pika-orbit.js");
        var layout = Path.Combine(projectRoot, "Views", "Shared", "_Layout.cshtml");
        var marketingHero = Path.Combine(projectRoot, "Views", "Shared", "_MarketingHero.cshtml");

        Assert.True(File.Exists(homeIndex), "Index.cshtml must exist.");
        Assert.True(File.Exists(homeCss), "pika-home.css must exist.");
        Assert.True(File.Exists(orbitCss), "pika-orbit.css must exist.");
        Assert.True(File.Exists(orbitJs), "pika-orbit.js must exist.");
        Assert.True(File.Exists(layout), "_Layout.cshtml must exist.");
        Assert.True(File.Exists(marketingHero), "_MarketingHero.cshtml must exist.");
    }
}
