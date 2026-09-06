# Pika Web Canonical Information Architecture

**Project:** Pika Web 2.0  
**Phase:** P01 — Information Architecture, Route Canonicalization & Discoverability Cleanup  
**Target Repository:** `CemHortoglu/PikaWeb`  
**Date:** September 2026  
**Governance Preconditions:** Strictly adheres to `PRODUCT_TRUTH.md`, `CLAIMS_REGISTRY.md`, `CONTENT_GUARDRAILS.md`, `ENTITY_REGISTRY.md`, and `P00_FINDINGS.md`.

---

## Executive Summary

Phase P01 establishes a canonical, durable, and evidence-backed public information architecture for Pika. Prior to this phase, the public website suffered from severe route-to-content mismatches, search cannibalization caused by duplicate landing pages, unauthorized and outdated pricing cards in an orphaned view, and false exposure of unreleased features in navigation and sitemaps.

Through P01:
1. Every public capability is mapped to an authoritative, non-conflicting canonical entity.
2. Route-to-content inversions have been eliminated, and 4 duplicate landing page pairs have been permanently 301-redirected to authoritative canonical endpoints.
3. The orphan legacy pricing view (`Views/Home/Pricing.cshtml`) has been deleted from disk, and all pricing URLs are permanently routed to custom-quote demo conversion paths in accordance with strict pricing governance.
4. Unconfirmed or thin routes (Push Notifications, Deliverability & Compliance, Data Management ETL, and Real-Time Event Processing) are quarantined with `noindex, follow`, removed from public navigation and sitemaps, while preserving self-canonical integrity.
5. Symmetrical canonical route sets in Turkish and English have been aligned and verified via automated regression suites.

---

## Canonical Public Entities

The following table defines the 26 canonical public entities of the Pika Web marketing surface. Each entity represents a distinct, authoritative product capability, corporate function, or conversion endpoint.

