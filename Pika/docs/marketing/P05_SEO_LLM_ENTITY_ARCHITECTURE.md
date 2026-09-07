# Pika Web 2.0 — P05: SEO + LLM Entity Architecture & Metadata Safety Convergence

---

## 1. Executive Summary & Epistemic Contract

### 1.1 Purpose of Phase P05
Phase P05 establishes the authoritative **SEO Entity Architecture**, **LLM Discoverability Architecture**, and **Public Metadata Safety Contract** for the Pika Web 2.0 platform. Building upon the foundational stages—Product Truth (P00), Information Architecture (P01), Visual Constitution (P02), Authentic Visual System (P03), and Content Bible (P04)—P05 ensures that external search engines, semantic web crawlers, generative AI retrieval engines, and LLM answer systems perceive exactly the same governed commercial entity that Pika's product reality and Content Bible define.

### 1.2 Epistemic Governance Principles
1. **Convergence on Product Reality:** Public search snippets, meta descriptions, Open Graph cards, structured data, and LLM text files must strictly reflect verified product capabilities. No marketing hype, speculative features, or unsupported compliance certifications may enter machine-readable surfaces.
2. **Strict Adherence to Claims Registry:** Every public claim made in HTML head metadata or LLM documents maps directly to `CLAIMS_REGISTRY.md` (CLM-001 through CLM-030). Claims marked `DO_NOT_MARKET` or `NEEDS_PRODUCT_CONFIRMATION` are categorically prohibited from SEO metadata.
3. **Push Governance Strictness:** Push notifications are classified as `CONTRADICTORY / UNCONFIRMED`. Push is excluded from all public navigation, sitemaps, structured data, and capability matrices until explicit product-owner confirmation. Future confirmation may alter its status, but it must never be represented as a current or roadmap capability today.
4. **Separation of Concerns:** P05 governs machine-readable metadata, search indexation contracts, structured data, and LLM ingestible files. It strictly refrains from redesigning views, altering public page bodies, modifying CSS, or restructuring routes.

---

## 2. Canonical Entity Graph

### 2.1 Core Entities & Attributes
The semantic web model of Pika is organized into distinct, hierarchically governed entities:

```
[ Pika: SoftwareApplication / Organization ]
  │
  ├── [ Customer Intelligence ] (Analytical Layer)
  │     ├── Customer Value Score (CVS) (0-100 Metric)
  │     ├── Purchasing Cycle (%80-%120 Consumption Rhythm)
  │     └── Churn Risk Classification
  │
  ├── [ Product Intelligence ] (Catalog Enrichment)
  │     ├── Need Groups (Consumer Motivation)
  │     ├── Product Roles (Traffic Driver, Basket Booster, Margin)
  │     └── Market Basket Affinity (Support, Confidence, Lift)
  │
  ├── [ Opportunity & Decision Engine (CCE) ]
  │     └── Günün Fırsatları (Daily Commercial Opportunities)
  │
  ├── [ Engagement & Orchestration ]
  │     ├── Audience Manager (Dynamic Segment Builder)
  │     ├── Campaign Manager (Multi-Channel Broadcast Hub)
  │     ├── Journey Manager (Visual Workflow Canvas)
  │     ├── Content Studio (Drag-and-Drop Designer)
  │     └── AI Campaign Assistant (Pika Pilot - Draft Generator)
  │
  ├── [ Execution Channels ]
  │     ├── Email (Transactional & Broadcast)
  │     ├── SMS (Operator Gateways & Rate Limiter)
  │     └── WhatsApp (Official Meta WhatsApp Business API)
  │
  └── [ Compliance & Governance ]
        ├── Consent Management (İYS & KVKK Sync)
        ├── Delivery Telemetry & Attribution (BI Kokpit)
        └── Role-Based Access Control (RBAC)
```

