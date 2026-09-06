# P00 Executive Findings: Canonical Product Truth, Claims & Governance Audit

**Project:** Pika Web 2.0  
**Phase:** P00 — Canonical Product Truth, Claims & Content Governance Audit  
**Target Repository:** `CemHortoglu/PikaWeb`  
**Audit Date:** September 2026  
**Audited Surface:** 10 Controllers, 59 Razor Views, 88 Public Wiki Articles, 15 Internal Engineering Wiki Articles, Services (`SeoHelper`, `LegacyRouteMapper`, `WikiService`), `Program.cs`, Configuration files, `sitemap.xml`, `robots.txt`, `llms.txt`, `llms-full.txt`, and 29 product visual assets.

---

## Executive Summary

This audit establishes an unyielding, evidence-backed foundation of canonical truth for Pika before any visual redesign, copywriting, or architectural restructuring begins.

The repository contains an authentic, sophisticated, and deeply engineered platform: **Pika is a B2B SaaS Customer Intelligence & Omnichannel Marketing Platform ("Müşteri Zekâsı ve Omnichannel Pazarlama Platformu")**. Its core technological differentiation lies in extracting commercial meaning from transaction and catalog data (Customer Context Engine, Product Intelligence, Customer Value Scoring, and Günün Fırsatları) before activating multi-channel campaigns across verified, compliant channels (native Email, native SMS, and official WhatsApp Business API).

However, the public-facing marketing presentation suffers from severe governance debt:
1. **Broken Live Assets:** A prominent 404 broken image on a flagship solution page.
2. **Channel Contradictions:** Push Notifications are advertised as an active, native live channel across multiple marketing pages and SEO tags, despite being explicitly marked as a **Roadmap** feature in architecture files and completely absent from backend delivery workers.
3. **Unverified & Exaggerated Metrics:** Claims like **"14.2x ROAS"**, **"%52 Açılma Oranı"**, **"%31 Ziyaret Artışı"**, and **"%41 Satış Dönüşümü"** are hardcoded into production views without sample/demo labeling, violating non-negotiable metric safety.
4. **Severe Route-Content Inversions:** Three solution routes deliver entirely different subjects than their URLs promise (`/cozumler/personalization` delivers Journey Orchestration; `/cozumler/template-management` delivers Audience Segmentation; `/cozumler/ab-testing` delivers Campaign Automation).
5. **Self-Cannibalizing Duplicate Landing Pages:** Four parallel duplicate solution routes dilute search ranking and confuse users.
6. **Visual Product-Proof Gap:** The repository houses **29 authentic, high-quality product screenshots** from `https://app.pika.tr`, yet **28 of them remain hidden in wiki folders** while marketing pages show hand-built CSS mockups.
7. **Dangerous Orphan Views:** An unrouted legacy `Pricing.cshtml` view on disk contains hardcoded package prices (`₺9.900/ay`, `₺24.900/ay`) and enterprise SLAs.

---

## Critical P0 Findings (Immediate Remediation Blockers)

1. **P0-1: Live HTTP 404 Broken Image on Flagship Solution Page**
   - *Location:* `Views/Solutions/CampaignManager.cshtml:232`
   - *Evidence:* `<img src="/wiki/assets/images/img_kampanya-yonetimi_0.png" ... />`
   - *Reality:* File does not exist on disk. Renders a broken image icon to prospective buyers.
   - *Impact:* Undermines enterprise product credibility. Must be replaced with existing authentic screenshot `img_yayinlama-sablon-ve-yonetim_27.png`.

2. **P0-2: Contradiction on Push Notification Availability**
   - *Locations:* `SeoHelper.cs:117, 194-198`, `_Layout.cshtml:347`, `SolutionsController.cs:40-41`, `llms-full.txt:11`
   - *Evidence:* Advertised as an active, native delivery channel ("SMS, WhatsApp, Email ve Push kampanyalarını tek merkezden oluşturun").
   - *Reality:* Explicitly marked `<h3>Push Notifications (Roadmap)</h3>` in `Views/Home/Solutions.cshtml:39`. Completely absent from backend delivery workers in `internal_wiki.json`. No APNs/FCM gateway or worker queue exists.
   - *Impact:* Prospects evaluating multi-channel platforms will feel misled if they contract Pika expecting live push notifications.

