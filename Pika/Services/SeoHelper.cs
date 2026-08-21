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
        // Homepage
        ["Home.Index"] = new(
            "/",
            "/en/",
            "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu",
            "Customer Intelligence & Omnichannel Marketing Platform",
            "Pika, müşteri ve ürün verisinden fırsatları tespit eden, yöneticiye doğru kararı sunan ve doğru anda aksiyona dönüştüren B2B SaaS platformudur. Günün Fırsatları, Journey, Campaign ve tüm kanallar tek platformda.",
            "Pika detects revenue opportunities from customer and product data, surfaces the right decisions, and drives timely omnichannel action. Daily Opportunities, Journey automation, Campaign and all channels — one platform."),

        // Home Subpages
        ["Home.Pika"] = new(
            "/pika",
            "/en/pika",
            "Pika Nedir?",
            "What is Pika?",
            "Pika, müşteri ve ürün verisini anlayarak günün fırsatlarını tespit eden, yöneticiye karar sunan ve doğru anda aksiyona dönüştüren müşteri zekâsı ve omnichannel pazarlama platformudur.",
            "Pika is a customer intelligence and omnichannel marketing platform that understands customer and product data, identifies daily opportunities, and converts decisions into timely action.",
            "Pika Nedir?",
            "What is Pika?"),

        ["Home.Corporate"] = new(
            "/kurumsal",
            "/en/corporate",
            "Kurumsal",
            "Corporate",
            "Pika kurumsal çözümleri, yüksek güvenlik standartları, ölçeklenebilir mimari ve KVKK/IYS uyumlu süreçlerle işletmenizin büyümesini destekler.",
            "Pika enterprise solutions support your business growth with high security standards, scalable architecture, and compliance.",
            "Kurumsal",
            "Corporate"),

        ["Home.DemoRequest"] = new(
            "/demo-talebi",
            "/en/demo-request",
            "Demo Talebi",
            "Request a Demo",
            "Pika omnichannel pazarlama platformunu canlı keşfedin. 15 dakikalık demo ile ihtiyacınıza özel akışları birlikte kuralım.",
            "Discover the Pika omnichannel marketing platform live. Schedule a 15-minute tailored demo with our team.",
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
            "Campaign Manager | Çok Kanallı Kampanya Yönetimi",
            "Campaign Manager | Multi-Channel Campaign Management",
            "SMS, WhatsApp, Email ve Push kampanyalarını tek merkezden oluşturun, zamanlayın, kişiselleştirin ve yönetin.",
            "Create, schedule, personalize and manage SMS, WhatsApp, Email and Push campaigns from a single hub.",
            "Campaign Manager",
            "Campaign Manager"),

        ["Solutions.AudienceManager"] = new(
            "/cozumler/audience-manager",
            "/en/solutions/audience-manager",
            "Audience Manager | Hedef Kitle ve Segmentasyon",
            "Audience Manager | Audience & Segmentation",
            "İşlem, davranış ve özel kurallara göre dinamik müşteri segmentleri oluşturun ve kampanyalarınızda hedefleyin.",
            "Create dynamic customer segments based on transactions, behaviors, and custom rules for targeted campaigns.",
            "Audience Manager",
            "Audience Manager"),

        ["Solutions.JourneyManager"] = new(
            "/cozumler/journey-manager",
            "/en/solutions/journey-manager",
            "Journey Manager | Müşteri Yolculuğu Otomasyonu",
            "Journey Manager | Customer Journey Automation",
            "Tetikleyici ve olay bazlı otomatik pazarlama akışları kurgulayın; müşterinize doğru anda doğru kanaldan ulaşın.",
            "Design event-driven automated marketing journeys to reach customers at the right moment across channels.",
            "Journey Manager",
            "Journey Manager"),

        ["Solutions.ContentStudio"] = new(
            "/cozumler/content-studio",
            "/en/solutions/content-studio",
            "Content Studio | İçerik ve Şablon Tasarımı",
            "Content Studio | Content & Template Design",
            "Sürükle-bırak görsel editör ve AI destekli metin üretimi ile çok kanallı kampanya içeriklerini dakikalar içinde tasarlayın.",
            "Design multi-channel campaign content in minutes with a drag-and-drop editor and AI-assisted copywriting.",
            "Content Studio",
            "Content Studio"),

        ["Solutions.ConsentManagement"] = new(
            "/cozumler/consent-management",
            "/en/solutions/consent-management",
            "Consent Management | İzin ve Uyumluluk Yönetimi",
            "Consent Management | Consent & Compliance Management",
            "IYS ve KVKK uyumlu ticari elektronik ileti izinlerini merkezi olarak yönetin, onaysız gönderimleri engelleyin.",
            "Centrally manage opt-in consents and ensure strict regulatory compliance across all communication channels.",
            "Consent Management",
            "Consent Management"),

        ["Solutions.EmailMarketing"] = new(
            "/kanallar/email",
            "/en/channels/email",
            "Email Marketing | E-Posta Pazarlama Çözümleri",
            "Email Marketing | Email Marketing Solutions",
            "Zengin görsel şablonlar, dinamik kişiselleştirme ve yüksek teslimat oranları ile e-posta kampanyalarınızı ölçekleyin.",
            "Scale your email marketing with rich templates, dynamic personalization, and high deliverability rates.",
            "Email Marketing",
            "Email Marketing"),

        ["Solutions.SmsCampaigns"] = new(
            "/kanallar/sms",
            "/en/channels/sms",
            "SMS Campaigns | SMS Kampanya Yönetimi",
            "SMS Campaigns | SMS Campaign Management",
            "Kritik duyurular ve anlık fırsatlar için yüksek teslimatlı, zamanlanmış ve kişiselleştirilmiş SMS gönderimleri yapın.",
            "Deliver high-impact, scheduled, and personalized SMS messages with reliable delivery performance.",
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
            "Web ve mobil uygulamalarda kullanıcı davranışlarına göre anlık tetiklenen zengin bildirimler gönderin.",
            "Engage web and mobile app users with real-time, behavior-triggered rich push notifications.",
            "Push Notifications",
            "Push Notifications"),

        ["Solutions.Personalization"] = new(
            "/cozumler/personalization",
            "/en/solutions/personalization",
            "Personalization | Kişiselleştirme Çözümleri",
            "Personalization | Personalization Solutions",
            "Müşteri öznitelikleri ve geçmiş alışveriş verilerine göre içerik, teklif ve ürün önerilerini dinamik olarak özelleştirin.",
            "Dynamically tailor content, offers, and recommendations based on customer attributes and transaction history.",
            "Personalization",
            "Personalization"),

        ["Solutions.TemplateManagement"] = new(
            "/cozumler/template-management",
            "/en/solutions/template-management",
            "Template Management | Şablon Yönetimi",
            "Template Management | Template Management",
            "Tüm iletişim kanalları için şablonları merkezi olarak sürümleyin, onaylayın ve marka standartlarını koruyun.",
            "Centrally version, approve, and maintain templates across all channels while preserving brand consistency.",
            "Template Management",
            "Template Management"),

        ["Solutions.ABTesting"] = new(
            "/cozumler/ab-testing",
            "/en/solutions/ab-testing",
            "A/B Testing | Kampanya A/B Testleri",
            "A/B Testing | Campaign A/B Testing",
            "Başlık, metin, kanal ve gönderim zamanı varyasyonlarını test ederek en yüksek dönüşüm getiren kurguyu belirleyin.",
            "Test headlines, copy, channels, and send times to scientifically identify the highest-converting variations.",
            "A/B Testing",
            "A/B Testing"),

        ["Solutions.Reporting"] = new(
            "/cozumler/analytics-reporting",
            "/en/solutions/analytics-reporting",
            "Analytics & Reporting | Performans ve Raporlama",
            "Analytics & Reporting | Analytics & Reporting",
            "Kampanya, kanal ve segment bazında anlık açılma, tıklama, dönüşüm ve gelir metriklerini canlı takip edin.",
            "Monitor live delivery, open, click, conversion, and revenue metrics across campaigns, channels, and segments.",
            "Analytics & Reporting",
            "Analytics & Reporting"),

        ["Solutions.DeliverabilityCompliance"] = new(
            "/cozumler/deliverability-compliance",
            "/en/solutions/deliverability-compliance",
            "Deliverability & Compliance | Teslim Edilebilirlik ve Uyumluluk",
            "Deliverability & Compliance | Deliverability & Compliance",
            "Gönderici itibarını koruyun, spam riskini minimize edin ve regülasyon uyumluluğunu uçtan uca güvenceye alın.",
            "Protect sender reputation, minimize spam placement, and ensure end-to-end regulatory compliance.",
            "Deliverability & Compliance",
            "Deliverability & Compliance"),

        ["Solutions.DataManagementEtl"] = new(
            "/cozumler/data-management-etl",
            "/en/solutions/data-management-etl",
            "Data Management ETL | Veri Yönetimi ve Entegrasyon",
            "Data Management ETL | Data Management ETL",
            "Farklı veri kaynaklarını birleştirin, temizleyin ve pazarlama kampanyaları için gerçek zamanlı kullanılabilir hale getirin.",
            "Ingest, transform, and synchronize customer data from multiple sources for real-time marketing activation.",
            "Data Management ETL",
            "Data Management ETL"),

        ["Solutions.RealTimeEventProcessing"] = new(
            "/cozumler/real-time-event-processing",
            "/en/solutions/real-time-event-processing",
            "Real-Time Event Processing | Gerçek Zamanlı Olay İşleme",
            "Real-Time Event Processing | Real-Time Event Processing",
            "Kullanıcı eylemlerini milisaniyeler içinde işleyerek anlık otomatik tetikleyicilerle etkileşim sağlayın.",
            "Process customer events in milliseconds to trigger instant, contextual omnichannel interactions.",
            "Real-Time Event Processing",
            "Real-Time Event Processing"),

        ["Solutions.Integrations"] = new(
            "/entegrasyonlar",
            "/en/integrations",
            "Integrations | Entegrasyonlar",
            "Integrations | Integrations",
            "CRM, e-ticaret altyapıları, ERP ve veri ambarları ile çift yönlü kesintisiz API entegrasyonu.",
            "Seamless two-way API integrations with CRM, e-commerce platforms, ERP, and data warehouses.",
            "Entegrasyonlar",
            "Integrations"),

        ["Solutions.SecurityPrivacy"] = new(
            "/guvenlik-ve-gizlilik",
            "/en/security-and-privacy",
            "Security & Privacy | Güvenlik ve Gizlilik",
            "Security & Privacy | Security & Privacy",
            "Kurumsal düzeyde veri şifreleme, rol bazlı erişim denetimi (RBAC), SSO ve KVKK uyumlu veri güvenliği altyapısı.",
            "Enterprise-grade data encryption, role-based access control (RBAC), SSO, and strict data security compliance.",
            "Güvenlik ve Gizlilik",
            "Security & Privacy"),

        // Special routes
        ["Solutions.AiCampaignAssistant"] = new(
            "/urunler/ai-kampanya-asistani",
            "/en/products/ai-campaign-assistant",
            "Pika AI Kampanya Asistanı | Yapay Zekâ Destekli Kampanya Üretimi",
            "Pika AI Campaign Assistant | AI-Powered Campaign Creation",
            "Kampanya fikrinizi yazın; Pika AI taslak, hedef kitle önerisi ve email şablonunu saniyeler içinde oluştursun.",
            "Enter your campaign idea; Pika AI generates the campaign draft, audience recommendations, and email template in seconds.",
            "AI Kampanya Asistanı",
            "AI Campaign Assistant"),

        ["Solutions.EcommerceAiCampaign"] = new(
            "/cozumler/e-ticaret-ai-kampanya-yonetimi",
            "/en/solutions/ecommerce-ai-campaign",
            "E-Ticaret AI Kampanya Yönetimi",
            "E-Commerce AI Campaign Management",
            "E-ticaret markaları için sepet terk, dinamik indirim ve kişiselleştirilmiş çapraz satış kampanyaları.",
            "AI campaign automation for e-commerce: abandoned cart recovery, dynamic discounts, and personalized cross-selling.",
            "E-Ticaret AI",
            "E-Commerce AI"),

        ["Solutions.WhatsAppCampaignManagement"] = new(
            "/kanallar/whatsapp-kampanya-yonetimi",
            "/en/channels/whatsapp-campaign-management",
            "WhatsApp Kampanya Yönetimi",
            "WhatsApp Campaign Management",
            "WhatsApp Business API ile kurumsal şablon onayları, otomatik akışlar ve zengin medya kampanyaları.",
            "Enterprise WhatsApp campaign management with verified templates, automated workflows, and rich media delivery.",
            "WhatsApp Kampanya",
            "WhatsApp Campaigns"),

        ["Solutions.IysKvkkCompliance"] = new(
            "/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi",
            "/en/solutions/iys-kvkk-compliance",
            "İYS ve KVKK Uyumlu Kampanya Yönetimi",
            "IYS & KVKK Compliant Campaign Management",
            "Ticari elektronik ileti mevzuatı ve KVKK gereksinimlerine tam uyumlu izin kontrolü ve denetim kayıtları.",
            "Full compliance with commercial electronic messaging regulations, consent validation, and audit logs.",
            "İYS & KVKK Uyumu",
            "IYS & KVKK Compliance"),

        ["Solutions.EmailMarketingTemplateStudio"] = new(
            "/kanallar/email-marketing-template-studio",
            "/en/channels/email-marketing-template-studio",
            "Email Marketing ve Template Studio",
            "Email Marketing & Template Studio",
            "Görsel sürükle-bırak şablon stüdyosu ile responsive, markanıza uygun e-posta tasarımları oluşturun.",
            "Create responsive, on-brand email marketing templates with an intuitive visual drag-and-drop studio.",
            "Email Template Studio",
            "Email Template Studio"),

        ["Solutions.UseCases"] = new(
            "/kullanim-senaryolari",
            "/en/use-cases",
            "Kullanım Senaryoları | Pika Omnichannel Çözümleri",
            "Use Cases | Pika Omnichannel Solutions",
            "Perakende, e-ticaret ve hizmet sektörlerinde omnichannel pazarlama otomasyonu kullanım senaryoları ve başarı hikayeleri.",
            "Omnichannel marketing automation use cases and customer journey blueprints for retail, e-commerce, and services.",
            "Kullanım Senaryoları",
            "Use Cases"),

        // Platform Controller — Semantic Architecture
        ["Platform.CustomerIntelligence"] = new(
            "/platform/customer-intelligence",
            "/en/platform/customer-intelligence",
            "Customer Intelligence | Müşteri Zekâsı",
            "Customer Intelligence | Customer Analytics",
            "Pika Customer Intelligence, müşteri davranışı ve transaction verisinden anlam çıkararak segmentasyon, değer analizi ve fırsat tespitine zemin hazırlar.",
            "Pika Customer Intelligence extracts meaning from customer behavior and transaction data to enable segmentation, value analysis, and opportunity detection.",
            "Customer Intelligence",
            "Customer Intelligence"),

        ["Platform.ProductIntelligence"] = new(
            "/platform/product-intelligence",
            "/en/platform/product-intelligence",
            "Product Intelligence | Ürün Zekâsı",
            "Product Intelligence | Product Analytics",
            "Pika Product Intelligence, ürünleri yalnızca katalog kaydı değil müşteri ihtiyacı, ticari rol ve analitik bağlamla anlamlandıran ürün zekâsı katmanıdır.",
            "Pika Product Intelligence enriches products beyond catalog records — with customer need, commercial role, and analytic context to power smarter decisions.",
            "Product Intelligence",
            "Product Intelligence"),

        ["Platform.Pika360"] = new(
            "/platform/pika-360",
            "/en/platform/pika-360",
            "Pika 360 | Bütünleşik Müşteri Karar Ekranı",
            "Pika 360 | Unified Customer Decision View",
            "Pika 360, tek bir müşteriyi yalnızca profil olarak değil; değer, davranış, risk, iletişim erişimi ve açık fırsat bağlamıyla birlikte değerlendiren karar ekranıdır.",
            "Pika 360 is a unified customer decision screen combining value, behavior, risk, channel access, and open opportunities in a single view.",
            "Pika 360",
            "Pika 360"),

        ["Platform.Opportunities"] = new(
            "/platform/gunun-firsatlari",
            "/en/platform/opportunities",
            "Günün Fırsatları | Fırsat ve Karar Motoru",
            "Daily Opportunities | Opportunity & Decision Engine",
            "Pika'nın fırsat motoru, müşteri ve ürün verisinden tekrar satın alma, cross-sell, upsell ve geri kazanım fırsatlarını tespit ederek günlük aksiyon öncelikleri oluşturur.",
            "Pika's opportunity engine detects repeat purchase, cross-sell, upsell, and win-back signals from customer and product data to build daily action priorities.",
            "Günün Fırsatları",
            "Daily Opportunities"),
    };

    public static PageSeoMetadata? GetMetadata(string? controller, string? action)
    {
        if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
            return null;

        var key = $"{controller}.{action}";
        if (RouteMetadata.TryGetValue(key, out var meta))
        {
            return meta;
        }

        // Action normalization mappings
        if (controller.Equals("Solutions", StringComparison.OrdinalIgnoreCase))
        {
            if (action.Equals("PersonalizationPage", StringComparison.OrdinalIgnoreCase))
                return RouteMetadata["Solutions.Personalization"];
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

    public static List<HreflangEntry> GetHreflangAlternates(string? controller, string? action)
    {
        var meta = GetMetadata(controller, action);
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
            return $"{brandName} | Omnichannel Pazarlama Otomasyonu";
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
