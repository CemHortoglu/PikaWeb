# Visual Asset & Product Screenshot Registry

This document inventories all real product UI screenshots, diagrams, and marketing graphics within the repository. It establishes which real assets can replace hand-built CSS mockups on public marketing pages, identifies technical format mismatches, enforces privacy (KVKK) safety, and governs screenshot deployment.

---

## The Core Visual Principle for Pika Web 2.0

> [!IMPORTANT]
> **If a real, current Pika product screenshot exists, future marketing implementation should normally prefer it over a hand-built fake CSS mockup.**
> 
> Currently, the repository contains **30 authentic product visual assets** (29 screenshots from the live Pika application under `wwwroot/wiki/assets/images/` and 1 integration diagram under `wwwroot/wiki/assets/`).
> 
> Under Phase P03, every asset has undergone an exhaustive visual, privacy, and metric governance audit:
> - **Public-Capable:** **28 assets** (11 `PUBLIC_HERO`, 15 `PUBLIC_SUPPORTING`, 2 `PUBLIC_WITH_CROP`).
> - **Immediately Approved for Public Deployment:** **27 assets** (all public-capable assets where `humanApprovalRequired` is `false`).
> - **Pending Human Confirmation:** **1 asset** (`img_playbook-sektorel-anlam_10.png` carries a Super Admin banner and requires human confirmation before marketing page deployment).
> - **Non-Public / Excluded:** **2 assets** (1 `INTERNAL_ONLY`, 1 `DUPLICATE`).

---

## Security & Privacy Resolution (P03 KVKK Correction)

> [!NOTE]
> **Security Notice:**
> An earlier source asset contained real personal identifiers and was replaced during P03 with a KVKK-safe anonymized marketing version.
> The canonical file `img_tekrar-satin-alma-analizi_6.png` now exclusively renders synthetic customer identifiers (`Müşteri #A1047`, `Müşteri #B3391`, `Mağaza A`, `Mağaza B`) and contains zero real personal data.

> [!CAUTION]
> **Git-History PII Security Follow-up:**
> - The current version of the affected Repeat Purchase visual asset (`img_tekrar-satin-alma-analizi_6.png`) is sanitized and contains synthetic demo identifiers.
> - Historical Git commits prior to P03 may still contain the superseded raw binary.
> - If this repository or its commit history has been or will be made publicly accessible, a formal Git-history sanitization pass (e.g., `git-filter-repo` or BFG) must be evaluated separately.
> - A historical Git rewrite is intentionally OUT OF SCOPE for P03.2 and must be coordinated with repository maintainers.

---

## Broken Image Incident Status

- **Status:** OPEN — APPROVED REPLACEMENT IDENTIFIED
- **Referencing File:** `Views/Solutions/CampaignManager.cshtml:232`
- **Broken Path:** `<img src="/wiki/assets/images/img_kampanya-yonetimi_0.png" ... />`
- **File System Reality:** `img_kampanya-yonetimi_0.png` **DOES NOT EXIST** on disk. It currently produces a live HTTP 404 error on the public Campaign Manager solution page.
- **Approved Future Replacement:** **`img_yayinlama-sablon-ve-yonetim_27.png`** (Campaign Operations Center & Publishing Interface).
- **Implementation Note:** The actual View replacement will occur during the Campaign Manager page implementation phase.

---

## Real Product UI Screenshot Inventory