3. **P0-3: Hardcoded Unverified Metrics Violating Metric Safety**
   - *Locations:* `Views/Solutions/CampaignManager.cshtml:133, 397, 407, 417`
   - *Claims:* `ROAS 14.2x`, `₺184.600 Sipariş Tutarı`, `%52 Açılma Oranı`, `%31 Ziyaret Artışı`, `%41 Satış Dönüşümü`.
   - *Reality:* Hardcoded directly into HTML cards with no source attribution or demo label.
   - *Impact:* Exposes Pika to legal, commercial, and reputation risk if interpreted as verified customer case results or performance guarantees.

4. **P0-4: Fabricated Enterprise Claims in `llms-full.txt`**
   - *Location:* `wwwroot/llms-full.txt:19, 66, 121`
   - *Claims:* "SOC-compliant infrastructure", "SSO/SAML integration", "chatbot flows, and two-way conversations with customers".
   - *Reality:* No SOC-2 audit report or attestation exists; authentication in `Program.cs` is strictly cookie/JWT token-based without SAML; WhatsApp integration is strictly outbound template notifications, not two-way conversational AI chatbots.
   - *Impact:* LLMs and search engines ingest false architectural capabilities, creating RFP disqualifications.

5. **P0-5: Dangerous Orphan Pricing View with Outdated Prices**
   - *Location:* `Views/Home/Pricing.cshtml`
   - *Content:* Hardcoded package cards: Start (`₺9.900/ay`), Growth (`₺24.900/ay`), Enterprise (`Özel`), and SLA promises.
   - *Reality:* Currently has no action in `HomeController.cs`, but resides in active views directory. If accidentally mapped or exposed, it locks sales into outdated or unauthorized price points.

---

## High-Priority P1 Findings

1. **P1-1: High-Severity Route/Content Mismatches**
   - `/cozumler/personalization` -> Displays *"Müşteri Etkileşim Yönetimi / Journey Orchestration"* (competes with `/cozumler/journey-manager`).
   - `/cozumler/template-management` -> Displays *"Müşteri Segmentasyonu ve Hedefleme / Audience Segmentation & Targeting"* (competes with `/cozumler/audience-manager`).
   - `/cozumler/ab-testing` -> Displays *"Kampanya Otomasyonu ve Zamanlama / Campaign Automation & Scheduling"* (competes with `/cozumler/campaign-manager`).

2. **P1-2: 4 Competing / Duplicate Solution Routes**
   - `/kanallar/whatsapp-kampanya-yonetimi` cannibalizes canonical `/kanallar/whatsapp`.
   - `/kanallar/email-marketing-template-studio` cannibalizes canonical `/kanallar/email` and `/cozumler/content-studio`.
   - `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` cannibalizes canonical `/cozumler/consent-management`.
   - `/cozumler/e-ticaret-ai-kampanya-yonetimi` cannibalizes canonical `/urunler/ai-kampanya-asistani`.

3. **P1-3: Sitemap Discrepancy Between TR and EN**
   - In `wwwroot/sitemap.xml`, 6 thin stub pages were commented out in Turkish with the note: *"Thin stub pages removed: personalization, template-management, ab-testing, deliverability-compliance, data-management-etl, real-time-event-processing. Add back when content is substantially improved."*
   - However, in lines 276-309, all 6 pages were **left active in the English sitemap section**, causing search engines to index thin, mismatched English pages.

4. **P1-4: Physical Headquarters Location Discrepancy**
   - `appsettings.json:19` lists the physical office in **Ankara** (`Teknopark Turkuaz Bina, Ostim Osb Mah. Yenimahalle / Ankara`).
   - `Views/Shared/_Layout.cshtml:114` JSON-LD schema hardcodes: `"addressLocality": "İstanbul"`.

5. **P1-5: Developer Meta-Commentary Left in Public View**
   - `Views/Solutions/DeliverabilityCompliance.cshtml:9` contains informal developer internal notes: *"Burası pazarlama metninde genelde “sıkıcı” gibi görünür ama satışta en büyük güven artırıcılardan biridir... bu başlığı ayrı bir çözüm olarak sunmak kurumsal duruşu güçlendirir."*

