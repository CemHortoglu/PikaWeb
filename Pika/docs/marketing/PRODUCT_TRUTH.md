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

## Product Architecture

Based on evidence from `App_Data/internal_wiki.json`, `App_Data/wiki.json`, controllers, and services:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                      PRESENTATION & ACCESS LAYER                        │
│  - Public MVC Web: https://pika.tr (ASP.NET Core 10)                    │
│  - Angular SPA App: https://app.pika.tr (/admin, /api/Users/Login)      │
│  - Knowledge Base: /wiki/ (SSR Public) & /internal/wiki/ (Auth-only)   │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                       INGESTION & GATEWAY LAYER                         │
│  - Ingestion API: /api/v1/ingest/ (Asynchronous buffer, 202 Accepted)   │
│  - Idempotency Engine: X-Idempotency-Key (24h Redis/MemoryCache TTL)    │
│  - Rate Limiting: Token Bucket algorithm (Default 1,000 req/min/tenant) │
│  - File Importers: Excel (.xlsx), CSV, Gmail/Outlook Contact Sync       │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                    NORMALIZATION & RESOLUTION ENGINES                   │
│  - Customer Resolution: Deduplication via SHA-256 hash, phone, email   │
│  - Product Intelligence Resolution Engine:                              │
│      * Text sanitization & variant stripping                            │
│      * Exact & Alias matching against Master Product Catalog            │
│      * Trigram Similarity matching (> 0.85 threshold)                   │
│      * Readiness Gating (enforces Need Group & Product Role metadata)   │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                  ANALYTIC & DECISION ENGINES (CCE)                      │
│  - Customer Value Score (CVS): Weighted deterministic algorithm:        │
│      Monetary (0.40) + Frequency (0.25) + Recency (0.20) + Loyalty(0.15)│
│  - Churn & Dormancy Risk Engine: Inactivity vs individual rhythm decay  │
│  - Replenishment Rhythm Engine: Consumption window (80% - 120% cycle)   │
│  - Cross-Sell Basket Association: Support (≥0.01), Conf (≥0.15), Lift>1.2│
│  - Customer Context Engine (CCE) Priority Hierarchy:                    │
│      1. Risk & Suppression Gates (Frequency Cap: 7 days, Opt-out)      │
│      2. Churn / Win-Back Opportunities                                  │
│      3. Repeat Purchase / Replenishment Opportunities                   │
│      4. Cross-Sell / Basket Affinity Opportunities                      │
│  - BI Snapshot Engine: Store, category, and sales channel aggregations  │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                    CAMPAIGN & JOURNEY ORCHESTRATION                     │
│  - Audience Manager: Rule tree evaluator (AND/OR logic, dynamic/static) │
│  - Campaign Manager: Multi-channel scheduler, target cohort binder      │
│  - Journey Manager: Visual state-machine canvas (triggers, delays, if)  │
│  - Content Studio: Visual drag-and-drop email builder & template store  │
│  - Pika Pilot (AI Assistant): LLM prompt router (8s fallback, no PII)   │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                     DELIVERY & DISPATCHER WORKERS                       │
│  - Job-Attempt-Event State Machine: DeliveryJob, DeliveryAttempt, Event │
│  - Retry Policy: Exponential backoff with jitter (Max 5 attempts)       │
│  - Dead-Letter Queue (DLQ): Automated failure capture & manual replay   │
│  - Throttling & Rate Limits: SMS Gateway (50 req/sec), WhatsApp API     │
│  - Compliance Gate: Pre-dispatch IYS check, KVKK consent & quiet hours │
└────────────────────────────────────┬────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼────────────────────────────────────┐
│                     SUPPORTED EXECUTION GATEWAYS                        │
│  - Email Gateway (SMTP / Dedicated Mail Delivery Engine)                │
│  - SMS Gateway (Commercial SMS aggregators / SMPP / HTTP)               │
│  - WhatsApp Business API (Meta Cloud / On-Premise BSP Gateway)          │
│  - [ROADMAP] Push Notification Gateways (Web Push / APNs / FCM)         │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## Current Public Capabilities

