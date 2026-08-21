namespace Pika.Services;

public static class SeoHelper
{
    public const string BaseDomain = "https://pika.tr";

    public record HreflangEntry(string Lang, string Url);
    public record PageSeoMetadata(
        string AlternatePathTr,
        string AlternatePathEn,
        string TitleTr,
        string TitleEn,
        string DescriptionTr,
        string DescriptionEn,
        string? BreadcrumbTitleTr = null,
        string? BreadcrumbTitleEn = null);

    private static readonly Dictionary<string, PageSeoMetadata> RouteMetadata = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Home.Index"] = new("/", "/en/",
            "Pika | Müşteri ve Ürün Zekâsından Doğru Aksiyona",
            "Pika | From Customer & Product Intelligence to Action",
            "Pika; müşteri, ürün ve işlem verisini anlamlandırır, Günün Fırsatları'nı görünür kılar ve yöneticinin seçtiği aksiyonu Campaign, Journey, Email, SMS, WhatsApp ve Push ile uygulamasına yardımcı olur.",
            "Pika turns customer, product and transaction data into understandable intelligence, surfaces Daily Opportunities, and helps teams execute controlled actions through Campaign, Journey and communication channels."),
        ["Home.EnglishIndex"] = new("/", "/en/",
            "Pika | Müşteri ve Ürün Zekâsından Doğru Aksiyona",
            "Pika | From Customer & Product Intelligence to Action",
            "Pika; müşteri, ürün ve işlem verisini anlamlandırır, Günün Fırsatları'nı görünür kılar ve doğru aksiyonu uygulamaya yardımcı olur.",
            "Pika turns customer, product and transaction data into understandable intelligence, surfaces Daily Opportunities, and helps teams execute controlled actions."),
        ["Home.Pika"] = new("/pika", "/en/pika", "Pika Nedir?", "What is Pika?",
            "Pika'nın veriden müşteri ve ürün zekâsına, fırsattan Campaign/Journey aksiyonuna ve ölçüme uzanan çalışma modelini keşfedin.",
            "Understand how Pika connects data, customer and product intelligence, opportunities, controlled Campaign/Journey execution and measurement."),
        ["Home.Corporate"] = new("/kurumsal", "/en/corporate", "Kurumsal", "Company",
            "Pika'nın ürün yaklaşımı, çalışma ilkeleri ve kurumsal iletişim bilgileri.",
            "Pika's product approach, working principles and company information."),
        ["Home.DemoRequest"] = new("/demo-talebi", "/en/demo-request", "Demo Talebi", "Request a Demo",
            "Pika'yı kendi veri ve kullanım senaryonuz üzerinden değerlendirmek için demo talep edin.",
            "Request a Pika demo around your data and use case."),
        ["Home.Faq"] = new("/kaynaklar/sss", "/en/resources/faq", "Sık Sorulan Sorular", "Frequently Asked Questions",
            "Pika, veri, Customer Intelligence, Product Intelligence, Günün Fırsatları, Campaign, Journey, kanallar, AI, izin ve kurulum hakkında sık sorulan sorular.",
            "Frequently asked questions about Pika, data, intelligence, opportunities, Campaign, Journey, channels, AI, consent and onboarding."),
        ["Home.Contact"] = new("/iletisim", "/en/contact", "İletişim", "Contact", "Pika ekibiyle satış, destek ve iş birliği konularında iletişime geçin.", "Contact Pika for sales, support and partnership inquiries."),
        ["Home.Career"] = new("/kariyer", "/en/careers", "Kariyer", "Careers", "Pika kariyer fırsatları ve ekip bilgileri.", "Careers and team opportunities at Pika."),
        ["Home.TermsOfUse"] = new("/kullanim-sartlari", "/en/terms-of-use", "Kullanım Şartları", "Terms of Use", "Pika web sitesi ve hizmetleri için kullanım şartları.", "Terms governing use of the Pika website and services."),
        ["Home.PrivacyPolicy"] = new("/gizlilik-politikasi", "/en/privacy-policy", "Gizlilik Politikası", "Privacy Policy", "Pika web sitesi gizlilik ve kişisel veri bilgilendirmesi.", "Pika website privacy and personal-data information."),

        ["Platform.CustomerIntelligence"] = new("/platform/customer-intelligence", "/en/platform/customer-intelligence", "Customer Intelligence | Müşteri Zekâsı", "Customer Intelligence",
            "Müşteri davranışı, gerçekleşmiş alışveriş bağlamı, değer ve risk sinyallerini birlikte değerlendirerek daha anlamlı hedefleme ve fırsat kararları üretin.",
            "Bring customer behavior, observed purchases, value and risk signals together to support better targeting and opportunity decisions."),
        ["Platform.ProductIntelligence"] = new("/platform/product-intelligence", "/en/platform/product-intelligence", "Product Intelligence | Ürün Zekâsı", "Product Intelligence",
            "Ürünleri kategori, ihtiyaç, ürün rolü ve satın alma ilişkileri bağlamında anlamlandırarak tekrar satın alma ve cross-sell gibi kullanım senaryolarını güçlendirin.",
            "Add category, need, product-role and purchase-relationship context to support repeat-purchase and cross-sell scenarios."),
        ["Platform.Pika360"] = new("/platform/pika-360", "/en/platform/pika-360", "Pika 360 | Müşteri Karar Görünümü", "Pika 360 | Customer Decision View",
            "Müşteri profilini davranış, satın alma, segment, fırsat ve aksiyon bağlamıyla tek görünümde değerlendirin.",
            "Review customer profile, behavior, purchases, segments, opportunities and action context in a single view."),
        ["Platform.Opportunities"] = new("/platform/gunun-firsatlari", "/en/platform/opportunities", "Günün Fırsatları | Pika", "Daily Opportunities | Pika",
            "Pika'nın müşteri ve ürün verisinden aksiyona dönüştürülebilir ticari durumları görünür kıldığı Günün Fırsatları çalışma alanını keşfedin.",
            "Explore Daily Opportunities, where Pika surfaces actionable commercial situations from customer and product context."),

        ["Solutions.AudienceManager"] = new("/platform/audience-manager", "/en/platform/audience-manager", "Audience Manager | Segmentasyon", "Audience Manager | Segmentation",
            "Müşteri zekâsını yönetilebilir hedef kitlelere dönüştürün; doğrulanmış işlem, davranış ve iş kurallarıyla segmentler oluşturun.",
            "Turn customer intelligence into manageable audiences using verified transaction, behavior and business rules."),
        ["Solutions.CampaignManager"] = new("/platform/campaign-manager", "/en/platform/campaign-manager", "Campaign Manager | Kampanya Yönetimi", "Campaign Manager",
            "Hedef kitle, kanal, içerik ve zamanlamayı kontrollü bir kampanya akışında bir araya getirin ve desteklenen sonuçları ölçün.",
            "Bring audience, channel, content and scheduling into a controlled campaign workflow and measure supported outcomes."),
        ["Solutions.JourneyManager"] = new("/platform/journey-manager", "/en/platform/journey-manager", "Journey Manager | Müşteri Akışları", "Journey Manager",
            "Tetikleyici, karar, bekleme ve aksiyon adımlarıyla çok aşamalı müşteri akışları tasarlayın.",
            "Design multi-step customer flows using triggers, decisions, waits and actions."),
        ["Solutions.ContentStudio"] = new("/platform/content-studio", "/en/platform/content-studio", "Content Studio | İçerik Tasarımı", "Content Studio",
            "Kampanya ve Journey iletişim içeriklerini düzenleyin, yeniden kullanılabilir şablonlarla operasyonu sadeleştirin.",
            "Create and manage communication content for Campaign and Journey workflows with reusable templates."),
        ["Solutions.ConsentManagement"] = new("/platform/consent-management", "/en/platform/consent-management", "Consent Management | İzin ve Kanal Uygunluğu", "Consent Management",
            "Kanal izni, opt-out, ulaşılabilirlik ve İYS bağlamını gönderim kararlarında birlikte değerlendirin. Pika hukuki danışmanlık veya mutlak uyum garantisi sunmaz.",
            "Evaluate channel consent, opt-out, reachability and IYS context before delivery. Pika does not replace legal advice or provide an absolute compliance guarantee."),
        ["Solutions.Reporting"] = new("/platform/analytics", "/en/platform/analytics", "Analytics | Ölçüm ve Raporlama", "Analytics | Measurement & Reporting",
            "Kampanya, Journey, kanal ve desteklenen satış/etkileşim sonuçlarını aynı ölçüm çerçevesinde değerlendirin.",
            "Review Campaign, Journey, channel and supported sales/engagement outcomes in a consistent measurement layer."),
        ["Solutions.Integrations"] = new("/platform/integrations", "/en/platform/integrations", "Entegrasyonlar | Pika", "Integrations | Pika",
            "Excel/CSV ile kontrollü başlangıçtan düzenli API veri akışına kadar Pika'nın veri entegrasyonu yaklaşımını inceleyin.",
            "Explore Pika's data-integration approach from controlled Excel/CSV onboarding to regular API data flows."),
        ["Solutions.AiCampaignAssistant"] = new("/platform/ai-kampanya-asistani", "/en/platform/ai-campaign-assistant", "Pika AI Kampanya Asistanı", "Pika AI Campaign Assistant",
            "Pika AI, hesaplanmış verileri açıklama, kampanya yaklaşımı ve içerik taslağı hazırlama gibi görevlerde kullanıcıya yardımcı olur; aksiyon kontrolünü kullanıcıdan devralmaz.",
            "Pika AI helps explain calculated context and prepare campaign approaches or content drafts while keeping execution under user control."),

        ["Solutions.EmailMarketing"] = new("/kanallar/email", "/en/channels/email", "Email | Pika Kanalları", "Email | Pika Channels", "Pika'da e-posta; fırsat, segment, Campaign ve Journey kararlarının uygulanabildiği iletişim kanallarından biridir.", "In Pika, email is an execution channel for opportunity, audience, Campaign and Journey decisions."),
        ["Solutions.SmsCampaigns"] = new("/kanallar/sms", "/en/channels/sms", "SMS | Pika Kanalları", "SMS | Pika Channels", "Pika'da SMS kanalını izin, uygunluk, hedef kitle ve zamanlama bağlamıyla yönetin.", "Use SMS in Pika with consent, eligibility, audience and timing context."),
        ["Solutions.WhatsAppMessaging"] = new("/kanallar/whatsapp", "/en/channels/whatsapp", "WhatsApp | Pika Kanalları", "WhatsApp | Pika Channels", "Pika'da WhatsApp kanalını izin, şablon ve Campaign/Journey bağlamıyla yönetin.", "Use WhatsApp in Pika within consent, template and Campaign/Journey workflows."),
        ["Solutions.PushNotifications"] = new("/kanallar/push", "/en/channels/push", "Push | Pika Kanalları", "Push | Pika Channels", "Pika'da Push bildirimlerini uygun hedef kitle ve Journey/Campaign bağlamında kullanın.", "Use Push notifications with suitable audiences and Journey/Campaign context in Pika."),
        ["Solutions.SecurityPrivacy"] = new("/guvenlik-ve-gizlilik", "/en/security-and-privacy", "Güvenlik ve Gizlilik | Pika", "Security & Privacy | Pika", "Pika'nın doğrulanmış erişim, izin, opt-out, veri ve operasyon güvenliği yaklaşımını; ürün ile müşteri sorumluluk sınırlarını inceleyin.", "Review Pika's verified approach to access, consent, opt-out, data and operational safeguards, including product/customer responsibility boundaries."),
        ["Solutions.UseCases"] = new("/kullanim-senaryolari", "/en/use-cases", "Kullanım Senaryoları | Pika", "Use Cases | Pika", "Tekrar satın alma, pasifleşme, cross-sell, Journey, kanal seçimi ve ölçüm gibi gerçek iş sorularının Pika içinde nasıl ele alındığını görün.", "See how Pika addresses real business questions such as repeat purchase, inactivity, cross-sell, Journey automation, channel choice and measurement.")
    };

    public static PageSeoMetadata? GetMetadata(string? controller, string? action)
    {
        if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action)) return null;
        var key = $"{controller}.{action}";
        return RouteMetadata.TryGetValue(key, out var meta) ? meta : null;
    }

    public static string? GetCanonicalUrl(string? controller, string? action, string culture, string? explicitCanonical = null)
    {
        if (!string.IsNullOrWhiteSpace(explicitCanonical))
            return explicitCanonical.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? explicitCanonical
                : $"{BaseDomain}{(explicitCanonical.StartsWith('/') ? explicitCanonical : "/" + explicitCanonical)}";

        var meta = GetMetadata(controller, action);
        if (meta == null) return null;
        var path = culture.Equals("en", StringComparison.OrdinalIgnoreCase) ? meta.AlternatePathEn : meta.AlternatePathTr;
        return $"{BaseDomain}{path}";
    }

    public static List<HreflangEntry> GetHreflangAlternates(string? controller, string? action)
    {
        var meta = GetMetadata(controller, action);
        if (meta == null) return [];
        return
        [
            new("tr", $"{BaseDomain}{meta.AlternatePathTr}"),
            new("en", $"{BaseDomain}{meta.AlternatePathEn}"),
            new("x-default", $"{BaseDomain}{meta.AlternatePathTr}")
        ];
    }

    public static IReadOnlyCollection<PageSeoMetadata> GetAllPublicMetadata() => RouteMetadata.Values.Distinct().ToArray();

    public static string FormatPageTitle(string pageTitle, string brandName = "Pika")
    {
        if (string.IsNullOrWhiteSpace(pageTitle)) return brandName;
        var trimmed = pageTitle.Trim();
        return trimmed.Contains(brandName, StringComparison.OrdinalIgnoreCase) ? trimmed : $"{trimmed} | {brandName}";
    }
}
