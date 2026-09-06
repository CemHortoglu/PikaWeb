# P00 Executive Findings: Canonical Product Truth, Claims & Governance Audit

**Project:** Pika Web 2.0  
**Phase:** P00.1 — Audit Hardening & Commit Hygiene  
**Target Repository:** `CemHortoglu/PikaWeb`  
**Audit Date:** September 2026  
**Audited Surface:** 10 Controllers, 59 Razor Views, 88 Public Wiki Articles, 15 Internal Engineering Wiki Articles, Services (`SeoHelper`, `LegacyRouteMapper`, `WikiService`), `Program.cs`, Configuration files, `sitemap.xml`, `robots.txt`, `llms.txt`, `llms-full.txt`, and 29 product visual assets.

---

## Executive Summary

This audit establishes an unyielding, evidence-backed foundation of canonical truth for Pika before any visual redesign, copywriting, or architectural restructuring begins.

### Repository Boundary & Evidence Axiom
`PikaWeb` is the public website repository and documentation hub; it does **not** contain the execution backend of `app.pika.tr` or `api.pika.tr`. Therefore, the absence of backend implementation code in PikaWeb is **not** proof of non-existence.

All findings are strictly evaluated under a 5-level evidence hierarchy:
- **Level A (`CODE_VERIFIED`):** Implementation code inspected in PikaWeb.
- **Level B (`DOCUMENTED`):** Supported by authoritative internal/public documentation (`internal_wiki.json`, `wiki.json`), but backend implementation code was not inspected in PikaWeb.
- **Level C (`MARKETING_ONLY`):** Found only in marketing views, legacy files, or SEO tags; unsupported by documentation or code in this repository.
- **Level D (`CONTRADICTORY`):** Sources disagree.
- **Level E (`UNVERIFIED`):** Insufficient repository evidence.

All audit observations are explicitly grouped into three core governance categories:
1. **CONFIRMED WEBSITE ISSUE:** Concrete bugs, broken assets, route inversions, sitemap mismatches, and orphan files directly verified in PikaWeb code.
2. **DOCUMENTED PRODUCT CAPABILITY:** Sophisticated backend architectures, mathematical formulas, data contracts, and safety protocols authoritatively documented in the repository.
3. **PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION:** Capabilities claimed in marketing or legacy files that conflict with documentation or lack proof in PikaWeb, requiring product management sign-off before being publicly marketed.

---

## 1. CONFIRMED WEBSITE ISSUES (Code-Verified in PikaWeb)

These issues exist directly within the `PikaWeb` codebase and represent concrete defects, governance debt, or presentation gaps requiring remediation in subsequent phases.

### 1.1 Live HTTP 404 Broken Image on Flagship Solution Page
- **Evidence Level:** `A. CODE_VERIFIED`
- **Location:** `Views/Solutions/CampaignManager.cshtml:232`
- **Code:** `<img src="/wiki/assets/images/img_kampanya-yonetimi_0.png" ... />`
- **Defect:** The referenced image file does not exist on disk. Renders a visible 404 broken image icon on a core sales landing page.
- **Remediation:** Replace with authentic screenshot `img_yayinlama-sablon-ve-yonetim_27.png` in Phase P01.

### 1.2 Severe Route / Content Inversions (3 Solution Routes)
- **Evidence Level:** `A. CODE_VERIFIED`
- **Defect:** Three solution routes deliver completely different subject matter than their URLs promise:
  1. **`/cozumler/personalization`** renders `Views/Solutions/Personalization.cshtml`, titled *"Müşteri Etkileşim Yönetimi / Journey Orchestration"*. Its content is 100% journey automation (competing with `/cozumler/journey-manager`).
  2. **`/cozumler/template-management`** renders `Views/Solutions/TemplateManagement.cshtml`, titled *"Müşteri Segmentasyonu ve Hedefleme / Audience Segmentation & Targeting"*. Its content is 100% audience rule trees (competing with `/cozumler/audience-manager`).
  3. **`/cozumler/ab-testing`** renders `Views/Solutions/ABTesting.cshtml`, titled *"Kampanya Otomasyonu ve Zamanlama / Campaign Automation & Scheduling"*. Its content is 100% campaign scheduling (competing with `/cozumler/campaign-manager`).
