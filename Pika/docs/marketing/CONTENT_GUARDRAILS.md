# Content Guardrails & Copy Governance

This document establishes strict boundaries, terminology rules, and authoring guidelines for all public marketing copy, SEO metadata, documentation, and LLM-facing materials across Pika Web 2.0.

---

## Approved Positioning

### Primary Strategic Proposition
**Turkish:**  
> **"Müşteri Zekâsı ve Omnichannel Pazarlama Platformu"**

**English:**  
> **"Customer Intelligence & Omnichannel Marketing Platform"**

### The Core Axiom
> **"Pika’nın başlangıç noktası mesaj göndermek değil, anlamaktır."**  
> *(Pika’s starting point is not merely sending messages; it is understanding customer, product, and transaction context.)*

### Core Strategic Chain (6 Adımlı Değer Zinciri)
```
VERİ (Data & Ingestion)
  → MÜŞTERİ VE ÜRÜN ZEKÂSI (Customer & Product Intelligence)
    → FIRSAT VE KARAR (CCE / Günün Fırsatları)
      → HEDEF KİTLE, KAMPANYA VE YOLCULUK (Audience, Campaign & Journey)
        → KANAL YÜRÜTME (Email, SMS, WhatsApp)
          → ÖLÇÜMLEME VE ÖĞRENME DÖNGÜSÜ (BI Cockpit & Analytics)
```

### Architectural Distinction (What Pika Is NOT)
Pika must never be reduced to or described merely as:
- a bulk SMS or transactional email broadcaster,
- a standalone WhatsApp API wrapper,
- a generic campaign scheduling calendar,
- a passive BI / dashboard reporting tool,
- a generic CDP (Customer Data Platform),
- an unconstrained AI copy generator,
- an arbitrary bag of disconnected marketing tools.

*Tone Rule:* Present this distinction constructively to demonstrate architectural depth and clarity. **Never engage in competitor-bashing copy.**

---

## Canonical Terminology

| Concept / Capability | Canonical Turkish Name | Canonical English Name | Strictly Avoid / Deprecated |
| :--- | :--- | :--- | :--- |
| **The Platform** | Pika | Pika | Pika Marketing Tool, Pika Mailer, Pika SMS |
| **Customer Intelligence** | Müşteri Zekâsı / Customer Intelligence | Customer Intelligence | CRM modülü, Müşteri Takip Sistemi |
| **Product Intelligence** | Ürün Zekâsı / Product Intelligence | Product Intelligence | Katalog yönetimi, Ürün listesi |
| **Unified View** | Pika 360 | Pika 360 | Customer 360 (tercih edilmeyen alias), Müşteri Kartı |
| **Opportunity Engine** | Günün Fırsatları / Fırsat ve Karar Motoru | Daily Opportunities / Opportunity Engine | Fırsatlar (fazla genel), Promosyon listesi |
| **Segmentation** | Audience Manager / Hedef Kitle Yönetimi | Audience Manager / Segmentation | Liste yönetimi, Adres defteri, Template Management |
| **Campaigns** | Campaign Manager / Kampanya Yönetimi | Campaign Manager | Toplu Mesaj Gönderici, AB Testing (sayfa adı olarak) |
| **Journeys** | Journey Manager / Müşteri Yolculuğu Otomasyonu | Journey Manager / Journey Orchestration | Müşteri Etkileşim Yönetimi, Personalization (sayfa adı olarak) |
| **Content Builder** | Content Studio / İçerik ve Şablon Tasarımı | Content Studio | Email Builder, Template Management (eski sayfa adı) |
| **AI Assistant** | Pika Pilot / AI Kampanya Asistanı | Pika Pilot / AI Campaign Assistant | Otonom Pazarlama Botu, Yapay Zekâ Kampanya Üreticisi |
| **Consent Governance**| İzin ve Uyumluluk Yönetimi (Consent Management) | Consent & Compliance Management | İYS Modülü, KVKK Ayarları, Deliverability (tek başına) |
| **Analytics & BI** | Analytics & Reporting / BI Kokpit | Analytics & Reporting / BI Cockpit | Raporlama Ekranı, Basit İstatistikler |
| **Email Channel** | E-Posta / Email Marketing | Email Marketing | Mail basma, E-posta gönderim aracı |
| **SMS Channel** | SMS / SMS Kampanyaları | SMS Campaigns | Toplu SMS, SMS gateway |
| **WhatsApp Channel** | WhatsApp / WhatsApp Kampanya Yönetimi | WhatsApp Messaging / Campaigns | WhatsApp Botu, WhatsApp Spam |
| **Push Channel** | Push Bildirimleri (Roadmap) | Push Notifications (Roadmap) | Native Push (Roadmap belirtilmeden kullanılamaz) |
| **Product Classification**| Need Group / İhtiyaç Grubu | Need Group | Ürün türü, Etiket |
| **Commercial Role** | Product Role / Ürün Rolü | Product Role | Ürün sınıfı, Reyon |
| **Customer Score** | Müşteri Değer Skoru (CVS) | Customer Value Score (CVS) | AI Skoru, Sadakat Puanı |

