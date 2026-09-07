# Pika Web 2.0 — P05: SEO + LLM Entity Architecture & Metadata Safety Convergence

---

## 1. Executive Summary & Epistemic Contract

### 1.1 Purpose of Phase P05
Phase P05 establishes the authoritative **SEO Entity Architecture**, **LLM Discoverability Architecture**, and **Public Metadata Safety Contract** for the Pika Web 2.0 platform. Building upon the closed baselines of P00 (Product Truth), P01 (Information Architecture), P02 (Visual Constitution), P03 (Authentic Visual System), and P04 (Content Bible), P05 ensures that external search engines, semantic web crawlers, generative AI retrieval engines, and LLM answer systems perceive exactly the same governed commercial entity that Pika's product reality and Content Bible define.

### 1.2 Epistemic Governance Principles
1. **Convergence on Product Reality:** Public search snippets, meta descriptions, Open Graph cards, structured data, and LLM text files must strictly reflect verified product capabilities. No marketing hype, speculative features, or unsupported compliance certifications may enter machine-readable surfaces.
2. **Strict Adherence to Claims Registry:** Every public claim made in HTML head metadata or LLM documents maps directly to `CLAIMS_REGISTRY.md` (CLM-001 through CLM-020). Claims marked `DO_NOT_MARKET` or `NEEDS_PRODUCT_CONFIRMATION` are categorically prohibited from SEO metadata. Where a public concept has no registered claim ID, its authority derives from `PRODUCT_OWNER_APPROVED_CANON` or is marked `NEEDS_EVIDENCE_REGISTRATION`.
3. **Push Governance Strictness:** Push notifications are classified as `CONTRADICTORY / UNCONFIRMED` (`CLM-001`). Push is excluded from all public navigation, sitemaps, structured data, and capability matrices until explicit product-owner confirmation. Future confirmation may alter its status, but it must never be represented as a current or roadmap capability today.
4. **Separation of Concerns:** P05 governs machine-readable metadata, search indexation contracts, structured data, and LLM ingestible files. It strictly refrains from redesigning views, altering public page bodies, modifying CSS, or restructuring routes.

---

## 2. Canonical Entity Graph

### 2.1 Core Entities & Attributes
The semantic web model of Pika is organized into distinct, hierarchically governed entities:

```
[ Pika: Organization / Platform ] (Source: PRODUCT_OWNER_APPROVED_CANON)
  │
  ├── [ Customer Intelligence ] (Analytical Layer — Evidence: B. DOCUMENTED)
  │     ├── Customer Value Score (CVS) (0-100 Metric)
  │     ├── Purchasing Cycle (%80-%120 Consumption Rhythm)
  │     └── Churn Risk Classification
  │
  ├── [ Product Intelligence ] (Catalog Enrichment — Evidence: B. DOCUMENTED)
  │     ├── Need Groups (Consumer Motivation)
  │     ├── Product Roles (Traffic Driver, Basket Booster, Margin)
  │     └── Market Basket Affinity (Support, Confidence, Lift)
  │
  ├── [ Opportunity & Decision Engine (CCE) ] (Evidence: B. DOCUMENTED)
  │     └── Günün Fırsatları (Daily Commercial Opportunities)
  │
  ├── [ Engagement & Orchestration ] (Evidence: B. DOCUMENTED)
  │     ├── Audience Manager (Dynamic Segment Builder)
  │     ├── Campaign Manager (Multi-Channel Broadcast Hub)
  │     ├── Journey Manager (Visual Workflow Canvas)
  │     ├── Content Studio (Drag-and-Drop Designer)
  │     └── AI Campaign Assistant (Pika Pilot - Draft Generator; CLM-017, CLM-018)
  │
  ├── [ Execution Channels ] (CLM-001)
  │     ├── Email (Transactional & Broadcast — Evidence: B. DOCUMENTED)
  │     ├── SMS (Operator Gateways & Rate Limiter — Evidence: B. DOCUMENTED)
  │     └── WhatsApp (Official Meta WhatsApp Business API — Evidence: B. DOCUMENTED)
  │
  └── [ Compliance & Governance ]
        ├── Consent Management (İYS & KVKK Sync — Evidence: A. CODE_VERIFIED / B. DOCUMENTED; CLM-010)
        ├── Delivery Telemetry & Attribution (BI Kokpit — Evidence: B. DOCUMENTED)
        └── Role-Based Access Control (RBAC — Evidence: B. DOCUMENTED)
```

### 2.2 Entity Relationship Constraints
- **Channel Execution Subordination:** Channels (Email, SMS, WhatsApp) do not operate in isolation; they are execution endpoints driven by Audience Manager cohorts or Journey Manager event triggers.
- **AI Positioning:** The AI Campaign Assistant (*Pika Pilot*) is strictly an assistive copilot requiring mandatory human approval (`CLM-017`). It consumes calculated contextual attributes (not raw personal identifiers; `CLM-018`) and assists with draft generation; it is never marketed as an autonomous marketing agent.
- **Push Quarantine:** Push is isolated from the active entity graph (`CLM-001`). It exists solely as an unindexed legacy reference route.

---

## 3. Category, Brand & Proposition Matrix

### 3.1 Canonical Category Definition
- **Turkish (TR):** `Müşteri Zekâsı ve Omnichannel Pazarlama Platformu`  
  *(Status: APPROVED_COPY | Source: PRODUCT_OWNER_APPROVED_CANON)*
