namespace Pika.Models.Auth;

public class AuthMeResponse
{
    public bool IsAuthenticated { get; init; }
    public string? UserName { get; init; }
    public string[] Roles { get; init; } = [];
    public Dictionary<string, string[]> Claims { get; init; } = new();
}
