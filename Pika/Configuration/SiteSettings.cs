namespace Pika.Configuration;

public sealed class SiteSettings
{
    public string BrandName { get; set; } = "Pika";
    public string MetaDescription { get; set; } = "Pika — Kullanıcı sadakat ve kampanya otomasyon platformu.";
    public string MetaKeywords { get; set; } = "marketing automation, journey orchestration, segmentation";
    public string TopbarSecurityText { get; set; } = "Kurumsal güvenlik • KVKK uyumlu süreçler";
    public string ContactFormTitle { get; set; } = "Bize Yazın";

    public ContactSettings Contact { get; set; } = new();
    public MailSettings Mail { get; set; } = new();
}

public sealed class ContactSettings
{
    public string SupportEmail { get; set; } = "info@pikaanalysis.com";
    public string SupportPhone { get; set; } = "+903123254114";
    public string SupportPhoneDisplay { get; set; } = "+90 (312) 325 41 14";
    public string Address { get; set; } = "İstanbul, Türkiye";
    public string MapEmbedUrl { get; set; } = string.Empty;
    public string MapLabel { get; set; } = "Haritada aç";
}

public sealed class MailSettings
{
    public string ContactFormRecipientEmail { get; set; } = "iletisim@pikaanalysis.com";
    public string DemoRequestRecipientEmail { get; set; } = "demo@pikaanalysis.com";
    public SmtpSettings Smtp { get; set; } = new();
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
