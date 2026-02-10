using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Pika.Configuration;

namespace Pika.Services;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default);
}

public sealed class EmailService : IEmailService
{
    private readonly SiteSettings _siteSettings;

    public EmailService(IOptions<SiteSettings> siteOptions)
    {
        _siteSettings = siteOptions.Value;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var smtp = _siteSettings.Mail.Smtp;
        if (string.IsNullOrWhiteSpace(smtp.Host) || string.IsNullOrWhiteSpace(smtp.FromEmail))
        {
            throw new InvalidOperationException("SMTP ayarları eksik. appsettings.json > SiteSettings:Mail:Smtp alanlarını doldurun.");
        }

        using var message = new MailMessage
        {
            From = new MailAddress(smtp.FromEmail, smtp.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        using var client = new SmtpClient(smtp.Host, smtp.Port)
        {
            EnableSsl = smtp.EnableSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(smtp.Username, smtp.Password)
        };

        cancellationToken.ThrowIfCancellationRequested();
        await client.SendMailAsync(message, cancellationToken);
    }
}
