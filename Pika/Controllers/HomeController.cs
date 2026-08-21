using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            ViewData["culture"] = "tr";
            RouteData.Values["culture"] = "tr";
            return View("Index");
        }

        [HttpGet("/en/")]
        public IActionResult EnglishIndex()
        {
            ViewData["culture"] = "en";
            RouteData.Values["culture"] = "en";
            return View("Index");
        }

        [HttpGet("/pika")]
        [HttpGet("/en/pika")]
        public IActionResult Pika() => View();

        [HttpGet("/kurumsal")]
        [HttpGet("/en/corporate")]
        public IActionResult Corporate() => View();

        [HttpGet("/demo-talebi")]
        [HttpGet("/en/demo-request")]
        public IActionResult DemoRequest() => View();

        [HttpGet("/kaynaklar/sss")]
        [HttpGet("/en/resources/faq")]
        public IActionResult Faq() => View();

        [HttpGet("/iletisim")]
        [HttpGet("/en/contact")]
        public IActionResult Contact() => View();

        [HttpGet("/kariyer")]
        [HttpGet("/en/careers")]
        public IActionResult Career() => View();

        [HttpGet("/kullanim-sartlari")]
        [HttpGet("/en/terms-of-use")]
        public IActionResult TermsOfUse() => View();

        [HttpGet("/gizlilik-politikasi")]
        [HttpGet("/en/privacy-policy")]
        public IActionResult PrivacyPolicy() => View();

        [HttpGet("/error/404")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound404()
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            ViewData["RobotsMeta"] = "noindex, nofollow";
            ViewData["Title"] = "404 - Sayfa Bulunamadı";
            return View("NotFound404");
        }

        [HttpGet("/error/500")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            ViewData["RobotsMeta"] = "noindex, nofollow";
            ViewData["Title"] = "500 - Sunucu Hatası";
            return View("Error");
        }
    }
}
