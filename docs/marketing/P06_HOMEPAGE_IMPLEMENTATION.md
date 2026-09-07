# PIKA WEB 2.0 — P06: HOMEPAGE V2 IMPLEMENTATION REPORT
**Canonical Document:** `docs/marketing/P06_HOMEPAGE_IMPLEMENTATION.md`  
**Phase:** P06 — Homepage V2 (Product-Led Commercial Narrative)  
**Status:** COMPLETED  
**Approved Baseline Commit:** `438f66cd0c16279a9bc8737902704b782e504351`  
**Target Category:**
- TR: `Müşteri Zekâsı ve Omnichannel Pazarlama Platformu`
- EN: `Customer Intelligence & Omnichannel Marketing Platform`

---

## 1. Redesign Objectives and Architectural Narrative

The P06 phase transforms the Pika homepage (`/` and `/en/`) from a generic, marketing-automation entry point into a mature B2B **Customer Intelligence & Omnichannel Marketing Platform**.

### Core Positioning Shift
- **Not a generic marketing automation tool:** Pika does not begin with empty bulk blasting or campaign calendars.
- **Not a pure messaging gateway:** Pika does not compete on wholesale SMS/WhatsApp pricing or messaging delivery pipes.
- **Not an AI-first marketing toy:** Pika does not lead with opaque generative chatbots or fictitious conversion promises.
- **The True Platform Thesis:** Pika starts by unifying customer, product, and transaction context (Data Layer), algorithmically uncovers immediate commercial opportunities (Intelligence & Decision Layer), and executes precision omnichannel engagements (Execution Layer) across confirmed channels.

---

## 2. Nine-Chapter Narrative Architecture

The homepage is organized into nine sequential, logical chapters that lead the buyer through Pika's commercial value:

| Chapter # | Chapter Name | Landmark ID | Architectural & Commercial Purpose |
|---|---|---|---|
| **1** | **Hero: Unified Commercial Intelligence Above the Fold** | `#hero` | Establishes the canonical category immediately. Dual-stage product visual pairing Pika 360 (`img_pika-360_7.png`) and Günün Fırsatları (`img_gunun-firsatlari.png`). Action CTAs: "Demo Talep Et", "Fırsat Motorunu İnceleyin", and "Teklif Al". |
| **2** | **Günün Fırsatları: Commercial Opportunity Engine** | `#firsatlar` | Demonstrates the core differentiator. Shows daily surfaced opportunities with 4 concrete commercial scenarios: Repurchase Rhythm (%80-%120 consumption window), Cross-Category Expansion, Basket Affinity, and Churn Prevention. |
| **3** | **Operating Model: Data → Intelligence → Action** | `#calisma-modeli` | Explains how raw data (e-commerce, offline receipts, POS/ERP) transforms through the intelligence layer into governed action. Anchored by the asynchronous ingestion workbench screenshot (`img_excel-csv-aktarimi_1.png`). |
| **4** | **Customer Intelligence & Product Intelligence** | `#zeka-katmani` | Twin-engine foundation. Customer side: CVS score (0-100), natural replenishment cycles, churn risk + repurchase analysis screenshot (`img_tekrar-satin-alma-analizi_6.png`). Product side: Need Groups, Product Roles, basket affinities + workbench screenshot (`img_urun-siniflandirma-workbench_13.png`). |
| **5** | **Pika 360: Unified Customer Decision Console** | `#pika-360` | Demonstrates single-customer view with actionable commercial recommendations. Centered on full-stage Pika 360 screenshot (`img_pika-360_7.png`) annotated with 4 pillars: Order Timeline, Calculated CVS, Active Opportunities, and Governed Dispatch. |
| **6** | **Decision to Action: Governed Execution Engine** | `#aksiyon-ve-icra` | Visual proof of execution tools: Journey Manager visual canvas (`img_journey-tasarim-tuvali_18.png`), Audience Manager (`img_kisi-listesi-ve-segmentler_3.png`), and Campaign Manager (`img_yayinlama-sablon-ve-yonetim_27.png`). Highlights confirmed channels: Email, SMS, WhatsApp. |
| **7** | **Pika Pilot: Grounded Campaign & Decision AI** | `#pika-pilot` | Positions AI as an operational accelerator grounded in real customer and catalog data, not an autonomous black box. Anchored by campaign assistant screenshot (`img_pika-pilot-ai-kampanya-asistani_16.png`). |
| **8** | **Measurement & Trust: Governed Execution** | `#yonetisim-ve-olcumleme` | Dark petrol theme representing enterprise governance. BI Cockpit screenshot (`img_bi-kokpit_5.png`). 4 trust pillars: Attributed Revenue (ilişkilendirilen ciro), Delivery Telemetry, Consent & Dispatch Windows (quiet hours), and Security & Isolation. |
| **9** | **Final Commercial CTA: Custom Quote & Engagement** | `#teklif-ve-demo` | Clean commercial card with the canonical pricing statement: *"İhtiyacınıza ve kullanım kapsamınıza göre özel teklif."* / *"Custom quote based on your specific needs and usage scope."* Direct actions for Demo Request and Custom Quote. |