These capabilities are fully implemented in code, documented in the internal/public wiki, and verified in active controllers:

1. **Customer Intelligence & Value Scoring:**
   - Unified customer profile aggregation.
   - Deterministic Customer Value Score based on 4-part weighted formula (Monetary, Frequency, Recency, Loyalty).
   - Inactivity and churn risk detection.
   - Individual replenishment rhythm tracking based on past order cycles.

2. **Product Intelligence (PI):**
   - Categorization beyond basic catalog codes: Playbooks, Need Groups, and Product Roles.
   - Dynamic classification fields and master product resolution pipeline (sanitization, alias matching, trigram matching).
   - Analytical readiness gating before products are fed to recommendation models.

3. **Pika 360:**
   - Single-screen customer profile summarizing value metrics, transaction timeline, risk level, channel eligibility, and pending opportunities.

4. **Günün Fırsatları / Daily Opportunities (CCE):**
   - Algorithmic opportunity discovery: Repeat purchase replenishment, win-back for churn-risk customers, and cross-sell basket association.
   - Priority arbitration hierarchy and 7-day contact frequency capping.

5. **Audience Manager (Hedef Kitle ve Segmentasyon):**
   - Dynamic and static customer cohort creation using multi-condition rule trees (AND/OR logic).
   - Transaction-based segmentation (purchase recency, frequency, monetary tier, store preference).
   - Channel eligibility filtering (consented vs unconsented contacts).

6. **Campaign Manager (Çok Kanallı Kampanya Yönetimi):**
   - Multi-channel campaign authoring and scheduling.
   - Cohort binding, quiet hours enforcement, and approval workflows.
   - Direct attribution tracking.

7. **Journey Manager (Müşteri Yolculuğu Otomasyonu):**
   - Visual event-driven journey canvas.
   - Triggers: purchase completion, segment entry/exit, inactivity duration.
   - Nodes: channel dispatch, wait steps, conditional branching, goal evaluation.

8. **Content Studio & Email Template Editor:**
   - Drag-and-drop responsive visual email template builder.
   - Parameter interpolation (`{{isim}}`, customer variables).
   - Centralized template library (Email Store).

9. **AI Campaign Assistant (Pika Pilot):**
   - Natural language campaign prompt interpretation.
   - Generates campaign title, proposed target segment, channel suggestions, and responsive HTML email copy drafts.
   - Strict privacy boundary: tokenized anonymous parameters only; zero PII sent to LLM providers.

10. **Consent & Regulatory Compliance Management:**
    - Integrated IYS (İleti Yönetim Sistemi) status validation before dispatch.
    - KVKK/GDPR opt-out DB lookup at execution time.
    - Public web consent preference update form (`/contact-consent/{token}`) with Cloudflare Turnstile protection.

11. **Analytics, BI & Reporting:**
    - Live delivery telemetry: delivered, bounced, rejected, opened, clicked, unsubscribed.
    - BI Cockpit: Store-level metric aggregation, sales channel comparison, and product category performance.

12. **Data Ingestion:**
    - Manual Excel (.xlsx) and CSV bulk imports with column mapping and schema validation.
    - Google Contacts / Outlook contact import (`/wiki/gmail-kisi-aktarimi`).
    - High-throughput Ingestion REST API with idempotency and rate limiting.

---

## Current With Limitations

Features that are operational but must be marketed with strict boundaries:

1. **WhatsApp Messaging:**
   - *Limitation:* Operates strictly via the official **WhatsApp Business API**. All outbound promotional and notification broadcasts require pre-approved Meta message templates and explicit opt-in consent.
   - *Marketing Rule:* Never promise arbitrary unapproved broadcast messaging or peer-to-peer WhatsApp scraping.
