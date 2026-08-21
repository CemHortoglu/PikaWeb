using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Pika.Configuration;

namespace Pika.Services;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default);
    Task SendAsync(string to, string subject, string htmlBody, IReadOnlyList<Attachment>? attachments, CancellationToken cancellationToken = default);
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

        var recipients = to.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (recipients.Length == 0)
        {
            throw new InvalidOperationException("Alıcı e-posta adresi bulunamadı.");
        }

        foreach (var recipient in recipients)
        {
            message.To.Add(recipient);
        }

        using var client = new SmtpClient(smtp.Host, smtp.Port)
        {
            EnableSsl = smtp.EnableSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(smtp.Username, smtp.Password)
        };

        cancellationToken.ThrowIfCancellationRequested();
        await client.SendMailAsync(message, cancellationToken);
    }

    public async Task SendAsync(string to, string subject, string htmlBody, IReadOnlyList<Attachment>? attachments, CancellationToken cancellationToken = default)
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

        var recipients = to.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (recipients.Length == 0)
        {
            throw new InvalidOperationException("Alıcı e-posta adresi bulunamadı.");
        }

        foreach (var recipient in recipients)
        {
            message.To.Add(recipient);
        }

        if (attachments is not null)
        {
            foreach (var att in attachments)
            {
                message.Attachments.Add(att);
            }
        }

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
