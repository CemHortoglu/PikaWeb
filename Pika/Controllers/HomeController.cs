using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [Route("{culture:regex(^(tr|en)$)}/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index() => View();

        [HttpGet("solutions")]
        public IActionResult Solutions() => View();

        [HttpGet("pricing")]
        public IActionResult Pricing() => View();

        [HttpGet("corporate")]
        public IActionResult Corporate() => View();

        [HttpGet("pika")]
        public IActionResult Pika() => View();

        [HttpGet("demo-talebi")]
        [HttpGet("demo-request")]
        public IActionResult DemoRequest() => View();

        [HttpGet("sss")]
        [HttpGet("faq")]
        public IActionResult Faq() => View();

        [HttpGet("contact")]
        public IActionResult Contact() => View();
    }
}
