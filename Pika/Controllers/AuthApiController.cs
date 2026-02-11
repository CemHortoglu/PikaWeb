using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pika.Models.Auth;

namespace Pika.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public ActionResult<AuthMeResponse> Me()
    {
        var userName = User.Identity?.Name ?? User.FindFirstValue(ClaimTypes.Name);
        var roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).Distinct().ToArray();

        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.Select(c => c.Value).Distinct().ToArray());

        return Ok(new AuthMeResponse
        {
            IsAuthenticated = true,
            UserName = userName,
            Roles = roles,
            Claims = claims
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}
