# Pika Product Truth

## Canonical Pika Definition

**Pika is a B2B SaaS Customer Intelligence & Omnichannel Marketing Platform ("Müşteri Zekâsı ve Omnichannel Pazarlama Platformu").**

### Core Foundational Axiom
> **"Pika’nın başlangıç noktası mesaj göndermek değil, anlamaktır."**  
> *(Pika's starting point is not merely sending messages; it is understanding customer, product, and transaction context.)*

Pika does not position itself as a commoditized bulk message broadcaster or a standalone dispatcher. Instead, it extracts commercial meaning from transactional data, identifies time-sensitive revenue opportunities, and orchestrates controlled, measurable actions across communication channels.

### The 6-Step Value Chain (Veriden Ölçüme Karar Zinciri)
Every capability in Pika is situated within a continuous 6-step decision and execution loop:

```
[1. DATA & INGESTION]
       │
       ▼
[2. CUSTOMER & PRODUCT INTELLIGENCE]
       │
       ▼
[3. OPPORTUNITY & DECISION (CCE / GÜNÜN FIRSATLARI)]
       │
       ▼
[4. AUDIENCE, CAMPAIGN & JOURNEY ORCHESTRATION]
       │
       ▼
[5. CHANNEL EXECUTION (EMAIL, SMS, WHATSAPP)]
       │
       ▼
[6. MEASUREMENT, BI & LEARNING LOOP]
```

1. **Veri ve Entegrasyon (Data & Ingestion):** Ingesting and validating customer profiles, product catalogs, and transactional order/invoice logs via Excel/CSV imports, automated sync, or the Ingestion REST API (`/api/v1/ingest/`).
2. **Müşteri ve Ürün Zekâsı (Customer & Product Intelligence):** Unifying customer transaction history into Pika 360, calculating lifecycle metrics, and enriching product catalogs with commercial context (Need Groups, Product Roles, Playbooks).
3. **Fırsat ve Karar (Opportunity & Decision):** Detecting actionable commercial signals through the Customer Context Engine (CCE) and Günün Fırsatları (replenishment rhythm, churn win-back, cross-sell affinity, upsell).
4. **Aksiyon ve Otomasyon (Action & Orchestration):** Translating opportunities into targeted cohorts using Audience Manager, designing multi-channel broadcasts in Campaign Manager, building event-driven automation workflows in Journey Manager, and composing creatives in Content Studio with AI assistance (Pika Pilot).
5. **Kanal Yürütme (Channel Execution):** Dispatching communications through compliant, opt-in verified channels: native Email, native SMS, and WhatsApp Business API.
6. **Ölçümleme ve BI (Measurement & BI):** Tracking delivery health, attribution, store-level performance, and feeding conversion results back into analytical models for continuous learning.

---

## Evidence Hierarchy & Repository Scope Boundary

### Repository Boundary Definition
`PikaWeb` is the public website repository containing marketing MVC views, controllers, public wiki documentation (`App_Data/wiki.json`), and internal engineering architecture documentation (`App_Data/internal_wiki.json`).

The actual operational backend services for `app.pika.tr` and `api.pika.tr` (e.g. database workers, queuing infrastructure, stream processing, external provider dispatchers) reside in external production services and repositories. **The absence of backend implementation code in PikaWeb is NOT proof of non-existence.**

To prevent governance debt, every claim and capability in this audit is rated under a strict 5-level evidence hierarchy:

### The 5 Evidence Levels

| Level | Code | Classification | Definition | Application in Audit |
| :--- | :--- | :--- | :--- | :--- |
| **Level A** | `CODE_VERIFIED` | Implementation Code Inspected | The actual implementation code exists, was inspected, and is verified directly within the `PikaWeb` repository (e.g. controllers, services, Razor views, models, configuration). | Website routes, navigation, consent token controller, SEO tags, view layouts, auth cookies. |
| **Level B** | `DOCUMENTED` | Authoritatively Documented | Supported by authoritative internal engineering or public documentation in the repository (`internal_wiki.json`, `wiki.json`) with concrete algorithms, data contracts, schemas, or worker flows, but backend implementation code resides in external services (`app.pika.tr`) and was not inspected here. | Customer Value Score formula, CCE rules, Product Intelligence resolution, Ingestion contracts, Hangfire delivery workers, AI prompt tokenization. |
| **Level C** | `MARKETING_ONLY` | Public Marketing Claim Only | Found only in marketing views, legacy files (`llms-full.txt`), or SEO copy; completely unsupported by documentation or code in this repository. | SOC-2 compliance, SAML 2.0 / SSO, automated A/B statistical winner engine, 14.2x ROAS case study. |
| **Level D** | `CONTRADICTORY` | Sources Disagree | Internal sources, code, views, or documentation conflict with one another (e.g. marketing claims a live channel, but solutions view marks it as roadmap and delivery docs omit it). | Push notifications (live in marketing vs Roadmap in `Solutions.cshtml`), real-time latency ("milliseconds" vs queue workers), office address (Ankara vs İstanbul). |
| **Level E** | `UNVERIFIED` | Insufficient Repository Evidence | Insufficient evidence across code, docs, and marketing assets. Cannot be proven or refuted from the repository alone. | Client tenant counts, real historical delivery volumes, origin of campaign conversion metrics. |

---

## Product Architecture

Based on evidence from `App_Data/internal_wiki.json`, `App_Data/wiki.json`, controllers, and services:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                      PRESENTATION & ACCESS LAYER                        │
│  - Public MVC Web: https://pika.tr (ASP.NET Core 10) [CODE_VERIFIED]    │
│  - Angular SPA App: https://app.pika.tr (/admin, /api) [DOCUMENTED]     │
│  - Knowledge Base: /wiki/ (SSR Public) & /internal/wiki/ [CODE_VERIFIED]│
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                       INGESTION & GATEWAY LAYER                         │
│  - Ingestion API: /api/v1/ingest/ (Asynchronous buffer, 202) [DOC]      │
│  - Idempotency Engine: X-Idempotency-Key (24h Redis TTL) [DOC]          │
│  - Rate Limiting: Token Bucket algorithm (1,000 req/min/tenant) [DOC]   │
│  - File Importers: Excel (.xlsx), CSV, Gmail/Outlook Contact Sync [DOC] │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                    NORMALIZATION & RESOLUTION ENGINES                   │
│  - Customer Resolution: Deduplication via SHA-256 hash, phone [DOC]     │
│  - Product Intelligence Resolution Engine: [DOCUMENTED]                 │
│      * Text sanitization & variant stripping                            │
│      * Exact & Alias matching against Master Product Catalog            │
│      * Trigram Similarity matching (> 0.85 threshold)                   │
│      * Readiness Gating (enforces Need Group & Product Role metadata)   │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                  ANALYTIC & DECISION ENGINES (CCE)                      │
│  - Customer Value Score (CVS): Weighted deterministic algorithm [DOC]   │
│      Monetary (0.40) + Frequency (0.25) + Recency (0.20) + Loyalty(0.15)│
│  - Churn & Dormancy Risk Engine: Inactivity vs individual rhythm [DOC]  │
│  - Replenishment Rhythm Engine: Consumption window (80% - 120%) [DOC]   │
│  - Cross-Sell Basket Association: Support(≥0.01), Conf(≥0.15), Lift>1.2 │
│  - Customer Context Engine (CCE) Priority Hierarchy: [DOCUMENTED]       │
│      1. Risk & Suppression Gates (Frequency Cap: 7 days, Opt-out)      │
│      2. Churn / Win-Back Opportunities                                  │
│      3. Repeat Purchase / Replenishment Opportunities                   │
│      4. Cross-Sell / Basket Affinity Opportunities                      │
│  - BI Snapshot Engine: Store, category, channel aggregations [DOC]      │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                    CAMPAIGN & JOURNEY ORCHESTRATION                     │
│  - Audience Manager: Rule tree evaluator (AND/OR logic) [DOC/CODE]      │
│  - Campaign Manager: Multi-channel scheduler, cohort binder [DOC/CODE]  │
│  - Journey Manager: Visual state-machine canvas [DOC/CODE]              │
│  - Content Studio: Drag-and-drop email builder & template store [DOC]   │
│  - Pika Pilot (AI Assistant): LLM prompt router (8s fallback, no PII)   │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                     DELIVERY & DISPATCHER WORKERS                       │
│  - Job-Attempt-Event State Machine: DeliveryJob, Attempt, Event [DOC]   │
│  - Retry Policy: Exponential backoff with jitter (Max 5 attempts) [DOC] │
│  - Dead-Letter Queue (DLQ): Automated failure capture & replay [DOC]    │
│  - Throttling & Rate Limits: SMS Gateway (50 req/sec), WhatsApp [DOC]   │
│  - Compliance Gate: Pre-dispatch IYS check, KVKK quiet hours [DOC]      │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                     SUPPORTED EXECUTION GATEWAYS                        │
│  - Email Gateway (SMTP / Dedicated Mail Delivery Engine) [DOCUMENTED]   │
│  - SMS Gateway (Commercial SMS aggregators / SMPP / HTTP) [DOCUMENTED]  │
│  - WhatsApp Business API (Meta Cloud / On-Premise BSP Gateway) [DOC]    │
│  - Push Notification Gateways: CONTRADICTORY / ROADMAP                  │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## Current Product Capabilities (Rated by Evidence Level)

### 1. Customer Intelligence & Value Scoring
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-musteri-deger-skoru-algoritmasi`, `App_Data/wiki.json: musteri-deger-skoru`)
- **Capability:** Unified customer profile aggregation; deterministic Customer Value Score based on 4-part weighted formula (0.40 Monetary, 0.25 Frequency, 0.20 Recency, 0.15 Loyalty); inactivity decay and churn risk calculation; individual replenishment rhythm tracking.

### 2. Product Intelligence (PI)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-product-intelligence-resolution-mimarisi`, `App_Data/wiki.json: product-intelligence-nedir`, `need-group-product-role`)
- **Capability:** Semantic catalog enrichment: Need Groups, Product Roles, and Commercial Playbooks. Multi-stage resolution pipeline: string sanitization, exact SKU matching, alias resolution, trigram similarity matching (> 0.85). Analytical readiness gating before feeding recommendation models.