- **Remediation:** Implement 301 redirects to canonical routes in Phase P01.

### 1.3 Duplicate / Self-Cannibalizing Landing Pages (4 Route Pairs)
- **Evidence Level:** `A. CODE_VERIFIED`
- **Defect:** Four duplicate solution route pairs split search equity and confuse prospects:
  - `/kanallar/whatsapp-kampanya-yonetimi` cannibalizes `/kanallar/whatsapp`
  - `/kanallar/email-marketing-template-studio` cannibalizes `/kanallar/email` and `/cozumler/content-studio`
  - `/cozumler/iys-kvkk-uyumlu-kampanya-yonetimi` cannibalizes `/cozumler/consent-management`
  - `/cozumler/e-ticaret-ai-kampanya-yonetimi` cannibalizes `/urunler/ai-kampanya-asistani`
- **Remediation:** Consolidate onto canonical routes via 301 redirects in Phase P01.

### 1.4 Sitemap Inconsistency Between Turkish and English
- **Evidence Level:** `A. CODE_VERIFIED`
- **Location:** `wwwroot/sitemap.xml`
- **Defect:** Six thin prototype stub pages (`personalization`, `template-management`, `ab-testing`, `deliverability-compliance`, `data-management-etl`, `real-time-event-processing`) were commented out in the Turkish sitemap with the note: *"Thin stub pages removed... Add back when content is substantially improved"*. However, in lines 276-309, all 6 pages were **left active in the English sitemap**, exposing thin, mismatched URLs to international search indexers.
- **Remediation:** Harmonize the English sitemap section in Phase P01.

### 1.5 Headquarters Office Address Contradiction
- **Evidence Level:** `D. CONTRADICTORY` / `A. CODE_VERIFIED`
- **Location:** `appsettings.json:19` vs `Views/Shared/_Layout.cshtml:114`
- **Defect:** `appsettings.json` explicitly lists the physical office and Google Maps coordinates in **Ankara** (`Teknopark Turkuaz Bina, Ostim / Yenimahalle / Ankara`). However, `_Layout.cshtml` JSON-LD schema hardcodes `"addressLocality": "İstanbul"`.
- **Remediation:** Align JSON-LD structured data with canonical company registration.

### 1.6 Unverified Performance Metrics Hardcoded in Production HTML
- **Evidence Level:** `A. CODE_VERIFIED` (in HTML) / `E. UNVERIFIED` (as customer proof)
- **Location:** `Views/Solutions/CampaignManager.cshtml:133, 397, 407, 417`
- **Defect:** Metrics including `ROAS 14.2x`, `₺184.600 Sipariş Tutarı`, `%52 Açılma Oranı`, `%31 Ziyaret Artışı`, and `%41 Satış Dönüşümü` are hardcoded directly into strategy cards without sample/demo disclosure.
- **Remediation:** Add mandatory `ÖRNEK SENARYO` / `TEMSİLİ METRİK` labels in Phase P01.

### 1.7 Orphan & Abandoned Views on Disk
- **Evidence Level:** `A. CODE_VERIFIED`
- **Defect:**
  - `Views/Home/Pricing.cshtml`: Contained hardcoded prices (`₺9.900/ay`, `₺24.900/ay`, `Enterprise Özel`) and SLA claims. Classified as **`RETIRED / DELETED IN P01`**. Deleted from disk in Phase P01; `/fiyatlandirma` (`/en/pricing`) route permanently 301-redirects to `/demo-talebi` (`/en/demo-request`). Pika operates strictly on a non-negotiable quotation-based commercial policy (*"İhtiyacınıza ve kullanım kapsamınıza göre özel teklif"*).
  - `Views/Home/Solutions.cshtml`: Unrouted legacy hub containing broken internal links.
  - `Views/Solutions/JourneyManager.cshtml.bak`: Abandoned editor backup file left in production directory.