| # | Entity Identifier | Entity Type | Turkish Canonical Slug | English Canonical Slug | Core Purpose / Scope | Status |
|---|---|---|---|---|---|---|
| 1 | `Homepage` | Core | `/` | `/en/` | Primary brand portal and platform overview | Active / Canonical |
| 2 | `AboutPika` | Corporate | `/pika` | `/en/pika` | Company overview, mission, and architectural philosophy | Active / Canonical |
| 3 | `Corporate` | Corporate | `/kurumsal` | `/en/corporate` | Corporate governance, company details, enterprise identity | Active / Canonical |
| 4 | `Faq` | Resources | `/kaynaklar/sss` | `/en/resources/faq` | Prospective buyer and customer FAQ | Active / Canonical |
| 5 | `Contact` | Corporate | `/iletisim` | `/en/contact` | Corporate contact information and support channels | Active / Canonical |
| 6 | `Careers` | Corporate | `/kariyer` | `/en/careers` | Talent acquisition and culture | Active / Canonical |
| 7 | `DemoRequest` | Conversion | `/demo-talebi` | `/en/demo-request` | Primary sales conversion and enterprise quotation request | Active / Canonical |
| 8 | `TermsOfUse` | Legal | `/kullanim-sartlari` | `/en/terms-of-use` | Platform terms of service | Active / Canonical |
| 9 | `PrivacyPolicy` | Legal | `/gizlilik-politikasi` | `/en/privacy-policy` | KVKK and privacy policy | Active / Canonical |
| 10 | `CustomerIntelligence` | Platform Core | `/platform/customer-intelligence` | `/en/platform/customer-intelligence` | Customer Context Engine, RFM, Value Scoring, 360 Profiles | Active / Canonical |
| 11 | `ProductIntelligence` | Platform Core | `/platform/product-intelligence` | `/en/platform/product-intelligence` | Catalog analysis, margins, affinity, stock velocity | Active / Canonical |
| 12 | `Pika360` | Platform Core | `/platform/pika-360` | `/en/platform/pika-360` | Unified customer 360-degree timeline and insights | Active / Canonical |
| 13 | `Opportunities` | Platform Core | `/platform/gunun-firsatlari` | `/en/platform/opportunities` | Daily AI opportunity detector and high-intent segments | Active / Canonical |
| 14 | `CampaignManager` | Solutions | `/cozumler/campaign-manager` | `/en/solutions/campaign-manager` | Multi-channel campaign orchestration & scheduling | Active / Canonical |
| 15 | `AudienceManager` | Solutions | `/cozumler/audience-manager` | `/en/solutions/audience-manager` | Behavioral and transactional segment builder | Active / Canonical |
| 16 | `JourneyManager` | Solutions | `/cozumler/journey-manager` | `/en/solutions/journey-manager` | Triggered lifecycle flows and event journeys | Active / Canonical |
| 17 | `ContentStudio` | Solutions | `/cozumler/content-studio` | `/en/solutions/content-studio` | Drag-and-drop template editor and dynamic content | Active / Canonical |
| 18 | `ConsentManagement` | Solutions | `/cozumler/consent-management` | `/en/solutions/consent-management` | IYS & KVKK compliance, opt-out automation, blacklist sync | Active / Canonical |
| 19 | `EmailMarketing` | Channels | `/kanallar/email` | `/en/channels/email` | Native transactional and marketing email delivery | Active / Canonical |
| 20 | `SmsCampaigns` | Channels | `/kanallar/sms` | `/en/channels/sms` | Native high-throughput SMS delivery | Active / Canonical |
| 21 | `WhatsAppMessaging` | Channels | `/kanallar/whatsapp` | `/en/channels/whatsapp` | Official Meta WhatsApp Business API integration | Active / Canonical |
| 22 | `AnalyticsReporting` | Solutions | `/cozumler/analytics-reporting` | `/en/solutions/analytics-reporting` | Unified campaign performance, attribution, deliverability | Active / Canonical |
| 23 | `Integrations` | Solutions | `/entegrasyonlar` | `/en/integrations` | E-commerce, CRM, ERP, and API connectivity | Active / Canonical |
| 24 | `SecurityPrivacy` | Platform Core | `/guvenlik-ve-gizlilik` | `/en/security-and-privacy` | Enterprise security, data residency, encryption, compliance | Active / Canonical |
| 25 | `AiCampaignAssistant`| Products | `/urunler/ai-kampanya-asistani` | `/en/products/ai-campaign-assistant` | AI campaign ideation, copy generation, and optimization | Active / Canonical |
| 26 | `UseCases` | Solutions | `/kullanim-senaryolari` | `/en/use-cases` | Industry-specific implementation scenarios | Active / Canonical |

---

## Canonical Route Map — Turkish