2. **Contact Consent Web Form (`ContactConsentController`):**
   - *Limitation:* The public opt-in update endpoint currently captures checkboxes only for **Email** and **SMS** consent (`emailConsent`, `smsConsent`). WhatsApp opt-in is handled through provider webhooks or application imports. Push consent does not exist.
   - *Marketing Rule:* Do not claim the public token portal manages Push or WhatsApp channel preferences.
3. **Ingestion API Rate Limits:**
   - *Limitation:* Default rate limit is 1,000 requests/minute per tenant with token bucket throttling.
   - *Marketing Rule:* Avoid claims of "unlimited instant throughput." Qualify with "governed enterprise ingestion."
4. **Delivery Dispatch Throttling:**
   - *Limitation:* SMS gateway is throttled at 50 requests/second per operator contract. Delivery worker retry policy is capped at 5 attempts with exponential backoff.
   - *Marketing Rule:* Do not claim "zero latency" or "instant million-message blast."

---

## Partial Capabilities

Capabilities where partial or basic code exists, but enterprise maturity is not yet achieved:

1. **A/B Testing (`/cozumler/ab-testing`):**
   - *Reality:* Simple variant allocation (split testing creatives/subject lines) exists within Campaign Manager workflows.
   - *Missing:* Automated multi-armed bandit winner selection and rigorous Bayesian/frequentist statistical significance calculators claimed in `llms-full.txt` do not exist.
   - *Marketing Rule:* Describe as "Varyant ve kreatif karşılaştırma" (Variant testing); do not market automated statistical winner engines.
2. **Real-Time Event Processing (`/cozumler/real-time-event-processing`):**
   - *Reality:* Asynchronous webhook ingestion and Hangfire queue triggers process events in near real-time (seconds).
   - *Missing:* Sub-millisecond stream processing (e.g. Apache Flink / Kafka stream analytics).
   - *Marketing Rule:* Never claim "sub-millisecond" or "milisaniyeler içinde" activation. Use "dakikalar veya saniyeler içinde olay bazlı tetikleme" (event-driven triggers in seconds).

---

## Roadmap

Capabilities that are planned or architecturally envisioned, but **NOT** commercially live or operational:

1. **Push Notifications (Web & Mobile Push):**
   - *Status:* **ROADMAP.**
   - *Evidence:*
     * Explicitly labeled as `<h3>Push Notifications (Roadmap)</h3>` in `Views/Home/Solutions.cshtml` (line 39).
     * Missing entirely from backend delivery workers (`internal_wiki.json: internal-mimari-genel-bakis` lists only "SMS, WhatsApp veya E-posta sağlayıcısına iletilir").
     * Missing from `ContactConsentController` payload.
     * Only 1 incidental mention in public wiki across 88 articles.
   - *Marketing Rule:* **MUST NOT BE MARKETED AS A CURRENT LIVE CAPABILITY.** Any mention must carry a clear "(Roadmap)" or "(Geliştirme Aşamasında)" tag.
2. **Conversational WhatsApp Chatbots (Two-Way Conversational AI):**
   - *Status:* **ROADMAP.**
   - *Evidence:* Claimed in legacy `llms-full.txt` ("chatbot flows, and two-way conversations with customers"), but no conversational state engine or dialog management exists in code. Pika executes template-based outbound dispatches.
   - *Marketing Rule:* Do not claim conversational chatbot builder capabilities.
3. **Enterprise SSO / SAML 2.0:**
   - *Status:* **ROADMAP.**
   - *Evidence:* Claimed in `llms-full.txt` ("SSO/SAML integration"), but `Program.cs` and `AccountController.cs` implement only Cookie Authentication, API token exchange, and JWT sliding expiration. No SAML or OpenID Connect enterprise federated login is configured.
   - *Marketing Rule:* Do not market SAML/SSO enterprise authentication as live.