| Filename | Declared / Detected | Dimensions | Safety Classification | Human Review | PII Status | Metric Status | Primary Mapped Entity | Recommended Presentation Mode |
| :--- | :--- | :--- | :--- | :---: | :--- | :--- | :--- | :--- |
| `img_ai-musteri-ozeti_8.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Customer Intelligence / Pika 360 | `FOCAL_CROP` / `EDITORIAL_SPLIT` |
| `img_aksiyon-calisma-alani_9.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Customer Intelligence (Repeat Purchase) | `FOCAL_CROP` / `EDITORIAL_SPLIT` |
| `img_bi-kokpit_5.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | Analytics & Reporting | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_dinamik-siniflandirma-alanlari_12.png` | .png / PNG | 1881 × 915 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Product Intelligence | `FULL_STAGE` / `EDITORIAL_SPLIT` |
| `img_email-store_20.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Content Studio / Email Channel | `FULL_STAGE` / `FOCAL_CROP` |
| `img_email-template-editor_17.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | NONE | NONE | Content Studio / Email Channel | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_excel-csv-aktarimi_1.png` | .png / PNG | 1885 × 974 | **PUBLIC_WITH_CROP** | No | NONE (Cropped) | NONE | Integrations | `FULL_STAGE` (Crop y:36) / `FOCAL_CROP` |
| `img_gmail-kisi-aktarimi_23.png` | .png / PNG | 1024 × 503 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Integrations / Consent Management | `FULL_STAGE` / `EDITORIAL_SPLIT` |
| `img_gonderim-operasyonu-izleme_28.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Analytics & Reporting | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_gunun-firsatlari.png` | .png / **JPEG** | 1024 × 832 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Günün Fırsatları | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_izin-kanal-zamanlama_25.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Consent Management / Channels | `FULL_STAGE` / `DETAIL_ZOOM` |
| `img_journey-karar-kurallari_19.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Journey Manager | `FOCAL_CROP` / `DETAIL_ZOOM` |
| `img_journey-store_21.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Journey Manager | `FULL_STAGE` / `FOCAL_CROP` |
| `img_journey-tasarim-tuvali_18.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | NONE | NONE | Journey Manager | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_kategori-yonetimi_4.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Product Intelligence | `FULL_STAGE` / `FOCAL_CROP` |
| `img_kisi-listesi-ve-segmentler_3.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Audience Manager | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_kullanici-roller-yetkiler_24.png` | .png / PNG | 1672 × 941 | **PUBLIC_WITH_CROP** | No | NONE (Cropped) | NONE | Security & Privacy (RBAC) | `FOCAL_CROP` (Crop x:980, y:215) |
| `img_master-urun-anlamlandirmalari_15.png` | .png / PNG | 1884 × 928 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Product Intelligence | `FULL_STAGE` / `FOCAL_CROP` |
| `img_need-group-product-role_11.png` | .png / PNG | 1879 × 930 | **PUBLIC_HERO** | No | NONE | NONE | Product Intelligence | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_pika-360_7.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | ANONYMIZED | DEMO_SYNTHETIC | Pika 360 / Customer Intelligence | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_pika-pilot-ai-kampanya-asistani_16.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | AI Campaign Assistant | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_playbook-sektorel-anlam_10.png` | .png / PNG | 1877 × 933 | **PUBLIC_SUPPORTING** | **Yes** | NONE | NONE | Product Intelligence | `FOCAL_CROP` (Crop y:450) |
| `img_review-resolution-readiness_14.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Integrations / Customer Intelligence | `FULL_STAGE` / `DETAIL_ZOOM` |
| `img_satis-veri-operasyonlari_2.png` | .png / PNG | 1672 × 941 | **PUBLIC_SUPPORTING** | No | NONE | DEMO_SYNTHETIC | Integrations | `FULL_STAGE` / `EDITORIAL_SPLIT` |
| `img_segment-sablonlari_22.png` | .png / PNG | 1888 × 932 | **INTERNAL_ONLY** | No | NONE | NONE | *Unsuitable for Public Marketing* | *DO_NOT_DEPLOY* |
| `img_segmentasyon-ve-firsatlar_26.png` | .png / **JPEG** | 1024 × 832 | **DUPLICATE** | No | ANONYMIZED | DEMO_SYNTHETIC | *Duplicate of img_gunun-firsatlari* | *USE_CANONICAL_ASSET* |
| `img_tekrar-satin-alma-analizi_6.png` | .png / PNG | 1774 × 887 | **PUBLIC_SUPPORTING** | No | ANONYMIZED | DEMO_SYNTHETIC | Customer Intelligence (Repeat Purchase) | `FULL_STAGE` / `FOCAL_CROP` |
| `img_urun-siniflandirma-workbench_13.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | NONE | NONE | Product Intelligence | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `img_yayinlama-sablon-ve-yonetim_27.png` | .png / PNG | 1672 × 941 | **PUBLIC_HERO** | No | NONE | DEMO_SYNTHETIC | Campaign Manager | `FULL_STAGE` / `ANNOTATED_STAGE` |
| `kisi-aktarimi-gmail-outlook-anonim.png` | .png / PNG | 1600 × 920 | **PUBLIC_SUPPORTING** | No | NONE | NONE | Integrations | `FULL_STAGE` / `EDITORIAL_SPLIT` |

---

## Marketing Views Violating the Real-Screenshot Principle

| Page / Route | Current Hand-Built Mockup Description | Approved Authentic Product Screenshot Replacement |
| :--- | :--- | :--- |
| **Homepage (`/`)** | Hand-crafted HTML/CSS cockpit card (`.ph-mock`) with synthetic stats (12.4K, %68, %24). | Replace with high-impact hero stage of **`img_gunun-firsatlari.png`** and **`img_pika-360_7.png`**. |
| **Pika 360 (`/platform/pika-360`)** | Hand-coded HTML customer card showing fake persona with CSS badges. | Replace with full-bleed framed asset **`img_pika-360_7.png`** with spotlight callouts. |
| **Günün Fırsatları (`/platform/gunun-firsatlari`)**| CSS list items illustrating repeat purchase, win-back, and cross-sell. | Replace with genuine cockpit UI **`img_gunun-firsatlari.png`**. |
| **Product Intelligence (`/platform/product-intelligence`)**| Hand-built CSS tables and cards explaining Need Groups and Product Roles. | Replace with real UI screenshots **`img_need-group-product-role_11.png`** and **`img_urun-siniflandirma-workbench_13.png`**. |
| **Campaign Manager (`/cozumler/campaign-manager`)**| Broken image link (`img_kampanya-yonetimi_0.png`) and hardcoded simulation box with unverified 14.2x ROAS. (Status: OPEN — APPROVED REPLACEMENT IDENTIFIED) | Approved future replacement: **`img_yayinlama-sablon-ve-yonetim_27.png`**. The actual View replacement will occur during the Campaign Manager page implementation phase. |
| **Journey Manager (`/cozumler/journey-manager`)**| Stylized CSS flowchart nodes with icons. | Replace with authentic visual journey builder **`img_journey-tasarim-tuvali_18.png`**. |
| **Content Studio (`/cozumler/content-studio`)**| Hand-coded HTML drag-and-drop simulation. | Replace with real email editor screenshot **`img_email-template-editor_17.png`**. |
| **Audience Manager (`/cozumler/audience-manager`)**| Hand-coded floating badge elements (12.8K kişi, 5 koşul) and grey mock bars. | Replace with genuine cohort management interface **`img_kisi-listesi-ve-segmentler_3.png`**. |
| **Analytics & Reporting (`/cozumler/analytics-reporting`)**| Hand-built CSS metric cards. | Replace with real BI Cockpit screenshot **`img_bi-kokpit_5.png`**. |
| **Consent Management (`/cozumler/consent-management`)**| Abstract shield icons and placeholder scene blocks. | Replace with real consent management console **`img_izin-kanal-zamanlama_25.png`**. |

---

## Unused & Legacy Marketing Assets Inventory

1. **`wwwroot/images/1.png` to `5.png`:**
   - *Status:* **UNUSED_LEGACY.**
   - *Audit Finding:* These 5 image files (approx. 100KB–150KB each) are not referenced anywhere in the Views, CSS, or scripts.
   - *Recommendation:* Keep unreferenced; candidate for future archival.
2. **`wwwroot/web/images/partner/partner-logo-1.png` to `11.png`:**
   - *Status:* **UNUSED_TEMPLATE_ARTIFACT.**
   - *Audit Finding:* 11 generic template placeholder logos from the original theme purchase. They are not referenced anywhere in the application.
   - *Recommendation:* Keep strictly unreferenced. Do not display on public site as fake client or partner proof.
3. **`wwwroot/og-image-generator.html` & `og-image.png`:**
   - *Status:* **OPERATIONAL.**
   - *Audit Finding:* `og-image.png` is correctly linked in `_Layout.cshtml` for OpenGraph and Twitter cards.
