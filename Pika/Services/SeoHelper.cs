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
        string? BreadcrumbTitleEn = null,
        bool NoIndex = false);

    private static readonly Dictionary<string, PageSeoMetadata> RouteMetadata = new(StringComparer.OrdinalIgnoreCase)
    {
        // Homepage
        ["Home.Index"] = new(
            "/",
            "/en/",
            "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu",
            "Customer Intelligence & Omnichannel Marketing Platform",
            "Pika, müşteri ve ürün verisinden fırsatları tespit eden, yöneticiye doğru kararı sunan ve doğru anda aksiyona dönüştüren B2B SaaS platformudur. Günün Fırsatları, Journey, Campaign; E-posta, SMS ve WhatsApp tek platformda.",
            "Pika detects revenue opportunities from customer and product data, surfaces the right decisions, and drives timely omnichannel action. Daily Opportunities, Journey automation, Campaign; Email, SMS, and WhatsApp — one platform."),

        // Home Subpages
        ["Home.Pika"] = new(
            "/pika",
            "/en/pika",
            "Pika Nedir? | Müşteri Zekâsı ve Omnichannel Pazarlama",
            "What Is Pika? | Customer Intelligence & Omnichannel Marketing",
            "Pika, müşteri, ürün ve satış verisini birlikte anlamlandırarak ticari fırsatları görünür kılan ve kontrollü omnichannel aksiyona dönüştüren platformdur.",
            "Pika brings customer, product and sales data together to reveal commercial opportunities and turn them into controlled omnichannel action.",
            "Pika Nedir?",
            "What Is Pika?"),

        ["Home.Corporate"] = new(
            "/kurumsal",
            "/en/corporate",
            "Kurumsal",
            "Corporate",
            "Pika kurumsal çözümleri, rol bazlı erişim denetimi, entegrasyon altyapısı ve KVKK/İYS izin yönetimi süreçleriyle işletmenizin iletişim operasyonlarını destekler.",
            "Pika enterprise solutions support your commercial communication operations with role-based access control, integration infrastructure, and consent management workflows.",
            "Kurumsal",
            "Corporate"),

        ["Home.DemoRequest"] = new(
            "/demo-talebi",
            "/en/demo-request",
            "Demo Talebi",
            "Request a Demo",
            "Pika müşteri zekâsı ve omnichannel pazarlama platformunu detaylı keşfedin. İhtiyacınıza ve kullanım kapsamınıza göre özel teklif alın.",
            "Discover the Pika customer intelligence and omnichannel marketing platform in detail. Get a tailored quote based on your requirements and scope of use.",
            "Demo Talebi",
            "Demo Request"),

        ["Home.Contact"] = new(
            "/iletisim",
            "/en/contact",
            "İletişim",
            "Contact",
            "Pika ekibiyle iletişime geçin. Satış, destek ve iş birliği talepleriniz için bize ulaşın.",
            "Get in touch with the Pika team for sales, support, and partnership inquiries.",
            "İletişim",
            "Contact"),

        ["Home.Career"] = new(
            "/kariyer",
            "/en/careers",
            "Kariyer",
            "Careers",
            "Pika ailesine katılın. Yenilikçi omnichannel pazarlama teknolojileri geliştiren tutkulu ekibimizde açık pozisyonları inceleyin.",
            "Join the Pika team. Explore open positions in our passionate team building next-generation omnichannel marketing technology.",
            "Kariyer",
            "Careers"),

        ["Home.Faq"] = new(
            "/kaynaklar/sss",
            "/en/resources/faq",
            "Sıkça Sorulan Sorular",
            "Frequently Asked Questions",
            "Pika omnichannel kampanya yönetimi, entegrasyonlar, izin süreçleri ve yapay zekâ özellikleri hakkında merak edilen soruların yanıtları.",
            "Answers to frequently asked questions about Pika omnichannel campaign management, integrations, consent, and AI features.",
            "S.S.S.",
            "FAQ"),

        ["Home.TermsOfUse"] = new(
            "/kullanim-sartlari",
            "/en/terms-of-use",
            "Kullanım Şartları",
            "Terms of Use",
            "Pika web sitesi ve hizmetlerinin kullanımına ilişkin şartlar ve yasal koşullar.",
            "Terms and legal conditions governing the use of Pika website and services.",
            "Kullanım Şartları",
            "Terms of Use"),

        ["Home.PrivacyPolicy"] = new(
            "/gizlilik-politikasi",
            "/en/privacy-policy",
            "Gizlilik Politikası",
            "Privacy Policy",
            "Pika gizlilik politikası ve kişisel verilerin korunması (KVKK) hakkındaki aydınlatma metni.",
            "Pika privacy policy and personal data protection principles.",
            "Gizlilik Politikası",
            "Privacy Policy"),

        // Solutions Controller
        ["Solutions.CampaignManager"] = new(
            "/cozumler/campaign-manager",
            "/en/solutions/campaign-manager",
            "Campaign Manager | Çok Kanallı Kampanya ve Kontrollü Gönderim | Pika",
            "Campaign Manager | Multi-Channel Campaigns & Controlled Delivery | Pika",
            "Pika Campaign Manager; hedef kitle, içerik, kanal, zamanlama ve gönderim öncesi kontrolleri tek kampanya akışında birleştirerek Email, SMS ve WhatsApp kampanyalarını yönetmenizi sağlar.",
            "Pika Campaign Manager brings audience, content, channel, timing and pre-send controls into one campaign workflow for Email, SMS and WhatsApp campaigns.",
            "Campaign Manager",
            "Campaign Manager"),

        ["Solutions.AudienceManager"] = new(
            "/cozumler/audience-manager",
            "/en/solutions/audience-manager",
            "Audience Manager | Dinamik Hedef Kitle ve Segmentasyon | Pika",
            "Audience Manager | Dynamic Audiences & Segmentation | Pika",
            "Pika Audience Manager; müşteri özellikleri, işlem ve davranış bağlamı ile kural mantığını birleştirerek yeniden kullanılabilir dinamik hedef kitleler oluşturmanıza yardımcı olur.",
            "Pika Audience Manager combines customer attributes, transaction and behavioral context with rule logic to help create reusable dynamic audiences.",
            "Audience Manager",
            "Audience Manager"),

        ["Solutions.JourneyManager"] = new(
            "/cozumler/journey-manager",
            "/en/solutions/journey-manager",
            "Journey Manager | Müşteri Yolculuğu ve Çok Adımlı Otomasyon | Pika",
            "Journey Manager | Customer Journeys & Multi-Step Automation | Pika",
            "Pika Journey Manager; koşul, dallanma, bekleme ve Email, SMS veya WhatsApp aksiyonlarını birleştirerek kontrollü çok adımlı müşteri yolculukları oluşturmanızı sağlar.",
            "Pika Journey Manager combines conditions, branching, waits and Email, SMS or WhatsApp actions to help build controlled multi-step customer journeys.",
            "Journey Manager",
            "Journey Manager"),

        ["Solutions.ContentStudio"] = new(
            "/cozumler/content-studio",
            "/en/solutions/content-studio",
            "Content Studio | Email Şablonları ve Çok Kanallı İçerik | Pika",
            "Content Studio | Email Templates & Multi-Channel Content | Pika",
            "Pika Content Studio; sürükle-bırak email editörü, modüler bloklar, şablonlar, kişiselleştirme alanları ve önizleme ile kampanya içeriklerini hazırlamanıza yardımcı olur.",
            "Pika Content Studio helps prepare campaign content with a drag-and-drop email editor, modular blocks, templates, personalization fields and preview.",
            "Content Studio",
            "Content Studio"),

        ["Solutions.ConsentManagement"] = new(
            "/cozumler/consent-management",
            "/en/solutions/consent-management",
            "Consent Management | İYS, Opt-Out ve Tercih Yönetimi | Pika",
            "Consent Management | Consent, Opt-Out & Preference Management | Pika",
            "Pika Consent Management; gönderim öncesi IYS durum kontrolü, merkezi opt-out yönetimi ve Email/SMS tercih güncelleme akışıyla iletişim uygunluğunu kontrol etmeye yardımcı olur.",
            "Pika Consent Management helps govern communication eligibility through pre-dispatch IYS status checks, centralized opt-out handling and an Email/SMS preference update flow.",
            "Consent Management",
            "Consent Management"),

        ["Solutions.EmailMarketing"] = new(
            "/kanallar/email",
            "/en/channels/email",
            "Email Marketing | E-Posta Pazarlama Çözümleri",
            "Email Marketing | Email Marketing Solutions",
            "Zengin görsel şablonlar, dinamik kişiselleştirme ve teslimat takibi ile e-posta kampanyalarınızı yönetin.",
            "Manage your email marketing with rich templates, dynamic personalization, and delivery tracking.",
            "Email Marketing",
            "Email Marketing"),

        ["Solutions.SmsCampaigns"] = new(
            "/kanallar/sms",
            "/en/channels/sms",
            "SMS Campaigns | SMS Kampanya Yönetimi",
            "SMS Campaigns | SMS Campaign Management",
            "Kritik duyurular ve anlık fırsatlar için zamanlanmış ve kişiselleştirilmiş SMS gönderimleri yapın.",
            "Deliver targeted, scheduled, and personalized SMS messages across operator networks.",
            "SMS Campaigns",
            "SMS Campaigns"),

        ["Solutions.WhatsAppMessaging"] = new(
            "/kanallar/whatsapp",
            "/en/channels/whatsapp",
            "WhatsApp Messaging | WhatsApp Kampanya ve Mesajlaşma",
            "WhatsApp Messaging | WhatsApp Marketing & Messaging",
            "WhatsApp Business API ile onaylı şablonlar, zengin medya ve etkileşimli mesajlaşma kampanyaları yönetin.",
            "Manage verified WhatsApp Business campaigns with rich media, interactive buttons, and template approvals.",
            "WhatsApp Messaging",
            "WhatsApp Messaging"),

        ["Solutions.PushNotifications"] = new(
            "/kanallar/push",
            "/en/channels/push",
            "Push Notifications | Anlık Bildirim Yönetimi",
            "Push Notifications | Push Notification Management",
            "Web ve mobil uygulamalarda kullanıcı davranışlarına göre tetiklenen bildirim senaryoları.",
            "Behavior-triggered notification workflows for web and mobile touchpoints.",
            "Push Notifications",
            "Push Notifications",
            NoIndex: true),

        ["Solutions.Reporting"] = new(
            "/cozumler/analytics-reporting",
            "/en/solutions/analytics-reporting",
            "Analytics & Reporting | Kampanya, Kanal ve Ciro Atfı | Pika",
            "Analytics & Reporting | Campaign, Channel & Revenue Attribution | Pika",
            "Pika Analytics & Reporting; kampanya, kanal, kitle ve teslimat sonuçlarını görünür hale getirir, etkileşim verilerini ve ilişkilendirilebilen satış veya ciro sonuçlarını attribution bağlamında değerlendirmenize yardımcı olur.",
            "Pika Analytics & Reporting makes campaign, channel, audience and delivery results visible and helps evaluate engagement plus attributable sales or revenue outcomes.",
            "Analytics & Reporting",
            "Analytics & Reporting"),

        ["Solutions.DeliverabilityCompliance"] = new(
            "/cozumler/deliverability-compliance",
            "/en/solutions/deliverability-compliance",
            "Deliverability & Compliance | Teslim Edilebilirlik ve Uyumluluk",
            "Deliverability & Compliance | Deliverability & Compliance",
            "Gönderici itibarını koruyun, spam riskini minimize edin ve izin/opt-out süreçlerini denetim altında tutun.",
            "Protect sender reputation, minimize spam placement, and manage consent and opt-out workflows.",
            "Deliverability & Compliance",
            "Deliverability & Compliance"),

        ["Solutions.DataManagementEtl"] = new(
            "/cozumler/data-management-etl",
            "/en/solutions/data-management-etl",
            "Data Management ETL | Veri Yönetimi ve Entegrasyon",
            "Data Management ETL | Data Management ETL",
            "Farklı veri kaynaklarını birleştirin, temizleyin ve pazarlama kampanyaları için kullanılabilir hale getirin.",
            "Ingest, transform, and synchronize customer data from multiple sources for marketing activation.",
            "Data Management ETL",
            "Data Management ETL"),

        ["Solutions.RealTimeEventProcessing"] = new(
            "/cozumler/real-time-event-processing",
            "/en/solutions/real-time-event-processing",
            "Real-Time Event Processing | Olay Bazlı Tetikleme",
            "Real-Time Event Processing | Event-Driven Processing",
            "Kullanıcı eylemlerini olay gerçekleştiğinde asenkron olarak işleyerek kurgulanan tetikleyicilerle etkileşim sağlayın.",
            "Process customer events asynchronously upon trigger events to drive contextual omnichannel interactions.",
            "Real-Time Event Processing",
            "Real-Time Event Processing"),

        ["Solutions.Integrations"] = new(
            "/entegrasyonlar",
            "/en/integrations",
            "Integrations | Entegrasyonlar",
            "Integrations | Integrations",
            "CRM, e-ticaret altyapıları, ERP ve veri ambarları ile asenkron API ve dosya entegrasyonu.",
            "Asynchronous API and file integrations with CRM, e-commerce platforms, ERP, and data warehouses.",
            "Entegrasyonlar",
            "Integrations"),

        ["Solutions.SecurityPrivacy"] = new(
            "/guvenlik-ve-gizlilik",
            "/en/security-and-privacy",
            "Security & Privacy | Güvenlik ve Gizlilik",
            "Security & Privacy | Security & Privacy",
            "Rol bazlı erişim denetimi (RBAC), API belirteç güvenliği ve KVKK aydınlatma standartları ile korunan veri altyapısı.",
            "Role-based access control (RBAC), API token security, and regulatory privacy standards for customer data protection.",
            "Güvenlik ve Gizlilik",
            "Security & Privacy"),

        // Special routes
        ["Solutions.AiCampaignAssistant"] = new(
            "/urunler/ai-kampanya-asistani",
            "/en/products/ai-campaign-assistant",
            "Pika Pilot | AI Kampanya Asistanı ve İçerik Taslağı | Pika",
            "Pika Pilot | AI Campaign Assistant & Content Drafting | Pika",
            "Pika Pilot; Pika'nın hesapladığı müşteri, ürün ve fırsat bağlamını kullanarak hedef kitle kriterleri, kanal kurgusu ve kampanya içeriği taslaklarını hazırlamaya yardımcı olur.",
            "Pika Pilot uses customer, product and opportunity context calculated by Pika to help draft audience criteria, channel plans and campaign content.",
            "Pika Pilot",
            "Pika Pilot"),

        ["Solutions.UseCases"] = new(
            "/kullanim-senaryolari",
            "/en/use-cases",
            "Kullanım Senaryoları | Pika Omnichannel Çözümleri",
            "Use Cases | Pika Omnichannel Solutions",
            "Perakende ve e-ticaret sektörlerinde müşteri zekâsı ve omnichannel pazarlama kullanım senaryoları ve örnek kurgular.",
            "Customer intelligence and omnichannel marketing use cases and workflow blueprints for retail and e-commerce.",
            "Kullanım Senaryoları",
            "Use Cases"),

        // Platform Controller — Semantic Architecture
        ["Platform.CustomerIntelligence"] = new(
            "/platform/customer-intelligence",
            "/en/platform/customer-intelligence",
            "Customer Intelligence | Müşteri Zekâsı ve Davranış Analizi | Pika",
            "Customer Intelligence | Customer Behavior & Value Intelligence | Pika",
            "Pika Customer Intelligence; işlem geçmişi, satın alma ritmi, müşteri değeri ve davranış değişimlerini birlikte değerlendirerek risk ve fırsat bağlamını görünür kılar.",
            "Pika Customer Intelligence evaluates transaction history, purchase rhythm, customer value and behavioral change together to make risk and opportunity context visible.",
            "Müşteri Zekâsı",
            "Customer Intelligence"),

        ["Platform.ProductIntelligence"] = new(
            "/platform/product-intelligence",
            "/en/platform/product-intelligence",
            "Product Intelligence | Ürün Zekâsı, Need Group ve Product Role | Pika",
            "Product Intelligence | Product Meaning, Need Groups & Roles | Pika",
            "Pika Product Intelligence; ürünleri stok kodunun ötesinde müşteri ihtiyacı, ürün rolü, tekrar satın alma davranışı ve ürün ilişkileri bağlamında anlamlandırır.",
            "Pika Product Intelligence gives products meaning beyond SKUs through customer need, product role, repeat-purchase behavior and product relationships.",
            "Ürün Zekâsı",
            "Product Intelligence"),

        ["Platform.Pika360"] = new(
            "/platform/pika-360",
            "/en/platform/pika-360",
            "Pika 360 | Tek Müşteri Karar Bağlamı ve Müşteri Zekâsı | Pika",
            "Pika 360 | Unified Customer Decision Context | Pika",
            "Pika 360; işlem geçmişi, müşteri değeri, davranış bağlamı, iletişim erişilebilirliği ve açık fırsatları tek müşteri karar görünümünde bir araya getirir.",
            "Pika 360 brings transaction history, customer value, behavioral context, communication reachability and open opportunities into a single customer decision view.",
            "Pika 360",
            "Pika 360"),

        ["Platform.Opportunities"] = new(
            "/platform/gunun-firsatlari",
            "/en/platform/opportunities",
            "Günün Fırsatları | Tekrar Satın Alma, Cross-sell ve Win-back | Pika",
            "Daily Opportunities | Repeat Purchase, Cross-sell & Win-back | Pika",
            "Pika Günün Fırsatları; müşteri ve ürün bağlamından tekrar satın alma, cross-sell ve win-back fırsatlarını günlük karar adayları olarak görünür hale getirir.",
            "Pika Daily Opportunities turns customer and product context into daily repeat-purchase, cross-sell and win-back opportunity candidates for evaluation.",
            "Günün Fırsatları",
            "Daily Opportunities"),
    };

    public static PageSeoMetadata? GetMetadata(string? controller, string? action)
    {
        if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
            return null;

        if (string.Equals(controller, "Home", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(action, "EnglishIndex", StringComparison.OrdinalIgnoreCase))
        {
            action = "Index";
        }

        var key = $"{controller}.{action}";
        if (RouteMetadata.TryGetValue(key, out var meta))
        {
            return meta;
        }

        return null;
    }

    public static string GetCanonicalUrl(string? controller, string? action, string culture, string? explicitCanonical = null)
    {
        if (!string.IsNullOrWhiteSpace(explicitCanonical))
        {
            return explicitCanonical.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? explicitCanonical
                : $"{BaseDomain}{(explicitCanonical.StartsWith('/') ? explicitCanonical : "/" + explicitCanonical)}";
        }

        var isTr = culture.Equals("tr", StringComparison.OrdinalIgnoreCase);
        var meta = GetMetadata(controller, action);
        if (meta != null)
        {
            var path = isTr ? meta.AlternatePathTr : meta.AlternatePathEn;
            return $"{BaseDomain}{path}";
        }

        // Default fallback
        if (string.Equals(controller, "Home", StringComparison.OrdinalIgnoreCase) &&
            (string.Equals(action, "Index", StringComparison.OrdinalIgnoreCase) || string.Equals(action, "EnglishIndex", StringComparison.OrdinalIgnoreCase)))
        {
            return isTr ? $"{BaseDomain}/" : $"{BaseDomain}/en/";
        }

        return isTr ? $"{BaseDomain}/" : $"{BaseDomain}/en/";
    }

    public static IReadOnlyDictionary<string, PageSeoMetadata> AllRouteMetadata => RouteMetadata;
    public static IEnumerable<PageSeoMetadata> AllPages => RouteMetadata.Values;

    public static List<HreflangEntry> GetHreflangAlternates(string? controller, string? action)
    {
        var meta = GetMetadata(controller, action);
        return GetHreflangAlternates(meta);
    }

    public static List<HreflangEntry> GetHreflangAlternates(PageSeoMetadata? meta)
    {
        if (meta == null)
        {
            return
            [
                new("tr", $"{BaseDomain}/"),
                new("en", $"{BaseDomain}/en/"),
                new("x-default", $"{BaseDomain}/")
            ];
        }

        return
        [
            new("tr", $"{BaseDomain}{meta.AlternatePathTr}"),
            new("en", $"{BaseDomain}{meta.AlternatePathEn}"),
            new("x-default", $"{BaseDomain}{meta.AlternatePathTr}")
        ];
    }

    public static string FormatPageTitle(string pageTitle, string brandName = "Pika")
    {
        if (string.IsNullOrWhiteSpace(pageTitle) || pageTitle.Trim().Equals(brandName, StringComparison.OrdinalIgnoreCase))
        {
            return $"{brandName} | Müşteri Zekâsı ve Omnichannel Pazarlama Platformu";
        }

        var trimmed = pageTitle.Trim();
        if (trimmed.EndsWith($"| {brandName}", StringComparison.OrdinalIgnoreCase) ||
            trimmed.EndsWith($"- {brandName}", StringComparison.OrdinalIgnoreCase) ||
            trimmed.Contains(brandName, StringComparison.OrdinalIgnoreCase))
        {
            return trimmed;
        }

        return $"{trimmed} | {brandName}";
    }
}
