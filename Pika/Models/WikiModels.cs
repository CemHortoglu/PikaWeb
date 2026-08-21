using System.Collections.Generic;

namespace Pika.Models
{
    public class WikiData
    {
        public Dictionary<string, WikiPage> Pages { get; set; } = new();
        public List<List<object>> Nav { get; set; } = new();
    }

    public class WikiPage
    {
        public string Slug { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Html { get; set; } = string.Empty;
        public List<string> Related { get; set; } = new();
    }

    public class WikiCategory
    {
        public string Title { get; set; } = string.Empty;
        public List<WikiPageSummary> Pages { get; set; } = new();
    }

    public class WikiPageSummary
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }

    public class WikiTocItem
    {
        public string Title { get; set; } = string.Empty;
        public string Anchor { get; set; } = string.Empty;
        public int Level { get; set; } = 2;
    }

    public class WikiArticleViewModel
    {
        public WikiPage CurrentPage { get; set; } = new();
        public List<WikiCategory> Categories { get; set; } = new();
        public List<WikiPageSummary> RelatedPages { get; set; } = new();
        public List<WikiTocItem> TableOfContents { get; set; } = new();
        public WikiPageSummary? PreviousPage { get; set; }
        public WikiPageSummary? NextPage { get; set; }
        public string CanonicalUrl { get; set; } = string.Empty;
    }

    public class WikiHomeViewModel
    {
        public List<WikiCategory> Categories { get; set; } = new();
        public int TotalArticles { get; set; }
        public string CanonicalUrl { get; set; } = "https://pika.tr/wiki/";
    }
}