- **English (EN):** `Customer Intelligence & Omnichannel Marketing Platform`  
  *(Status: APPROVED_COPY | Source: PRODUCT_OWNER_APPROVED_CANON)*

### 3.2 Canonical Brand Ideas & Anchors
| Dimension | Turkish (TR) | English (EN) | Governance Status | Source |
| :--- | :--- | :--- | :--- | :--- |
| **Brand Idea** | "Daha çok mesaj değil, daha doğru ilişki." | "Not more messages. Better customer relationships." | APPROVED_COPY | PRODUCT_OWNER_APPROVED_CANON |
| **Brand Anchor** | Doğru Anda. | At the Right Moment. | APPROVED_COPY | PRODUCT_OWNER_APPROVED_CANON |
| **Core Axiom** | "Pika’nın başlangıç noktası mesaj göndermek değil, anlamaktır." | "Pika’s starting point is not merely sending messages; it is understanding customer, product, and transaction context." | APPROVED_COPY | PRODUCT_OWNER_APPROVED_CANON |

### 3.3 Three Pillars of Value
1. **Understand (Anlayın):** Unify POS, e-commerce, and transaction data into deterministic customer profiles and catalog dynamics.
2. **Discover (Bulun):** Calculate consumption intervals, basket affinities, and replenishment windows via the Customer Context Engine.
3. **Act (Aksiyona Geçin):** Orchestrate compliant, rate-governed multi-channel messaging across Email, SMS, and WhatsApp.

---

## 4. URL & Routing Entity Alignment

### 4.1 Canonical Route Matrix (TR / EN)
All public indexable pages maintain an exact 1:1 mapping between Turkish and English routes:

| Primary Entity | Canonical Route (TR) | Canonical Route (EN) | Indexation Status |
| :--- | :--- | :--- | :--- |
| **Homepage** | `/` | `/en/` | Index, Follow |
| **Product Overview** | `/pika` | `/en/pika` | Index, Follow |
| **Customer Intelligence** | `/platform/customer-intelligence` | `/en/platform/customer-intelligence` | Index, Follow |
| **Product Intelligence** | `/platform/product-intelligence` | `/en/platform/product-intelligence` | Index, Follow |
| **Pika 360** | `/platform/pika-360` | `/en/platform/pika-360` | Index, Follow |
| **Günün Fırsatları** | `/platform/gunun-firsatlari` | `/en/platform/opportunities` | Index, Follow |
| **Campaign Manager** | `/cozumler/campaign-manager` | `/en/solutions/campaign-manager` | Index, Follow |
| **Audience Manager** | `/cozumler/audience-manager` | `/en/solutions/audience-manager` | Index, Follow |
| **Journey Manager** | `/cozumler/journey-manager` | `/en/solutions/journey-manager` | Index, Follow |
| **Content Studio** | `/cozumler/content-studio` | `/en/solutions/content-studio` | Index, Follow |
| **Email Marketing** | `/kanallar/email` | `/en/channels/email` | Index, Follow |
| **SMS Campaigns** | `/kanallar/sms` | `/en/channels/sms` | Index, Follow |
| **WhatsApp Messaging** | `/kanallar/whatsapp` | `/en/channels/whatsapp` | Index, Follow |
| **Analytics & Reporting** | `/cozumler/analytics-reporting` | `/en/solutions/analytics-reporting` | Index, Follow |
| **Consent Management** | `/cozumler/consent-management` | `/en/solutions/consent-management` | Index, Follow |
| **Integrations** | `/entegrasyonlar` | `/en/integrations` | Index, Follow |
| **Security & Privacy** | `/guvenlik-ve-gizlilik` | `/en/security-and-privacy` | Index, Follow |
| **AI Campaign Assistant** | `/urunler/ai-kampanya-asistani` | `/en/products/ai-campaign-assistant` | Index, Follow |
| **Demo Request** | `/demo-talebi` | `/en/demo-request` | Index, Follow |
| **Contact** | `/iletisim` | `/en/contact` | Index, Follow |
| **FAQ** | `/kaynaklar/sss` | `/en/resources/faq` | Index, Follow |
| **Use Cases** | `/kullanim-senaryolari` | `/en/use-cases` | Index, Follow |

### 4.2 Redirect Policy (Resolved in P01)
- `/cozumler/personalization` → 301 Permanent Redirect → `/cozumler/journey-manager`
- `/cozumler/template-management` → 301 Permanent Redirect → `/cozumler/content-studio` (and `/en/solutions/template-management` → `/en/solutions/content-studio`)
- `/cozumler/ab-testing` → 301 Permanent Redirect → `/cozumler/campaign-manager`
- `/cozumler/deliverability-compliance` → 301 Permanent Redirect → `/cozumler/consent-management`
- `/cozumler/real-time-event-processing` → 301 Permanent Redirect → `/entegrasyonlar`
- `/fiyatlandirma` / `/en/pricing` → 301 Permanent Redirect → `/demo-talebi` / `/en/demo-request` (`CLM-014`)

### 4.3 Noindex Quarantined Routes
- `/kanallar/push` and `/en/channels/push`: Marked with `noindex, follow` and excluded from `sitemap.xml` because Push is `CONTRADICTORY / UNCONFIRMED` (`CLM-001`).

---

## 5. Hreflang & Multi-Language Architecture

