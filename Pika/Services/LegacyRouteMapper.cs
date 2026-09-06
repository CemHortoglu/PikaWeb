using System;
using System.Collections.Generic;

namespace Pika.Services;

public static class LegacyRouteMapper
{
    private static readonly Dictionary<string, string> Redirects = new(StringComparer.OrdinalIgnoreCase)
    {
        // ==========================================
        // 1. HOME & GLOBAL LEGACY ROUTES
        // ==========================================
        ["/tr"] = "/",
        ["/tr/"] = "/",
        ["/home"] = "/",
        ["/home/"] = "/",
        ["/home/index"] = "/",
        ["/Home"] = "/",
        ["/Home/Index"] = "/",
        ["/tr/home"] = "/",
        ["/tr/home/"] = "/",
        ["/tr/home/index"] = "/",
        ["/tr/Home"] = "/",
        ["/tr/Home/Index"] = "/",

        ["/en"] = "/en/",
        ["/en/home"] = "/en/",
        ["/en/home/"] = "/en/",
        ["/en/home/index"] = "/en/",
        ["/en/Home"] = "/en/",
        ["/en/Home/Index"] = "/en/",

        // Pika / Hakkımızda
        ["/home/pika"] = "/pika",
        ["/Home/Pika"] = "/pika",
        ["/tr/home/pika"] = "/pika",
        ["/tr/Home/Pika"] = "/pika",
        ["/tr/pika"] = "/pika",
        ["/en/home/pika"] = "/en/pika",
        ["/en/Home/Pika"] = "/en/pika",
        ["/en/about-pika"] = "/en/pika",

        // Corporate / Kurumsal
        ["/home/corporate"] = "/kurumsal",
        ["/Home/Corporate"] = "/kurumsal",
        ["/tr/home/corporate"] = "/kurumsal",
        ["/tr/Home/Corporate"] = "/kurumsal",
        ["/tr/corporate"] = "/kurumsal",
        ["/corporate"] = "/kurumsal",
        ["/tr/kurumsal"] = "/kurumsal",
        ["/en/home/corporate"] = "/en/corporate",
        ["/en/Home/Corporate"] = "/en/corporate",

        // Demo Request / Demo Talebi
        ["/home/demo-talebi"] = "/demo-talebi",
        ["/home/demo-request"] = "/demo-talebi",
        ["/Home/DemoRequest"] = "/demo-talebi",
        ["/tr/home/demo-talebi"] = "/demo-talebi",
        ["/tr/home/demo-request"] = "/demo-talebi",
        ["/tr/Home/DemoRequest"] = "/demo-talebi",
        ["/tr/demo-talebi"] = "/demo-talebi",
        ["/demo-request"] = "/demo-talebi",
        ["/en/home/demo-request"] = "/en/demo-request",
        ["/en/home/demo-talebi"] = "/en/demo-request",
        ["/en/Home/DemoRequest"] = "/en/demo-request",
        ["/en/demo-talebi"] = "/en/demo-request",

        // FAQ / SSS
        ["/home/sss"] = "/kaynaklar/sss",
        ["/home/faq"] = "/kaynaklar/sss",
        ["/Home/Faq"] = "/kaynaklar/sss",
        ["/tr/home/sss"] = "/kaynaklar/sss",
        ["/tr/home/faq"] = "/kaynaklar/sss",
        ["/tr/Home/Faq"] = "/kaynaklar/sss",
        ["/sss"] = "/kaynaklar/sss",
        ["/faq"] = "/kaynaklar/sss",
        ["/tr/sss"] = "/kaynaklar/sss",
        ["/tr/faq"] = "/kaynaklar/sss",
        ["/tr/kaynaklar/sss"] = "/kaynaklar/sss",
        ["/en/home/faq"] = "/en/resources/faq",
        ["/en/home/sss"] = "/en/resources/faq",
        ["/en/Home/Faq"] = "/en/resources/faq",
        ["/en/faq"] = "/en/resources/faq",
        ["/en/sss"] = "/en/resources/faq",

        // Contact / İletişim
        ["/home/contact"] = "/iletisim",
        ["/Home/Contact"] = "/iletisim",
        ["/tr/home/contact"] = "/iletisim",
        ["/tr/Home/Contact"] = "/iletisim",
        ["/contact"] = "/iletisim",
        ["/tr/contact"] = "/iletisim",
        ["/tr/iletisim"] = "/iletisim",
        ["/en/home/contact"] = "/en/contact",
        ["/en/Home/Contact"] = "/en/contact",

        // Career / Kariyer
        ["/home/kariyer"] = "/kariyer",
        ["/home/career"] = "/kariyer",
        ["/Home/Career"] = "/kariyer",
        ["/tr/home/kariyer"] = "/kariyer",
        ["/tr/home/career"] = "/kariyer",
        ["/tr/Home/Career"] = "/kariyer",
        ["/career"] = "/kariyer",
        ["/tr/career"] = "/kariyer",
        ["/tr/kariyer"] = "/kariyer",
        ["/en/home/career"] = "/en/careers",
        ["/en/home/kariyer"] = "/en/careers",
        ["/en/Home/Career"] = "/en/careers",
        ["/en/career"] = "/en/careers",

        // Terms of Use / Kullanım Şartları
        ["/home/kullanim-sartlari"] = "/kullanim-sartlari",
        ["/home/terms-of-use"] = "/kullanim-sartlari",
        ["/Home/TermsOfUse"] = "/kullanim-sartlari",
        ["/tr/home/kullanim-sartlari"] = "/kullanim-sartlari",
        ["/tr/home/terms-of-use"] = "/kullanim-sartlari",
        ["/tr/Home/TermsOfUse"] = "/kullanim-sartlari",
        ["/terms-of-use"] = "/kullanim-sartlari",
        ["/tr/kullanim-sartlari"] = "/kullanim-sartlari",
        ["/tr/terms-of-use"] = "/kullanim-sartlari",
        ["/en/home/terms-of-use"] = "/en/terms-of-use",
        ["/en/home/kullanim-sartlari"] = "/en/terms-of-use",
        ["/en/Home/TermsOfUse"] = "/en/terms-of-use",

        // Privacy Policy / Gizlilik Politikası
        ["/home/gizlilik-politikasi"] = "/gizlilik-politikasi",
        ["/home/privacy-policy"] = "/gizlilik-politikasi",
        ["/Home/PrivacyPolicy"] = "/gizlilik-politikasi",
        ["/tr/home/gizlilik-politikasi"] = "/gizlilik-politikasi",
        ["/tr/home/privacy-policy"] = "/gizlilik-politikasi",
        ["/tr/Home/PrivacyPolicy"] = "/gizlilik-politikasi",
        ["/privacy-policy"] = "/gizlilik-politikasi",
        ["/tr/gizlilik-politikasi"] = "/gizlilik-politikasi",
        ["/tr/privacy-policy"] = "/gizlilik-politikasi",
        ["/en/home/privacy-policy"] = "/en/privacy-policy",
        ["/en/home/gizlilik-politikasi"] = "/en/privacy-policy",
        ["/en/Home/PrivacyPolicy"] = "/en/privacy-policy",

        // ==========================================
        // 2. PLATFORM CONTROLLER ROUTES
        // ==========================================
        ["/Platform/CustomerIntelligence"] = "/platform/customer-intelligence",
        ["/platform/customerintelligence"] = "/platform/customer-intelligence",
        ["/tr/platform/customer-intelligence"] = "/platform/customer-intelligence",
        ["/tr/platform/customerintelligence"] = "/platform/customer-intelligence",
        ["/tr/Platform/CustomerIntelligence"] = "/platform/customer-intelligence",
        ["/en/Platform/CustomerIntelligence"] = "/en/platform/customer-intelligence",
        ["/en/platform/customerintelligence"] = "/en/platform/customer-intelligence",

        ["/Platform/ProductIntelligence"] = "/platform/product-intelligence",
        ["/platform/productintelligence"] = "/platform/product-intelligence",
        ["/tr/platform/product-intelligence"] = "/platform/product-intelligence",
        ["/tr/platform/productintelligence"] = "/platform/product-intelligence",
        ["/tr/Platform/ProductIntelligence"] = "/platform/product-intelligence",
        ["/en/Platform/ProductIntelligence"] = "/en/platform/product-intelligence",
        ["/en/platform/productintelligence"] = "/en/platform/product-intelligence",

        ["/Platform/Pika360"] = "/platform/pika-360",
        ["/platform/pika360"] = "/platform/pika-360",
        ["/tr/platform/pika-360"] = "/platform/pika-360",
        ["/tr/platform/pika360"] = "/platform/pika-360",
        ["/tr/Platform/Pika360"] = "/platform/pika-360",
        ["/en/Platform/Pika360"] = "/en/platform/pika-360",
        ["/en/platform/pika360"] = "/en/platform/pika-360",

        ["/Platform/Opportunities"] = "/platform/gunun-firsatlari",
        ["/platform/opportunities"] = "/platform/gunun-firsatlari",
        ["/tr/platform/opportunities"] = "/platform/gunun-firsatlari",
        ["/tr/platform/firsatlar"] = "/platform/gunun-firsatlari",
        ["/tr/Platform/Opportunities"] = "/platform/gunun-firsatlari",
        ["/platform/firsatlar"] = "/platform/gunun-firsatlari",
        ["/tr/platform/gunun-firsatlari"] = "/platform/gunun-firsatlari",
        ["/en/Platform/Opportunities"] = "/en/platform/opportunities",
        ["/en/platform/firsatlar"] = "/en/platform/opportunities",
        ["/en/platform/gunun-firsatlari"] = "/en/platform/opportunities",

        // ==========================================
        // 3. SOLUTIONS & CHANNELS ROUTES
        // ==========================================
        ["/Solutions/CampaignManager"] = "/cozumler/campaign-manager",
        ["/solutions/campaign-manager"] = "/cozumler/campaign-manager",
        ["/solutions/campaignmanager"] = "/cozumler/campaign-manager",
        ["/tr/solutions/campaign-manager"] = "/cozumler/campaign-manager",
        ["/tr/solutions/campaignmanager"] = "/cozumler/campaign-manager",
        ["/tr/Solutions/CampaignManager"] = "/cozumler/campaign-manager",
        ["/en/Solutions/CampaignManager"] = "/en/solutions/campaign-manager",
        ["/en/solutions/campaignmanager"] = "/en/solutions/campaign-manager",

        ["/Solutions/AudienceManager"] = "/cozumler/audience-manager",
        ["/solutions/audience-manager"] = "/cozumler/audience-manager",
        ["/solutions/audiencemanager"] = "/cozumler/audience-manager",
        ["/tr/solutions/audience-manager"] = "/cozumler/audience-manager",
        ["/tr/solutions/audiencemanager"] = "/cozumler/audience-manager",
        ["/tr/Solutions/AudienceManager"] = "/cozumler/audience-manager",
        ["/en/Solutions/AudienceManager"] = "/en/solutions/audience-manager",
        ["/en/solutions/audiencemanager"] = "/en/solutions/audience-manager",

        ["/Solutions/JourneyManager"] = "/cozumler/journey-manager",
        ["/solutions/journey-manager"] = "/cozumler/journey-manager",
        ["/solutions/journeymanager"] = "/cozumler/journey-manager",
        ["/tr/solutions/journey-manager"] = "/cozumler/journey-manager",
        ["/tr/solutions/journeymanager"] = "/cozumler/journey-manager",
        ["/tr/Solutions/JourneyManager"] = "/cozumler/journey-manager",
        ["/en/Solutions/JourneyManager"] = "/en/solutions/journey-manager",
        ["/en/solutions/journeymanager"] = "/en/solutions/journey-manager",

        ["/Solutions/ContentStudio"] = "/cozumler/content-studio",
        ["/solutions/content-studio"] = "/cozumler/content-studio",
        ["/solutions/contentstudio"] = "/cozumler/content-studio",
        ["/tr/solutions/content-studio"] = "/cozumler/content-studio",
        ["/tr/solutions/contentstudio"] = "/cozumler/content-studio",
        ["/tr/Solutions/ContentStudio"] = "/cozumler/content-studio",
        ["/en/Solutions/ContentStudio"] = "/en/solutions/content-studio",
        ["/en/solutions/contentstudio"] = "/en/solutions/content-studio",

        ["/Solutions/ConsentManagement"] = "/cozumler/consent-management",
        ["/solutions/consent-management"] = "/cozumler/consent-management",
        ["/solutions/consentmanagement"] = "/cozumler/consent-management",
        ["/tr/solutions/consent-management"] = "/cozumler/consent-management",
        ["/tr/solutions/consentmanagement"] = "/cozumler/consent-management",
        ["/tr/Solutions/ConsentManagement"] = "/cozumler/consent-management",
        ["/en/Solutions/ConsentManagement"] = "/en/solutions/consent-management",
        ["/en/solutions/consentmanagement"] = "/en/solutions/consent-management",

        ["/Solutions/EmailMarketing"] = "/kanallar/email",
        ["/solutions/email-marketing"] = "/kanallar/email",
        ["/solutions/emailmarketing"] = "/kanallar/email",
        ["/tr/solutions/email-marketing"] = "/kanallar/email",
        ["/tr/solutions/emailmarketing"] = "/kanallar/email",
        ["/tr/Solutions/EmailMarketing"] = "/kanallar/email",
        ["/kanallar/e-posta"] = "/kanallar/email",
        ["/tr/kanallar/email"] = "/kanallar/email",
        ["/en/Solutions/EmailMarketing"] = "/en/channels/email",
        ["/en/solutions/email-marketing"] = "/en/channels/email",
        ["/en/solutions/emailmarketing"] = "/en/channels/email",

        ["/Solutions/SmsCampaigns"] = "/kanallar/sms",
        ["/solutions/sms-campaigns"] = "/kanallar/sms",
        ["/solutions/smscampaigns"] = "/kanallar/sms",
        ["/tr/solutions/sms-campaigns"] = "/kanallar/sms",
        ["/tr/solutions/smscampaigns"] = "/kanallar/sms",
        ["/tr/Solutions/SmsCampaigns"] = "/kanallar/sms",
        ["/tr/kanallar/sms"] = "/kanallar/sms",
        ["/en/Solutions/SmsCampaigns"] = "/en/channels/sms",
        ["/en/solutions/sms-campaigns"] = "/en/channels/sms",
        ["/en/solutions/smscampaigns"] = "/en/channels/sms",

        ["/Solutions/WhatsAppMessaging"] = "/kanallar/whatsapp",
        ["/solutions/whatsapp-messaging"] = "/kanallar/whatsapp",
        ["/solutions/whatsappmessaging"] = "/kanallar/whatsapp",
        ["/tr/solutions/whatsapp-messaging"] = "/kanallar/whatsapp",
        ["/tr/solutions/whatsappmessaging"] = "/kanallar/whatsapp",
        ["/tr/Solutions/WhatsAppMessaging"] = "/kanallar/whatsapp",
        ["/tr/kanallar/whatsapp"] = "/kanallar/whatsapp",
        ["/en/Solutions/WhatsAppMessaging"] = "/en/channels/whatsapp",
        ["/en/solutions/whatsapp-messaging"] = "/en/channels/whatsapp",
        ["/en/solutions/whatsappmessaging"] = "/en/channels/whatsapp",

        ["/Solutions/PushNotifications"] = "/kanallar/push",
        ["/solutions/push-notifications"] = "/kanallar/push",
        ["/solutions/pushnotifications"] = "/kanallar/push",
        ["/tr/solutions/push-notifications"] = "/kanallar/push",
        ["/tr/solutions/pushnotifications"] = "/kanallar/push",
        ["/tr/Solutions/PushNotifications"] = "/kanallar/push",
        ["/kanallar/push-bildirim"] = "/kanallar/push",
        ["/kanallar/push-bildirimleri"] = "/kanallar/push",
        ["/kanallar/push-notification"] = "/kanallar/push",
        ["/tr/kanallar/push-notification"] = "/kanallar/push",
        ["/tr/kanallar/push"] = "/kanallar/push",
        ["/en/Solutions/PushNotifications"] = "/en/channels/push",
        ["/en/solutions/push-notifications"] = "/en/channels/push",
        ["/en/solutions/pushnotifications"] = "/en/channels/push",
        ["/en/channels/push-notification"] = "/en/channels/push",
        ["/en/channels/push-notifications"] = "/en/channels/push",

        // 3.1 Retired Duplicate / Mismatched Routes (Direct 301 Canonicalization)
        ["/cozumler/personalization"] = "/cozumler/journey-manager",
        ["/cozumler/kisisellestirme"] = "/cozumler/journey-manager",
        ["/tr/cozumler/kisisellestirme"] = "/cozumler/journey-manager",
        ["/tr/cozumler/personalization"] = "/cozumler/journey-manager",
        ["/en/solutions/personalization"] = "/en/solutions/journey-manager",
        ["/Solutions/Personalization"] = "/cozumler/journey-manager",
        ["/solutions/personalization"] = "/cozumler/journey-manager",
        ["/solutions/personalizationpage"] = "/cozumler/journey-manager",
        ["/tr/solutions/personalization"] = "/cozumler/journey-manager",
        ["/tr/solutions/personalizationpage"] = "/cozumler/journey-manager",
        ["/tr/Solutions/Personalization"] = "/cozumler/journey-manager",
        ["/en/Solutions/Personalization"] = "/en/solutions/journey-manager",
        ["/en/solutions/personalizationpage"] = "/en/solutions/journey-manager",

        ["/cozumler/template-management"] = "/cozumler/content-studio",
        ["/cozumler/sablon-yonetimi"] = "/cozumler/content-studio",
        ["/tr/cozumler/sablon-yonetimi"] = "/cozumler/content-studio",
        ["/tr/cozumler/template-management"] = "/cozumler/content-studio",
        ["/en/solutions/template-management"] = "/en/solutions/content-studio",
        ["/Solutions/TemplateManagement"] = "/cozumler/content-studio",
        ["/solutions/template-management"] = "/cozumler/content-studio",
        ["/solutions/templatemanagement"] = "/cozumler/content-studio",
        ["/tr/solutions/template-management"] = "/cozumler/content-studio",
        ["/tr/solutions/templatemanagement"] = "/cozumler/content-studio",
        ["/tr/Solutions/TemplateManagement"] = "/cozumler/content-studio",
        ["/en/Solutions/TemplateManagement"] = "/en/solutions/content-studio",
        ["/en/solutions/templatemanagement"] = "/en/solutions/content-studio",

        ["/cozumler/ab-testing"] = "/cozumler/campaign-manager",
        ["/cozumler/ab-testleri"] = "/cozumler/campaign-manager",
        ["/tr/cozumler/ab-testleri"] = "/cozumler/campaign-manager",
        ["/tr/cozumler/ab-testing"] = "/cozumler/campaign-manager",
        ["/en/solutions/ab-testing"] = "/en/solutions/campaign-manager",
        ["/Solutions/ABTesting"] = "/cozumler/campaign-manager",
        ["/solutions/ab-testing"] = "/cozumler/campaign-manager",
        ["/solutions/abtesting"] = "/cozumler/campaign-manager",
        ["/tr/solutions/ab-testing"] = "/cozumler/campaign-manager",
        ["/tr/solutions/abtesting"] = "/cozumler/campaign-manager",
        ["/tr/Solutions/ABTesting"] = "/cozumler/campaign-manager",
        ["/en/Solutions/ABTesting"] = "/en/solutions/campaign-manager",
        ["/en/solutions/abtesting"] = "/en/solutions/campaign-manager",

        ["/Solutions/Reporting"] = "/cozumler/analytics-reporting",
        ["/solutions/reporting"] = "/cozumler/analytics-reporting",
        ["/solutions/analytics-reporting"] = "/cozumler/analytics-reporting",
        ["/tr/solutions/reporting"] = "/cozumler/analytics-reporting",
        ["/tr/solutions/analytics-reporting"] = "/cozumler/analytics-reporting",
        ["/tr/Solutions/Reporting"] = "/cozumler/analytics-reporting",
        ["/reporting"] = "/cozumler/analytics-reporting",
        ["/tr/reporting"] = "/cozumler/analytics-reporting",
        ["/en/Solutions/Reporting"] = "/en/solutions/analytics-reporting",
        ["/en/solutions/reporting"] = "/en/solutions/analytics-reporting",

        ["/Solutions/DeliverabilityCompliance"] = "/cozumler/deliverability-compliance",
        ["/solutions/deliverability-compliance"] = "/cozumler/deliverability-compliance",
        ["/solutions/deliverabilitycompliance"] = "/cozumler/deliverability-compliance",
        ["/tr/solutions/deliverability-compliance"] = "/cozumler/deliverability-compliance",
        ["/tr/solutions/deliverabilitycompliance"] = "/cozumler/deliverability-compliance",
        ["/tr/Solutions/DeliverabilityCompliance"] = "/cozumler/deliverability-compliance",
        ["/en/Solutions/DeliverabilityCompliance"] = "/en/solutions/deliverability-compliance",
        ["/en/solutions/deliverabilitycompliance"] = "/en/solutions/deliverability-compliance",

        ["/Solutions/DataManagementEtl"] = "/cozumler/data-management-etl",
        ["/solutions/data-management-etl"] = "/cozumler/data-management-etl",
        ["/solutions/datamanagementetl"] = "/cozumler/data-management-etl",
        ["/tr/solutions/data-management-etl"] = "/cozumler/data-management-etl",
        ["/tr/solutions/datamanagementetl"] = "/cozumler/data-management-etl",
        ["/tr/Solutions/DataManagementEtl"] = "/cozumler/data-management-etl",
        ["/en/Solutions/DataManagementEtl"] = "/en/solutions/data-management-etl",
        ["/en/solutions/datamanagementetl"] = "/en/solutions/data-management-etl",

        ["/Solutions/RealTimeEventProcessing"] = "/cozumler/real-time-event-processing",
        ["/solutions/real-time-event-processing"] = "/cozumler/real-time-event-processing",
        ["/solutions/realtimeeventprocessing"] = "/cozumler/real-time-event-processing",
        ["/tr/solutions/real-time-event-processing"] = "/cozumler/real-time-event-processing",
        ["/tr/solutions/realtimeeventprocessing"] = "/cozumler/real-time-event-processing",
        ["/tr/Solutions/RealTimeEventProcessing"] = "/cozumler/real-time-event-processing",
        ["/en/Solutions/RealTimeEventProcessing"] = "/en/solutions/real-time-event-processing",
        ["/en/solutions/realtimeeventprocessing"] = "/en/solutions/real-time-event-processing",

        ["/Solutions/Integrations"] = "/entegrasyonlar",
        ["/solutions/integrations"] = "/entegrasyonlar",
        ["/tr/solutions/integrations"] = "/entegrasyonlar",
        ["/tr/Solutions/Integrations"] = "/entegrasyonlar",
        ["/integrations"] = "/entegrasyonlar",
        ["/tr/entegrasyonlar"] = "/entegrasyonlar",
        ["/en/Solutions/Integrations"] = "/en/integrations",
        ["/en/solutions/integrations"] = "/en/integrations",

        ["/Solutions/SecurityPrivacy"] = "/guvenlik-ve-gizlilik",
        ["/solutions/security-privacy"] = "/guvenlik-ve-gizlilik",
        ["/solutions/securityprivacy"] = "/guvenlik-ve-gizlilik",
        ["/tr/solutions/security-privacy"] = "/guvenlik-ve-gizlilik",
        ["/tr/solutions/securityprivacy"] = "/guvenlik-ve-gizlilik",
        ["/tr/Solutions/SecurityPrivacy"] = "/guvenlik-ve-gizlilik",
        ["/guvenlik-ve-uyum"] = "/guvenlik-ve-gizlilik",
        ["/tr/guvenlik-ve-gizlilik"] = "/guvenlik-ve-gizlilik",
        ["/en/Solutions/SecurityPrivacy"] = "/en/security-and-privacy",
        ["/en/solutions/security-privacy"] = "/en/security-and-privacy",
        ["/en/solutions/securityprivacy"] = "/en/security-and-privacy",

        ["/Solutions/AiCampaignAssistant"] = "/urunler/ai-kampanya-asistani",
        ["/tr/products/ai-campaign-assistant"] = "/urunler/ai-kampanya-asistani",
        ["/products/ai-campaign-assistant"] = "/urunler/ai-kampanya-asistani",
        ["/urunler/ai-campaign-assistant"] = "/urunler/ai-kampanya-asistani",
        ["/tr/urunler/ai-kampanya-asistani"] = "/urunler/ai-kampanya-asistani",
        ["/en/Solutions/AiCampaignAssistant"] = "/en/products/ai-campaign-assistant",
        ["/en/products/ai-kampanya-asistani"] = "/en/products/ai-campaign-assistant",

        ["/cozumler/e-ticaret-ai-kampanya"] = "/urunler/ai-kampanya-asistani",
        ["/tr/cozumler/e-ticaret-ai-kampanya"] = "/urunler/ai-kampanya-asistani",
        ["/cozumler/e-ticaret-ai-kampanya-yonetimi"] = "/urunler/ai-kampanya-asistani",
        ["/en/solutions/ecommerce-ai-campaign"] = "/en/products/ai-campaign-assistant",
        ["/Solutions/EcommerceAiCampaign"] = "/urunler/ai-kampanya-asistani",
        ["/tr/cozumler/e-ticaret-ai-kampanya-yonetimi"] = "/urunler/ai-kampanya-asistani",
        ["/en/Solutions/EcommerceAiCampaign"] = "/en/products/ai-campaign-assistant",
        ["/en/cozumler/e-ticaret-ai-kampanya-yonetimi"] = "/en/products/ai-campaign-assistant",

        ["/kanallar/whatsapp-kampanya-yonetimi"] = "/kanallar/whatsapp",
        ["/cozumler/whatsapp-kampanya-yonetimi"] = "/kanallar/whatsapp",
        ["/tr/cozumler/whatsapp-kampanya-yonetimi"] = "/kanallar/whatsapp",
        ["/en/channels/whatsapp-campaign-management"] = "/en/channels/whatsapp",
        ["/en/solutions/whatsapp-campaign-management"] = "/en/channels/whatsapp",
        ["/Solutions/WhatsAppCampaignManagement"] = "/kanallar/whatsapp",
        ["/tr/kanallar/whatsapp-kampanya-yonetimi"] = "/kanallar/whatsapp",
        ["/en/Solutions/WhatsAppCampaignManagement"] = "/en/channels/whatsapp",
        ["/en/kanallar/whatsapp-kampanya-yonetimi"] = "/en/channels/whatsapp",

        ["/cozumler/iys-kvkk-uyumluluk"] = "/cozumler/consent-management",
        ["/tr/cozumler/iys-kvkk-uyumluluk"] = "/cozumler/consent-management",
        ["/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi"] = "/cozumler/consent-management",
        ["/en/solutions/iys-kvkk-compliance"] = "/en/solutions/consent-management",
        ["/Solutions/IysKvkkCompliance"] = "/cozumler/consent-management",
        ["/tr/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi"] = "/cozumler/consent-management",
        ["/en/Solutions/IysKvkkCompliance"] = "/en/solutions/consent-management",
        ["/en/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi"] = "/en/solutions/consent-management",

        ["/kanallar/email-marketing-template-studio"] = "/cozumler/content-studio",
        ["/cozumler/email-marketing-sablon-studyosu"] = "/cozumler/content-studio",
        ["/tr/cozumler/email-marketing-sablon-studyosu"] = "/cozumler/content-studio",
        ["/en/channels/email-marketing-template-studio"] = "/en/solutions/content-studio",
        ["/en/solutions/email-marketing-template-studio"] = "/en/solutions/content-studio",
        ["/Solutions/EmailMarketingTemplateStudio"] = "/cozumler/content-studio",
        ["/tr/kanallar/email-marketing-template-studio"] = "/cozumler/content-studio",
        ["/en/Solutions/EmailMarketingTemplateStudio"] = "/en/solutions/content-studio",
        ["/en/kanallar/email-marketing-template-studio"] = "/en/solutions/content-studio",

        // 3.2 Pricing Governance (Quotation-based model -> Direct 301 to Demo Request)
        ["/pricing"] = "/demo-talebi",
        ["/fiyatlandirma"] = "/demo-talebi",
        ["/fiyatlar"] = "/demo-talebi",
        ["/ucretler"] = "/demo-talebi",
        ["/tr/ucretler"] = "/demo-talebi",
        ["/home/pricing"] = "/demo-talebi",
        ["/Home/Pricing"] = "/demo-talebi",
        ["/tr/pricing"] = "/demo-talebi",
        ["/tr/fiyatlandirma"] = "/demo-talebi",
        ["/tr/fiyatlar"] = "/demo-talebi",
        ["/tr/home/pricing"] = "/demo-talebi",
        ["/tr/Home/Pricing"] = "/demo-talebi",
        ["/en/pricing"] = "/en/demo-request",
        ["/en/prices"] = "/en/demo-request",
        ["/en/home/pricing"] = "/en/demo-request",
        ["/en/Home/Pricing"] = "/en/demo-request",

        ["/Solutions/UseCases"] = "/kullanim-senaryolari",
        ["/tr/kullanim-senaryolari"] = "/kullanim-senaryolari",
        ["/en/Solutions/UseCases"] = "/en/use-cases",
        ["/en/kullanim-senaryolari"] = "/en/use-cases",

        // Legacy Wiki paths
        ["/wiki/index.html"] = "/wiki/",
        ["/wiki/index"] = "/wiki/",
        ["/tr/wiki"] = "/wiki/",
        ["/tr/wiki/"] = "/wiki/",

        // Legacy Internal Wiki paths
        ["/internal/wiki/index.html"] = "/internal/wiki/",
        ["/internal/wiki/index"] = "/internal/wiki/",
        ["/tr/internal/wiki"] = "/internal/wiki/",
        ["/tr/internal/wiki/"] = "/internal/wiki/",

        // ==========================================
        // 4. BROKEN NAV/FOOTER LINK FIXES
        // ==========================================
        // Use Cases — nav was using wrong prefix /kaynaklar/
        ["/kaynaklar/kullanim-senaryolari"] = "/kullanim-senaryolari",
        ["/en/resources/use-cases"] = "/en/use-cases",

        // Security — nav/footer was using wrong path
        ["/kaynaklar/guvenlik-gizlilik"] = "/guvenlik-ve-gizlilik",
        ["/en/resources/security-privacy"] = "/en/security-and-privacy",

        // Consent Management — nav/footer was using wrong path
        ["/cozumler/izin-yonetimi"] = "/cozumler/consent-management",

        // Integrations — nav/footer was using wrong path
        ["/cozumler/entegrasyonlar"] = "/entegrasyonlar",

        // Career English — nav was using /en/career instead of /en/careers
        ["/en/career"] = "/en/careers",

        // Terms of Use — footer was using wrong spelling /kullanim-kosullari
        ["/kullanim-kosullari"] = "/kullanim-sartlari",
        ["/tr/kullanim-kosullari"] = "/kullanim-sartlari"
    };

    public static bool TryGetRedirect(string path, out string? targetUrl)
    {
        targetUrl = null;
        if (string.IsNullOrWhiteSpace(path)) return false;

        var clean = path.Trim();
        if (clean.Length > 1 && clean.EndsWith('/') && !clean.Equals("/en/", StringComparison.OrdinalIgnoreCase) && !clean.Equals("/wiki/", StringComparison.OrdinalIgnoreCase) && !clean.Equals("/internal/wiki/", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean.TrimEnd('/');
        }

        // Direct match
        if (Redirects.TryGetValue(clean, out var target))
        {
            if (string.Equals(clean, target, StringComparison.OrdinalIgnoreCase))
            {
                targetUrl = null;
                return false;
            }
            targetUrl = target;
            return true;
        }

        // Catch /tr/wiki/{slug} -> /wiki/{slug}
        if (clean.StartsWith("/tr/wiki/", StringComparison.OrdinalIgnoreCase))
        {
            targetUrl = clean.Substring(3); // removes "/tr"
            return true;
        }

        return false;
    }
}
