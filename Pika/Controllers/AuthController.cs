using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers;

[Route("{culture:regex(^(tr|en)$)}/[controller]")]
public class AuthController : Controller
{
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        return RedirectToAction("Login", "Account", new { returnUrl });
    }
}