### 3. Pika 360
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: pika-360`, `App_Data/internal_wiki.json: internal-cce-karar-motoru-mimarisi`) & `A. CODE_VERIFIED` (`Views/Platform/Pika360.cshtml`)
- **Capability:** Single-screen unified customer console displaying aggregate value metrics, transaction timeline, churn risk category, channel eligibility status, and active pending opportunities. Visualized in authentic screenshot `img_pika-360_7.png`.

### 4. Günün Fırsatları / Daily Opportunities (CCE)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-cce-karar-motoru-mimarisi`, `App_Data/wiki.json: gunun-firsatlari-ve-karar-motoru`)
- **Capability:** Algorithmic opportunity discovery engine: Repeat purchase replenishment (80-120% cycle), win-back for churn-risk customers, and cross-sell basket association (Support ≥ 0.01, Confidence ≥ 0.15, Lift > 1.2). Priority arbitration hierarchy and strict 7-day contact frequency capping.

### 5. Audience Manager (Hedef Kitle ve Segmentasyon)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: kisi-listesi-ve-segmentler`, `segment-yonetimi-ve-filtreler`) & `A. CODE_VERIFIED` (`Views/Solutions/AudienceManager.cshtml`)
- **Capability:** Dynamic and static cohort creation using multi-condition rule trees (AND/OR logic); transaction-based filtering (recency, monetary tier, store preference); channel consent gating. Visualized in authentic screenshot `img_kisi-listesi-ve-segmentler_3.png`.

