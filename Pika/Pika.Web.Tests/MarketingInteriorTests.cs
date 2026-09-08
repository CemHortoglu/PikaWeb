using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Pika.Web.Tests;

public class MarketingInteriorTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;
    public MarketingInteriorTests(WebApplicationFactory<Program> factory) => this.factory = factory;
    public static IEnumerable<object[]> Pages()
    {
        var pairs = new[] {
            ("/platform/customer-intelligence", "/en/platform/customer-intelligence"),
            ("/platform/product-intelligence", "/en/platform/product-intelligence"),
            ("/platform/pika-360", "/en/platform/pika-360"),
            ("/platform/gunun-firsatlari", "/en/platform/opportunities"),
            ("/cozumler/campaign-manager", "/en/solutions/campaign-manager"),
            ("/cozumler/journey-manager", "/en/solutions/journey-manager"),
            ("/cozumler/audience-manager", "/en/solutions/audience-manager"),
            ("/cozumler/content-studio", "/en/solutions/content-studio"),
            ("/cozumler/analytics-reporting", "/en/solutions/analytics-reporting"),
            ("/cozumler/consent-management", "/en/solutions/consent-management"),
            ("/entegrasyonlar", "/en/integrations"),
            ("/guvenlik-ve-gizlilik", "/en/security-and-privacy"),
            ("/urunler/ai-kampanya-asistani", "/en/products/ai-campaign-assistant"),
            ("/kanallar/whatsapp", "/en/channels/whatsapp"),
            ("/kanallar/sms", "/en/channels/sms"),
            ("/kanallar/email", "/en/channels/email"),
            ("/pika", "/en/pika"),
            ("/kullanim-senaryolari", "/en/use-cases"),
            ("/kaynaklar/sss", "/en/resources/faq"),
            ("/kariyer", "/en/careers")
        };
        foreach (var pair in pairs) { yield return new object[] { pair.Item1, "tr" }; yield return new object[] { pair.Item2, "en" }; }
    }

    [Theory, MemberData(nameof(Pages))]
    public async Task MenuDestination_PreservesLanguageHeadingAndWorkingContentAnchor(string path, string language)
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains($"<html lang=\"{language}\"", html);
        Assert.Single(Regex.Matches(html, "<h1(?:\\s|>)"));
        Assert.Contains("id=\"inner-content\"", html);
        Assert.Contains("href=\"#inner-content\"", html);
        Assert.Contains("/css/pika-inner.css", html);
        Assert.Contains("/css/pika-interior-brand.css", html);
        var schemas = Regex.Matches(html, "<script type=\"application/ld(?:\\+|&#x2B;)json\">(.*?)</script>", RegexOptions.Singleline)
            .Select(match => JsonDocument.Parse(match.Groups[1].Value)).ToList();
        try
        {
            var page = Assert.Single(schemas.Where(schema => schema.RootElement.TryGetProperty("@type", out var type) && type.GetString() == "WebPage")).RootElement;
            Assert.Equal(language, page.GetProperty("inLanguage").GetString());
            Assert.Equal(Regex.Match(html, "<link rel=\"canonical\" href=\"([^\"]+)\"").Groups[1].Value, page.GetProperty("url").GetString());
            Assert.False(string.IsNullOrWhiteSpace(page.GetProperty("description").GetString()));
        }
        finally { schemas.ForEach(schema => schema.Dispose()); }
        Assert.Contains("/js/pika-inner.js", html);
        Assert.Contains("rel=\"canonical\"", html);
        if (!path.Contains("consent-management"))
        {
            Assert.Contains(language == "tr" ? "Örnek görünüm" : "Illustration", html);
        }
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/en/")]
    public async Task Homepage_DoesNotLoadInteriorAssets(string path)
    {
        using var client = factory.CreateClient();
        var html = await client.GetStringAsync(path);
        Assert.DoesNotContain("/css/pika-inner.css", html);
        // The shared layout may evolve for interiors; homepage output stays isolated.
        Assert.DoesNotContain("/css/pika-interior-brand.css", html);
        Assert.DoesNotContain("#webpage", html);
        Assert.DoesNotContain("/js/pika-inner.js", html);
        Assert.DoesNotContain("class=\"inner-hero", html);
    }

    [Theory]
    [InlineData("/iletisim", "contactForm")]
    [InlineData("/en/contact", "contactForm")]
    [InlineData("/demo-talebi", "pageDemoRequestForm")]
    [InlineData("/en/demo-request", "pageDemoRequestForm")]
    public async Task ContactForms_KeepSubmissionFields(string path, string formId)
    {
        using var client = factory.CreateClient();
        var html = await client.GetStringAsync(path);
        Assert.Contains($"id=\"{formId}\"", html);
        Assert.Contains("name=\"email\"", html);
        Assert.Contains("name=\"message\"", html);
        Assert.Contains("type=\"submit\"", html);
    }
}