### 2.2 Entity Relationship Constraints
- **Channel Execution Subordination:** Channels (Email, SMS, WhatsApp) do not operate in isolation; they are execution endpoints driven by Audience Manager cohorts or Journey Manager event triggers.
- **AI Positioning:** The AI Campaign Assistant (*Pika Pilot*) is strictly an assistive copilot requiring mandatory human approval. It consumes calculated contextual attributes (not raw PII) and assists with draft generation; it is never marketed as an autonomous marketing agent.
- **Push Quarantine:** Push is isolated from the active entity graph. It exists solely as an unindexed legacy reference route.

---

## 3. Category, Brand & Proposition Matrix

### 3.1 Canonical Category Definition
- **Turkish (TR):** `Müşteri Zekâsı ve Omnichannel Pazarlama Platformu`  
  *(Status: APPROVED_COPY | Source: PRODUCT_OWNER_APPROVED_CANON)*
- **English (EN):** `Customer Intelligence & Omnichannel Marketing Platform`  
  *(Status: APPROVED_COPY | Source: PRODUCT_OWNER_APPROVED_CANON)*

### 3.2 Canonical Brand Ideas & Anchors
| Dimension | Turkish (TR) | English (EN) | Governance Status |
| :--- | :--- | :--- | :--- |
| **Brand Idea** | "Daha çok mesaj değil, daha doğru ilişki." | "Not more messages. Better customer relationships." | APPROVED_COPY |
| **Brand Anchor** | Doğru Anda. | At the Right Moment. | APPROVED_COPY |
| **Core Axiom** | "Pika’nın başlangıç noktası mesaj göndermek değil, anlamaktır." | "Pika’s starting point is not merely sending messages; it is understanding customer, product, and transaction context." | APPROVED_COPY |

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
- `/cozumler/template-management` → 301 Permanent Redirect → `/cozumler/audience-manager`
- `/cozumler/ab-testing` → 301 Permanent Redirect → `/cozumler/campaign-manager`
- `/cozumler/deliverability-compliance` → 301 Permanent Redirect → `/cozumler/consent-management`
- `/cozumler/real-time-event-processing` → 301 Permanent Redirect → `/entegrasyonlar`

### 4.3 Noindex Quarantined Routes
- `/kanallar/push` and `/en/channels/push`: Marked with `noindex, follow` and excluded from `sitemap.xml` because Push is `CONTRADICTORY / UNCONFIRMED`.

---

## 5. Hreflang & Multi-Language Architecture

### 5.1 Symmetric Pairing Contract
Every canonical indexable route must emit three symmetric `<link rel="alternate">` tags:
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
Non-canonical, redirected, or noindexed routes (such as `/kanallar/push`) are excluded from public hreflang cross-indexing to prevent crawl budget waste and conflicting indexation directives.

---

## 6. Structured Data (JSON-LD) Specification

### 6.1 Organization Schema
Placed globally on all pages within `Views/Shared/_Layout.cshtml`:
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
Placed globally on all pages within `Views/Shared/_Layout.cshtml`:
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
Injected conditionally on all non-root pages:
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

### 6.4 SoftwareApplication Schema (Guidance)
When deployed on product and solution landing pages:
- `applicationCategory`: `BusinessApplication`
- `operatingSystem`: `Web-based, Cloud SaaS`
- `offers`: Must point to the quotation request URL (`https://pika.tr/demo-talebi`) with `priceCurrency: "TRY"` and `priceSpecification` noting quote-based commercial terms. No fake or hardcoded subscription tiers.

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
- Every page emits an absolute canonical tag: `<link rel="canonical" href="https://pika.tr{path}" />`
- Standard indexable pages emit: `<meta name="robots" content="index, follow" />`
- Quarantined pages emit: `<meta name="robots" content="noindex, follow" />`

### 7.4 Open Graph & Twitter Cards
- `og:type`: `website`
- `og:site_name`: `Pika`
- `og:url`: Canonical absolute URL
- `og:image`: `https://pika.tr/web/assets/images/og-default.png` (1200x630px)
- `twitter:card`: `summary_large_image`

