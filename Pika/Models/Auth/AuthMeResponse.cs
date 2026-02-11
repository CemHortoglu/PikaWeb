namespace Pika.Models.Auth;

public class AuthMeResponse
{
    public bool IsAuthenticated { get; init; }
    public string? UserName { get; init; }
    public string[] Roles { get; init; } = [];
    public Dictionary<string, string[]> Claims { get; init; } = new();
    public AuthTokenInfo? Token { get; init; }
    public string? LanguageCode { get; init; }
}

public class AuthTokenInfo
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime? Expiration { get; init; }
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime? RefreshTokenExpiration { get; init; }
}