### 5.1 Symmetric Pairing Contract
Every canonical indexable route emits three symmetric `<link rel="alternate">` tags via `SeoHelper.GetHreflangAlternates`:
1. `hreflang="tr"`: Points to the absolute URL of the Turkish version.
2. `hreflang="en"`: Points to the absolute URL of the English version.
3. `hreflang="x-default"`: Points to the Turkish URL (the default operating language).

```html
<!-- Example for https://pika.tr/platform/customer-intelligence -->
<link rel="alternate" hreflang="tr" href="https://pika.tr/platform/customer-intelligence" />
<link rel="alternate" hreflang="en" href="https://pika.tr/en/platform/customer-intelligence" />
<link rel="alternate" hreflang="x-default" href="https://pika.tr/platform/customer-intelligence" />
```

### 5.2 Quarantine Exclusion
Non-canonical, redirected, or noindexed routes (such as `/kanallar/push`) are excluded from public hreflang cross-indexing in `_Layout.cshtml` to prevent crawl budget waste and conflicting indexation directives.

---

## 6. Structured Data (JSON-LD) Specification

### 6.1 Organization Schema
Emitted globally on all pages within `Views/Shared/_Layout.cshtml`:
```json
{
  "@context": "https://schema.org",
  "@type": "Organization",
  "@id": "https://pika.tr/#organization",
  "name": "Pika",
  "alternateName": "Pika Müşteri Zekâsı ve Omnichannel Pazarlama Platformu",
  "url": "https://pika.tr/",
  "logo": "https://pika.tr/logo.png",
  "description": "Pika; müşteri, ürün ve satış verisini birlikte anlamlandırarak işletmenin bugün hangi müşteride hangi ticari fırsatın oluştuğunu görmesini ve bu fırsatı kontrollü, ölçülebilir çok kanallı aksiyona dönüştürmesini sağlayan müşteri zekâsı ve pazarlama platformudur.",
  "contactPoint": [
    {
      "@type": "ContactPoint",
      "telephone": "+90 (850) 000 00 00",
      "contactType": "customer support",
      "availableLanguage": ["Turkish", "English"]
    },
    {
      "@type": "ContactPoint",
      "email": "info@pika.com.tr",
      "contactType": "sales"
    }
  ],
  "sameAs": [
    "https://www.linkedin.com/company/pikatr"
  ]
}
```
> [!IMPORTANT]
> **Forbidden Properties in Organization Schema:**
> - `foundingLocation`: Categorically omitted. The founding location is unconfirmed (`CLM-020`: İstanbul vs Ankara contradiction).
> - Speculative ratings, reviews, or unverified employee headcounts.

### 6.2 WebSite Schema
Emitted globally on all pages within `Views/Shared/_Layout.cshtml`:
```json
{
  "@context": "https://schema.org",
  "@type": "WebSite",
  "@id": "https://pika.tr/#website",
  "url": "https://pika.tr/",
  "name": "Pika",
  "inLanguage": ["tr-TR", "en-US"],
  "publisher": {
    "@id": "https://pika.tr/#organization"
  }
}
```
> [!NOTE]
> **Absence of SearchAction:** Pika does not provide a public on-site search endpoint. Emitting a `SearchAction` potentialAction would result in Google Search Console rich-result validation warnings.

### 6.3 BreadcrumbList Schema
Injected conditionally on all non-root canonical pages:
```json
{
  "@context": "https://schema.org",
  "@type": "BreadcrumbList",
  "itemListElement": [
    {
      "@type": "ListItem",
      "position": 1,
      "name": "Ana Sayfa",
      "item": "https://pika.tr/"
    },
    {
      "@type": "ListItem",
      "position": 2,
      "name": "Müşteri Zekâsı",
      "item": "https://pika.tr/platform/customer-intelligence"
    }
  ]
}
```

### 6.4 SoftwareApplication / Product / Offer Schema
- **Status:** `DEFERRED — REQUIRES SEPARATE GOVERNANCE`.
- SoftwareApplication, Product, and Offer structured data are deferred until verified data schemas can be modeled truthfully without speculative properties or pricing assumptions.

---

## 7. HTML Head Governance

### 7.1 Title Tag Standardization
All titles follow the deterministic pattern defined in `SeoHelper.FormatPageTitle`:
- `{PageTitle} | Pika`
- Length Budget: 50–65 characters.
- Fallback Title: `Pika | Müşteri Zekâsı ve Omnichannel Pazarlama Platformu`

### 7.2 Meta Description Standards
- Length Budget: 140–160 characters.
- Language Concordance: Turkish meta descriptions on `tr` routes; English on `en` routes.
- Content Rule: Must state what the platform/module does, for whom, and what commercial benefit it unlocks, without absolute or unverified claims.

### 7.3 Canonical & Robots Directives
- Every canonical page emits an absolute canonical tag: `<link rel="canonical" href="https://pika.tr{path}" />`
- Robots precedence in `_Layout.cshtml`:
  1. Explicit `ViewData["RobotsMeta"]` if supplied.
  2. Otherwise, if `seoMeta?.NoIndex == true`: `"noindex, follow"`.
  3. Otherwise: `"index, follow"`.

### 7.4 Open Graph & Twitter Cards
- `og:type`: `website`
- `og:site_name`: `Pika`
- `og:url`: Canonical absolute URL
- `og:image`: Currently falls back to `https://pika.tr/logo.png` in `_Layout.cshtml`. Dedicated 1200x630px social share card assets are `DEFERRED — FUTURE ASSET TASK`.
- `twitter:card`: `summary_large_image`

