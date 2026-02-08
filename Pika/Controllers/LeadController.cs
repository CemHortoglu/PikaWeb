using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [ApiController]
    [Route("lead")]
    public class LeadController : ControllerBase
    {
        public record DemoRequestDto(string FullName, string Company, string Email, string Phone, string Message, string? Lang);

        [HttpPost("demo-request")]
        public IActionResult DemoRequest([FromBody] DemoRequestDto dto)
        {
            // TODO: DB/CRM/email entegrasyonu
            return Ok(new { ok = true, message = "Demo talebiniz alındı. Ekibimiz en kısa sürede dönüş yapacak." });
        }
    }
}
