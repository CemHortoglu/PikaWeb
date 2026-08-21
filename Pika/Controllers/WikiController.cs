using System;
using Microsoft.AspNetCore.Mvc;
using Pika.Models;
using Pika.Services;

namespace Pika.Controllers
{
    [Route("wiki")]
    public class WikiController : Controller
    {
        private readonly IWikiService _wikiService;

        public WikiController(IWikiService wikiService)
        {
            _wikiService = wikiService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var model = _wikiService.GetHomeViewModel();
            return View(model);
        }

        [HttpGet("{slug}")]
        public IActionResult Article(string slug)
        {
            if (string.Equals(slug, "index", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(slug, "index.html", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectPermanent("/wiki/");
            }

            var model = _wikiService.GetArticleViewModel(slug);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}