---

## 8. Page-by-Page Metadata Contract (Runtime Values)

The following table documents the authoritative runtime metadata implemented in `Services/SeoHelper.cs`:

| Key / Route (TR / EN) | Runtime Title (TR / EN) | Primary Entity | Index Status |
| :--- | :--- | :--- | :--- |
| **Home.Index**<br>`/` / `/en/` | Müşteri Zekâsı ve Omnichannel Pazarlama Platformu / Customer Intelligence & Omnichannel Marketing Platform | Pika Platform | `index, follow` |
| **Home.Pika**<br>`/pika` / `/en/pika` | Pika Nedir? / What is Pika? | Pika Platform | `index, follow` |
| **Home.Corporate**<br>`/kurumsal` / `/en/corporate` | Kurumsal / Corporate | Pika Platform | `index, follow` |
| **Home.DemoRequest**<br>`/demo-talebi` / `/en/demo-request` | Demo Talebi / Request a Demo | Demo Request | `index, follow` |
| **Home.Contact**<br>`/iletisim` / `/en/contact` | İletişim / Contact | Contact | `index, follow` |
| **Home.Career**<br>`/kariyer` / `/en/careers` | Kariyer / Careers | Corporate | `index, follow` |
| **Home.Faq**<br>`/kaynaklar/sss` / `/en/resources/faq` | Sıkça Sorulan Sorular / Frequently Asked Questions | FAQ | `index, follow` |
| **Home.TermsOfUse**<br>`/kullanim-sartlari` / `/en/terms-of-use` | Kullanım Şartları / Terms of Use | Legal | `index, follow` |
| **Home.PrivacyPolicy**<br>`/gizlilik-politikasi` / `/en/privacy-policy` | Gizlilik Politikası / Privacy Policy | Legal | `index, follow` |
| **Platform.CustomerIntelligence**<br>`/platform/customer-intelligence` / `/en/platform/customer-intelligence` | Customer Intelligence \| Müşteri Zekâsı / Customer Intelligence \| Customer Analytics | Customer Intelligence | `index, follow` |
| **Platform.ProductIntelligence**<br>`/platform/product-intelligence` / `/en/platform/product-intelligence` | Product Intelligence \| Ürün Zekâsı / Product Intelligence \| Product Analytics | Product Intelligence | `index, follow` |
| **Platform.Pika360**<br>`/platform/pika-360` / `/en/platform/pika-360` | Pika 360 \| Bütünleşik Müşteri Karar Ekranı / Pika 360 \| Unified Customer Decision View | Pika 360 | `index, follow` |
| **Platform.Opportunities**<br>`/platform/gunun-firsatlari` / `/en/platform/opportunities` | Günün Fırsatları \| Fırsat ve Karar Motoru / Daily Opportunities \| Opportunity & Decision Engine | Günün Fırsatları | `index, follow` |
| **Solutions.CampaignManager**<br>`/cozumler/campaign-manager` / `/en/solutions/campaign-manager` | Campaign Manager \| Çok Kanallı Kampanya Yönetimi / Campaign Manager \| Multi-Channel Campaign Management | Campaign Manager | `index, follow` |
| **Solutions.AudienceManager**<br>`/cozumler/audience-manager` / `/en/solutions/audience-manager` | Audience Manager \| Hedef Kitle ve Segmentasyon / Audience Manager \| Audience & Segmentation | Audience Manager | `index, follow` |
| **Solutions.JourneyManager**<br>`/cozumler/journey-manager` / `/en/solutions/journey-manager` | Journey Manager \| Müşteri Yolculuğu Otomasyonu / Journey Manager \| Customer Journey Automation | Journey Manager | `index, follow` |
| **Solutions.ContentStudio**<br>`/cozumler/content-studio` / `/en/solutions/content-studio` | Content Studio \| İçerik ve Şablon Tasarımı / Content Studio \| Content & Template Design | Content Studio | `index, follow` |
| **Solutions.ConsentManagement**<br>`/cozumler/consent-management` / `/en/solutions/consent-management` | Consent Management \| İzin ve Uyumluluk Yönetimi / Consent & Compliance Management | Consent Management | `index, follow` |
| **Solutions.EmailMarketing**<br>`/kanallar/email` / `/en/channels/email` | Email Marketing \| E-Posta Pazarlama Çözümleri / Email Marketing \| Email Marketing Solutions | Email Marketing | `index, follow` |
| **Solutions.SmsCampaigns**<br>`/kanallar/sms` / `/en/channels/sms` | SMS Campaigns \| SMS Kampanya Yönetimi / SMS Campaigns \| SMS Campaign Management | SMS Campaigns | `index, follow` |
| **Solutions.WhatsAppMessaging**<br>`/kanallar/whatsapp` / `/en/channels/whatsapp` | WhatsApp Messaging \| WhatsApp Kampanya ve Mesajlaşma / WhatsApp Messaging \| WhatsApp Marketing & Messaging | WhatsApp Messaging | `index, follow` |
| **Solutions.Reporting**<br>`/cozumler/analytics-reporting` / `/en/solutions/analytics-reporting` | Analytics & Reporting \| Performans ve Raporlama / Analytics & Reporting \| Analytics & Reporting | Analytics & Reporting | `index, follow` |
| **Solutions.Integrations**<br>`/entegrasyonlar` / `/en/integrations` | Integrations \| Entegrasyonlar / Integrations \| Integrations | Integrations | `index, follow` |
| **Solutions.SecurityPrivacy**<br>`/guvenlik-ve-gizlilik` / `/en/security-and-privacy` | Security & Privacy \| Güvenlik ve Gizlilik / Security & Privacy \| Security & Privacy | Security & Privacy | `index, follow` |
| **Solutions.AiCampaignAssistant**<br>`/urunler/ai-kampanya-asistani` / `/en/products/ai-campaign-assistant` | Pika AI Kampanya Asistanı \| Yapay Zekâ Destekli Kampanya Üretimi / Pika AI Campaign Assistant \| AI-Powered Campaign Creation | AI Campaign Assistant | `index, follow` |
| **Solutions.UseCases**<br>`/kullanim-senaryolari` / `/en/use-cases` | Kullanım Senaryoları \| Pika Omnichannel Çözümleri / Use Cases \| Pika Omnichannel Solutions | Use Cases | `index, follow` |
| **Solutions.PushNotifications**<br>`/kanallar/push` / `/en/channels/push` | Push Notifications \| Anlık Bildirim Yönetimi / Push Notifications \| Push Notification Management | Push (Quarantined; CLM-001) | `noindex, follow` |

