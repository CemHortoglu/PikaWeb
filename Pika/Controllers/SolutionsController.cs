using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    [Route("{culture:regex(^(tr|en)$)}/solutions")]
    public class SolutionsController : Controller
    {
        [HttpGet("email-marketing")]
        [HttpGet("[action]")]
        public IActionResult EmailMarketing() => View();

        [HttpGet("sms-campaigns")]
        [HttpGet("[action]")]
        public IActionResult SmsCampaigns() => View();

        [HttpGet("whatsapp-messaging")]
        [HttpGet("[action]")]
        public IActionResult WhatsAppMessaging() => View();

        [HttpGet("push-notifications")]
        [HttpGet("[action]")]
        public IActionResult PushNotifications() => View();

        [HttpGet("in-app-messaging")]
        [HttpGet("[action]")]
        public IActionResult InAppMessaging() => View();

        [HttpGet("personalization")]
        [HttpGet("[action]")]
        public IActionResult PersonalizationPage() => View("Personalization");

        [HttpGet("template-management")]
        [HttpGet("[action]")]
        public IActionResult TemplateManagement() => View();

        [HttpGet("ab-testing")]
        [HttpGet("[action]")]
        public IActionResult ABTesting() => View();

        [HttpGet("reporting")]
        public IActionResult Reporting() => View();

        [HttpGet("deliverability-compliance")]
        [HttpGet("[action]")]
        public IActionResult DeliverabilityCompliance() => View();

        [HttpGet("data-management-etl")]
        [HttpGet("[action]")]
        public IActionResult DataManagementEtl() => View();

        [HttpGet("real-time-event-processing")]
        [HttpGet("[action]")]
        public IActionResult RealTimeEventProcessing() => View();


        [HttpGet("integrations")]
        [HttpGet("[action]")]
        public IActionResult Integrations() => View();

        [HttpGet("security-privacy")]
        [HttpGet("[action]")]
        public IActionResult SecurityPrivacy() => View();
    }
}
