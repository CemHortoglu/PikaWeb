# Pika Web 2.0 Product Visual System (P03)

## 1. Visual Proof Mission

Pika Web 2.0 establishes **Real Product UI as Primary Visual Proof**.

In B2B enterprise software, technical buyers, marketing executives, and data protection officers demand proof of platform depth, operational readiness, and compliance integrity. Generic hand-built CSS dashboards, simulated metric widgets, and abstract stock illustrations undermine credibility. 

Whenever an authentic, safe, and relevant Pika product screenshot exists, future marketing pages must prefer it over hand-crafted mockups. However, raw software interfaces cannot be deployed blindly. Every visual asset must pass the P03 visual safety, quality, privacy (KVKK/GDPR), and metric governance audit before public exposure.

---

## 2. Audit Method

Every asset under `wwwroot/wiki/assets/images/` and `wwwroot/wiki/assets/` underwent a four-layer empirical audit:

1. **Binary Header & Format Inspection:** Direct byte extraction of container signatures (PNG vs JPEG SOF markers), bit depth, color channels, and pixel dimensions. Format mismatches (e.g., JPEG files named with `.png` extension) were formally identified.
2. **Visual Inspection:** High-resolution rendering to inspect application shell elements, visual clutter, active navigation state, and text legibility at target viewport widths.
3. **Privacy & Data Protection Audit:** Rigorous scrutiny for personal names, email addresses, phone numbers, Turkish Republic Identity Numbers (TCKN), real company/tenant titles, and internal environment identifiers.
4. **Metric & Telemetry Provenance Analysis:** Classification of numbers displayed in software tables and KPI cards (real customer outcomes vs. demo/synthetic simulation data vs. natural software counters).

---

## 3. Asset Safety Classification

Every asset is assigned exactly one primary safety status from the standardized vocabulary:

- **`PUBLIC_HERO` (11 Assets):** High-aesthetic, pristine software views suitable for flagship above-the-fold stages, hero sections, and primary capability demonstrations.
- **`PUBLIC_SUPPORTING` (15 Assets):** Safe and authentic secondary proof, configuration panels, drawer inspectors, or blueprint catalogs.
- **`PUBLIC_WITH_CROP` (2 Assets):** High-value software functionality that requires a defined pixel crop to remove extraneous browser chrome or internal directories.
- **`PUBLIC_WITH_REDACTION` (0 Assets):** No assets require redaction; clean crops and anonymized assets eliminate clumsy masking boxes.
- **`INTERNAL_ONLY` (1 Asset):** Sparse, incomplete, or testing-state views unsuitable for marketing.
- **`OBSOLETE` (0 Assets):** All current repository assets reflect the modern Pika application interface.
- **`DUPLICATE` (1 Asset):** Byte-for-byte identical file of an already registered canonical asset.
- **`NEEDS_VISUAL_CONFIRMATION` (0 Assets):** 100% of the 30 assets have been visually audited and confirmed.

*Note on Human Approval:* Administrative policy approval is tracked via the independent flag `humanApprovalRequired: true|false`.

---

## 4. Complete Asset Inventory

The repository contains **30 product visual assets** (29 under `wwwroot/wiki/assets/images/` and 1 diagram under `wwwroot/wiki/assets/`).

