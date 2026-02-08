using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [Route("{culture:regex(^(tr|en)$)}/[controller]")]
    public class AuthController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public IActionResult Login(string email, string password, bool rememberMe)
        {
            // TODO: gerçek auth burada
            TempData["Toast"] = "Giriş denemesi alındı (demo).";
            return RedirectToAction("Index", "Home");
        }
    }
}
