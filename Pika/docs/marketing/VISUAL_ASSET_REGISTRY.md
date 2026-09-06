# Visual Asset & Product Screenshot Registry

This document inventories all real product UI screenshots, diagrams, and marketing graphics within the repository. It establishes which real assets can replace hand-built CSS mockups on public marketing pages and identifies broken image links.

---

## The Core Visual Principle for Pika Web 2.0

> [!IMPORTANT]
> **If a real, current Pika product screenshot exists, future marketing implementation should normally prefer it over a hand-built fake CSS mockup.**
> 
> Currently, the repository contains **29 authentic product screenshots** from the live Pika application (`https://app.pika.tr`). However, **28 of them are completely absent from public marketing pages**, leaving marketing visitors with synthetic CSS cards while real, high-credibility software screenshots remain hidden inside the Wiki assets folder.

---

## Broken Image Incident (P0 Priority)

- **Referencing File:** `Views/Solutions/CampaignManager.cshtml:232`
- **Broken Path:** `<img src="/wiki/assets/images/img_kampanya-yonetimi_0.png" ... />`
- **File System Reality:** `img_kampanya-yonetimi_0.png` **DOES NOT EXIST** on disk. It produces a live HTTP 404 error on a flagship public solution page.
- **Immediate Recommended Replacement:** Replace with `img_yayinlama-sablon-ve-yonetim_27.png` or `img_aksiyon-calisma-alani_9.png`.

---

## Real Product UI Screenshot Inventory (`wwwroot/wiki/assets/images/`)

