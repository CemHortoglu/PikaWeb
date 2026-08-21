using Microsoft.AspNetCore.Mvc;

namespace Pika.Controllers;

/// <summary>
/// Public marketing actions. Route names are an explicit public contract and are
/// intentionally decoupled from controller/action names.
/// </summary>
public class SolutionsController : Controller
{
    [HttpGet("/platform/campaign-manager")]
    [HttpGet("/en/platform/campaign-manager")]
    public IActionResult CampaignManager() => View();

    [HttpGet("/platform/audience-manager")]
    [HttpGet("/en/platform/audience-manager")]
    public IActionResult AudienceManager() => View();

    [HttpGet("/platform/journey-manager")]
    [HttpGet("/en/platform/journey-manager")]
    public IActionResult JourneyManager() => View();

    [HttpGet("/platform/content-studio")]
    [HttpGet("/en/platform/content-studio")]
    public IActionResult ContentStudio() => View();

    [HttpGet("/platform/consent-management")]
    [HttpGet("/en/platform/consent-management")]
    public IActionResult ConsentManagement() => View();

    [HttpGet("/platform/analytics")]
    [HttpGet("/en/platform/analytics")]
    public IActionResult Reporting() => View();

    [HttpGet("/platform/integrations")]
    [HttpGet("/en/platform/integrations")]
    public IActionResult Integrations() => View();

    [HttpGet("/platform/ai-kampanya-asistani")]
    [HttpGet("/en/platform/ai-campaign-assistant")]
    public IActionResult AiCampaignAssistant() => View();

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

    [HttpGet("/guvenlik-ve-gizlilik")]
    [HttpGet("/en/security-and-privacy")]
    public IActionResult SecurityPrivacy() => View();

    [HttpGet("/kullanim-senaryolari")]
    [HttpGet("/en/use-cases")]
    public IActionResult UseCases() => View();
}
