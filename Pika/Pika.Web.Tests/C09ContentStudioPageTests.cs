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

public class C09ContentStudioPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C09ContentStudioPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /cozumler/content-studio = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/solutions/content-studio = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsContentStudio_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "ContentStudio");
        Assert.NotNull(entry);

        Assert.Equal("Content Studio | Email Şablonları ve Çok Kanallı İçerik | Pika", entry.TitleTr);
        Assert.Equal("Content Studio | Email Templates & Multi-Channel Content | Pika", entry.TitleEn);
        Assert.Equal("Pika Content Studio; sürükle-bırak email editörü, modüler bloklar, şablonlar, kişiselleştirme alanları ve önizleme ile kampanya içeriklerini hazırlamanıza yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Content Studio helps prepare campaign content with a drag-and-drop email editor, modular blocks, templates, personalization fields and preview.", entry.DescriptionEn);
        Assert.Equal("Content Studio", entry.BreadcrumbTitleTr);
        Assert.Equal("Content Studio", entry.BreadcrumbTitleEn);
    }

    [Fact]
    public async Task TurkishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Content Studio | Email Şablonları ve Çok Kanallı İçerik | Pika", ExtractTitle(html));
        Assert.Equal("Pika Content Studio; sürükle-bırak email editörü, modüler bloklar, şablonlar, kişiselleştirme alanları ve önizleme ile kampanya içeriklerini hazırlamanıza yardımcı olur.", ExtractMetaDescription(html));
    }

    [Fact]
    public async Task EnglishPage_RendersExactTitleAndMetaDescription()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Content Studio | Email Templates & Multi-Channel Content | Pika", ExtractTitle(html));
        Assert.Equal("Pika Content Studio helps prepare campaign content with a drag-and-drop email editor, modular blocks, templates, personalization fields and preview.", ExtractMetaDescription(html));
    }

    // 04: Exactly one H1
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_HaveExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        var matches = Regex.Matches(html, @"<h1[^>]*>.*?</h1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Single(matches);
    }

    // 05: TR normalized H1: İçeriği koddan değil, iletişim amacından başlayarak hazırlayın.
    [Fact]
    public async Task TurkishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("İçeriği koddan değil, iletişim amacından başlayarak hazırlayın.", ExtractH1(html));
    }

    // 06: EN normalized H1: Start content preparation with the communication objective, not with code.
    [Fact]
    public async Task EnglishPage_HasNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal("Start content preparation with the communication objective, not with code.", ExtractH1(html));
    }

    // 07: TR contains: Content Studio nedir?
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Content Studio nedir?", html);
    }

    // 08: EN contains: What is Content Studio?
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("What is Content Studio?", html);
    }

    // 09: Both contain: Content Studio, Audience Manager, Campaign Manager, Journey Manager, Pika Pilot
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainCanonicalEcosystemEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio", decoded);
        Assert.Contains("Audience Manager", decoded);
        Assert.Contains("Campaign Manager", decoded);
        Assert.Contains("Journey Manager", decoded);
        Assert.Contains("Pika Pilot", decoded);
    }

    // 10: Both contain confirmed channels: Email, SMS, WhatsApp
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainConfirmedChannels(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Email", html);
        Assert.Contains("SMS", html);
        Assert.Contains("WhatsApp", html);
    }

    // 11: Rendered body contains ZERO Push / Mobile Push / Web Push
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_RenderedBodyContainsZeroPushMentions(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Mobile Push", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Web Push", body, StringComparison.OrdinalIgnoreCase);
    }

    // 12: Both contain: Templates / Şablonlar, Personalization / Kişiselleştirme, Preview / Önizleme
    [Fact]
    public async Task TurkishPage_ContainsTemplatesPersonalizationPreview()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Şablon", html);
        Assert.Contains("Kişiselleştirme", html);
        Assert.Contains("Önizleme", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsTemplatesPersonalizationPreview()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("Template", html);
        Assert.Contains("Personalization", html);
        Assert.Contains("Preview", html);
    }

    // 13: TR contains: Sürükle-bırak
    [Fact]
    public async Task TurkishPage_ContainsSurukleBirak()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("sürükle-bırak", html, StringComparison.OrdinalIgnoreCase);
    }

    // 14: EN contains: drag-and-drop
    [Fact]
    public async Task EnglishPage_ContainsDragAndDrop()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("drag-and-drop", html, StringComparison.OrdinalIgnoreCase);
    }

    // 15: View contains no legacy tags (ViewData Title, MetaDescription, MetaKeywords, CanonicalUrl, JsonLd, /wiki/, <img)
    [Fact]
    public void ViewFile_UsesEditorialImagesWithoutLegacyTagsOrWikiLinks()
    {
        var solutionDir = GetProjectRoot();
        var viewPath = Path.Combine(solutionDir, "Views", "Solutions", "ContentStudio.cshtml");
        Assert.True(File.Exists(viewPath), $"View file should exist at {viewPath}");

        var content = File.ReadAllText(viewPath);
        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("wikiBase", content);
        Assert.Contains("pika-story-image", content);
    }

    // 16: No real screenshots
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoRealScreenshots(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("img_email-template-editor_17.png", html);
        Assert.DoesNotContain("/wiki/assets/images/", html);
        Assert.Contains("pika-story-image", html);
    }

    // 17: No fake pseudo-editor copy: AI Assistant, AI Asistan, Tasarım Alanı, Design Area
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoFakePseudoEditorCopy(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("AI Assistant", html);
        Assert.DoesNotContain("AI Asistan", html);
        Assert.DoesNotContain("Tasarım Alanı", html);
        Assert.DoesNotContain("Design Area", html);
    }

    // 18: No prohibited speed claims
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoProhibitedSpeedClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("dakikalar içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("saniyeler içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("in minutes", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("in seconds", body, StringComparison.OrdinalIgnoreCase);
    }

    // 19: No Canlı Önizleme / Live Preview
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoLivePreviewClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Canlı Önizleme", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Canlı önizleme", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Live Preview", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Live preview", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("real-time preview", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("instant preview", body, StringComparison.OrdinalIgnoreCase);
    }

    // 20: Page clearly distinguishes: Content Studio vs Campaign Manager
    [Fact]
    public async Task TurkishPage_DistinguishesContentStudioVsCampaignManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio, Campaign Manager değildir.", decoded);
        Assert.Contains("Content Studio içeriği hazırlar. Campaign Manager hedef kitle, içerik, kanal, zamanlama ve kontrollü gönderimi kampanya bağlamında yönetir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesContentStudioVsCampaignManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio is not Campaign Manager.", decoded);
        Assert.Contains("Content Studio prepares content. Campaign Manager manages audience, content, channel, timing and controlled delivery within campaign context.", decoded);
    }

    // 21: Page clearly distinguishes: Content Studio vs Journey Manager
    [Fact]
    public async Task TurkishPage_DistinguishesContentStudioVsJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio, Journey Manager değildir.", decoded);
        Assert.Contains("Content Studio iletişim içeriğinin hazırlanmasına odaklanır. Journey Manager çok adımlı koşul, sıra, bekleme ve iletişim akışını yönetir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesContentStudioVsJourneyManager()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio is not Journey Manager.", decoded);
        Assert.Contains("Content Studio focuses on communication-content preparation. Journey Manager manages multi-step conditions, sequence, waits and communication flow.", decoded);
    }

    // 22: Page clearly distinguishes: Content Studio vs Pika Pilot
    [Fact]
    public async Task TurkishPage_DistinguishesContentStudioVsPikaPilot()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio, Pika Pilot değildir.", decoded);
        Assert.Contains("Pika Pilot AI destekli kampanya ve içerik taslağı hazırlığını hızlandırabilir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_DistinguishesContentStudioVsPikaPilot()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Content Studio is not Pika Pilot.", decoded);
        Assert.Contains("Pika Pilot can accelerate AI-assisted campaign and content-draft preparation.", decoded);
    }

    // 23: Page explains AI draft != publication/send
    [Fact]
    public async Task TurkishPage_ExplainsAiDraftIsNotPublishOrSend()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("AI tarafından oluşturulan içerik önerisi yayınlanmış veya gönderilmiş içerik değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsAiDraftIsNotPublishOrSend()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("AI-generated content suggestions are not published or sent content.", decoded);
    }

    // 24: Page explains preview != identical rendering guarantee
    [Fact]
    public async Task TurkishPage_ExplainsPreviewIsNotIdenticalRenderingGuarantee()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Önizleme gerçek email istemcisinin birebir rendering garantisi değildir.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ExplainsPreviewIsNotIdenticalRenderingGuarantee()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Preview is not a guarantee of identical rendering in every real email client.", decoded);
    }

    // 25: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_RendersExactlyEightCanonicalFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "Content Studio nedir?",
            "Content Studio ile kod yazmadan email hazırlanabilir mi?",
            "Content Studio şablonları yeniden kullanabilir mi?",
            "Content Studio kişiselleştirilmiş içerik hazırlayabilir mi?",
            "Content Studio email içeriğini mobilde önizleyebilir mi?",
            "Content Studio ile Pika Pilot arasındaki fark nedir?",
            "Content Studio içerikleri doğrudan gönderir mi?",
            "Content Studio hangi iletişim kanalları için içerik hazırlamaya yardımcı olur?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var summaryMatches = Regex.Matches(html, @"<summary[^>]*>(.*?)</summary>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 26: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_RendersExactlyEightCanonicalFaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        var expectedQuestions = new[]
        {
            "What is Content Studio?",
            "Can Content Studio prepare email without writing code?",
            "Can Content Studio reuse templates?",
            "Can Content Studio prepare personalized content?",
            "Can Content Studio preview email content on mobile?",
            "What is the difference between Content Studio and Pika Pilot?",
            "Does Content Studio send content directly?",
            "Which communication channels can Content Studio help prepare content for?"
        };

        foreach (var q in expectedQuestions)
        {
            Assert.Contains(q, decoded);
        }

        var summaryMatches = Regex.Matches(html, @"<summary[^>]*>(.*?)</summary>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Assert.Equal(8, summaryMatches.Count);
    }

    // 27: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoPageLevelProhibitedJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("\"FAQPage\"", html);
        Assert.DoesNotContain("\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"Offer\"", html);
    }

    // 28: Correct internal links
    [Fact]
    public async Task TurkishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/pika\"", html);
        Assert.Contains("href=\"/cozumler/audience-manager\"", html);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", html);
        Assert.Contains("href=\"/cozumler/journey-manager\"", html);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", html);
        Assert.Contains("href=\"/kanallar/email\"", html);
        Assert.Contains("href=\"/kanallar/sms\"", html);
        Assert.Contains("href=\"/kanallar/whatsapp\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsAllRequiredInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("href=\"/en/pika\"", html);
        Assert.Contains("href=\"/en/solutions/audience-manager\"", html);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", html);
        Assert.Contains("href=\"/en/solutions/journey-manager\"", html);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", html);
        Assert.Contains("href=\"/en/channels/email\"", html);
        Assert.Contains("href=\"/en/channels/sms\"", html);
        Assert.Contains("href=\"/en/channels/whatsapp\"", html);
    }

    // 29: Exact pricing copy in TR & EN
    [Fact]
    public async Task TurkishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/cozumler/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_ContainsExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/solutions/content-studio");
        var html = await response.Content.ReadAsStringAsync();
        var decoded = WebUtility.HtmlDecode(html);

        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 30: No fixed public pricing
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoFixedPriceOrFreeTrialClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("₺", body);
        Assert.DoesNotContain("$", body);
        Assert.DoesNotContain("€", body);
        Assert.DoesNotContain("free trial", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ücretsiz deneme", body, StringComparison.OrdinalIgnoreCase);
    }

    // 31: No fake content metrics
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoFakeContentMetrics(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("guaranteed engagement", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed click rate", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed content performance", body, StringComparison.OrdinalIgnoreCase);
    }

    // 32: No Wiki links
    [Theory]
    [InlineData("/cozumler/content-studio")]
    [InlineData("/en/solutions/content-studio")]
    public async Task BothPages_ContainNoWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);
        var html = await response.Content.ReadAsStringAsync();
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("wikiBase", body, StringComparison.OrdinalIgnoreCase);
    }
}
