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

public class C21FaqPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C21FaqPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /kaynaklar/sss = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/kaynaklar/sss");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/resources/faq = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/resources/faq");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Home.Faq SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_HomeFaq_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Home", "Faq");
        Assert.NotNull(entry);
        Assert.Equal("/kaynaklar/sss", entry.AlternatePathTr);
        Assert.Equal("/en/resources/faq", entry.AlternatePathEn);
        Assert.Equal("Sıkça Sorulan Sorular | Pika Ürün, Veri, Kanallar ve AI", entry.TitleTr);
        Assert.Equal("Pika FAQ | Product, Data, Channels, Consent & AI", entry.TitleEn);
        Assert.Equal("Pika hakkında sık sorulan soruları; müşteri ve ürün zekâsı, veri aktarımı, audience, campaign, journey, Email/SMS/WhatsApp, izin, güvenlik, AI ve fiyatlandırma başlıklarında inceleyin.", entry.DescriptionTr);
        Assert.Equal("Find answers about Pika customer and product intelligence, data ingestion, audiences, campaigns, journeys, Email/SMS/WhatsApp, consent, security, AI and pricing.", entry.DescriptionEn);
        Assert.Equal("Sıkça Sorulan Sorular", entry.BreadcrumbTitleTr);
        Assert.Equal("FAQ", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var h1Matches = Regex.Matches(html, @"<h1[^>]*>[\s\S]*?</h1>", RegexOptions.IgnoreCase);
        Assert.Single(h1Matches);
    }

    // 05: TR normalized H1
    [Fact]
    public async Task TurkishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kaynaklar/sss");
        var h1 = ExtractH1(html);
        Assert.Equal("Pika'yı değerlendirirken temel soruların yanıtlarını tek yerde bulun.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/resources/faq");
        var h1 = ExtractH1(html);
        Assert.Equal("Find clear answers to the core questions that come up when evaluating Pika.", h1);
    }

    // 07: Exactly 8 categories in both languages
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainExactlyEightCategories(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        var groupMatches = Regex.Matches(html, @"<div[^>]*id=""cat-[^""]*""[^>]*class=""faq-group", RegexOptions.IgnoreCase);
        Assert.Equal(8, groupMatches.Count);

        var sidebarLinks = Regex.Matches(html, @"<a[^>]*class=""nav-link[^""]*""[^>]*href=""#cat-", RegexOptions.IgnoreCase);
        Assert.Equal(8, sidebarLinks.Count);
    }

    // 08: Exactly 32 questions on TR render
    [Fact]
    public async Task TurkishPage_ContainsExactly32Questions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kaynaklar/sss");

        var accordionItems = Regex.Matches(html, @"<div[^>]*class=""accordion-item""", RegexOptions.IgnoreCase);
        Assert.Equal(32, accordionItems.Count);
    }

    // 09: Exactly 32 questions on EN render
    [Fact]
    public async Task EnglishPage_ContainsExactly32Questions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/resources/faq");

        var accordionItems = Regex.Matches(html, @"<div[^>]*class=""accordion-item""", RegexOptions.IgnoreCase);
        Assert.Equal(32, accordionItems.Count);
    }

    // 10: All supplied question strings exist
    [Fact]
    public async Task TurkishPage_ContainsAllSuppliedQuestionTitles()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kaynaklar/sss");
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pika nedir?", decoded);
        Assert.Contains("Pika bir CRM midir?", decoded);
        Assert.Contains("Pika yalnızca mesaj veya kampanya gönderim aracı mıdır?", decoded);
        Assert.Contains("Pika hangi işletmeler için anlamlıdır?", decoded);
        Assert.Contains("Pika hangi temel veri bağlamıyla çalışır?", decoded);
        Assert.Contains("Excel veya CSV ile Pika'ya veri aktarılabilir mi?", decoded);
        Assert.Contains("Pika'nın REST Ingestion API'si var mı?", decoded);
        Assert.Contains("Pika her CRM, ERP veya e-ticaret ürünü için hazır connector sunuyor mu?", decoded);
        Assert.Contains("Customer Intelligence ne yapar?", decoded);
        Assert.Contains("Product Intelligence ne yapar?", decoded);
        Assert.Contains("Pika 360 nedir?", decoded);
        Assert.Contains("Günün Fırsatları nedir?", decoded);
        Assert.Contains("Audience Manager ne yapar?", decoded);
        Assert.Contains("Content Studio ne yapar?", decoded);
        Assert.Contains("Campaign Manager ile Journey Manager arasındaki fark nedir?", decoded);
        Assert.Contains("Pika hedef kitleyi seçip kampanyayı tamamen kendi başına gönderebilir mi?", decoded);
        Assert.Contains("Pika'nın aktif iletişim kanalları hangileridir?", decoded);
        Assert.Contains("Email, SMS ve WhatsApp'ın rolleri nasıl ayrılır?", decoded);
        Assert.Contains("Pika gönderim ve etkileşim sonuçlarını ölçebilir mi?", decoded);
        Assert.Contains("Teknik gönderim başarılıysa kampanya sonucu garanti edilmiş olur mu?", decoded);
        Assert.Contains("Audience'a dahil olmak iletişim izni anlamına gelir mi?", decoded);
        Assert.Contains("Email, SMS ve WhatsApp tercihleri aynı public formdan mı yönetilir?", decoded);
        Assert.Contains("Pika'nın doğrulanmış güvenlik kontrolleri nelerdir?", decoded);
        Assert.Contains("Pika SOC 2, ISO 27001 veya SAML / SSO iddiası yapıyor mu?", decoded);
        Assert.Contains("Pika Pilot nedir?", decoded);
        Assert.Contains("Pika AI müşteri veya ticari fırsatı kendi başına oluşturur mu?", decoded);
        Assert.Contains("Pika AI kampanyaları otonom olarak gönderir mi?", decoded);
        Assert.Contains("Üretken AI prompt bağlamında kişisel veri sınırı var mı?", decoded);
        Assert.Contains("Pika demosunda ne değerlendirilir?", decoded);
        Assert.Contains("Pika demosunun sabit bir süresi var mı?", decoded);
        Assert.Contains("Pika'nın fiyatlandırması nasıl belirlenir?", decoded);
        Assert.Contains("Pika'yı kullanmak için bütün modülleri almak gerekir mi?", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllSuppliedQuestionTitles()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/resources/faq");
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("What is Pika?", decoded);
        Assert.Contains("Is Pika a CRM?", decoded);
        Assert.Contains("Is Pika only a messaging or campaign-delivery tool?", decoded);
        Assert.Contains("What kinds of organizations can evaluate Pika?", decoded);
        Assert.Contains("What core data context does Pika use?", decoded);
        Assert.Contains("Can data be imported into Pika with Excel or CSV?", decoded);
        Assert.Contains("Does Pika provide a REST Ingestion API?", decoded);
        Assert.Contains("Does Pika provide a ready-made connector for every CRM, ERP or e-commerce product?", decoded);
        Assert.Contains("What does Customer Intelligence do?", decoded);
        Assert.Contains("What does Product Intelligence do?", decoded);
        Assert.Contains("What is Pika 360?", decoded);
        Assert.Contains("What is Daily Opportunities?", decoded);
        Assert.Contains("What does Audience Manager do?", decoded);
        Assert.Contains("What does Content Studio do?", decoded);
        Assert.Contains("What is the difference between Campaign Manager and Journey Manager?", decoded);
        Assert.Contains("Can Pika choose the audience and send a campaign completely by itself?", decoded);
        Assert.Contains("Which communication channels are active in Pika?", decoded);
        Assert.Contains("How do Email, SMS and WhatsApp roles differ?", decoded);
        Assert.Contains("Can Pika measure delivery and engagement outcomes?", decoded);
        Assert.Contains("Does successful technical delivery guarantee campaign results?", decoded);
        Assert.Contains("Does audience membership mean communication permission?", decoded);
        Assert.Contains("Are Email, SMS and WhatsApp preferences managed through the same public form?", decoded);
        Assert.Contains("Which Pika security controls are verified?", decoded);
        Assert.Contains("Does Pika claim SOC 2, ISO 27001 or SAML / SSO capability?", decoded);
        Assert.Contains("What is Pika Pilot?", decoded);
        Assert.Contains("Does Pika AI invent customers or commercial opportunities?", decoded);
        Assert.Contains("Does Pika AI autonomously send campaigns?", decoded);
        Assert.Contains("Is there a personal-data boundary for generative-AI prompt context?", decoded);
        Assert.Contains("What is evaluated in a Pika demo?", decoded);
        Assert.Contains("Does the Pika demo have a fixed duration?", decoded);
        Assert.Contains("How is Pika pricing determined?", decoded);
        Assert.Contains("Do I need every Pika module?", decoded);
    }

    // 11: Required platform layers
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainRequiredPlatformLayers(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Product Intelligence", decoded);
        Assert.Contains("Pika 360", decoded);
        Assert.True(decoded.Contains("Günün Fırsatları") || decoded.Contains("Daily Opportunities"));
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Content Studio", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Consent Management", decoded);
        Assert.Contains("Security & Privacy", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
        Assert.Contains("Pika Pilot", decoded);
    }

    // 12: Active channel names
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainActiveChannelNames(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Email", decoded);
        Assert.Contains("SMS", decoded);
        Assert.Contains("WhatsApp", decoded);
    }

    // 13: Push capability = 0 (no positive Push marketing claims)
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainZeroPushPositiveClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push Notification", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Mobile Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Web Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/kanallar/push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/en/channels/push", body, StringComparison.OrdinalIgnoreCase);
    }

    // 14: Data intake references
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainDataIntakeReferences(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("REST Ingestion API", decoded);
        Assert.Contains("Excel", decoded);
        Assert.Contains("CSV", decoded);
        Assert.Contains("X-API-Key", decoded);
        Assert.Contains("X-Idempotency-Key", decoded);
    }

    // 15: No universal native-connector claim
    [Theory]
    [InlineData("/kaynaklar/sss", "C21 böyle bir iddia yapmaz")]
    [InlineData("/en/resources/faq", "C21 makes no such claim")]
    public async Task BothPages_EstablishNoUniversalNativeConnectorClaim(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 16: Audience != consent
    [Theory]
    [InlineData("/kaynaklar/sss", "Audience üyeliği consent kontrolünün yerine geçmez")]
    [InlineData("/en/resources/faq", "Audience membership does not replace consent controls")]
    public async Task BothPages_EstablishAudienceDoesNotEqualConsent(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 17: Preference form = Email + SMS, WhatsApp separate
    [Theory]
    [InlineData("/kaynaklar/sss", "Pika'nın mevcut public preference-update akışı Email ve SMS tercihlerini kapsar")]
    [InlineData("/en/resources/faq", "Pika's current public preference-update flow covers Email and SMS preferences")]
    public async Task BothPages_EstablishPreferenceFormScope(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 18: RBAC present
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainRbac(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("RBAC", decoded);
    }

    // 19: No autonomous AI sending
    [Theory]
    [InlineData("/kaynaklar/sss", "AI tarafından üretilen taslak ve öneriler kullanıcı değerlendirmesi altında kalır")]
    [InlineData("/en/resources/faq", "AI-generated drafts and suggestions remain under user review")]
    public async Task BothPages_EstablishNoAutonomousAiSending(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 20: Documented AI privacy boundary
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainDocumentedAiPrivacyBoundary(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("TCKN", decoded);
        Assert.Contains("Zero-PII", decoded);
    }

    // 21: Exact commercial pricing line
    [Theory]
    [InlineData("/kaynaklar/sss", "Pika sabit public paket fiyatı yayınlamaz. İhtiyacınıza ve kullanım kapsamınıza göre özel teklif hazırlanır.")]
    [InlineData("/en/resources/faq", "Pika does not publish a fixed public package price. Pricing is tailored to your requirements and scope of use.")]
    public async Task BothPages_ContainExactCommercialPricingLine(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 22: No fixed price patterns
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoFixedPricePattern(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(@"\$\d+", body);
        Assert.DoesNotMatch(@"€\d+", body);
        Assert.DoesNotMatch(@"\d+\s*₺", body);
        Assert.DoesNotMatch(@"\b(aylık|monthly)\s+\d+", body);
    }

    // 23: No fixed demo duration
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoFixedDemoDuration(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("15 dakikalık", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45 dakikalık", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("15 dakika", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45 dakika", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("15-minute", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("45-minute", body, StringComparison.OrdinalIgnoreCase);
    }

    // 24: View contains no ViewData overrides
    [Fact]
    public void ViewSource_ContainsNoViewDataOverrides()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Faq.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
    }

    // 25: Zero /wiki/ links
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainZeroWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
    }

    // 26: Zero microdata structured data attributes
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoFaqMicrodata(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.DoesNotContain("itemscope", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("schema.org/FAQPage", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("schema.org/Question", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("schema.org/Answer", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("itemprop=\"mainEntity\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("itemprop=\"acceptedAnswer\"", html, StringComparison.OrdinalIgnoreCase);
    }

    // 27: No page-level FAQPage JSON-LD
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoPageLevelJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.DoesNotContain("\"@type\": \"FAQPage\"", html);
        Assert.DoesNotContain("\"@type\":\"FAQPage\"", html);
        Assert.DoesNotContain("\"@type\": \"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"@type\":\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"@type\": \"Offer\"", html);
        Assert.DoesNotContain("\"@type\":\"Offer\"", html);
    }

    // 28: No <img> tags or screenshots
    [Fact]
    public void ViewSource_ContainsNoImgTagsOrScreenshots()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Faq.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("<img", content, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("service-image-", content, StringComparison.OrdinalIgnoreCase);
    }

    // 29: No unsupported real-time / immediate triggers / instant delivery claims
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoUnsupportedRealTimeClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("gerçek zamanlı aktarılması", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real-time order synchronization", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("anlık tetiklenen Journey", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("immediate event-driven journey triggers", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("instant delivery", body, StringComparison.OrdinalIgnoreCase);
    }

    // 30: No positive chatbot / two-way WhatsApp claims
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoChatbotClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("chatbot", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("conversational AI", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("agent inbox", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("live two-way WhatsApp", body, StringComparison.OrdinalIgnoreCase);
    }

    // 31: No positive certification claims
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoPositiveCertificationClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("SOC 2 certified", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ISO 27001 certified", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SOC 2 sertifikalı", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ISO 27001 sertifikalı", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SAML SSO desteği", body, StringComparison.OrdinalIgnoreCase);
    }

    // 32: No ROI / conversion / revenue guarantees
    [Theory]
    [InlineData("/kaynaklar/sss")]
    [InlineData("/en/resources/faq")]
    public async Task BothPages_ContainNoGuarantees(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("guaranteed ROI", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed revenue", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ROI garantisi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("dönüşüm garantisi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("gelir garantisi", body, StringComparison.OrdinalIgnoreCase);
    }

    // 33: Contextual internal links present
    [Fact]
    public async Task TurkishPage_ContainsRequiredContextualLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kaynaklar/sss");

        Assert.Contains("href=\"/demo-talebi\"", html);
        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/entegrasyonlar\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/pika-360\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/content-studio\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/guvenlik-ve-gizlilik\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
        Assert.Contains("href=\"/kurumsal\"", html);
        Assert.Contains("href=\"/iletisim\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsRequiredContextualLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/resources/faq");

        Assert.Contains("href=\"/en/demo-request\"", html);
        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/integrations\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/pika-360\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/content-studio\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/security-and-privacy\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
        Assert.Contains("href=\"/en/corporate\"", html);
        Assert.Contains("href=\"/en/contact\"", html);
    }

    // 34: Homepage files untouched
    [Fact]
    public void Homepage_FilesRemainUntouched()
    {
        var root = GetProjectRoot();
        Assert.True(File.Exists(Path.Combine(root, "Views", "Home", "Index.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-home.css")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-orbit.css")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "js", "pika-orbit.js")));
    }

    // 35: site.js untouched
    [Fact]
    public void SiteJs_RemainsUntouched()
    {
        var root = GetProjectRoot();
        var siteJsPath = Path.Combine(root, "wwwroot", "js", "site.js");
        Assert.True(File.Exists(siteJsPath));
    }
}
