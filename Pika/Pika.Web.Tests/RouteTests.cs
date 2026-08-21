using System.Net;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Pika.Web.Tests;

public class RouteTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RouteTests(WebApplicationFactory<Program> factory) => _factory = factory;

    private HttpClient Client() => _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task Root_IsTurkishCanonical_AndContainsProductStoryInInitialHtml()
    {
        var response = await Client().GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<html lang=\"tr\"", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/\"", html);
        Assert.Contains("Doğru Anda", html);
        Assert.Contains("Günün Fırsatları", html);
        Assert.Contains("Customer Intelligence", html);
        Assert.Contains("Product Intelligence", html);
    }

    [Fact]
    public async Task EnglishRoot_IsEnglishCanonical()
    {
        var response = await Client().GetAsync("/en/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<html lang=\"en\"", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/en/\"", html);
    }

    [Theory]
    [InlineData("/pika", "https://pika.tr/pika", "Pika Nedir")]
    [InlineData("/platform/customer-intelligence", "https://pika.tr/platform/customer-intelligence", "Customer Intelligence")]
    [InlineData("/platform/product-intelligence", "https://pika.tr/platform/product-intelligence", "Product Intelligence")]
    [InlineData("/platform/pika-360", "https://pika.tr/platform/pika-360", "Pika 360")]
    [InlineData("/platform/gunun-firsatlari", "https://pika.tr/platform/gunun-firsatlari", "Günün Fırsatları")]
    [InlineData("/platform/audience-manager", "https://pika.tr/platform/audience-manager", "Audience Manager")]
    [InlineData("/platform/campaign-manager", "https://pika.tr/platform/campaign-manager", "Campaign Manager")]
    [InlineData("/platform/journey-manager", "https://pika.tr/platform/journey-manager", "Journey Manager")]
    [InlineData("/platform/content-studio", "https://pika.tr/platform/content-studio", "Content Studio")]
    [InlineData("/platform/analytics", "https://pika.tr/platform/analytics", "Analytics")]
    [InlineData("/platform/ai-kampanya-asistani", "https://pika.tr/platform/ai-kampanya-asistani", "Pika AI")]
    [InlineData("/platform/consent-management", "https://pika.tr/platform/consent-management", "Consent Management")]
    [InlineData("/platform/integrations", "https://pika.tr/platform/integrations", "Entegrasyon")]
    [InlineData("/kanallar/email", "https://pika.tr/kanallar/email", "Email")]
    [InlineData("/kanallar/sms", "https://pika.tr/kanallar/sms", "SMS")]
    [InlineData("/kanallar/whatsapp", "https://pika.tr/kanallar/whatsapp", "WhatsApp")]
    [InlineData("/kanallar/push", "https://pika.tr/kanallar/push", "Push")]
    [InlineData("/kullanim-senaryolari", "https://pika.tr/kullanim-senaryolari", "Kullanım Senaryoları")]
    [InlineData("/guvenlik-ve-gizlilik", "https://pika.tr/guvenlik-ve-gizlilik", "Güvenlik ve Gizlilik")]
    [InlineData("/kaynaklar/sss", "https://pika.tr/kaynaklar/sss", "Sık Sorulan Sorular")]
    public async Task TurkishPublicPages_AreSemantic200AndSelfCanonical(string path, string canonical, string content)
    {
        var response = await Client().GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains($"<link rel=\"canonical\" href=\"{canonical}\"", html);
        Assert.Contains(content, html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("/tr/Home/", html, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("/tr", "/")]
    [InlineData("/tr/home", "/")]
    [InlineData("/Home/Index", "/")]
    [InlineData("/Home/Pika", "/pika")]
    [InlineData("/tr/Home/Pika", "/pika")]
    [InlineData("/Home/Faq", "/kaynaklar/sss")]
    [InlineData("/tr/Home/Faq", "/kaynaklar/sss")]
    [InlineData("/tr/kullanim-senaryolari", "/kullanim-senaryolari")]
    [InlineData("/tr/solutions/security-privacy", "/guvenlik-ve-gizlilik")]
    [InlineData("/tr/solutions/whatsapp-messaging", "/kanallar/whatsapp")]
    [InlineData("/solutions/email-marketing", "/kanallar/email")]
    [InlineData("/solutions/sms-campaigns", "/kanallar/sms")]
    [InlineData("/solutions/push-notifications", "/kanallar/push")]
    [InlineData("/cozumler/campaign-manager", "/platform/campaign-manager")]
    [InlineData("/cozumler/audience-manager", "/platform/audience-manager")]
    [InlineData("/cozumler/journey-manager", "/platform/journey-manager")]
    [InlineData("/entegrasyonlar", "/platform/integrations")]
    [InlineData("/urunler/ai-kampanya-asistani", "/platform/ai-kampanya-asistani")]
    [InlineData("/platform/firsatlar", "/platform/gunun-firsatlari")]
    public async Task LegacyPublicRoutes_RedirectOneHopToFinalCanonical(string oldPath, string finalPath)
    {
        var response = await Client().GetAsync(oldPath);
        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal(finalPath, response.Headers.Location?.OriginalString);

        var canonicalResponse = await Client().GetAsync(finalPath);
        Assert.Equal(HttpStatusCode.OK, canonicalResponse.StatusCode);
    }

    [Fact]
    public async Task LegacyRedirect_PreservesTrackingQuery()
    {
        var response = await Client().GetAsync("/tr/solutions/whatsapp-messaging?utm_source=chatgpt.com&utm_campaign=test");
        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal("/kanallar/whatsapp?utm_source=chatgpt.com&utm_campaign=test", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task UTM_DoesNotChangeCanonical()
    {
        var response = await Client().GetAsync("/pika?utm_source=chatgpt.com");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/pika\"", html);
        Assert.DoesNotContain("utm_source", html.Substring(Math.Max(0, html.IndexOf("rel=\"canonical\"", StringComparison.Ordinal) - 80), 200), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Sitemap_ContainsOnlyCanonicalPublicArchitecture()
    {
        var response = await Client().GetAsync("/sitemap.xml");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var xml = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(xml);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var urls = doc.Descendants(ns + "loc").Select(x => x.Value).ToArray();

        Assert.Contains("https://pika.tr/", urls);
        Assert.Contains("https://pika.tr/pika", urls);
        Assert.Contains("https://pika.tr/platform/gunun-firsatlari", urls);
        Assert.Contains("https://pika.tr/kanallar/whatsapp", urls);
        Assert.Contains("https://pika.tr/wiki/", urls);
        Assert.DoesNotContain(urls, x => x.Contains("/tr/", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(urls, x => x.Contains("/Home/", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(urls, x => x.Contains("/cozumler/", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(urls, x => x.Contains("/solutions/", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(urls, x => x.Contains("/internal/", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Robots_AllowsPublicAndOaiSearchBot_BlocksInternal()
    {
        var response = await Client().GetAsync("/robots.txt");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var text = await response.Content.ReadAsStringAsync();
        Assert.Contains("User-agent: OAI-SearchBot", text);
        Assert.Contains("Disallow: /internal/", text);
        Assert.Contains("Sitemap: https://pika.tr/sitemap.xml", text);
    }

    [Fact]
    public async Task UnknownPublicUrl_ReturnsReal404AndNoindex()
    {
        var response = await Client().GetAsync("/this-page-does-not-exist-20260821");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("noindex", html, StringComparison.OrdinalIgnoreCase);
    }
}
