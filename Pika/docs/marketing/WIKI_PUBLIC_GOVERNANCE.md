# PIKA PUBLIC WIKI GOVERNANCE & TAXONOMY SPECIFICATION

> **Document Status:** Active Canonical Standard  
> **Last Updated:** September 2026  
> **Version:** 1.0 (Convergence Release)  
> **Scope:** Public Knowledge Base (`/wiki/`), Internal Engineering Docs (`/internal/wiki/`), Robots Meta, Sitemap Governance, Canonical Product Truth Reconciliation

---

## 1. Executive Summary & Principles

Pika's Public Knowledge Base (`/wiki/`) serves two distinct operational audiences while safeguarding technical intellectual property and adhering to search engine / LLM indexability best practices:

1. **Strategic & High-Level Evaluation (Prospects, Executive Buyers, Evaluators):**
   - Articles that explain platform value, the 6-step customer value chain, deterministic intelligence models, omnichannel orchestration, and governance.
   - **Classification:** `INDEX` (Emits `<meta name="robots" content="index, follow">`, included in `sitemap.xml`).
   - **Count:** 58 articles (+ 1 wiki root `/wiki/` = 59 wiki URLs in sitemap).

2. **Detailed Operational & System Administration (Active Operators, Catalog Admins, Setup Teams):**
   - Step-by-step UI guides, field-level data mapping instructions, canvas workflows, delivery console error diagnosis, and operational settings.
   - While public and discoverable via in-app search, these documents do not represent entry points for organic search or LLM citations and contain low-entropy procedural copy.
   - **Classification:** `NOINDEX` (Emits `<meta name="robots" content="noindex, follow">`, excluded from `sitemap.xml`).
   - **Count:** 30 articles.

3. **Dahili Mühendislik Dokümantasyonu (Quarantined Internal Knowledge Base):**
   - 15 internal architectural, algorithm implementation, database schema, Hangfire worker, and deployment pipeline specifications quarantined under `/internal/wiki/`.
   - Protected by `[Authorize]` attributes; unauthenticated access redirects to `/Account/Login`. Completely absent from public JSON, public navigation, public search, and `sitemap.xml`.

---

## 2. Canonical Product Truth Reconciliation Rules

Every public Wiki article must adhere to the following product truth guardrails:

| Area | Canonical Truth Standard | Prohibited / Deprecated Framing | Corrected Articles |
| :--- | :--- | :--- | :--- |
| **Platform Category** | Customer Intelligence & Omnichannel Marketing Platform | Standalone CRM, pure CDP, basic email blast tool | All 88 articles aligned |
| **Customer Value Score (CVS)** | `(0.40 × Monetary) + (0.25 × Frequency) + (0.20 × Recency) + (0.15 × Loyalty)` | `%60 ciro + %40 sıklık`, or treating Rhythm as a 5th score component | `musteri-deger-skoru`, `musteri-degeri-sadakat`, `sadakat-hedefe-yakinlik`, `sss`, `sozluk` |
| **Shopping Rhythm** | Independent behavioral context signal comparing customer recency against their historical median repurchase interval | 5th score factor, weighted multiplier | `musteri-deger-skoru`, `musteri-degeri-sadakat` |
| **Active Marketing Channels** | **E-posta, SMS, WhatsApp** | Push notifications marketed as active channel | `kampanya-yoneticisi-ve-kurgular`, `kampanya-kanallari-ve-rol-dagilimi` |
| **Repeat Purchase Timing** | Historical cycle window (80%–120% median repurchase interval). Statuses: Zamanı Yaklaşan, Geciken, Döngü Dışında | Fake purchase probability % (`%87 olasılık`), forecast order amounts (`1.450 TL`), forward revenue predictions | `tekrar-satin-alma-analizi` |
| **AI Role & Autonomy** | Explainer, synthesizer, creative copy assistant with Human-in-the-loop approval. Decision logic is deterministic system rules. | Autonomous decision-maker, autonomous sending agent, black-box predictor | `ai-rolu-guven-siniri`, `next-best-action`, `sss` |
| **Opportunity Confidence** | Evidence quality & data sufficiency (`Kanıt Yeterliliği`: Sınırlı, Gelişen, Güçlü Kanıt) | Algorithmic "Güven Skoru Motoru" or purchase probability predictor | `firsat-guveni-kanit`, `firsattan-aksiyona-gecis` |
| **High-Risk Claims** | Enterprise-grade TLS, tenant isolation, KVKK / İYS consent checks | SOC 2, ISO 27001, SAML SSO guarantees, 99.99% uptime SLA, guaranteed revenue increases | All 88 articles verified clean (0 violations) |
| **Engineering Leakage** | Customer-safe terms (`kuyruk durumu ve başarısız iş inceleme havuzu`) | Internal code symbols, Hangfire, MediatR, DbContext, dead-letter queues | `gonderim-operasyonu-izleme`, `sss`, `sozluk` |

