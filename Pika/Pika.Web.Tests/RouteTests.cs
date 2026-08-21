using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Pika.Web.Tests;

public class RouteTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RouteTests(WebApplicationFactory<Program> factory)
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

    // ==========================================
    // 1. CANONICAL HOMEPAGE TESTS (TR & EN)
    // ==========================================

    [Fact]
    public async Task RootUrl_Returns200_WithTurkishCanonicalAndHreflang()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<html lang=\"tr\"", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/\" />", html);
        Assert.Contains("<link rel=\"alternate\" hreflang=\"tr\" href=\"https://pika.tr/\" />", html);
        Assert.Contains("<link rel=\"alternate\" hreflang=\"en\" href=\"https://pika.tr/en/\" />", html);
        Assert.Contains("<link rel=\"alternate\" hreflang=\"x-default\" href=\"https://pika.tr/\" />", html);
    }

    [Theory]
    [InlineData("/tr")]
    [InlineData("/tr/")]
    [InlineData("/tr/home")]
    [InlineData("/tr/home/")]
    [InlineData("/tr/home/index")]
    [InlineData("/tr/Home")]
    [InlineData("/home")]
    [InlineData("/home/index")]
    [InlineData("/Home/Index")]
    public async Task LegacyTurkishHomepageUrls_Return301_RedirectingDirectlyToRoot(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal("/", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task LegacyTurkishHomepageUrl_PreservesQueryString()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/tr/home?utm_source=google&utm_campaign=summer");

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal("/?utm_source=google&utm_campaign=summer", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task EnglishHomepageWithTrailingSlash_Returns200_WithEnglishCanonical()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("<html lang=\"en\"", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://pika.tr/en/\" />", html);
        Assert.Contains("<link rel=\"alternate\" hreflang=\"tr\" href=\"https://pika.tr/\" />", html);
        Assert.Contains("<link rel=\"alternate\" hreflang=\"en\" href=\"https://pika.tr/en/\" />", html);
        Assert.Contains("<link rel=\"alternate\" hreflang=\"x-default\" href=\"https://pika.tr/\" />", html);
    }

    [Fact]
    public async Task EnglishHomepageWithoutTrailingSlash_Returns301_ToTrailingSlash()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en");

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal("/en/", response.Headers.Location?.OriginalString);
    }

    [Theory]
    [InlineData("/en/home")]
    [InlineData("/en/home/")]
    [InlineData("/en/home/index")]
    [InlineData("/en/Home")]
    [InlineData("/en/Home/Index")]
    public async Task LegacyEnglishHomepageUrls_Return301_RedirectingToEnglishRoot(string url)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal("/en/", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task DomainNormalization_RedirectsWwwToApexDomainWith301()
    {
        var client = CreateNoRedirectClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Host = "www.pika.tr";

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal("https://pika.tr/", response.Headers.Location?.OriginalString);
    }

    // ==========================================
    // 2. DECOUPLED SEMANTIC MARKETING PAGES (TR & EN)
    // ==========================================

    [Theory]
    [InlineData("/pika", "Hakkımızda", "https://pika.tr/pika", "https://pika.tr/en/pika")]
    [InlineData("/kurumsal", "Kurumsal", "https://pika.tr/kurumsal", "https://pika.tr/en/corporate")]
    [InlineData("/kaynaklar/sss", "Sıkça Sorulan Sorular", "https://pika.tr/kaynaklar/sss", "https://pika.tr/en/resources/faq")]
    [InlineData("/iletisim", "İletişim", "https://pika.tr/iletisim", "https://pika.tr/en/contact")]
    [InlineData("/kariyer", "Kariyer", "https://pika.tr/kariyer", "https://pika.tr/en/careers")]
    [InlineData("/demo-talebi", "Demo Talebi", "https://pika.tr/demo-talebi", "https://pika.tr/en/demo-request")]
    [InlineData("/kullanim-sartlari", "Kullanım Şartları", "https://pika.tr/kullanim-sartlari", "https://pika.tr/en/terms-of-use")]
    [InlineData("/gizlilik-politikasi", "Gizlilik Politikası", "https://pika.tr/gizlilik-politikasi", "https://pika.tr/en/privacy-policy")]
    [InlineData("/platform/customer-intelligence", "Customer Intelligence", "https://pika.tr/platform/customer-intelligence", "https://pika.tr/en/platform/customer-intelligence")]
    [InlineData("/platform/product-intelligence", "Product Intelligence", "https://pika.tr/platform/product-intelligence", "https://pika.tr/en/platform/product-intelligence")]
    [InlineData("/platform/pika-360", "Pika 360", "https://pika.tr/platform/pika-360", "https://pika.tr/en/platform/pika-360")]
    [InlineData("/platform/firsatlar", "Günün Fırsatları", "https://pika.tr/platform/firsatlar", "https://pika.tr/en/platform/opportunities")]
    [InlineData("/cozumler/campaign-manager", "Campaign Manager", "https://pika.tr/cozumler/campaign-manager", "https://pika.tr/en/solutions/campaign-manager")]
    [InlineData("/cozumler/audience-manager", "Audience Manager", "https://pika.tr/cozumler/audience-manager", "https://pika.tr/en/solutions/audience-manager")]
    [InlineData("/cozumler/journey-manager", "Journey Manager", "https://pika.tr/cozumler/journey-manager", "https://pika.tr/en/solutions/journey-manager")]
    [InlineData("/cozumler/content-studio", "Content Studio", "https://pika.tr/cozumler/content-studio", "https://pika.tr/en/solutions/content-studio")]
    [InlineData("/cozumler/consent-management", "Consent Management", "https://pika.tr/cozumler/consent-management", "https://pika.tr/en/solutions/consent-management")]
    [InlineData("/kanallar/email", "Email Marketing", "https://pika.tr/kanallar/email", "https://pika.tr/en/channels/email")]
    [InlineData("/kanallar/sms", "SMS Campaigns", "https://pika.tr/kanallar/sms", "https://pika.tr/en/channels/sms")]
    [InlineData("/kanallar/whatsapp", "WhatsApp Messaging", "https://pika.tr/kanallar/whatsapp", "https://pika.tr/en/channels/whatsapp")]
    [InlineData("/kanallar/push-notification", "Push Notifications", "https://pika.tr/kanallar/push-notification", "https://pika.tr/en/channels/push-notifications")]
    [InlineData("/cozumler/analytics-reporting", "Analytics & Reporting", "https://pika.tr/cozumler/analytics-reporting", "https://pika.tr/en/solutions/analytics-reporting")]
    [InlineData("/entegrasyonlar", "Entegrasyonlar", "https://pika.tr/entegrasyonlar", "https://pika.tr/en/integrations")]
    [InlineData("/guvenlik-ve-gizlilik", "Güvenlik ve Gizlilik", "https://pika.tr/guvenlik-ve-gizlilik", "https://pika.tr/en/security-and-privacy")]
    [InlineData("/urunler/ai-kampanya-asistani", "AI Kampanya Asistanı", "https://pika.tr/urunler/ai-kampanya-asistani", "https://pika.tr/en/products/ai-campaign-assistant")]
    [InlineData("/kullanim-senaryolari", "Kullanım Senaryoları", "https://pika.tr/kullanim-senaryolari", "https://pika.tr/en/use-cases")]
    public async Task TurkishSemanticPages_Return200_WithExactCanonicalAndHreflang(
        string path, string titleKeyword, string expectedTrCanonical, string expectedEnAlternate)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains(titleKeyword, html);
        Assert.Contains($"<link rel=\"canonical\" href=\"{expectedTrCanonical}\" />", html);
        Assert.Contains($"<link rel=\"alternate\" hreflang=\"tr\" href=\"{expectedTrCanonical}\" />", html);
        Assert.Contains($"<link rel=\"alternate\" hreflang=\"en\" href=\"{expectedEnAlternate}\" />", html);
        Assert.Contains($"<link rel=\"alternate\" hreflang=\"x-default\" href=\"{expectedTrCanonical}\" />", html);
    }

    [Theory]
    [InlineData("/en/pika", "About Us", "https://pika.tr/en/pika", "https://pika.tr/pika")]
    [InlineData("/en/corporate", "Corporate", "https://pika.tr/en/corporate", "https://pika.tr/kurumsal")]
    [InlineData("/en/resources/faq", "Frequently Asked Questions", "https://pika.tr/en/resources/faq", "https://pika.tr/kaynaklar/sss")]
    [InlineData("/en/contact", "Contact", "https://pika.tr/en/contact", "https://pika.tr/iletisim")]
    [InlineData("/en/careers", "Careers", "https://pika.tr/en/careers", "https://pika.tr/kariyer")]
    [InlineData("/en/demo-request", "Request a Demo", "https://pika.tr/en/demo-request", "https://pika.tr/demo-talebi")]
    [InlineData("/en/terms-of-use", "Terms of Use", "https://pika.tr/en/terms-of-use", "https://pika.tr/kullanim-sartlari")]
    [InlineData("/en/privacy-policy", "Privacy Policy", "https://pika.tr/en/privacy-policy", "https://pika.tr/gizlilik-politikasi")]
    [InlineData("/en/platform/customer-intelligence", "Customer Intelligence", "https://pika.tr/en/platform/customer-intelligence", "https://pika.tr/platform/customer-intelligence")]
    [InlineData("/en/platform/product-intelligence", "Product Intelligence", "https://pika.tr/en/platform/product-intelligence", "https://pika.tr/platform/product-intelligence")]
    [InlineData("/en/platform/pika-360", "Pika 360", "https://pika.tr/en/platform/pika-360", "https://pika.tr/platform/pika-360")]
    [InlineData("/en/platform/opportunities", "Daily Opportunities", "https://pika.tr/en/platform/opportunities", "https://pika.tr/platform/firsatlar")]
    [InlineData("/en/solutions/campaign-manager", "Campaign Manager", "https://pika.tr/en/solutions/campaign-manager", "https://pika.tr/cozumler/campaign-manager")]
    [InlineData("/en/channels/whatsapp", "WhatsApp Messaging", "https://pika.tr/en/channels/whatsapp", "https://pika.tr/kanallar/whatsapp")]
    [InlineData("/en/channels/email", "Email Marketing", "https://pika.tr/en/channels/email", "https://pika.tr/kanallar/email")]
    [InlineData("/en/channels/sms", "SMS Campaigns", "https://pika.tr/en/channels/sms", "https://pika.tr/kanallar/sms")]
    public async Task EnglishSemanticPages_Return200_WithExactCanonicalAndHreflang(
        string path, string titleKeyword, string expectedEnCanonical, string expectedTrAlternate)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains(titleKeyword, html);
        Assert.Contains($"<link rel=\"canonical\" href=\"{expectedEnCanonical}\" />", html);
        Assert.Contains($"<link rel=\"alternate\" hreflang=\"en\" href=\"{expectedEnCanonical}\" />", html);
        Assert.Contains($"<link rel=\"alternate\" hreflang=\"tr\" href=\"{expectedTrAlternate}\" />", html);
    }

    // ==========================================
    // 3. 301 REDIRECTION OF ALL LEGACY MVC-SHAPED ROUTES
    // ==========================================

    [Theory]
    // Home Controller legacy routes
    [InlineData("/Home/Pika", "/pika")]
    [InlineData("/home/pika", "/pika")]
    [InlineData("/tr/home/pika", "/pika")]
    [InlineData("/tr/Home/Pika", "/pika")]
    [InlineData("/tr/pika", "/pika")]
    [InlineData("/Home/Faq", "/kaynaklar/sss")]
    [InlineData("/home/faq", "/kaynaklar/sss")]
    [InlineData("/home/sss", "/kaynaklar/sss")]
    [InlineData("/tr/home/sss", "/kaynaklar/sss")]
    [InlineData("/tr/home/faq", "/kaynaklar/sss")]
    [InlineData("/Home/Corporate", "/kurumsal")]
    [InlineData("/home/corporate", "/kurumsal")]
    [InlineData("/tr/home/corporate", "/kurumsal")]
    [InlineData("/Home/Contact", "/iletisim")]
    [InlineData("/home/contact", "/iletisim")]
    [InlineData("/tr/home/contact", "/iletisim")]
    [InlineData("/Home/Career", "/kariyer")]
    [InlineData("/home/career", "/kariyer")]
    [InlineData("/tr/home/kariyer", "/kariyer")]
    [InlineData("/Home/DemoRequest", "/demo-talebi")]
    [InlineData("/home/demo-talebi", "/demo-talebi")]
    [InlineData("/tr/home/demo-talebi", "/demo-talebi")]
    [InlineData("/Home/TermsOfUse", "/kullanim-sartlari")]
    [InlineData("/home/terms-of-use", "/kullanim-sartlari")]
    [InlineData("/tr/home/kullanim-sartlari", "/kullanim-sartlari")]
    [InlineData("/Home/PrivacyPolicy", "/gizlilik-politikasi")]
    [InlineData("/home/privacy-policy", "/gizlilik-politikasi")]
    [InlineData("/tr/home/gizlilik-politikasi", "/gizlilik-politikasi")]

    // Platform Controller legacy routes
    [InlineData("/Platform/CustomerIntelligence", "/platform/customer-intelligence")]
    [InlineData("/platform/customerintelligence", "/platform/customer-intelligence")]
    [InlineData("/tr/platform/customer-intelligence", "/platform/customer-intelligence")]
    [InlineData("/Platform/ProductIntelligence", "/platform/product-intelligence")]
    [InlineData("/tr/platform/product-intelligence", "/platform/product-intelligence")]
    [InlineData("/Platform/Pika360", "/platform/pika-360")]
    [InlineData("/tr/platform/pika-360", "/platform/pika-360")]
    [InlineData("/Platform/Opportunities", "/platform/firsatlar")]
    [InlineData("/tr/platform/firsatlar", "/platform/firsatlar")]
    [InlineData("/tr/platform/opportunities", "/platform/firsatlar")]

    // Solutions Controller legacy routes
    [InlineData("/Solutions/CampaignManager", "/cozumler/campaign-manager")]
    [InlineData("/solutions/campaign-manager", "/cozumler/campaign-manager")]
    [InlineData("/tr/solutions/campaign-manager", "/cozumler/campaign-manager")]
    [InlineData("/Solutions/AudienceManager", "/cozumler/audience-manager")]
    [InlineData("/tr/solutions/audience-manager", "/cozumler/audience-manager")]
    [InlineData("/Solutions/JourneyManager", "/cozumler/journey-manager")]
    [InlineData("/tr/solutions/journey-manager", "/cozumler/journey-manager")]
    [InlineData("/Solutions/ContentStudio", "/cozumler/content-studio")]
    [InlineData("/tr/solutions/content-studio", "/cozumler/content-studio")]
    [InlineData("/Solutions/ConsentManagement", "/cozumler/consent-management")]
    [InlineData("/tr/solutions/consent-management", "/cozumler/consent-management")]
    [InlineData("/Solutions/WhatsAppMessaging", "/kanallar/whatsapp")]
    [InlineData("/solutions/whatsapp-messaging", "/kanallar/whatsapp")]
    [InlineData("/tr/solutions/whatsapp-messaging", "/kanallar/whatsapp")]
    [InlineData("/Solutions/EmailMarketing", "/kanallar/email")]
    [InlineData("/solutions/email-marketing", "/kanallar/email")]
    [InlineData("/tr/solutions/email-marketing", "/kanallar/email")]
    [InlineData("/Solutions/SmsCampaigns", "/kanallar/sms")]
    [InlineData("/solutions/sms-campaigns", "/kanallar/sms")]
    [InlineData("/tr/solutions/sms-campaigns", "/kanallar/sms")]
    [InlineData("/Solutions/PushNotifications", "/kanallar/push-notification")]
    [InlineData("/solutions/push-notifications", "/kanallar/push-notification")]
    [InlineData("/tr/solutions/push-notifications", "/kanallar/push-notification")]
    [InlineData("/Solutions/Reporting", "/cozumler/analytics-reporting")]
    [InlineData("/solutions/reporting", "/cozumler/analytics-reporting")]
    [InlineData("/tr/solutions/reporting", "/cozumler/analytics-reporting")]
    [InlineData("/tr/solutions/analytics-reporting", "/cozumler/analytics-reporting")]
    [InlineData("/Solutions/Integrations", "/entegrasyonlar")]
    [InlineData("/solutions/integrations", "/entegrasyonlar")]
    [InlineData("/tr/solutions/integrations", "/entegrasyonlar")]
    [InlineData("/Solutions/SecurityPrivacy", "/guvenlik-ve-gizlilik")]
    [InlineData("/solutions/security-privacy", "/guvenlik-ve-gizlilik")]
    [InlineData("/tr/solutions/security-privacy", "/guvenlik-ve-gizlilik")]
    [InlineData("/Solutions/AiCampaignAssistant", "/urunler/ai-kampanya-asistani")]
    [InlineData("/tr/products/ai-campaign-assistant", "/urunler/ai-kampanya-asistani")]
    [InlineData("/Solutions/UseCases", "/kullanim-senaryolari")]
    [InlineData("/tr/kullanim-senaryolari", "/kullanim-senaryolari")]

    // English legacy MVC routes
    [InlineData("/en/Home/Pika", "/en/pika")]
    [InlineData("/en/home/pika", "/en/pika")]
    [InlineData("/en/about-pika", "/en/pika")]
    [InlineData("/en/Home/Faq", "/en/resources/faq")]
    [InlineData("/en/home/faq", "/en/resources/faq")]
    [InlineData("/en/home/sss", "/en/resources/faq")]
    [InlineData("/en/Home/Corporate", "/en/corporate")]
    [InlineData("/en/home/corporate", "/en/corporate")]
    [InlineData("/en/Home/Contact", "/en/contact")]
    [InlineData("/en/home/contact", "/en/contact")]
    [InlineData("/en/Home/Career", "/en/careers")]
    [InlineData("/en/home/career", "/en/careers")]
    [InlineData("/en/Home/DemoRequest", "/en/demo-request")]
    [InlineData("/en/home/demo-request", "/en/demo-request")]
    [InlineData("/en/Solutions/WhatsAppMessaging", "/en/channels/whatsapp")]
    [InlineData("/en/solutions/whatsapp-messaging", "/en/channels/whatsapp")]
    [InlineData("/en/Solutions/EmailMarketing", "/en/channels/email")]
    [InlineData("/en/solutions/email-marketing", "/en/channels/email")]
    [InlineData("/en/Solutions/SmsCampaigns", "/en/channels/sms")]
    [InlineData("/en/solutions/sms-campaigns", "/en/channels/sms")]
    [InlineData("/en/Solutions/PushNotifications", "/en/channels/push-notifications")]
    [InlineData("/en/solutions/push-notifications", "/en/channels/push-notifications")]
    [InlineData("/en/Solutions/CampaignManager", "/en/solutions/campaign-manager")]
    [InlineData("/en/Platform/CustomerIntelligence", "/en/platform/customer-intelligence")]
    public async Task LegacyMvcRoutes_Return301_DirectlyToSemanticCanonical(string legacyUrl, string expectedCanonical)
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync(legacyUrl);

        Assert.Equal(HttpStatusCode.MovedPermanently, response.StatusCode);
        Assert.Equal(expectedCanonical, response.Headers.Location?.OriginalString);
    }

    // ==========================================
    // 4. SITEMAP & ROBOTS VERIFICATION
    // ==========================================

    [Fact]
    public async Task SitemapXml_Returns200_AndContainsOnlyValidCanonicalUrls()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/sitemap.xml");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var xmlString = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(xmlString);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locs = doc.Descendants(ns + "loc").Select(x => x.Value).ToList();

        Assert.NotEmpty(locs);
        Assert.Contains("https://pika.tr/", locs);
        Assert.Contains("https://pika.tr/en/", locs);
        Assert.Contains("https://pika.tr/pika", locs);
        Assert.Contains("https://pika.tr/en/pika", locs);
        Assert.Contains("https://pika.tr/platform/customer-intelligence", locs);
        Assert.Contains("https://pika.tr/en/platform/customer-intelligence", locs);
        Assert.Contains("https://pika.tr/kanallar/whatsapp", locs);
        Assert.Contains("https://pika.tr/en/channels/whatsapp", locs);

        // Disallowed / legacy paths in sitemap
        Assert.DoesNotContain("https://pika.tr/tr/", locs);
        Assert.DoesNotContain("https://pika.tr/tr/home", locs);
        Assert.DoesNotContain("https://pika.tr/Home/Pika", locs);
        Assert.DoesNotContain("https://pika.tr/Solutions/", locs);
        Assert.DoesNotContain("https://pika.tr/Account/Login", locs);
        Assert.DoesNotContain("https://pika.tr/error/404", locs);
        Assert.DoesNotContain("/internal/", string.Join(",", locs));
    }

    [Fact]
    public async Task AllSitemapUrls_Return200OK()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/sitemap.xml");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var xmlString = await response.Content.ReadAsStringAsync();
        var doc = XDocument.Parse(xmlString);
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locs = doc.Descendants(ns + "loc").Select(x => x.Value).ToList();

        foreach (var loc in locs)
        {
            var uri = new Uri(loc);
            var path = uri.PathAndQuery;
            var pageResponse = await client.GetAsync(path);
            Assert.True(pageResponse.StatusCode == HttpStatusCode.OK,
                $"Sitemap URL '{loc}' returned {pageResponse.StatusCode} instead of 200 OK");
        }
    }

    [Fact]
    public async Task RobotsTxt_Returns200_AndContainsSitemapDirective()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/robots.txt");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Sitemap: https://pika.tr/sitemap.xml", content);
        Assert.Contains("Disallow: /Account/", content);
        Assert.Contains("Disallow: /Auth/", content);
        Assert.Contains("Disallow: /api/", content);
        Assert.Contains("Disallow: /internal/", content);
    }

    [Fact]
    public async Task UnknownRoute_Returns404NotFound_WithNoindexAndNoCanonicalToHome()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/this-is-an-unknown-path-for-testing-404");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("name=\"robots\" content=\"noindex, nofollow\"", html);
        Assert.DoesNotContain("<link rel=\"canonical\" href=\"https://pika.tr/\"", html);
    }
}
