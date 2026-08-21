using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pika.Configuration;
using Pika.Models.ContactConsent;
using Pika.Services;

namespace Pika.Controllers;

[AllowAnonymous]
[Route("{culture:regex(^(tr|en)$)}/contact-consent")]
public class ContactConsentController : Controller
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly SiteSettings _siteSettings;
    private readonly AuthSettings _authSettings;
    private readonly ITurnstileVerifier _turnstileVerifier;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ContactConsentController> _logger;

    public ContactConsentController(
        IOptions<SiteSettings> siteOptions,
        IOptions<AuthSettings> authOptions,
        ITurnstileVerifier turnstileVerifier,
        IHttpClientFactory httpClientFactory,
        ILogger<ContactConsentController> logger)
    {
        _siteSettings = siteOptions.Value;
        _authSettings = authOptions.Value;
        _turnstileVerifier = turnstileVerifier;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet("{token}")]
    public IActionResult Index(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return View("NotFound");
        }

        var vm = new ContactConsentViewModel
        {
            Token = token,
            CloudflareSiteKey = _siteSettings.CloudflareTurnstile?.SiteKey ?? string.Empty
        };

        return View(vm);
    }

    [HttpPost("submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        [FromForm] ContactConsentRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Token))
        {
            return BadRequest(new { ok = false, message = "Geçersiz istek." });
        }

        var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        var captchaValid = await _turnstileVerifier.VerifyAsync(
            request.CfTurnstileResponse,
            remoteIp,
            cancellationToken);

        if (!captchaValid)
        {
            return BadRequest(new
            {
                ok = false,
                message = "Doğrulama başarısız. Lütfen tekrar deneyin."
            });
        }

        if (string.IsNullOrWhiteSpace(_authSettings.ContactConsentSubmitUrl))
        {
            _logger.LogError("ContactConsentSubmitUrl yapılandırılmamış.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ok = false,
                message = "İşlem şu anda tamamlanamıyor. Lütfen daha sonra tekrar deneyin."
            });
        }

        var payload = JsonSerializer.Serialize(new
        {
            token = request.Token,
            emailConsent = request.EmailConsent,
            smsConsent = request.SmsConsent
        });

        try
        {
            var client = _httpClientFactory.CreateClient();
            using var apiRequest = new HttpRequestMessage(HttpMethod.Post, _authSettings.ContactConsentSubmitUrl)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
            apiRequest.Headers.Add("accept", "*/*");

            using var response = await client.SendAsync(apiRequest, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { ok = false, message = "Link geçersiz veya süresi dolmuş." });
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "Contact consent submit başarısız. Status: {Status}, Body: {Body}",
                    response.StatusCode,
                    errorBody);

                return StatusCode((int)response.StatusCode, new
                {
                    ok = false,
                    message = "İşlem tamamlanamadı. Lütfen tekrar deneyin."
                });
            }

            return Ok(new { ok = true, message = "Tercihleriniz kaydedildi. Teşekkür ederiz." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Contact consent submit sırasında hata oluştu.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ok = false,
                message = "Sunucu hatası. Lütfen daha sonra tekrar deneyin."
            });
        }
    }
}
