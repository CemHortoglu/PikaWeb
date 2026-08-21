namespace Pika.Models.ContactConsent;

public class ContactConsentRequest
{
    public string Token { get; set; } = string.Empty;
    public bool EmailConsent { get; set; }
    public bool SmsConsent { get; set; }
    public string? CfTurnstileResponse { get; set; }
}
