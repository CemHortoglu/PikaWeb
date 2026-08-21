using System.Net;
using System.Net.Http;
using System.Text.Json;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRecaptchaService _recaptchaService;

        public LeadController(
            IOptions<SiteSettings> siteOptions,
            IEmailService emailService,
            ILogger<LeadController> logger,
            IHttpClientFactory httpClientFactory,
            IRecaptchaService recaptchaService)
        {
            _siteSettings = siteOptions.Value;
            _emailService = emailService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _recaptchaService = recaptchaService;
        }

        public record DemoRequestDto(
            string FirstName,
            string LastName,
            string Email,
            string Phone,
            string CompanyName,
            string Website,
            string City,
            string Message,
            string? Lang,
            string? RecaptchaToken);

        public record ContactRequestDto(
            string FullName,
            string Email,
            string Message,
            string? Lang,
            string? RecaptchaToken);

        public record CareerRequestDto(
            string FullName,
            string Email,
            string Phone,
            string Position,
            string CvUrl,
            string Message,
            string? Lang,
            string? RecaptchaToken);

        [HttpPost("demo-request")]
        public async Task<IActionResult> DemoRequest([FromBody] DemoRequestDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Phone) ||
                string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                return BadRequest(new { ok = false, message = "Zorunlu alanları doldurun." });
            }

            var captchaValid = await _recaptchaService.ValidateTokenAsync(dto.RecaptchaToken ?? string.Empty);
            if (!captchaValid)
            {
                var lang0 = dto.Lang?.ToLowerInvariant() == "en" ? "en" : "tr";
                var captchaMsg = lang0 == "en"
                    ? "Captcha verification failed. Please try again."
                    : "Captcha doğrulaması başarısız. Lütfen tekrar deneyin.";
                return BadRequest(new { ok = false, message = captchaMsg });
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
                <p><b>Şehir:</b> {WebUtility.HtmlEncode(dto.City)}</p>
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

        private async Task<bool> VerifyTurnstileAsync(string? token, string secret, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Turnstile token boş veya null geldi");
                return false;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                var response = await client.PostAsync(
                    "https://challenges.cloudflare.com/turnstile/v0/siteverify",
                    new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["secret"] = secret,
                        ["response"] = token,
                        ["remoteip"] = remoteIp
                    }),
                    ct);

                var json = await response.Content.ReadAsStringAsync(ct);
                _logger.LogInformation("Turnstile yanıtı: {Response}", json);

                using var doc = JsonDocument.Parse(json);
                var success = doc.RootElement.TryGetProperty("success", out var successProp) && successProp.GetBoolean();
                if (!success)
                {
                    var errorCodes = doc.RootElement.TryGetProperty("error-codes", out var errProp)
                        ? errProp.ToString()
                        : "yok";
                    _logger.LogWarning("Turnstile doğrulama başarısız. Error codes: {ErrorCodes}", errorCodes);
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Turnstile doğrulaması sırasında hata oluştu");
                return false;
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

            var captchaValid = await _recaptchaService.ValidateTokenAsync(dto.RecaptchaToken ?? string.Empty);
            if (!captchaValid)
            {
                var lang0 = dto.Lang?.ToLowerInvariant() == "en" ? "en" : "tr";
                var captchaMsg = lang0 == "en"
                    ? "Captcha verification failed. Please try again."
                    : "Captcha doğrulaması başarısız. Lütfen tekrar deneyin.";
                return BadRequest(new { ok = false, message = captchaMsg });
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

        [HttpPost("career")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Career([FromForm] CareerRequestDto dto, IFormFile? cvFile, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Phone) ||
                string.IsNullOrWhiteSpace(dto.Position))
            {
                return BadRequest(new { ok = false, message = "Zorunlu alanları doldurun." });
            }

            if (cvFile is not null)
            {
                var ext = Path.GetExtension(cvFile.FileName).ToLowerInvariant();
                if (ext != ".pdf")
                {
                    return BadRequest(new { ok = false, message = "Sadece PDF dosyası yükleyebilirsiniz." });
                }
                if (cvFile.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { ok = false, message = "CV dosyası en fazla 5 MB olabilir." });
                }
            }

            var captchaValid = await _recaptchaService.ValidateTokenAsync(dto.RecaptchaToken ?? string.Empty);
            if (!captchaValid)
            {
                var lang0 = dto.Lang?.ToLowerInvariant() == "en" ? "en" : "tr";
                var captchaMsg = lang0 == "en"
                    ? "Captcha verification failed. Please try again."
                    : "Captcha doğrulaması başarısız. Lütfen tekrar deneyin.";
                return BadRequest(new { ok = false, message = captchaMsg });
            }

            var lang = dto.Lang?.ToLowerInvariant() == "en" ? "en" : "tr";
            var recipient = _siteSettings.Mail.CareerFormRecipientEmail;
            var subject = lang == "en"
                ? $"[Career Application] {dto.FullName} - {dto.Position}"
                : $"[Kariyer Başvurusu] {dto.FullName} - {dto.Position}";

            var body = $@"
                <h2>{(lang == "en" ? "New Career Application" : "Yeni Kariyer Başvurusu")}</h2>
                <p><b>{(lang == "en" ? "Full Name" : "Ad Soyad")}:</b> {WebUtility.HtmlEncode(dto.FullName)}</p>
                <p><b>{(lang == "en" ? "Email" : "E-posta")}:</b> {WebUtility.HtmlEncode(dto.Email)}</p>
                <p><b>{(lang == "en" ? "Phone" : "Telefon")}:</b> {WebUtility.HtmlEncode(dto.Phone)}</p>
                <p><b>{(lang == "en" ? "Position" : "Başvurulan Pozisyon")}:</b> {WebUtility.HtmlEncode(dto.Position)}</p>
                <p><b>{(lang == "en" ? "CV / LinkedIn" : "CV / LinkedIn")}:</b> {WebUtility.HtmlEncode(dto.CvUrl ?? "")}</p>
                <p><b>{(lang == "en" ? "CV File" : "CV Dosyası")}:</b> {(cvFile is not null ? WebUtility.HtmlEncode(cvFile.FileName) : "-")}</p>
                <p><b>{(lang == "en" ? "Message" : "Mesaj")}:</b><br/>{WebUtility.HtmlEncode(dto.Message ?? "")}</p>";

            System.Net.Mail.Attachment? attachment = null;
            Stream? cvStream = null;
            try
            {
                List<System.Net.Mail.Attachment>? attachments = null;
                if (cvFile is not null)
                {
                    cvStream = cvFile.OpenReadStream();
                    attachment = new System.Net.Mail.Attachment(cvStream, cvFile.FileName, "application/pdf");
                    attachments = [attachment];
                }

                await _emailService.SendAsync(recipient, subject, body, attachments, cancellationToken);

                var successMessage = lang == "en"
                    ? "Your application has been received. We will review it and get back to you."
                    : "Başvurunuz alındı. İnceleyip size dönüş yapacağız.";

                return Ok(new { ok = true, message = successMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kariyer başvurusu e-postası gönderilemedi. Recipient: {RecipientEmail}", recipient);
                var errorMessage = lang == "en"
                    ? "Your application could not be sent right now. Please try again later."
                    : "Başvurunuz şu anda gönderilemedi. Lütfen daha sonra tekrar deneyin.";
                return StatusCode(StatusCodes.Status500InternalServerError, new { ok = false, message = errorMessage });
            }
            finally
            {
                attachment?.Dispose();
                if (cvStream is not null) await cvStream.DisposeAsync();
            }
        }
    }
}