---

## 3. Public Wiki Complete Inventory (88 Articles)

### Category 1: Başlarken (Getting Started) — 6 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `pika-nedir` | Pika Nedir? | Prospect / Customer | **INDEX** | Core platform definition, value proposition, 6-step value chain overview. |
| `pika-ne-degildir` | Pika Ne Değildir? | Prospect / Customer | **INDEX** | Boundary definition distinguishing Pika from standalone CRM, pure CDP, and mass-blast tools. |
| `deger-onerisi-ve-is-modeli` | Değer Önerisi ve İş Modeli | Prospect / Customer | **INDEX** | Strategic commercial positioning, unit economics, and customer retention ROI. |
| `kurulum-ve-ilk-adimlar` | Kurulum ve İlk Adımlar | Customer / Operator | **INDEX** | High-level onboarding flow from initial data load to first actionable campaign. |
| `pika-hangi-verileri-kullanir` | Pika Hangi Verileri Kullanır? | Prospect / Customer | **INDEX** | Data footprint transparency: sales transactions, customer identity, catalog data, and consent records. |
| `bilgi-bankasi-haritasi` | Bilgi Bankası Haritası | All Audiences | **INDEX** | Knowledge base site index and navigation hub for users and search crawlers. |

---

### Category 2: Müşteriyi ve Ürünü Anlayın (Customer & Product Intelligence) — 17 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `pika-360` | Pika 360 | Prospect / Customer | **INDEX** | Flagship customer 360 cockpit overview unifying value, risk, and reachability. |
| `customer-intelligence-nedir` | Customer Intelligence ve Müşteri Analitiği | Prospect / Customer | **INDEX** | Foundational conceptual guide to behavioral customer intelligence. |
| `musteri-degeri-sadakat` | Müşteri Değeri ve Sadakat | Prospect / Customer | **INDEX** | Conceptual separation of commercial value, loyalty status, and risk signals. |
| `musteri-deger-skoru` | Müşteri Değer Skoru | Prospect / Customer | **INDEX** | Canonical 4-factor CVS formula definition and business interpretation. |
| `sadakat-hedefe-yakinlik` | Sadakat ve Hedefe Yakınlık | Prospect / Customer | **INDEX** | Loyalty milestones, tier progression, and goal proximity mechanics. |
| `deger-risk-birlikte-okuma` | Değer ve Risk Göstergelerini Birlikte Okumak | Prospect / Customer | **INDEX** | Strategic matrix combining customer value tiers with churn/passivity risk. |
| `tekrar-satin-alma-analizi` | Tekrar Satın Alma Analizi | Prospect / Customer | **INDEX** | 80%–120% historical cycle window analysis for timely repurchase opportunities. |
| `product-intelligence-nedir` | Product Intelligence Nedir? | Prospect / Customer | **INDEX** | Foundational guide to semantic product intelligence and catalog taxonomy. |
| `playbook-sektorel-anlam` | Playbook: Sektörel Ürün Anlamlandırması | Prospect / Customer | **INDEX** | Vertical playbook framework translating raw SKUs into commercial meaning. |
| `need-group-product-role` | Need Group ve Product Role Mimarisi | Prospect / Customer | **INDEX** | Product taxonomy roles: consumable, cross-sell anchor, upgrade target. |
| `dinamik-siniflandirma-alanlari` | Dinamik Sınıflandırma Alanları | Operator / Catalog Admin | **NOINDEX** | Detailed field configuration workbench for custom product attributes. |
| `kategori-playbook-baglantisi` | Kategori Playbook Bağlantısı | Operator / Catalog Admin | **NOINDEX** | Procedural manual mapping categories to sector playbooks. |
| `urun-siniflandirma-workbench` | Ürün Sınıflandırma Workbench | Operator | **NOINDEX** | Operational UI instructions for bulk batch classification. |
| `master-urun-anlamlandirmalari` | Master Ürün Anlamlandırmaları | Operator / Data Admin | **NOINDEX** | Technical entity deduplication and product master record setup. |
| `review-resolution-readiness` | Review, Resolution ve Readiness İş Akışları | Operator | **NOINDEX** | Step-by-step exception resolution queue workflow for unmatched items. |
| `product-intelligence-musteri-firsati` | Product Intelligence ile Müşteri Fırsatı Üretimi | Prospect / Customer | **INDEX** | Strategic bridge connecting catalog intelligence to commercial opportunity generation. |
| `ai-musteri-ozeti` | AI Müşteri Özeti | Prospect / Customer | **INDEX** | AI-generated narrative customer insights backed by deterministic system metrics. |

