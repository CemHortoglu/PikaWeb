# PIKA WEB 2.0 — P06: HOMEPAGE V2 IMPLEMENTATION REPORT (FLUID & ANIMATED)
**Canonical Document:** `docs/marketing/P06_HOMEPAGE_IMPLEMENTATION.md`  
**Phase:** P06 — Homepage V2 (Product-Led Commercial Narrative in Insider One Style)  
**Status:** COMPLETED  
**Design Reference:** [Insider One](https://insiderone.com/) adapted to Pika's brand palette & product truth  
**Approved Baseline Commit:** `438f66cd0c16279a9bc8737902704b782e504351`  
**Target Category:**
- TR: `Müşteri Zekâsı ve Omnichannel Pazarlama Platformu`
- EN: `Customer Intelligence & Omnichannel Marketing Platform`

---

## 1. Redesign Philosophy: From Screenshot Manual to Fluid Visual Storytelling

Following direct product owner feedback, Pika's homepage was fundamentally elevated from a dry, screenshot-heavy software manual into a **world-class, fluid, animated commercial experience** directly modeled after the visual sophistication of **Insider One**.

### Why Literal Screenshots Were Eliminated from the Homepage
- In modern high-end B2B tech marketing (Insider One, Stripe, Linear, Braze), literal full desktop application screenshots feel static, clinical, and difficult to parse on commercial entry surfaces.
- Instead, the homepage now employs **bespoke, stylized UI compositions, floating glassmorphism cards, realistic smartphone mockups with interactive chat bubbles, SVG data flow pipelines, and animated micro-meters**.
- Full desktop software screens remain safely housed where they belong: in product documentation, feature deep-dive pages, and the internal wiki.

---

## 2. Nine-Chapter Narrative in Insider One Style

| Chapter # | Chapter Name | Landmark ID | Visual Storytelling & Insider One-Style Execution |
|---|---|---|---|
| **1** | **Hero: Unified Commercial Intelligence Above the Fold** | `#hero` | **Atmospheric Dark Aurora Hero**: Deep Midnight Petrol (`#050e17`) with emerald/cyan radiant horizon glow. Left: High-contrast typography with gradient accent text and business email demo input bar. Right: Central dark-glass decision console with floating glassmorphism badges (`Melis K. %92 Tüketim Döngüsü`, `CVS: 86/100`, `Onaylı Gönderim`). |
| **-** | **Trust & Channel Ribbon** | (Hero sub-ribbon) | Monochromatic translucent bar highlighting confirmed channels (WhatsApp Business API, Email, SMS), CCE Algorithm, and pre-dispatch İYS/KVKK compliance. |
| **2** | **Günün Fırsatları: Commercial Opportunity Engine** | `#firsatlar` | **4-Card Interactive Bento Grid**: Repurchase Rhythm (%80-%120 consumption window with animated progress bar), Cross-Category Expansion (Need Group match), Basket Association, and Churn Intervention. |
| **3** | **Operating Model: Data → Intelligence → Action** | `#calisma-modeli` | **3-Step Interactive Engine**: Vertical step tabs (Data & Ingestion, Customer & Product Intel, Opportunity & Governed Action) paired with a high-tech SVG pipeline stage showing real-time event flow. |
| **4** | **Customer Intelligence & Product Intelligence** | `#zeka-katmani` | **Twin Intelligence Bento**: Left card features circular CVS score dial (84/100) and consumption timeline. Right card features Product Role mapping (Core Hero, Basket Builder) and Need Group hierarchy. |
| **5** | **Pika 360: Unified Customer Decision Console** | `#pika-360` | **Stylized Single-Customer Profile Stage**: Header with VIP Sadık avatar, lifetime turnover (₺24.850), CVS score (91/100), multi-channel interaction timeline, and 1-click execution triggers. |
| **6** | **Decision to Action: Governed Execution Engine** | `#aksiyon-ve-icra` | **Omnichannel Bento & Smartphone Mockup**: Visual Journey canvas node workflow paired with a **realistic smartphone mockup showing an official Meta WhatsApp Business API conversation** with quick-reply buttons. Confirmed channels: Email, SMS, WhatsApp (strictly NO push). |
| **7** | **Pika Pilot: Grounded Campaign & Decision AI** | `#pika-pilot` | **Luminous AI Studio Terminal**: Dark-glass card with glowing gradient border (`teal -> cyan -> indigo`), interactive natural-language prompt simulation bar, and instant cohort/dispatch recommendations. |
| **8** | **Measurement & Trust: Governed Execution** | `#yonetisim-ve-olcumleme` | **Dark Petrol Telemetry Stage**: SVG area chart displaying **Attributed Revenue** (*ilişkilendirilen ciro*), delivery state machines, İYS & quiet-hours dispatch windows, and multi-tenant security. |
| **9** | **Final Commercial CTA: Custom Quote & Engagement** | `#teklif-ve-demo` | **Atmospheric Horizon CTA**: Glowing aurora night sky, canonical pricing notice (*"İhtiyacınıza ve kullanım kapsamınıza göre özel teklif."*), immediate action triggers, and oversized `PIKA` brand typography watermark. |

---

## 3. Design Tokens & CSS Architecture

The stylesheet `Pika/wwwroot/css/pika-home.css` was completely refactored with zero regressions to the shared layout:
- **Atmospheric Colors:**
  - `--pw2-dark-midnight`: `#050e17`
  - `--pw2-dark-petrol`: `#081622`
  - `--pw2-brand-teal`: `#0d9488`
  - `--pw2-brand-cyan`: `#06b6d4`
  - `--pw2-brand-teal-glow`: `rgba(13, 148, 136, 0.35)`
- **Keyframe Animations:**
  - `@keyframes floatSlow`: Floating movement for hero micro-badges.
  - `@keyframes floatMedium`: Out-of-phase floating for metric badges.
  - `@keyframes pulseDot`: Radar ping animation for real-time status dots.
  - `@keyframes pulseGlow`: Subtle card border glow.
- **Component Primitives:**
  - `.pw2-hero-insider`: Radial aurora background with gradient text.
  - `.pw2-hero-input-bar`: Inline business email input with embedded button.
  - `.pw2-phone-mockup`: Smartphone chassis with WhatsApp chat bubble and interactive action buttons.
  - `.pw2-cce-card`: Opportunity bento cards with dynamic visual meters.
  - `.pw2-ai-terminal`: Glass terminal with glowing neon gradient top border.
  - `.pw2-telemetry-stage`: Attributed revenue SVG area chart container.
  - `.pw2-brand-watermark`: Giant oversized footer watermark.
- **Preserved Layout Invariants:**
  - Shared navbar mega menu styles (`.pika-mega-*`) preserved completely.

---

## 4. Governance & Policy Guardrails (100% Preserved)

1. **Category:** Strictly `Müşteri Zekâsı ve Omnichannel Pazarlama Platformu` (TR) / `Customer Intelligence & Omnichannel Marketing Platform` (EN).
2. **Zero "enterprise-grade":** No prestige adjectives.
3. **Execution Channels:** Strictly **Email, SMS, WhatsApp**. Strictly **NO push notifications** anywhere on the homepage.
4. **Channel claims:** Strictly **NO "tüm kanallar" or "all channels"**.
5. **No fake metrics:** No `12.4K`, `%68`, `%24`.
6. **Commercial Quote:** Canonical statement: *"İhtiyacınıza ve kullanım kapsamınıza göre özel teklif."* / *"Custom quote based on your specific needs and usage scope."*
7. **Attributed Revenue:** Phrased as *"ilişkilendirilen ciro"* / *"attributed revenue"*.
8. **Dispatch Windows:** Phrased as *"gönderim zaman pencereleri"* / *"quiet-hours dispatch window"*.
9. **SEO Authority:** `Index.cshtml` does NOT override `ViewData["Title"]` or `ViewData["MetaDescription"]`; resolved via `SeoHelper`.
10. **Landmarks:** All 9 section landmark IDs intact: `hero`, `firsatlar`, `calisma-modeli`, `zeka-katmani`, `pika-360`, `aksiyon-ve-icra`, `pika-pilot`, `yonetisim-ve-olcumleme`, `teklif-ve-demo`.

---

## 5. Automated Verification Results

- **Test Suite:** `Pika.Web.Tests/P06HomepageTests.cs`
- **Execution:**
  ```text
  C:\Projects\PikaWeb\Pika\Pika.Web.Tests\bin\Debug\net10.0\Pika.Web.Tests.dll (.NETCoreApp,Version=v10.0)
  Başarılı! - Başarısız: 0, Başarılı: 301, Atlanan: 0, Toplam: 301, Süre: 1 s
  ```
- **Total Passing Tests:** **301 tests passed, 0 failed, 0 skipped** (100% pass rate).