| Turkish Canonical URL | Controller & Action | View File | Alternate English URL | Robots Directive |
|---|---|---|---|---|
| `https://pika.tr/` | `HomeController.Index` | `Views/Home/Index.cshtml` | `https://pika.tr/en/` | `index, follow` |
| `https://pika.tr/pika` | `HomeController.Pika` | `Views/Home/Pika.cshtml` | `https://pika.tr/en/pika` | `index, follow` |
| `https://pika.tr/kurumsal` | `HomeController.Corporate` | `Views/Home/Corporate.cshtml` | `https://pika.tr/en/corporate` | `index, follow` |
| `https://pika.tr/kaynaklar/sss` | `HomeController.Faq` | `Views/Home/Faq.cshtml` | `https://pika.tr/en/resources/faq` | `index, follow` |
| `https://pika.tr/iletisim` | `HomeController.Contact` | `Views/Home/Contact.cshtml` | `https://pika.tr/en/contact` | `index, follow` |
| `https://pika.tr/kariyer` | `HomeController.Career` | `Views/Home/Career.cshtml` | `https://pika.tr/en/careers` | `index, follow` |
| `https://pika.tr/demo-talebi` | `HomeController.DemoRequest` | `Views/Home/DemoRequest.cshtml` | `https://pika.tr/en/demo-request` | `index, follow` |
| `https://pika.tr/kullanim-sartlari` | `HomeController.TermsOfUse` | `Views/Home/TermsOfUse.cshtml` | `https://pika.tr/en/terms-of-use` | `index, follow` |
| `https://pika.tr/gizlilik-politikasi` | `HomeController.PrivacyPolicy` | `Views/Home/PrivacyPolicy.cshtml` | `https://pika.tr/en/privacy-policy` | `index, follow` |
| `https://pika.tr/platform/customer-intelligence` | `PlatformController.CustomerIntelligence` | `Views/Platform/CustomerIntelligence.cshtml` | `https://pika.tr/en/platform/customer-intelligence` | `index, follow` |
| `https://pika.tr/platform/product-intelligence` | `PlatformController.ProductIntelligence` | `Views/Platform/ProductIntelligence.cshtml` | `https://pika.tr/en/platform/product-intelligence` | `index, follow` |
| `https://pika.tr/platform/pika-360` | `PlatformController.Pika360` | `Views/Platform/Pika360.cshtml` | `https://pika.tr/en/platform/pika-360` | `index, follow` |
| `https://pika.tr/platform/gunun-firsatlari` | `PlatformController.Opportunities` | `Views/Platform/Opportunities.cshtml` | `https://pika.tr/en/platform/opportunities` | `index, follow` |
| `https://pika.tr/cozumler/campaign-manager` | `SolutionsController.CampaignManager` | `Views/Solutions/CampaignManager.cshtml` | `https://pika.tr/en/solutions/campaign-manager` | `index, follow` |
| `https://pika.tr/cozumler/audience-manager` | `SolutionsController.AudienceManager` | `Views/Solutions/AudienceManager.cshtml` | `https://pika.tr/en/solutions/audience-manager` | `index, follow` |
| `https://pika.tr/cozumler/journey-manager` | `SolutionsController.JourneyManager` | `Views/Solutions/JourneyManager.cshtml` | `https://pika.tr/en/solutions/journey-manager` | `index, follow` |
| `https://pika.tr/cozumler/content-studio` | `SolutionsController.ContentStudio` | `Views/Solutions/ContentStudio.cshtml` | `https://pika.tr/en/solutions/content-studio` | `index, follow` |
| `https://pika.tr/cozumler/consent-management` | `SolutionsController.ConsentManagement` | `Views/Solutions/ConsentManagement.cshtml` | `https://pika.tr/en/solutions/consent-management` | `index, follow` |
| `https://pika.tr/kanallar/email` | `SolutionsController.EmailMarketing` | `Views/Solutions/EmailMarketing.cshtml` | `https://pika.tr/en/channels/email` | `index, follow` |
| `https://pika.tr/kanallar/sms` | `SolutionsController.SmsCampaigns` | `Views/Solutions/SmsCampaigns.cshtml` | `https://pika.tr/en/channels/sms` | `index, follow` |
| `https://pika.tr/kanallar/whatsapp` | `SolutionsController.WhatsAppMessaging` | `Views/Solutions/WhatsAppMessaging.cshtml` | `https://pika.tr/en/channels/whatsapp` | `index, follow` |
| `https://pika.tr/cozumler/analytics-reporting` | `SolutionsController.Reporting` | `Views/Solutions/Reporting.cshtml` | `https://pika.tr/en/solutions/analytics-reporting` | `index, follow` |
| `https://pika.tr/entegrasyonlar` | `SolutionsController.Integrations` | `Views/Solutions/Integrations.cshtml` | `https://pika.tr/en/integrations` | `index, follow` |
| `https://pika.tr/guvenlik-ve-gizlilik` | `SolutionsController.SecurityPrivacy` | `Views/Solutions/SecurityPrivacy.cshtml` | `https://pika.tr/en/security-and-privacy` | `index, follow` |
| `https://pika.tr/urunler/ai-kampanya-asistani` | `SolutionsController.AiCampaignAssistant` | `Views/Solutions/AiCampaignAssistant.cshtml` | `https://pika.tr/en/products/ai-campaign-assistant` | `index, follow` |
| `https://pika.tr/kullanim-senaryolari` | `SolutionsController.UseCases` | `Views/Solutions/UseCases.cshtml` | `https://pika.tr/en/use-cases` | `index, follow` |

