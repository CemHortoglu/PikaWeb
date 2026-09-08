using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pika.Configuration;
using Pika.Services;
using Xunit;

namespace Pika.Web.Tests;

public class C22ContactPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C22ContactPageTests(WebApplicationFactory<Program> factory)
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

    private SiteSettings GetSiteSettings()
    {
        using var scope = _factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IOptions<SiteSettings>>().Value;
    }

    // 01: GET /iletisim = 200
    [Fact]
    public async Task TurkishContactPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/iletisim");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/contact = 200
    [Fact]
    public async Task EnglishContactPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/contact");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Home.Contact SEO TR/EN
    [Fact]
    public void HomeContactSeoMetadata_HasExactCanonicalValues()
    {
        var meta = SeoHelper.GetMetadata("Home", "Contact");
        Assert.NotNull(meta);
        Assert.Equal("/iletisim", meta.AlternatePathTr);
        Assert.Equal("/en/contact", meta.AlternatePathEn);
        Assert.Equal("İletişim | Pika Ürün, Demo ve Ticari Değerlendirme", meta.TitleTr);
        Assert.Equal("Contact Pika | Product, Demo & Commercial Evaluation", meta.TitleEn);
        Assert.Equal("Pika ekibine ürün, veri, entegrasyon ve ticari değerlendirme sorularınız için ulaşın. Yapılandırılmış ürün değerlendirmesi için Demo Talebi sayfasını kullanın.", meta.DescriptionTr);
        Assert.Equal("Contact Pika with product, data, integration and commercial-evaluation questions. Use Demo Request for a structured product and usage-scope evaluation.", meta.DescriptionEn);
        Assert.Equal("İletişim", meta.BreadcrumbTitleTr);
        Assert.Equal("Contact", meta.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_HaveExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var matches = Regex.Matches(html, @"<h1\b[^>]*>", RegexOptions.IgnoreCase);
        Assert.Single(matches);
    }

    // 05: Exact normalized TR H1
    [Fact]
    public async Task TurkishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/iletisim");
        var h1 = ExtractH1(html);
        Assert.Equal("Sorunuzu, ihtiyacınızı veya değerlendirmek istediğiniz Pika kapsamını paylaşın.", h1);
    }

    // 06: Exact normalized EN H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/contact");
        var h1 = ExtractH1(html);
        Assert.Equal("Share your question, requirement or the Pika scope you want to evaluate.", h1);
    }

    // 07: Both render id="contactForm"
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_RenderContactFormId(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("id=\"contactForm\"", html);
    }

    // 08: Both render id="contactResult"
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_RenderContactResultId(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("id=\"contactResult\"", html);
    }

    // 09: Form field names remain exactly: fullName, email, message
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainExactFormFieldNames(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("name=\"fullName\"", html);
        Assert.Contains("name=\"email\"", html);
        Assert.Contains("name=\"message\"", html);
    }

    // 10: All three form fields are required
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_HaveAllThreeFieldsRequired(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Matches(@"<input[^>]*name=""fullName""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""email""[^>]*required", html);
        Assert.Matches(@"<textarea[^>]*name=""message""[^>]*required", html);
    }

    // 11: data-recipient-email remains on contact form
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_RenderDataRecipientEmail(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var site = GetSiteSettings();
        Assert.Contains($"data-recipient-email=\"{site.Mail.ContactFormRecipientEmail}\"", html);
    }

    // 12: Direct email uses site.Contact.SupportEmail
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_UseConfiguredSupportEmail(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var site = GetSiteSettings();
        Assert.Contains($"href=\"mailto:{site.Contact.SupportEmail}\"", html);
        Assert.Contains(site.Contact.SupportEmail, html);
    }

    // 13: Phone link uses site.Contact.SupportPhone
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_UseConfiguredSupportPhone(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        var site = GetSiteSettings();
        Assert.Contains($"href=\"tel:{site.Contact.SupportPhone}\"", decoded);
    }

    // 14: Display phone uses site.Contact.SupportPhoneDisplay
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_UseConfiguredSupportPhoneDisplay(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        var site = GetSiteSettings();
        Assert.Contains(site.Contact.SupportPhoneDisplay, decoded);
    }

    // 15: Address uses site.Contact.Address
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_UseConfiguredAddress(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        var site = GetSiteSettings();
        Assert.Contains(site.Contact.Address, decoded);
    }

    // 16: Map remains conditional on site.Contact.MapEmbedUrl
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_RenderMapConditionally(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        var site = GetSiteSettings();
        if (!string.IsNullOrWhiteSpace(site.Contact.MapEmbedUrl))
        {
            Assert.Contains(site.Contact.MapEmbedUrl, decoded);
            Assert.Contains("<iframe", html);
        }
    }

    // 17: Privacy links correct TR/EN
    [Fact]
    public async Task TurkishPage_LinksToTurkishPrivacyPolicy()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/iletisim");
        Assert.Contains("href=\"/gizlilik-politikasi\"", html);
    }

    [Fact]
    public async Task EnglishPage_LinksToEnglishPrivacyPolicy()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/contact");
        Assert.Contains("href=\"/en/privacy-policy\"", html);
    }

    // 18: Both contain canonical Demo Request link
    [Fact]
    public async Task TurkishPage_ContainsCanonicalDemoRequestLink()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/iletisim");
        Assert.Contains("href=\"/demo-talebi\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsCanonicalDemoRequestLink()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/contact");
        Assert.Contains("href=\"/en/demo-request\"", html);
    }

    // 19: Both contain Corporate link
    [Fact]
    public async Task TurkishPage_ContainsCorporateLink()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/iletisim");
        Assert.Contains("href=\"/kurumsal\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsCorporateLink()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/contact");
        Assert.Contains("href=\"/en/corporate\"", html);
    }

    // 20: Both contain FAQ link
    [Fact]
    public async Task TurkishPage_ContainsFaqLink()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/iletisim");
        Assert.Contains("href=\"/kaynaklar/sss\"", html);
    }

    [Fact]
    public async Task EnglishPage_ContainsFaqLink()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/contact");
        Assert.Contains("href=\"/en/resources/faq\"", html);
    }

    // 21: Contact.cshtml contains no data-bs-target="#demoModal"
    [Fact]
    public void ContactView_ContainsNoDemoModalTarget()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Contact.cshtml");
        var viewContent = File.ReadAllText(viewPath);
        Assert.DoesNotContain("data-bs-target=\"#demoModal\"", viewContent);
    }

    // 22: Contact.cshtml contains no data-bs-toggle="modal" for canonical demo CTA
    [Fact]
    public void ContactView_ContainsNoModalToggle()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Contact.cshtml");
        var viewContent = File.ReadAllText(viewPath);
        Assert.DoesNotContain("data-bs-toggle=\"modal\"", viewContent);
    }

    // 23: No fixed response-time claims
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainNoFixedResponseTimeClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("24 saat içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("aynı gün", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("en kısa sürede", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("within 24 hours", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("same day", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("as soon as possible", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("immediately", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("quick response", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("fast response", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ekibimiz sizinle iletişime geçsin", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("our team will get in touch", body, StringComparison.OrdinalIgnoreCase);
    }

    // 24: No unsupported corporate claims (24/7, SLA, SOC 2, ISO 27001, market leader, global customers)
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainNoUnsupportedCorporateClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("24/7", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SLA", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SOC 2", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ISO 27001", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("market leader", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("pazar lideri", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("global customers", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("küresel müşteriler", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed ROI", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed conversion", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("guaranteed revenue", body, StringComparison.OrdinalIgnoreCase);
    }

    // 25: Push = 0
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainNoPushCapabilityClaim(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push", body, StringComparison.OrdinalIgnoreCase);
    }

    // 26: No /wiki/ links
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainNoWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
    }

    // 27: No <img> tags
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainNoImgTags(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotMatch(@"<img\b", body);
    }

    // 28: View contains no ViewData overrides
    [Fact]
    public void ContactView_ContainsNoViewDataOverrides()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Contact.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 29: No page-level JSON-LD
    [Theory]
    [InlineData("/iletisim")]
    [InlineData("/en/contact")]
    public async Task BothPages_ContainNoPageLevelJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);
        Assert.DoesNotContain("application/ld+json", body, StringComparison.OrdinalIgnoreCase);
    }

    // 30: site.js untouched
    [Fact]
    public void SiteJs_RemainsUntouched()
    {
        var root = GetProjectRoot();
        var siteJsPath = Path.Combine(root, "wwwroot", "js", "site.js");
        Assert.True(File.Exists(siteJsPath));
        var content = File.ReadAllText(siteJsPath);
        Assert.Contains("bindContactForm", content);
        Assert.Contains("#contactForm", content);
        Assert.Contains("/lead/contact", content);
    }

    // 31: Homepage files untouched
    [Fact]
    public void Homepage_FilesRemainUntouched()
    {
        var root = GetProjectRoot();
        Assert.True(File.Exists(Path.Combine(root, "Views", "Home", "Index.cshtml")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-home.css")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "css", "pika-orbit.css")));
        Assert.True(File.Exists(Path.Combine(root, "wwwroot", "js", "pika-orbit.js")));
    }
}
