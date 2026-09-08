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
        public IActionResult PersonalizationPage() => RedirectPermanent("/cozumler/journey-manager");

        [HttpGet("/en/solutions/personalization")]
        public IActionResult PersonalizationPageEn() => RedirectPermanent("/en/solutions/journey-manager");

        [HttpGet("/cozumler/template-management")]
        public IActionResult TemplateManagement() => RedirectPermanent("/cozumler/content-studio");

        [HttpGet("/en/solutions/template-management")]
        public IActionResult TemplateManagementEn() => RedirectPermanent("/en/solutions/content-studio");

        [HttpGet("/cozumler/ab-testing")]
        public IActionResult ABTesting() => RedirectPermanent("/cozumler/campaign-manager");

        [HttpGet("/en/solutions/ab-testing")]
        public IActionResult ABTestingEn() => RedirectPermanent("/en/solutions/campaign-manager");

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
        public IActionResult EcommerceAiCampaign() => RedirectPermanent("/urunler/ai-kampanya-asistani");

        [HttpGet("/en/solutions/ecommerce-ai-campaign", Name = "EcommerceAiCampaignEn")]
        public IActionResult EcommerceAiCampaignEn() => RedirectPermanent("/en/products/ai-campaign-assistant");

        [HttpGet("/kanallar/whatsapp-kampanya-yonetimi", Name = "WhatsAppCampaignManagement")]
        public IActionResult WhatsAppCampaignManagement() => RedirectPermanent("/kanallar/whatsapp");

        [HttpGet("/en/channels/whatsapp-campaign-management", Name = "WhatsAppCampaignManagementEn")]
        public IActionResult WhatsAppCampaignManagementEn() => RedirectPermanent("/en/channels/whatsapp");

        [HttpGet("/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi", Name = "IysKvkkCompliance")]
        public IActionResult IysKvkkCompliance() => RedirectPermanent("/cozumler/consent-management");

        [HttpGet("/en/solutions/iys-kvkk-compliance", Name = "IysKvkkComplianceEn")]
        public IActionResult IysKvkkComplianceEn() => RedirectPermanent("/en/solutions/consent-management");

        [HttpGet("/kanallar/email-marketing-template-studio", Name = "EmailMarketingTemplateStudio")]
        public IActionResult EmailMarketingTemplateStudio() => RedirectPermanent("/cozumler/content-studio");

        [HttpGet("/en/channels/email-marketing-template-studio", Name = "EmailMarketingTemplateStudioEn")]
        public IActionResult EmailMarketingTemplateStudioEn() => RedirectPermanent("/en/solutions/content-studio");

        [HttpGet("/kullanim-senaryolari", Name = "UseCases")]
        [HttpGet("/en/use-cases", Name = "UseCasesEn")]
        public IActionResult UseCases() => View();

        [HttpGet("/kullanim-senaryolari/tekrar-satin-alma", Name = "RepeatPurchaseUseCase")]
        [HttpGet("/en/use-cases/repeat-purchase", Name = "RepeatPurchaseUseCaseEn")]
        public IActionResult RepeatPurchaseUseCase() => View();

        [HttpGet("/kullanim-senaryolari/capraz-satis", Name = "CrossSellUseCase")]
        [HttpGet("/en/use-cases/cross-sell", Name = "CrossSellUseCaseEn")]
        public IActionResult CrossSellUseCase() => View();

        [HttpGet("/kullanim-senaryolari/geri-kazanim", Name = "WinBackUseCase")]
        [HttpGet("/en/use-cases/win-back", Name = "WinBackUseCaseEn")]
        public IActionResult WinBackUseCase() => View();

        [HttpGet("/kullanim-senaryolari/musteri-segmentasyonu", Name = "CustomerSegmentationUseCase")]
        [HttpGet("/en/use-cases/customer-segmentation", Name = "CustomerSegmentationUseCaseEn")]
        public IActionResult CustomerSegmentationUseCase() => View();

        [HttpGet("/kullanim-senaryolari/musteri-degeri", Name = "CustomerValueUseCase")]
        [HttpGet("/en/use-cases/customer-value", Name = "CustomerValueUseCaseEn")]
        public IActionResult CustomerValueUseCase() => View();
    }
}