---

## Canonical Route Map — English

| English Canonical URL | Controller & Action | View File | Alternate Turkish URL | Robots Directive |
|---|---|---|---|---|
| `https://pika.tr/en/` | `HomeController.Index` | `Views/Home/Index.cshtml` | `https://pika.tr/` | `index, follow` |
| `https://pika.tr/en/pika` | `HomeController.Pika` | `Views/Home/Pika.cshtml` | `https://pika.tr/pika` | `index, follow` |
| `https://pika.tr/en/corporate` | `HomeController.Corporate` | `Views/Home/Corporate.cshtml` | `https://pika.tr/kurumsal` | `index, follow` |
| `https://pika.tr/en/resources/faq` | `HomeController.Faq` | `Views/Home/Faq.cshtml` | `https://pika.tr/kaynaklar/sss` | `index, follow` |
| `https://pika.tr/en/contact` | `HomeController.Contact` | `Views/Home/Contact.cshtml` | `https://pika.tr/iletisim` | `index, follow` |
| `https://pika.tr/en/careers` | `HomeController.Career` | `Views/Home/Career.cshtml` | `https://pika.tr/kariyer` | `index, follow` |
| `https://pika.tr/en/demo-request` | `HomeController.DemoRequest` | `Views/Home/DemoRequest.cshtml` | `https://pika.tr/demo-talebi` | `index, follow` |
| `https://pika.tr/en/terms-of-use` | `HomeController.TermsOfUse` | `Views/Home/TermsOfUse.cshtml` | `https://pika.tr/kullanim-sartlari` | `index, follow` |
| `https://pika.tr/en/privacy-policy` | `HomeController.PrivacyPolicy` | `Views/Home/PrivacyPolicy.cshtml` | `https://pika.tr/gizlilik-politikasi` | `index, follow` |
| `https://pika.tr/en/platform/customer-intelligence` | `PlatformController.CustomerIntelligence` | `Views/Platform/CustomerIntelligence.cshtml` | `https://pika.tr/platform/customer-intelligence` | `index, follow` |
| `https://pika.tr/en/platform/product-intelligence` | `PlatformController.ProductIntelligence` | `Views/Platform/ProductIntelligence.cshtml` | `https://pika.tr/platform/product-intelligence` | `index, follow` |
| `https://pika.tr/en/platform/pika-360` | `PlatformController.Pika360` | `Views/Platform/Pika360.cshtml` | `https://pika.tr/platform/pika-360` | `index, follow` |
| `https://pika.tr/en/platform/opportunities` | `PlatformController.Opportunities` | `Views/Platform/Opportunities.cshtml` | `https://pika.tr/platform/gunun-firsatlari` | `index, follow` |
| `https://pika.tr/en/solutions/campaign-manager` | `SolutionsController.CampaignManager` | `Views/Solutions/CampaignManager.cshtml` | `https://pika.tr/cozumler/campaign-manager` | `index, follow` |
| `https://pika.tr/en/solutions/audience-manager` | `SolutionsController.AudienceManager` | `Views/Solutions/AudienceManager.cshtml` | `https://pika.tr/cozumler/audience-manager` | `index, follow` |
| `https://pika.tr/en/solutions/journey-manager` | `SolutionsController.JourneyManager` | `Views/Solutions/JourneyManager.cshtml` | `https://pika.tr/cozumler/journey-manager` | `index, follow` |
| `https://pika.tr/en/solutions/content-studio` | `SolutionsController.ContentStudio` | `Views/Solutions/ContentStudio.cshtml` | `https://pika.tr/cozumler/content-studio` | `index, follow` |
| `https://pika.tr/en/solutions/consent-management` | `SolutionsController.ConsentManagement` | `Views/Solutions/ConsentManagement.cshtml` | `https://pika.tr/cozumler/consent-management` | `index, follow` |
| `https://pika.tr/en/channels/email` | `SolutionsController.EmailMarketing` | `Views/Solutions/EmailMarketing.cshtml` | `https://pika.tr/kanallar/email` | `index, follow` |
| `https://pika.tr/en/channels/sms` | `SolutionsController.SmsCampaigns` | `Views/Solutions/SmsCampaigns.cshtml` | `https://pika.tr/kanallar/sms` | `index, follow` |
| `https://pika.tr/en/channels/whatsapp` | `SolutionsController.WhatsAppMessaging` | `Views/Solutions/WhatsAppMessaging.cshtml` | `https://pika.tr/kanallar/whatsapp` | `index, follow` |
| `https://pika.tr/en/solutions/analytics-reporting` | `SolutionsController.Reporting` | `Views/Solutions/Reporting.cshtml` | `https://pika.tr/cozumler/analytics-reporting` | `index, follow` |
| `https://pika.tr/en/integrations` | `SolutionsController.Integrations` | `Views/Solutions/Integrations.cshtml` | `https://pika.tr/entegrasyonlar` | `index, follow` |
| `https://pika.tr/en/security-and-privacy` | `SolutionsController.SecurityPrivacy` | `Views/Solutions/SecurityPrivacy.cshtml` | `https://pika.tr/guvenlik-ve-gizlilik` | `index, follow` |
| `https://pika.tr/en/products/ai-campaign-assistant` | `SolutionsController.AiCampaignAssistant` | `Views/Solutions/AiCampaignAssistant.cshtml` | `https://pika.tr/urunler/ai-kampanya-asistani` | `index, follow` |
| `https://pika.tr/en/use-cases` | `SolutionsController.UseCases` | `Views/Solutions/UseCases.cshtml` | `https://pika.tr/kullanim-senaryolari` | `index, follow` |