---

## 8. Page-by-Page Metadata Contract

The following table documents the authoritative metadata contract implemented in `Services/SeoHelper.cs`:

| Route (TR / EN) | Title (TR / EN) | Primary Entity | Secondary Entities | Index Status |
| :--- | :--- | :--- | :--- | :--- |
| `/` / `/en/` | Pika - Müşteri Zekâsı ve Omnichannel Pazarlama Platformu / Customer Intelligence & Omnichannel Marketing Platform | Pika Platform | All Modules | index, follow |
| `/pika` / `/en/pika` | Pika Nedir? - Müşteri Zekâsı ve Pazarlama Platformu / What is Pika? - Customer Intelligence & Marketing Platform | Pika Platform | Intelligence, CCE | index, follow |
| `/platform/customer-intelligence` / `/en/platform/customer-intelligence` | Müşteri Zekâsı - Müşteri Değeri ve Tüketim Ritmi Analitiği / Customer Intelligence - Value & Consumption Rhythms | Customer Intelligence | CVS, Churn Risk, Pika 360 | index, follow |
| `/platform/product-intelligence` / `/en/platform/product-intelligence` | Ürün Zekâsı - İhtiyaç Grupları ve Ürün Rolleri / Product Intelligence - Need Groups & Product Roles | Product Intelligence | Need Group, Product Role, Cross-sell | index, follow |
| `/platform/pika-360` / `/en/platform/pika-360` | Pika 360 - Bütünleşik Müşteri Profili ve Aksiyon Konsolu / Pika 360 - Unified Customer Profile & Action Cockpit | Pika 360 | Customer Profile, CVS | index, follow |
| `/platform/gunun-firsatlari` / `/en/platform/opportunities` | Günün Fırsatları - Karar Motoru ile Proaktif Kampanyalar / Daily Opportunities - Decision Engine for Campaigns | Günün Fırsatları | CCE, Repeat Purchase, Cross-sell | index, follow |
| `/cozumler/campaign-manager` / `/en/solutions/campaign-manager` | Campaign Manager - Çok Kanallı Kampanya Yönetimi / Campaign Manager - Multi-Channel Campaign Orchestration | Campaign Manager | Email, SMS, WhatsApp, Attribution | index, follow |
| `/cozumler/audience-manager` / `/en/solutions/audience-manager` | Audience Manager - Kural Bazlı Dinamik Segmentasyon / Audience Manager - Dynamic Rule-Based Segmentation | Audience Manager | Segments, RFM, Dynamic Rules | index, follow |
| `/cozumler/journey-manager` / `/en/solutions/journey-manager` | Journey Manager - Otomatik Müşteri Yolculukları / Journey Manager - Customer Journey Automation | Journey Manager | Automation Canvas, Triggers | index, follow |
| `/cozumler/content-studio` / `/en/solutions/content-studio` | Content Studio - Sürükle Bırak Şablon Tasarımı / Content Studio - Drag-and-Drop Template Designer | Content Studio | Email Editor, Templates | index, follow |
| `/kanallar/email` / `/en/channels/email` | E-Posta Pazarlama - Şablon Yönetimi ve Teslimat Altyapısı / Email Marketing - Template Management & Delivery Infrastructure | Email Marketing | Content Studio, Campaign Manager | index, follow |
| `/kanallar/sms` / `/en/channels/sms` | SMS Kampanyaları - Hız Sınırlandırmalı Operatör İletimi / SMS Campaigns - Rate-Limited Operator Delivery | SMS Campaigns | Delivery Workers, Rate Limiter | index, follow |
| `/kanallar/whatsapp` / `/en/channels/whatsapp` | WhatsApp Business API - Onaylı Şablon ve Bildirimler / WhatsApp Business API - Approved Templates & Messaging | WhatsApp Messaging | WhatsApp API, Consent Management | index, follow |
| `/cozumler/analytics-reporting` / `/en/solutions/analytics-reporting` | Analitik ve Raporlama - Teslimat Telemetrisi ve Ciro Atfı / Analytics & Reporting - Delivery Telemetry & Attributed Revenue | Analytics & Reporting | BI Kokpit, Attribution | index, follow |
| `/cozumler/consent-management` / `/en/solutions/consent-management` | İzin ve Tercih Yönetimi - İYS ve KVKK Entegrasyonu / Consent & Preference Management - IYS & KVKK Integration | Consent Management | IYS, KVKK, Opt-out DB | index, follow |
| `/entegrasyonlar` / `/en/integrations` | Entegrasyonlar - API, Webhook ve Veri Aktarımı / Integrations - API, Webhooks & Data Import | Integrations | Ingestion API, CSV/Excel | index, follow |
| `/guvenlik-ve-gizlilik` / `/en/security-and-privacy` | Güvenlik ve Gizlilik - Rol Bazlı Erişim ve Veri Koruma / Security & Privacy - RBAC & Data Protection | Security & Privacy | RBAC, Audit Logs, Consent | index, follow |
| `/urunler/ai-kampanya-asistani` / `/en/products/ai-campaign-assistant` | AI Kampanya Asistanı - Pika Pilot ile Akıllı İçerik / AI Campaign Assistant - Smart Briefs with Pika Pilot | AI Campaign Assistant | Pika Pilot, Content Studio | index, follow |
| `/demo-talebi` / `/en/demo-request` | Demo Talebi - Pika Müşteri Zekâsı Platformu / Demo Request - Pika Customer Intelligence Platform | Demo Request | Consultation, Custom Quote | index, follow |
| `/iletisim` / `/en/contact` | İletişim - Satış ve Destek Ekibiyle Görüşün / Contact - Reach Our Sales & Support Team | Contact | Sales, Support | index, follow |
| `/kaynaklar/sss` / `/en/resources/faq` | Sık Sorulan Sorular - Pika Hakkında Merak Edilenler / Frequently Asked Questions - Learn More About Pika | FAQ | Onboarding, Integrations, Pricing | index, follow |
| `/kullanim-senaryolari` / `/en/use-cases` | Kullanım Senaryoları - Sektörel Çözümler ve Kurgular / Use Cases - Industry Solutions & Workflow Playbooks | Use Cases | Retail, E-Commerce | index, follow |
| `/kanallar/push` / `/en/channels/push` | Push Bildirimleri - Davranış Bazlı Bildirim Kurguları / Push Notifications - Behavior-Triggered Workflows | Push Notifications | Quarantined / Unconfirmed | noindex, follow |

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
- **Push Governance Invariant:** Push is explicitly clarified as:
  > *(Note: Push is CONTRADICTORY / UNCONFIRMED and is excluded from public channel/capability signals until explicit product-owner confirmation. Future confirmation may change its governance state. It must not be inferred as a live capability.)*

