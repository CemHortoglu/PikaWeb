# Pika Web 2.0 Design Constitution

**Phase:** P02 / P02.1 — Brand & Web Design Constitution (Accessibility & Contract Hardening)  
**Target Repository:** `CemHortoglu/PikaWeb`  
**Governing Inputs:** `PRODUCT_TRUTH.md`, `CLAIMS_REGISTRY.md`, `CONTENT_GUARDRAILS.md`, `ENTITY_REGISTRY.md`, `PUBLIC_ROUTE_AUDIT.md`, `VISUAL_ASSET_REGISTRY.md`, `P00_FINDINGS.md`, `P01_INFORMATION_ARCHITECTURE.md`.  
**Namespace:** `.pw2-*`  
**Foundational Stylesheet:** `Pika/wwwroot/css/pika-design-system.css`

---

## 1. Design Mission

Pika Web 2.0 exists to visually communicate an operationally serious, enterprise-oriented B2B software platform: **"Müşteri Zekâsı ve Omnichannel Pazarlama Platformu"** (Customer Intelligence & Omnichannel Marketing Platform).

Pika is trusted with customer, product, transaction, and campaign workflows. The visual design must communicate:
- **Enterprise Gravitas:** A solid, reliable platform engineered for retail, e-commerce, and multi-store commercial operations.
- **Analytical & Decision Depth:** Visualizing intelligence before action (Customer Context Engine, Product Intelligence, Customer Value Scoring, and Günün Fırsatları).
- **Operational Clarity:** High-density, understandable interfaces where technical complexity is mastered rather than masked behind empty whitespace.
- **Authenticity Over Illusion:** Real product proof from `https://app.pika.tr` is the centerpiece of the visual argument.

### What Pika Web 2.0 Must NOT Look Like
- A lightweight AI wrapper or toy generator.
- A generic SaaS card-grid marketplace template.
- A crypto, web3, or gamified fintech app.
- A consumer lifestyle or social application.
- An unstyled Bootstrap developer demo.
- A collection of disconnected pages styled independently.

**Quality Reference Standard:** SmartMessage and Related Digital class enterprise marketing sites, with the structural discipline and visual control of mature global martech leaders (Braze, Insider, Bloomreach, Optimove). These are quality benchmarks only; Pika retains its own unique visual identity and must never clone competitor layouts, illustrations, or typography.

---

## 2. Brand Character

Pika’s brand character is grounded in operational trust and technical intelligence:

| Core Trait | Visual Manifestation in Pika Web 2.0 | What to Strictly Avoid |
| :--- | :--- | :--- |
| **Calm** | Controlled whitespace, restrained palettes, quiet backgrounds (`#ffffff`, `#f8faf9`). | Neon flashes, noisy textures, aggressive alert banners. |
| **Precise** | Sharp typography hierarchy, hairline borders (`1px solid #e3ede8`), structured data alignment. | Arbitrary offset borders, sloppy card padding, irregular margins. |
| **Intelligent** | Clear workflow diagrams, structured decision matrices, authentic software telemetry. | Decorative stock diagrams, sci-fi nodes, floating abstract 3D spheres. |
| **Premium** | Subdued elevation shadows, solid dark petrol structural chapters (`#0c3a30`), high-resolution screenshots. | Cheap drop shadows, gradient text overloads, glassmorphism blur. |
| **Technical** | Explicit data contracts, formula explanations, clear operational states. | Oversimplified consumer illustrations, cartoon mascots, hand-waving abstractions. |
| **Human** | Readable editorial type sizes, clear human approval steps in AI workflows, contextual explanations. | Cold, robotic cyber aesthetics or fake AI-generated avatar headshots. |
| **Controlled** | Lime accent (`#84c225` / `#9edd05`) used with surgical restraint (CTAs, focus rings, small badges). | Drenching whole sections or borders in neon lime green. |

---

## 3. Non-Negotiable Visual Rules

1. **Anti-Card Grid Rule:** Do NOT build pages out of repeated 3-column card cemeteries. If information can be expressed through hierarchy, editorial split, timeline, or direct typography, it must NOT be forced into a card.
2. **Real Product First:** Hand-built CSS dashboard mockups are strictly prohibited when an authentic screenshot exists in `wwwroot/wiki/assets/images/`.
3. **Accent Restraint:** The lime accent (`#84c225` / `#9edd05`) is strictly an *accent*. It may highlight active states, primary CTA buttons, and directional arrows; it must never become the background or border of every element.
4. **No Decorative Gimmicks:** The following are strictly banned:
   - Coloured left/right card borders (e.g. `border-left: 4px solid green`).
   - Gradient borders and animated rainbow borders.
   - Glassmorphism (`backdrop-filter: blur()`) with semi-transparent surfaces.
   - Neon outer glows (`box-shadow: 0 0 20px #84c225`).
   - Giant blurred color blobs (`filter: blur(80px)`).
   - Floating emoji or emoji-like sticker icons.
   - Arbitrary floating 3D objects or isometric stock illustrations.
5. **No Visual Leakage:** New styling must live under the `.pw2-*` namespace. No unnamespaced element rules (`body`, `h1`, `p`, etc.) may be defined globally in `pika-design-system.css`.
6. **Metric Safety Compliance:** All sample metrics and dashboard numbers must carry mandatory `ÖRNEK SENARYO` / `TEMSİLİ GÖSTERGE` disclosure badges per `CONTENT_GUARDRAILS.md`.

---

## 4. Existing Token Reconciliation

`Pika/wwwroot/css/pika-components.css` contains an existing design token foundation on `:root`. To prevent competing token systems and provide a clean migration path, P02 explicitly reconciles every foundational token under the `.pw2-*` token system.

### Classification Key
- **`ADOPT`**: Preserves the established Pika value identically under the modern `--pw2-*` token family.
- **`ALIAS`**: Directly maps an existing shorthand token to its canonical semantic token.
- **`REFINE`**: Adjusts a value slightly to satisfy explicit P02 design/accessibility principles with documented rationale.
- **`NEW`**: Introduces an essential design token required by P02 section archetypes or WCAG AA compliance.