- **Remediation:** In Phase P01, delete `Pricing.cshtml` (completed), redirect pricing routes to `/demo-talebi` (completed), delete `JourneyManager.cshtml.bak` (completed), and archive `Solutions.cshtml`. Ensure no fixed package prices or calculators are ever exposed publicly.

### 1.8 Developer Meta-Commentary Left in Public View
- **Evidence Level:** `A. CODE_VERIFIED`
- **Location:** `Views/Solutions/DeliverabilityCompliance.cshtml:9`
- **Defect:** Public page contains informal developer rationale: *"Burası pazarlama metninde genelde “sıkıcı” gibi görünür ama satışta en büyük güven artırıcılardan biridir... bu başlığı ayrı bir çözüm olarak sunmak kurumsal duruşu güçlendirir."*
- **Remediation:** Cleanse internal developer notes in Phase P01.

### 1.9 Visual Product-Proof Gap (28 Hidden Screenshots)
- **Evidence Level:** `A. CODE_VERIFIED`
- **Location:** `wwwroot/wiki/assets/images/*.png` (29 files)
- **Defect:** The repository contains 29 authentic, high-resolution screenshots from `https://app.pika.tr`, but **28 of them remain isolated in wiki folders**. Flagship marketing pages display hand-crafted CSS mockups instead of authentic UI proof.
- **Remediation:** Deploy authentic screenshots across marketing pages during Phase P01.

### 1.10 Outdated LLM Ingestion File
- **Evidence Level:** `A. CODE_VERIFIED`
- **Location:** `wwwroot/llms-full.txt`
- **Defect:** References non-existent URLs (`/en/solutions/email-marketing`, `/en/home/corporate`) and makes unsupported claims (SOC-2 compliance, SAML 2.0, chatbots).
- **Remediation:** Overhaul `llms-full.txt` to match `PRODUCT_TRUTH.md` in Phase P01.

---

## 2. DOCUMENTED PRODUCT CAPABILITIES (Documented in Repo)

These capabilities represent authentic engineering systems, mathematical models, and architectural workflows documented authoritatively in `App_Data/internal_wiki.json` and `App_Data/wiki.json`. Because backend code is hosted outside PikaWeb, these are classified as `B. DOCUMENTED`.

### 2.1 Customer Value Score (CVS) Engine
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-musteri-deger-skoru-algoritmasi`, `App_Data/wiki.json: musteri-deger-skoru`)
- **Capability:** A deterministic 0-100 value score calculated via a weighted multi-factor formula:
  $$\text{CVS} = (\text{Monetary} \times 0.40) + (\text{Frequency} \times 0.25) + (\text{Recency} \times 0.20) + (\text{Loyalty} \times 0.15)$$
- **Governance:** Can be marketed as deterministic behavioral scoring; must never be claimed as "AI customer scoring".

### 2.2 Customer Context Engine (CCE) & Günün Fırsatları
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-cce-karar-motoru-mimarisi`, `App_Data/wiki.json: gunun-firsatlari-ve-karar-motoru`)
- **Capability:** Daily opportunity discovery prioritizing:
  1. Risk & Suppression Gates (strict 7-day contact frequency capping, opt-out status)
  2. Churn / Win-Back Opportunities (individual rhythm inactivity decay)
  3. Repeat Purchase / Replenishment Opportunities (80% - 120% consumption cycle)
  4. Cross-Sell / Basket Affinity Opportunities
- **Governance:** Core proprietary commercial differentiator; fully approved for high-priority marketing.

### 2.3 Product Intelligence (PI) & Catalog Resolution
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-product-intelligence-resolution-mimarisi`, `App_Data/wiki.json: product-intelligence-nedir`, `need-group-product-role`)
- **Capability:** Semantic catalog enrichment beyond basic SKUs: Need Groups, Product Roles (Traffic Driver, Basket Builder, Margin Driver), and Commercial Playbooks. Multi-tier resolution: sanitization, alias matching, and trigram similarity matching (> 0.85) with analytical readiness gating.
- **Governance:** Fully approved for public positioning; authentic screenshot available (`img_need-group-product-role_11.png`).

### 2.4 Cross-Sell Basket Association Engine
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-cross-sell-sepet-analizi-motoru`, `App_Data/wiki.json: cross-sell-firsatlari`)
- **Capability:** Market basket association mining operating on historical transaction lines with explicit thresholds: Support ≥ 0.01, Confidence ≥ 0.15, and Lift > 1.2.
- **Governance:** Market as statistical association mining, not black-box AI.

