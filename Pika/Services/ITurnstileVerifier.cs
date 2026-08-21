namespace Pika.Services;

public interface ITurnstileVerifier
{
    Task<bool> VerifyAsync(string? token, string? remoteIp, CancellationToken cancellationToken = default);
}
