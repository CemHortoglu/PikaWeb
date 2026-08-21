using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pika.Models;
using Pika.Services;

namespace Pika.Controllers
{
    [Authorize]
    [Route("internal/wiki")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class InternalWikiController : Controller
    {
        private readonly IInternalWikiService _internalWikiService;

        public InternalWikiController(IInternalWikiService internalWikiService)
        {
            _internalWikiService = internalWikiService;
        }

        private void SetInternalSecurityHeaders()
        {
            Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive, nosnippet";
            Response.Headers["Cache-Control"] = "private, no-store, max-age=0, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            ViewData["RobotsMeta"] = "noindex, nofollow, noarchive, nosnippet";
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            SetInternalSecurityHeaders();
            var model = _internalWikiService.GetHomeViewModel();
            return View(model);
        }

        [HttpGet("{slug}")]
        public IActionResult Article(string slug)
        {
            SetInternalSecurityHeaders();

            if (string.Equals(slug, "index", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(slug, "index.html", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectPermanent("/internal/wiki/");
            }

            var model = _internalWikiService.GetArticleViewModel(slug);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string q)
        {
            SetInternalSecurityHeaders();
            var results = _internalWikiService.Search(q);
            return Json(results);
        }
    }
}
