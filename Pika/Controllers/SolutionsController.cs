using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [Route("{culture:regex(^(tr|en)$)}/solutions")]
    public class SolutionsController : Controller
    {
        [HttpGet("loyalty")]
        public IActionResult Loyalty() => View();

        [HttpGet("campaigns")]
        public IActionResult Campaigns() => View();

        [HttpGet("segmentation")]
        public IActionResult Segmentation() => View();

        [HttpGet("analytics")]
        public IActionResult Analytics() => View();

        [HttpGet("integrations")]
        public IActionResult Integrations() => View();
    }
}