---

## Permanent Redirect Map

All redirects are implemented as direct, single-hop **HTTP 301 Moved Permanently** via `LegacyRouteMapper` middleware and `SolutionsController`. Redirect chains and loops are strictly eliminated.

### 1. Retired Duplicate & Mismatched Solution Routes
- `/cozumler/personalization` & `/cozumler/kisisellestirme` &rarr; `/cozumler/journey-manager`
- `/en/solutions/personalization` &rarr; `/en/solutions/journey-manager`
- `/cozumler/template-management` & `/cozumler/sablon-yonetimi` &rarr; `/cozumler/content-studio`
- `/en/solutions/template-management` &rarr; `/en/solutions/content-studio`
- `/cozumler/ab-testing` & `/cozumler/ab-testleri` &rarr; `/cozumler/campaign-manager`
- `/en/solutions/ab-testing` &rarr; `/en/solutions/campaign-manager`
- `/cozumler/e-ticaret-ai-kampanya` & `/cozumler/e-ticaret-ai-kampanya-yonetimi` &rarr; `/urunler/ai-kampanya-asistani`
- `/en/solutions/ecommerce-ai-campaign` &rarr; `/en/products/ai-campaign-assistant`
- `/kanallar/whatsapp-kampanya-yonetimi` & `/cozumler/whatsapp-kampanya-yonetimi` &rarr; `/kanallar/whatsapp`
- `/en/channels/whatsapp-campaign-management` & `/en/solutions/whatsapp-campaign-management` &rarr; `/en/channels/whatsapp`
- `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` & `/cozumler/iys-kvkk-uyumluluk` &rarr; `/cozumler/consent-management`
- `/en/solutions/iys-kvkk-compliance` &rarr; `/en/solutions/consent-management`
- `/kanallar/email-marketing-template-studio` & `/cozumler/email-marketing-sablon-studyosu` &rarr; `/cozumler/content-studio`
- `/en/channels/email-marketing-template-studio` & `/en/solutions/email-marketing-template-studio` &rarr; `/en/solutions/content-studio`