---

## Approved Product Descriptions

### Short Platform Elevator Pitch (TR)
> "Pika; müşteri, ürün ve satış verilerinden ticari fırsatları tespit eden, yöneticiye doğru kararı sunan ve doğru anda çok kanallı aksiyona dönüştüren müşteri zekâsı ve pazarlama platformudur."

### Short Platform Elevator Pitch (EN)
> "Pika is a B2B SaaS customer intelligence and omnichannel marketing platform that unifies customer, product, and transaction data to detect commercial opportunities and orchestrate timely, measurable engagement."

### Module Core Definitions
- **Customer Intelligence:** "Müşteri davranışını, alışveriş sıklığını, yaşam döngüsü ritmini ve değer skorunu hesaplayarak kime ne zaman ulaşılması gerektiğini belirleyen zekâ katmanıdır."
- **Product Intelligence:** "Ürünleri sadece stok kodu olarak değil; tüketim ihtiyacı (Need Group), ticari rolü (Product Role) ve sepet birlikteliği bağlamında anlamlandıran analitik motordur."
- **Pika 360:** "Müşterinin geçmiş işlem özetini, güncel değer skorunu, kayıp riskini, kanal izin durumunu ve bekleyen fırsatlarını tek ekranda toplayan bütünleşik karar konsoludur."
- **Günün Fırsatları:** "Müşteri tüketim ritmi, sepet birliktelikleri ve pasifleşme sinyallerinden günlük ticari aksiyon öncelikleri üreten deterministik fırsat motorudur."
- **AI Kampanya Asistanı (Pika Pilot):** "Pazarlama ekiplerinin doğal dildeki kampanya fikirlerini saniyeler içinde hedef kitle kuralı, kanal dağılımı ve onaylanabilir e-posta taslağına dönüştüren yardımcı üretken yapay zekâ asistanıdır."

---

## Claims That Require Evidence & The 5-Level Governance Model

PikaWeb is the public website and documentation repository; backend implementation code of `app.pika.tr` is hosted externally. Claims are classified and governed by the 5-level evidence model:
- `A. CODE_VERIFIED`: Directly verifiable in PikaWeb code.
- `B. DOCUMENTED`: Authoritatively documented in `internal_wiki.json` / `wiki.json`.
- `C. MARKETING_ONLY`: Public claim only; must NOT be marketed without verification.
- `D. CONTRADICTORY`: Sources disagree; requires qualification or product confirmation.
- `E. UNVERIFIED`: Insufficient evidence in repo; strictly prohibited until substantiated.

The following claims are **strictly restricted**. They cannot be added to any public marketing page, SEO title, meta description, or LLM file without written proof attached to the repository:

1. **Customer Counts & Brand Volume (`UNVERIFIED`):** Any mention of "X+ müşteri", "Y marka", or "Z ülkede aktif".
2. **Message Throughput & Latency (`CONTRADICTORY` / `DOCUMENTED`):** Claims such as "Milyonlarca mesaj saniyede iletilir" or "Milisaniye içinde tetikleme". Must qualify to "saniyeler içinde olay bazlı tetikleme" and document 50 req/sec SMS limits.
3. **Specific Performance Uplift (`UNVERIFIED`):** Uplift statistics such as "Dönüşümde %30 artış", "Sepet terkinde %40 azalma", "14.2x ROAS". Must be labeled `ÖRNEK SENARYO` / `TEMSİLİ GÖSTERGE`.
4. **Platform Uptime & SLAs (`UNVERIFIED`):** Numerical uptime promises such as "99.99% Uptime", "Kurumsal SLA garantisi".
5. **Certifications & Partnerships (`MARKETING_ONLY`):** Claims of "SOC-2 Certified", "ISO 27001 Certified", "Meta Certified Partner".
6. **Enterprise SSO / SAML 2.0 (`MARKETING_ONLY`):** Not configured in PikaWeb; prohibited from marketing until confirmed in backend identity server.
7. **Conversational WhatsApp Chatbots (`CONTRADICTORY`):** Prohibited from marketing; position WhatsApp strictly as outbound approved template messaging.
8. **Push Notifications (`CONTRADICTORY`):** Prohibited from live claims; must always include `(Roadmap)` qualifier.

---

## Forbidden Unsupported Claims

The following claims are **PROHIBITED** from all public Pika materials:

- ❌ "Pika supports web and mobile push notifications out of the box." *(Push is ROADMAP.)*
- ❌ "Pika provides conversational AI chatbots for two-way WhatsApp support." *(Out of scope; outbound templates only.)*
- ❌ "Pika is a SOC-compliant enterprise platform." *(No SOC attestation exists.)*
- ❌ "Pika features enterprise SAML 2.0 / Okta SSO integration." *(Not configured.)*
- ❌ "Pika's AI automatically launches campaigns without human intervention." *(Safety violation; human approval is mandatory.)*
- ❌ "Pika guarantees 100% email inbox delivery and zero spam placement." *(Unrealistic deliverability promise.)*
- ❌ "Pika eliminates all KVKK and IYS legal liabilities." *(Pika provides technical tooling, not legal indemnity.)*

---

## AI Wording Rules

When writing about artificial intelligence in Pika, always maintain technical precision:

| Action | Allowed Terminology | Strictly Forbidden Terminology |
| :--- | :--- | :--- |
| **Campaign Drafts** | "Yapay zekâ destekli taslak üretimi", "Pika Pilot kampanya asistanı", "Doğal dil ile kampanya önerisi" | "Otonom yapay zekâ", "Kendi kendine pazarlama yapan AI", "Pazarlamacıya gerek bırakmayan sistem" |
| **Customer Scoring** | "Deterministik analitik hesaplama", "İşlem ve davranış bazlı skorlama" | "Yapay zekânın tahmin ettiği müşteri değeri", "AI skoru" |
| **Cross-Sell & Basket** | "Birliktelik analizi", "Sepet ve ürün rolü eşleştirmesi" | "AI zihin okuyucu ürün tahmini" |
| **Safety & Control** | "İnsan onaylı iş akışı", "Yardımcı co-pilot yaklaşımı", "Kontrollü üretim" | "Tam otomatik kontrolsüz gönderim" |
| **Data Privacy** | "KVKK uyumlu anonim parametreler", "Kişisel veri (PII) paylaşılmayan güvenli model mimarisi" | "Verileriniz yapay zekâ modellerimizi eğitir" |

---

## Compliance Wording Rules

1. **IYS (İleti Yönetim Sistemi):**
   - *Allowed:* "Gönderim öncesi IYS izin sorgulama ve onay kontrolü altyapısı", "İYS uyumlu ticari ileti süreçleri".
   - *Forbidden:* "İYS cezalarına karşı %100 yasal koruma garantisi".
2. **KVKK / GDPR:**
   - *Allowed:* "KVKK ve GDPR ilkeleriyle uyumlu veri işleme altyapısı", "Merkezi opt-out ve izin tercih yönetimi".
   - *Forbidden:* "KVKK sertifikalı platform" (KVKK has no commercial software product certification).

---

## Roadmap Wording Rules