---

## Contradictory Product Claims

| Topic | Representation A | Representation B | Verdict / Canonical Truth |
| :--- | :--- | :--- | :--- |
| **Push Channel** | "SMS, WhatsApp, Email ve Push kampanyalarını tek merkezden yönetin" (`SeoHelper.cs:117`, `_Layout.cshtml`) | "Push Notifications (Roadmap)" (`Views/Home/Solutions.cshtml:39`) | **Push is ROADMAP.** It has no gateway or delivery worker in backend. Must be marked `(Roadmap)` everywhere. |
| **Pika Positioning** | "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu" (`SeoHelper.cs:25`, `llms.txt:1`) | "Kullanıcı sadakat ve kampanya otomasyon platformu" (`appsettings.json:11`) / "Omnichannel Pazarlama Otomasyonu ve Müşteri Yolculuğu" (`Index.cshtml:7`) | **"Müşteri Zekâsı ve Omnichannel Pazarlama Platformu"** is the approved canonical positioning. Update legacy configs. |
| **AI Role** | "Pika AI taslak, hedef kitle önerisi ve email şablonunu saniyeler içinde oluştursun" (`SeoHelper.cs:299`) | "Kesinlikle hayır. Pika'da yapay zekâ yalnızca yardımcıdır. Tüm onay insandadır." (`Faq.cshtml:325`) | AI is strictly an **assistive co-pilot (Pika Pilot)**. It never sends messages autonomously. |
| **Address** | Teknopark Turkuaz Bina, Ostim / Ankara (`appsettings.json:19`) | İstanbul (`_Layout.cshtml:114` JSON-LD) | **Requires Human Confirmation** to align company registration. |
| **WhatsApp Scope**| "Chatbot flows and two-way conversations" (`llms-full.txt:66`) | "Onaylı şablonlar, bildirim ve kampanya gönderimleri" (`wiki.json`, `internal_wiki.json`) | Pika supports **outbound template dispatches** via WhatsApp Business API; not conversational AI chatbots. |

---

## Unsupported Claims

| Claim Text | File Location | Nature of Risk | Remediation Rule |
| :--- | :--- | :--- | :--- |
| `ROAS 14.2x` | `Views/Solutions/CampaignManager.cshtml:133` | Unsubstantiated ROI multiplier | Add `ÖRNEK SENARYO` badge or remove. |
| `%52 Açılma Oranı` | `Views/Solutions/CampaignManager.cshtml:397` | Unverified performance metric | Add `Temsili Örnek` label or remove badge. |
| `%31 Ziyaret Artışı` | `Views/Solutions/CampaignManager.cshtml:407` | Unverified outcome uplift | Add `Temsili Örnek` label or remove badge. |
| `%41 Satış Dönüşümü` | `Views/Solutions/CampaignManager.cshtml:417` | Unverified conversion percentage | Add `Temsili Örnek` label or remove badge. |
| `₺184.600 Sipariş Tutarı` | `Views/Solutions/CampaignManager.cshtml:131` | Synthetic revenue presented as live | Frame clearly as simulation data. |
| `12.4K Gönderim, %68 Açılma`| `Views/Home/Index.cshtml:160, 167` | Synthetic hero cockpit stats | Keep within mock UI; add footnote. |
| `SOC-compliant infrastructure`| `wwwroot/llms-full.txt:19` | Unsupported audit certification | Delete immediately from LLM files. |
| `SSO / SAML integration` | `wwwroot/llms-full.txt:121` | Nonexistent enterprise auth feature | Mark as Roadmap or remove. |
| `Milisaniyeler içinde aksiyon`| `SeoHelper.cs:268`, `RealTimeEventProcessing.cshtml:9` | Exaggerated technical latency | Rephrase to "saniyeler içinde olay bazlı tetikleme". |
| `Uluslararası müşteriler` | `Views/Home/Career.cshtml:81` | Unsubstantiated global proof | Remove or qualify until verified. |

---

## Route / Content Mismatches