---

## Internal-only

Artifacts, systems, and logic that exist solely for internal engineering or administration and must never be exposed as customer-facing features:

1. **Internal Knowledge Base (`/internal/wiki/*`):**
   - Accessible only behind `[Authorize]` cookie session; sends `X-Robots-Tag: noindex, nofollow, noarchive, nosnippet`. Contains proprietary formulas, runbooks, and architectural notes.
2. **Algorithm Weights & Multipliers:**
   - CVS formula weights (0.40 Monetary, 0.25 Frequency, 0.20 Recency, 0.15 Loyalty).
   - Cross-sell thresholds (Support 0.01, Confidence 0.15, Lift 1.2).
   - These are proprietary implementation details, not configurable self-service sliders for marketing users.
3. **Dead-Letter Queue (DLQ) & Hangfire Dashboards:**
   - Operator recovery tools, retry counters, and worker telemetry.
4. **Internal AI Provider Routing Rules:**
   - The 8-second fallback threshold and model provider failover architecture.

---

## Legacy

Deprecated, orphan, or structurally obsolete code and copy that must not define future public pages:

1. **Orphan Legacy Pricing View (`Views/Home/Pricing.cshtml`):**
   - Contains hardcoded pricing packages (`₺9.900/ay`, `₺24.900/ay`, `Start / Growth / Enterprise`), features matrix, and SLA claims.
   - Has **NO** controller action in `HomeController.cs`, but remains on disk. Must never accidentally be re-exposed.
2. **Orphan Legacy Solutions Hub (`Views/Home/Solutions.cshtml`):**
   - Unmapped View that contains misleading links: links "Journey Orchestration" to `/cozumler/personalization`, "Audience Segmentation" to `/cozumler/template-management`, and "Campaign Automation" to `/cozumler/ab-testing`.
3. **Abandoned Backup File (`Views/Solutions/JourneyManager.cshtml.bak`):**
   - Orphan backup file left in production views directory.
4. **Thin Prototype Solution Stubs:**
   - 6 early stub pages (`personalization`, `template-management`, `ab-testing`, `deliverability-compliance`, `data-management-etl`, `real-time-event-processing`) that were removed from Turkish sitemap (`sitemap.xml`) with the comment *"Thin stub pages removed... Add back when content is substantially improved"*, but accidentally left in English sitemap and live routes.
5. **Duplicate / Cannibalizing Solution Pages:**
   - `/kanallar/whatsapp-kampanya-yonetimi` duplicating `/kanallar/whatsapp`
   - `/kanallar/email-marketing-template-studio` duplicating `/kanallar/email` and `/cozumler/content-studio`
   - `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` duplicating `/cozumler/consent-management`
   - `/cozumler/e-ticaret-ai-kampanya-yonetimi` duplicating `/urunler/ai-kampanya-asistani`
6. **Obsolete LLM Document (`wwwroot/llms-full.txt`):**
   - References obsolete, broken URLs (`/en/solutions/email-marketing`, `/en/home/corporate`, etc.) and unsupported claims (SOC compliance, SAML, chatbots).

---

## Unknown / Requires Product Confirmation

Items where code evidence is insufficient or contradictory:

1. **Commercial Client & Volume Proof:**
   - What are Pika's real verified client counts, message volumes, and transaction sizes?
   - *Status:* ZERO customer names or numerical stats exist in the codebase.
2. **Provenance of Campaign Manager Metrics:**
   - In `Views/Solutions/CampaignManager.cshtml`, lines 131-133 and 397-417 display `ROAS 14.2x`, `₺184.600 Sipariş Tutarı`, `%52 Açılma Oranı`, `%31 Ziyaret Artışı`, and `%41 Satış Dönüşümü`.
   - *Status:* Provenance unknown. Must be confirmed whether these are real historical case study results or purely illustrative sample data requiring `DEMO / ÖRNEK` badges.