Any capability marked as **ROADMAP** (e.g. Push Notifications, Conversational WhatsApp bots) must adhere to these rules:
1. If mentioned in main navigation or feature grids, it **MUST** be explicitly suffixed with `(Roadmap)` or `(Geliştirme Aşamasında)`.
2. Action buttons must **NEVER** say "Hemen Gönderin" or "Canlı Kullanın". Acceptable action: "Yol Haritasını İnceleyin" or "Bilgi Alın".
3. Must never be included in core platform capability summaries without qualifier.

---

## Demo Data Wording Rules

Whenever numerical examples or mock dashboard screenshots are displayed:
1. They must be explicitly framed as illustrative samples.
2. In UI mockups, display a badge or caption:
   - *Turkish:* `Örnek Gösterim` or `Temsili Senaryo Verisi`
   - *English:* `Illustrative Sample` or `Simulation Data`
3. Never disguise synthetic metrics as real client success stories.

---

## Pricing Governance (NON-NEGOTIABLE)

**Pika does NOT publish fixed public pricing.**

Commercial pricing is strictly quotation-based and varies according to:
- Customer / contact volume
- Transaction volume
- Messaging / channel usage (Email, SMS, WhatsApp)
- Onboarding and setup scope
- Integration requirements (custom ERP, POS, CRM, e-commerce)
- Enabled product modules
- Support / service scope (SLA levels, account management)
- Contract terms

### Absolute Pricing Guardrails
1. **Do not publish monthly package prices** (e.g. `₺X/ay`, `$Y/mo`).
2. **Do not publish "starting from" prices** (e.g. `₺9.900'den başlayan fiyatlarla`).
3. **Do not invent Start / Growth / Enterprise price amounts.**
4. **Do not expose legacy `Views/Home/Pricing.cshtml` values.**
5. **Do not create pricing calculators** on the public marketing website unless explicitly approved in a future task.
6. **Do not imply that all customers receive identical commercial terms.**

### Approved Public Pricing Language
- **Turkish:** *"İhtiyacınıza ve kullanım kapsamınıza göre özel teklif"* / *"İşletmenizin ölçeğine ve entegrasyon ihtiyaçlarına göre uyarlanan kurumsal teklif"*
- **English:** *"Custom enterprise quotation tailored to your volume, modules, and operational scope"*

### Primary CTAs for Pricing Intent
- **Turkish:**
  - `Teklif Al`
  - `Demo Talep Et`
  - `Satış Ekibiyle Görüşün`
- **English:**
  - `Request a Quote`
  - `Request a Demo`
  - `Talk to Sales`

### Legacy View Governance
The view file `Views/Home/Pricing.cshtml` is strictly classified as **`LEGACY / DO_NOT_MARKET`**. It must remain unrouted and is recommended for archival/removal from public routing in the Phase P01 route-cleanup phase.

---

## Competitor Reference Rules

Competitors (SmartMessage, Related Digital, Insider, Braze, Bloomreach, Optimove, etc.) may be used internally as design and information-architecture benchmarks.
1. **Never copy competitor claims, statistics, client counts, or badges.**
2. **Never mention competitor names disparagingly in public copy.**
3. Focus entirely on Pika's authentic strengths: transactional depth, product intelligence, daily opportunity discovery, and compliant orchestration.

---

## Content Authoring Rules for Future Codex Tasks

> [!IMPORTANT]
> ### STRICT INSTRUCTION FOR FUTURE CODEX TASKS
> 
> **Future Codex tasks MUST NOT independently write new customer-facing marketing claims.**
> 
> 1. Approved copy will be supplied by the content owner in later phase prompts.
> 2. If marketing copy or narrative text is missing for a newly created page or component, Codex tasks must insert explicit **`<!-- TODO: APPROVED COPY REQUIRED -->`** markers rather than inventing claims.
> 3. No numbers, percentages, client references, uptime promises, or partner logos may be added unless specifically provided in the prompt or sourced from `PRODUCT_TRUTH.md`.
> 4. Any attempt to "improve" copy by making it sound more impressive or enterprise-grade using unverified adjectives ("devrim niteliğinde", "dünyanın en iyi", "milisaniyelik") is a violation of this governance policy.
