using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [ApiController]
    [Route("{culture:regex(^(tr|en)$)}/lead")]
    public class LeadController : ControllerBase
    {
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
            var message = dto.Lang?.ToLowerInvariant() == "en"
                ? "Your demo request has been received. Our team will contact you shortly."
                : "Demo talebiniz alındı. Ekibimiz en kısa sürede dönüş yapacak.";

            return Ok(new { ok = true, message });
        }
    }
}
