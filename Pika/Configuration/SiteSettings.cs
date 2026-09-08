namespace Pika.Configuration;

public sealed class SiteSettings
{
    public string BrandName { get; set; } = "Pika";
    public string MetaDescription { get; set; } = "Pika — AI destekli omnichannel campaign orchestration platformu. Segmentasyon, otomasyon, kişiselleştirme ve çok kanallı iletişim yönetimi.";
    public string MetaKeywords { get; set; } = "marketing automation, journey orchestration, segmentation";
    public string TopbarSecurityText { get; set; } = "KVKK ve izin yönetimi uyumlu çok kanallı iletişim altyapısı";
    public string ContactFormTitle { get; set; } = "Bize Yazın";

    public ContactSettings Contact { get; set; } = new();
    public MailSettings Mail { get; set; } = new();
    public CloudflareTurnstileSettings CloudflareTurnstile { get; set; } = new();
    public GoogleRecaptchaSettings GoogleRecaptcha { get; set; } = new();
    public SocialSettings Social { get; set; } = new();
}

public sealed class SocialSettings
{
    public string InstagramUrl { get; set; } = "https://www.instagram.com/pika_tr/";
    public string LinkedinUrl { get; set; } = "https://www.linkedin.com/company/pika-tr/";
    public string YoutubeUrl { get; set; } = "https://www.youtube.com/@Pika-tr";
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
    public string Address { get; set; } = "İstanbul, Türkiye";
    public string MapEmbedUrl { get; set; } = string.Empty;
    public string MapLabel { get; set; } = "Haritada aç";
}

public sealed class MailSettings
{
    public string ContactFormRecipientEmail { get; set; } = "info@pika.tr;cemhortoglu@gmail.com;pikaanalysis@gmail.com";
    public string DemoRequestRecipientEmail { get; set; } = "info@pika.tr;cemhortoglu@gmail.com;pikaanalysis@gmail.com";
    public string CareerFormRecipientEmail { get; set; } = "info@pika.tr;cemhortoglu@gmail.com;pikaanalysis@gmail.com";
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
