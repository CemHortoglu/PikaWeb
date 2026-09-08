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

public class C15EmailMarketingPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C15EmailMarketingPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /kanallar/email = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/kanallar/email");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/channels/email = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/channels/email");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.EmailMarketing SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsEmailMarketing_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "EmailMarketing");
        Assert.NotNull(entry);
        Assert.Equal("/kanallar/email", entry.AlternatePathTr);
        Assert.Equal("/en/channels/email", entry.AlternatePathEn);
        Assert.Equal("Email Marketing | Şablon, Kişiselleştirme ve Teslimat Takibi | Pika", entry.TitleTr);
        Assert.Equal("Email Marketing | Templates, Personalization & Delivery Tracking | Pika", entry.TitleEn);
        Assert.Equal("Pika Email; Content Studio'da hazırlanan görsel şablonları ve kişiselleştirilmiş içeriği Campaign Manager veya Journey Manager üzerinden kontrollü Email iletişiminde kullanmanıza ve teslimat ile etkileşim sonuçlarını ölçmenize yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Email helps use visual templates and personalized content prepared in Content Studio through controlled Email communication in Campaign Manager or Journey Manager, with delivery and engagement measurement.", entry.DescriptionEn);
        Assert.Equal("Email", entry.BreadcrumbTitleTr);
        Assert.Equal("Email", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
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
        var html = await client.GetStringAsync("/kanallar/email");
        var h1 = ExtractH1(html);
        Assert.Equal("Zengin Email içeriğini, Pika'nın müşteri bağlamı ve kontrollü aksiyon zinciriyle buluşturun.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/email");
        var h1 = ExtractH1(html);
        Assert.Equal("Connect rich Email content to Pika's customer context and controlled action chain.", h1);
    }

    // 07: TR contains: Pika Email nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/email");
        Assert.Contains("Pika Email nedir?", html);
    }

    // 08: EN contains: What is Pika Email?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerQuestion()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/email");
        Assert.Contains("What is Pika Email?", html);
    }

    // 09: Both contain required entities
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainRequiredEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Consent Management", decoded);
        Assert.Contains("Analytics & Reporting", decoded);
        Assert.Contains("Pika Pilot", decoded);
    }

    // 10: Both establish Email = communication / execution channel
    [Theory]
    [InlineData("/kanallar/email", "iletişim kanalıdır.")]
    [InlineData("/en/channels/email", "is the communication channel.")]
    public async Task BothPages_EstablishEmailIsExecutionChannel(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 11: Both establish Content Studio prepares Email content
    [Theory]
    [InlineData("/kanallar/email", "Content Studio, Email içeriğinin görsel olarak hazırlanabildiği ve düzenlenebildiği Pika içerik katmanıdır.")]
    [InlineData("/en/channels/email", "Content Studio is Pika's content layer where Email content can be visually prepared and edited.")]
    public async Task BothPages_EstablishContentStudioRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 12: Both establish Audience Manager defines the audience
    [Theory]
    [InlineData("/kanallar/email", "Audience Manager müşteri verisi ve açık kurallar üzerinden gerçek hedef kitle tanımını yönetir.")]
    [InlineData("/en/channels/email", "Audience Manager manages the actual audience definition through customer data and explicit rules.")]
    public async Task BothPages_EstablishAudienceManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 13: Both establish Campaign Manager manages campaign delivery
    [Theory]
    [InlineData("/kanallar/email", "Campaign Manager tanımlı hedef kitle, hazırlanmış içerik, kanal, zamanlama ve gönderim öncesi kontrolleri aynı kampanya bağlamında yönetir.")]
    [InlineData("/en/channels/email", "Campaign Manager manages defined audience, prepared content, channel, timing and pre-send controls within the same campaign context.")]
    public async Task BothPages_EstablishCampaignManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 14: Both establish Journey Manager manages multi-step flow
    [Theory]
    [InlineData("/kanallar/email", "Journey Manager başlangıç koşulları, dallanma, bekleme ve aksiyon adımlarından oluşan çok adımlı müşteri akışını yönetir.")]
    [InlineData("/en/channels/email", "Journey Manager manages a multi-step customer flow consisting of entry conditions, branching, waits and action steps.")]
    public async Task BothPages_EstablishJourneyManagerRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 15: Both establish Consent Management remains separate
    [Theory]
    [InlineData("/kanallar/email", "Consent Management iletişim uygunluğu ve opt-out bağlamını değerlendirir.")]
    [InlineData("/en/channels/email", "Consent Management evaluates communication eligibility and opt-out context.")]
    public async Task BothPages_EstablishConsentManagementRelationship(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 16: Both contain unsubscribe and opt-out
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainUnsubscribeAndOptOut(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("unsubscribe", decoded, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("opt-out", decoded, StringComparison.OrdinalIgnoreCase);
    }

    // 17: Both contain Email telemetry terms
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainEmailTelemetryTerms(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("Delivery", decoded);
        Assert.Contains("Bounce", decoded);
        Assert.Contains("Open", decoded);
        Assert.Contains("Click", decoded);
    }

    // 18: Both state delivery does not guarantee commercial outcome
    [Theory]
    [InlineData("/kanallar/email", "Teknik gönderim, müşteri davranışını garanti etmez.")]
    [InlineData("/en/channels/email", "Technical delivery does not guarantee customer behavior.")]
    public async Task BothPages_StateDeliveryDoesNotGuaranteeOutcome(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 19: Both state inbox placement is not guaranteed
    [Theory]
    [InlineData("/kanallar/email", "C15 belirli bir inbox placement")]
    [InlineData("/en/channels/email", "C15 does not guarantee a specific inbox-placement")]
    public async Task BothPages_StateInboxPlacementNotGuaranteed(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 20: View contains no ViewData overrides, no JsonLd section, no /wiki/, no <img
    [Fact]
    public void ViewSource_ContainsNoViewDataOverridesOrWikiLinksOrImgTags()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "EmailMarketing.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("<img", content);
    }

    // 21: No screenshots in rendered HTML
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
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

    // 22: No positive speed claims
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainNoPositiveSpeedClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenSpeedPatterns = new[]
        {
            @"\bsaniyeler içinde\b",
            @"\bin seconds\b",
            @"\bdakikalar içinde\b",
            @"\bin minutes\b",
            @"\binstant delivery\b",
            @"\banında gönderim\b",
            @"\banında tetikleyin\b",
            @"\btrigger instantly\b",
            @"\bfastest\b",
            @"\ben hızlı\b"
        };

        foreach (var pattern in forbiddenSpeedPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden speed claim matching '{pattern}'");
        }
    }

    // 23: No positive best-time claims
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainNoPositiveBestTimeClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenTimePatterns = new[]
        {
            @"\bhighest open rate\b",
            @"\ben yüksek açılma\b",
            @"\bbest send time\b",
            @"\bideal send time\b",
            @"\boptimal send time\b",
            @"\bideal gönderim\b"
        };

        foreach (var pattern in forbiddenTimePatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden time claim matching '{pattern}'");
        }
    }

    // 24: No positive conversion claims
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainNoPositiveConversionClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenConversionPatterns = new[]
        {
            @"\bdönüşümü artırın\b",
            @"\bincrease conversions\b",
            @"\bhigh-conversion\b",
            @"\bdönüşüm garantileyen\b",
            @"\bguaranteed conversion\b",
            @"\bguaranteed engagement\b",
            @"\bguaranteed opens\b",
            @"\bguaranteed clicks\b",
            @"\bguaranteed revenue\b",
            @"\binbox guarantee\b",
            @"\bguaranteed inbox placement\b"
        };

        foreach (var pattern in forbiddenConversionPatterns)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden conversion claim matching '{pattern}'");
        }
    }

    // 25: No positive SPF, DKIM, DMARC, dedicated IP, spam-score claims
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
    public async Task BothPages_ContainNoUnverifiedEmailInfrastructureClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        var forbiddenTerms = new[]
        {
            @"\bSPF\b",
            @"\bDKIM\b",
            @"\bDMARC\b",
            @"\bdedicated IP\b",
            @"\bspam-score\b",
            @"\bspam checker\b",
            @"\binbox optimization\b"
        };

        foreach (var pattern in forbiddenTerms)
        {
            Assert.False(Regex.IsMatch(body, pattern, RegexOptions.IgnoreCase),
                $"Page contains forbidden infrastructure claim matching '{pattern}'");
        }
    }

    // 26: No fake percentages or metrics
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
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

    // 27: No fixed pricing
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
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

    // 28: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_ContainsExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/email");

        var expectedQuestions = new[]
        {
            "Pika Email nedir?",
            "Pika ile görsel Email şablonları hazırlanabilir mi?",
            "Email içeriği kişiselleştirilebilir mi?",
            "Email Campaign Manager ve Journey Manager ile nasıl çalışır?",
            "Email iletişiminde opt-out ve unsubscribe kontrolü var mı?",
            "Pika Email hangi sonuçları ölçebilir?",
            "Email gönderimi inbox, açılma veya dönüşüm garantisi verir mi?",
            "Pika Email yapay zekâ ile kampanyayı kendi başına oluşturup gönderir mi?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, html);
        }

        var faqSummaryMatches = Regex.Matches(html, @"<summary[^>]*class=""email-faq-summary""[^>]*>([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, faqSummaryMatches.Count);
    }

    // 29: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_ContainsExact8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/email");

        var expectedQuestions = new[]
        {
            "What is Pika Email?",
            "Can visual Email templates be prepared in Pika?",
            "Can Email content be personalized?",
            "How does Email work with Campaign Manager and Journey Manager?",
            "Does Email communication include opt-out and unsubscribe handling?",
            "Which Email outcomes can Pika measure?",
            "Does Email delivery guarantee inbox placement, opens or conversion?",
            "Does Pika Email use AI to create and send campaigns by itself?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, html);
        }

        var faqSummaryMatches = Regex.Matches(html, @"<summary[^>]*class=""email-faq-summary""[^>]*>([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, faqSummaryMatches.Count);
    }

    // 30: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
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

    // 31: Correct contextual internal links
    [Fact]
    public async Task TurkishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kanallar/email");

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/platform/gunun-firsatlari\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/content-studio\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/cozumler/consent-management\"", html);
        Assert.Contains("href=\"/cozumler/analytics-reporting\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/channels/email");

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/platform/customer-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/product-intelligence\"", html);
        Assert.Contains("href=\"/en/platform/opportunities\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/content-studio\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/solutions/consent-management\"", html);
        Assert.Contains("href=\"/en/solutions/analytics-reporting\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
    }

    // 32: Exact pricing lines
    [Theory]
    [InlineData("/kanallar/email", "İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.")]
    [InlineData("/en/channels/email", "Pricing is tailored to your requirements and scope of use.")]
    public async Task BothPages_ContainExactPricingLine(string url, string expectedLine)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedLine, decoded);
    }

    // 33: Zero Wiki links
    [Theory]
    [InlineData("/kanallar/email")]
    [InlineData("/en/channels/email")]
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