### 6. Campaign Manager (Çok Kanallı Kampanya Yönetimi)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: kampanya-yoneticisi-ve-kurgular`) & `A. CODE_VERIFIED` (`Views/Solutions/CampaignManager.cshtml`)
- **Capability:** Multi-channel broadcast scheduling, audience binding, quiet hours enforcement, approval workflows, and direct transaction attribution. Visualized in authentic screenshot `img_yayinlama-sablon-ve-yonetim_27.png`.

### 7. Journey Manager (Müşteri Yolculuğu Otomasyonu)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: kampanya-journey-orkestrasyonu`) & `A. CODE_VERIFIED` (`Views/Solutions/JourneyManager.cshtml`)
- **Capability:** Visual event-driven state-machine canvas. Triggers (purchase, segment entry, inactivity); actions (channel dispatch, delays, conditional branching, goal evaluation). Visualized in authentic screenshot `img_journey-tasarim-tuvali_18.png`.

### 8. Content Studio & Email Template Editor
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: icerik-studyosu-ve-gorsel-yonetimi`) & `A. CODE_VERIFIED` (`Views/Solutions/ContentStudio.cshtml`)
- **Capability:** Drag-and-drop responsive visual email template builder, dynamic merge variables (`{{isim}}`), centralized template library (Email Store). Visualized in authentic screenshot `img_email-template-editor_17.png`.

### 9. AI Campaign Assistant (Pika Pilot)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-ai-mimarisi-ve-prompt-yonetimi`, `App_Data/wiki.json: pika-pilot-ai-kampanya-asistani`) & `A. CODE_VERIFIED` (`Views/Solutions/AiCampaignAssistant.cshtml`)
- **Capability:** Natural language campaign brief parser generating campaign title, suggested audience rule trees, channel distribution, and responsive HTML email copy drafts. Strict privacy boundary: anonymous tokenized metrics only; zero PII sent to LLM APIs; 8s fallback timeout. Visualized in screenshot `img_pika-pilot-ai-kampanya-asistani_16.png`.