3. **Headquarters / Office Address Conflict:**
   - `appsettings.json` records: `Teknopark Turkuaz Bina, Ostim Osb Mah. 100. Yıl Bulvarı, 55/E Kat:4 No:14 06374 Ostim / Yenimahalle / Ankara`.
   - `Views/Shared/_Layout.cshtml` JSON-LD (line 114) records: `addressLocality: "İstanbul"`.
   - *Status:* Physical headquarters location must be authoritatively verified.
4. **Push Notification Commercial Availability:**
   - Is there any third-party push gateway currently operational in production, or is Push strictly a 2026/2027 roadmap item?

---

## Channels

| Channel | Product Status | Marketing Permission | Implementation Reality | Constraints / Required Qualifiers |
| :--- | :--- | :--- | :--- | :--- |
| **Email** | CURRENT | PUBLIC_OK | Native SMTP / Mail delivery worker, visual editor, store, HTML rendering. | Pre-validated sender domain (SPF/DKIM) required. |
| **SMS** | CURRENT | PUBLIC_OK | Native SMS aggregator gateways, character calculation, opt-out handling. | 50 req/sec gateway throttling; IYS commercial messaging consent check mandatory. |
| **WhatsApp** | CURRENT | PUBLIC_OK | WhatsApp Business API integration, verified template dispatches. | Meta approval required for templates; opt-in consent mandatory; no unapproved broadcast scraping. |
| **Push** | ROADMAP | PUBLIC_WITH_QUALIFIER | No delivery worker or gateway provider in backend code. Listed as Roadmap in `Solutions.cshtml`. | **Must always carry "(Roadmap)" qualifier.** Forbidden: "Pika ile push gönderin", "Native push desteği". |

---

## AI Capability Boundary

Pika uses artificial intelligence strictly as an **assistive and explanatory co-pilot**, not as an unconstrained autonomous agent or black-box decision maker.

| Category | Is AI Used? | Technology / Engine | Public Marketing Description |
| :--- | :--- | :--- | :--- |
| **Customer Value Score** | **NO** (Deterministic) | Mathematical weighted formula (Monetary, Frequency, Recency, Loyalty). | Deterministic statistical scoring. Never claim "AI computes customer value." |
| **Churn / Dormancy Risk** | **NO** (Rule-Based) | Inactivity elapsed days vs customer-specific historical purchasing rhythm. | Rule-based purchasing rhythm analysis. |
| **Cross-Sell Affinity** | **NO** (Statistical) | Association Rule Mining (Support, Confidence, Lift algorithms). | Statistical market basket analysis. |
| **Next Best Action / Timing** | **NO** (Rule Trees) | Customer Context Engine (CCE) hierarchical arbitration & frequency caps. | Contextual decision rules and priority trees. |
| **AI Müşteri Özeti** | **YES** (LLM) | Summarizes normalized customer metrics into concise natural language Turkish paragraphs. | "AI-powered customer behavior summary." |
| **AI Campaign Assistant (Pika Pilot)** | **YES** (LLM) | Converts natural language prompt into campaign draft, recommended cohort rules, and email template. | "AI Campaign Co-pilot (Taslak ve içerik asistanı)." |
| **Content Copywriting** | **YES** (LLM) | Subject line and message text generation within Content Studio. | "AI copy generation and variation assistant." |

### Absolute AI Boundary Rules
1. AI never initiates or sends a campaign autonomously. Human review and explicit approval are mandatory.
2. No PII (TCKN, phone numbers, real customer names) is ever submitted to LLM APIs. Only anonymized, tokenized behavioral parameters are used.
3. If an LLM provider fails or times out (> 8s), Pika gracefully falls back to deterministic default templates.

---

## Data & Integration Capability Boundary