> [!NOTE]
> Any future copywriting refinements to titles or descriptions are marked `PROPOSED (FUTURE / NEEDS_CONTENT_OWNER)` and must not alter runtime code without explicit content-owner approval.

---

## 9. LLM Context Architecture

### 9.1 `llms.txt` Specification
- **Target Audience:** LLM inference agents and semantic bots indexing site structure.
- **Tone:** Concise, factual, high-density Turkish.
- **Content:** Canonical positioning statement, 6-step platform loop, direct documentation links to marketing and wiki hubs.
- **Constraints:** ZERO mentions of Push; ZERO mentions of SOC, SAML, or fixed pricing.

### 9.2 `llms-full.txt` Specification
- **Target Audience:** Deep-context AI agents, enterprise procurement researchers, and technical evaluators.
- **Tone:** Comprehensive, structured English documentation.
- **Content:** Full platform architecture, module definitions, commercial model, integration capabilities, and governance policies.
- **Push Governance Invariant (`CLM-001`):** Push is explicitly qualified as:
  > *(Note: Push is CONTRADICTORY / UNCONFIRMED and is excluded from public channel/capability signals until explicit product-owner confirmation. Future confirmation may change its governance state. It must not be inferred as a live capability.)*
- **AI Privacy Boundary (`CLM-018`):** Restrained, evidence-safe phrasing:
  > *Internal product documentation describes an anonymized-parameter AI boundary where direct personal identifiers are intended to be excluded from LLM prompt context, and all dispatches require explicit human review.*

### 9.3 Anti-Hallucination Guardrails for LLM Grounding
1. **No Autonomous AI Bot Claims (`CLM-017`):** LLM consumers must understand that Pika Pilot provides assistive draft generation requiring human sign-off; it does not independently dispatch messages.
2. **No Real-Time Millisecond Guarantees (`CLM-009`):** Data ingestion is asynchronous and batch-optimized; avoid claiming sub-second real-time streaming.
3. **Deterministic Intelligence:** Customer Value Score and RFM metrics are computed via deterministic mathematical formulas, not opaque deep learning black boxes.

---

## 10. Claim-Safety in Public Metadata

The following table maps the authoritative claims from `CLAIMS_REGISTRY.md` (CLM-001 through CLM-020) to metadata enforcement:

| Claim ID | Claim Summary | Registry Status | Evidence Level | Metadata Enforcement Rule |
| :--- | :--- | :--- | :--- | :--- |
| **CLM-001** | Email/SMS/WhatsApp/Push channel scope ("Tüm kanallar tek platformda") | CONTRADICTORY | `D. CONTRADICTORY` | Push excluded from live channel lists; marked `noindex`; "tüm kanallar" / "all channels" removed from homepage metadata. |
| **CLM-002** | ROAS 14.2x | NEEDS_PRODUCT_CONFIRMATION | `E. UNVERIFIED` | Excluded from all public metadata and search descriptions. |
| **CLM-003** | Unverified uplift percentages (%52 open, %31 visit, %41 conversion) | NEEDS_PRODUCT_CONFIRMATION | `E. UNVERIFIED` | Excluded from all public metadata and search descriptions. |
| **CLM-007** | SOC-compliant infrastructure | DO_NOT_MARKET | `C. MARKETING_ONLY` | Omitted from metadata, structured data, and LLM text. No SOC attestation is present in the repository. |
| **CLM-008** | SSO / SAML integration | DO_NOT_MARKET | `C. MARKETING_ONLY` | Omitted from metadata; restricted to Role-Based Access Control (RBAC). SSO/SAML is not verified for public marketing in current governance. |
| **CLM-009** | Millisecond processing ("Milisaniyeler içinde") | CONTRADICTORY | `D. CONTRADICTORY` | "milisaniyeler içinde" excised; metadata states asynchronous event-driven processing. |
| **CLM-010** | IYS / KVKK compliance wording | DOCUMENTED PRODUCT CAPABILITY | `B. DOCUMENTED` / `A. CODE_VERIFIED` | Marketed as technical workflow and consent management infrastructure; not an absolute legal indemnity. |
| **CLM-012** | Chatbot flows / two-way WhatsApp conversation | DO_NOT_MARKET | `D. CONTRADICTORY` / `C. MARKETING_ONLY` | WhatsApp positioned strictly as approved template/notification messaging; chatbot claims forbidden. |
| **CLM-014** | Fixed public pricing packages | RETIRED / DELETED IN P01 | `A. CODE_VERIFIED` | Fixed pricing forbidden; quotation-based model enforced across metadata and LLM files. |
| **CLM-016** | Corporate SLA tiers | DO_NOT_MARKET / NEEDS_CONFIRMATION | `E. UNVERIFIED` | SLA tiers omitted from public metadata and commercial terms. |
| **CLM-017** | Autonomous AI marketing dispatch | REFUTED (Safety Boundary) | `A. CODE_VERIFIED` | AI positioned as an assistive draft co-pilot with mandatory human verification. |
| **CLM-018** | AI personal data handling / Zero-PII guarantee | DOCUMENTED PRODUCT CAPABILITY | `B. DOCUMENTED` | Documented anonymized-parameter boundary; absolute zero-PII guarantee avoided without owner confirmation. |
| **CLM-019** | Legacy positioning ("Kullanıcı sadakat ve kampanya...") | LEGACY / OUTDATED | `A. CODE_VERIFIED` | Replaced with canonical "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu". |
| **CLM-020** | Headquarters contradiction (İstanbul vs Ankara) | NEEDS_PRODUCT_CONFIRMATION | `D. CONTRADICTORY` | `foundingLocation` completely omitted from Organization JSON-LD. |