### Token Reconciliation Matrix

| Category | Existing Token (`pika-components.css`) | New PW2 Token | Status | Value | Rationale |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Brand Primary** | `--pika-color-brand-primary` | `--pw2-brand-primary` | `ADOPT` | `#84c225` | Canonical Pika lime green brand color. Unchanged. |
| **Primary Hover** | `--pika-color-brand-primary-hover` | `--pw2-brand-primary-hover` | `ADOPT` | `#73aa20` | Darker lime for active and hover states. |
| **Primary Dark** | `--pika-color-brand-primary-dark` | `--pw2-brand-primary-dark` | `ADOPT` | `#5c8c15` | Retained for non-text graphics (4.03:1 on white). |
| **Accent Ink** | *(None)* | `--pw2-accent-ink` | `NEW` | `#3f5802` | High-contrast brand text ink: 8.05:1 on white, 7.54:1 on tint (WCAG AA compliant >= 4.5:1). |
| **Brand Secondary** | `--pika-color-brand-secondary` | `--pw2-brand-secondary` / `--pw2-bg-dark` | `ADOPT` | `#0c3a30` | Deep petrol/forest green structural background. |
| **Secondary Hover** | `--pika-color-brand-secondary-hover`| `--pw2-brand-secondary-hover`| `ADOPT` | `#124d40` | Subtle hover state for dark buttons/links. |
| **Brand Accent** | `--pika-color-brand-accent` | `--pw2-accent` | `ADOPT` | `#9edd05` | High-visibility lime highlight accent. |
| **Surface Tint** | `--pika-color-brand-surface-tint` | `--pw2-bg-tint` | `ADOPT` | `#f4f9f1` | Very light brand green wash for eyebrows and highlights. |
| **Border Tint** | `--pika-color-brand-border-subtle` | `--pw2-border-tint` | `ADOPT` | `#dbeec8` | Soft green border for badges and chips. |
| **Canvas Pure** | `--pika-color-bg` | `--pw2-bg-primary` | `ADOPT` | `#ffffff` | Pure white default background. |
| **Surface Subtle** | `--pika-color-surface-subtle` | `--pw2-bg-subtle` | `ADOPT` | `#f8faf9` | Alternating section surface. |
| **Surface Muted** | `--pika-color-surface-muted` | `--pw2-bg-muted` | `ADOPT` | `#f1f6f3` | Inset wells, code blocks, table headers. |
| **Surface Dark** | `--pika-color-surface-dark` | `--pw2-bg-dark` | `ADOPT` | `#0c3a30` | Solid structural dark chapter background. |
| **Dark Deep** | `--pika-color-surface-dark-subtle`| `--pw2-bg-dark-deep` | `ADOPT` | `#082922` | Deep petrol anchor for stage insets and frames. |
| **Stage Surface** | *(None)* | `--pw2-surface-stage` | `NEW` | `#0e322a` | High-contrast framing surface for authentic screenshots. |
| **Text Primary** | `--pika-color-text-primary` | `--pw2-text-primary` | `ADOPT` | `#0d2821` | Deep charcoal-petrol ink: 15.64:1 on white, 14.92:1 on subtle. |
| **Text Secondary** | `--pika-color-text-secondary` | `--pw2-text-secondary` | `ADOPT` | `#365147` | Editorial body text: 8.66:1 on white, 8.26:1 on subtle. |
| **Text Muted** | `--pika-color-text-muted` (`#5e7a70`) | `--pw2-text-muted` | `REFINE` | `#4a685e` | Refined to established `--ph-text-muted` value. Achieves 6.12:1 on white, 5.84:1 on subtle (safely exceeds 4.5:1 on both). |
| **Text Subtle** | `--pika-color-text-subtle` | `--pw2-text-subtle` | `ADOPT` | `#8da49c` | Non-critical placeholders, disabled elements only. |
| **Text On Dark** | `--pika-color-text-on-dark` | `--pw2-text-on-dark` | `ADOPT` | `#ffffff` | Pure white text on dark petrol: 12.62:1 contrast. |
| **On Dark Muted** | `--pika-color-text-on-dark-muted` | `--pw2-text-on-dark-muted` | `ADOPT` | `rgba(255, 255, 255, 0.74)` | 7.65:1 effective contrast on dark petrol. |
| **Focus on Light**| *(None)* | `--pw2-focus-on-light` | `NEW` | `#3f5802` | High-contrast focus indicator on light surfaces (8.05:1 on white, exceeds 3:1 non-text threshold). |
| **Focus on Dark** | *(None)* | `--pw2-focus-on-dark` | `NEW` | `#9edd05` | High-contrast focus indicator on dark petrol surfaces (7.69:1 on #0c3a30, exceeds 3:1 non-text threshold). |
| **Border Default** | `--pika-color-border` | `--pw2-border-default` | `ADOPT` | `#e3ede8` | Crisp structural border. |
| **Border Subtle** | `--pika-color-border-subtle` | `--pw2-border-subtle` | `ADOPT` | `#edf4f0` | Soft hairline dividers. |
| **Border Strong** | `--pika-color-border-strong` | `--pw2-border-strong` | `ADOPT` | `#c2d8cd` | Input states and focused borders. |
| **Border Dark** | `--pika-color-border-dark` | `--pw2-border-dark` | `ADOPT` | `rgba(255, 255, 255, 0.12)` | Subtle hairline on dark chapters. |
| **Status Success** | `--pika-color-success` | `--pw2-status-success` | `ADOPT` | `#15803d` | Semantic success state. |
| **Status Warning** | `--pika-color-warning` | `--pw2-status-warning` | `ADOPT` | `#b45309` | Semantic warning / risk state. |
| **Status Danger** | `--pika-color-danger` | `--pw2-status-danger` | `ADOPT` | `#b91c1c` | Semantic error state. |
| **Status Info** | `--pika-color-info` | `--pw2-status-info` | `ADOPT` | `#0369a1` | Semantic informational state. |
| **Spacing 2XS** | `--pika-space-2xs` | `--pw2-space-2xs` | `ADOPT` | `4px` | Micro spacing / tight badge padding. |
| **Spacing XS** | `--pika-space-xs` | `--pw2-space-xs` | `ADOPT` | `8px` | Icon gaps, compact padding. |
| **Spacing SM** | `--pika-space-sm` | `--pw2-space-sm` | `ADOPT` | `12px` | Compact component spacing. |
| **Spacing MD** | `--pika-space-md` | `--pw2-space-md` | `ADOPT` | `16px` | Standard button and element padding. |
| **Spacing LG** | `--pika-space-lg` | `--pw2-space-lg` | `ADOPT` | `24px` | Standard component gutters. |
| **Spacing XL** | `--pika-space-xl` | `--pw2-space-xl` | `ADOPT` | `32px` | Major layout gutters. |
| **Spacing 2XL** | `--pika-space-2xl` | `--pw2-space-2xl` | `ADOPT` | `48px` | Section vertical rhythm (compact). |
| **Spacing 3XL** | `--pika-space-3xl` | `--pw2-space-3xl` | `ADOPT` | `64px` | Section vertical rhythm (standard). |
| **Spacing 4XL** | `--pika-space-4xl` | `--pw2-space-4xl` | `ADOPT` | `96px` | Large section vertical rhythm. |
| **Spacing 5XL** | *(None)* | `--pw2-space-5xl` | `NEW` | `128px` | Major chapter/hero vertical rhythm. |
| **Radius XS** | `--pika-radius-xs` | `--pw2-radius-xs` | `ADOPT` | `4px` | Micro tags, code insets. |
| **Radius SM** | `--pika-radius-sm` | `--pw2-radius-sm` | `ADOPT` | `8px` | Inputs, secondary buttons, tooltips. |
| **Radius MD** | `--pika-radius-md` | `--pw2-radius-md` | `ADOPT` | `12px` | Standard buttons, bounded objects. |
| **Radius LG** | `--pika-radius-lg` | `--pw2-radius-lg` | `ADOPT` | `16px` | Product screenshot frames, modal containers. |
| **Radius XL** | `--pika-radius-xl` | `--pw2-radius-xl` | `REFINE` | `24px` | Refined from 22px to 24px (standard 8pt grid alignment). |
| **Radius Pill** | `--pika-radius-pill` | `--pw2-radius-pill` | `ADOPT` | `9999px` | Strictly for status chips, tags, and small badges. |
| **Reading Width** | `--pika-prose-max` | `--pw2-w-reading` | `REFINE` | `760px` | Explicit responsive container replacing raw 65ch. |
| **Standard Width**| `--pika-container-max` | `--pw2-w-standard` | `REFINE` | `1200px` | Reconciles Bootstrap 1140px and legacy Pika 1240px. |
| **Stage Width** | *(None)* | `--pw2-w-stage` | `NEW` | `1340px` | Purpose-built width for 1672px authentic screenshots. |

---

## 5. Colour System

### The Canonical 5-Color Hierarchy

```
[PURE WHITE CANVAS: #ffffff]
          │
          ▼
[SUBTLE RESTRAINED SURFACE: #f8faf9 / #f4f9f1]
          │
          ▼
[SOLID DARK PETROL STRUCTURE: #0c3a30 / #082922]
          │
          ▼
[DARK NEUTRAL TYPOGRAPHY: #0d2821 / #365147 / #4a685e]
          │
          ▼
[PIKA LIME ACCENT (STRICTLY RESTRAINED): #84c225 / #9edd05]
```

### Color Usage Rules
- **Primary Background (`--pw2-bg-primary: #ffffff`):** Default for 80% of page surfaces. Clean, open, high clarity.
- **Subtle Background (`--pw2-bg-subtle: #f8faf9`):** Alternating background for editorial sections to establish visual rhythm.
- **Dark Petrol (`--pw2-bg-dark: #0c3a30`):** Restrained solid background for high-impact chapters: Hero banners, Value Chain overviews, Architecture, Security & Governance, and Final CTAs. (No default decorative gradient).
- **Lime Accent (`--pw2-accent: #9edd05` / `--pw2-brand-primary: #84c225`):**
  - **Allowed:** Primary CTA buttons, small status indicator dots, active tab underlines, diagram directional arrows, subtle badge backgrounds (`#f4f9f1` with `#dbeec8` border).
  - **Forbidden:** Card backgrounds, card borders, whole section backgrounds, large headline text, body text.
- **Accessible Brand Ink (`--pw2-accent-ink: #3f5802`):** Used exclusively for small informative text on white/tint surfaces (`.pw2-eyebrow`, `.pw2-badge--status`, `.pw2-btn--tertiary:hover`) to guarantee $ge$ 4.5:1 contrast.

---

## 6. Typography System

### Font Delivery Audit & Constitution
The repository imports `Inter Tight` via an external CSS `@import` in `style.css` (`@import "https://fonts.googleapis.com/css2?family=Inter+Tight...";`). However:
- There are **no local font binary files** for `Inter Tight` in `wwwroot/web/fonts/` (only `remixicon.*` and legacy `flaticon_finto.*` exist).
- In environments where Google Fonts CDN is slow, unverified, or blocked by network firewalls, font rendering must degrade gracefully without layout shifts or missing glyphs.
- **P02 Mandate:** Do NOT download font binaries and do NOT introduce new font dependencies during this phase.

### Safe Font Stack
```css
--pw2-font-sans: 'Inter Tight', system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
--pw2-font-mono: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace;
```
- **Preferred Family:** `Inter Tight` (when delivered via existing CSS import).
- **Guaranteed Safe Fallback:** Modern system font stack (`system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif`).

### Typography Hierarchy Scale

| Token | Class | Desktop Size / LH | Mobile Size / LH | Weight | Tracking | Intended Usage |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| `--pw2-text-display` | `.pw2-display` | `56px / 1.12` | `36px / 1.2` | `700` | `-0.03em` | Primary homepage & flagship hero headlines. |
| `--pw2-text-h1` | `.pw2-h1` | `42px / 1.18` | `30px / 1.25` | `700` | `-0.025em`| Solution & platform page primary headlines. |
| `--pw2-text-h2` | `.pw2-h2` | `32px / 1.25` | `24px / 1.3` | `600` | `-0.02em` | Major section chapter titles. |
| `--pw2-text-h3` | `.pw2-h3` | `22px / 1.35` | `18px / 1.4` | `600` | `-0.01em` | Subsection & capability headlines. |
| `--pw2-text-h4` | `.pw2-h4` | `18px / 1.4` | `16px / 1.45` | `600` | `0` | Small group titles, table headers. |
| `--pw2-text-lead` | `.pw2-lead` | `20px / 1.55` | `17px / 1.55` | `400` | `-0.01em` | Introductory paragraph beneath H1/H2. |
| `--pw2-text-body` | `.pw2-body` | `16px / 1.6` | `15px / 1.6` | `400` | `0` | Standard body copy, explanations. |
| `--pw2-text-body-sm`| `.pw2-body-sm` | `14px / 1.5` | `13px / 1.5` | `400` | `0` | Footnotes, secondary descriptions. |
| `--pw2-text-eyebrow`| `.pw2-eyebrow` | `12px / 1.0` | `11px / 1.0` | `700` | `+0.08em` | Small category pills / pre-headings. Uppercase. |
| `--pw2-text-caption`| `.pw2-caption` | `12px / 1.4` | `11px / 1.4` | `500` | `0` | Screenshot sources, diagram notes, demo tags. |

### Typography Discipline Rules
1. **Never use pure black (`#000000`)** for headlines or body text. Always use `--pw2-text-primary: #0d2821`.
2. **Never use illegibly light grey** for body text. Body text must maintain at least 4.5:1 contrast against its background (WCAG AA).
3. **No Excessive Bolding:** Use `weight: 400` for body text, `weight: 600` for subheadings, and reserve `weight: 700` for primary page display titles and uppercase eyebrows.

---

## 7. Spacing System

A mathematical 4pt / 8pt spacing family:

```
4px   (2XS)  → Micro gaps, icon tags, tight badge insets
8px   (XS)   → Compact padding, icon-text gap
12px  (SM)   → Small element spacing, compact button py
16px  (MD)   → Standard input/button padding, list gaps
24px  (LG)   → Component margins, grid column gap
32px  (XL)   → Major component gutters
48px  (2XL)  → Compact section vertical padding (mobile/subtle)
64px  (3XL)  → Standard section vertical rhythm
96px  (4XL)  → Major chapter spacing on desktop
128px (5XL)  → Large hero vertical rhythm on wide desktop
```

### Vertical Rhythm
- **Hero Sections:** `padding: 96px 0` desktop, `48px 0` mobile.
- **Standard Sections:** `padding: 80px 0` desktop, `48px 0` mobile.
- **Compact / Secondary Sections:** `padding: 56px 0` desktop, `36px 0` mobile.
- **Information Density:** Vertical rhythm must provide breathable spacing without creating barren, empty gaps. Every section must have a distinct compositional purpose.

---

## 8. Container & Grid System

### Three Intentional Content Widths

```
1. READING WIDTH:       760px  (47.5rem)   ─── Long-form narrative, FAQs, terms, methodology
2. STANDARD WIDTH:     1200px  (75rem)     ─── Editorial splits, feature overviews, 2-column layouts
3. PRODUCT STAGE:      1340px  (83.75rem)  ─── High-resolution screenshots, workflow canvases
```

### Container Rationale & Bootstrap Reconciliation
- **Existing Stack:** PikaWeb uses Bootstrap 5.3.3.
- **Bootstrap 5 Breakpoints:**
  - `sm`: `>= 576px`
  - `md`: `>= 768px`
  - `lg`: `>= 992px`
  - `xl`: `>= 1200px` (Container: `1140px`)
  - `xxl`: `>= 1400px` (Container: `1320px`)
- **Pika Web 2.0 Responsive Alignment:**
  - **Mobile:** `< 768px` (Side gutters: `16px` / `1rem`)
  - **Tablet:** `768px – 991px` (Side gutters: `24px` / `1.5rem`)
  - **Desktop:** `992px – 1399px` (Side gutters: `32px` / `2rem`)
  - **Wide Desktop:** `>= 1400px` (Side gutters: `40px` / `2.5rem`)
- **Stage Width (1340px):** On screens `>= 1400px`, the 1340px product stage width leaves a 30px–50px comfortable margin on each side, allowing authentic 1672px product screenshots to render crisply at 80% natural scale with complete legible fidelity.

---

## 9. Surface, Radius, Border & Shadow System

### Surfaces
- `.pw2-surface`: Pure white elevated surface (`#ffffff`) with subtle border.
- `.pw2-surface--subtle`: Light sage tint (`#f8faf9`) for soft grouping.
- `.pw2-surface--muted`: Inset grey/green well (`#f1f6f3`) for code, raw data contracts, or telemetry.
- `.pw2-surface--dark`: Solid deep petrol structural container (`#0c3a30`).
- `.pw2-surface--stage`: High-contrast screenshot backdrop (`#0e322a`).

### Radius Scale (Restrained)
- `4px` (`--pw2-radius-xs`): Code tags, micro chips.
- `8px` (`--pw2-radius-sm`): Form inputs, secondary buttons.
- `12px` (`--pw2-radius-md`): Standard interactive buttons, discrete objects.
- `16px` (`--pw2-radius-lg`): Product screenshot frames, modal dialogs.
- `24px` (`--pw2-radius-xl`): Flagship hero stages.
- `9999px` (`--pw2-radius-pill`): **STRICTLY RESERVED** for small tags, state chips, and eyebrow badges. Buttons and cards must NOT use pill shapes.

### Shadows (Subdued & Enterprise)
- Hairline subtle elevation only. No diffuse glowing drop shadows.
- Standard component: `box-shadow: 0 2px 6px -1px rgba(12, 38, 32, 0.06), 0 1px 3px rgba(12, 38, 32, 0.03)`.
- Product screenshot frame: `box-shadow: 0 16px 36px -6px rgba(12, 38, 32, 0.10), 0 4px 12px rgba(12, 38, 32, 0.04)`.

---

## 10. Button System

Buttons are visual hierarchy primitives and are decoupled from rigid copy:

### Interactive Target Mandate
- **All buttons intended for user interaction must retain a minimum 44px interactive target.**
- `.pw2-btn--sm` retains `min-height: 44px; padding: 10px 18px;` with a compact font size.
- `.pw2-btn--tertiary` retains `min-height: 44px; display: inline-flex; align-items: center; padding: 8px 4px;` to protect the clickable hit area while maintaining lightweight visual link aesthetics.

### Visual Hierarchy Variants
1. **Primary Button (`.pw2-btn--primary`):**
   - Solid lime fill (`#84c225`), dark petrol text (`#0d2821`, `weight: 600`). Contrast: **7.24:1**.
   - Hover: `#73aa20` background, `#0d2821` text. Contrast: **5.59:1**.
   - Active: Retains `#73aa20` background with `transform: translateY(1px)` for accessible physical feedback without contrast degradation.
2. **Secondary Button (`.pw2-btn--secondary`):**
   - Outlined border (`1.5px solid #c2d8cd`), transparent/white background, dark petrol text (`#0d2821`).
   - Hover: `#f8faf9` background, `#0d2821` border.
3. **Dark Section Primary Button (`.pw2-btn--dark-primary`):**
   - Solid lime fill (`#84c225`), dark text (`#0d2821`) on dark petrol backgrounds.
4. **Dark Section Secondary Button (`.pw2-btn--dark-secondary`):**
   - White hairline outline (`1.5px solid rgba(255, 255, 255, 0.3)`), white text (`#ffffff`).
5. **Tertiary / Text Link (`.pw2-btn--tertiary`):**
   - Lightweight link appearance with 44px touch area. Hover color: `var(--pw2-accent-ink)` (**8.05:1** contrast).

### Focus Behavior
- Light surfaces: `outline: 2px solid var(--pw2-focus-on-light);` (**8.05:1** contrast).
- Dark sections: `outline: 2px solid var(--pw2-focus-on-dark);` (**7.69:1** contrast).
- Zero glow effects.

### Preferred Site-Wide Conversion Labels
- **Primary Conversion CTA:**
  - TR: *"Demo Talep Et"*
  - EN: *"Request a Demo"*
- **Alternative Commercial Intent CTA:**
  - TR: *"Teklif Al"*
  - EN: *"Request a Quote"*
- **Exploratory CTAs:**
  - TR: *"Çözümleri İnceleyin"*, *"Yolculuğu Görün"*, *"Dokümantasyon"*
  - EN: *"Explore Solutions"*, *"View Journey"*, *"Documentation"*

*Mandate:* No glowing buttons, no pulsating neon animations, no fixed pricing CTAs.

---

## 11. Section Archetypes as Composition Patterns

Section archetypes are **compositional patterns** built from foundational PW2 primitives, not monolithic one-class components:

### 1. HERO / PRODUCT HERO
- **Primitive Composition:**
  - Container: `.pw2-section .pw2-section--hero` + `.pw2-container`
  - Content: `.pw2-display` + `.pw2-lead` + CTA row (`.pw2-btn--primary` + `.pw2-btn--secondary`)
  - Media: `.pw2-product-stage` containing `.pw2-product-frame` with authentic screenshot (`img_gunun-firsatlari.png` or `img_pika-360_7.png`)
- **Text Budget:** Headline max 12 words; lead max 35 words.

### 2. EDITORIAL SPLIT
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container`
  - Layout: `.pw2-editorial-split` (optional `.pw2-editorial-split--reversed`)
  - Text Column: `.pw2-eyebrow` + `.pw2-h2` + `.pw2-lead` + `.pw2-body` + `.pw2-btn--tertiary`
  - Visual Column: `.pw2-product-frame` (focal crop of real UI)

### 3. PRODUCT STAGE
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container .pw2-container--stage`
  - Header: `.pw2-text-center` + `.pw2-eyebrow` + `.pw2-h2` + `.pw2-lead`
  - Stage: `.pw2-product-stage` containing `.pw2-product-frame` + `.pw2-product-caption`

### 4. FULL-WIDTH SCREENSHOT
- **Primitive Composition:**
  - Container: `.pw2-section .pw2-section--subtle` + `.pw2-container .pw2-container--full`
  - Frame: `.pw2-product-frame .pw2-product-frame--stage` spanning wide viewport width with authentic high-res UI

### 5. SCREENSHOT + EXPLANATION
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container`
  - Layout: `.pw2-editorial-split` with 60% `.pw2-product-frame` and 40% structured explanation card `.pw2-surface`

### 6. DARK CHAPTER
- **Primitive Composition:**
  - Container: `.pw2-section .pw2-section--dark` + `.pw2-container`
  - Header: `.pw2-eyebrow` + `.pw2-h2` + `.pw2-lead`
  - Content: Solid petrol background (`#0c3a30`), high-contrast white text, CTAs (`.pw2-btn--dark-primary` + `.pw2-btn--dark-secondary`)

### 7. PROCESS / VALUE CHAIN
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container`
  - Flow: `.pw2-value-chain` containing 6 `.pw2-value-chain__step` nodes representing Pika's 6-step loop

### 8. DATA → INTELLIGENCE FLOW
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container`
  - Flow: `.pw2-data-flow` containing `.pw2-data-flow__col` (Input sources) + `.pw2-data-flow__arrow` + `.pw2-data-flow__col` (Intelligence engines) + `.pw2-data-flow__arrow` + `.pw2-data-flow__col` (Channel execution)

### 9. FEATURE DETAIL
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container`
  - Grid: `.pw2-trust-layout` (3-column layout without cards) containing `.pw2-h4` + `.pw2-body` separated by hairlines

### 10. TRUST / GOVERNANCE
- **Primitive Composition:**
  - Container: `.pw2-section .pw2-section--subtle` + `.pw2-container`
  - Content: `.pw2-trust-layout` with authentic UI proof (`img_kullanici-roller-yetkiler_24.png` or `img_izin-kanal-zamanlama_25.png`) + `.pw2-badge`

### 11. METRIC / PROOF
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container`
  - Proof Layout: `.pw2-proof-layout` containing `.pw2-surface` units with prominent numbers and **mandatory `.pw2-badge--demo`**

### 12. FAQ
- **Primitive Composition:**
  - Container: `.pw2-section` + `.pw2-container .pw2-container--reading`
  - Accordion: Multiple `.pw2-faq-item` units containing `.pw2-faq-question` + `.pw2-faq-answer`

### 13. FINAL CTA
- **Primitive Composition:**
  - Container: `.pw2-section .pw2-section--dark` + `.pw2-container .pw2-container--reading` + `.pw2-text-center`
  - Content: `.pw2-h2` + `.pw2-lead` + CTA row (`.pw2-btn--dark-primary` + `.pw2-btn--dark-secondary`)

---

## 12. Card Governance

> [!IMPORTANT]
> **Design Axiom:** "If content can be expressed more clearly through hierarchy, composition, or flow, do NOT put it in a card."

### When Cards ARE Allowed
- **Discrete Objects:** Independent items that can be rearranged, filtered, or compared (e.g. 3 distinct commercial modules, customer journey blueprint templates).
- **Bounded Comparison Units:** Side-by-side technical comparisons (e.g. Native Email vs SMS vs WhatsApp capabilities).
- **Interactive Selectors:** Tab switchers or filter chips.

### When Cards ARE Prohibited
- Every capability bullet point in a section.
- Every paragraph in an explanation.
- Steps in a sequential workflow (use timeline or process flow).
- Channel delivery summaries (use editorial split).
- Trust and compliance points (use structured list layout).

---

## 13. Product Screenshot Principles

The 29 authentic screenshots in `Pika/wwwroot/wiki/assets/images/` are the primary visual proof of Pika Web 2.0.

### Screenshot Framing Standards
- **App Chrome Frame (`.pw2-product-frame`):** Subtle outer border (`1px solid #e3ede8`), rounded corners (`16px`), light background, inner image fitted without distortion.
- **Subtle App Header:** For full screenshots, a minimal simulated window bar with 3 subtle monochrome dots (not bright red/yellow/green) anchors the image.
- **Legibility Over Completeness:** Never shrink an entire 1672px desktop dashboard into an unreadable 300px thumbnail. If a feature is discussed, use a **meaningful crop** (e.g. cropping the opportunity card from `img_gunun-firsatlari.png` or the customer value score from `img_pika-360_7.png`).
- **Annotations & Spotlight:** Use subtle numbered callout badges (1, 2, 3) or hairline zoom frames rather than bright fluorescent arrows.
- **Mobile Behavior:** Large dashboards must either crop intentionally to their focal point or provide a smooth horizontal pan container with a subtle caption: *"Görseli kaydırarak inceleyebilirsiniz"*.

---

## 14. Iconography

- **Canonical Icon Family:** **Remixicon** (`ri-*`).
- **Consistency Rules:**
  - Use line/stroke icons (`ri-*-line`) by default. Reserve filled icons (`ri-*-fill`) strictly for active states or warning indicators.
  - Standard sizes: `16px` (compact/inline), `20px` (standard button/list), `24px` (feature header).
  - No giant decorative icon circles with multicolored backgrounds.
  - No random emoji in UI headings or cards.
- **Technical Debt Notice:** `_Layout.cshtml` currently imports Remixicon twice (CDN `remixicon@4.6.0` and local `~/web/css/remixicon.css`). This duplication is recorded as existing technical debt to be resolved in a subsequent asset cleanup phase; do not alter existing CDN imports in P02.

---

## 15. Diagram Language

Diagrams must visually represent the 6-step value chain and data ingestion pipelines without decorative bloat:

```
┌─────────────────┐       ┌────────────────────────┐       ┌────────────────────────┐
│  VERİ GİRİŞİ    │  ───▶ │  MÜŞTERİ VE ÜRÜN       │  ───▶ │  GÜNÜN FIRSATLARI      │
│  Excel, CSV, API│       │  Pika 360, Need Groups │       │  Tekrar Alım, Churn    │
└─────────────────┘       └────────────────────────┘       └────────────────────────┘
                                                                       │
┌─────────────────┐       ┌────────────────────────┐                   ▼
│  ÖLÇÜM VE BI    │  ◀─── │  KANAL YÜRÜTME         │  ◀─── ┌────────────────────────┐
│  Ciro, Açılma   │       │  E-Posta, SMS, WhatsApp│       │  AKSIYON VE OTOMASYON  │
└─────────────────┘       └────────────────────────┘       │  Audience, Journey Mgr │
                                                           └────────────────────────┘
```

- **Nodes:** Simple rectangular or subtly rounded containers (`#ffffff` or `#f8faf9`) with crisp borders (`#e3ede8`).
- **Connectors:** Restrained solid or dashed lines (`#c2d8cd`) with clean arrowheads.
- **Prohibited:** Neon circuit board graphics, artificial cloud clusters, floating 3D cubes.

---

## 16. Data Visualisation

- **Simulation Labeling:** All marketing representations of Customer Value Scores, RFM matrices, and campaign conversion charts must carry explicit sample badges:
  - Turkish: `ÖRNEK SENARYO` / `TEMSİLİ GÖSTERGE`
  - English: `SIMULATION DATA` / `ILLUSTRATIVE SAMPLE`
- **Authentic Gauges:** Match the visual language of `img_pika-360_7.png` (semi-circular gauge for CVS, colored risk bands: Green = Active, Yellow = At Risk, Red = Churn).
- **Prohibited:** Fabricating arbitrary client revenue curves or pretending demo data represents actual client performance.

---

## 17. Motion & Interaction

- **Duration & Timing:** Fast and responsive.
  - Micro-interactions (hover, color, focus): `150ms` (`--pw2-trans-fast`).
  - Dropdowns and reveals: `220ms` (`--pw2-trans-base`).
  - Major section transitions: `350ms` (`--pw2-trans-slow`).
  - Timing function: `cubic-bezier(0.16, 1, 0.3, 1)` (smooth deceleration).
- **Accessibility & Reduced Motion:**
  - All motion rules in `pika-design-system.css` must honor `@media (prefers-reduced-motion: reduce)`.
  - When reduced motion is preferred, transitions must drop to `0.01ms` or immediate cuts.
  - No critical information may depend on animation to become visible.

---

## 18. Dark Section Usage

- Pika is **NOT** a dark-mode website; it is primarily a crisp white editorial platform.
- **Dark Petrol Sections (`#0c3a30`)** are permitted only for purposeful structural chapters:
  - Technical Architecture & Ingestion Workers
  - Data Security & Zero-PII Trust Boundaries
  - High-impact mid-page transition dividers
  - Final CTA banners
- **Default Surface:** Solid `var(--pw2-bg-dark): #0c3a30`. No default decorative gradients.
- **Dark Section Text:** Must use `--pw2-text-on-dark: #ffffff` and `--pw2-text-on-dark-muted: rgba(255, 255, 255, 0.74)` to guarantee WCAG AA contrast.

---

## 19. Responsive Behaviour

Layouts must adapt intentionally across the 4 verified breakpoints:

```
1. MOBILE (< 768px):
   - Single column flow.
   - Text remains left-aligned; do NOT force center-alignment.
   - Large screenshots switch to focal crop or horizontal panning.
   - Touch targets maintain minimum 44px height.

2. TABLET (768px – 991px):
   - Editorial splits compress to 50/50 with tightened margins.
   - 3-column features wrap to 2+1 or vertical stack.
   - Gutter spacing: 24px.

3. DESKTOP (992px – 1399px):
   - Standard 1200px container active.
   - Full 2-column editorial splits with generous whitespace.
   - Gutter spacing: 32px.

4. WIDE DESKTOP (>= 1400px):
   - 1340px product stage width unlocked.
   - 1672px screenshots render with maximum fidelity.
   - Gutter spacing: 40px.
```

---

## 20. Accessibility (WCAG 2.1 AA Compliance)

### Verified Relative Luminance Contrast Ratios
All color pairs have been verified via the standard WCAG 2.1 relative luminance algorithm:

| Foreground | Background | Actual Ratio | Standard | Status |
| :--- | :--- | :--- | :--- | :--- |
| Primary Text (`#0d2821`) | Pure White (`#ffffff`) | **15.64:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Primary Text (`#0d2821`) | Subtle Sage (`#f8faf9`) | **14.92:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Secondary Text (`#365147`) | Pure White (`#ffffff`) | **8.66:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Secondary Text (`#365147`) | Subtle Sage (`#f8faf9`) | **8.26:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Muted Text (`#4a685e`) | Pure White (`#ffffff`) | **6.12:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Muted Text (`#4a685e`) | Subtle Sage (`#f8faf9`) | **5.84:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Accent Ink (`#3f5802`) | Pure White (`#ffffff`) | **8.05:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Accent Ink (`#3f5802`) | Green Tint (`#f4f9f1`) | **7.54:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Primary Button Text (`#0d2821`) | Primary Lime (`#84c225`) | **7.24:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Primary Button Text (`#0d2821`) | Hover Lime (`#73aa20`) | **5.59:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Focus on Light (`#3f5802`) | Pure White (`#ffffff`) | **8.05:1** | Non-Text UI ($ge$ 3.0:1) | PASS |
| Focus on Dark (`#9edd05`) | Dark Petrol (`#0c3a30`) | **7.69:1** | Non-Text UI ($ge$ 3.0:1) | PASS |
| Text on Dark (`#ffffff`) | Dark Petrol (`#0c3a30`) | **12.62:1** | WCAG AA ($ge$ 4.5:1) | PASS |
| Text on Dark Muted (74%) | Dark Petrol (`#0c3a30`) | **7.65:1** | WCAG AA ($ge$ 4.5:1) | PASS |

### Mandatory Contrast Rules for Future Tasks
1. **Normal text (< 18pt / < 24px regular):** Must achieve $ge$ **4.5:1** against adjacent background.
2. **Large text ($ge$ 18pt / $ge$ 14pt bold):** Must achieve $ge$ **3.0:1** against adjacent background.
3. **Focus indicators and interactive UI boundaries:** Must achieve $ge$ **3.0:1** against adjacent background.
4. **Baseline is WCAG AA:** Individual high-contrast pairs that reach AAA do not warrant a blanket AAA claim for the platform.

### Target Size & Focus Discipline
- **Target Size:** Every clickable button, link, accordion toggle, or tab must provide at least a **44px × 44px** hit area.
- **Focus Rings:** Distinct 2px solid outlines with 2px offset (`--pw2-focus-on-light` on light surfaces, `--pw2-focus-on-dark` on dark sections). Zero glow effects.
- **Screen Reader Support:** Provide `.pw2-sr-only` utility for accessibility labels.

---

## 21. Header & Footer Visual Governance

The canonical information architecture established in P01 must remain unchanged. P02 establishes visual styling rules for later refinement:
- **Header:** Height: `72px` desktop, `64px` mobile. Clean white surface with subtle hairline bottom border (`#e3ede8`). Nav links: `#0d2821`, font-size `15px`, font-weight `500`. Primary conversion CTA: `Demo Talep Et`.
- **Dropdowns:** White surface, subtle elevation shadow, structured multi-column layout for platform modules.
- **Footer:** Deep petrol surface (`#0c3a30`), organized into clear functional columns (Platform, Çözümler, Kanallar, Kurumsal, Yasal). Explicit copyright and address matching `appsettings.json` (Ankara Teknopark).

---

## 22. Content vs Design Responsibilities

- **The Design System adapts to approved content.** Codex must never delete or invent product claims to make a layout "look balanced."
- **Content Precedence:** In later phases (P03–P25), approved headlines, body copy, and metadata will be supplied in prompts or drawn from `PRODUCT_TRUTH.md`.
- **Missing Copy Protocol:** If copy is missing for a planned block, insert `<!-- TODO: APPROVED COPY REQUIRED -->` rather than generating placeholder marketing fluff.

---

## 23. Legacy CSS Migration Strategy

To guarantee zero regression during Web 2.0 implementation:
1. **P02 Foundation:** Load `pika-design-system.css` globally. All classes use the `.pw2-*` namespace. Existing pages remain 100% visually identical because no legacy classes are overridden.
2. **Phase P03–P25 Migration:** Each page will be migrated individually to `.pw2-*` archetypes.
3. **Legacy Deprecation:** Once all pages are migrated, legacy files (`pika-components.css`, `pika-home.css`, `pika-product.css`) will be retired in a planned cleanup phase.

---

## 24. Pika Web 2.0 CSS Namespace

All styles defined in `pika-design-system.css` must strictly conform to:
- Custom properties declared on `:root` with the `--pw2-*` prefix.
- Class selectors beginning with `.pw2-*` or scoped below a `.pw2-*` ancestor.
- Absolutely NO unnamespaced element selectors (`body`, `h1`, `p`, `a`, `button`, `img`, `*`, `:focus`).
- Focus states and reduced motion rules scoped strictly within `.pw2-*` components.

---

## 25. Visual Anti-Patterns (Strictly Prohibited)

The following 16 anti-patterns are permanently banned from Pika Web 2.0:
1. ❌ **The SaaS Card Cemetery:** Placing every sentence, capability, or step inside identical rounded cards.
2. ❌ **Coloured Edge Borders:** Arbitrary left/top borders on cards (e.g. `border-left: 4px solid #84c225`).
3. ❌ **Gradient Borders:** Any gradient-stroked card or button border.
4. ❌ **Fake Dashboard UI:** Hand-crafting HTML/CSS mockups when authentic screenshots exist in `wwwroot/wiki/assets/images/`.
5. ❌ **Neon Glow Effects:** Applying `box-shadow` glows with brand colors to cards or buttons.
6. ❌ **Glassmorphism:** Using translucent panels with `backdrop-filter: blur()`.
7. ❌ **Meaningless Floating Shapes:** Floating spheres, cylinders, rings, or decorative geometric blobs.
8. ❌ **Excessive Mobile Center-Alignment:** Forcing all headings, paragraphs, and lists into center alignment on mobile screens.
9. ❌ **Barren Empty Whitespace:** Large blank areas with no compositional or explanatory purpose.
10. ❌ **Unreadably Shrunk Screenshots:** Scaling complex dashboard screenshots down until interface text is illegible.
11. ❌ **Inconsistent Icon Families:** Mixing line icons, filled icons, flat vector art, and 3D icons.
12. ❌ **Unlabelled Demo Metrics:** Displaying percentages (e.g. `%52 Açılma`, `14.2x ROAS`) without mandatory `ÖRNEK SENARYO` labels.
13. ❌ **Generic Stock Photography:** Stock photos of business people shaking hands, laptops on coffee tables, or corporate buildings.
14. ❌ **AI-Generated People:** Synthetic portrait avatars used as fake customer testimonials.
15. ❌ **Competitor Layout Cloning:** Copying layouts directly from SmartMessage, Braze, or Insider.
16. ❌ **Pill Buttons:** Forcing primary action buttons into full rounded pill shapes (`border-radius: 9999px`).

---

## 26. Acceptance Checklist for Future Pages

Every subsequent page phase (P03–P25) must satisfy this quantitative checklist before approval:

- [ ] **Real Product Proof:** Does the page use authentic screenshots from `VISUAL_ASSET_REGISTRY.md`? (0 fake CSS dashboards).
- [ ] **Anti-Card Verification:** Are there fewer than 2 standard card grids across the entire page?
- [ ] **Section Variety:** Does the page use at least 3 distinct section archetypes (e.g. Hero + Editorial Split + Product Stage + Value Chain)?
- [ ] **Namespace Compliance:** Are 100% of new CSS classes scoped under `.pw2-*`?
- [ ] **Typography Hierarchy:** Is the heading hierarchy strictly sequential (H1 $\rightarrow$ H2 $\rightarrow$ H3)?
- [ ] **Accent Restraint:** Is the lime accent restricted to CTAs, badges, and small focal highlights?
- [ ] **Accessibility Compliance:** Do all text/background pairs meet $ge$ 4.5:1 and UI boundaries/focus rings meet $ge$ 3:1?
- [ ] **Touch Target Verification:** Do all interactive elements (buttons, links, toggles) maintain $ge$ 44px hit targets?
- [ ] **Screenshot Legibility:** Are screenshots cropped or zoomed so interface text is sharp and legible?
- [ ] **Mobile Responsiveness:** Does the page flow naturally on mobile with left-aligned reading text and accessible touch targets?
- [ ] **Metric Governance:** Are all demo/sample metrics explicitly badged with `ÖRNEK SENARYO`?
- [ ] **Quotation Pricing Alignment:** Are all pricing links directed to `/demo-talebi` without fixed package amounts or SLA promises?
