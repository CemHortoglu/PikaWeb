# Public Route & Content Consistency Audit

This audit evaluates every public marketing route in PikaWeb for content consistency, canonical alignment, duplicate cannibalization risks, and future remediation paths.

---

## Executive Route Summary Statistics

- **Total Active Controller Actions:** 40
- **Total Public URL Paths Mapped:** 76 (across TR and EN)
- **High-Risk Route/Content Mismatches:** 3 (`/cozumler/personalization`, `/cozumler/template-management`, `/cozumler/ab-testing`)
- **Duplicate / Self-Cannibalizing Route Pairs:** 4 (WhatsApp, Email Template, Compliance, AI E-commerce)
- **Thin Prototype Stubs Discrepancy:** 6 (Removed from TR `sitemap.xml`, but active in routes and EN sitemap)
- **Orphan Views (No Controller Action):** 3 (`Views/Home/Pricing.cshtml`, `Views/Home/Solutions.cshtml`, `Views/Solutions/JourneyManager.cshtml.bak`)

---

## Comprehensive Public Route Inventory

| TR Route | EN Route | Controller.Action | View File | Current H1 | Current Title | Actual Page Subject | Canonical Product / Entity | Route / Content Consistency | Status | Recommendation | SEO / Cannibalization Risk |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| `/` | `/en/` | `Home.Index` / `Home.EnglishIndex` | `Views/Home/Index.cshtml` | "Müşteriyi Doğru Anda Anla, Doğru Aksiyonu Al" | "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu" (SeoHelper) / "Omnichannel Pazarlama Otomasyonu ve Müşteri Yolculuğu Platformu" (View) | Platform overview, intelligence, opportunities, channels, AI panel | Pika Core Platform | INCONSISTENT (H1 and View Title use older phrasing; SeoHelper uses modern canon) | CURRENT | REWRITE_LATER | Low technical risk; high narrative consistency risk. Harmonize H1 and View Title with modern positioning. |
| `/pika` | `/en/pika` | `Home.Pika` | `Views/Home/Pika.cshtml` | "Pika Nedir?" | "Pika Nedir? - Pika" | Detailed product definition, value chain, architecture, philosophy | Pika Core Platform | CONSISTENT | CURRENT | KEEP | Minimal risk. High informational depth. |
| `/kurumsal` | `/en/corporate` | `Home.Corporate` | `Views/Home/Corporate.cshtml` | "Kurumsal Çözümler" | "Kurumsal - Pika" | Enterprise security, architecture, compliance overview | Corporate / Trust | CONSISTENT | CURRENT | REWRITE_LATER | Low risk. Content is thin; needs deeper enterprise profile in later phases. |
| `/demo-talebi` | `/en/demo-request` | `Home.DemoRequest` | `Views/Home/DemoRequest.cshtml` | "Pika'yı Canlı Keşfedin" | "Demo Talebi - Pika" | Lead capture demo booking form | Conversion Funnel | CONSISTENT | CURRENT | KEEP | Core lead generation form with Google reCAPTCHA. |
| `/kaynaklar/sss` | `/en/resources/faq` | `Home.Faq` | `Views/Home/Faq.cshtml` | "Sıkça Sorulan Sorular" | "Sıkça Sorulan Sorular - Pika" | Marketing, security, compliance, and AI FAQ | Resources / FAQ | CONSISTENT | CURRENT | KEEP | High value for customer trust. Explicitly refutes autonomous AI dispatch. |
| `/iletisim` | `/en/contact` | `Home.Contact` | `Views/Home/Contact.cshtml` | "Bizimle İletişime Geçin" | "İletişim - Pika" | Contact information, inquiry form, Google Maps embed | Company / Contact | PARTIALLY_INCONSISTENT (Displays Ankara address, but Schema JSON-LD claims İstanbul) | CURRENT | REWRITE_LATER | Address consistency conflict between page text and global JSON-LD schema. |
| `/kariyer` | `/en/careers` | `Home.Career` | `Views/Home/Career.cshtml` | "Pika'da Geleceği Birlikte İnşa Edelim" | "Kariyer - Pika" | Job openings, engineering culture, application form | Company / Careers | PARTIALLY_INCONSISTENT (Unverified claim: "Uluslararası müşteriler") | CURRENT | REWRITE_LATER | Remove unsubstantiated international client claim in later rewrite. |
| `/kullanim-sartlari` | `/en/terms-of-use` | `Home.TermsOfUse` | `Views/Home/TermsOfUse.cshtml` | "Kullanım Şartları" | "Kullanım Şartları - Pika" | Legal terms and conditions | Legal | CONSISTENT | CURRENT | KEEP | Required legal compliance document. |
| `/gizlilik-politikasi` | `/en/privacy-policy` | `Home.PrivacyPolicy` | `Views/Home/PrivacyPolicy.cshtml` | "Gizlilik Politikası ve KVKK Aydınlatma Metni" | "Gizlilik Politikası - Pika" | KVKK data controller disclosure and privacy policy | Legal | CONSISTENT | CURRENT | KEEP | Required regulatory privacy policy. |
| `/platform/customer-intelligence` | `/en/platform/customer-intelligence` | `Platform.CustomerIntelligence` | `Views/Platform/CustomerIntelligence.cshtml` | "Customer Intelligence" | "Customer Intelligence \| Müşteri Zekâsı" | Customer behavior analysis, CVS scoring, purchasing rhythm | Customer Intelligence | CONSISTENT | CURRENT | KEEP | Core platform pillar. Uses hand-built mockups; should integrate real screenshots later. |
| `/platform/product-intelligence` | `/en/platform/product-intelligence` | `Platform.ProductIntelligence` | `Views/Platform/ProductIntelligence.cshtml` | "Product Intelligence" | "Product Intelligence \| Ürün Zekâsı" | Need Groups, Product Roles, Playbooks, catalog enrichment | Product Intelligence | CONSISTENT | CURRENT | KEEP | Core platform pillar. Rich product depth. |
| `/platform/pika-360` | `/en/platform/pika-360` | `Platform.Pika360` | `Views/Platform/Pika360.cshtml` | "Pika 360" | "Pika 360 \| Bütünleşik Müşteri Karar Ekranı" | Single unified customer decision console, timeline, risk, opportunities | Pika 360 | CONSISTENT | CURRENT | KEEP | Core platform pillar. Ready for real screenshot `img_pika-360_7.png`. |
| `/platform/gunun-firsatlari` | `/en/platform/opportunities` | `Platform.Opportunities` | `Views/Platform/Opportunities.cshtml` | "Günün Fırsatları" | "Günün Fırsatları \| Fırsat ve Karar Motoru" | CCE opportunity discovery, replenishment, win-back, cross-sell | Günün Fırsatları | CONSISTENT | CURRENT | KEEP | Core platform pillar. High commercial differentiation. |
| `/cozumler/campaign-manager` | `/en/solutions/campaign-manager` | `Solutions.CampaignManager` | `Views/Solutions/CampaignManager.cshtml` | "Campaign Manager" | "Campaign Manager \| Çok Kanallı Kampanya Yönetimi" | Multi-channel campaign authoring, scheduling, attribution | Campaign Manager | CONTRADICTORY (Claims native Push; displays unverified 14.2x ROAS; has broken 404 image) | CURRENT | REWRITE_LATER | High: Broken image `/wiki/assets/images/img_kampanya-yonetimi_0.png` (line 232); unverified ROAS/percentage claims; push roadmap qualifier missing. |
| `/cozumler/audience-manager` | `/en/solutions/audience-manager` | `Solutions.AudienceManager` | `Views/Solutions/AudienceManager.cshtml` | "Audience Manager" | "Audience Manager \| Hedef Kitle ve Segmentasyon" | Rule-based cohort builder, dynamic segments, RFM tagging | Audience Manager | CONSISTENT | CURRENT | KEEP | Core engagement pillar. Fully aligned with product capability. |
| `/cozumler/journey-manager` | `/en/solutions/journey-manager` | `Solutions.JourneyManager` | `Views/Solutions/JourneyManager.cshtml` | "Journey Manager" | "Journey Manager \| Müşteri Yolculuğu Otomasyonu" | Visual event-driven customer journey automation flows | Journey Manager | CONSISTENT | CURRENT | KEEP | Core engagement pillar. |
| `/cozumler/content-studio` | `/en/solutions/content-studio` | `Solutions.ContentStudio` | `Views/Solutions/ContentStudio.cshtml` | "Content Studio" | "Content Studio \| İçerik ve Şablon Tasarımı" | Visual drag-and-drop template editor, AI copy assistance | Content Studio | CONSISTENT | CURRENT | KEEP | Core engagement pillar. |
| `/cozumler/consent-management` | `/en/solutions/consent-management` | `Solutions.ConsentManagement` | `Views/Solutions/ConsentManagement.cshtml` | "Consent Management" | "Consent Management \| İzin ve Uyumluluk Yönetimi" | IYS consent validation, KVKK opt-out, preference management | Consent Management | CONSISTENT | CURRENT | KEEP | Core compliance module. |
| `/kanallar/email` | `/en/channels/email` | `Solutions.EmailMarketing` | `Views/Solutions/EmailMarketing.cshtml` | "Email Marketing" | "Email Marketing \| E-Posta Pazarlama Çözümleri" | Native email marketing, deliverability, responsive templates | Email Channel | CONSISTENT | CURRENT | KEEP | Canonical channel landing page. |
| `/kanallar/sms` | `/en/channels/sms` | `Solutions.SmsCampaigns` | `Views/Solutions/SmsCampaigns.cshtml` | "SMS Campaigns" | "SMS Campaigns \| SMS Kampanya Yönetimi" | Native SMS messaging, high deliverability, operator gateways | SMS Channel | CONSISTENT | CURRENT | KEEP | Canonical channel landing page. |
| `/kanallar/whatsapp` | `/en/channels/whatsapp` | `Solutions.WhatsAppMessaging` | `Views/Solutions/WhatsAppMessaging.cshtml` | "WhatsApp Messaging" | "WhatsApp Messaging \| WhatsApp Kampanya ve Mesajlaşma" | WhatsApp Business API, approved templates, notifications | WhatsApp Channel | CONSISTENT | CURRENT | KEEP | Canonical channel landing page. |
| `/kanallar/push` | `/en/channels/push` | `Solutions.PushNotifications` | `Views/Solutions/PushNotifications.cshtml` | "Push Notifications" | "Push Notifications \| Anlık Bildirim Yönetimi" | Web and mobile push notifications | Push Channel | CONTRADICTORY (Marketed as live product; actually ROADMAP) | ROADMAP | REWRITE_LATER | CRITICAL: Must add prominent "(Roadmap / Geliştirme Aşamasında)" banner. |
| `/cozumler/personalization` | `/en/solutions/personalization` | `Solutions.PersonalizationPage` | `Views/Solutions/Personalization.cshtml` | "Müşteri Etkileşim Yönetimi" | "Müşteri Etkileşim Yönetimi" (View) / "Personalization" (SeoHelper) | Multi-step journey flows, cart abandonment, event triggers | Journey Manager (MISMATCH) | MISMATCH: Route says `personalization`; page content is 100% `Journey Orchestration` | LEGACY_STUB | REDIRECT_LATER | HIGH: Keyword cannibalization with `/cozumler/journey-manager`. 301 redirect to `/cozumler/journey-manager`. |
| `/cozumler/template-management` | `/en/solutions/template-management` | `Solutions.TemplateManagement` | `Views/Solutions/TemplateManagement.cshtml` | "Müşteri Segmentasyonu ve Hedefleme" | "Müşteri Segmentasyonu ve Hedefleme" (View) / "Template Management" (SeoHelper) | Dynamic rule-based audience segmentation | Audience Manager (MISMATCH) | MISMATCH: Route says `template-management`; page content is 100% `Audience Segmentation` | LEGACY_STUB | REDIRECT_LATER | HIGH: Keyword cannibalization with `/cozumler/audience-manager`. 301 redirect to `/cozumler/audience-manager`. |
| `/cozumler/ab-testing` | `/en/solutions/ab-testing` | `Solutions.ABTesting` | `Views/Solutions/ABTesting.cshtml` | "Kampanya Otomasyonu ve Zamanlama" | "Kampanya Otomasyonu ve Zamanlama" (View) / "A/B Testing" (SeoHelper) | Campaign planning, variant allocation, scheduling | Campaign Manager (MISMATCH) | MISMATCH: Route says `ab-testing`; page content is 100% `Campaign Automation & Scheduling` | LEGACY_STUB | REDIRECT_LATER | HIGH: Keyword cannibalization with `/cozumler/campaign-manager`. 301 redirect to `/cozumler/campaign-manager`. |
| `/cozumler/analytics-reporting` | `/en/solutions/analytics-reporting` | `Solutions.Reporting` | `Views/Solutions/Reporting.cshtml` | "Analytics & Reporting" | "Analytics & Reporting \| Performans ve Raporlama" | Campaign analytics, store metrics, channel impact | Analytics & Reporting | CONSISTENT | CURRENT | KEEP | Canonical reporting solution page. |
| `/cozumler/deliverability-compliance` | `/en/solutions/deliverability-compliance` | `Solutions.DeliverabilityCompliance` | `Views/Solutions/DeliverabilityCompliance.cshtml` | "Teslim Edilebilirlik ve Uyumluluk" | "Teslim Edilebilirlik ve Uyumluluk" | Sender reputation, bounce handling, SPF/DKIM/DMARC | Consent Management / Deliverability | INCONSISTENT (Contains developer internal commentary in line 9) | LEGACY_STUB | REDIRECT_LATER | HIGH: Cannibalizes `/cozumler/consent-management`. 301 redirect to `/cozumler/consent-management`. |
| `/cozumler/data-management-etl` | `/en/solutions/data-management-etl` | `Solutions.DataManagementEtl` | `Views/Solutions/DataManagementEtl.cshtml` | "Veri Yönetimi ve ETL" | "Veri Yönetimi ve ETL" | Data ingestion, normalization, profile merge | Integrations & Ingestion | WEAK_STUB | LEGACY_STUB | REDIRECT_LATER | Cannibalizes `/entegrasyonlar`. 301 redirect to `/entegrasyonlar`. |
| `/cozumler/real-time-event-processing` | `/en/solutions/real-time-event-processing` | `Solutions.RealTimeEventProcessing` | `Views/Solutions/RealTimeEventProcessing.cshtml` | "Gerçek Zamanlı Olay İşleme" | "Gerçek Zamanlı Olay İşleme" | Webhooks, event triggers, latency | Journey Manager / Ingestion | WEAK_STUB | LEGACY_STUB | REDIRECT_LATER | Thin stub making exaggerated "millisecond" claims. Merge into Journey Manager and 301 redirect. |
| `/entegrasyonlar` | `/en/integrations` | `Solutions.Integrations` | `Views/Solutions/Integrations.cshtml` | "Entegrasyonlar" | "Integrations \| Entegrasyonlar" | Ingestion API, CRM, POS, e-commerce connectors | Integrations | CONSISTENT | CURRENT | KEEP | Core platform integration overview. |
| `/guvenlik-ve-gizlilik` | `/en/security-and-privacy` | `Solutions.SecurityPrivacy` | `Views/Solutions/SecurityPrivacy.cshtml` | "Güvenlik ve Gizlilik" | "Security & Privacy \| Güvenlik ve Gizlilik" | Data encryption, multi-tenant isolation, RBAC, KVKK | Trust / Security | CONSISTENT | CURRENT | KEEP | Enterprise security reassurance. |
| `/urunler/ai-kampanya-asistani` | `/en/products/ai-campaign-assistant` | `Solutions.AiCampaignAssistant` | `Views/Solutions/AiCampaignAssistant.cshtml` | "Pika Pilot AI Kampanya Asistanı" | "Pika AI Kampanya Asistanı \| Yapay Zekâ Destekli Kampanya Üretimi" | Natural language prompt to campaign draft, segment and email template | AI Campaign Assistant | CONSISTENT | CURRENT | KEEP | Canonical AI product landing page. Contains real screenshot `img_pika-pilot-ai-kampanya-asistani_16.png`. |
| `/cozumler/e-ticaret-ai-kampanya-yonetimi` | `/en/solutions/ecommerce-ai-campaign` | `Solutions.EcommerceAiCampaign` | `Views/Solutions/EcommerceAiCampaign.cshtml` | "E-Ticaret İçin AI Destekli Kampanya Yönetimi" | "E-Ticaret AI Kampanya Yönetimi" | E-commerce specific cart reminder and AI campaign scenarios | AI Campaign Assistant / Use Cases | DUPLICATE | DUPLICATE | REDIRECT_LATER | HIGH: Directly cannibalizes `/urunler/ai-kampanya-asistani`. 301 redirect to `/urunler/ai-kampanya-asistani` or merge into Use Cases. |
| `/kanallar/whatsapp-kampanya-yonetimi` | `/en/channels/whatsapp-campaign-management` | `Solutions.WhatsAppCampaignManagement` | `Views/Solutions/WhatsAppCampaignManagement.cshtml` | "WhatsApp Kampanya Yönetimi" | "WhatsApp Kampanya Yönetimi" | WhatsApp campaign scheduling, templates, reporting | WhatsApp Channel | DUPLICATE | DUPLICATE | REDIRECT_LATER | HIGH: Directly cannibalizes `/kanallar/whatsapp`. 301 redirect to `/kanallar/whatsapp`. |
| `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` | `/en/solutions/iys-kvkk-compliance` | `Solutions.IysKvkkCompliance` | `Views/Solutions/IysKvkkCompliance.cshtml` | "İYS ve KVKK Uyumlu Kampanya Yönetimi" | "İYS ve KVKK Uyumlu Kampanya Yönetimi" | Consent verification, regulatory audit logs | Consent Management | DUPLICATE | DUPLICATE | REDIRECT_LATER | HIGH: Directly cannibalizes `/cozumler/consent-management`. 301 redirect to `/cozumler/consent-management`. |
| `/kanallar/email-marketing-template-studio` | `/en/channels/email-marketing-template-studio` | `Solutions.EmailMarketingTemplateStudio` | `Views/Solutions/EmailMarketingTemplateStudio.cshtml` | "Email Marketing ve Template Studio" | "Email Marketing ve Template Studio" | Hybrid email campaigns and drag-and-drop template editor | Email / Content Studio | DUPLICATE | DUPLICATE | REDIRECT_LATER | HIGH: Confuses email channel with Content Studio. 301 redirect to `/kanallar/email`. |
| `/kullanim-senaryolari` | `/en/use-cases` | `Solutions.UseCases` | `Views/Solutions/UseCases.cshtml` | "Kullanım Senaryoları" | "Kullanım Senaryoları \| Pika Omnichannel Çözümleri" | Industry blueprints: retail, e-commerce, services | Use Cases | CONSISTENT | CURRENT | KEEP | High-value commercial storytelling. |

