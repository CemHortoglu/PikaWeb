namespace Pika.Configuration;

public class RecaptchaSettings
{
    public bool IsEnabled { get; set; } = false;
    public string SiteKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}