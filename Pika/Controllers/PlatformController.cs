using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    public class PlatformController : Controller
    {
        [HttpGet("/platform/customer-intelligence")]
        [HttpGet("/en/platform/customer-intelligence")]
        public IActionResult CustomerIntelligence() => View();

        [HttpGet("/platform/product-intelligence")]
        [HttpGet("/en/platform/product-intelligence")]
        public IActionResult ProductIntelligence() => View();

        [HttpGet("/platform/pika-360")]
        [HttpGet("/en/platform/pika-360")]
        public IActionResult Pika360() => View();

        [HttpGet("/platform/gunun-firsatlari")]
        [HttpGet("/en/platform/opportunities")]
        public IActionResult Opportunities() => View();
    }
}