### 10. Consent & Regulatory Compliance Management
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-teslimat-konsolu-ve-worker-mimarisi`, `App_Data/wiki.json: izin-optout-iys`) & `A. CODE_VERIFIED` (`Controllers/ContactConsentController.cs`, `Views/Solutions/ConsentManagement.cshtml`)
- **Capability:** Pre-dispatch IYS query and automated suppression; central KVKK opt-out DB lookup; tokenized public web preference update endpoint (`/contact-consent/{token}`) protected by Cloudflare Turnstile.

### 11. Analytics, BI & Reporting
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: bi-kokpit`, `App_Data/internal_wiki.json: internal-teslimat-konsolu-ve-worker-mimarisi`) & `A. CODE_VERIFIED` (`Views/Solutions/Reporting.cshtml`)
- **Capability:** Real-time delivery telemetry (delivered, bounced, opened, clicked, unsubscribed); BI Cockpit aggregating physical store vs online channel sales metrics and campaign revenue attribution. Visualized in authentic screenshot `img_bi-kokpit_5.png`.

### 12. Data Ingestion & Data Contracts
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-api-mimarisi-ve-veri-kontratlari`, `App_Data/wiki.json: api-entegrasyonu`, `gmail-kisi-aktarimi`)
- **Capability:** Manual Excel (.xlsx) and CSV bulk imports with schema validation; Google Contacts & Outlook contact sync; asynchronous Ingestion REST API (`/api/v1/ingest/`) with `X-Idempotency-Key` and token bucket rate limiting.

---

## Current With Limitations

1. **WhatsApp Messaging:**
   - **Evidence Level:** `B. DOCUMENTED` (`App_Data/wiki.json: kanal-operasyonlari`, `App_Data/internal_wiki.json: internal-mimari-genel-bakis`)
   - **Limitation:** Operates strictly via the official **WhatsApp Business API**. All outbound promotional and notification broadcasts require pre-approved Meta message templates and explicit opt-in consent.
   - **Marketing Rule:** Never claim arbitrary peer-to-peer scraping or unapproved broadcast automation.
2. **Contact Consent Web Form (`ContactConsentController`):**
   - **Evidence Level:** `A. CODE_VERIFIED` (`Controllers/ContactConsentController.cs`)
   - **Limitation:** The public preference portal currently captures consent checkboxes only for **Email** and **SMS** (`emailConsent`, `smsConsent`). WhatsApp opt-in is handled via incoming webhooks/imports; Push consent does not exist.
   - **Marketing Rule:** Do not claim the public token portal manages Push or WhatsApp channel preferences.
3. **Ingestion API Rate Limits:**
   - **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-api-mimarisi-ve-veri-kontratlari`)
   - **Limitation:** Default rate limit is 1,000 requests/minute per tenant with token bucket throttling.
   - **Marketing Rule:** Avoid claims of "unlimited instant throughput." Qualify with "governed enterprise ingestion."