| Filename | Declared | Detected | Dims (px) | Aspect | Primary Safety Status | Human Review | PII Status | Metric Status | Primary Mapped Entity |
| :--- | :--- | :--- | :--- | :--- | :--- | :---: | :--- | :--- | :--- |
| `img_ai-musteri-ozeti_8.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Customer Intelligence / Pika 360 |
| `img_aksiyon-calisma-alani_9.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Customer Intelligence (Repeat Purchase) |
| `img_bi-kokpit_5.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | Analytics & Reporting |
| `img_dinamik-siniflandirma-alanlari_12.png` | .png | PNG | 1881 × 915 | 2.06 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Product Intelligence |
| `img_email-store_20.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Content Studio / Email Channel |
| `img_email-template-editor_17.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | NONE | Content Studio / Email Channel |
| `img_excel-csv-aktarimi_1.png` | .png | PNG | 1885 × 974 | 1.94 | **PUBLIC_WITH_CROP** | No | NONE (Cropped) | NONE | Integrations |
| `img_gmail-kisi-aktarimi_23.png` | .png | PNG | 1024 × 503 | 2.04 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Integrations / Consent / Security |
| `img_gonderim-operasyonu-izleme_28.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | Analytics & Reporting / Security |
| `img_gunun-firsatlari.png` | .png | **JPEG** | 1024 × 832 | 1.23 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Günün Fırsatları |
| `img_izin-kanal-zamanlama_25.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Consent Management / Channels |
| `img_journey-karar-kurallari_19.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Journey Manager |
| `img_journey-store_21.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Journey Manager |
| `img_journey-tasarim-tuvali_18.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | NONE | Journey Manager |
| `img_kategori-yonetimi_4.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Product Intelligence |
| `img_kisi-listesi-ve-segmentler_3.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Audience Manager |
| `img_kullanici-roller-yetkiler_24.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_WITH_CROP** | No | NONE (Cropped) | NONE | Security & Privacy (RBAC) |
| `img_master-urun-anlamlandirmalari_15.png` | .png | PNG | 1884 × 928 | 2.03 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Product Intelligence |
| `img_need-group-product-role_11.png` | .png | PNG | 1879 × 930 | 2.02 | **PUBLIC_HERO** | No | NONE | NONE | Product Intelligence |
| `img_pika-360_7.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Pika 360 / Customer Intelligence |
| `img_pika-pilot-ai-kampanya-asistani_16.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | AI Campaign Assistant |
| `img_playbook-sektorel-anlam_10.png` | .png | PNG | 1877 × 933 | 2.01 | **PUBLIC_SUPPORTING** | **Yes** | NONE | NONE | Product Intelligence |
| `img_review-resolution-readiness_14.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Integrations / Customer Intelligence |
| `img_satis-veri-operasyonlari_2.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Integrations |
| `img_segment-sablonlari_22.png` | .png | PNG | 1888 × 932 | 2.03 | **INTERNAL_ONLY** | No | NONE | NONE | *Unsuitable for Public Marketing* |
| `img_segmentasyon-ve-firsatlar_26.png` | .png | **JPEG** | 1024 × 832 | 1.23 | **DUPLICATE** | No | ANONYMIZED | DEMO_SYNTHETIC | *Duplicate of img_gunun-firsatlari* |
| `img_tekrar-satin-alma-analizi_6.png` | .png | PNG | 1774 × 887 | 2.00 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Customer Intelligence / Repeat Purchase |
| `img_urun-siniflandirma-workbench_13.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | NONE | Product Intelligence |
| `img_yayinlama-sablon-ve-yonetim_27.png` | .png | PNG | 1672 × 941 | 1.78 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | Campaign Manager |
| `kisi-aktarimi-gmail-outlook-anonim.png` | .png | PNG | 1600 × 920 | 1.74 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Integrations |

---

## 5. Privacy / PII Findings

### Critical Security Resolution:
> [!IMPORTANT]
> **PII Removal & Sanitization Notice:**
> An earlier source asset contained real personal identifiers and was replaced during P03 with a KVKK-safe anonymized marketing version.
> The canonical file `img_tekrar-satin-alma-analizi_6.png` on disk now exclusively renders synthetic customer identifiers (`Müşteri #A1047`, `Müşteri #B3391`, `Mağaza A`, `Mağaza B`) and contains zero real personal names, zero personal email addresses, zero phone numbers, and zero commercial tenant links.

