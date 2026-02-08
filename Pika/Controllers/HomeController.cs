using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult Solutions() => View();
        public IActionResult Pricing() => View();
        public IActionResult Corporate() => View();
        public IActionResult Contact() => View();
    }
}