4. **Delivery Dispatch Throttling:**
   - **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-teslimat-konsolu-ve-worker-mimarisi`)
   - **Limitation:** SMS gateway is throttled at 50 requests/second per operator contract. Delivery worker retry policy is capped at 5 attempts with exponential backoff.
   - **Marketing Rule:** Do not claim "zero latency" or "instant million-message blast."

---

## Partial Capabilities (Requires Product Confirmation)

1. **A/B Testing (`/cozumler/ab-testing`):**
   - **Evidence Level:** `C. MARKETING_ONLY` (re: automated winner selection) / `A. CODE_VERIFIED` (re: view layout)
   - **Reality in Repo:** `ABTesting.cshtml` outlines manual variant allocation. No automated multi-armed bandit winner engine or Bayesian statistical significance calculator exists in code or documentation.
   - **Governance Status:** `PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION`.
   - **Marketing Rule:** Market strictly as "Varyant ve kreatif karşılaştırma" (Variant testing). Do not claim automated statistical winner selection engines without product confirmation.
2. **Real-Time Event Processing (`/cozumler/real-time-event-processing`):**
   - **Evidence Level:** `D. CONTRADICTORY`
   - **Reality in Repo:** Webhooks and Hangfire workers process ingested events asynchronously in near real-time (seconds). Marketing copy and SEO claims "milisaniyeler içinde" (sub-millisecond streaming).
   - **Governance Status:** `PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION`.
   - **Marketing Rule:** Qualify claims to "saniyeler içinde olay bazlı tetikleme" (event-driven triggers in seconds). Never claim sub-millisecond stream processing.

---

## Roadmap Capabilities (Strictly Non-Live)

1. **Push Notifications (Web & Mobile Push):**
   - **Evidence Level:** `D. CONTRADICTORY`
   - **Evidence:** Advertised in marketing views (`CampaignManager.cshtml`) and `SeoHelper.cs:117` as live; but explicitly marked `<h3>Push Notifications (Roadmap)</h3>` in `Views/Home/Solutions.cshtml:39` and completely omitted from `internal_wiki.json` delivery workers.
   - **Governance Status:** `PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION` / `PUBLIC_WITH_QUALIFIER`.
   - **Marketing Rule:** **MUST ALWAYS CARRY A "(Roadmap)" QUALIFIER.** Never state or imply native push is currently operational.
2. **Conversational WhatsApp Chatbots (Two-Way Conversational AI):**
   - **Evidence Level:** `D. CONTRADICTORY` / `C. MARKETING_ONLY`
   - **Evidence:** Claimed in `wwwroot/llms-full.txt:66` ("chatbot flows, and two-way conversations with customers"), but documentation and architecture define strictly outbound template-based notifications via WhatsApp Business API.
   - **Governance Status:** `PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION` / `DO_NOT_MARKET`.
   - **Marketing Rule:** Prohibit conversational chatbot marketing claims. Position WhatsApp strictly as "Şablon ve bildirim kampanya yönetimi".
3. **Enterprise SSO / SAML 2.0:**
   - **Evidence Level:** `C. MARKETING_ONLY`
   - **Evidence:** Claimed in `wwwroot/llms-full.txt:121` ("SSO/SAML integration"). In PikaWeb, authentication in `Program.cs` and `AccountController.cs` is strictly cookie/JWT token-based. No SAML 2.0 middleware exists.
   - **Governance Status:** `PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION` / `DO_NOT_MARKET`.
   - **Marketing Rule:** Do not market SAML/SSO enterprise federated authentication as live without backend product confirmation.

---

## Internal-Only Systems & Artifacts

These systems and logic exist solely for internal engineering or administration and must never be exposed as customer-facing features:

1. **Internal Knowledge Base (`/internal/wiki/*`):**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - Accessible only behind `[Authorize]` cookie session; sends `X-Robots-Tag: noindex, nofollow, noarchive, nosnippet`. Contains proprietary formulas, runbooks, and architectural notes.
2. **Algorithmic Formulas, Weights & Multipliers:**
   - **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json`)
   - CVS formula weights (0.40 Monetary, 0.25 Frequency, 0.20 Recency, 0.15 Loyalty).
   - Cross-sell thresholds (Support 0.01, Confidence 0.15, Lift 1.2).
   - These are proprietary implementation details, not configurable self-service sliders for marketing users.
3. **Dead-Letter Queue (DLQ) & Hangfire Dashboards:**
   - **Evidence Level:** `B. DOCUMENTED` (`internal-teslimat-konsolu-ve-worker-mimarisi`)
   - Operator recovery tools, retry counters, and worker telemetry.
4. **Internal AI Provider Routing Rules:**
   - **Evidence Level:** `B. DOCUMENTED` (`internal-ai-mimarisi-ve-prompt-yonetimi`)
   - The 8-second fallback threshold and model provider failover architecture.

---

## Legacy & Orphan Artifacts

Deprecated, orphan, or structurally obsolete files that must not define future public pages:

1. **Orphan Legacy Pricing View (`Views/Home/Pricing.cshtml`):**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - **Governance Status:** `RETIRED / DELETED IN P01`
   - Contained hardcoded pricing packages (`₺9.900/ay`, `₺24.900/ay`, `Start / Growth / Enterprise`), features matrix, and SLA claims. Deleted from disk in Phase P01; `/fiyatlandirma` (`/en/pricing`) route permanently 301-redirects to `/demo-talebi` (`/en/demo-request`). Fixed package prices and calculators are strictly forbidden.
2. **Orphan Legacy Solutions Hub (`Views/Home/Solutions.cshtml`):**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - Unmapped View containing misleading links (e.g. linking "Journey Orchestration" to `/cozumler/personalization`).
3. **Abandoned Backup File (`Views/Solutions/JourneyManager.cshtml.bak`):**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - Leftover editor backup file on disk.
4. **Thin Prototype Solution Stubs:**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - 6 early stub pages (`personalization`, `template-management`, `ab-testing`, `deliverability-compliance`, `data-management-etl`, `real-time-event-processing`) removed from Turkish sitemap, but live in English sitemap and routes.
5. **Duplicate / Cannibalizing Solution Pages:**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - 4 duplicate pairs (`/kanallar/whatsapp-kampanya-yonetimi`, `/kanallar/email-marketing-template-studio`, `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi`, `/cozumler/e-ticaret-ai-kampanya-yonetimi`).
6. **Obsolete LLM Document (`wwwroot/llms-full.txt`):**
   - **Evidence Level:** `A. CODE_VERIFIED`
   - References obsolete URLs (`/en/solutions/email-marketing`) and unsupported claims (SOC-2, SAML, Chatbots).

---

## Commercial Model & Pricing Governance (NON-NEGOTIABLE)

### Canonical Pricing Policy
**Pika does NOT publish fixed public pricing.**

Commercial pricing is strictly quotation-based and varies according to operational and architectural factors:
- Customer / contact volume
- Transaction volume
- Messaging / channel usage (Email, SMS, WhatsApp)
- Onboarding and setup scope
- Integration requirements (ERP, POS, e-commerce, custom APIs)
- Enabled product modules
- Support / service scope (dedicated technical onboarding, account management, support coverage)
- Contract terms

### Absolute Pricing Mandates
- **Do not publish monthly package prices** (e.g. `₺X/ay`, `$Y/mo`).
- **Do not publish "starting from" prices** (e.g. `₺9.900'den başlayan fiyatlarla`).
- **Do not invent Start / Growth / Enterprise price amounts.**
- **Do not expose legacy `Views/Home/Pricing.cshtml` values** (view deleted in P01; route permanently 301-redirects to `/demo-talebi`).
- **Do not claim SLA tiers exist.**
- **Do not create pricing calculators** on the public marketing website unless explicitly approved in a future task.
- **Do not imply that all customers receive the same commercial terms.**

### Approved Public Formulations
- **Turkish:** *"İhtiyacınıza ve kullanım kapsamınıza göre özel teklif"* / *"İşletmenizin ölçeğine ve entegrasyon ihtiyaçlarına göre uyarlanan kurumsal teklif"*
- **English:** *"Custom enterprise quotation tailored to your volume, modules, and operational scope"*

### Primary CTAs for Pricing Intent
- **Turkish:** `Teklif Al`, `Demo Talep Et`, `Satış Ekibiyle Görüşün`
- **English:** `Request a Quote`, `Request a Demo`, `Talk to Sales`

---

## Channels Governance Matrix

| Channel | Product Status | Marketing Permission | Implementation Reality in Repo | Evidence Level | Constraints / Required Qualifiers |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Email** | CURRENT | PUBLIC_OK | Native SMTP / Mail delivery worker, visual editor, template store, HTML rendering. | `B. DOCUMENTED` / `A. CODE_VERIFIED` (Views) | Pre-validated sender domain (SPF/DKIM/DMARC) required. |
| **SMS** | CURRENT | PUBLIC_OK | Native SMS aggregator gateways, character calculation, opt-out handling. | `B. DOCUMENTED` / `A. CODE_VERIFIED` (Views) | 50 req/sec gateway throttling; IYS commercial messaging consent check mandatory. |
| **WhatsApp** | CURRENT | PUBLIC_OK | WhatsApp Business API integration, verified template dispatches. | `B. DOCUMENTED` / `A. CODE_VERIFIED` (Views) | Meta approval required for templates; opt-in consent mandatory; no unapproved broadcast scraping. |
| **Push** | ROADMAP | PUBLIC_WITH_QUALIFIER | No delivery worker or gateway provider in backend code. Listed as Roadmap in `Solutions.cshtml`. | `D. CONTRADICTORY` | **Must always carry "(Roadmap)" qualifier.** Forbidden: "Pika ile push gönderin", "Native push desteği". |

---

## AI Capability Boundary

Pika uses artificial intelligence strictly as an **assistive and explanatory co-pilot**, not as an unconstrained autonomous agent or black-box decision maker.

| Category | Is AI Used? | Technology / Engine | Evidence Level | Public Marketing Description |
| :--- | :--- | :--- | :--- | :--- |
| **Customer Value Score** | **NO** (Deterministic) | Mathematical weighted formula (Monetary, Frequency, Recency, Loyalty). | `B. DOCUMENTED` | Deterministic statistical scoring. Never claim "AI computes customer value." |
| **Churn / Dormancy Risk** | **NO** (Rule-Based) | Inactivity elapsed days vs customer-specific historical purchasing rhythm. | `B. DOCUMENTED` | Rule-based purchasing rhythm analysis. |
| **Cross-Sell Affinity** | **NO** (Statistical) | Association Rule Mining (Support, Confidence, Lift algorithms). | `B. DOCUMENTED` | Statistical market basket analysis. |
| **Next Best Action / Timing** | **NO** (Rule Trees) | Customer Context Engine (CCE) hierarchical arbitration & frequency caps. | `B. DOCUMENTED` | Contextual decision rules and priority trees. |
| **AI Müşteri Özeti** | **YES** (LLM) | Summarizes normalized customer metrics into concise natural language Turkish paragraphs. | `B. DOCUMENTED` | "AI-powered customer behavior summary." |
| **AI Campaign Assistant (Pika Pilot)** | **YES** (LLM) | Converts natural language prompt into campaign draft, recommended cohort rules, and email template. | `B. DOCUMENTED` / `A. CODE_VERIFIED` | "AI Campaign Co-pilot (Taslak ve içerik asistanı)." |
| **Content Copywriting** | **YES** (LLM) | Subject line and message text generation within Content Studio. | `B. DOCUMENTED` | "AI copy generation and variation assistant." |

### Absolute AI Boundary Rules
1. **Zero Autonomous Dispatch:** AI never initiates or sends a campaign autonomously. Human review and explicit approval are mandatory (`Faq.cshtml:325`).
2. **Zero-PII Guarantee:** No PII (TCKN, phone numbers, real customer names) is ever submitted to LLM APIs. Only anonymized, tokenized behavioral parameters are used (`internal-ai-mimarisi-ve-prompt-yonetimi:2`).
3. **Deterministic Fallback:** If an LLM provider fails or times out (> 8s), Pika gracefully falls back to deterministic default templates.

---

## Data & Integration Capability Boundary

1. **Ingestion Modalities:**
   - Batch file uploads: Excel (`.xlsx`) and CSV with dynamic column mapping (`B. DOCUMENTED`).
   - REST API: Asynchronous buffer endpoint (`/api/v1/ingest/`) returning `202 Accepted` (`B. DOCUMENTED`).
   - Contact Import: Automated Gmail & Outlook address book sync (`/wiki/gmail-kisi-aktarimi`) (`B. DOCUMENTED`).
2. **Contract Safeguards:**
   - Mandatory `X-Idempotency-Key` prevents duplicate transaction billing/ingestion (`B. DOCUMENTED`).
   - Token bucket rate limiter caps traffic at 1,000 requests/min per tenant (`B. DOCUMENTED`).
3. **Data Normalization:**
   - Identity resolution merges customer records across phone, email, and external customer ID (`B. DOCUMENTED`).
   - Product master matching maps vendor SKUs to unified Need Groups and Product Roles (`B. DOCUMENTED`).

---

## Consent / Compliance Boundary

1. **IYS (İleti Yönetim Sistemi):**
   - Outbound commercial messages are checked against IYS consent databases before dispatch. Unapproved contacts are automatically suppressed (`B. DOCUMENTED`).
2. **KVKK / GDPR:**
   - Opt-out requests (SMS STOP, email unsubscribe) update central suppression lists immediately (`B. DOCUMENTED`).
   - Public token-based consent portal allows recipients to manage channel permissions (`A. CODE_VERIFIED`).
3. **Legal Responsibility Disclaimer:**
   - Pika provides technical compliance workflows and audit trails, but does not provide legal indemnity. Businesses remain legally responsible for their own opt-in collection and marketing permissions.

---

## Analytics / Measurement Boundary

1. **Direct Revenue Attribution:**
   - Pika attributes order volume and turnover directly to campaigns that influenced the transaction within defined attribution windows (`B. DOCUMENTED`).
2. **BI Cockpit:**
   - Provides physical store vs online channel sales metrics, category performance comparisons, and campaign ROI baselines (`B. DOCUMENTED`).
3. **Observability:**
   - Full delivery telemetry tracking attempts, gateway status codes, bounces, opens, and clicks (`B. DOCUMENTED`).

---

## Product Evidence Index

| Entity / Concept | Primary Repository Evidence | Secondary Evidence | Evidence Level |
| :--- | :--- | :--- | :--- |
| **Pika Definition** | `App_Data/wiki.json: pika-nedir`, `pika-ne-degildir`, `pika-nasil-calisir` | `wwwroot/llms.txt: lines 1-16` | `B. DOCUMENTED` |
| **6-Step Decision Cycle** | `App_Data/wiki.json: pika-nasil-calisir` | `wwwroot/llms.txt: lines 8-15` | `B. DOCUMENTED` |
| **Ingestion API & Rate Limits** | `App_Data/internal_wiki.json: internal-api-mimarisi-ve-veri-kontratlari` | `internal-mimari-genel-bakis` | `B. DOCUMENTED` |
| **Customer Value Score** | `App_Data/internal_wiki.json: internal-musteri-deger-skoru-algoritmasi` | `App_Data/wiki.json: musteri-deger-skoru` | `B. DOCUMENTED` |
| **CCE Decision Engine** | `App_Data/internal_wiki.json: internal-cce-karar-motoru-mimarisi` | `App_Data/wiki.json: gunun-firsatlari-ve-karar-motoru` | `B. DOCUMENTED` |
| **Product Intelligence** | `App_Data/internal_wiki.json: internal-product-intelligence-resolution-mimarisi`| `App_Data/wiki.json: product-intelligence-nedir` | `B. DOCUMENTED` |
| **Cross-Sell Basket Mining** | `App_Data/internal_wiki.json: internal-cross-sell-sepet-analizi-motoru` | `App_Data/wiki.json: cross-sell-firsatlari` | `B. DOCUMENTED` |
| **Delivery Workers & DLQ** | `App_Data/internal_wiki.json: internal-teslimat-konsolu-ve-worker-mimarisi` | `internal-retry-politikasi-ve-hata-yonetimi` | `B. DOCUMENTED` |
| **AI LLM Routing & Privacy**| `App_Data/internal_wiki.json: internal-ai-mimarisi-ve-prompt-yonetimi` | `App_Data/wiki.json: ai-rolu-guven-siniri` | `B. DOCUMENTED` |
| **Public Consent Web Form** | `Controllers/ContactConsentController.cs` | `Views/ContactConsent/Index.cshtml` | `A. CODE_VERIFIED` |
| **Push Status (Roadmap)** | `Views/Home/Solutions.cshtml: line 39` | Absence from `internal_wiki.json` delivery layer | `D. CONTRADICTORY` |
| **Real Product Screenshots** | `wwwroot/wiki/assets/images/*.png` (29 files) | `Views/Solutions/AiCampaignAssistant.cshtml: line 107` | `A. CODE_VERIFIED` |
| **Public Routes & Views** | 10 Controllers, 59 Razor Views, `Program.cs` | Route table in `PUBLIC_ROUTE_AUDIT.md` | `A. CODE_VERIFIED` |