---

### Category 3: Fırsat ve Karar (Opportunity & Decision Engine) — 7 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `gunun-firsatlari-ve-karar-motoru` | Günün Fırsatları ve Karar Motoru | Prospect / Customer | **INDEX** | Daily algorithmic opportunity board ranking immediate revenue actions. |
| `segmentasyon-ve-firsatlar` | Segmentasyon ve Fırsatlar | Prospect / Customer | **INDEX** | Concept linking static/dynamic audience segments to actionable commercial triggers. |
| `firsat-turleri` | Fırsat Türleri | Prospect / Customer | **INDEX** | Taxonomy of opportunity categories: repurchase, cross-sell, upsell, win-back, loyalty. |
| `cross-sell-firsatlari` | Cross-sell / Çapraz Satış Analizi | Prospect / Customer | **INDEX** | Basket affinity, support, confidence, and lift methodology for complementary offerings. |
| `upsell-firsatlari` | Upsell / Yükseltme Analizi | Prospect / Customer | **INDEX** | Premium upgrade candidate identification based on usage maturation. |
| `next-best-action` | Next Best Action, Kanal ve Zaman | Prospect / Customer | **INDEX** | Deterministic multi-dimensional evaluation of action, channel, and send window. |
| `firsat-guveni-kanit` | Fırsat Güveni ve Kanıt | Prospect / Customer | **INDEX** | Evidence sufficiency levels (Sınırlı, Gelişen, Güçlü) ensuring explainable recommendations. |

---