*Note: For public concepts without an existing registry ID, authority derives from `PRODUCT_OWNER_APPROVED_CANON` (Category, Brand Idea, Core Promise) or is tracked as `NEEDS_EVIDENCE_REGISTRATION`.*

---

## 11. Channel Governance

### 11.1 Confirmed Channels
1. **Email:** Drag-and-drop template composition, responsive layouts, dynamic personalization tokens, delivery status machine. *(Evidence: `B. DOCUMENTED`)*
2. **SMS:** Rate-limited queue dispatcher, operator gateway delivery, automated opt-out handling. *(Evidence: `B. DOCUMENTED`)*
3. **WhatsApp:** Official Meta WhatsApp Business API integration, pre-approved message templates. *(Evidence: `B. DOCUMENTED`)*

### 11.2 Push Status & Quarantine Contract (`CLM-001`)
- **Governance State:** `CONTRADICTORY / UNCONFIRMED`.
- **Public Signal Exclusion:** Excluded from header/footer navigation, canonical sitemaps, active marketing collateral, and platform channel summaries.
- **Indexation Directives:** If the legacy route `/kanallar/push` is requested, it serves with `<meta name="robots" content="noindex, follow" />` and is excluded from hreflang alternate lists.
- **Future Re-evaluation:** If the product owner validates push notification infrastructure in a future phase, it may only be promoted after formal evidence update in `PRODUCT_TRUTH.md` and `CLAIMS_REGISTRY.md`.

---

## 12. Integration & Developer Surface Architecture

### 12.1 Ingestion API
- **Endpoint:** `/api/v1/ingest/` *(Evidence: `B. DOCUMENTED`)*
- **Methodology:** Asynchronous REST ingestion designed for transactional batching.
- **Authentication:** Token-based API keys (`X-API-Key` header) with payload idempotency keys. *(Evidence: `B. DOCUMENTED`)*
- **Payload Entities:** Customers (`/customers`), Orders/Transactions (`/transactions`), Catalog/Products (`/products`).

### 12.2 File-Based Data Exchange
- Built-in CSV and Excel upload workflows for POS systems, ERP exports, and customer lists. *(Evidence: `B. DOCUMENTED`)*

### 12.3 Outbound Webhooks & Delivery Telemetry
- Event-driven dispatch callbacks reporting delivery status, bounce events, and unsubscribe triggers. *(Evidence: `B. DOCUMENTED`)*

---

## 13. Security, Privacy & Compliance Messaging Contract

### 13.1 Regulatory Compliance
- **KVKK Compliance:** Native consent tracking, explicit marketing opt-in logging, customer data anonymization workflows. *(Evidence: `A. CODE_VERIFIED` / `B. DOCUMENTED`; CLM-010)*
- **İYS (İleti Yönetim Sistemi) Integration:** Synchronized opt-in/opt-out status verification workflows prior to dispatching commercial electronic messages. *(Evidence: `B. DOCUMENTED`; CLM-010)*

### 13.2 Technical Security Safeguards
- **Role-Based Access Control (RBAC):** Tiered permissions separating campaign creators, analysts, and administrative managers. *(Evidence: `B. DOCUMENTED`)*
- **Transport Security:** Standard TLS/HTTPS encryption across public web endpoints.
- **Audit Logging:** Internal audit trail workflows. *(Status: `NEEDS_EVIDENCE_REGISTRATION`)*

### 13.3 Forbidden Security Claims
- **SOC Attestation (`CLM-007`):** No SOC attestation is present in the repository. Broad SOC-compliance claims are strictly forbidden.
- **SAML / Enterprise SSO (`CLM-008`):** SSO/SAML is not verified for public marketing in current governance. Public claims must restrict to Role-Based Access Control (RBAC).

