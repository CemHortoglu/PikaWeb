using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Pika.Models;

namespace Pika.Services
{
    public class WikiService : IWikiService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<WikiService> _logger;
        private readonly WikiData _wikiData;
        private readonly List<WikiCategory> _categories = new();
        private readonly Dictionary<string, WikiPage> _pagesBySlug = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<WikiPageSummary> _allPages = new();
        private readonly Dictionary<string, List<WikiTocItem>> _tocBySlug = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _processedHtmlBySlug = new(StringComparer.OrdinalIgnoreCase);

        public WikiService(IWebHostEnvironment env, ILogger<WikiService> logger)
        {
            _env = env;
            _logger = logger;

            var jsonPath = Path.Combine(_env.ContentRootPath, "App_Data", "wiki.json");
            if (File.Exists(jsonPath))
            {
                try
                {
                    var json = File.ReadAllText(jsonPath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    _wikiData = JsonSerializer.Deserialize<WikiData>(json, options) ?? new WikiData();
                    Initialize();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load wiki data from {JsonPath}", jsonPath);
                    _wikiData = new WikiData();
                }
            }
            else
            {
                _logger.LogWarning("Wiki data file not found at {JsonPath}", jsonPath);
                _wikiData = new WikiData();
            }
        }

        private void Initialize()
        {
            foreach (var kvp in _wikiData.Pages)
            {
                var slug = kvp.Key;
                var page = kvp.Value;
                page.Slug = slug;
                _pagesBySlug[slug] = page;

                // Public documentation is intentionally customer-safe. The historical
                // content source may still contain implementation terminology that
                // belongs in the authenticated Internal Wiki. Sanitize before any
                // public HTML is rendered or indexed.
                var customerSafeHtml = SanitizePublicHtml(slug, page.Html);
                var (processedHtml, toc) = ProcessHtmlAndGenerateToc(customerSafeHtml);
                _processedHtmlBySlug[slug] = processedHtml;
                _tocBySlug[slug] = toc;
            }

            foreach (var navItem in _wikiData.Nav)
            {
                if (navItem.Count < 2)
                    continue;

                var sectionTitle = navItem[0]?.ToString() ?? string.Empty;
                var category = new WikiCategory
                {
                    Title = sectionTitle
                };

                if (navItem[1] is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var slugElement in jsonElement.EnumerateArray())
                    {
                        var slug = slugElement.GetString();
                        if (string.IsNullOrEmpty(slug) || !_pagesBySlug.TryGetValue(slug, out var page))
                            continue;

                        var summary = new WikiPageSummary
                        {
                            Slug = slug,
                            Title = page.Title,
                            Section = sectionTitle,
                            Summary = SanitizePublicSummary(page.Summary)
                        };
                        category.Pages.Add(summary);
                        _allPages.Add(summary);
                    }
                }

                _categories.Add(category);
            }
        }

        public WikiData GetWikiData() => _wikiData;

        public WikiPage? GetPage(string slug)
        {
            if (!_pagesBySlug.TryGetValue(slug, out var page))
                return null;

            return new WikiPage
            {
                Slug = page.Slug,
                Section = page.Section,
                Title = page.Title,
                Summary = SanitizePublicSummary(page.Summary),
                Html = _processedHtmlBySlug.TryGetValue(slug, out var html) ? html : SanitizePublicHtml(slug, page.Html),
                Related = page.Related
            };
        }

        public List<WikiCategory> GetCategories() => _categories;
        public List<WikiPageSummary> GetAllPages() => _allPages;

        public WikiHomeViewModel GetHomeViewModel()
        {
            return new WikiHomeViewModel
            {
                Categories = _categories,
                TotalArticles = _allPages.Count,
                CanonicalUrl = "https://pika.tr/wiki/"
            };
        }