---

## Detailed Investigation of Suspicious & Mismatched Mappings

### Case 1: `/cozumler/personalization`
- **Current Route:** `/cozumler/personalization` (EN: `/en/solutions/personalization`)
- **Controller Action:** `SolutionsController.PersonalizationPage` -> `View("Personalization")`
- **Current H1:** "Müşteri Etkileşim Yönetimi" (EN: "Journey Orchestration")
- **Actual Content:** Discusses multi-step automated journeys, purchase completion triggers, wait steps, and cart abandonment flows.
- **Root Problem:** The URL promises "personalization", but the page delivers a stub version of "Journey Orchestration". Meanwhile, a complete 27KB page already exists at `/cozumler/journey-manager`.
- **Target Remediation:** Permanent 301 redirect `/cozumler/personalization` to `/cozumler/journey-manager`.

### Case 2: `/cozumler/template-management`
- **Current Route:** `/cozumler/template-management` (EN: `/en/solutions/template-management`)
- **Controller Action:** `SolutionsController.TemplateManagement` -> `View()`
- **Current H1:** "Müşteri Segmentasyonu ve Hedefleme" (EN: "Audience Segmentation & Targeting")
- **Actual Content:** Discusses rule trees, AND/OR logic, inactivity parameters, and dynamic cohort generation. Contains zero information about templates.
- **Root Problem:** The URL promises "template management", but the page delivers "Audience Segmentation". Meanwhile, a complete 27KB page already exists at `/cozumler/audience-manager`.
- **Target Remediation:** Permanent 301 redirect `/cozumler/template-management` to `/cozumler/audience-manager`.

