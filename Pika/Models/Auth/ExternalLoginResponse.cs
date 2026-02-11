using System.Text.Json.Serialization;

namespace Pika.Models.Auth;

public class ExternalLoginResponse
{
    [JsonPropertyName("token")]
    public ExternalTokenPayload? Token { get; set; }

    [JsonPropertyName("languageCode")]
    public string? LanguageCode { get; set; }
}

public class ExternalTokenPayload
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("expiration")]
    public DateTime? Expiration { get; set; }

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonPropertyName("refreshTokenExpiration")]
    public DateTime? RefreshTokenExpiration { get; set; }
}