### 9.3 Anti-Hallucination Guardrails for LLM Grounding
1. **No Autonomous AI Bot Claims:** LLM consumers must understand that Pika Pilot provides assistive draft generation requiring human sign-off; it does not independently dispatch messages.
2. **No Real-Time Millisecond Guarantees:** Data ingestion is asynchronous and batch-optimized; avoid claiming sub-second real-time streaming.
3. **Deterministic Intelligence:** Customer Value Score and RFM metrics are computed via deterministic mathematical formulas, not opaque deep learning black boxes.

---

## 10. Claim-Safety in Public Metadata

The following table maps key Claims Registry items to metadata enforcement:

| Claim ID | Claim Theme | Governance Status | Metadata Enforcement Rule |
| :--- | :--- | :--- | :--- |
| **CLM-001** | Category Definition | PUBLIC_OK | Must use "Müşteri Zekâsı ve Omnichannel Pazarlama Platformu" |
| **CLM-002** | Brand Idea | PUBLIC_OK | Use "Daha çok mesaj değil, daha doğru ilişki" / "Doğru Anda" |
| **CLM-003** | Core Promise | PUBLIC_OK | Understand, Discover, Act |
| **CLM-004** | Push Notifications | CONTRADICTORY / UNCONFIRMED | Exclude from indexable titles/metas; mark route `noindex`; omit from channel lists |
| **CLM-005** | Pricing Model | PUBLIC_OK | Quote-based only; no public subscription tiers or prices |
| **CLM-006** | AI Capabilities | PUBLIC_OK | Assistive draft generation; mandatory human approval; privacy bounds |
| **CLM-007** | WhatsApp Bot / 2-way | DO_NOT_MARKET | Never describe as chatbot, two-way conversational agent, or support bot |
| **CLM-008** | Fixed Demo Duration | UNCONFIRMED | Removed "15-minute demo"; use personalized consultation/demo |
| **CLM-009** | Millisecond Latency | CONTRADICTORY | Removed "in milliseconds" / "milisaniyeler içinde"; state asynchronous ingestion |
| **CLM-011** | HMAC Authentication | CONTRADICTORY / UNCONFIRMED | Removed HMAC claim; state API-key authentication and idempotency |
| **CLM-012** | High Deliverability | UNCONFIRMED | Removed "yüksek teslimat garantisi"; state delivery management infrastructure |
| **CLM-016** | SAML / SSO | UNCONFIRMED | Removed from metadata; state Role-Based Access Control (RBAC) |
| **CLM-017** | SOC-Compliant | CONTRADICTORY / FORBIDDEN | Completely omitted from all metadata, structured data, and LLM text |
| **CLM-020** | Founding Location | CONTRADICTORY | Omitted from Organization JSON-LD (do not output İstanbul or Ankara) |
| **CLM-025** | Direct Causal Revenue | CONTRADICTORY | Replace "doğrudan ciro atfı" with "ilişkilendirilen ciro atfı" (attributed revenue) |

