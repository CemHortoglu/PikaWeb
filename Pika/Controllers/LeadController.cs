using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pika.Configuration;
using Pika.Services;

namespace Pika.Controllers
{
    [ApiController]
    [Route("{culture:regex(^(tr|en)$)}/lead")]
    public class LeadController : ControllerBase
    {
        private readonly SiteSettings _siteSettings;
        private readonly IEmailService _emailService;
        private readonly ILogger<LeadController> _logger;

        public LeadController(IOptions<SiteSettings> siteOptions, IEmailService emailService, ILogger<LeadController> logger)
        {
            _siteSettings = siteOptions.Value;
            _emailService = emailService;
            _logger = logger;
        }

        public record DemoRequestDto(
            string FirstName,
            string LastName,
            string Email,
            string Phone,
            string CompanyName,
            string Website,
            string Message,
            string? Lang);

        public record ContactRequestDto(
            string FullName,
            string Email,
            string Message,
            string? Lang);

        [HttpPost("demo-request")]
        public async Task<IActionResult> DemoRequest([FromBody] DemoRequestDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Phone) ||
                string.IsNullOrWhiteSpace(dto.CompanyName) ||
                string.IsNullOrWhiteSpace(dto.Website))
            {
                return BadRequest(new { ok = false, message = "Zorunlu alanları doldurun." });
            }

            var lang = dto.Lang?.ToLowerInvariant() == "en" ? "en" : "tr";
            var recipient = _siteSettings.Mail.DemoRequestRecipientEmail;
            var subject = lang == "en"
                ? $"[Demo Request] {dto.CompanyName} - {dto.FirstName} {dto.LastName}"
                : $"[Demo Talebi] {dto.CompanyName} - {dto.FirstName} {dto.LastName}";

            var body = $@"
                <h2>{(lang == "en" ? "New Demo Request" : "Yeni Demo Talebi")}</h2>
                <p><b>Ad Soyad:</b> {WebUtility.HtmlEncode(dto.FirstName)} {WebUtility.HtmlEncode(dto.LastName)}</p>
                <p><b>E-posta:</b> {WebUtility.HtmlEncode(dto.Email)}</p>
                <p><b>Telefon:</b> {WebUtility.HtmlEncode(dto.Phone)}</p>
                <p><b>Şirket:</b> {WebUtility.HtmlEncode(dto.CompanyName)}</p>
                <p><b>Website:</b> {WebUtility.HtmlEncode(dto.Website)}</p>
                <p><b>Mesaj:</b><br/>{WebUtility.HtmlEncode(dto.Message)}</p>";

            try
            {
                await _emailService.SendAsync(recipient, subject, body, cancellationToken);

                var successMessage = lang == "en"
                    ? "Your demo request has been received. Our team will contact you shortly."
                    : "Demo talebiniz alındı. Ekibimiz en kısa sürede dönüş yapacak.";

                return Ok(new { ok = true, message = successMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Demo talebi e-postası gönderilemedi. Recipient: {RecipientEmail}", recipient);
                var errorMessage = lang == "en"
                    ? "Your request could not be sent right now. Please try again later."
                    : "Talebiniz şu anda gönderilemedi. Lütfen daha sonra tekrar deneyin.";
                return StatusCode(StatusCodes.Status500InternalServerError, new { ok = false, message = errorMessage });
            }
        }

        [HttpPost("contact")]
        public async Task<IActionResult> Contact([FromBody] ContactRequestDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest(new { ok = false, message = "Zorunlu alanları doldurun." });
            }

            var lang = dto.Lang?.ToLowerInvariant() == "en" ? "en" : "tr";
            var recipient = _siteSettings.Mail.ContactFormRecipientEmail;
            var subject = lang == "en"
                ? $"[Contact Form] {dto.FullName}"
                : $"[İletişim Formu] {dto.FullName}";

            var body = $@"
                <h2>{(lang == "en" ? "New Contact Form Message" : "Yeni İletişim Formu Mesajı")}</h2>
                <p><b>Ad Soyad:</b> {WebUtility.HtmlEncode(dto.FullName)}</p>
                <p><b>E-posta:</b> {WebUtility.HtmlEncode(dto.Email)}</p>
                <p><b>Mesaj:</b><br/>{WebUtility.HtmlEncode(dto.Message)}</p>";

            try
            {
                await _emailService.SendAsync(recipient, subject, body, cancellationToken);

                var successMessage = lang == "en"
                    ? "Your message has been sent. We will get back to you shortly."
                    : "Mesajınız gönderildi. En kısa sürede dönüş yapacağız.";

                return Ok(new { ok = true, message = successMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İletişim formu e-postası gönderilemedi. Recipient: {RecipientEmail}", recipient);
                var errorMessage = lang == "en"
                    ? "Your message could not be sent right now. Please try again later."
                    : "Mesajınız şu anda gönderilemedi. Lütfen daha sonra tekrar deneyin.";
                return StatusCode(StatusCodes.Status500InternalServerError, new { ok = false, message = errorMessage });
            }
        }
    }
}
