# Pika visual refresh — 8 September 2026

- Removed decorative thick colored side borders from marketing pages, homepage content, knowledge base, cookie notice and legacy navigation styles. Structural dividers and CSS arrow shapes remain functional.
- Redesigned `/pika` using a generated editorial photograph, compact architecture cards, a six-step grid and distinct light/dark sections. All 14 sections and their original bilingual text remain.
- Fixed knowledge-base hero contrast, including article variants; styled the six `.journey-node` steps and corrected the mobile breadcrumb/header layout. Article data was not edited.
- Validation: 1,621 of 1,625 tests passed. Four previously reproduced failures remain: two homepage accessibility expectations and two corporate metadata HTML-decoding expectations. Desktop and mobile browser checks verified image loading, heading contrast and no horizontal overflow on the sampled pages.

## Image asset

Built-in ImageGen output: `Pika/wwwroot/images/pika-commerce-editorial.png` (1536 × 1024). Conceptual illustration; not an actual customer endorsement.

Final generation prompt:

> Create a premium photorealistic editorial brand image for a B2B customer intelligence and commerce website called Pika. Landscape 3:2 composition, no text no logos no UI. Art-directed contemporary boutique retail scene: foreground sculptural cream stone counter with carefully curated coffee packaging, ceramic cup, glass bottle and folded neutral fabric, middle ground a stylish shop owner and a customer having a natural warm conversation, not looking at camera. Sophisticated deep petrol architectural walls (#10292e), muted olive and small fresh lime accents, warm ivory surfaces, soft daylight through large windows, understated luxury, authentic human interaction and tangible products, refined European design magazine photography, natural skin texture, cinematic but credible, spacious layered composition. No glowing holograms, no robots, no stock-photo handshake, no charts, no lettering, no borders. This is conceptual illustrative imagery, not an actual customer testimonial.

## Scenario photo galleries
Journey Manager and Content Studio use selectable expanding photo panels at desktop widths of 1100px and above. Audience Manager uses six fully readable photo cards. All original bilingual headings and descriptions remain server-rendered. Small screens and no-JavaScript rendering show every description. Native buttons expose expanded state; arrow/Home/End keys select desktop panels and reduced motion is respected.

Generated assets: Pika/wwwroot/images/pika-story-cart.png, pika-story-repeat.png, pika-story-reconnect.png. Existing pika-commerce-editorial.png supplies the welcome scene. Images illustrate scenarios, not customer endorsements.

Prompts:
- Cart: Premium editorial lifestyle photograph for Pika customer engagement website, portrait 3:4. Close view of a person browsing online shopping on a laptop at a refined warm ivory desk, kraft parcel and unbranded ceramic objects nearby, soft window light, deep petrol and muted olive palette, natural believable hands, cinematic retail campaign photography, generous composition for dark text overlay at bottom. No legible text, no logos, no brand names, no infographic. Conceptual shopping scene, not actual customer.
- Repeat: Premium editorial still life photograph for Pika customer engagement website, portrait 3:4. Sculptural ceramic coffee cup, paper bag of coffee beans with no printing, glass coffee server, a few beans and olive linen on cream stone countertop, deep petrol wall, side sunlight, tactile luxury magazine photography, warm highlights, visually rich top half, darker lower half suitable for overlay. No text, no logos, no border. Conceptual repeat purchase illustration.
- Reconnect: Premium editorial lifestyle photograph for Pika customer engagement website, portrait 3:4. Candid adult woman in an understated olive jacket sitting by a contemporary cafe window glancing at her smartphone with a subtle warm smile, deep petrol interior, soft daylight, warm ivory textures, authentic natural skin, elegant magazine campaign photography, subject mainly upper half, lower area darker for overlay. No text, no logos, no visible phone interface, no testimonial claims.

Validation: build 0 errors; 196 related tests passed, none skipped. Browser verified Journey panel selection, 390px viewport with all four descriptions visible and no horizontal overflow, and card counts on Audience Manager and Content Studio.
