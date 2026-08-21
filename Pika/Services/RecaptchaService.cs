using Microsoft.Extensions.Options;
using Pika.Configuration;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Pika.Services;

public class RecaptchaService : IRecaptchaService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RecaptchaSettings _settings;
    private readonly ILogger<RecaptchaService> _logger;

    public RecaptchaService(IHttpClientFactory httpClientFactory, IOptions<RecaptchaSettings> settings, ILogger<RecaptchaService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        if (!_settings.IsEnabled)
            return true;

        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_settings.SecretKey}&response={token}", null);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RecaptchaVerificationResponse>(stringResponse);

                return result != null && result.Success && result.Score >= 0.5;
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error validating reCAPTCHA token.");
        }

        return false;
    }
}

public class RecaptchaVerificationResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("success")]
    public bool Success { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("score")]
    public double Score { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("action")]
    public string Action { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("challenge_ts")]
    public string ChallengeTs { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("hostname")]
    public string Hostname { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("error-codes")]
    public List<string> ErrorCodes { get; set; }
}