### 2.5 Ingestion API & Data Contracts
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-api-mimarisi-ve-veri-kontratlari`, `App_Data/wiki.json: api-entegrasyonu`)
- **Capability:** Asynchronous REST ingestion (`/api/v1/ingest/`) returning `202 Accepted`; HMAC signature authentication; mandatory `X-Idempotency-Key` with 24-hour TTL; token bucket rate limiter capped at 1,000 requests/minute per tenant.
- **Governance:** Market as robust, enterprise-governed data ingestion; never claim "unlimited instant sync".

### 2.6 Delivery Infrastructure & Worker Architecture
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-teslimat-konsolu-ve-worker-mimarisi`, `internal-retry-politikasi-ve-hata-yonetimi`)
- **Capability:** Hangfire background processing; Job/Attempt/Event state machine; exponential backoff retry policy (maximum 5 attempts); Dead-Letter Queue (DLQ) for failed dispatches; SMS gateway rate throttling (50 requests/second).
- **Governance:** High-trust operational infrastructure; authentic screenshot available (`img_gonderim-operasyonu-izleme_28.png`).

### 2.7 AI Architecture & Zero-PII Boundary (Pika Pilot)
- **Evidence Level:** `B. DOCUMENTED` (`App_Data/internal_wiki.json: internal-ai-mimarisi-ve-prompt-yonetimi`, `App_Data/wiki.json: ai-rolu-guven-siniri`) & `A. CODE_VERIFIED` (`AiCampaignAssistant.cshtml`)
- **Capability:** Natural language prompt interpretation generating campaign title, suggested audience rule trees, and email template drafts.
- **Safety Safeguards:**
  - Strict zero-PII boundary: customer personal data (TCKN, phone, names) is never sent to external LLM APIs; only tokenized anonymous metrics are supplied.
  - 8-second fallback timeout returning deterministic templates if LLM providers fail.
  - Zero autonomous dispatch: human review and explicit approval are mandatory (`Faq.cshtml:325`).

### 2.8 Consent Governance & Preference Management
- **Evidence Level:** `B. DOCUMENTED` (`internal-teslimat-konsolu-ve-worker-mimarisi`) & `A. CODE_VERIFIED` (`ContactConsentController.cs`)
- **Capability:** Pre-dispatch IYS query and automated contact suppression; KVKK opt-out DB lookup; tokenized public web preference portal (`/contact-consent/{token}`) protected by Cloudflare Turnstile.

---

## 3. PRODUCT IMPLEMENTATION REQUIRES CONFIRMATION

These items cannot be verified as commercially operational from the `PikaWeb` repository alone. They represent marketing claims, contradictory statements, or architectural uncertainties that require explicit product confirmation before public release:

### 3.1 Push Notification Commercial Readiness
- **Evidence Level:** `D. CONTRADICTORY`
- **Contradiction:** Marketed in views (`CampaignManager.cshtml`, `_Layout.cshtml`) and `SeoHelper.cs` as an active native channel; but marked `<h3>Push Notifications (Roadmap)</h3>` in `Views/Home/Solutions.cshtml:39` and completely omitted from `internal_wiki.json` delivery worker gateways.
- **Action:** Confirm whether any third-party push gateway is live in `app.pika.tr`. Until verified, enforce `(Roadmap)` qualifier across all public marketing pages.

### 3.2 Enterprise SSO / SAML 2.0
- **Evidence Level:** `C. MARKETING_ONLY`
- **Contradiction:** Claimed in legacy `llms-full.txt:121`. PikaWeb implements only cookie auth and JWT sliding token exchange (`Program.cs`, `AccountController.cs`).
- **Action:** Confirm whether SAML 2.0 / Okta / Azure AD federation is supported in `app.pika.tr` identity server. Mark as `DO_NOT_MARKET` until confirmed.

