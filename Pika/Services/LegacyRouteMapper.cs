namespace Pika.Services;

/// <summary>
/// One-hop permanent migrations from historical MVC/locale/taxonomy URLs to the
/// public semantic URL contract. Canonical public pages never use these paths.
/// </summary>
public static class LegacyRouteMapper
{
    private static readonly Dictionary<string, string> Redirects = new(StringComparer.OrdinalIgnoreCase)
    {
        // Home / locale history
        ["/tr"] = "/",
        ["/tr/"] = "/",
        ["/home"] = "/",
        ["/home/"] = "/",
        ["/home/index"] = "/",
        ["/tr/home"] = "/",
        ["/tr/home/"] = "/",
        ["/tr/home/index"] = "/",
        ["/en"] = "/en/",
        ["/en/home"] = "/en/",
        ["/en/home/"] = "/en/",
        ["/en/home/index"] = "/en/",

        // HomeController-shaped public history
        ["/home/pika"] = "/pika",
        ["/tr/home/pika"] = "/pika",
        ["/tr/pika"] = "/pika",
        ["/en/home/pika"] = "/en/pika",
        ["/en/about-pika"] = "/en/pika",
        ["/home/faq"] = "/kaynaklar/sss",
        ["/home/sss"] = "/kaynaklar/sss",
        ["/tr/home/faq"] = "/kaynaklar/sss",
        ["/tr/home/sss"] = "/kaynaklar/sss",
        ["/faq"] = "/kaynaklar/sss",
        ["/sss"] = "/kaynaklar/sss",
        ["/tr/faq"] = "/kaynaklar/sss",
        ["/tr/sss"] = "/kaynaklar/sss",
        ["/en/home/faq"] = "/en/resources/faq",
        ["/en/faq"] = "/en/resources/faq",
        ["/home/corporate"] = "/kurumsal",
        ["/tr/home/corporate"] = "/kurumsal",
        ["/tr/kurumsal"] = "/kurumsal",
        ["/corporate"] = "/kurumsal",
        ["/home/contact"] = "/iletisim",
        ["/tr/home/contact"] = "/iletisim",
        ["/tr/iletisim"] = "/iletisim",
        ["/contact"] = "/iletisim",
        ["/home/career"] = "/kariyer",
        ["/tr/home/career"] = "/kariyer",
        ["/tr/kariyer"] = "/kariyer",
        ["/career"] = "/kariyer",
        ["/home/demo-request"] = "/demo-talebi",
        ["/tr/home/demo-request"] = "/demo-talebi",
        ["/tr/demo-talebi"] = "/demo-talebi",
        ["/demo-request"] = "/demo-talebi",
        ["/tr/kullanim-sartlari"] = "/kullanim-sartlari",
        ["/tr/gizlilik-politikasi"] = "/gizlilik-politikasi",

        // PlatformController history
        ["/platform/customerintelligence"] = "/platform/customer-intelligence",
        ["/tr/platform/customer-intelligence"] = "/platform/customer-intelligence",
        ["/tr/platform/customerintelligence"] = "/platform/customer-intelligence",
        ["/platform/productintelligence"] = "/platform/product-intelligence",
        ["/tr/platform/product-intelligence"] = "/platform/product-intelligence",
        ["/tr/platform/productintelligence"] = "/platform/product-intelligence",
        ["/platform/pika360"] = "/platform/pika-360",
        ["/tr/platform/pika-360"] = "/platform/pika-360",
        ["/tr/platform/pika360"] = "/platform/pika-360",
        ["/platform/opportunities"] = "/platform/gunun-firsatlari",
        ["/platform/firsatlar"] = "/platform/gunun-firsatlari",
        ["/tr/platform/opportunities"] = "/platform/gunun-firsatlari",
        ["/tr/platform/firsatlar"] = "/platform/gunun-firsatlari",
        ["/tr/platform/gunun-firsatlari"] = "/platform/gunun-firsatlari",

        // Product capability taxonomy: old solutions/cozumler -> platform
        ["/cozumler/campaign-manager"] = "/platform/campaign-manager",
        ["/solutions/campaign-manager"] = "/platform/campaign-manager",
        ["/tr/solutions/campaign-manager"] = "/platform/campaign-manager",
        ["/tr/cozumler/campaign-manager"] = "/platform/campaign-manager",
        ["/en/solutions/campaign-manager"] = "/en/platform/campaign-manager",

        ["/cozumler/audience-manager"] = "/platform/audience-manager",
        ["/solutions/audience-manager"] = "/platform/audience-manager",
        ["/tr/solutions/audience-manager"] = "/platform/audience-manager",
        ["/tr/cozumler/audience-manager"] = "/platform/audience-manager",
        ["/en/solutions/audience-manager"] = "/en/platform/audience-manager",

        ["/cozumler/journey-manager"] = "/platform/journey-manager",
        ["/solutions/journey-manager"] = "/platform/journey-manager",
        ["/tr/solutions/journey-manager"] = "/platform/journey-manager",
        ["/tr/cozumler/journey-manager"] = "/platform/journey-manager",
        ["/en/solutions/journey-manager"] = "/en/platform/journey-manager",

        ["/cozumler/content-studio"] = "/platform/content-studio",
        ["/solutions/content-studio"] = "/platform/content-studio",
        ["/tr/solutions/content-studio"] = "/platform/content-studio",
        ["/en/solutions/content-studio"] = "/en/platform/content-studio",

        ["/cozumler/consent-management"] = "/platform/consent-management",
        ["/solutions/consent-management"] = "/platform/consent-management",
        ["/tr/solutions/consent-management"] = "/platform/consent-management",
        ["/en/solutions/consent-management"] = "/en/platform/consent-management",

        ["/cozumler/analytics-reporting"] = "/platform/analytics",
        ["/solutions/analytics-reporting"] = "/platform/analytics",
        ["/tr/solutions/analytics-reporting"] = "/platform/analytics",
        ["/en/solutions/analytics-reporting"] = "/en/platform/analytics",

        ["/entegrasyonlar"] = "/platform/integrations",
        ["/tr/entegrasyonlar"] = "/platform/integrations",
        ["/en/integrations"] = "/en/platform/integrations",

        ["/urunler/ai-kampanya-asistani"] = "/platform/ai-kampanya-asistani",
        ["/tr/urunler/ai-kampanya-asistani"] = "/platform/ai-kampanya-asistani",
        ["/en/products/ai-campaign-assistant"] = "/en/platform/ai-campaign-assistant",

        // Channels
        ["/solutions/email-marketing"] = "/kanallar/email",
        ["/tr/solutions/email-marketing"] = "/kanallar/email",
        ["/tr/kanallar/email"] = "/kanallar/email",
        ["/solutions/sms-campaigns"] = "/kanallar/sms",
        ["/tr/solutions/sms-campaigns"] = "/kanallar/sms",
        ["/tr/kanallar/sms"] = "/kanallar/sms",
        ["/solutions/whatsapp-messaging"] = "/kanallar/whatsapp",
        ["/tr/solutions/whatsapp-messaging"] = "/kanallar/whatsapp",
        ["/tr/kanallar/whatsapp"] = "/kanallar/whatsapp",
        ["/solutions/push-notifications"] = "/kanallar/push",
        ["/tr/solutions/push-notifications"] = "/kanallar/push",
        ["/kanallar/push-notification"] = "/kanallar/push",
        ["/tr/kanallar/push"] = "/kanallar/push",

        // Trust / resources
        ["/solutions/security-privacy"] = "/guvenlik-ve-gizlilik",
        ["/tr/solutions/security-privacy"] = "/guvenlik-ve-gizlilik",
        ["/tr/guvenlik-ve-gizlilik"] = "/guvenlik-ve-gizlilik",
        ["/tr/kullanim-senaryolari"] = "/kullanim-senaryolari",

        // Consolidate older thin/specialised marketing pages into their durable parent pages.
        ["/cozumler/personalization"] = "/platform/customer-intelligence",
        ["/cozumler/template-management"] = "/platform/content-studio",
        ["/cozumler/ab-testing"] = "/platform/campaign-manager",
        ["/cozumler/deliverability-compliance"] = "/kanallar/email",
        ["/cozumler/data-management-etl"] = "/platform/integrations",
        ["/cozumler/real-time-event-processing"] = "/platform/journey-manager",
        ["/cozumler/e-ticaret-ai-kampanya-yonetimi"] = "/kullanim-senaryolari",
        ["/kanallar/whatsapp-kampanya-yonetimi"] = "/kanallar/whatsapp",
        ["/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi"] = "/platform/consent-management",
        ["/kanallar/email-marketing-template-studio"] = "/platform/content-studio",
        ["/en/solutions/personalization"] = "/en/platform/customer-intelligence",
        ["/en/solutions/template-management"] = "/en/platform/content-studio",
        ["/en/solutions/ab-testing"] = "/en/platform/campaign-manager",
        ["/en/solutions/deliverability-compliance"] = "/en/channels/email",
        ["/en/solutions/data-management-etl"] = "/en/platform/integrations",
        ["/en/solutions/real-time-event-processing"] = "/en/platform/journey-manager",
        ["/en/solutions/ecommerce-ai-campaign"] = "/en/use-cases",
        ["/en/channels/whatsapp-campaign-management"] = "/en/channels/whatsapp",
        ["/en/solutions/iys-kvkk-compliance"] = "/en/platform/consent-management",
        ["/en/channels/email-marketing-template-studio"] = "/en/platform/content-studio",

        // Wiki shell history
        ["/wiki/index"] = "/wiki/",
        ["/wiki/index.html"] = "/wiki/"
    };

    public static bool TryGetRedirect(string? path, out string? target)
    {
        target = null;
        if (string.IsNullOrWhiteSpace(path)) return false;

        var normalized = Normalize(path);
        if (!Redirects.TryGetValue(normalized, out var mapped)) return false;
        if (string.Equals(normalized, Normalize(mapped), StringComparison.OrdinalIgnoreCase)) return false;

        target = mapped;
        return true;
    }

    private static string Normalize(string path)
    {
        if (path.Length > 1 && path.EndsWith('/')) path = path.TrimEnd('/');
        return string.IsNullOrEmpty(path) ? "/" : path;
    }
}
