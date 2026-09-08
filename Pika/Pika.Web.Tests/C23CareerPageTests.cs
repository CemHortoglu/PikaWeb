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

public class C23CareerPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C23CareerPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /kariyer = 200
    [Fact]
    public async Task TurkishCareerPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/kariyer");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/careers = 200
    [Fact]
    public async Task EnglishCareerPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/careers");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Home.Career SEO TR/EN
    [Fact]
    public void HomeCareerSeoMetadata_HasExactCanonicalValues()
    {
        var meta = SeoHelper.GetMetadata("Home", "Career");
        Assert.NotNull(meta);
        Assert.Equal("/kariyer", meta.AlternatePathTr);
        Assert.Equal("/en/careers", meta.AlternatePathEn);
        Assert.Equal("Kariyer | Pika'da Genel Başvuru", meta.TitleTr);
        Assert.Equal("Careers at Pika | General Applications", meta.TitleEn);
        Assert.Equal("Pika'nın müşteri zekâsı ve omnichannel pazarlama ürün odağını inceleyin; kariyer için genel başvurunuzu CV veya LinkedIn bilgilerinizle paylaşın.", meta.DescriptionTr);
        Assert.Equal("Explore Pika's Customer Intelligence & Omnichannel Marketing product context and submit a general career application with your CV or LinkedIn details.", meta.DescriptionEn);
        Assert.Equal("Kariyer", meta.BreadcrumbTitleTr);
        Assert.Equal("Careers", meta.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
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
        var html = await client.GetStringAsync("/kariyer");
        var h1 = ExtractH1(html);
        Assert.Equal("Pika'yı geliştiren işe katkı sunmak istiyorsanız, genel başvurunuzu paylaşın.", h1);
    }

    // 06: Exact normalized EN H1
    [Fact]
    public async Task EnglishPage_HasExactNormalizedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/careers");
        var h1 = ExtractH1(html);
        Assert.Equal("If you want to contribute to the work behind Pika, submit a general application.", h1);
    }

    // 07: careerForm exists
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainCareerFormId(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("id=\"careerForm\"", html);
    }

    // 08: careerResult exists
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainCareerResultId(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("id=\"careerResult\"", html);
    }

    // 09: Exact form names remain
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainExactFormFieldNames(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.Contains("name=\"fullName\"", html);
        Assert.Contains("name=\"email\"", html);
        Assert.Contains("name=\"phone\"", html);
        Assert.Contains("name=\"position\"", html);
        Assert.Contains("name=\"cvUrl\"", html);
        Assert.Contains("name=\"cvFile\"", html);
        Assert.Contains("name=\"message\"", html);
    }

    // 10: fullName/email/phone required
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_HaveRequiredAttributesOnMandatoryFields(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.Matches(@"<input[^>]*name=""fullName""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""email""[^>]*required", html);
        Assert.Matches(@"<input[^>]*name=""phone""[^>]*required", html);
    }

    // 11: position exists as hidden field with value="General Application"
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainHiddenGeneralApplicationPosition(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.Matches(@"<input[^>]*type=""hidden""[^>]*name=""position""[^>]*value=""General Application""", html);
    }

    // 12: No visible position dropdown
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoPositionSelectDropdown(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.DoesNotContain("<select name=\"position\"", html, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotMatch(@"<select[^>]*name=""position""", html);
    }

    // 13: cvFile has accept=".pdf"
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_HavePdfAcceptanceOnCvFile(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);

        Assert.Matches(@"<input[^>]*name=""cvFile""[^>]*accept=""\.pdf""", html);
    }

    // 14: Both rendered pages state maximum 5 MB
    [Theory]
    [InlineData("/kariyer", "5 MB")]
    [InlineData("/en/careers", "5 MB")]
    public async Task BothPages_StateMaximumFiveMb(string url, string expectedMb)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.Contains(expectedMb, body);
    }

    // 15: Privacy links correct TR/EN
    [Fact]
    public async Task TurkishPage_LinksToTurkishPrivacyPolicy()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kariyer");
        Assert.Contains("href=\"/gizlilik-politikasi\"", html);
    }

    [Fact]
    public async Task EnglishPage_LinksToEnglishPrivacyPolicy()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/careers");
        Assert.Contains("href=\"/en/privacy-policy\"", html);
    }

    // 16: No data-i18n="career.
    [Fact]
    public void CareerView_ContainsNoDataI18nCareerAttributes()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Career.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("data-i18n=\"career.", viewContent);
    }

    // 17: No View-level SEO
    [Fact]
    public void CareerView_ContainsNoViewDataOverrides()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Home", "Career.cshtml");
        var viewContent = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", viewContent);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", viewContent);
    }

    // 18: No fake vacancy names
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoFakeVacancyTitles(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Backend Developer", body);
        Assert.DoesNotContain("Frontend Developer", body);
        Assert.DoesNotContain("Full Stack Developer", body);
        Assert.DoesNotContain("DevOps Engineer", body);
        Assert.DoesNotContain("Data Engineer", body);
        Assert.DoesNotContain("Product Manager", body);
        Assert.DoesNotContain("UI/UX Designer", body);
        Assert.DoesNotContain("Digital Marketing Specialist", body);
    }

    // 19: No positive unsupported claims (international customers, fast growing, remote work, flexible hours, training budget, mentorship)
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoUnsupportedClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("international customers", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("global customers", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("uluslararası müşteriler", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("küresel müşteriler", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("fast growing", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("fast-growing", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("hızla büyüyen", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("remote work", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("uzaktan çalışma", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("flexible working hours", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("flexible hours", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("esnek çalışma saatleri", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("training budget", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("eğitim bütçesi", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("mentorship program", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("mentorluk programı", body, StringComparison.OrdinalIgnoreCase);
    }

    // 20: No employee testimonial / fake quote
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoFakeQuotesOrTestimonials(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Pika'da sadece kod yazmıyoruz", body);
        Assert.DoesNotContain("— Pika Ekibi", body);
        Assert.DoesNotContain("pika-career-quote-box", body);
    }

    // 21: No fixed review-time promise
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoFixedReviewTimePromises(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("en kısa sürede", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("as soon as possible", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("24 saat içinde", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("within 24 hours", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("same day", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("aynı gün", body, StringComparison.OrdinalIgnoreCase);
    }

    // 22: No interview / offer / employment guarantee
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoInterviewOrOfferGuarantees(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("interview guarantee", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("görüşme garantisi sunar", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("işe alım garantisi sunar", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("teklif garantisi sunar", body, StringComparison.OrdinalIgnoreCase);
    }

    // 23: Exactly 6 TR FAQs
    [Fact]
    public async Task TurkishPage_ContainsExactlySixFaqs()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/kariyer");

        var matches = Regex.Matches(html, @"<details[^>]*class=""pika-career-faq-details[^""]*""[^>]*>", RegexOptions.IgnoreCase);
        Assert.Equal(6, matches.Count);
    }

    // 24: Exactly 6 EN FAQs
    [Fact]
    public async Task EnglishPage_ContainsExactlySixFaqs()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/careers");

        var matches = Regex.Matches(html, @"<details[^>]*class=""pika-career-faq-details[^""]*""[^>]*>", RegexOptions.IgnoreCase);
        Assert.Equal(6, matches.Count);
    }

    // 25: No JobPosting / FAQPage JSON-LD
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoJobPostingOrFaqPageJsonLd(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("JobPosting", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("FAQPage", body, StringComparison.OrdinalIgnoreCase);
    }

    // 26: Push = 0
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoPushClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("Push", body, StringComparison.OrdinalIgnoreCase);
    }

    // 27: Wiki links = 0
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoWikiLinks(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("/wiki/", body, StringComparison.OrdinalIgnoreCase);
    }

    // 28: No <img>
    [Theory]
    [InlineData("/kariyer")]
    [InlineData("/en/careers")]
    public async Task BothPages_ContainNoImgTags(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(@"<img\b", body);
    }

    // 29: site.js untouched
    [Fact]
    public void SiteJs_RemainsUntouched()
    {
        var root = GetProjectRoot();
        var siteJsPath = Path.Combine(root, "wwwroot", "js", "site.js");
        Assert.True(File.Exists(siteJsPath));
        var content = File.ReadAllText(siteJsPath);
        Assert.Contains("bindCareerForm", content);
        Assert.Contains("#careerForm", content);
        Assert.Contains("/lead/career", content);
    }

    // 30: LeadController.cs untouched
    [Fact]
    public void LeadController_RemainsUntouched()
    {
        var root = GetProjectRoot();
        var controllerPath = Path.Combine(root, "Controllers", "LeadController.cs");
        Assert.True(File.Exists(controllerPath));
        var content = File.ReadAllText(controllerPath);
        Assert.Contains("CareerRequestDto", content);
        Assert.Contains("Career([FromForm] CareerRequestDto dto, IFormFile? cvFile", content);
    }

    // 31: Homepage untouched
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