---

## 3. Screenshot Inventory & Manifest Alignment

All 10 screenshot placements utilize verified, real screenshots from `docs/marketing/VISUAL_EVIDENCE_MANIFEST.md`. Manifest-driven demo badges (`Örnek Gösterim` / `Sample View`) are applied strictly where `metricStatus == "DEMO_SYNTHETIC"` and omitted where `metricStatus == "NONE"`.

| # | Chapter | Asset Path | Alt Text (TR / EN Summary) | Metric Status | Demo Badge | Rationale & Cropping |
|---|---|---|---|---|---|---|
| 1 | Hero (Main) | `~/wiki/assets/images/img_pika-360_7.png` | Pika 360 müşteri karar konsolu / Pika 360 decision console | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Establishes single-customer intelligence as the anchor above the fold. |
| 2 | Hero (Companion) | `~/wiki/assets/images/img_gunun-firsatlari.png` | Günün Fırsatları karar kartları / Daily Opportunities decision cards | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Shows real opportunity identification next to the customer timeline. |
| 3 | Chapter 2 | `~/wiki/assets/images/img_gunun-firsatlari.png` | Günün Fırsatları detay ekranı / Daily Opportunities workspace | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Full-width stage providing direct visual evidence for CCE algorithm. |
| 4 | Chapter 3 | `~/wiki/assets/images/img_excel-csv-aktarimi_1.png` | Asenkron veri aktarımı / Ingestion workbench | `NONE` | *None* | Demonstrates realistic data ingestion. Clean top crop (`y: 36px`) via CSS `.pw2-home-ingestion-crop`. |
| 5 | Chapter 4 (Left) | `~/wiki/assets/images/img_tekrar-satin-alma-analizi_6.png` | Tekrar satın alma döngü analizi / Repurchase rhythm analysis | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Proves mathematical calculation of customer consumption windows. |
| 6 | Chapter 4 (Right) | `~/wiki/assets/images/img_urun-siniflandirma-workbench_13.png` | Ürün sınıflandırma çalışma alanı / Product classification workbench | `NONE` | *None* | Proves product role assignment and merchandising taxonomy. |
| 7 | Chapter 5 | `~/wiki/assets/images/img_pika-360_7.png` | Pika 360 müşteri karar konsolu / Pika 360 decision console | `DEMO_SYNTHETIC` | **Örnek Gösterim** | High-resolution central inspection stage with capability annotations. |
| 8 | Chapter 6 (Primary) | `~/wiki/assets/images/img_journey-tasarim-tuvali_18.png` | Journey Manager görsel tasarım tuvali / Visual journey canvas | `NONE` | *None* | Large visual canvas proving node/event/condition execution engine. |
| 9 | Chapter 6 (Aux 1) | `~/wiki/assets/images/img_kisi-listesi-ve-segmentler_3.png` | Audience Manager kitle ve segment yönetimi / Audience cohort rules | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Supporting proof of dynamic behavioral segmentation. |
| 10 | Chapter 6 (Aux 2) | `~/wiki/assets/images/img_yayinlama-sablon-ve-yonetim_27.png` | Campaign Manager yayınlama konsolu / Broadcast dispatch console | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Supporting proof of governed broadcast scheduling and approvals. |
| 11 | Chapter 7 | `~/wiki/assets/images/img_pika-pilot-ai-kampanya-asistani_16.png` | Pika Pilot AI kampanya asistanı / AI campaign assistant workspace | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Proves grounded AI assistance using natural language and real prompts. |
| 12 | Chapter 8 | `~/wiki/assets/images/img_bi-kokpit_5.png` | Pika BI Kokpit analitik ekranı / BI Cockpit analytics stage | `DEMO_SYNTHETIC` | **Örnek Gösterim** | Proves attributed turnover, store metrics, and delivery telemetry. |