```
[Requested URL: /cozumler/personalization]
   │
   ├── Renders: Views/Solutions/Personalization.cshtml
   ├── Title: "Müşteri Etkileşim Yönetimi / Journey Orchestration"
   └── Actual Content: Event triggers, wait steps, and cart journeys (MISMATCH: Belongs to Journey Manager)

[Requested URL: /cozumler/template-management]
   │
   ├── Renders: Views/Solutions/TemplateManagement.cshtml
   ├── Title: "Müşteri Segmentasyonu ve Hedefleme / Audience Segmentation & Targeting"
   └── Actual Content: Rule trees, AND/OR logic, cohort parameters (MISMATCH: Belongs to Audience Manager)

[Requested URL: /cozumler/ab-testing]
   │
   ├── Renders: Views/Solutions/ABTesting.cshtml
   ├── Title: "Kampanya Otomasyonu ve Zamanlama / Campaign Automation & Scheduling"
   └── Actual Content: Calendar windows, targeting rules, delivery queues (MISMATCH: Belongs to Campaign Manager)
```

---

## SEO / LLM Entity Inconsistencies

Search engines and LLM crawlers are currently receiving fragmented entity descriptions:

1. **`SeoHelper.cs` Title:** `"Müşteri Zekâsı ve Omnichannel Pazarlama Platformu"`
2. **`Views/Home/Index.cshtml` Title:** `"Omnichannel Pazarlama Otomasyonu ve Müşteri Yolculuğu Platformu"`
3. **`appsettings.json` Meta Description:** `"Pika — Kullanıcı sadakat ve kampanya otomasyon platformu"`
4. **`_Layout.cshtml` JSON-LD Description:** `"Türkiye'de omnichannel pazarlama otomasyonu, müşteri yolculuğu yönetimi ve AI destekli kampanya çözümleri sunan B2B SaaS platformu"`
5. **`wwwroot/llms.txt`:** `"Müşteri Zekâsı ve Omnichannel Pazarlama Platformu"` (Correct, canonical)
6. **`wwwroot/llms-full.txt`:** `"Pika - Omnichannel Marketing Automation Platform"` (Outdated entity definition, broken URLs)

**Remediation Target:** In Phase P01/P02, standardize all layers to:
`PIKA: Müşteri Zekâsı ve Omnichannel Pazarlama Platformu`

---

## Visual Product-Proof Gaps

The repository possesses extensive authentic visual proof that is currently wasted:

| Solution / Platform Area | Current Marketing Page Visual | Available Real Authentic Screenshot in Repo | Credibility Impact of Switching |
| :--- | :--- | :--- | :--- |
| **Günün Fırsatları** | Generic CSS text lists | `wwwroot/wiki/assets/images/img_gunun-firsatlari.png` | **MASSIVE**: Proves live algorithmic opportunity discovery cockpit. |
| **Pika 360** | Hand-built fake persona card | `wwwroot/wiki/assets/images/img_pika-360_7.png` | **MASSIVE**: Displays real unified customer timeline, CVS, and risks. |
| **Product Intelligence** | Stylized HTML icon cards | `wwwroot/wiki/assets/images/img_need-group-product-role_11.png` | **HIGH**: Demonstrates real Need Group & Product Role classification. |
| **Analytics & BI** | Hand-built CSS metric boxes | `wwwroot/wiki/assets/images/img_bi-kokpit_5.png` | **MASSIVE**: Proves genuine BI store metrics and turnover analytics. |
| **Email Studio** | CSS block preview | `wwwroot/wiki/assets/images/img_email-template-editor_17.png` | **HIGH**: Demonstrates real drag-and-drop responsive template builder. |
| **Journey Manager** | Stylized CSS flowchart | `wwwroot/wiki/assets/images/img_journey-tasarim-tuvali_18.png` | **MASSIVE**: Proves authentic visual workflow state-machine. |
| **Audience Manager** | Floating badges | `wwwroot/wiki/assets/images/img_kisi-listesi-ve-segmentler_3.png` | **HIGH**: Shows actual cohort list and dynamic segmentation engine. |
| **Campaign Manager** | Broken image (`img_kampanya-yonetimi_0.png`) | `wwwroot/wiki/assets/images/img_yayinlama-sablon-ve-yonetim_27.png` | **CRITICAL**: Eliminates 404 error and displays live dispatch canvas. |

---

## Legacy Risks