### 2. Historical Aliases Preservation
- `/platform/firsatlar` & `/tr/platform/firsatlar` &rarr; `/platform/gunun-firsatlari`
- `/Platform/Opportunities` & `/tr/platform/opportunities` &rarr; `/platform/gunun-firsatlari`
- `/kanallar/push-notification` & `/kanallar/push-bildirim` & `/kanallar/push-bildirimleri` &rarr; `/kanallar/push`
- `/Solutions/PushNotifications` & `/solutions/push-notifications` & `/tr/solutions/push-notifications` &rarr; `/kanallar/push`
- `/en/channels/push-notification` & `/en/channels/push-notifications` &rarr; `/en/channels/push`
- `/en/Solutions/PushNotifications` & `/en/solutions/push-notifications` &rarr; `/en/channels/push`

### 3. Pricing Governance Redirects (Quotation Conversion)
- `/pricing` &rarr; `/demo-talebi`
- `/fiyatlandirma` &rarr; `/demo-talebi`
- `/fiyatlar` &rarr; `/demo-talebi`
- `/ucretler` & `/tr/ucretler` &rarr; `/demo-talebi`
- `/home/pricing` & `/Home/Pricing` & `/tr/home/pricing` &rarr; `/demo-talebi`
- `/en/pricing` & `/en/prices` &rarr; `/en/demo-request`
- `/en/home/pricing` & `/en/Home/Pricing` &rarr; `/en/demo-request`

### 4. Legacy MVC & Normalization Redirects
- `www.pika.tr/*` &rarr; `https://pika.tr/*`
- `/tr`, `/tr/`, `/home`, `/home/index`, `/Home/Index` &rarr; `/`
- `/en` &rarr; `/en/`
- `/en/home`, `/en/home/index`, `/en/Home/Index` &rarr; `/en/`
- Controller/action patterns (`/Home/Pika`, `/Home/Contact`, `/Platform/CustomerIntelligence`, `/Solutions/CampaignManager`, etc.) &rarr; Decoupled semantic canonical paths.

---

## Temporarily Noindexed Routes

Four route pairs have been marked with `<meta name="robots" content="noindex, follow" />`. They return HTTP 200 OK to allow crawling, retain a self-referential canonical URL, omit `hreflang` tags, and are entirely excluded from `sitemap.xml` and public navigation.

| Route (TR) | Route (EN) | Reason for Noindex Quarantine | Current Underlying View |
|---|---|---|---|
| `/kanallar/push` | `/en/channels/push` | **Unconfirmed Channel:** Push is currently not approved as a publicly marketable live capability. Repository sources are contradictory and product implementation requires explicit product-owner confirmation. | `Views/Solutions/PushNotifications.cshtml` |
| `/cozumler/deliverability-compliance` | `/en/solutions/deliverability-compliance` | **Thin Route:** High thematic overlap with `ConsentManagement` and `EmailMarketing`; lacks distinct product substance. | `Views/Solutions/DeliverabilityCompliance.cshtml` |
| `/cozumler/data-management-etl` | `/en/solutions/data-management-etl` | **Thin Route:** Architectural capability subsumed under `CustomerIntelligence` and `Integrations`. | `Views/Solutions/DataManagementEtl.cshtml` |
| `/cozumler/real-time-event-processing` | `/en/solutions/real-time-event-processing` | **Thin Route:** Platform feature already articulated inside `CustomerIntelligence` and `JourneyManager`. | `Views/Solutions/RealTimeEventProcessing.cshtml` |

### Architectural Invariants for Noindexed Pages
1. **HTTP 200 OK:** Must serve valid pages so legacy crawlers don't encounter 404 errors.
2. **Self-Referential Canonical:** Points strictly to its own URL (e.g. `https://pika.tr/kanallar/push`). Never canonicalizes to another entity or homepage.
3. **No Hreflang Alternates:** Does not emit alternate language links while noindexed.
4. **Sitemap Cleanliness:** Never included in `sitemap.xml`.
5. **Navigation Absence:** Excluded from header dropdowns, mobile menu, and footer links.

---

## Navigation Architecture

Navigation in `Views/Shared/_Layout.cshtml` reflects the revised information architecture:

### 1. Platform Mega-Dropdown
- **Müşteri Zekâsı / Customer Intelligence** (`/platform/customer-intelligence`)
- **Ürün Zekâsı / Product Intelligence** (`/platform/product-intelligence`)
- **Pika 360** (`/platform/pika-360`)
- **Günün Fırsatları / Daily Opportunities** (`/platform/gunun-firsatlari`)
- **Güvenlik ve Gizlilik / Security & Privacy** (`/guvenlik-ve-gizlilik`) *(Added in P01 under Measure/Platform)*

### 2. Solutions Mega-Dropdown
- **Kampanya Yöneticisi / Campaign Manager** (`/cozumler/campaign-manager`)
- **Kitle Yöneticisi / Audience Manager** (`/cozumler/audience-manager`)
- **Yolculuk Yöneticisi / Journey Manager** (`/cozumler/journey-manager`)
- **İçerik Stüdyosu / Content Studio** (`/cozumler/content-studio`)
- **İzin Yönetimi / Consent Management** (`/cozumler/consent-management`)
- **Analitik ve Raporlama / Analytics & Reporting** (`/cozumler/analytics-reporting`)
- **Kullanım Senaryoları / Use Cases** (`/kullanim-senaryolari`)

### 3. Channels Mega-Dropdown
- **E-posta Pazarlama / Email Marketing** (`/kanallar/email`)
- **SMS Kampanyaları / SMS Campaigns** (`/kanallar/sms`)
- **WhatsApp Mesajlaşma / WhatsApp Messaging** (`/kanallar/whatsapp`)
- *(Push Notification explicitly REMOVED from navigation in P01)*

### 4. Products & Integrations
- **AI Kampanya Asistanı / AI Campaign Assistant** (`/urunler/ai-kampanya-asistani`)
- **Entegrasyonlar / Integrations** (`/entegrasyonlar`)

### 5. Corporate & Resources (Header & Footer)
- **Hakkımızda / About Us** (`/pika`)
- **Kurumsal / Corporate** (`/kurumsal`)
- **Sıkça Sorulan Sorular / FAQ** (`/kaynaklar/sss`)
- **İletişim / Contact** (`/iletisim`)
- **Kariyer / Careers** (`/kariyer`)
- **Kullanım Şartları / Terms of Use** (`/kullanim-sartlari`)
- **Gizlilik Politikası / Privacy Policy** (`/gizlilik-politikasi`)

### 6. Primary Action (CTA)
- **Demo Talebi / Request a Demo** (`/demo-talebi` / `/en/demo-request`)

---

## Removed Legacy Views

The following view has been permanently removed from the repository:
- **`Views/Home/Pricing.cshtml`**
  - **Reason for Removal:** The file was an unrouted orphan view containing hardcoded package prices (`₺9.900/ay`, `₺24.900/ay`), enterprise SLA guarantees, and monthly tiers. Its presence on disk posed a severe commercial risk if mapped or exposed.
  - **Replacement:** Pricing intent is serviced by the custom-quote demo funnel (`/demo-talebi` and `/en/demo-request`).

---

## Pricing Governance

Pika operates under an enterprise quotation-based commercial model.

### Non-Negotiable Rules
1. **No Fixed Public Pricing:** Pika does not publish monthly package rates, "starting from" figures, or tier amounts.
2. **Dynamic Quoting Factors:** Pricing is customized based on customer/contact volume, monthly transaction throughput, messaging/channel volume, onboarding scope, ERP/CRM integration requirements, and SLA tiers.
3. **No Pricing Calculators:** Pricing calculators or tier selectors are prohibited on public marketing pages unless explicitly authorized in future commercial phases.
4. **Approved Public Formulation:**
   - *Turkish:* "İhtiyacınıza ve kullanım kapsamınıza göre özel teklif"
   - *English:* "Custom quotation tailored to your requirements and usage scale"
5. **Primary Pricing CTA:** "Teklif Alın" or "Demo Talebi" linking to `/demo-talebi` (`/en/demo-request`).

---

## Sitemap Rules