---

## 11. Channel Governance

### 11.1 Confirmed Channels
1. **Email:** Drag-and-drop template composition, responsive layouts, dynamic personalization tokens, delivery status machine.
2. **SMS:** Rate-limited queue dispatcher, operator gateway delivery, automated opt-out / cancellation handling.
3. **WhatsApp:** Official Meta WhatsApp Business API integration, pre-approved message templates, HSM outbound notifications.

### 11.2 Push Status & Quarantine Contract
- **Governance State:** `CONTRADICTORY / UNCONFIRMED`.
- **Public Signal Exclusion:** Excluded from header/footer navigation, canonical sitemaps, active marketing collateral, and platform channel summaries.
- **Indexation Directives:** If the legacy route `/kanallar/push` is requested, it serves with `<meta name="robots" content="noindex, follow" />` and is excluded from hreflang alternate lists.
- **Future Re-evaluation:** If the product owner validates push notification infrastructure in a future phase, it may only be promoted after formal evidence update in `PRODUCT_TRUTH.md` and `CLAIMS_REGISTRY.md`.

---

## 12. Integration & Developer Surface Architecture

### 12.1 Ingestion API
- **Endpoint:** `/api/v1/ingest/`
- **Methodology:** Asynchronous REST ingestion designed for transactional batching.
- **Authentication:** Token-based API keys (`X-API-Key` header) with payload idempotency keys.
- **Payload Entities:** Customers (`/customers`), Orders/Transactions (`/transactions`), Catalog/Products (`/products`).

### 12.2 File-Based Data Exchange
- Built-in CSV and Excel upload workers for offline retail POS systems, legacy ERP exports, and segmented list synchronizations.