### Privacy Inspection Results Across Other Assets:
1. **RFC 2606 Reserved Domains:** Files such as `img_ai-musteri-ozeti_8.png`, `img_izin-kanal-zamanlama_25.png`, and `img_review-resolution-readiness_14.png` exclusively use dummy emails on the reserved `@example.com` domain and sequential 555 dummy phones.
2. **In-Software Automated Masking:** `img_gmail-kisi-aktarimi_23.png` demonstrates Pika's real-time privacy engine, visibly showing masked fields (`ayse.y****@gmail.com`, `+90 532 *** ** 45`).
3. **Internal Organization Data:** `img_kullanici-roller-yetkiler_24.png` contains internal user records in its left directory. To eliminate employee exposure, the mandatory crop recipe focuses exclusively on the right-hand **Role-Based Access Control (RBAC) Module Permission Matrix** (`x: 980, y: 215, w: 640, h: 680`).
4. **Browser Chrome Exposure:** `img_excel-csv-aktarimi_1.png` contained desktop browser navigation chrome at the top. The mandatory crop starts at `y: 36`, stripping the browser toolbar and external Google avatar.

---

## 6. Metric / Demo Data Findings

Under `CONTENT_GUARDRAILS.md` and `CLAIMS_REGISTRY.md`, software screenshots must not be conflated with verified empirical customer performance claims.

### Disclosure Requirements:
Whenever a screenshot displaying operational quantities or monetary values is featured prominently on a public marketing view, it must be accompanied by an unambiguous demo indicator:

- **Turkish (TR):** `Örnek Gösterim` or `Temsili Senaryo Verisi`
- **English (EN):** `Illustrative Sample` or `Simulation Data`

### Asset-Specific Metric Classifications:
- `img_bi-kokpit_5.png`: Displays macro cockpit numbers (`₺48.7M ciro`, `128.542 aktif müşteri`). **Must carry demo indicator.**
- `img_gunun-firsatlari.png`: Displays opportunity basket counts (`₺428.650 referans değer`). **Must carry demo indicator.**
- `img_kisi-listesi-ve-segmentler_3.png`: Displays RFM segment values (`₺34.5M VIP ciro`). **Must carry demo indicator.**
- `img_gonderim-operasyonu-izleme_28.png`: Displays high-throughput delivery telemetry (`1.28M gönderildi`, `2.345 / sn`). **Must carry simulation data indicator.**

---

## 7. Canonical Product → Visual Map

To maintain strict truth-in-advertising, slots are populated **only** when an authentic, directly representative screenshot exists. Where no authentic visual exists, the slot explicitly states **`NONE — NO STRONG AUTHENTIC VISUAL`**.

