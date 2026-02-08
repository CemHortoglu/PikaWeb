using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password, bool rememberMe)
        {
            // TODO: gerçek auth burada
            TempData["Toast"] = "Giriş denemesi alındı (demo).";
            return RedirectToAction("Index", "Home");
        }
    }
}