### Category 4: Aksiyon ve Otomasyon (Action & Automation) — 13 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `firsattan-aksiyona-gecis` | Fırsattan Aksiyona Geçiş | Customer / Operator | **INDEX** | 4-step governance bridge from analytical signal to approved campaign delivery. |
| `kampanya-journey-orkestrasyonu` | Journey, Kampanya ve Orkestrasyon Katmanı | Prospect / Customer | **INDEX** | Strategic distinction between broadcast campaigns and multi-stage lifecycle journeys. |
| `kampanya-yoneticisi-ve-kurgular` | Campaign Manager ve Kampanya Kurguları | Customer / Operator | **INDEX** | Omnichannel campaign builder workflow across Email, SMS, and WhatsApp. |
| `journey-tasarim-tuvali` | Journey Tasarım Tuvali | Operator | **NOINDEX** | Step-by-step canvas node configuration, triggers, delays, and splits. |
| `journey-karar-kurallari` | Journey Karar Kuralları | Operator | **NOINDEX** | Detailed conditional logic and branch rule setup within automated journeys. |
| `journey-store` | Journey Store | Operator | **NOINDEX** | Catalog of pre-configured journey templates and deployment instructions. |
| `icerik-studyosu-ve-gorsel-yonetimi` | Content Studio ve İçerik Tasarımı | Customer / Operator | **INDEX** | Centralized marketing asset, template, and responsive layout management. |
| `email-template-editor` | E-posta Şablon Editörü | Operator | **NOINDEX** | Procedural visual editor guide: drag-and-drop blocks, styling, and merge tags. |
| `email-store` | E-posta Şablon Mağazası | Operator | **NOINDEX** | In-app template store browsing, previewing, and cloning procedures. |
| `pika-pilot-ai-kampanya-asistani` | Pika Pilot: AI Kampanya Asistanı | Prospect / Customer | **INDEX** | Generative assistant for creative subject line, email copy, and message drafting. |
| `segment-sablonlari` | Segment Şablonları | Operator | **NOINDEX** | Library of standard industry segmentation queries and filter presets. |
| `aksiyon-calisma-alani` | Aksiyon Çalışma Alanı | Operator | **NOINDEX** | Operator dashboard for queuing, reviewing, and triggering individual customer actions. |
| `yayinlama-sablon-ve-yonetim` | Yayınlama, Şablon ve Yönetim | Operator | **NOINDEX** | Template versioning, governance approvals, and release management procedures. |

---

### Category 5: Kanallar ve İzinler (Channels & Permissions) — 6 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `kampanya-kanallari-ve-rol-dagilimi` | Kampanya Kanalları ve Rol Dağılımı | Prospect / Customer | **INDEX** | Strategic comparative guide on channel roles: Email, SMS, WhatsApp. |
| `kanal-operasyonlari` | Kanal Operasyonları | Operator | **NOINDEX** | Gateway credentials, sender ID setup, webhook configuration, and dispatch ops. |
| `iletisim-listeleri-ve-opt-out` | İletişim Listeleri ve Tercih Yönetimi | Operator / Compliance | **NOINDEX** | Procedural contact list hygiene, unsubscribe link embedding, and suppression rules. |
| `izin-optout-iys` | İzin, Opt-out ve İYS Uyumu | Prospect / Customer / Compliance | **INDEX** | Legal compliance architecture: KVKK consent, İYS sync, and opt-out priority. |
| `izin-kanal-zamanlama` | İzin, Kanal ve Zamanlama Uyumu | Customer / Operator | **INDEX** | Eligibility matrix combining active consent, reachable address, and frequency capping. |
| `iletisim-kanallarini-birlikte-okumak` | İletişim Kanallarını Birlikte Okumak | Prospect / Customer | **INDEX** | Cross-channel engagement signals and deduplication to prevent communication fatigue. |

---

### Category 6: Veri ve Entegrasyon (Data & Integration) — 9 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `veri-entegrasyon-genel` | Veri Entegrasyonu Genel Bakış | Prospect / Technical Buyer | **INDEX** | High-level data architecture: batch Excel ingestion vs real-time REST API streaming. |
| `excel-csv-aktarimi` | Mevcut Verinizden Başlayın | Operator | **NOINDEX** | Detailed step-by-step file upload tutorial for Excel and CSV spreadsheets. |
| `kolon-eslestirme` | Kolon Eşleştirme ve Şema Kurulumu | Operator | **NOINDEX** | Field mapping workbench: mapping source columns to Pika schema entities. |
| `veri-dogrulama-kalite` | Veri Doğrulama ve Kalite Kontrolü | Operator | **NOINDEX** | Procedural error handling for format mismatches, duplicate rows, and null fields. |
| `ice-aktarma-sonuclari` | İçe Aktarma Sonuçları ve Hata Yönetimi | Operator | **NOINDEX** | Import report inspection: success row counts, rejected rows, and error downloads. |
| `satis-veri-operasyonlari` | Satış Veri Operasyonları | Operator | **NOINDEX** | Daily sales batch processing screens, sync history, and order reconciliation. |
| `veri-hazirligi-guvenilirlik` | Veri Hazırlığı ve Analitik Güvenilirlik | Operator | **NOINDEX** | Minimum sample size criteria and data readiness checks before enabling analytics. |
| `api-entegrasyonu` | API Entegrasyonu ve Veri Sözleşmeleri | Technical Buyer / Developer | **INDEX** | REST ingestion contracts, payload specifications, API key authentication, and payloads. |
| `veri-sonrasi` | Veri Yüklendikten Sonra Ne Olur? | Prospect / Customer | **INDEX** | End-to-end data processing pipeline from raw ingestion to model snapshots and alerts. |