### 3.3 Conversational WhatsApp Chatbots (Two-Way Conversational AI)
- **Evidence Level:** `D. CONTRADICTORY` / `C. MARKETING_ONLY`
- **Contradiction:** Claimed in `llms-full.txt:66` ("chatbot flows, and two-way conversations with customers"). Documentation specifies outbound template dispatches via WhatsApp Business API.
- **Action:** Confirm whether any conversational AI dialog engine exists in `app.pika.tr`. Position strictly as outbound notification/campaign management until confirmed.

### 3.4 Real-Time Latency SLA
- **Evidence Level:** `D. CONTRADICTORY`
- **Contradiction:** Marketing copy claims "milisaniyeler içinde" (sub-millisecond streaming). Documentation describes asynchronous REST buffers, Redis caches, and Hangfire worker queues operating in seconds.
- **Action:** Confirm measured event-to-dispatch latency SLA. Enforce "saniyeler içinde olay bazlı tetikleme" in all copy.

### 3.5 A/B Testing Maturity & Statistical Engine
- **Evidence Level:** `C. MARKETING_ONLY`
- **Contradiction:** Legacy `llms-full.txt:86` claims automated multi-armed bandit winner selection and statistical significance calculation. Repo views only show manual variant allocation.
- **Action:** Confirm whether an automated statistical winner engine is operational in `app.pika.tr`. Market strictly as variant testing until confirmed.

### 3.6 Commercial Client & Delivery Volume Proof
- **Evidence Level:** `E. UNVERIFIED`
- **Uncertainty:** Zero client brand names, approved customer logos, or verified historical delivery volumes exist in the repository.
- **Action:** Request approved client logos, testimonial quotes, and aggregate metrics from leadership before introducing social proof sections.

### 3.7 Provenance of Campaign Manager Metrics
- **Evidence Level:** `E. UNVERIFIED`
- **Uncertainty:** In `Views/Solutions/CampaignManager.cshtml`, `ROAS 14.2x`, `₺184.600 Sipariş Tutarı`, `%52 Açılma Oranı`, `%31 Ziyaret Artışı`, and `%41 Satış Dönüşümü` are displayed without context.
- **Action:** Confirm whether these are from an actual historical customer case study or are synthetic illustrative examples requiring mandatory `ÖRNEK SENARYO` labels.

### 3.8 Physical Headquarters Location
- **Evidence Level:** `D. CONTRADICTORY`
- **Contradiction:** `appsettings.json:19` lists office in Teknopark Ostim / Ankara; `Views/Shared/_Layout.cshtml:114` JSON-LD schema hardcodes İstanbul.
- **Action:** Confirm official corporate registration city to align Schema.org structured data.

---

## Input Directives for Phase P01 (Information Architecture & Strategy)

1. **Fix Broken Asset:** Point `CampaignManager.cshtml:232` to `img_yayinlama-sablon-ve-yonetim_27.png`.
2. **Execute 301 Redirect Plan:** Resolve the 3 route inversions and 4 duplicate route pairs.
3. **Clean English Sitemap:** Remove the 6 thin stubs from the English section of `sitemap.xml`.
4. **Enforce Push Qualifier:** Add `(Roadmap)` badge to all Push navigation and heading elements.
5. **Deploy Real Screenshots:** Transition marketing pages from hand-built CSS mockups to the 28 authentic screenshots in `VISUAL_ASSET_REGISTRY.md`.
6. **Rewrite `llms-full.txt`:** Remove SOC-2, SAML, and Chatbot claims; align URLs with canonical routes.
7. **Label Mockup Data:** Add `ÖRNEK SENARYO` / `TEMSİLİ GÖSTERGE` badges to all synthetic performance metrics.
8. **Enforce Quotation-Based Pricing Governance:** Ensure no fixed package prices, tiers, or calculators are introduced. Archive/remove `Views/Home/Pricing.cshtml` from public routing. Align all pricing CTAs with `Teklif Al`, `Demo Talep Et`, or `Satış Ekibiyle Görüşün`.