| Canonical Entity | Primary Hero Visual | Primary Stage Visual | Supporting Visual 1 | Supporting Visual 2 |
| :--- | :--- | :--- | :--- | :--- |
| **Pika 360** | `img_pika-360_7.png` | `img_pika-360_7.png` | `img_ai-musteri-ozeti_8.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **Günün Fırsatları** | `img_gunun-firsatlari.png` | `img_gunun-firsatlari.png` | NONE — NO STRONG AUTHENTIC VISUAL | NONE — NO STRONG AUTHENTIC VISUAL |
| **Product Intelligence** | `img_need-group-product-role_11.png` | `img_urun-siniflandirma-workbench_13.png` | `img_dinamik-siniflandirma-alanlari_12.png` | `img_master-urun-anlamlandirmalari_15.png` |
| **Customer Intelligence** | `img_ai-musteri-ozeti_8.png` | `img_tekrar-satin-alma-analizi_6.png` | `img_aksiyon-calisma-alani_9.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **Audience Manager** | `img_kisi-listesi-ve-segmentler_3.png` | `img_kisi-listesi-ve-segmentler_3.png` | NONE — NO STRONG AUTHENTIC VISUAL | NONE — NO STRONG AUTHENTIC VISUAL |
| **Campaign Manager** | `img_yayinlama-sablon-ve-yonetim_27.png` | `img_yayinlama-sablon-ve-yonetim_27.png` | `img_pika-pilot-ai-kampanya-asistani_16.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **Journey Manager** | `img_journey-tasarim-tuvali_18.png` | `img_journey-tasarim-tuvali_18.png` | `img_journey-store_21.png` | `img_journey-karar-kurallari_19.png` |
| **Content Studio** | `img_email-template-editor_17.png` | `img_email-template-editor_17.png` | `img_email-store_20.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **AI Campaign Assistant** | `img_pika-pilot-ai-kampanya-asistani_16.png` | `img_pika-pilot-ai-kampanya-asistani_16.png` | NONE — NO STRONG AUTHENTIC VISUAL | NONE — NO STRONG AUTHENTIC VISUAL |
| **Email Channel** | `img_email-template-editor_17.png` | `img_email-template-editor_17.png` | `img_email-store_20.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **SMS Channel** | NONE — NO STRONG AUTHENTIC VISUAL | NONE — NO STRONG AUTHENTIC VISUAL | `img_izin-kanal-zamanlama_25.png` | `img_gonderim-operasyonu-izleme_28.png` |
| **WhatsApp Channel** | NONE — NO STRONG AUTHENTIC VISUAL | NONE — NO STRONG AUTHENTIC VISUAL | `img_izin-kanal-zamanlama_25.png` | `img_gonderim-operasyonu-izleme_28.png` |
| **Analytics & Reporting** | `img_bi-kokpit_5.png` | `img_bi-kokpit_5.png` | `img_gonderim-operasyonu-izleme_28.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **Consent Management** | `img_izin-kanal-zamanlama_25.png` | `img_izin-kanal-zamanlama_25.png` | `img_gmail-kisi-aktarimi_23.png` | NONE — NO STRONG AUTHENTIC VISUAL |
| **Integrations & Data** | `img_excel-csv-aktarimi_1.png` (Crop) | `img_satis-veri-operasyonlari_2.png` | `img_review-resolution-readiness_14.png` | `kisi-aktarimi-gmail-outlook-anonim.png` |
| **Security & Privacy** | `img_kullanici-roller-yetkiler_24.png` (Crop)| `img_kullanici-roller-yetkiler_24.png` (Crop) | `img_gmail-kisi-aktarimi_23.png` | NONE — NO STRONG AUTHENTIC VISUAL |

---

## 8. Homepage Visual Narrative

The Pika Web 2.0 homepage follows a disciplined, 6-stage value narrative representing the end-to-end customer lifecycle loop. Authentic screenshots anchor each chapter:

1. **Chapter 1: Ingestion & Identity Resolution (Data Foundation)**
   - *Theme:* Unifying offline transactions, spreadsheets, and CRM contacts into an integrated data spine.
   - *Primary Visual:* `img_excel-csv-aktarimi_1.png` (Clean application crop at `y: 36`) or `img_review-resolution-readiness_14.png`.
   - *Mode:* `EDITORIAL_SPLIT`.
2. **Chapter 2: Customer & Product Intelligence (Context Engine)**
   - *Theme:* Dual-engine analytical understanding — customer behavioral value alongside merchandise need groups.
   - *Primary Visual:* `img_pika-360_7.png` (Customer Profile Cockpit) alongside `img_urun-siniflandirma-workbench_13.png` (Product Intelligence Workbench).
   - *Mode:* `FULL_STAGE` with restrained callouts.
3. **Chapter 3: Günün Fırsatları (Commercial Decision Engine)**
   - *Theme:* Daily actionable recommendations (replenishment, cross-sell, churn prevention) prioritized by commercial value.
   - *Primary Visual:* `img_gunun-firsatlari.png`.
   - *Mode:* `ANNOTATED_STAGE` spotlighting opportunity rows and consent badges.