---

### Category 7: Kullanım Rehberleri (Usage Guides & Administration) — 8 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `gmail-kisi-aktarimi` | Gmail / Outlook Kişi Aktarımı | Operator | **NOINDEX** | Step-by-step CSV export and import instructions for webmail contact address books. |
| `kisi-listesi-ve-segmentler` | Kişi Listesi ve Segmentler | Operator | **NOINDEX** | Operator contact management screen: table filters, contact tagging, and profile edits. |
| `segment-yonetimi-ve-filtreler` | Dinamik Segment Oluşturma ve Kural Filtreleri | Operator | **NOINDEX** | Procedural rule builder instructions: combining AND/OR conditions and previewing sizes. |
| `kategori-yonetimi` | Kategori Yönetimi | Operator | **NOINDEX** | Catalog category tree builder: parent/child nesting and ERP integration code sync. |
| `kullanici-roller-yetkiler` | Kullanıcı Rolleri ve Yetkilendirme | Admin | **NOINDEX** | RBAC administration screen: assigning Admin, Operator, and Viewer permissions. |
| `guvenlik-ve-veri-izolasyonu` | Güvenlik ve Veri İzolasyonu | Prospect / Technical Buyer | **INDEX** | Multi-tenant logical data isolation, encryption in transit/rest, and access safeguards. |
| `audit-ve-uyumluluk` | Denetim İzi ve Uyumluluk | Prospect / Compliance | **INDEX** | Audit trail logging, operational traceability, KVKK subject rights compliance. |
| `hata-yonetimi-ve-guvenli-mod` | Hata Yönetimi ve Güvenli Mod | Customer / Technical | **INDEX** | System resilience principles: graceful degradation, schema fallback, and circuit breakers. |

---

### Category 8: Ölçüm ve Analitik (Measurement & Analytics) — 10 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `bi-kokpit` | BI Kokpit | Prospect / Customer | **INDEX** | Executive dashboard summarizing revenue, active customers, retention, and campaign impact. |
| `urun-haritasi` | Ürün Haritası | Prospect / Customer | **INDEX** | Visual product matrix mapping SKU performance by customer volume and repeat frequency. |
| `urun-kategori-performansi` | Ürün ve Kategori Performansı | Prospect / Customer | **INDEX** | Comparative revenue, quantity, and classification coverage metrics across catalog categories. |
| `magaza-performans-skoru` | Mağaza Karşılaştırması ve Sıralamalar | Prospect / Customer | **INDEX** | Multi-axial store benchmarking across revenue, growth rank, and customer repeat rates. |
| `kanal-performansi` | Satış Kanalı Performansı | Prospect / Customer | **INDEX** | Commercial sales channel analysis (Store vs Web vs App) and omnichannel customer overlap. |
| `kampanya-performansi-ve-olcumleme` | Kampanya Performansı ve Ölçümleme Mantığı | Prospect / Customer | **INDEX** | 3-tier measurement framework: delivery, engagement, and windowed attribution. |
| `teslimat-konsolu` | Teslimat Konsolu ve Gönderim Takibi | Operator | **NOINDEX** | Operator log screen for tracking individual message dispatches and delivery statuses. |
| `basarisiz-yeniden-deneme` | Başarısız Gönderimler ve İletişim Güvenliği | Operator | **NOINDEX** | Retry queue operations: separating transient provider errors from permanent opt-outs. |
| `gonderim-hizlandirma` | Gönderim Yönetimi ve İletişim Güvenliği | Customer / Operator | **INDEX** | Throughput optimization, rate limiting, and ISP reputation preservation policies. |
| `gonderim-operasyonu-izleme` | Gönderim Operasyonu ve İzleme | Operator | **NOINDEX** | Low-level dispatch monitor: worker health, queue status, and failed job review pools. |

