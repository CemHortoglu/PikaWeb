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

public class C16SmsCampaignsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C16SmsCampaignsPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /kanallar/sms = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/kanallar/sms");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/channels/sms = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/channels/sms");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.SmsCampaigns SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsSmsCampaigns_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "SmsCampaigns");
        Assert.NotNull(entry);
        Assert.Equal("/kanallar/sms", entry.AlternatePathTr);
        Assert.Equal("/en/channels/sms", entry.AlternatePathEn);
        Assert.Equal("SMS Marketing | İYS Kontrollü Kampanya ve SMS Gönderimi | Pika", entry.TitleTr);
        Assert.Equal("SMS Marketing | Controlled Campaigns & SMS Delivery | Pika", entry.TitleEn);
        Assert.Equal("Pika SMS; tanımlı hedef kitleye yönelik kısa ve zaman hassasiyetli iletişimleri Campaign Manager veya Journey Manager üzerinden, gönderim öncesi IYS ve opt-out kontrolleriyle kontrollü biçimde yürütmenize yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika SMS helps execute concise, time-sensitive communication for defined audiences through Campaign Manager or Journey Manager with pre-dispatch IYS and opt-out controls.", entry.DescriptionEn);
        Assert.Equal("SMS", entry.BreadcrumbTitleTr);
        Assert.Equal("SMS", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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
        var html = await client.GetStringAsync("/kanallar/sms");
        var h1 = ExtractH1(html);
        Assert.Equal("Kısa ve zaman hassasiyetli iletişimi, Pika'nın kontrollü aksiyon zincirinde SMS kanalına taşıyın.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/sms");
        var h1 = ExtractH1(html);
        Assert.Equal("Bring concise, time-sensitive communication to the SMS channel inside Pika's controlled action chain.", h1);
    }

    // 07: TR contains: Pika SMS nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/sms");
        Assert.Contains("Pika SMS nedir?", html);
    }

    // 08: EN contains: What is Pika SMS?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/sms");
        Assert.Contains("What is Pika SMS?", html);
    }

    // 09: Both contain required entities
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainRequiredEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Customer Intelligence", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Consent Management", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
    }

    // 10: Both establish SMS = communication / execution channel
    [Theory]
    [InlineData("/kanallar/sms", "iletişim kanalıdır.")]
    [InlineData("/en/channels/sms", "is one of Pika's confirmed communication channels")]
    public async Task BothPages_EstablishSmsIsExecutionChannel(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 11: Both establish Audience Manager defines the actual audience
    [Theory]
    [InlineData("/kanallar/sms", "Audience Manager gerçek hedef kitleyi müşteri verisi ve açık kurallar üzerinden tanımlar.")]
    [InlineData("/en/channels/sms", "Audience Manager defines the actual target audience through customer data and explicit rules.")]
    public async Task BothPages_EstablishAudienceManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 12: Both establish Campaign Manager manages campaign context
    [Theory]
    [InlineData("/kanallar/sms", "Campaign Manager tanımlı hedef kitle, içerik, kanal, zamanlama ve gönderim öncesi kontrolleri aynı kampanya bağlamında yönetir.")]
    [InlineData("/en/channels/sms", "Campaign Manager manages defined audience, content, channel, timing and pre-send controls within the same campaign context.")]
    public async Task BothPages_EstablishCampaignManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 13: Both establish Journey Manager manages multi-step flow
    [Theory]
    [InlineData("/kanallar/sms", "Journey Manager başlangıç koşulları, dallanma, bekleme ve aksiyon adımlarından oluşan çok adımlı müşteri akışını yönetir.")]
    [InlineData("/en/channels/sms", "Journey Manager manages a multi-step customer flow consisting of entry conditions, branching, waits and action steps.")]
    public async Task BothPages_EstablishJourneyManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 14: Both establish SMS does not autonomously trigger journeys
    [Theory]
    [InlineData("/kanallar/sms", "journey'yi tetikleyen otonom motor değildir")]
    [InlineData("/en/channels/sms", "is not an autonomous journey-trigger engine")]
    public async Task BothPages_EstablishSmsDoesNotAutonomouslyTrigger(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 15: Both contain IYS, opt-out, quiet-hours
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainIysOptOutQuietHours(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("IYS", decoded);
        Assert.Contains("opt-out", decoded, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("quiet-hours", decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 16: Both establish IYS control occurs pre-dispatch for relevant commercial SMS communication
    [Theory]
    [InlineData("/kanallar/sms", "gönderim öncesi IYS durum kontrolü")]
    [InlineData("/en/channels/sms", "pre-dispatch IYS status check")]
    public async Task BothPages_EstablishIysPreDispatch(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 17: Both establish opt-out participates in suppression
    [Theory]
    [InlineData("/kanallar/sms", "opt-out durumu suppression")]
    [InlineData("/en/channels/sms", "opt-out state forms part of suppression")]
    public async Task BothPages_EstablishOptOutSuppression(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 18: Both establish quiet-hours is a communication-time control
    [Theory]
    [InlineData("/kanallar/sms", "quiet-hours mekanizması")]
    [InlineData("/en/channels/sms", "quiet-hours mechanism")]
    public async Task BothPages_EstablishQuietHoursControl(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 19: Both contain operator gateway / operator-gateway context
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainOperatorGatewayContext(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.True(decoded.Contains("operator-gateway", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("operator gateway", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("operatör ağ geçidi", StringComparison.OrdinalIgnoreCase));
    }

    // 20: Both state operator-gateway dispatch does not guarantee immediate delivery
    [Theory]
    [InlineData("/kanallar/sms", "belirli bir sürede veya kesin biçimde teslim edileceği anlamına gelmez")]
    [InlineData("/en/channels/sms", "does not mean it will be delivered to the customer's device within a particular time or with certainty")]
    public async Task BothPages_StateOperatorGatewayDoesNotGuaranteeImmediateDelivery(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 21: Both contain delivery-outcome / status measurement context
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainDeliveryOutcomeTelemetry(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.True(decoded.Contains("delivery outcome", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("delivery-outcome", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("teslimat sonucu", StringComparison.OrdinalIgnoreCase) ||
                    decoded.Contains("status", StringComparison.OrdinalIgnoreCase));
    }

    // 22: Both establish Analytics & Reporting evaluates measured results
    [Theory]
    [InlineData("/kanallar/sms", "Analytics & Reporting bu sonucu")]
    [InlineData("/en/channels/sms", "Analytics & Reporting evaluates that outcome")]
    public async Task BothPages_EstablishAnalyticsReportingRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 23: View contains no ViewData overrides, no JsonLd section, no /wiki/, no <img
    [Fact]
    public void ViewSource_ContainsNoViewDataOverridesOrWikiLinksOrImgTags()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "SmsCampaigns.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("<img", content);
    }

    // 24: No screenshots in rendered HTML
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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

    // 25: Push channel = 0 occurrences in rendered HTML and view source
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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
        var viewPath = Path.Combine(root, "Views", "Solutions", "SmsCampaigns.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.False(Regex.IsMatch(content, @"\bpush\b", RegexOptions.IgnoreCase),
            "View source must contain zero occurrences of 'push'");
    }

    // 26: No positive speed claims
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainNoPositiveSpeedClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenSpeedPatterns = new[]
        {
            @"\bHızlı Teslim\b",
            @"\bFast Delivery\b",
            @"\bZamanlı ve Anlık Gönderim\b",
            @"\bScheduled & Instant Delivery\b",
            @"\banında tetikleme\b",
            @"\banında tetikleyin\b",
            @"\btrigger instantly\b",
            @"\bSMS is the fastest\b",
            @"\bsaniyeler içinde\b",
            @"\bin seconds\b",
            @"\bdakikalar içinde\b",
            @"\bin minutes\b"
        };

        foreach (var pattern in forbiddenSpeedPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden speed claim matching '{pattern}'");
        }

        // Allow explicit negative boundaries (e.g. "SMS, yüksek görünürlük veya okuma garantisi değildir.")
        Assert.False(Regex.IsMatch(body, @"\bHigh Visibility\b", RegexOptions.IgnoreCase),
            "Page contains unsupported High Visibility claim");
        Assert.False(Regex.IsMatch(body, @"\bYüksek Görünürlük\b(?!\s*veya okuma garantisi değildir)", RegexOptions.IgnoreCase),
            "Page contains unsupported positive Yüksek Görünürlük claim");
    }

    // 27: No positive visibility or conversion claims
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainNoPositiveVisibilityOrConversionClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenPatterns = new[]
        {
            @"\bhigh read rate\b",
            @"\byüksek okunma oranı\b",
            @"\bguaranteed delivery\b",
            @"\bgarantili teslimat\b",
            @"\bguaranteed reach\b",
            @"\bgarantili erişim\b",
            @"\bincrease conversions\b",
            @"\bdönüşümü artırın\b",
            @"\bherkes okur\b",
            @"\beveryone reads\b",
            @"\bguaranteed sales\b",
            @"\bgarantili satış\b"
        };

        foreach (var pattern in forbiddenPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden claim matching '{pattern}'");
        }
    }

    // 28: No Email-specific SMS claims (open rate, open tracking, Email-style click tracking)
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainNoEmailSpecificClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenTerms = new[]
        {
            @"\bopen rate\b",
            @"\bopen tracking\b",
            @"\baçılma oranı\b",
            @"\baçılma takibi\b",
            @"\bEmail-style click tracking\b"
        };

        foreach (var pattern in forbiddenTerms)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden email-specific claim matching '{pattern}'");
        }
    }

    // 29: No provider brand names
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainNoProviderBrandNames(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenBrands = new[]
        {
            @"\bTwilio\b",
            @"\bNetgsm\b",
            @"\bİleti Merkezi\b",
            @"\bIleti Merkezi\b",
            @"\bInfobip\b",
            @"\bSinch\b",
            @"\bMessageBird\b",
            @"\bVonage\b",
            @"\bClickSend\b"
        };

        foreach (var pattern in forbiddenBrands)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains provider brand matching '{pattern}'");
        }
    }

    // 30: No fixed character limits presented as platform limits
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
    public async Task BothPages_ContainNoFixedCharacterLimitsAsPlatformLimits(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenLimitPatterns = new[]
        {
            @"\b160 karakter sınırı\b",
            @"\b160 character limit\b",
            @"\b160-character limit\b",
            @"\bsadece 160 karakter\b",
            @"\bonly 160 characters\b"
        };

        foreach (var pattern in forbiddenLimitPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains fixed character limit claim matching '{pattern}'");
        }
    }

    // 31: No fake percentages or metrics
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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

    // 32: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/sms");

        var expectedQuestions = new[]
        {
            "Pika SMS nedir?",
            "Pika SMS hedef kitleyi otomatik olarak seçer mi?",
            "SMS Campaign Manager ve Journey Manager ile nasıl çalışır?",
            "Pika SMS gönderiminden önce IYS kontrolü yapar mı?",
            "SMS iletişiminde opt-out ve gönderim zamanı kontrolü var mı?",
            "Pika SMS anında veya kesin teslimat garantisi verir mi?",
            "SMS için hangi sonuçlar ölçülebilir?",
            "SMS gönderimi müşterinin mesajı okuyacağını veya dönüşüm sağlayacağını garanti eder mi?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, html);
        }

        var faqSummaryMatches = Regex.Matches(html, @"<summary[^>]*class=""sms-faq-summary""[^>]*>([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, faqSummaryMatches.Count);
    }

    // 33: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/sms");

        var expectedQuestions = new[]
        {
            "What is Pika SMS?",
            "Does Pika SMS automatically choose the audience?",
            "How does SMS work with Campaign Manager and Journey Manager?",
            "Does Pika check IYS status before SMS delivery?",
            "Does SMS communication include opt-out and communication-time controls?",
            "Does Pika SMS guarantee instant or definitive delivery?",
            "Which SMS outcomes can be measured?",
            "Does SMS delivery guarantee that the customer reads the message or converts?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, html);
        }

        var faqSummaryMatches = Regex.Matches(html, @"<summary[^>]*class=""sms-faq-summary""[^>]*>([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, faqSummaryMatches.Count);
    }

    // 34: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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

    // 35: Correct contextual internal links on TR & EN
    [Fact]
    public async Task TurkishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/sms");

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
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/sms");

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
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
    }

    // 36: Exact pricing lines
    [Theory]
    [InlineData("/kanallar/sms", "İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.")]
    [InlineData("/en/channels/sms", "Pricing is tailored to your requirements and scope of use.")]
    public async Task BothPages_ContainExactPricingLine(string url, string expectedLine)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedLine, decoded);
    }

    // 37: No fixed pricing
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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

    // 38: Zero Wiki links
    [Theory]
    [InlineData("/kanallar/sms")]
    [InlineData("/en/channels/sms")]
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
