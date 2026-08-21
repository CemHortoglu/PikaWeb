using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Pika.Services;

namespace Pika.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class SitemapController : Controller
{
    private readonly IWikiService _wikiService;

    public SitemapController(IWikiService wikiService) => _wikiService = wikiService;

    [HttpGet("/sitemap.xml")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public IActionResult Index()
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var urls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var meta in SeoHelper.GetAllPublicMetadata())
        {
            urls.Add($"{SeoHelper.BaseDomain}{meta.AlternatePathTr}");
            urls.Add($"{SeoHelper.BaseDomain}{meta.AlternatePathEn}");
        }

        urls.Add($"{SeoHelper.BaseDomain}/wiki/");
        foreach (var page in _wikiService.GetAllPages())
            urls.Add($"{SeoHelper.BaseDomain}/wiki/{Uri.EscapeDataString(page.Slug)}");

        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement(ns + "urlset",
                urls.OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                    .Select(url => new XElement(ns + "url", new XElement(ns + "loc", url)))));

        return Content(doc.ToString(SaveOptions.DisableFormatting), "application/xml; charset=utf-8");
    }
}