| Filename | Feature / Module Represented | Reflects Current UI? | Sensitive Data? | Public Marketing Suitable? | Best Future Landing Page | Potential Crop / Spotlight Use | Notes / Replacement Needed? |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| `img_pika-360_7.png` | Pika 360 Unified Customer Profile (Value, Risk, Behavior, Channel Access, Opportunities) | YES (App UI) | Anonymized sample persona | **YES (Top Priority)** | `/platform/pika-360` & Homepage | Spotlight customer value score, churn risk gauge, and opportunity cards | High aesthetic quality. Replaces fake CSS customer profile. |
| `img_gunun-firsatlari.png` | Günün Fırsatları / Daily Opportunities Cockpit | YES (App UI) | Anonymized | **YES (Top Priority)** | `/platform/gunun-firsatlari` & Homepage | Crop opportunity cards (Replenishment, Win-back, Cross-sell) | Flagship product proof for Pika's opportunity engine. |
| `img_bi-kokpit_5.png` | BI Cockpit (Executive Revenue, Store Metrics, Channel Breakdown) | YES (App UI) | Anonymized aggregate | **YES (Top Priority)** | `/cozumler/analytics-reporting` | Crop store performance rankings and turnover distribution | Replaces generic reporting cards. Shows genuine BI capability. |
| `img_ai-musteri-ozeti_8.png` | AI Customer Summary (Natural language customer profile interpretation) | YES (App UI) | Anonymized | **YES (Top Priority)** | `/platform/pika-360` & `/platform/customer-intelligence` | Spotlight natural language AI summary block | High credibility proof of assistive AI in Pika. |
| `img_pika-pilot-ai-kampanya-asistani_16.png` | Pika Pilot AI Campaign Assistant (Prompt-to-draft engine) | YES (App UI) | Anonymized | **YES (Already Used)** | `/urunler/ai-kampanya-asistani` | Spotlight prompt bar and generated draft cards | The only screenshot currently active in marketing views. |
| `img_journey-tasarim-tuvali_18.png` | Journey Manager visual workflow canvas (Triggers, nodes, delays) | YES (App UI) | Anonymized | **YES (Top Priority)** | `/cozumler/journey-manager` | Spotlight multi-channel branching nodes (SMS/Email/Wait) | Replaces hand-built journey diagrams. Proves drag-and-drop workflow capability. |
| `img_journey-karar-kurallari_19.png` | Journey Decision and Branching Rules | YES (App UI) | Anonymized | **YES** | `/cozumler/journey-manager` | Crop conditional rule configuration modal | Proves granular rule-based routing depth. |
| `img_journey-store_21.png` | Pre-built Journey Templates Library (Journey Store) | YES (App UI) | None | **YES** | `/cozumler/journey-manager` & `/kullanim-senaryolari` | Spotlight ready-to-use blueprints (Onboarding, Win-back) | High commercial appeal for rapid time-to-value. |
| `img_email-template-editor_17.png` | Visual drag-and-drop responsive email builder | YES (App UI) | Mock product creative | **YES (Top Priority)** | `/cozumler/content-studio` & `/kanallar/email` | Spotlight block drag-and-drop canvas and mobile preview toggle | Crucial proof that Pika contains a modern email studio. |
| `img_email-store_20.png` | Pre-built Email Template Library (Email Store) | YES (App UI) | Mock designs | **YES** | `/cozumler/content-studio` & `/kanallar/email` | Crop grid of responsive email templates | Demonstrates out-of-the-box template readiness. |
| `img_need-group-product-role_11.png` | Need Group & Product Role Classification Interface | YES (App UI) | Retail catalog sample | **YES (Top Priority)** | `/platform/product-intelligence` | Spotlight Need Group and Product Role tag assignments | Core proof of Product Intelligence methodology. |
| `img_tekrar-satin-alma-analizi_6.png` | Repeat Purchase Consumption Cycle & Rhythm Analysis | YES (App UI) | Anonymized | **YES (Top Priority)** | `/platform/customer-intelligence` & `/platform/gunun-firsatlari` | Spotlight purchasing frequency distribution histogram | High technical credibility for replenishment algorithms. |
| `img_aksiyon-calisma-alani_9.png` | Action Workspace (Converting opportunities to campaigns) | YES (App UI) | Anonymized | **YES** | `/cozumler/campaign-manager` | Spotlight action queue and cohort dispatch trigger | Ideal replacement for broken image in `CampaignManager.cshtml`. |
| `img_yayinlama-sablon-ve-yonetim_27.png`| Campaign Publishing and Template Binding Interface | YES (App UI) | Anonymized | **YES** | `/cozumler/campaign-manager` | Spotlight dispatch scheduling controls and channel selection | Alternative high-res replacement for `CampaignManager.cshtml`. |
| `img_gonderim-operasyonu-izleme_28.png` | Delivery Operations Monitoring (Attempt logs, status codes) | YES (App UI) | Anonymized job logs | **YES** | `/cozumler/analytics-reporting` & `/guvenlik-ve-gizlilik` | Spotlight gateway status telemetry (Delivered, Bounced) | Proves enterprise delivery worker infrastructure. |
| `img_izin-kanal-zamanlama_25.png` | Consent, Channel Eligibility & Quiet-Hours Scheduling | YES (App UI) | Anonymized | **YES** | `/cozumler/consent-management` & `/kanallar/sms` | Spotlight IYS permission check and quiet hours toggle | Crucial visual proof for regulatory compliance tooling. |
| `img_kisi-listesi-ve-segmentler_3.png` | Contact List & Dynamic Segment Tags | YES (App UI) | Anonymized | **YES** | `/cozumler/audience-manager` | Spotlight segment filter bar and RFM cohort tags | Proves Audience Manager software capability. |
| `img_segment-sablonlari_22.png` | Segment Templates & Pre-built Cohort Rules | YES (App UI) | None | **YES** | `/cozumler/audience-manager` | Crop pre-defined segment cards (VIP, At-Risk, Lapsed) | Illustrates out-of-the-box segmentation logic. |
| `img_segmentasyon-ve-firsatlar_26.png` | Segmentation & Opportunity Correlation Matrix | YES (App UI) | Anonymized | **YES** | `/platform/customer-intelligence` | Crop opportunity count by customer tier | Demonstrates intelligence-to-opportunity bridge. |
| `img_dinamik-siniflandirma-alanlari_12.png`| Dynamic Attribute Classification Fields | YES (App UI) | Anonymized | **YES** | `/platform/product-intelligence` | Crop dynamic custom field definitions | Shows flexible schema customization. |
| `img_urun-siniflandirma-workbench_13.png`| Product Classification Workbench | YES (App UI) | Catalog sample | **YES** | `/platform/product-intelligence` | Spotlight bulk catalog enrichment workbench | Demonstrates operational efficiency for merchandisers. |
| `img_review-resolution-readiness_14.png`| Review, Resolution & Analytical Readiness Pipeline | YES (App UI) | Catalog sample | **YES** | `/platform/product-intelligence` | Spotlight resolution readiness score | Proves data governance depth. |
| `img_master-urun-anlamlandirmalari_15.png`| Master Product Contextualization & Association | YES (App UI) | Catalog sample | **YES** | `/platform/product-intelligence` | Crop cross-sell product association linkages | Shows market basket analytics in action. |
| `img_playbook-sektorel-anlam_10.png` | Industry Playbook Semantic Mapping Interface | YES (App UI) | None | **YES** | `/platform/product-intelligence` & `/kullanim-senaryolari` | Spotlight industry-specific taxonomy presets | Shows vertical specialization (Retail, E-commerce). |
| `img_excel-csv-aktarimi_1.png` | Excel and CSV Ingestion & Column Mapping Screen | YES (App UI) | Sample columns | **YES** | `/entegrasyonlar` | Spotlight drag-and-drop file upload & column mapper | Shows onboarding simplicity for non-API users. |
| `img_satis-veri-operasyonlari_2.png` | Sales & Order Data Ingestion Operations Screen | YES (App UI) | Sample invoice records | **YES** | `/entegrasyonlar` | Spotlight transaction stream and validation checks | Proves transactional data ingestion foundation. |
| `img_gmail-kisi-aktarimi_23.png` | Google Contacts Import Workflow | YES (App UI) | Anonymized | **YES** | `/entegrasyonlar` & `/wiki/` | Spotlight OAuth sync button and contact preview | Proves built-in Google contact connector. |
| `img_kullanici-roller-yetkiler_24.png` | User Roles and RBAC Permission Matrix | YES (App UI) | Role names | **YES** | `/guvenlik-ve-gizlilik` | Spotlight role permission toggles (Admin, Marketer, Viewer)| Direct visual proof of enterprise RBAC security. |
| `img_kategori-yonetimi_4.png` | Category Hierarchy Management | YES (App UI) | Catalog sample | **YES** | `/platform/product-intelligence` | Crop category tree | Useful supporting asset. |
| `kisi-aktarimi-gmail-outlook-anonim.png`| Anonymized Gmail/Outlook Contact Mapping Diagram | Diagram / Graphic | None | **YES** | `/wiki/gmail-kisi-aktarimi` & `/entegrasyonlar` | Educational diagram showing contact ingestion | Explanatory asset. |