4. **Chapter 4: Action & Workflows (Campaign & Journey Orchestration)**
   - *Theme:* Instant transition from opportunity detection into automated drag-and-drop multi-channel journeys.
   - *Primary Visual:* `img_journey-tasarim-tuvali_18.png` (Visual workflow builder) supported by `img_yayinlama-sablon-ve-yonetim_27.png`.
   - *Mode:* `FULL_STAGE`.
5. **Chapter 5: Channel Execution (Studio & Delivery)**
   - *Theme:* Responsive drag-and-drop email design and regulatory-guarded message dispatch across Email, SMS, and WhatsApp.
   - *Primary Visual:* `img_email-template-editor_17.png`.
   - *Mode:* `EDITORIAL_SPLIT`.
6. **Chapter 6: Measurement & Attribution (Executive BI)**
   - *Theme:* Real-time delivery telemetry, conversion feedback, and executive revenue attribution closing the analytical loop.
   - *Primary Visual:* `img_bi-kokpit_5.png`.
   - *Mode:* `FULL_STAGE` with `Örnek Gösterim` indicator.

---

## 9. Fake UI Replacement Matrix

The following table audits current marketing views for synthetic CSS mockups and establishes their authentic screenshot replacements for future implementation phases:

| View Path | Current Synthetic / Mockup Element | Authentic Replacement Asset | Recommended Mode | Implementation Phase |
| :--- | :--- | :--- | :--- | :--- |
| `Views/Home/Index.cshtml` | Lines 140–220: `.ph-mock` with fake dots, hardcoded stats (12.4K, %68, %24) and CSS bars | `img_gunun-firsatlari.png` and `img_pika-360_7.png` | `FULL_STAGE` + `ANNOTATED_STAGE` | P04 |
| `Views/Platform/Pika360.cshtml` | Lines 59–130: `.ph-module-card` hand-coded profile blocks | `img_pika-360_7.png` | `FULL_STAGE` | Future Platform Phase |
| `Views/Platform/Opportunities.cshtml` | Hand-crafted HTML card list of opportunities | `img_gunun-firsatlari.png` | `FULL_STAGE` | Future Platform Phase |
| `Views/Platform/ProductIntelligence.cshtml` | Hand-coded HTML tables for Need Groups & Product Roles | `img_need-group-product-role_11.png` & `img_urun-siniflandirma-workbench_13.png` | `EDITORIAL_SPLIT` | Future Platform Phase |
| `Views/Platform/CustomerIntelligence.cshtml` | CSS card wrappers simulating customer scoring | `img_ai-musteri-ozeti_8.png` & `img_tekrar-satin-alma-analizi_6.png` | `FULL_STAGE` | Future Platform Phase |
| `Views/Solutions/AudienceManager.cshtml` | Lines 34–56: `.pika-sol-scene-main` floating badges; Lines 93–107: grey CSS mock bars | `img_kisi-listesi-ve-segmentler_3.png` | `FULL_STAGE` | Future Solutions Phase |
| `Views/Solutions/CampaignManager.cshtml` | Line 232: Broken 404 image link `img_kampanya-yonetimi_0.png`; ungrounded "ROAS 14.2x" badge | `img_yayinlama-sablon-ve-yonetim_27.png` | `FULL_STAGE` | Future Solutions Phase |
| `Views/Solutions/JourneyManager.cshtml` | Lines 34–56: `.pika-sol-scene-main` and abstract flowchart icons | `img_journey-tasarim-tuvali_18.png` | `FULL_STAGE` | Future Solutions Phase |
| `Views/Solutions/ContentStudio.cshtml` | Lines 34–56: `.pika-sol-scene-main` and grey CSS mock preview bars | `img_email-template-editor_17.png` | `FULL_STAGE` | Future Solutions Phase |
| `Views/Solutions/Reporting.cshtml` | Lines 34–56: `.pika-sol-scene-main` and CSS dashboard placeholders | `img_bi-kokpit_5.png` | `FULL_STAGE` | Future Solutions Phase |
| `Views/Solutions/ConsentManagement.cshtml` | Lines 34–56: `.pika-sol-scene-main` and abstract shield icons | `img_izin-kanal-zamanlama_25.png` | `FULL_STAGE` | Future Solutions Phase |