### Case 3: `/cozumler/ab-testing`
- **Current Route:** `/cozumler/ab-testing` (EN: `/en/solutions/ab-testing`)
- **Controller Action:** `SolutionsController.ABTesting` -> `View()`
- **Current H1:** "Kampanya Otomasyonu ve Zamanlama" (EN: "Campaign Automation & Scheduling")
- **Actual Content:** Discusses campaign calendar windows, targeting rules, variant allocations, and delivery queues.
- **Root Problem:** The URL promises "A/B testing", but the page delivers "Campaign Automation". Meanwhile, a complete 37KB page already exists at `/cozumler/campaign-manager`.
- **Target Remediation:** Permanent 301 redirect `/cozumler/ab-testing` to `/cozumler/campaign-manager`.

---

## Duplicate & Cannibalization Analysis

| Primary Canonical Route | Duplicate Competing Route | Nature of Duplicate | Recommendation |
| :--- | :--- | :--- | :--- |
| `/kanallar/whatsapp` | `/kanallar/whatsapp-kampanya-yonetimi` | Both target "WhatsApp campaign management"; identical value propositions. | 301 redirect duplicate to `/kanallar/whatsapp`. |
| `/kanallar/email` & `/cozumler/content-studio` | `/kanallar/email-marketing-template-studio` | Hybrid page blending email sending with template studio; splits SEO ranking. | 301 redirect duplicate to `/kanallar/email`. |
| `/cozumler/consent-management` | `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` | Duplicate landing page targeting the exact same IYS and KVKK compliance keywords. | 301 redirect duplicate to `/cozumler/consent-management`. |
| `/urunler/ai-kampanya-asistani` | `/cozumler/e-ticaret-ai-kampanya-yonetimi` | Duplicate landing page built for e-commerce AI queries; cannibalizes Pika Pilot product page. | 301 redirect duplicate to `/urunler/ai-kampanya-asistani`. |

