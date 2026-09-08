using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class C17WhatsAppMessagingPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C17WhatsAppMessagingPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /kanallar/whatsapp = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/kanallar/whatsapp");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/channels/whatsapp = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/channels/whatsapp");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.WhatsAppMessaging SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsWhatsAppMessaging_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "WhatsAppMessaging");
        Assert.NotNull(entry);
        Assert.Equal("/kanallar/whatsapp", entry.AlternatePathTr);
        Assert.Equal("/en/channels/whatsapp", entry.AlternatePathEn);
        Assert.Equal("WhatsApp Marketing | Onaylı Şablonlar ve Kontrollü Mesajlaşma | Pika", entry.TitleTr);
        Assert.Equal("WhatsApp Marketing | Approved Templates & Controlled Messaging | Pika", entry.TitleEn);
        Assert.Equal("Pika WhatsApp; onaylı WhatsApp Business API şablonlarını, tanımlı hedef kitle ve Campaign Manager veya Journey Manager bağlamında, mevcut opt-in kontrolleriyle kontrollü iletişimde kullanmanıza yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika WhatsApp helps use approved WhatsApp Business API templates for defined audiences through Campaign Manager or Journey Manager within controlled communication and available opt-in context.", entry.DescriptionEn);
        Assert.Equal("WhatsApp", entry.BreadcrumbTitleTr);
        Assert.Equal("WhatsApp", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
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
        var html = await client.GetStringAsync("/kanallar/whatsapp");
        var h1 = ExtractH1(html);
        Assert.Equal("Onaylı WhatsApp iletişimini, Pika'nın kontrollü aksiyon zincirinde doğru müşteri bağlamına taşıyın.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/whatsapp");
        var h1 = ExtractH1(html);
        Assert.Equal("Bring approved WhatsApp communication to the right customer context inside Pika's controlled action chain.", h1);
    }

    // 07: TR contains: Pika WhatsApp nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/whatsapp");
        Assert.Contains("Pika WhatsApp nedir?", html);
    }

    // 08: EN contains: What is Pika WhatsApp?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/whatsapp");
        Assert.Contains("What is Pika WhatsApp?", html);
    }

    // 09: Both contain required entities
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainRequiredEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("WhatsApp Business API", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Consent Management", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
    }

    // 10: Both establish WhatsApp = communication / execution channel
    [Theory]
    [InlineData("/kanallar/whatsapp", "iletişim kanalıdır")]
    [InlineData("/en/channels/whatsapp", "communication channel")]
    public async Task BothPages_EstablishWhatsAppIsExecutionChannel(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 11: Both establish approved template context
    [Theory]
    [InlineData("/kanallar/whatsapp", "onaylı WhatsApp Business template")]
    [InlineData("/en/channels/whatsapp", "approved WhatsApp Business template")]
    public async Task BothPages_EstablishApprovedTemplateContext(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 12: Both establish Pika does not guarantee template approval
    [Theory]
    [InlineData("/kanallar/whatsapp", "Pika şunu garanti etmez")]
    [InlineData("/en/channels/whatsapp", "Pika does not guarantee")]
    public async Task BothPages_EstablishPikaDoesNotGuaranteeTemplateApproval(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 13: Both establish explicit opt-in context is required
    [Theory]
    [InlineData("/kanallar/whatsapp", "explicit customer opt-in")]
    [InlineData("/en/channels/whatsapp", "explicit customer opt-in")]
    public async Task BothPages_EstablishExplicitOptInRequired(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 14: Both establish public Email/SMS preference form does not manage WhatsApp consent
    [Theory]
    [InlineData("/kanallar/whatsapp", "WhatsApp consent aynı public checkbox akışından yönetilmez")]
    [InlineData("/en/channels/whatsapp", "WhatsApp consent is not managed through that same public checkbox flow")]
    public async Task BothPages_EstablishPublicPreferenceFormBoundary(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 15: Both establish WhatsApp opt-in can come from webhook / import sources
    [Theory]
    [InlineData("/kanallar/whatsapp", "ayrı incoming webhook veya import")]
    [InlineData("/en/channels/whatsapp", "separate incoming webhook or import")]
    public async Task BothPages_EstablishOptInSources(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 16: Both establish Audience Manager defines actual audience
    [Theory]
    [InlineData("/kanallar/whatsapp", "Audience Manager gerçek hedef kitleyi müşteri verisi ve açık kurallar üzerinden tanımlar.")]
    [InlineData("/en/channels/whatsapp", "Audience Manager defines the actual target audience through customer data and explicit rules.")]
    public async Task BothPages_EstablishAudienceManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 17: Both establish Campaign Manager manages campaign action
    [Theory]
    [InlineData("/kanallar/whatsapp", "Campaign Manager tanımlı hedef kitle, içerik, kanal, zamanlama ve gönderim öncesi kontrolleri aynı kampanya bağlamında yönetir.")]
    [InlineData("/en/channels/whatsapp", "Campaign Manager manages defined audience, content, channel, timing and pre-send controls within the same campaign context.")]
    public async Task BothPages_EstablishCampaignManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 18: Both establish Journey Manager manages multi-step flow
    [Theory]
    [InlineData("/kanallar/whatsapp", "Journey Manager başlangıç koşulları, dallanma, bekleme ve aksiyon adımlarından oluşan çok adımlı müşteri akışını yönetir.")]
    [InlineData("/en/channels/whatsapp", "Journey Manager manages a multi-step customer flow consisting of entry conditions, branching, waits and action steps.")]
    public async Task BothPages_EstablishJourneyManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 19: Both establish WhatsApp is not a journey engine
    [Theory]
    [InlineData("/kanallar/whatsapp", "journey'yi yöneten otonom motor değildir")]
    [InlineData("/en/channels/whatsapp", "is not an autonomous journey engine")]
    public async Task BothPages_EstablishWhatsAppIsNotAJourneyEngine(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 20: Both establish WhatsApp is not a chatbot
    [Theory]
    [InlineData("/kanallar/whatsapp", "WhatsApp, chatbot değildir.")]
    [InlineData("/en/channels/whatsapp", "WhatsApp is not a chatbot.")]
    public async Task BothPages_EstablishWhatsAppIsNotAChatbot(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 21: Both establish no verified live two-way customer-service product
    [Theory]
    [InlineData("/kanallar/whatsapp", "canlı iki yönlü müşteri hizmeti veya agent inbox")]
    [InlineData("/en/channels/whatsapp", "live two-way customer-service or agent-inbox")]
    public async Task BothPages_EstablishNoLiveTwoWayCustomerService(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 22: Both establish no unapproved free-form template bypass
    [Theory]
    [InlineData("/kanallar/whatsapp", "onaysız serbest mesaj kanalı değildir")]
    [InlineData("/en/channels/whatsapp", "is not an unapproved free-form messaging channel")]
    public async Task BothPages_EstablishNoUnapprovedTemplateBypass(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 23: Both contain delivery outcome / status measurement context
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainDeliveryOutcomeTelemetry(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.True(decoded.Contains("delivery outcome", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("delivery-outcome", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("delivery sonuc", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("status", StringComparison.OrdinalIgnoreCase));
    }

    // 24: Both establish Analytics & Reporting evaluates results
    [Theory]
    [InlineData("/kanallar/whatsapp", "Analytics & Reporting: bu sonucu kampanya, audience")]
    [InlineData("/en/channels/whatsapp", "Analytics & Reporting: evaluates that outcome together with campaign, audience")]
    public async Task BothPages_EstablishAnalyticsReportingRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        var plain = Regex.Replace(decoded, @"<[^>]+>", string.Empty);
        Assert.Contains(expectedPhrase, plain);
    }

    // 25: View contains no ViewData overrides, no JsonLd section, no /wiki/, no <img
    [Fact]
    public void ViewSource_ContainsNoViewDataOverridesOrWikiLinksOrImgTags()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "WhatsAppMessaging.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("<img", content);
    }

    // 26: No screenshots in rendered HTML
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoScreenshots(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain(".png", body);
        Assert.DoesNotContain(".jpg", body);
        Assert.DoesNotContain(".jpeg", body);
        Assert.DoesNotContain(".webp", body);
        Assert.DoesNotContain("/wiki/assets/images", body);
    }

    // 27: Push channel = 0 occurrences in rendered HTML and view source
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_RenderedBody_ContainsZeroPushOccurrences(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.False(Regex.IsMatch(body, @"\bpush\b", RegexOptions.IgnoreCase),
            "Rendered body must contain zero occurrences of 'push'");
    }

    [Fact]
    public void ViewSource_ContainsZeroPushOccurrences()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "WhatsAppMessaging.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.False(Regex.IsMatch(content, @"\bpush\b", RegexOptions.IgnoreCase),
            "View source must contain zero occurrences of 'push'");
    }

    // 28: No positive chatbot claims
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoPositiveChatbotClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenPatterns = new[]
        {
            @"\bconversational AI platform\b",
            @"\blive chat software\b",
            @"\bcustomer support inbox\b",
            @"\bautomated customer service\b",
            @"\binstant conversations\b",
            @"\breal-time conversations\b",
            @"\bconversational commerce\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains unsupported positive chatbot claim matching '{pattern}'");
        }
    }

    // 29: No positive two-way, live chat, or automated response claims
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoPositiveTwoWayOrLiveChatClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenPatterns = new[]
        {
            @"\blive customer support\b",
            @"\btwo-way conversations\b",
            @"\btwo-way chat\b",
            @"\bcanlı müşteri desteği\b",
            @"\bcanlı sohbet\b",
            @"\bmanage replies\b",
            @"\bautomatic reply\b",
            @"\bAI reply\b",
            @"\bautonomous reply\b",
            @"\botomatik yanıt\b",
            @"\botonom yanıt\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden two-way or live-service claim matching '{pattern}'");
        }
    }

    // 30: No positive unapproved-template dispatch claim
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoPositiveUnapprovedTemplateDispatchClaim(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenPatterns = new[]
        {
            @"\bsend unapproved templates\b",
            @"\bonaysız şablon gönderimi\b",
            @"\bonaysız serbest mesaj gönderin\b",
            @"\btemplate bypass\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden template-bypass claim matching '{pattern}'");
        }
    }

    // 31: No guaranteed template-approval claim
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoGuaranteedTemplateApprovalClaim(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenPatterns = new[]
        {
            @"\bguaranteed template approval\b",
            @"\bgarantili şablon onayı\b",
            @"\bher şablon onaylanır\b",
            @"\bguaranteed WhatsApp approval\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains guaranteed template approval claim matching '{pattern}'");
        }
    }

    // 32: No guaranteed delivery, read, reply, conversion claim
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoOutcomeGuarantees(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenPatterns = new[]
        {
            @"\bguaranteed delivery\b",
            @"\bgarantili teslimat\b",
            @"\bguaranteed read\b",
            @"\bgarantili okuma\b",
            @"\bguaranteed reply\b",
            @"\bgarantili cevap\b",
            @"\bguaranteed engagement\b",
            @"\bguaranteed conversion\b",
            @"\bgarantili dönüşüm\b",
            @"\bguaranteed revenue\b",
            @"\bgarantili ciro\b",
            @"\bhighest engagement\b",
            @"\bhigh engagement\b",
            @"\binstant delivery\b",
            @"\breal-time messaging\b",
            @"\breal time messaging\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden outcome guarantee matching '{pattern}'");
        }
    }

    // 33: No fake percentages or metrics
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoFakeMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"%\s*\d+", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\d+\s*%", RegexOptions.IgnoreCase), body);
        Assert.DoesNotContain("₺", body);
        Assert.DoesNotContain("$", body);
    }

    // 34: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/whatsapp");

        var expectedQuestions = new[]
        {
            "Pika WhatsApp nedir?",
            "Pika WhatsApp chatbot veya canlı müşteri hizmeti sunuyor mu?",
            "WhatsApp mesajları onaylı template kullanmak zorunda mı?",
            "Pika WhatsApp hedef kitleyi otomatik olarak seçer mi?",
            "WhatsApp Campaign Manager ve Journey Manager ile nasıl çalışır?",
            "WhatsApp opt-in aynı Email/SMS preference ekranından mı yönetilir?",
            "Pika WhatsApp template onayını garanti eder mi?",
            "WhatsApp gönderimi müşterinin mesajı okuyacağını veya dönüşüm sağlayacağını garanti eder mi?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, html);
        }

        var faqSummaryMatches = Regex.Matches(html, @"<summary[^>]*class=""wa-faq-summary""[^>]*>([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, faqSummaryMatches.Count);
    }

    // 35: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/whatsapp");

        var expectedQuestions = new[]
        {
            "What is Pika WhatsApp?",
            "Does Pika WhatsApp provide a chatbot or live customer service?",
            "Does WhatsApp communication require approved templates?",
            "Does Pika WhatsApp automatically choose the audience?",
            "How does WhatsApp work with Campaign Manager and Journey Manager?",
            "Is WhatsApp opt-in managed through the same Email/SMS preference screen?",
            "Does Pika guarantee WhatsApp template approval?",
            "Does WhatsApp delivery guarantee that the customer reads the message or converts?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, html);
        }

        var faqSummaryMatches = Regex.Matches(html, @"<summary[^>]*class=""wa-faq-summary""[^>]*>([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, faqSummaryMatches.Count);
    }

    // 36: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoPageLevelProhibitedJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        var jsonLdMatches = Regex.Matches(html, @"<script[^>]*type=[""']application/ld(?:&#x2B;|\+)json[""'][^>]*>([\s\S]*?)</script>", RegexOptions.IgnoreCase);
        foreach (Match match in jsonLdMatches)
        {
            var content = match.Groups[1].Value;
            Assert.DoesNotContain("FAQPage", content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("SoftwareApplication", content, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Offer", content, StringComparison.OrdinalIgnoreCase);
        }
    }

    // 37: Correct contextual internal links on TR & EN
    [Fact]
    public async Task TurkishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/whatsapp");

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/whatsapp");

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
    }

    // 38: Exact pricing lines
    [Theory]
    [InlineData("/kanallar/whatsapp", "İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.")]
    [InlineData("/en/channels/whatsapp", "Pricing is tailored to your requirements and scope of use.")]
    public async Task BothPages_ContainExactPricingLine(string url, string expectedLine)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedLine, decoded);
    }

    // 39: No fixed pricing
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainNoFixedPricing(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/ay", body);
        Assert.DoesNotContain("TL/ay", body);
        Assert.DoesNotContain("$/mo", body);
        Assert.DoesNotContain("€/mo", body);
    }

    // 40: Zero Wiki links
    [Theory]
    [InlineData("/kanallar/whatsapp")]
    [InlineData("/en/channels/whatsapp")]
    public async Task BothPages_ContainZeroWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("href=\"/wiki", body);
        Assert.DoesNotContain("href=\"https://wiki.pika.tr", body);
        Assert.DoesNotContain("href=\"http://wiki.pika.tr", body);
    }
}
