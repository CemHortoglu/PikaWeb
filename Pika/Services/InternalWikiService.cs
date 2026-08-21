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
    public class InternalWikiService : IInternalWikiService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<InternalWikiService> _logger;
        private readonly WikiData _wikiData;
        private readonly List<WikiCategory> _categories = new();
        private readonly Dictionary<string, WikiPage> _pagesBySlug = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<WikiPageSummary> _allPages = new();
        private readonly Dictionary<string, List<WikiTocItem>> _tocBySlug = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _processedHtmlBySlug = new(StringComparer.OrdinalIgnoreCase);

        public InternalWikiService(IWebHostEnvironment env, ILogger<InternalWikiService> logger)
        {
            _env = env;
            _logger = logger;

            var jsonPath = Path.Combine(_env.ContentRootPath, "App_Data", "internal_wiki.json");
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
                    _logger.LogError(ex, "Failed to load internal wiki data from {JsonPath}", jsonPath);
                    _wikiData = new WikiData();
                }
            }
            else
            {
                _logger.LogWarning("Internal wiki data file not found at {JsonPath}", jsonPath);
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

                var (processedHtml, toc) = ProcessHtmlAndGenerateToc(page.Html);
                _processedHtmlBySlug[slug] = processedHtml;
                _tocBySlug[slug] = toc;
            }

            // Parse navigation structure
            foreach (var navItem in _wikiData.Nav)
            {
                if (navItem.Count >= 2)
                {
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
                            if (!string.IsNullOrEmpty(slug) && _pagesBySlug.TryGetValue(slug, out var page))
                            {
                                var summary = new WikiPageSummary
                                {
                                    Slug = slug,
                                    Title = page.Title,
                                    Section = sectionTitle,
                                    Summary = page.Summary
                                };
                                category.Pages.Add(summary);
                                _allPages.Add(summary);
                            }
                        }
                    }
                    _categories.Add(category);
                }
            }
        }

        public WikiData GetWikiData() => _wikiData;

        public WikiPage? GetPage(string slug)
        {
            if (_pagesBySlug.TryGetValue(slug, out var page))
            {
                return new WikiPage
                {
                    Slug = page.Slug,
                    Section = page.Section,
                    Title = page.Title,
                    Summary = page.Summary,
                    Html = _processedHtmlBySlug.TryGetValue(slug, out var html) ? html : page.Html,
                    Related = page.Related
                };
            }
            return null;
        }

        public List<WikiCategory> GetCategories() => _categories;

        public List<WikiPageSummary> GetAllPages() => _allPages;

        public WikiHomeViewModel GetHomeViewModel()
        {
            return new WikiHomeViewModel
            {
                Categories = _categories,
                TotalArticles = _allPages.Count,
                CanonicalUrl = "https://pika.tr/internal/wiki/"
            };
        }

        public WikiArticleViewModel? GetArticleViewModel(string slug)
        {
            var page = GetPage(slug);
            if (page == null)
            {
                return null;
            }

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
                            Summary = relPage.Summary
                        });
                    }
                }
            }

            // Find previous and next page
            WikiPageSummary? prev = null;
            WikiPageSummary? next = null;
            var currentIndex = _allPages.FindIndex(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
            if (currentIndex > 0)
            {
                prev = _allPages[currentIndex - 1];
            }
            if (currentIndex >= 0 && currentIndex < _allPages.Count - 1)
            {
                next = _allPages[currentIndex + 1];
            }

            var toc = _tocBySlug.TryGetValue(slug, out var tocList) ? tocList : new List<WikiTocItem>();

            return new WikiArticleViewModel
            {
                CurrentPage = page,
                Categories = _categories,
                RelatedPages = relatedSummaries,
                TableOfContents = toc,
                PreviousPage = prev,
                NextPage = next,
                CanonicalUrl = $"https://pika.tr/internal/wiki/{slug}"
            };
        }

        public List<WikiPageSummary> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<WikiPageSummary>();

            var cleanQ = query.Trim();
            return _allPages.Where(p =>
                p.Title.Contains(cleanQ, StringComparison.OrdinalIgnoreCase) ||
                p.Summary.Contains(cleanQ, StringComparison.OrdinalIgnoreCase) ||
                p.Section.Contains(cleanQ, StringComparison.OrdinalIgnoreCase)
            ).Take(10).ToList();
        }

        private (string ProcessedHtml, List<WikiTocItem> Toc) ProcessHtmlAndGenerateToc(string rawHtml)
        {
            var toc = new List<WikiTocItem>();
            if (string.IsNullOrWhiteSpace(rawHtml))
            {
                return (rawHtml, toc);
            }

            var usedAnchors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var headingRegex = new Regex(@"<(h[23])([^>]*)>(.*?)</\1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var processedHtml = headingRegex.Replace(rawHtml, match =>
            {
                var tag = match.Groups[1].Value.ToLowerInvariant();
                var attributes = match.Groups[2].Value;
                var innerTextWithTags = match.Groups[3].Value;

                var cleanTitle = Regex.Replace(innerTextWithTags, "<.*?>", string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(cleanTitle))
                {
                    return match.Value;
                }

                var idMatch = Regex.Match(attributes, @"id=[""']([^""']+)[""']", RegexOptions.IgnoreCase);
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
                        {
                            counter++;
                        }
                        anchor = $"{anchor}-{counter}";
                    }
                    usedAnchors.Add(anchor);
                    attributes = $"{attributes} id=\"{anchor}\"";
                }

                var level = tag == "h2" ? 2 : 3;
                toc.Add(new WikiTocItem
                {
                    Title = cleanTitle,
                    Anchor = anchor,
                    Level = level
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