---

## Orphan Views Inventory

1. **`Views/Home/Pricing.cshtml` (Size: 4.3 KB):**
   - *Status:* **ORPHAN_DANGEROUS.**
   - *Issue:* Hardcoded pricing cards (`₺9.900/ay`, `₺24.900/ay`), SLA claims, feature comparison matrix. There is no controller action routing to this view, but if accidentally routed or exposed, it would cause immediate commercial pricing conflicts.
   - *Recommendation:* Keep strictly unrouted. Move to `ARCHIVE_LATER` during Phase P01.
2. **`Views/Home/Solutions.cshtml` (Size: 5.6 KB):**
   - *Status:* **ORPHAN_MISLEADING.**
   - *Issue:* An unrouted Solutions hub containing wrong hyperlink mappings (`Journey Orchestration` -> `/cozumler/personalization`, `Audience Segmentation` -> `/cozumler/template-management`, `Campaign Automation` -> `/cozumler/ab-testing`).
   - *Recommendation:* Keep strictly unrouted. `DELETE_CANDIDATE` or `ARCHIVE_LATER`.
3. **`Views/Solutions/JourneyManager.cshtml.bak` (Size: 16.0 KB):**
   - *Status:* **ABANDONED_BACKUP.**
   - *Issue:* Git / editor backup file left in production directory.
   - *Recommendation:* Delete in later cleanup phase.