---

## 10. Screenshot Presentation Modes

To prevent ad-hoc styling and maintain strict visual consistency, future pages must restrict screenshot presentation to 6 standardized modes:

1. **`FULL_STAGE`:** Full application workspace framed within `.pw2-product-frame`. Used for primary hero and flagship capability demonstrations where application shell depth provides proof of maturity.
2. **`FOCAL_CROP`:** CSS-based non-destructive cropping focusing on a specific table, modal, or workflow canvas, removing sidebars or empty gutters.
3. **`DETAIL_ZOOM`:** High-density spotlight on a granular control group, scoring drawer, or rule builder.
4. **`ANNOTATED_STAGE`:** Full or cropped stage featuring 1 to 3 restrained numbered callout markers (`.pw2-product-callout__marker`) linked to concise technical explanations.
5. **`EDITORIAL_SPLIT`:** A balanced 50/50 or 60/40 two-column layout placing the framed screenshot beside structured editorial copy.
6. **`HORIZONTAL_MOBILE_PAN`:** For complex desktop application screens that cannot be legibly reduced to 320px width, rendered inside `.pw2-product-viewport--pan` with `.pw2-product-scroll-hint`.

---

## 11. Crop Recipe Standard

### Pixel Integrity Rules:
- Original source files must **never** be destructively edited, resized, or overwritten on disk during design specification.
- Crop rectangles reference exact integer pixel coordinates against the uncompressed source image:
  $$	ext{Bounds Check: } 0 le x, quad 0 le y, quad x + 	ext{width} le 	ext{Source Width}, quad y + 	ext{height} le 	ext{Source Height}$$
- Approximate or guessed coordinate values are strictly prohibited.

---

## 12. Priority Crop Recipes

