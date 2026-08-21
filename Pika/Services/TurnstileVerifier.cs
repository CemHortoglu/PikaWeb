using System.Text.Json;
using Microsoft.Extensions.Options;
using Pika.Configuration;

namespace Pika.Services;

public sealed class TurnstileVerifier : ITurnstileVerifier
{
    private const string VerifyEndpoint = "https://challenges.cloudflare.com/turnstile/v0/siteverify";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SiteSettings _siteSettings;
    private readonly ILogger<TurnstileVerifier> _logger;

    public TurnstileVerifier(
        IHttpClientFactory httpClientFactory,
        IOptions<SiteSettings> siteOptions,
        ILogger<TurnstileVerifier> logger)
    {
        _httpClientFactory = httpClientFactory;
        _siteSettings = siteOptions.Value;
        _logger = logger;
    }

    public async Task<bool> VerifyAsync(string? token, string? remoteIp, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning("Turnstile token boş veya null geldi.");
            return false;
        }

        var secret = _siteSettings.CloudflareTurnstile?.SecretKey;
        if (string.IsNullOrWhiteSpace(secret))
        {
            _logger.LogError("Turnstile SecretKey yapılandırılmamış.");
            return false;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(
                VerifyEndpoint,
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["secret"] = secret,
                    ["response"] = token,
                    ["remoteip"] = remoteIp ?? string.Empty
                }),
                cancellationToken);

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);

            var success = doc.RootElement.TryGetProperty("success", out var successProp)
                          && successProp.GetBoolean();

            if (!success)
            {
                var errorCodes = doc.RootElement.TryGetProperty("error-codes", out var errProp)
                    ? errProp.ToString()
                    : "yok";
                _logger.LogWarning("Turnstile doğrulama başarısız. Error codes: {ErrorCodes}", errorCodes);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Turnstile doğrulaması sırasında hata oluştu.");
            return false;
        }
    }
}
