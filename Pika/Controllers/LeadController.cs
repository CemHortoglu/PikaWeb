using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pika.Configuration;

namespace Pika.Controllers
{
    [ApiController]
    [Route("{culture:regex(^(tr|en)$)}/lead")]
    public class LeadController : ControllerBase
    {
        private readonly SiteSettings _siteSettings;
        private readonly ILogger<LeadController> _logger;

        public LeadController(IOptions<SiteSettings> siteOptions, ILogger<LeadController> logger)
        {
            _siteSettings = siteOptions.Value;
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

        [HttpPost("demo-request")]
        public IActionResult DemoRequest([FromBody] DemoRequestDto dto)
        {
            _logger.LogInformation(
                "Demo request received for {RecipientEmail}. FullName: {FirstName} {LastName}, Company: {CompanyName}, Email: {Email}, Phone: {Phone}",
                _siteSettings.Mail.DemoRequestRecipientEmail,
                dto.FirstName,
                dto.LastName,
                dto.CompanyName,
                dto.Email,
                dto.Phone);

            var message = dto.Lang?.ToLowerInvariant() == "en"
                ? "Your demo request has been received. Our team will contact you shortly."
                : "Demo talebiniz alındı. Ekibimiz en kısa sürede dönüş yapacak.";

            return Ok(new { ok = true, message });
        }
    }
}