---

## 4. Evidence Hygiene & Dead Code Removal

### Removed
- **Fictitious Synthetic Mocks:** Removed all fake CSS dashboard mocks (`.ph-mock-dash`, `.ph-mock-card`, `.ph-mock-stat`, `.ph-flow-step`, `.ph-seg-visual`, `.ph-ai-panel`).
- **Unverified Metric Claims:** Completely purged legacy hardcoded statistics (`12.4K`, `%68`, `%24`).
- **Unsupported Prestige Adjectives:** Completely purged all occurrences of `"enterprise-grade"` across copy and titles.
- **Generic Automation Copy:** Removed obsolete category string `"Omnichannel Pazarlama Otomasyonu ve Müşteri Yolculuğu Platformu"`.
- **Public Pricing Artifacts:** Ensured no tier boxes, pricing cards, or SLAs exist on the homepage.
- **Unconfirmed Channels:** Cleaned all instances of "web push", "mobile push", "push notifications", and unconstrained "tüm kanallar".

### Preserved
- Shared navigation system styles (`.pika-mega-*`, `.pika-nav-*`).
- Existing global button primitives and base CSS variables.
- Modal infrastructure (`#demoModal`).

### Added
- Scoped homepage layout extensions in `pika-home.css` using `.pw2-home-*` and `.pw2-product-*` primitives.
- Product frame styling (`.pw2-product-frame`) with window header dots, title badges, and dark backdrop stages.
- Manifest-compliant demo badges (`.pw2-badge--demo`).

---

## 5. Channel Governance Policy

Pika's confirmed execution channels are strictly limited to:
1. **E-posta (Email)**
2. **SMS**
3. **WhatsApp (Resmi Meta WhatsApp Business API)**

- **Push Notifications:** STRICTLY FORBIDDEN on active marketing surfaces. Push capability remains unconfirmed in production code and must never be advertised.
- **Channel Phrasing:** The phrase "tüm kanallar" / "all channels" is strictly prohibited. Pika communicates: *"E-posta, SMS ve WhatsApp"* (TR) / *"Email, SMS, and WhatsApp"* (EN).
- **Attributed Revenue:** Governed as *"ilişkilendirilen ciro"* or *"ciro atfı"* in TR, and *"attributed revenue"* in EN. Never phrased as causal "sales caused by".
- **Dispatch Windows:** Governed as *"gönderim zaman pencereleri"* in TR, and *"quiet-hours dispatch window"* in EN. Never phrased as "KVKK quiet hours".

---

## 6. Design Tokens & CSS Architecture

All new styles in `pika-home.css` adhere to the P02 Design Constitution:
- **Color Tokens:**
  - `--pw2-bg-surface` (`#ffffff`), `--pw2-bg-subtle` (`#f8fafc`), `--pw2-bg-dark` (`#0c1926` / deep petrol)
  - `--pw2-border-subtle` (`#e2e8f0`), `--pw2-border-stage` (`#cbd5e1`)
  - `--pw2-text-primary` (`#0f172a`), `--pw2-text-secondary` (`#475569`), `--pw2-text-muted` (`#64748b`)
  - `--pw2-brand-teal` (`#0d9488`), `--pw2-accent-ink` (`#0284c7`), `--pw2-accent-amber` (`#d97706`)
- **Typography:**
  - Standardized on Plus Jakarta Sans via `.pw2-display`, `.pw2-h2`, `.pw2-h3`, `.pw2-lead`, and `.pw2-body-sm`.
- **Zero Layout Regression:**
  - Styles are scoped under `.pika-home-v2` body class and `.pw2-home-*` BEM namespaces.
  - Navbar mega menu classes (`.pika-mega-*`) were carefully preserved.

---

## 7. Bilingual Parity Verification (TR / EN)

Complete symmetric parity is maintained across `/` (TR) and `/en/` (EN):
- **SEO Authority:** Handled centrally by `SeoHelper` without view-level `ViewData["Title"]` or `ViewData["MetaDescription"]` overrides.
- **Culture Routing:** `/en/` maps cleanly to `HomeController.EnglishIndex`, normalized in `SeoHelper.GetMetadata` to `Home.Index` so canonical English titles and meta descriptions resolve seamlessly.
- **Section Landmarks:** Identical chapter structure and element IDs across both cultures.
- **Copy Accuracy:** All 9 chapters have tailored English equivalents matching B2B enterprise SaaS standards while adhering to P04 Content Bible rules.