        public WikiArticleViewModel? GetArticleViewModel(string slug)
        {
            var page = GetPage(slug);
            if (page == null)
                return null;

            var relatedSummaries = new List<WikiPageSummary>();
            if (page.Related != null)
            {
                foreach (var relSlug in page.Related)
                {
                    if (_pagesBySlug.TryGetValue(relSlug, out var relPage))
                    {
                        relatedSummaries.Add(new WikiPageSummary
                        {
                            Slug = relSlug,
                            Title = relPage.Title,
                            Section = relPage.Section,
                            Summary = SanitizePublicSummary(relPage.Summary)
                        });
                    }
                }
            }

            WikiPageSummary? prev = null;
            WikiPageSummary? next = null;
            var currentIndex = _allPages.FindIndex(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
            if (currentIndex > 0)
                prev = _allPages[currentIndex - 1];
            if (currentIndex >= 0 && currentIndex < _allPages.Count - 1)
                next = _allPages[currentIndex + 1];

            var toc = _tocBySlug.TryGetValue(slug, out var tocList) ? tocList : new List<WikiTocItem>();

            return new WikiArticleViewModel
            {
                CurrentPage = page,
                Categories = _categories,
                RelatedPages = relatedSummaries,
                TableOfContents = toc,
                PreviousPage = prev,
                NextPage = next,
                CanonicalUrl = $"https://pika.tr/wiki/{slug}"
            };
        }

        /// <summary>
        /// Keeps the public knowledge base useful to customers and crawlers while
        /// removing implementation-level language that belongs in Internal Wiki.
        /// App_Data/wiki.json is not a public static asset; this transformation is
        /// applied before server-rendered public HTML is produced.
        /// </summary>
        private static string SanitizePublicHtml(string slug, string? rawHtml)
        {
            if (string.IsNullOrWhiteSpace(rawHtml))
                return rawHtml ?? string.Empty;

            var html = rawHtml;

            // Exact implementation weights are intentionally not a public product contract.
            html = html.Replace(
                "Normalize gelir %60 + satın alma sıklığı %40.",
                "Gerçekleşmiş ticari değer ve satın alma sıklığı gibi açıklanabilir sinyaller birlikte değerlendirilir.",
                StringComparison.OrdinalIgnoreCase);
            html = html.Replace(
                "Güncel deterministik formül normalize edilmiş toplam gelir %60 + satın alma sıklığı %40’tır. Ortalama sepet, sadakat veya churn bu skorun içinde değildir; ayrı bağlamlardır.",
                "Müşteri Değer Skoru gerçekleşmiş ticari değer ve satın alma sıklığı gibi açıklanabilir sinyalleri birlikte değerlendirir. Ortalama sepet, sadakat ve pasifleşme riski ayrı karar bağlamlarıdır.",
                StringComparison.OrdinalIgnoreCase);
            html = html.Replace(
                "Güncel uygulamada normalize toplam gelir %60 + satın alma sıklığı %40 ile hesaplanan 0–100 sistem metriği.",
                "Gerçekleşmiş ticari değer ve satın alma sıklığı gibi açıklanabilir sinyallerden türetilen karşılaştırmalı müşteri değer göstergesi.",
                StringComparison.OrdinalIgnoreCase);
            html = html.Replace(
                "Customer Value Score’un mevcut %60 gelir + %40 sıklık formülünün parçası değildir.",
                "Customer Value Score’un temel ticari değer bağlamının parçası değildir.",
                StringComparison.OrdinalIgnoreCase);

            // Internal audit language -> customer-facing product language.
            html = html.Replace(
                "Standart ayrı bir genel Upsell BI başarı-olasılığı servisi doğrulanmış değildir. Upsell bağlamı Product Role/Playbook ile tanımlanır ve iş kuralları/Journey tarafında uygulanabilir.",
                "Pika, upsell bağlamını Product Role/Playbook ve iş kuralları üzerinden ele alır; tüm senaryolar için evrensel bir başarı olasılığı skoru sunulduğu varsayılmamalıdır.",
                StringComparison.OrdinalIgnoreCase);
            html = html.Replace(
                "Standart mağaza BI’da birleşik 0–100 skor yoktur. Revenue rank, growth rank ve repeat-rate rank gibi ayrı açıklanabilir sıralamalar vardır.",
                "Pika mağaza performansını tek bir birleşik skora indirgemek yerine gelir, büyüme ve tekrar satın alma gibi ayrı açıklanabilir göstergelerle değerlendirebilir.",
                StringComparison.OrdinalIgnoreCase);

            // Data-quality internals -> customer-readable terminology.
            html = html.Replace("Customer-unmatched", "Müşteri eşleştirme sorunu", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("customer-unmatched", "müşteri eşleştirme sorunu", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("Product-unmatched / product-unclassified", "ürün eşleştirme veya sınıflandırma sorunu", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("product-unmatched / product-unclassified", "ürün eşleştirme veya sınıflandırma sorunu", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("coverage, warning, unknown bucket ve data-quality issue", "veri kapsamı, uyarı ve veri kalitesi göstergeleri", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("data-through, last-successful, freshness status, family health ve warning", "son veri tarihi, son başarılı güncelleme ve veri tazeliği göstergeleri", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("Read-model veya snapshot’ın", "İlgili analitik görünümün", StringComparison.OrdinalIgnoreCase);

            // Public delivery documentation describes outcomes and safeguards, not queue/worker internals.
            if (slug.Equals("gonderim-son-katman", StringComparison.OrdinalIgnoreCase))
            {
                html = Regex.Replace(
                    html,
                    @"<h2>Teknik operasyon neden ayrı katmandır\?</h2>\s*<p>.*?</p>",
                    "<h2>Gönderim operasyonu neden ayrı izlenir?</h2><p>Doğru bir müşteri ve kampanya kararı verilmiş olsa bile kanal tarafında geçici teknik sorun, geçersiz adres veya kalıcı izin engeli oluşabilir. Pika bu nedenle gönderim durumunu karar/fırsat katmanından ayrı izler ve kullanıcının mesajın neden ilerlemediğini müşteri güvenli bir operasyon görünümünde anlayabilmesini hedefler.</p>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            if (slug.Equals("gonderim-operasyonu-izleme", StringComparison.OrdinalIgnoreCase))
            {
                html = Regex.Replace(
                    html,
                    @"<figcaption>.*?</figcaption>",
                    "<figcaption><strong>Demo görünüm:</strong> E-posta, SMS ve WhatsApp gönderimlerinde bekleyen, işlenen, gönderilen, teslim edilen ve hata alan iletişimlerin müşteri güvenli operasyon durumları izlenebilir.</figcaption>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
                html = Regex.Replace(
                    html,
                    @"<h2>İzleme hangi seviyelerde yapılır\?</h2>\s*<p>.*?</p>",
                    "<h2>İzleme hangi seviyelerde yapılır?</h2><p>Kampanya veya Journey genel durumu ile tekil iletişimlerin gönderim sonuçları farklı seviyelerde izlenir. Böylece bir kampanya genel olarak devam ederken belirli bir kanalda veya belirli alıcılarda sorun oluşup oluşmadığı anlaşılabilir; kullanıcıya gerekli operasyon görünürlüğü sunulur.</p>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
            }

            // Public FAQ/help content should explain retry behavior without exposing dead-letter/queue mechanics.
            html = Regex.Replace(
                html,
                @"<details><summary>Gönderim başarısızsa sistem sürekli tekrar mı dener\?</summary><p>.*?</p></details>",
                "<details><summary>Gönderim başarısızsa sistem sürekli tekrar mı dener?</summary><p>Hayır. Geçici teknik sorunlar kontrollü yeniden denemeye uygun olabilir; geçersiz adres, izin veya opt-out gibi kalıcı engeller tekrar gönderim nedeni değildir. Başarısız iletişimler izlenebilir tutulur ve gereken durumlarda operasyon ekibinin incelemesine açılır.</p></details>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Remove engineering-only glossary entries from customer glossary.
            html = RemoveGlossaryItem(html, "Delivery Job / Gönderim Görevi");
            html = RemoveGlossaryItem(html, "Retry / Yeniden Deneme");
            html = RemoveGlossaryItem(html, "Dead-letter");

            // Do not leak public-facing queue/worker terminology in prose that may have escaped
            // the known article-specific rewrites above.
            html = html.Replace("worker/queue problemi", "kanal veya gönderim altyapısı sorunu", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("delivery job / attempt / event / policy", "gönderim durumu ve sonuç kayıtları", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("tekil delivery job, attempt geçmişi, worker sağlığı ve queue/dead-letter birikimi", "tekil gönderim durumu, kanal sonucu ve operasyon uyarıları", StringComparison.OrdinalIgnoreCase);
            html = html.Replace("retry limiti aşan işler dead-letter/inceleme akışına taşınabilir", "uygun yeniden denemeler sonuç vermezse kayıt operasyonel incelemeye alınabilir", StringComparison.OrdinalIgnoreCase);

            return html;
        }

        private static string RemoveGlossaryItem(string html, string label)
        {
            var pattern = $@"<div class=[\"']glossary-item[\"']>\s*<strong>{Regex.Escape(label)}</strong>.*?</div>";
            return Regex.Replace(html, pattern, string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        private static string SanitizePublicSummary(string? summary)
        {
            if (string.IsNullOrWhiteSpace(summary))
                return summary ?? string.Empty;

            var value = summary;
            value = value.Replace("worker", "gönderim", StringComparison.OrdinalIgnoreCase)
                         .Replace("queue", "işlem", StringComparison.OrdinalIgnoreCase)
                         .Replace("dead-letter", "operasyon incelemesi", StringComparison.OrdinalIgnoreCase)
                         .Replace("delivery job", "gönderim kaydı", StringComparison.OrdinalIgnoreCase);
            return value;
        }

        private (string ProcessedHtml, List<WikiTocItem> Toc) ProcessHtmlAndGenerateToc(string rawHtml)
        {
            var toc = new List<WikiTocItem>();
            if (string.IsNullOrWhiteSpace(rawHtml))
                return (rawHtml, toc);

            var usedAnchors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var headingRegex = new Regex(@"<(h[23])([^>]*)>(.*?)</\1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var processedHtml = headingRegex.Replace(rawHtml, match =>
            {
                var tag = match.Groups[1].Value.ToLowerInvariant();
                var attributes = match.Groups[2].Value;
                var innerTextWithTags = match.Groups[3].Value;
                var cleanTitle = Regex.Replace(innerTextWithTags, "<.*?>", string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(cleanTitle))
                    return match.Value;

                var idMatch = Regex.Match(attributes, @"id=[\"']([^\"']+)[\"']", RegexOptions.IgnoreCase);
                string anchor;
                if (idMatch.Success)
                {
                    anchor = idMatch.Groups[1].Value;
                }
                else
                {
                    anchor = Slugify(cleanTitle);
                    if (usedAnchors.Contains(anchor))
                    {
                        var counter = 2;
                        while (usedAnchors.Contains($"{anchor}-{counter}"))
                            counter++;
                        anchor = $"{anchor}-{counter}";
                    }
                    usedAnchors.Add(anchor);
                    attributes = $"{attributes} id=\"{anchor}\"";
                }

                toc.Add(new WikiTocItem
                {
                    Title = cleanTitle,
                    Anchor = anchor,
                    Level = tag == "h2" ? 2 : 3
                });

                return $"<{tag}{attributes}>{innerTextWithTags}</{tag}>";
            });

            return (processedHtml, toc);
        }

        private static string Slugify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "section";

            var str = text.ToLowerInvariant();
            str = str.Replace("ç", "c")
                     .Replace("ğ", "g")
                     .Replace("ı", "i")
                     .Replace("ö", "o")
                     .Replace("ş", "s")
                     .Replace("ü", "u");
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim('-');
            str = Regex.Replace(str, @"-+", "-");
            return string.IsNullOrWhiteSpace(str) ? "section" : str;
        }
    }
}
