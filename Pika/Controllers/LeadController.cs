using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [ApiController]
    [Route("lead")]
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
            return Ok(new { ok = true, message = "Demo talebiniz alındı. Ekibimiz en kısa sürede dönüş yapacak." });
        }
    }
}
