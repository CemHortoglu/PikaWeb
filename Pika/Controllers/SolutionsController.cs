using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers
{
    public class SolutionsController : Controller
    {
        [HttpGet("/cozumler/campaign-manager")]
        [HttpGet("/en/solutions/campaign-manager")]
        public IActionResult CampaignManager() => View();

        [HttpGet("/cozumler/audience-manager")]
        [HttpGet("/en/solutions/audience-manager")]
        public IActionResult AudienceManager() => View();

        [HttpGet("/cozumler/journey-manager")]
        [HttpGet("/en/solutions/journey-manager")]
        public IActionResult JourneyManager() => View();

        [HttpGet("/cozumler/content-studio")]
        [HttpGet("/en/solutions/content-studio")]
        public IActionResult ContentStudio() => View();

        [HttpGet("/cozumler/consent-management")]
        [HttpGet("/en/solutions/consent-management")]
        public IActionResult ConsentManagement() => View();

        [HttpGet("/kanallar/email")]
        [HttpGet("/en/channels/email")]
        public IActionResult EmailMarketing() => View();

        [HttpGet("/kanallar/sms")]
        [HttpGet("/en/channels/sms")]
        public IActionResult SmsCampaigns() => View();

        [HttpGet("/kanallar/whatsapp")]
        [HttpGet("/en/channels/whatsapp")]
        public IActionResult WhatsAppMessaging() => View();

        [HttpGet("/kanallar/push")]
        [HttpGet("/en/channels/push")]
        public IActionResult PushNotifications() => View();

        [HttpGet("/cozumler/personalization")]
        [HttpGet("/en/solutions/personalization")]
        public IActionResult PersonalizationPage() => View("Personalization");

        [HttpGet("/cozumler/template-management")]
        [HttpGet("/en/solutions/template-management")]
        public IActionResult TemplateManagement() => View();

        [HttpGet("/cozumler/ab-testing")]
        [HttpGet("/en/solutions/ab-testing")]
        public IActionResult ABTesting() => View();

        [HttpGet("/cozumler/analytics-reporting")]
        [HttpGet("/en/solutions/analytics-reporting")]
        public IActionResult Reporting() => View();

        [HttpGet("/cozumler/deliverability-compliance")]
        [HttpGet("/en/solutions/deliverability-compliance")]
        public IActionResult DeliverabilityCompliance() => View();

        [HttpGet("/cozumler/data-management-etl")]
        [HttpGet("/en/solutions/data-management-etl")]
        public IActionResult DataManagementEtl() => View();

        [HttpGet("/cozumler/real-time-event-processing")]
        [HttpGet("/en/solutions/real-time-event-processing")]
        public IActionResult RealTimeEventProcessing() => View();

        [HttpGet("/entegrasyonlar")]
        [HttpGet("/en/integrations")]
        public IActionResult Integrations() => View();

        [HttpGet("/guvenlik-ve-gizlilik")]
        [HttpGet("/en/security-and-privacy")]
        public IActionResult SecurityPrivacy() => View();

        [HttpGet("/urunler/ai-kampanya-asistani", Name = "AiCampaignAssistant")]
        [HttpGet("/en/products/ai-campaign-assistant", Name = "AiCampaignAssistantEn")]
        public IActionResult AiCampaignAssistant() => View();

        [HttpGet("/cozumler/e-ticaret-ai-kampanya-yonetimi", Name = "EcommerceAiCampaign")]
        [HttpGet("/en/solutions/ecommerce-ai-campaign", Name = "EcommerceAiCampaignEn")]
        public IActionResult EcommerceAiCampaign() => View();

        [HttpGet("/kanallar/whatsapp-kampanya-yonetimi", Name = "WhatsAppCampaignManagement")]
        [HttpGet("/en/channels/whatsapp-campaign-management", Name = "WhatsAppCampaignManagementEn")]
        public IActionResult WhatsAppCampaignManagement() => View();

        [HttpGet("/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi", Name = "IysKvkkCompliance")]
        [HttpGet("/en/solutions/iys-kvkk-compliance", Name = "IysKvkkComplianceEn")]
        public IActionResult IysKvkkCompliance() => View();

        [HttpGet("/kanallar/email-marketing-template-studio", Name = "EmailMarketingTemplateStudio")]
        [HttpGet("/en/channels/email-marketing-template-studio", Name = "EmailMarketingTemplateStudioEn")]
        public IActionResult EmailMarketingTemplateStudio() => View();

        [HttpGet("/kullanim-senaryolari", Name = "UseCases")]
        [HttpGet("/en/use-cases", Name = "UseCasesEn")]
        public IActionResult UseCases() => View();
    }
}