---

## Marketing Views Violating the Real-Screenshot Principle

| Page / Route | Current Hand-Built Mockup Description | Available Authentic Product Screenshot to Replace It |
| :--- | :--- | :--- |
| **Homepage (`/`)** | Hand-crafted HTML/CSS cockpit card (`.ph-mock-stat`, `.ph-mock-stat-val`) with hardcoded synthetic stats (12.4K, %68, %24). | Replace with high-impact hero crop of **`img_gunun-firsatlari.png`** or **`img_pika-360_7.png`**. |
| **Pika 360 (`/platform/pika-360`)** | Hand-coded HTML customer card showing fake persona with CSS badges. | Replace with full-bleed framed asset **`img_pika-360_7.png`** with spotlight callouts. |
| **Günün Fırsatları (`/platform/gunun-firsatlari`)**| CSS list items illustrating repeat purchase, win-back, and cross-sell. | Replace with genuine cockpit UI **`img_gunun-firsatlari.png`**. |
| **Product Intelligence (`/platform/product-intelligence`)**| Hand-built CSS tables and cards explaining Need Groups and Product Roles. | Replace with real UI screenshots **`img_need-group-product-role_11.png`** and **`img_urun-siniflandirma-workbench_13.png`**. |
| **Campaign Manager (`/cozumler/campaign-manager`)**| Broken image link (`img_kampanya-yonetimi_0.png`) and hardcoded simulation box with unverified 14.2x ROAS. | Replace with real software UI **`img_yayinlama-sablon-ve-yonetim_27.png`** or **`img_aksiyon-calisma-alani_9.png`**. |
| **Journey Manager (`/cozumler/journey-manager`)**| Stylized CSS flowchart nodes with icons. | Replace with authentic visual journey builder **`img_journey-tasarim-tuvali_18.png`**. |
| **Content Studio (`/cozumler/content-studio`)**| Hand-coded HTML drag-and-drop simulation. | Replace with real email editor screenshot **`img_email-template-editor_17.png`**. |
| **Audience Manager (`/cozumler/audience-manager`)**| Hand-coded floating badge elements (12.8K kişi, 5 koşul). | Replace with genuine cohort management interface **`img_kisi-listesi-ve-segmentler_3.png`**. |
| **Analytics & Reporting (`/cozumler/analytics-reporting`)**| Hand-built CSS metric cards. | Replace with real BI Cockpit screenshot **`img_bi-kokpit_5.png`**. |

---

## Unused & Legacy Marketing Assets Inventory

1. **`wwwroot/images/1.png` to `5.png`:**
   - *Status:* **UNUSED_LEGACY.**
   - *Audit Finding:* These 5 image files (approx. 100KB–150KB each) are not referenced anywhere in the Views, CSS, or scripts.
   - *Recommendation:* Candidate for archiving in cleanup phase.
2. **`wwwroot/web/images/partner/partner-logo-1.png` to `11.png`:**
   - *Status:* **UNUSED_TEMPLATE_ARTIFACT.**
   - *Audit Finding:* 11 generic template placeholder logos from the original theme purchase. They are not referenced anywhere in the application.
   - *Recommendation:* Keep strictly unreferenced. Do not display on public site as fake client or partner proof.
3. **`wwwroot/og-image-generator.html` & `og-image.png`:**
   - *Status:* **OPERATIONAL.**
   - *Audit Finding:* `og-image.png` is correctly linked in `_Layout.cshtml` for OpenGraph and Twitter cards.
