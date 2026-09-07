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

public class C14SecurityPrivacyPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public C14SecurityPrivacyPageTests(WebApplicationFactory<Program> factory)
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

    // 01: GET /guvenlik-ve-gizlilik = 200
    [Fact]
    public async Task TurkishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/guvenlik-ve-gizlilik");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 02: GET /en/security-and-privacy = 200
    [Fact]
    public async Task EnglishPage_Returns200Success()
    {
        var client = CreateNoRedirectClient();
        var response = await client.GetAsync("/en/security-and-privacy");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // 03: Exact Solutions.SecurityPrivacy SeoHelper title/meta TR/EN
    [Fact]
    public void SeoHelper_SolutionsSecurityPrivacy_HasExactCanonicalValues()
    {
        var entry = SeoHelper.GetMetadata("Solutions", "SecurityPrivacy");
        Assert.NotNull(entry);
        Assert.Equal("/guvenlik-ve-gizlilik", entry.AlternatePathTr);
        Assert.Equal("/en/security-and-privacy", entry.AlternatePathEn);
        Assert.Equal("Security & Privacy | RBAC, API Erişimi ve Veri Gizliliği | Pika", entry.TitleTr);
        Assert.Equal("Security & Privacy | RBAC, API Access & Data Privacy | Pika", entry.TitleEn);
        Assert.Equal("Pika Security & Privacy; rol bazlı erişim denetimi (RBAC), maskelenmiş iletişim verisi görünümü, API anahtar erişimi ve paylaşılan güvenlik sorumluluklarıyla müşteri verisine erişimi kontrollü tutmaya yardımcı olur.", entry.DescriptionTr);
        Assert.Equal("Pika Security & Privacy helps control access to customer data through role-based access control (RBAC), masked communication-data views, API-key access and shared security responsibilities.", entry.DescriptionEn);
        Assert.Equal("Güvenlik ve Gizlilik", entry.BreadcrumbTitleTr);
        Assert.Equal("Security & Privacy", entry.BreadcrumbTitleEn);
    }

    // 04: Exactly one H1 per page
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_HaveExactlyOneH1(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var matches = Regex.Matches(html, @"<h1(?:>|\s[^>]*>)", RegexOptions.IgnoreCase);
        Assert.Single(matches);
    }

    // 05: TR normalized H1
    [Fact]
    public async Task TurkishPage_HasExpectedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/guvenlik-ve-gizlilik");
        var h1 = ExtractH1(html);
        Assert.Equal("Müşteri verisine erişimi, açık yetki sınırlarıyla kontrol altında tutun.", h1);
    }

    // 06: EN normalized H1
    [Fact]
    public async Task EnglishPage_HasExpectedH1()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/security-and-privacy");
        var h1 = ExtractH1(html);
        Assert.Equal("Keep access to customer data under control through explicit permission boundaries.", h1);
    }

    // 07: TR contains Direct Answer heading
    [Fact]
    public async Task TurkishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/guvenlik-ve-gizlilik");
        Assert.Contains("Pika Security & Privacy nedir?", WebUtility.HtmlDecode(html));
    }

    // 08: EN contains Direct Answer heading
    [Fact]
    public async Task EnglishPage_ContainsDirectAnswerHeading()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/security-and-privacy");
        Assert.Contains("What is Pika Security & Privacy?", WebUtility.HtmlDecode(html));
    }

    // 09: Both contain RBAC and localized terms
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Rol Bazlı Erişim Denetimi")]
    [InlineData("/en/security-and-privacy", "Role-Based Access Control")]
    public async Task BothPages_ContainRbacAndLocalizedTerms(string url, string expectedTerm)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("RBAC", decoded);
        Assert.Contains(expectedTerm, decoded);
    }

    // 10: Both establish authentication != authorization
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Kimlik doğrulama:", "Yetkilendirme:")]
    [InlineData("/en/security-and-privacy", "Authentication asks:", "Authorization asks:")]
    public async Task BothPages_EstablishAuthenticationVsAuthorization(string url, string authTerm, string authzTerm)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(authTerm, decoded);
        Assert.Contains(authzTerm, decoded);
    }

    // 11: Both contain masked data concept
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Maskelenmiş Veri Görünümü")]
    [InlineData("/en/security-and-privacy", "Masked Data View")]
    public async Task BothPages_ContainMaskedDataConcept(string url, string expectedTerm)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedTerm, decoded);
    }

    // 12: Both explain masked != deleted
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Maskelenmiş veri, silinmiş veri değildir.")]
    [InlineData("/en/security-and-privacy", "Masked data is not deleted data.")]
    public async Task BothPages_ExplainMaskedNotDeleted(string url, string expectedPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedPhrase, decoded);
    }

    // 13: Both contain X-API-Key
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_ContainXApiKey(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.Contains("X-API-Key", html);
    }

    // 14: Both distinguish API authentication vs data validation
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "API authentication:", "Data validation:")]
    [InlineData("/en/security-and-privacy", "API authentication asks:", "Data validation asks:")]
    public async Task BothPages_DistinguishApiAuthVsValidation(string url, string apiAuth, string dataVal)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(apiAuth, decoded);
        Assert.Contains(dataVal, decoded);
    }

    // 15: Both contain Consent Management, Integrations, Pika Pilot
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_ContainRelatedEntities(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("Consent Management", decoded);
        Assert.True(decoded.Contains("Integrations") || decoded.Contains("Entegrasyonlar"));
        Assert.Contains("Pika Pilot", decoded);
    }

    // 16: Both distinguish Security & Privacy vs Consent Management
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Security & Privacy, Consent Management değildir.")]
    [InlineData("/en/security-and-privacy", "Security & Privacy is not Consent Management.")]
    public async Task BothPages_DistinguishSecurityVsConsentManagement(string url, string boundaryPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(boundaryPhrase, decoded);
    }

    // 17: Both distinguish Security & Privacy vs Integrations
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Security & Privacy, Integrations değildir.")]
    [InlineData("/en/security-and-privacy", "Security & Privacy is not Integrations.")]
    public async Task BothPages_DistinguishSecurityVsIntegrations(string url, string boundaryPhrase)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(boundaryPhrase, decoded);
    }

    // 18: Both contain explicit negative boundary for SOC 2
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "SOC 2 sertifikası iddia edilmez.")]
    [InlineData("/en/security-and-privacy", "No SOC 2 certification claim.")]
    public async Task BothPages_ContainSoc2NegativeBoundary(string url, string expectedBoundary)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedBoundary, decoded);
    }

    // 19: Both contain explicit negative boundary for SAML / SSO
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "SAML / SSO yeteneği iddia edilmez.")]
    [InlineData("/en/security-and-privacy", "No SAML / SSO capability claim.")]
    public async Task BothPages_ContainSamlSsoNegativeBoundary(string url, string expectedBoundary)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedBoundary, decoded);
    }

    // 20: Both contain explicit negative boundary for specific TLS version
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Belirli TLS sürümü garanti edilmez.")]
    [InlineData("/en/security-and-privacy", "No specific TLS version guarantee.")]
    public async Task BothPages_ContainTlsNegativeBoundary(string url, string expectedBoundary)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedBoundary, decoded);
    }

    // 21: Both contain explicit negative boundary for AES-256
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Belirli at-rest encryption standardı iddia edilmez.")]
    [InlineData("/en/security-and-privacy", "No specific at-rest encryption claim.")]
    public async Task BothPages_ContainAes256NegativeBoundary(string url, string expectedBoundary)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedBoundary, decoded);
        Assert.Contains("AES-256", decoded);
    }

    // 22: Both contain explicit negative boundary for physical database-per-tenant claim
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "Fiziksel tenant izolasyonu iddia edilmez.")]
    [InlineData("/en/security-and-privacy", "No physical tenant-isolation claim.")]
    public async Task BothPages_ContainTenantIsolationNegativeBoundary(string url, string expectedBoundary)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedBoundary, decoded);
    }

    // 23: Both contain AI privacy boundary
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "AI VE VERİ SINIRI")]
    [InlineData("/en/security-and-privacy", "AI & DATA BOUNDARY")]
    public async Task BothPages_ContainAiPrivacyBoundary(string url, string expectedEyebrow)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(expectedEyebrow, decoded);
    }

    // 24: Both contain TCKN, raw phone numbers, full names within explicit AI-data boundary
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik", "TCKN", "ham telefon numarası", "tam ad")]
    [InlineData("/en/security-and-privacy", "TCKN", "raw phone numbers", "full names")]
    public async Task BothPages_ContainAiBoundaryIdentifiers(string url, string tckn, string phone, string name)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains(tckn, decoded);
        Assert.Contains(phone, decoded);
        Assert.Contains(name, decoded);
    }

    // 25: Both state Zero-PII is NOT claimed
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_StateZeroPiiNotClaimed(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("Zero-PII", decoded);
        if (url.Contains("guvenlik-ve-gizlilik"))
        {
            Assert.Contains("Pika Zero-PII platformudur", decoded);
            Assert.Contains("Yanlış mutlak iddia", decoded);
        }
        else
        {
            Assert.Contains("Pika is a Zero-PII platform", decoded);
            Assert.Contains("The incorrect absolute claim is", decoded);
        }
    }

    // 26: View contains no ViewData overrides, no JsonLd section, no /wiki/, no <img
    [Fact]
    public void ViewSource_ContainsNoViewDataOverridesOrWikiLinksOrImgTags()
    {
        var root = GetProjectRoot();
        var viewPath = Path.Combine(root, "Views", "Solutions", "SecurityPrivacy.cshtml");
        var content = File.ReadAllText(viewPath);

        Assert.DoesNotContain("ViewData[\"Title\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaDescription\"]", content);
        Assert.DoesNotContain("ViewData[\"MetaKeywords\"]", content);
        Assert.DoesNotContain("ViewData[\"CanonicalUrl\"]", content);
        Assert.DoesNotContain("@section JsonLd", content);
        Assert.DoesNotContain("/wiki/", content);
        Assert.DoesNotContain("<img", content);
    }

    // 27: No real screenshot in rendered HTML
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
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

    // 28: No positive unverified certification/capability claims
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_ContainNoPositiveUnverifiedClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        // Reject positive certification/capability claims while allowing valid negative statements
        Assert.DoesNotMatch(new Regex(@"\b(is|we are|platform is)\s+SOC\s*2\s+certified\b(?!\s*(\?|without))", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bSOC\s*2\s+sertifikasına\s+sahip\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bSOC\s*2\s+sertifikalıdır\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\b(is|we are|platform is)\s+ISO\s*27001\s+certified\b(?!\s*(\?|without))", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bISO\s*27001\s+sertifikalıdır\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bSAML\s+ready\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bSSO\s+ready\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bAES-256\s+encrypted\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bAES-256\s+ile\s+şifrelenmiş\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bmilitary-grade\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bbank-grade\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\b100%\s+secure\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\b%100\s+güvenli\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bfully\s+secure\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bHTTPS\s*/\s*TLS\s*1\.2\+\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bTLS\s*1\.3\s+guaranteed\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bTLS\s*1\.3\s+garantisi\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\b24/7\s+SOC\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bSIEM\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bWAF\s+guarantee\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bDDoS\s+guarantee\b", RegexOptions.IgnoreCase), body);
    }

    // 29: No real-time opt-out claims
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_ContainNoRealTimeOptOutClaims(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\breal-time\s+opt-out\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\banlık\s+opt-out\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\breal-time\s+synchronization\b", RegexOptions.IgnoreCase), body);
    }

    // 30: No specific fixed role list in public body
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_ContainNoFixedRoleList(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\bAdmin,\s*Marketing\s+Manager,\s*Operator\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"\bAdmin,\s*Pazarlama\s+Yöneticisi,\s*Operatör\b", RegexOptions.IgnoreCase), body);
    }

    // 31: Exactly 8 supplied TR FAQ questions
    [Fact]
    public async Task TurkishPage_HasExactly8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/guvenlik-ve-gizlilik");
        var matches = Regex.Matches(html, @"<summary class=""sec-faq-summary"">([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, matches.Count);

        string[] expectedTrQuestions = {
            "Pika Security & Privacy nedir?",
            "Pika rol bazlı erişim denetimi kullanıyor mu?",
            "Pika hassas müşteri verilerini maskeler mi?",
            "Pika Ingestion API erişimi nasıl kontrol edilir?",
            "Pika her müşteri için ayrı fiziksel veritabanı kullanıyor mu?",
            "Pika SOC 2 veya ISO 27001 sertifikalı mı?",
            "Pika AI kişisel verileri tamamen hiç kullanmıyor mu?",
            "Security & Privacy ile Consent Management arasındaki fark nedir?"
        };

        for (int i = 0; i < expectedTrQuestions.Length; i++)
        {
            var summaryText = NormalizeWhitespace(WebUtility.HtmlDecode(matches[i].Groups[1].Value));
            Assert.Equal(expectedTrQuestions[i], summaryText);
        }
    }

    // 32: Exactly 8 supplied EN FAQ questions
    [Fact]
    public async Task EnglishPage_HasExactly8FaqQuestions()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/security-and-privacy");
        var matches = Regex.Matches(html, @"<summary class=""sec-faq-summary"">([\s\S]*?)</summary>", RegexOptions.IgnoreCase);
        Assert.Equal(8, matches.Count);

        string[] expectedEnQuestions = {
            "What is Pika Security & Privacy?",
            "Does Pika use role-based access control?",
            "Does Pika mask sensitive customer data?",
            "How is access to the Pika Ingestion API controlled?",
            "Does Pika use a separate physical database for every customer?",
            "Is Pika SOC 2 or ISO 27001 certified?",
            "Does Pika AI use no personal data at all?",
            "What is the difference between Security & Privacy and Consent Management?"
        };

        for (int i = 0; i < expectedEnQuestions.Length; i++)
        {
            var summaryText = NormalizeWhitespace(WebUtility.HtmlDecode(matches[i].Groups[1].Value));
            Assert.Equal(expectedEnQuestions[i], summaryText);
        }
    }

    // 33: No page-level FAQPage, SoftwareApplication, Offer JSON-LD
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_HaveNoPageLevelSchemaTypes(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        Assert.DoesNotContain("\"FAQPage\"", html);
        Assert.DoesNotContain("\"SoftwareApplication\"", html);
        Assert.DoesNotContain("\"Offer\"", html);
    }

    // 34: Correct contextual internal links
    [Fact]
    public async Task TurkishPage_ContainsExpectedInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/guvenlik-ve-gizlilik");
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.Contains("href=\"/pika\"", body);
        Assert.Contains("href=\"/cozumler/consent-management\"", body);
        Assert.Contains("href=\"/entegrasyonlar\"", body);
        Assert.Contains("href=\"/urunler/ai-kampanya-asistani\"", body);
        Assert.Contains("href=\"/cozumler/campaign-manager\"", body);
    }

    [Fact]
    public async Task EnglishPage_ContainsExpectedInternalLinks()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/security-and-privacy");
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.Contains("href=\"/en/pika\"", body);
        Assert.Contains("href=\"/en/solutions/consent-management\"", body);
        Assert.Contains("href=\"/en/integrations\"", body);
        Assert.Contains("href=\"/en/products/ai-campaign-assistant\"", body);
        Assert.Contains("href=\"/en/solutions/campaign-manager\"", body);
    }

    // 35: Exact pricing copy
    [Fact]
    public async Task TurkishPage_HasExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/guvenlik-ve-gizlilik");
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("İhtiyacınıza ve kullanım kapsamınıza göre özel teklif.", decoded);
    }

    [Fact]
    public async Task EnglishPage_HasExactPricingCopy()
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync("/en/security-and-privacy");
        var decoded = WebUtility.HtmlDecode(html);
        Assert.Contains("Pricing is tailored to your requirements and scope of use.", decoded);
    }

    // 36: No fixed pricing values
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_HaveNoFixedPricingValues(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotMatch(new Regex(@"\$\d+"), body);
        Assert.DoesNotMatch(new Regex(@"€\d+"), body);
        Assert.DoesNotMatch(new Regex(@"₺\d+"), body);
        Assert.DoesNotMatch(new Regex(@"\b\d+\s*(TL|USD|EUR)\b", RegexOptions.IgnoreCase), body);
        Assert.DoesNotMatch(new Regex(@"/(ay|mo|month)\b", RegexOptions.IgnoreCase), body);
    }

    // 37: Wiki links = 0 in main page body
    [Theory]
    [InlineData("/guvenlik-ve-gizlilik")]
    [InlineData("/en/security-and-privacy")]
    public async Task BothPages_ContainZeroWikiLinksInBody(string url)
    {
        var client = CreateNoRedirectClient();
        var html = await client.GetStringAsync(url);
        var body = ExtractMainBodyWithoutScripts(html);

        Assert.DoesNotContain("href=\"/wiki", body);
        Assert.DoesNotContain("href=\"https://wiki.pika.tr", body);
        Assert.DoesNotContain("href=\"http://wiki.pika.tr", body);
    }
}