---

## 14. Commercial & Pricing Model Discoverability

### 14.1 Locked Commercial Rules (`CLM-014`)
Pika operates strictly on a customized quotation commercial model. The following rules govern all public metadata and LLM surfaces:
1. **No Fixed Public Pricing:** No subscription tiers, packages, or prices are published.
2. **No Public Starting Price:** No baseline "starts at ₺X" claims exist.
3. **No Public Pricing Calculator:** Self-service calculators are forbidden.
4. **Tailored Quotations:** Pricing is tailored to requirements and scope of use.
5. **Approved Commercial CTAs:**
   - `Demo Talep Et` (TR) / `Request a Demo` (EN)
   - `Teklif Al` (TR) / `Get a Quote` (EN)
   - `Satış Ekibiyle Görüşün` (TR) / `Contact Sales` (EN)
6. **SLA Omission (`CLM-016`):** Corporate SLA tiers remain `DO_NOT_MARKET / NEEDS_CONFIRMATION` and are not advertised as a public capability or pricing factor.

---

## 15. Sitemap & Indexation Governance

### 15.1 Existing Sitemap Contract (`sitemap.xml`)
1. **Root Priority:** Root homepage (`https://pika.tr/`, `https://pika.tr/en/`) has `priority = 1.0`.
2. **Subpage Priority:** All indexable canonical subpages have `priority = 0.8`.
3. **Change Frequency:** Set to `changefreq = weekly`.
4. **Exclusion of Redirects:** 301 redirect stubs (`/cozumler/personalization`, `/cozumler/template-management`, `/cozumler/ab-testing`, etc.) are completely excluded.
5. **Exclusion of Noindex Pages:** Quarantined routes (`/kanallar/push` and `/en/channels/push`) are completely excluded.
6. **Date Stamping:** The current `sitemap.xml` does not emit `<lastmod>` elements; any future date-stamping task is `DEFERRED`.

---

## 16. Internal Linking & Anchor Text System

### 16.1 Semantic Link Hubs
Cross-linking follows the platform value chain:
- **Data Ingestion** links forward to **Customer Intelligence** and **Product Intelligence**.
- **Intelligence Engines** link forward to **Günün Fırsatları** (Decision Layer).
- **Günün Fırsatları** links forward to **Campaign Manager** and **Journey Manager**.
- **Campaign Manager** links forward to **Channels (Email, SMS, WhatsApp)** and **Consent Management**.
- **All Modules** link to **Demo Request** as the commercial action anchor.

### 16.2 Approved Anchor Texts
| Target Page | Approved TR Anchor Texts | Approved EN Anchor Texts | Forbidden Anchors |
| :--- | :--- | :--- | :--- |
| Customer Intelligence | Müşteri Zekâsı, CVS Analizi | Customer Intelligence, CVS Analytics | CRM Modülü, Müşteri Takip |
| Product Intelligence | Ürün Zekâsı, İhtiyaç Grupları | Product Intelligence, Need Groups | Ürün Kataloğu, Stok Listesi |
| Günün Fırsatları | Günün Fırsatları, Karar Motoru | Daily Opportunities, Opportunity Engine | İndirimli Ürünler, Günlük Fırsat |
| Campaign Manager | Kampanya Yönetimi, Campaign Manager | Campaign Manager, Campaign Orchestration | Toplu Mailer, SMS Gönderici |
| WhatsApp | WhatsApp Business API, Kurumsal WhatsApp | WhatsApp Business API, Official WhatsApp | WhatsApp Botu, Chatbot |
| Consent Management | İYS ve İzin Yönetimi, KVKK Uyum | Consent Management, IYS Compliance | Spam Koruması, SMS Onayı |

---

## 17. Knowledge Graph & External Citation Strategy

### 17.1 Authoritative External Footprint
- **LinkedIn:** `https://www.linkedin.com/company/pikatr`
- **Official Domain:** `https://pika.tr`
- **Application Portal:** `https://app.pika.tr`

### 17.2 Entity Disambiguation for Knowledge Engines
- **Distinction from Media Tools:** Pika must be disambiguated from creative AI video tools (such as Pika Labs / Pika Art).
- **Core Semantic Disambiguator:** Pika is a **B2B SaaS Enterprise Retail Marketing Platform** operating in Turkey, focused on transaction data, RFM analysis, and regulated communication delivery.

---

## 18. Contradiction & Anti-Pattern Prevention Matrix