---

### Category 9: SSS ve Kaynaklar (FAQ & Resources) — 11 Articles
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `ai-rolu-guven-siniri` | AI'ın Rolü ve Güven Sınırı | Prospect / Customer | **INDEX** | Clarification of AI boundaries: deterministic calculations vs creative text assistance. |
| `en-iyi-uygulamalar` | En İyi Uygulamalar ve Tavsiyeler | Prospect / Customer | **INDEX** | Strategic guidance on data quality, progressive segmentation, and sustainable cadence. |
| `hizli-baslangic-senaryolari` | Hızlı Başlangıç Senaryoları | Customer / Operator | **INDEX** | Concrete 7-day, 14-day, and 30-day operational milestone blueprints for new deployments. |
| `ornek-kampanya-kurgulari` | Örnek Kampanya Kurguları | Customer / Operator | **INDEX** | Industry campaign recipes for win-back, repeat purchase, VIP appreciation, and cross-sell. |
| `ozellik-veri-gereksinimleri` | Özellik → Veri Gereksinimi Matrisi | Prospect / Customer | **INDEX** | Complete capability-to-data mapping defining minimum evidence required for each feature. |
| `pika-pilot-ipuclari` | Pika Pilot Kullanım İpuçları | Customer / Operator | **INDEX** | Practical prompting tips and context best practices for AI campaign generation. |
| `sik-yapilan-hatalar` | Sık Yapılan Hatalar ve Kaçınma Yolları | Customer / Operator | **INDEX** | Critical operational pitfalls: over-messaging, unsegmented blasts, ignoring opt-outs. |
| `sss` | Sık Sorulan Sorular | Prospect / Customer | **INDEX** | Authoritative FAQ answering product, data, AI, attribution, and channel questions. |
| `sozluk` | Pika Sözlüğü | Prospect / Customer | **INDEX** | Comprehensive glossary defining all commercial, analytical, and marketing terminology. |
| `teknik-altyapi-ve-guvenlik` | Teknik Altyapı ve Güvenlik | Prospect / Technical Buyer | **INDEX** | Architecture overview, data sovereignty, encryption standards, and hosting model. |
| `veri-hazirlama-rehberi` | Veri Hazırlama Rehberi | Customer / Operator | **INDEX** | Formatting standards, date/currency conventions, and identifier hygiene for initial data. |

---

### Off-Navigation Technical Page — 1 Article
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `gonderim-son-katman` | Gönderim Karar Zincirinin Son Katmanıdır | Operator / Technical | **NOINDEX** | Deep operational principle note on delivery mechanics; omitted from primary nav bar. |

---

## 4. Quarantined Internal Engineering Wiki (15 Articles)

The following articles contain proprietary engineering designs, database entity relationships, worker architectures, and deployment pipelines. They are quarantined under `/internal/wiki/` and served via `InternalWikiController` requiring authenticated employee access (`[Authorize]`):