1. **`Views/Home/Pricing.cshtml`:** Contains legacy prices (`₺9.900`, `₺24.900`). Must be archived and kept strictly unrouted.
2. **`Views/Home/Solutions.cshtml`:** Unrouted legacy hub containing broken cross-links. Must be archived or deleted.
3. **`Views/Solutions/JourneyManager.cshtml.bak`:** Leftover editor backup file. Must be deleted.
4. **`wwwroot/web/images/partner/partner-logo-1.png` to `11.png`:** Generic theme placeholder logos. Must never be exposed as fake client proof.
5. **`wwwroot/images/1.png` to `5.png`:** 5 orphan image files occupying storage with zero references.

---

## Product Questions Requiring Human Confirmation

The following questions cannot be resolved from code inspection and require authoritative human confirmation before marketing copy is authored in subsequent phases:

1. **Push Notifications:**
   - Push is documented as a Roadmap capability in code, but marketed as live across several views. *Is Push currently live with any third-party gateway, or should all public marketing strictly qualify Push as "(Roadmap)"?*
2. **Provenance of Metrics in Campaign Manager:**
   - In `CampaignManager.cshtml`, `ROAS 14.2x`, `₺184.600 Sipariş Tutarı`, `%52 Açılma Oranı`, `%31 Ziyaret Artışı`, and `%41 Satış Dönüşümü` are displayed. *Are these metrics from a real historical client campaign (if so, which brand/vertical?), or are they illustrative synthetic examples that must be labeled `ÖRNEK SENARYO`?*
3. **Company Headquarters Address:**
   - `appsettings.json` specifies Teknopark Ostim / Ankara, while `_Layout.cshtml` JSON-LD schema specifies İstanbul. *Which city is the legal and authoritative headquarters for Schema.org and contact records?*
4. **Pricing Policy:**
   - `Pricing.cshtml` contains outdated packages (`₺9.900/ay`, `₺24.900/ay`). *Is Pika pursuing a custom-quote enterprise sales motion ("Demo Talebi / İletişim"), or will standardized public pricing packages be introduced?*
5. **Customer & Brand Proof:**
   - Does Pika have verified client logos, case studies, or testimonial quotes approved for public release on the website?

---

## Recommended Inputs for P01

When transitioning to **Phase P01 (Information Architecture & Content Strategy)**, the following remediation inputs must be executed:

1. **Fix Broken Image (P0):**
   - Point `Views/Solutions/CampaignManager.cshtml:232` to existing screenshot `img_yayinlama-sablon-ve-yonetim_27.png`.
2. **Implement 301 Redirect Plan for Mismatched & Duplicate Routes:**
   - Redirect `/cozumler/personalization` -> `/cozumler/journey-manager`
   - Redirect `/cozumler/template-management` -> `/cozumler/audience-manager`
   - Redirect `/cozumler/ab-testing` -> `/cozumler/campaign-manager`
   - Redirect `/kanallar/whatsapp-kampanya-yonetimi` -> `/kanallar/whatsapp`
   - Redirect `/kanallar/email-marketing-template-studio` -> `/kanallar/email`
   - Redirect `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` -> `/cozumler/consent-management`
   - Redirect `/cozumler/e-ticaret-ai-kampanya-yonetimi` -> `/urunler/ai-kampanya-asistani`
   - Redirect `/cozumler/deliverability-compliance` -> `/cozumler/consent-management`
   - Redirect `/cozumler/data-management-etl` -> `/entegrasyonlar`
   - Redirect `/cozumler/real-time-event-processing` -> `/cozumler/journey-manager`
3. **Harmonize Sitemap (`sitemap.xml`):**
   - Clean up the English section of `sitemap.xml` to match the Turkish section by removing the 6 thin stub URLs.
4. **Push Notification Qualification:**
   - Add explicit `(Roadmap)` badge to all Push navigation items and landing page titles.
5. **Replace Hand-Built CSS Mockups with Real UI Screenshots:**
   - Deploy the 28 authentic screenshots catalogued in `VISUAL_ASSET_REGISTRY.md` into their corresponding landing pages.
6. **Rewrite Outdated `llms-full.txt`:**
   - Align `llms-full.txt` with `PRODUCT_TRUTH.md`, updating all canonical routes and stripping unverified SOC/SAML/Chatbot claims.
