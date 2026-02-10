using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [Route("{culture:regex(^(tr|en)$)}/solutions")]
    public class SolutionsController : Controller
    {
        [HttpGet("email-marketing")]
        public IActionResult EmailMarketing() => View();

        [HttpGet("sms-campaigns")]
        public IActionResult SmsCampaigns() => View();

        [HttpGet("whatsapp-messaging")]
        public IActionResult WhatsAppMessaging() => View();

        [HttpGet("push-notifications")]
        public IActionResult PushNotifications() => View();

        [HttpGet("in-app-messaging")]
        public IActionResult InAppMessaging() => View();

        [HttpGet("personalization")]
        public IActionResult Personalization() => View();

        [HttpGet("template-management")]
        public IActionResult TemplateManagement() => View();

        [HttpGet("ab-testing")]
        public IActionResult ABTesting() => View();

        [HttpGet("reporting")]
        public IActionResult Reporting() => View();

        [HttpGet("deliverability-compliance")]
        public IActionResult DeliverabilityCompliance() => View();

        [HttpGet("data-management-etl")]
        public IActionResult DataManagementEtl() => View();

        [HttpGet("real-time-event-processing")]
        public IActionResult RealTimeEventProcessing() => View();
    }
}
