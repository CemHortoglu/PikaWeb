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
    [ProducesResponseType<AuthMeResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthMeResponse>> Me()
    {
        return Ok(await BuildAuthResponseAsync());
    }

    [AllowAnonymous]
    [HttpGet("session")]
    [ProducesResponseType<AuthMeResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthMeResponse>> Session()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Ok(new AuthMeResponse
            {
                IsAuthenticated = false,
                UserName = null,
                Roles = [],
                Claims = new Dictionary<string, string[]>(),
                Token = null,
                LanguageCode = null
            });
        }

        return Ok(await BuildAuthResponseAsync());
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    private async Task<AuthMeResponse> BuildAuthResponseAsync()
    {
        var userName = User.Identity?.Name ?? User.FindFirstValue(ClaimTypes.Name);
        var roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).Distinct().ToArray();

        var claims = User.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(g => g.Key, g => g.Select(c => c.Value).Distinct().ToArray());

        var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var token = new AuthTokenInfo
        {
            AccessToken = authResult.Properties?.GetTokenValue("access_token") ?? string.Empty,
            RefreshToken = authResult.Properties?.GetTokenValue("refresh_token") ?? string.Empty,
            Expiration = ParseDate(authResult.Properties?.GetTokenValue("expires_at")),
            RefreshTokenExpiration = ParseDate(authResult.Properties?.GetTokenValue("refresh_expires_at"))
        };

        return new AuthMeResponse
        {
            IsAuthenticated = true,
            UserName = userName,
            Roles = roles,
            Claims = claims,
            Token = token,
            LanguageCode = authResult.Properties?.GetTokenValue("language_code") ?? "tr"
        };
    }

    private static DateTime? ParseDate(string? value)
    {
        if (DateTime.TryParse(value, out var dateTime))
        {
            return dateTime;
        }

        return null;
    }
}
