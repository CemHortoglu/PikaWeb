using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pika.Configuration;
using Pika.Models.Auth;

namespace Pika.Controllers;

[Route("[controller]")]
public class AccountController : Controller
{
    private readonly AuthSettings _authSettings;

    public AccountController(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings.Value;
    }

    [HttpGet("Login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return Redirect(GetSafeReturnUrl(returnUrl));
        }

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!IsValidUser(model.Username, model.Password))
        {
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, model.Username),
            new(ClaimTypes.NameIdentifier, model.Username)
        };

        foreach (var role in _authSettings.DemoUser.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                AllowRefresh = true
            });

        return Redirect(GetSafeReturnUrl(model.ReturnUrl));
    }

    [Authorize]
    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied() => View();

    private bool IsValidUser(string username, string password)
    {
        return string.Equals(username, _authSettings.DemoUser.Username, StringComparison.OrdinalIgnoreCase)
               && string.Equals(password, _authSettings.DemoUser.Password, StringComparison.Ordinal);
    }

    private string GetSafeReturnUrl(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return _authSettings.AngularAppUrl;
    }
}