1. **Ingestion Modalities:**
   - Batch file uploads: Excel (`.xlsx`) and CSV with dynamic column mapping.
   - REST API: Asynchronous buffer endpoint (`/api/v1/ingest/`) returning `202 Accepted`.
   - Contact Import: Automated Gmail & Outlook address book sync (`/wiki/gmail-kisi-aktarimi`).
2. **Contract Safeguards:**
   - Mandatory `X-Idempotency-Key` prevents duplicate transaction billing/ingestion.
   - Token bucket rate limiter caps traffic at 1,000 requests/min per tenant.
3. **Data Normalization:**
   - Identity resolution merges customer records across phone, email, and external customer ID.
   - Product master matching maps vendor SKUs to unified Need Groups and Product Roles.

---

## Consent / Compliance Boundary

1. **IYS (İleti Yönetim Sistemi):**
   - Outbound commercial messages are checked against IYS consent databases before dispatch. Unapproved contacts are automatically suppressed.
2. **KVKK / GDPR:**
   - Opt-out requests (SMS STOP, email unsubscribe) update central suppression lists immediately.
   - Public token-based consent portal allows recipients to manage channel permissions.
3. **Legal Responsibility Disclaimer:**
   - Pika provides technical compliance workflows and audit trails, but does not provide legal indemnity. Businesses remain legally responsible for their own opt-in collection and marketing permissions.

---

## Analytics / Measurement Boundary

1. **Direct Revenue Attribution:**
   - Pika attributes order volume and turnover directly to campaigns that influenced the transaction within defined attribution windows.
2. **BI Cockpit:**
   - Provides physical store vs online channel sales metrics, category performance comparisons, and campaign ROI baselines.
3. **Observability:**
   - Full delivery telemetry tracking attempts, gateway status codes, bounces, opens, and clicks.

---

## Product Evidence Index

| Entity / Concept | Primary Repository Evidence | Secondary Evidence |
| :--- | :--- | :--- |
| **Pika Definition** | `App_Data/wiki.json: pika-nedir`, `pika-ne-degildir`, `pika-nasil-calisir` | `wwwroot/llms.txt: lines 1-16` |
| **6-Step Decision Cycle** | `App_Data/wiki.json: pika-nasil-calisir` | `wwwroot/llms.txt: lines 8-15` |
| **Ingestion API & Rate Limits** | `App_Data/internal_wiki.json: internal-api-mimarisi-ve-veri-kontratlari` | `internal-mimari-genel-bakis` |
| **Customer Value Score** | `App_Data/internal_wiki.json: internal-musteri-deger-skoru-algoritmasi` | `App_Data/wiki.json: musteri-deger-skoru` |
| **CCE Decision Engine** | `App_Data/internal_wiki.json: internal-cce-karar-motoru-mimarisi` | `App_Data/wiki.json: gunun-firsatlari-ve-karar-motoru` |
| **Product Intelligence** | `App_Data/internal_wiki.json: internal-product-intelligence-resolution-mimarisi`| `App_Data/wiki.json: product-intelligence-nedir` |
| **Cross-Sell Basket Mining** | `App_Data/internal_wiki.json: internal-cross-sell-sepet-analizi-motoru` | `App_Data/wiki.json: cross-sell-firsatlari` |
| **Delivery Workers & DLQ** | `App_Data/internal_wiki.json: internal-teslimat-konsolu-ve-worker-mimarisi` | `internal-retry-politikasi-ve-hata-yonetimi` |
| **AI LLM Routing & Privacy**| `App_Data/internal_wiki.json: internal-ai-mimarisi-ve-prompt-yonetimi` | `App_Data/wiki.json: ai-rolu-guven-siniri` |
| **Push Status (Roadmap)** | `Views/Home/Solutions.cshtml: line 39` | Absence from `internal_wiki.json` delivery layer |
| **Real Product Screenshots** | `wwwroot/wiki/assets/images/*.png` (29 files) | `Views/Solutions/AiCampaignAssistant.cshtml: line 107` |