### 12.3 Outbound Webhooks & Delivery Telemetry
- Event-driven dispatch callbacks reporting delivery status, bounce events, and unsubscribe triggers.

---

## 13. Security, Privacy & Compliance Messaging Contract

### 13.1 Regulatory Compliance
- **KVKK Compliance:** Native consent tracking, explicit marketing opt-in logging, customer data anonymization workflows.
- **İYS (İleti Yönetim Sistemi) Integration:** Synchronized opt-in/opt-out status checking prior to dispatching commercial electronic messages.

### 13.2 Technical Security Safeguards
- **Role-Based Access Control (RBAC):** Tiered permissions separating campaign creators, analysts, and administrative managers.
- **Transport Security:** Mandatory TLS/HTTPS encryption across all public and API endpoints.
- **Audit Logging:** System-level audit trails for customer data exports and bulk list modifications.

### 13.3 Forbidden Security Claims
- **No SOC-2 / SOC-3 Claims:** The platform has not undergone public SOC certification audits.
- **No SAML 2.0 / Enterprise SSO Claims:** Current authentication uses ASP.NET Core Identity with role management. Enterprise SSO is unconfirmed.

---

## 14. Commercial & Pricing Model Discoverability

### 14.1 Tailored Quotation Model
Pika does not sell packaged software off the shelf. Every implementation is sized and quoted individually based on enterprise metrics:
1. Contact & Customer Profile Count
2. Monthly Transaction Volume
3. Outbound Message Dispatch Volume (SMS, WhatsApp, Email)
4. Integration Complexity (Custom ERP/POS APIs vs standard connectors)
5. Service & Dedicated Support Level Agreements (SLA)

### 14.2 Zero Public Pricing Tiers
- Search engines and LLMs must be explicitly guided: **No fixed public pricing, tiers, or self-checkout carts exist.**
- The primary commercial conversion path is the **Demo & Quote Request** (`/demo-talebi` / `/en/demo-request`).

---

## 15. Sitemap & Indexation Governance

### 15.1 `sitemap.xml` Rules
1. **Canonical URLs Only:** Only canonical, indexable 200 OK URLs are listed.
2. **Exclusion of Redirects:** Former stubs (`/cozumler/personalization`, etc.) are 301 redirects and must NEVER appear in the sitemap.
3. **Exclusion of Noindex Pages:** `/kanallar/push` and `/en/channels/push` are excluded from the sitemap.
4. **Symmetric TR/EN Inclusion:** Both Turkish and English URLs are present with identical modification dates.

### 15.2 Crawl Budget & Priority Governance
- Root Homepage (`/`, `/en/`): `priority = 1.0`
- Core Platform Hubs (`/pika`, `/platform/*`): `priority = 0.9`
- Solutions & Channels (`/cozumler/*`, `/kanallar/*`): `priority = 0.8`
- Supporting Resources & Legal (`/kaynaklar/*`, `/guvenlik-ve-gizlilik`): `priority = 0.6`

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

The following anti-patterns are strictly banned across all SEO metadata, Open Graph cards, structured data, and LLM text:

| Banned Anti-Pattern | Reason for Ban | Correct Governed Alternative |
| :--- | :--- | :--- |
| Mentioning "Push" as an active channel | Push is UNCONFIRMED / CONTRADICTORY | Confirmed channels: Email, SMS, WhatsApp |
| "15 dakikalık demo" / "15-minute demo" | Demo timing is unconfirmed | "Kapsamlı platform demosu" / "Tailored demo" |
| "Milisaniyeler içinde veri işleme" | Ingestion is asynchronous / batch-oriented | "Asenkron veri işleme ve API entegrasyonu" |
| "Doğrudan ciro artışı garantisi" | Outcome causality cannot be guaranteed | "Kampanya etkileşimi ve ilişkilendirilen ciro atfı" |
| "SOC-compliant altyapı" | Audit certification unverified | "Rol bazlı erişim denetimi ve güvenli altyapı" |
| "SAML 2.0 / Enterprise SSO" | Not implemented in codebase | "Rol bazlı yetkilendirme (RBAC)" |
| "WhatsApp chatbotu / 2-way AI" | Two-way chatbotting not supported | "Meta onaylı şablonlarla kurumsal bildirimler" |
| "Aylık 999 TL'den başlayan fiyatlar" | Pika has zero public pricing packages | "İhtiyaca özel teklif modeli / Demo Talebi" |
| "foundingLocation: İstanbul" in JSON-LD | Founding city has contradictory evidence | Completely omit `foundingLocation` property |

