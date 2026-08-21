namespace Pika.Configuration;

public sealed class SiteSettings
{
    public string BrandName { get; set; } = "Pika";
    public string MetaDescription { get; set; } = "Pika; müşteri ve ürün verisini anlamlandırır, Günün Fırsatları'nı görünür kılar ve kontrollü Campaign/Journey aksiyonlarına dönüştürmeye yardımcı olur.";
    public string MetaKeywords { get; set; } = "customer intelligence, product intelligence, campaign manager, journey manager";
    public string TopbarSecurityText { get; set; } = string.Empty;
    public string ContactFormTitle { get; set; } = "Bize Yazın";
    public ContactSettings Contact { get; set; } = new();
    public MailSettings Mail { get; set; } = new();
    public CloudflareTurnstileSettings CloudflareTurnstile { get; set; } = new();
    public GoogleRecaptchaSettings GoogleRecaptcha { get; set; } = new();
}

public sealed class GoogleRecaptchaSettings
{
    public bool IsEnabled { get; set; } = false;
    public string SiteKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}

public sealed class ContactSettings
{
    public string SupportEmail { get; set; } = "info@pika.tr";
    public string SupportPhone { get; set; } = "+903122567278";
    public string SupportPhoneDisplay { get; set; } = "+90 (312) 256 72 78";
    public string Address { get; set; } = string.Empty;
    public string MapEmbedUrl { get; set; } = string.Empty;
    public string MapLabel { get; set; } = "Haritada aç";
}

public sealed class MailSettings
{
    public string ContactFormRecipientEmail { get; set; } = "info@pika.tr";
    public string DemoRequestRecipientEmail { get; set; } = "info@pika.tr";
    public string CareerFormRecipientEmail { get; set; } = "info@pika.tr";
    public SmtpSettings Smtp { get; set; } = new();
}

public sealed class CloudflareTurnstileSettings
{
    public string SiteKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}

public sealed class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Pika Web";
}