The canonical `sitemap.xml` reflects the clean marketing and wiki surface:
1. **Total URLs:** Exactly 141 URLs (26 Turkish canonical marketing URLs + 26 English canonical marketing URLs + 89 public Wiki URLs).
2. **Purity Exclusions:**
   - Zero redirected routes (no duplicate pairs, no legacy aliases, no MVC controller paths).
   - Zero noindexed routes (`/kanallar/push`, `deliverability-compliance`, `data-management-etl`, `real-time-event-processing`).
   - Zero pricing URLs (`/pricing`, `/fiyatlandirma`).
   - Zero internal wiki, auth, login, or error URLs.
3. **HTTP Status Guarantee:** 100% of URLs listed in `sitemap.xml` return HTTP 200 OK.

---

## SEO Canonicalization Rules

1. **Apex Domain Normalization:** All requests to `www.pika.tr` permanently 301-redirect to `https://pika.tr`.
2. **Strict Self-Canonicalization:** Every indexable public page carries an exact self-referential canonical tag (`<link rel="canonical" href="https://pika.tr/..." />`).
3. **Multi-Lingual Hreflang Symmetry:**
   - Turkish canonical pages emit:
     - `<link rel="alternate" hreflang="tr" href="https://pika.tr/..." />`
     - `<link rel="alternate" hreflang="en" href="https://pika.tr/en/..." />`
     - `<link rel="alternate" hreflang="x-default" href="https://pika.tr/..." />`
   - English canonical pages emit:
     - `<link rel="alternate" hreflang="en" href="https://pika.tr/en/..." />`
     - `<link rel="alternate" hreflang="tr" href="https://pika.tr/..." />`
4. **Trailing Slash Standards:**
   - Homepages: `https://pika.tr/` and `https://pika.tr/en/` retain trailing slashes.
   - Inner Pages: All inner pages (`/pika`, `/cozumler/campaign-manager`, etc.) are standardized without trailing slashes.

---

## LLM Route Consistency Rules

1. **`wwwroot/llms.txt`:** Serves as the concise entrypoint for LLMs and crawler agents. References only active canonical routes and accurately states supported channels.
2. **`wwwroot/llms-full.txt`:**
   - Retired duplicates (`/cozumler/personalization`, `/cozumler/template-management`, `/cozumler/ab-testing`, etc.) have been removed.
   - Thin noindexed routes have been removed from the primary solutions inventory.
   - `/urunler/ai-kampanya-asistani` (`/en/products/ai-campaign-assistant`) has been formally registered.
   - False claims (e.g. SOC-2, SAML SSO, two-way AI chatbots) removed during P00 audit remain strictly barred.

---

## Deferred Pages

The following pages require substantive copywriting, visual asset preparation, or engineering confirmation before they can be promoted to canonical indexable status:
1. **Push Notifications (`/kanallar/push` / `/en/channels/push`):** Current public product status requires explicit product-owner confirmation; not approved as a publicly marketable live capability.
2. **Deliverability & Compliance (`/cozumler/deliverability-compliance`):** Needs redesign as a specialized technical whitepaper or consolidation into `ConsentManagement` and `EmailMarketing`.
3. **Data Management ETL (`/cozumler/data-management-etl`):** Needs positioning either as developer documentation in the Wiki or integration architecture guides.
4. **Real-Time Event Processing (`/cozumler/real-time-event-processing`):** Needs customer-facing workflow diagrams and real-time triggers proof before standalone indexing.

---

## Human Decisions Still Required

1. **Push Notifications Strategy:** Provide explicit product-owner confirmation on live mobile/web push capability and roadmap timeline. Until confirmed and approved, Push remains quarantined with `noindex, follow` and excluded from primary navigation and sitemaps.
2. **Consolidation of Thin Solution Views:** Decide in P02/P03 whether `DeliverabilityCompliance`, `DataManagementEtl`, and `RealTimeEventProcessing` should be permanently 301-redirected into parent platform pages or expanded with distinct content and screenshots.
3. **Enterprise Case Study Attribution:** Approve customer names or anonymized case study disclaimers for metrics currently staged in demo cards.