---

## 8. Test Coverage & Verification

A dedicated test suite, `P06HomepageTests.cs`, was added to `Pika.Web.Tests`:

| Test Method | Category / Requirement | Status |
|---|---|---|
| `Homepage_Returns200Ok` | HTTP 200 verification for `/` and `/en/` | **PASSED** |
| `TurkishHomepage_RendersCanonicalTitleAndMetaDescriptionFromSeoHelper` | Canonical TR Title & Meta Description via `SeoHelper` | **PASSED** |
| `EnglishHomepage_RendersCanonicalTitleAndMetaDescriptionFromSeoHelper` | Canonical EN Title & Meta Description via `SeoHelper` | **PASSED** |
| `Homepage_ContainsNoPushNotificationClaims` | Strict absence of push notification claims | **PASSED** |
| `Homepage_DoesNotContainUnconstrainedChannelClaims` | Strict absence of "tüm kanallar" / "all channels" | **PASSED** |
| `Homepage_DoesNotRenderSoftwareApplicationOrFAQPageSchema` | Absence of SoftwareApplication, FAQPage, or Offer schemas | **PASSED** |
| `Homepage_DoesNotContainOldSyntheticMetrics` | Absence of `12.4K`, `%68`, `%24`, `.ph-mock*` | **PASSED** |
| `Homepage_DoesNotContainOldCategoryNameOrEnterpriseGrade` | Absence of legacy category name and "enterprise-grade" | **PASSED** |
| `Homepage_ContainsAllNineChapterLandmarkSections` | All 9 chapter landmark sections present with IDs | **PASSED** |
| `Homepage_AllReferencedScreenshotFilesExistOnDisk` | Physical existence of all 10 screenshot files in `wwwroot` | **PASSED** |
| `Homepage_DemoBadgesAreRenderedOnlyForDemoSyntheticScreenshots` | Manifest-driven demo badge presence & absence | **PASSED** |
| `TurkishHomepage_RendersCanonicalCommercialAndGovernanceCopy` | TR quote copy, attributed revenue, dispatch windows | **PASSED** |
| `EnglishHomepage_RendersCanonicalCommercialAndGovernanceCopy` | EN quote copy, attributed revenue, dispatch windows | **PASSED** |
| `Homepage_CompliesWithAccessibilityAndSemanticStandards` | Single H1, alt tags on images, aria-hidden on icons | **PASSED** |

**Total Suite Result:** 300 tests passed, 0 failed, 0 skipped.

---

## 9. Deferred Roadmap Items (P07–P12)

The following items are recognized as out-of-scope for P06 and deferred to subsequent phases:
- **P07 (Product Deep-Dive Pages):** Updating `/platform/customer-intelligence`, `/platform/product-intelligence`, and `/platform/pika-360` to align with the new visual system.
- **P08 (Solutions & Use Cases):** Redesigning `/cozumler/*` solution pages and formalizing the noindex status of `/cozumler/push-bildirimleri`.
- **P09 (Integrations & Developers):** Dedicated integration catalog, API guides, and webhooks documentation.
- **P10 (Resources & Case Studies):** Content hub and case studies.
- **P11 (Commercial & Demo Journey):** Interactive quotation builder and tailored demo booking funnel.
- **P12 (Production Launch & Final Governance Verification):** End-to-end performance audits, bundle minification, and launch sign-off.

---

## 10. Commit Baseline & Diff Summary

- **Baseline Commit:** `438f66cd0c16279a9bc8737902704b782e504351`
- **Modified Files:**
  - `Pika/Services/SeoHelper.cs` (+7 lines): Normalizes `EnglishIndex` action to `Index` in `GetMetadata`.
  - `Pika/Views/Home/Index.cshtml` (+667 / -564 lines): Rebuilt complete 9-chapter product narrative, real screenshots, manifest badges, and bilingual parity.
  - `Pika/wwwroot/css/pika-home.css` (+431 / -326 lines): Removed all dead mock CSS, added responsive `.pw2-home-*` layout styles, preserved navbar styles.
- **Added Files:**
  - `Pika/Pika.Web.Tests/P06HomepageTests.cs` (388 lines): Automated verification suite covering 14 governance invariants.
  - `Pika/docs/marketing/P06_HOMEPAGE_IMPLEMENTATION.md`: Canonical implementation documentation.