| Banned Anti-Pattern | Related Claim | Reason for Ban | Correct Governed Alternative |
| :--- | :--- | :--- | :--- |
| Mentioning "Push" as an active channel | `CLM-001` | Push is UNCONFIRMED / CONTRADICTORY | Confirmed channels: Email, SMS, WhatsApp |
| "tüm kanallar tek platformda" | `CLM-001` | Implies Push channel availability | "E-posta, SMS ve WhatsApp tek platformda" |
| "15 dakikalık demo" / "15-minute demo" | `NEEDS_EVIDENCE_REGISTRATION` | Demo timing is unconfirmed | "Kapsamlı platform demosu" / "Tailored demo" |
| "Milisaniyeler içinde veri işleme" | `CLM-009` | Ingestion is asynchronous / batch-oriented | "Asenkron veri işleme ve olay bazlı tetikleme" |
| "Doğrudan ciro artışı garantisi" | `CLM-002`, `CLM-003` | Outcome causality cannot be guaranteed | "Kampanya etkileşimi ve ilişkilendirilen ciro atfı" |
| "SOC-compliant altyapı" | `CLM-007` | No SOC attestation present in repo | "Rol bazlı erişim denetimi ve güvenli altyapı" |
| "SAML 2.0 / Enterprise SSO" | `CLM-008` | SSO/SAML not verified for public marketing | "Rol bazlı yetkilendirme (RBAC)" |
| "WhatsApp chatbotu / 2-way AI" | `CLM-012` | Two-way chatbotting not supported | "Meta onaylı şablonlarla kurumsal bildirimler" |
| "Aylık 999 TL'den başlayan fiyatlar" | `CLM-014` | Pika has zero public pricing packages | "İhtiyaca özel teklif modeli / Demo Talebi" |
| "foundingLocation: İstanbul" in JSON-LD | `CLM-020` | Founding city has contradictory evidence | Completely omit `foundingLocation` property |

---

## 19. Automated Verification & Testing Contract

To ensure that the SEO entity architecture and metadata safety rules cannot regress during future development, the following suite of automated tests is enforced in `Pika.Web.Tests/SeoGovernanceTests.cs`:

1. **`CanonicalRouteMetadata_MaintainsSymmetricTrEnHreflangPairs`:** Validates that every canonical indexable page in `SeoHelper.AllPages` has matching `AlternatePathTr` and `AlternatePathEn` entries with an `x-default` fallback.
2. **`Solutions_PushNotifications_IsMarkedNoindexAndExcludedFromActiveMarketing`:** Asserts that `Solutions.PushNotifications` is flagged `NoIndex = true` in `SeoHelper` and `ViewData["RobotsMeta"]` contains `noindex`.
3. **`Layout_RobotsMeta_RespectsSeoHelperNoIndexAsRuntimeFallback`:** Verifies that when `ViewData["RobotsMeta"]` is absent, `_Layout.cshtml` falls back to `noindex, follow` if `seoMeta.NoIndex == true`.
4. **`HomepageMetadata_DoesNotContainAllChannelsOrTumKanallar`:** Asserts that homepage metadata does not contain "tüm kanallar" or "all channels" (`CLM-001`).
5. **`Layout_OrganizationJsonLd_OmitsFoundingLocation`:** Asserts that neither `foundingLocation`, `İstanbul`, nor `Ankara` are present in the Organization schema (`CLM-020`).
6. **`Layout_OrganizationJsonLd_MatchesCanonicalP04BrandDefinition`:** Verifies that Organization `alternateName` and `description` match canonical P04 approved copy.
7. **`Layout_WebSiteJsonLd_ExistsAndOmitsSearchAction`:** Verifies that a valid `WebSite` schema is defined with `publisher` pointing to `#organization` and zero `SearchAction` elements.
8. **`LlmsFiles_DoNotMarketPushAsCurrentOrRoadmap`:** Reads `llms.txt` and `llms-full.txt` to verify that Push is never marketed as a current or roadmap capability.
9. **`LlmsFiles_OmitAllStrictlyForbiddenClaims`:** Verifies that forbidden terms (`SOC-compliant`, `SAML`, `SSO`, `chatbot`, fixed pricing) do not appear anywhere in LLM grounding files.
10. **`SeoHelper_TitlesAndDescriptions_DoNotContainForbiddenClaims`:** Scans all metadata definitions in `SeoHelper.cs` to ensure zero occurrences of forbidden claims.
11. **`P05Documentation_DoesNotReferenceNonExistentClaimIds`:** Verifies that `P05_SEO_LLM_ENTITY_ARCHITECTURE.md` only references valid registry IDs (`CLM-001` through `CLM-020`) and never unregistered higher claim IDs.
12. **`EntityRegistry_DoesNotContainUngovernedEvidenceLabels`:** Verifies that `ENTITY_REGISTRY.md` uses the established 5-level evidence model and contains zero occurrences of `CODEBASE_VERIFIED`, `UI_VERIFIED`, or `UNCONFIRMED_CAPABILITY`.

---

## 20. Implementation Traceability & Sign-Off

| Deliverable | Source / Target File | Verification Method | Status |
| :--- | :--- | :--- | :--- |
| **SEO Master Architecture** | `docs/marketing/P05_SEO_LLM_ENTITY_ARCHITECTURE.md` | Inspection & Governance Review | SIGNED OFF |
| **Entity Registry Alignment** | `docs/marketing/ENTITY_REGISTRY.md` | 5-level evidence vocabulary & P01 redirects verified | SIGNED OFF |
| **Metadata Safety Hardening** | `Services/SeoHelper.cs` | Zero forbidden claims, homepage channel scope fixed | SIGNED OFF |
| **Layout Structured Data & Runtime NoIndex** | `Views/Shared/_Layout.cshtml` | Founding location removed, NoIndex fallback wired | SIGNED OFF |
| **LLM Grounding Context** | `wwwroot/llms.txt` & `wwwroot/llms-full.txt` | P04 canon alignment, Push quarantine & AI privacy bounds | SIGNED OFF |
| **Automated Test Guardrails**| `Pika.Web.Tests/SeoGovernanceTests.cs` | 100% dotnet test pass rate | SIGNED OFF |

---
*End of P05.1 Architecture Specification. Phase P05 is CLOSED upon successful test pass.*