| Asset Filename | Source Dims | Recipe Purpose | Mode | $x$ | $y$ | Width | Height | Mathematical Verification |
| :--- | :--- | :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| `img_excel-csv-aktarimi_1.png` | 1885 × 974 | Clean App Stage (Strip Browser Bar) | `FULL_STAGE` | 0 | 36 | 1885 | 938 | $0+1885 le 1885$, $36+938=974 le 974$ |
| `img_excel-csv-aktarimi_1.png` | 1885 × 974 | Excel Ingestion Modal Focal Crop | `FOCAL_CROP` | 778 | 36 | 1107 | 938 | $778+1107=1885 le 1885$, $36+938=974 le 974$ |
| `img_kullanici-roller-yetkiler_24.png` | 1672 × 941 | RBAC Permission Matrix (Zero User PII) | `FOCAL_CROP` | 980 | 215 | 640 | 680 | $980+640=1620 le 1672$, $215+680=895 le 941$ |
| `img_pika-360_7.png` | 1672 × 941 | Decision Summary & Opportunities | `FOCAL_CROP` | 230 | 80 | 1400 | 820 | $230+1400=1630 le 1672$, $80+820=900 le 941$ |
| `img_pika-360_7.png` | 1672 × 941 | Customer Metric Strip | `DETAIL_ZOOM` | 230 | 415 | 1380 | 260 | $230+1380=1610 le 1672$, $415+260=675 le 941$ |
| `img_gunun-firsatlari.png` | 1024 × 832 | Opportunities Cockpit (No Sidebar) | `FOCAL_CROP` | 135 | 65 | 880 | 720 | $135+880=1015 le 1024$, $65+720=785 le 832$ |
| `img_gunun-firsatlari.png` | 1024 × 832 | Top Actionable Opportunity Rows | `DETAIL_ZOOM` | 140 | 390 | 870 | 260 | $140+870=1010 le 1024$, $390+260=650 le 832$ |
| `img_journey-tasarim-tuvali_18.png` | 1672 × 941 | Workflow Canvas Workspace | `FOCAL_CROP` | 200 | 120 | 1430 | 780 | $200+1430=1630 le 1672$, $120+780=900 le 941$ |
| `img_journey-tasarim-tuvali_18.png` | 1672 × 941 | Decision & Branching Node Detail | `DETAIL_ZOOM` | 440 | 350 | 750 | 420 | $440+750=1190 le 1672$, $350+420=770 le 941$ |
| `img_bi-kokpit_5.png` | 1672 × 941 | BI Dashboard Canvas (No Sidebar) | `FOCAL_CROP` | 230 | 70 | 1410 | 840 | $230+1410=1640 le 1672$, $70+840=910 le 941$ |
| `img_bi-kokpit_5.png` | 1672 × 941 | Executive KPI Header Strip | `DETAIL_ZOOM` | 230 | 140 | 1400 | 160 | $230+1400=1630 le 1672$, $140+160=300 le 941$ |
| `img_tekrar-satin-alma-analizi_6.png` | 1774 × 887 | Timing Distribution & Potential Customers | `FOCAL_CROP` | 200 | 90 | 1540 | 760 | $200+1540=1740 le 1774$, $90+760=850 le 887$ |
| `img_urun-siniflandirma-workbench_13.png`| 1672 × 941 | Catalog Workbench (No Sidebar) | `FOCAL_CROP` | 230 | 80 | 1410 | 820 | $230+1410=1640 le 1672$, $80+820=900 le 941$ |
| `img_urun-siniflandirma-workbench_13.png`| 1672 × 941 | AI Recommendation Drawer (Cappuccino) | `DETAIL_ZOOM` | 1040 | 280 | 590 | 580 | $1040+590=1630 le 1672$, $280+580=860 le 941$ |
| `img_email-template-editor_17.png` | 1672 × 941 | Email Canvas & Block Drawer | `FOCAL_CROP` | 230 | 70 | 1410 | 840 | $230+1410=1640 le 1672$, $70+840=910 le 941$ |
| `img_yayinlama-sablon-ve-yonetim_27.png` | 1672 × 941 | Operations Management Grid | `FOCAL_CROP` | 230 | 80 | 1410 | 820 | $230+1410=1640 le 1672$, $80+820=900 le 941$ |

---

## 13. Annotation System

Annotations must strictly follow the P02 design principles: **restrained, informative, accessible, and non-intrusive**.

### Annotation Rules:
- **Maximum Density:** Default maximum of **3 callouts** per screenshot.
- **Marker Design:** Solid 24px circular indicator (`.pw2-product-callout__marker`) in dark petrol (`#0c3a30`) with pure white bold monospace text and a 2px crisp white border.
- **Prohibited Effects:** Zero pulsating animations, zero radar blips, zero neon green halos, zero fluorescent arrows, and zero hand-drawn doodles.
- **Label Placement:** Concise label chips (`.pw2-product-callout__label`) rendered with `--pw2-bg-primary`, subtle border, and high-contrast typography.
- **Mobile Fallback:** On viewport widths $< 768px$, floating callout markers may convert into an ordered technical legend directly below the image.

---

## 14. Product Chrome Rules

- **Preserve Real Application Shell:** When a screenshot includes Pika's real sidebar navigation and breadcrumbs (e.g., `img_pika-360_7.png`, `img_bi-kokpit_5.png`), preserve it where viewport width permits. Real navigation proves functional breadth and SaaS maturity.
- **Simulated Browser Chrome:** The subtle top bar (`.pw2-product-frame__header` with 3 monochrome dots) should be used when:
  1. An asset has undergone a focal crop that removes native window edges.
  2. Standalone canvas editors (such as `img_journey-tasarim-tuvali_18.png`) benefit from window framing.