1. `internal-mimari-genel-bakis`: Platform monolith, ASP.NET Core MVC/API gateway, Angular SPA host, and Hangfire worker boundaries.
2. `internal-api-mimarisi-ve-veri-kontratlari`: Ingestion endpoint contracts, DTO schemas, bearer JWT tokens, and rate limits.
3. `internal-teslimat-konsolu-ve-worker-mimarisi`: Multi-threaded delivery pipeline, channel provider adapter architecture, and worker pools.
4. `internal-retry-politikasi-ve-hata-yonetimi`: Exponential backoff algorithm, transient HTTP status handling, and dead-letter review quarantine.
5. `internal-gonderim-izleme-ve-telemetri`: Distributed OpenTelemetry tracing, Serilog structured sink configuration, and Prometheus metrics.
6. `internal-veri-kalitesi-ve-anomali-denetimi`: Statistical outlier detection, schema type validation pipelines, and null-ratio thresholding.
7. `internal-musteri-deger-skoru-algoritmasi`: Mathematical normalization formulas, quantile scoring, percentile weighting, and DB materialized views.
8. `internal-magaza-metrik-hesaplama-mimarisi`: Nightly batch rollup jobs, store rank caching, and partition-level aggregation queries.
9. `internal-cross-sell-sepet-analizi-motoru`: Apriori association rule mining, support/confidence/lift matrix generation, and cache invalidation.
10. `internal-product-intelligence-resolution-mimarisi`: Fuzzy string matching, tokenization, Levenshtein distances, and category mapping tables.
11. `internal-cce-karar-motoru-mimarisi`: Customer Context Engine snapshot store, state machine transitions, and daily opportunity generator.
12. `internal-ai-mimarisi-ve-prompt-yonetimi`: LLM provider client abstractions, system prompt templates, context token budgeting, and guardrail filters.
13. `internal-guvenlik-ve-yetkilendirme-mimarisi`: ASP.NET Core Identity claims, tenant boundary filters, cryptographic secret rotation, and anti-forgery.
14. `internal-hangfire-ve-arka-plan-is-yonetimi`: Cron job definitions, Hangfire SQL storage, recurring queue priorities, and concurrency semaphores.
15. `internal-surum-yonetimi-ve-deployment-pipeline`: Git branch release strategy, Docker container build pipelines, DB migrations, and health checks.

---

## 5. Governance Enforcement & Automated Regression Testing

To prevent regression or accidental drift, the following automated tests in `Pika.Web.Tests/WikiTests.cs` and `Pika.Web.Tests/WikiSplitGenerator.cs` enforce this specification:

- `AllSitemapWikiUrls_Return200OK`: Asserts exactly 59 Wiki URLs in `sitemap.xml` (root + 58 INDEX articles) and verifies sample NOINDEX URLs are excluded.
- `PublicInternalBoundary_ContainsNoInternalArticlesOrNav`: Asserts 0 internal articles in public wiki JSON, public nav, or public views.
- `NavIntegrity_AllSlugsResolve_NoBrokenRelated`: Asserts all 87 nav slugs and 88 public pages resolve with 0 broken `related` links.
- `CustomerValueScore_ReflectsCanonicalFourFactorFormula_OmitsRhythmFromScore`: Asserts CVS has factors 0.40, 0.25, 0.20, 0.15, Rhythm is independent context, and `%60 ciro + %40 sıklık` is 100% absent.
- `Channels_DoNotMarketPushAsActiveChannel_EmailSmsWhatsAppOnly`: Asserts Push is completely removed from public channels.
- `RepeatPurchase_ExcludesUnsupportedForecastsAndProbabilities_UsesCycleWindow`: Asserts canonical 80%–120% cycle window and 0 fake forecast/probability claims.
- `AiRole_AssertsHumanInTheLoop_NoAutonomousSending`: Asserts Human-in-the-loop and absence of autonomous sending claims.
- `OpportunityConfidence_FramesAsEvidenceQuality_NotStandaloneScoreEngine`: Asserts confidence is framed as evidence quality (`Kanıt Yeterliliği`).
- `HighRiskClaims_AbsenceOfSoc2Iso27001SamlSsoAndGuarantees`: Asserts absence of SOC 2, ISO 27001, SAML SSO, 99.99% SLA, or revenue guarantees.
- `RobotsMeta_EmitsSingleTag_IndexFollowOrNoindexFollow`: Asserts INDEX pages emit `index, follow`, NOINDEX emit `noindex, follow`, with 0 duplicate or conflicting tags.
- `AssetIntegrity_AllReferencedImagesExistOnDisk`: Asserts all 29 referenced screenshots exist in `wwwroot/wiki/assets/images/`.
- `WikiGovernance_GovernanceCountsMatch58Index30Noindex`: Asserts exactly 88 public pages, 58 indexable, and 30 noindex.
- `GenerateWikis`: Asserts idempotent wiki generation without drift when `REGENERATE_WIKI=1` is executed.