---

## 19. Automated Verification & Testing Contract

To ensure that the SEO entity architecture and metadata safety rules cannot regress during future development, the following suite of automated tests is enforced in `Pika.Web.Tests/SeoGovernanceTests.cs`:

1. **`Hreflang_Pairs_AreSymmetricAcrossAllIndexedCanonicalPages`:** Validates that every canonical indexable page in `SeoHelper.AllPages` has matching `AlternatePathTr` and `AlternatePathEn` entries with an `x-default` fallback.
2. **`PushNotifications_IsMarkedNoindexAndExcludedFromIndex`:** Asserts that `Solutions.PushNotifications` is flagged `NoIndex = true` and `RobotsMeta` contains `noindex`.
3. **`Layout_OrganizationJsonLd_OmitsFoundingLocation`:** Inspects `_Layout.cshtml` to ensure that neither `foundingLocation`, `İstanbul`, nor `Ankara` are present in the Organization schema.
4. **`Layout_OrganizationJsonLd_MatchesCanonicalP04BrandDefinition`:** Verifies that Organization `alternateName` and `description` match canonical P04 approved copy.
5. **`Layout_WebSiteJsonLd_ExistsAndOmitsSearchAction`:** Verifies that a valid `WebSite` schema is defined with `publisher` pointing to `#organization` and zero `SearchAction` elements.
6. **`LlmsFiles_DoNotMarketPushAsCurrentOrRoadmap`:** Reads `llms.txt` and `llms-full.txt` to verify that Push is never marketed as a current or roadmap capability.
7. **`LlmsFiles_OmitAllStrictlyForbiddenClaims`:** Verifies that forbidden terms (`SOC-compliant`, `SAML`, `SSO`, `chatbot`, fixed pricing) do not appear anywhere in LLM grounding files.
8. **`SeoHelper_TitlesAndDescriptions_DoNotContainForbiddenClaims`:** Scans all metadata definitions in `SeoHelper.cs` to ensure zero occurrences of forbidden claims.

---

## 20. Implementation Traceability & Sign-Off

| Deliverable | Source / Target File | Verification Method | Status |
| :--- | :--- | :--- | :--- |
| **SEO Master Architecture** | `docs/marketing/P05_SEO_LLM_ENTITY_ARCHITECTURE.md` | Inspection & Governance Review | SIGNED OFF |
| **Entity Registry Alignment** | `docs/marketing/ENTITY_REGISTRY.md` | Status, Public Use, Evidence columns verified | SIGNED OFF |
| **Metadata Safety Hardening** | `Services/SeoHelper.cs` | Zero forbidden claims, corrected fallbacks | SIGNED OFF |
| **Layout Structured Data** | `Views/Shared/_Layout.cshtml` | Founding location removed, WebSite JSON-LD added | SIGNED OFF |
| **LLM Grounding Context** | `wwwroot/llms.txt` & `wwwroot/llms-full.txt` | P04 canon alignment, Push quarantine enforced | SIGNED OFF |
| **Automated Test Guardrails**| `Pika.Web.Tests/SeoGovernanceTests.cs` | 100% dotnet test pass rate | SIGNED OFF |

---
*End of P05 Architecture Specification. Phase P05 is CLOSED upon successful test pass.*