- **No Double Chrome:** Never place simulated browser chrome over a screenshot that already visibly contains desktop browser navigation (as identified and resolved in `img_excel-csv-aktarimi_1.png`).

---

## 15. Mobile Behaviour

Complex desktop enterprise software interfaces must not simply be shrunk to unreadable 320px images. Each asset must follow an intentional mobile strategy:

1. **`CROP_TO_FOCUS`:** On viewports $< 768px$, dynamically apply a focal crop focusing on the critical table or drawer (e.g., showing the right drawer of `img_urun-siniflandirma-workbench_13.png`).
2. **`HORIZONTAL_PAN`:** For data tables and wide journey canvases (`img_journey-tasarim-tuvali_18.png`, `img_gunun-firsatlari.png`), place the asset inside `.pw2-product-viewport--pan` to allow fluid, momentum-based horizontal exploration while preserving full legibility. The container displays `.pw2-product-scroll-hint` ("Kaydırarak inceleyin ↔") on mobile devices.
3. **`STACKED_DETAIL_CROPS`:** Breaking a dense multi-column cockpit into two sequential detail crops stacked vertically.
4. **`STATIC_FULL_IMAGE_IF_LEGIBLE`:** Applied only to clean, high-contrast assets that maintain legible text at reduced widths (e.g., `kisi-aktarimi-gmail-outlook-anonim.png`).

---

## 16. Assets Not Approved for Public Use

| Filename | Status | Rationale & Remediation |
| :--- | :--- | :--- |
| `img_segment-sablonlari_22.png` | **INTERNAL_ONLY** | Unfinished form state showing an active validation error (`Mağaza zorunludur`) and large empty whitespace. Does not reflect Pika's product quality. Use `img_kisi-listesi-ve-segmentler_3.png` instead. |
| `img_segmentasyon-ve-firsatlar_26.png` | **DUPLICATE** | Byte-for-byte exact duplicate of `img_gunun-firsatlari.png` (MD5 `c7de15c47f1a3a03e24aeade000c667e`). Excluded to prevent redundant asset indexing. |

---

## 17. Assets Requiring Human Confirmation

| Filename | Governance Review Item | Recommendation |
| :--- | :--- | :--- |
| `img_playbook-sektorel-anlam_10.png` | Header displays **"PI DEFINITION LAYER · SUPER ADMIN"** and badges **"Super Admin Only" / "Code Immutable"**. | Safe for technical/architecture documentation, but if used on general marketing pages, apply `FOCAL_CROP` (`y: 450`) to showcase the taxonomy table without super-admin administrative headers. |

---

## 18. P04–P25 Visual Usage Contract

Every subsequent marketing page implementation phase must comply with these binding rules:

1. **Zero Fake UI:** Hand-crafted HTML/CSS mockups, fake stats, and grey wireframe bars must be replaced with approved authentic screenshots according to the **Fake UI Replacement Matrix** (Section 9).
2. **Canonical Mapping Fidelity:** Pages must utilize the designated primary hero and stage assets assigned in the **Canonical Product $ightarrow$ Visual Map** (Section 7). Do not invent ad-hoc asset assignments.
3. **Mandatory Metric Disclosures:** Any displayed screenshot containing aggregate numbers or currency values must include the `Örnek Gösterim` / `Illustrative Sample` disclosure.
4. **Coordinate-Exact CSS Crops:** Cropped implementations must strictly use the verified coordinates documented in **Priority Crop Recipes** (Section 12) via `.pw2-product-crop`.
5. **Restrained Annotations:** Annotations are capped at 3 markers per screen, strictly using `.pw2-product-callout` primitives with zero animations or glows.
6. **Mobile Adaptability:** All screenshots must implement an approved mobile behavior (Section 15); unformatted desktop images overflowing or shrinking to illegibility on mobile are strictly prohibited.
