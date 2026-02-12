using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Text.Json;
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
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly AuthSettings _authSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IOptions<AuthSettings> authSettings, IHttpClientFactory httpClientFactory)
    {
        _authSettings = authSettings.Value;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("Login")]
    public IActionResult Login(string? returnUrl = null)
    {
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

        var loginResult = await LoginAgainstPublishApi(model.Username, model.Password);
        if (loginResult?.Token is null || string.IsNullOrWhiteSpace(loginResult.Token.AccessToken))
        {
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }

        var claims = BuildClaimsFromJwt(loginResult.Token.AccessToken);
        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
            claims.Add(new Claim(ClaimTypes.Name, model.Username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Username));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            AllowRefresh = true
        };

        authProperties.StoreTokens(
        [
            new AuthenticationToken { Name = "access_token", Value = loginResult.Token.AccessToken },
            new AuthenticationToken { Name = "refresh_token", Value = loginResult.Token.RefreshToken },
            new AuthenticationToken { Name = "expires_at", Value = loginResult.Token.Expiration?.ToString("O") ?? string.Empty },
            new AuthenticationToken { Name = "refresh_expires_at", Value = loginResult.Token.RefreshTokenExpiration?.ToString("O") ?? string.Empty },
            new AuthenticationToken { Name = "language_code", Value = loginResult.LanguageCode ?? "tr" }
        ]);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);

        var credentials = $"{model.Username}:{model.Password}";
        var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
        var redirectUrl = BuildPublishLoginUrl(encodedToken);

        return Redirect(redirectUrl);
    }

    [Authorize]
    [HttpGet("AccessDenied")]
    public IActionResult AccessDenied() => View();

    private async Task<ExternalLoginResponse?> LoginAgainstPublishApi(string username, string password)
    {
        var client = _httpClientFactory.CreateClient();

        var requestBody = JsonSerializer.Serialize(new { username, password });
        using var request = new HttpRequestMessage(HttpMethod.Post, _authSettings.ApiLoginUrl)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("accept", "*/*");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExternalLoginResponse>(content, JsonOptions);
    }

    private string BuildPublishLoginUrl(string encodedToken)
    {
        var publishBaseUrl = string.IsNullOrWhiteSpace(_authSettings.PublishAppUrl)
            ? "https://app.publish.tr"
            : _authSettings.PublishAppUrl;

        if (!Uri.TryCreate(publishBaseUrl, UriKind.Absolute, out var publishBaseUri))
        {
            publishBaseUri = new Uri("https://app.publish.tr");
        }

        var loginUri = new Uri(publishBaseUri, "/login");
        return $"{loginUri}?token={UrlEncoder.Default.Encode(encodedToken)}";
    }

    private static List<Claim> BuildClaimsFromJwt(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(accessToken))
        {
            return [];
        }

        var jwt = handler.ReadJwtToken(accessToken);
        var claims = jwt.Claims.ToList();

        var userName = claims.FirstOrDefault(c => c.Type is "Username" or ClaimTypes.Name)?.Value;
        if (!string.IsNullOrWhiteSpace(userName) && claims.All(c => c.Type != ClaimTypes.Name))
        {
            claims.Add(new Claim(ClaimTypes.Name, userName));
        }

        var role = claims.FirstOrDefault(c => c.Type is "role" or "RoleName" or ClaimTypes.Role)?.Value;
        if (!string.IsNullOrWhiteSpace(role) && claims.All(c => c.Type != ClaimTypes.Role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var userId = claims.FirstOrDefault(c => c.Type is "UserId" or ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrWhiteSpace(userId) && claims.All(c => c.Type != ClaimTypes.NameIdentifier))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
        }

        return claims;
    }

}
