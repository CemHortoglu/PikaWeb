# Pika Knowledge Base / Wiki Architecture

## 1. Overview
Pika Knowledge Base is a high-performance, server-side rendered (SSR) documentation and knowledge layer designed for fast human readability, search engine crawling (Googlebot/Bingbot), and AI discoverability (ChatGPT Search / OAI-SearchBot).

---

## 2. Source of Truth
- **Data File**: [`App_Data/wiki.json`](file:///c:/Projects/pika.web/App_Data/wiki.json)
- **Static Assets**:
  - CSS: `wwwroot/wiki/assets/style.css`
  - Images: `wwwroot/wiki/assets/images/`

The JSON file contains two main sections:
1. `nav`: An ordered array defining categories/sections and their list of article slugs.
2. `pages`: A dictionary keyed by article slug containing:
   - `section`: Category name (e.g., `"Pika’ya Başlarken"`).
   - `title`: Article title (e.g., `"Pika Nedir?"`).
   - `summary`: Short meta description.
   - `html`: Semantic HTML body (`<section>`, `<h2>`, `<h3>`, `<p>`, `<ul>`, `<figure>`, etc.).
   - `related`: Array of related article slugs.

---

## 3. How to Add a New Article
1. Open [`App_Data/wiki.json`](file:///c:/Projects/pika.web/App_Data/wiki.json).
2. Choose a clean, lowercase kebab-case slug (e.g., `yeni-ozellik-rehberi`).
3. Add the slug to the appropriate category in the `nav` list.
4. Add the article object under `pages`:
   ```json
   "yeni-ozellik-rehberi": {
     "section": "Pika’ya Başlarken",
     "title": "Yeni Özellik Rehberi",
     "summary": "Yeni özelliğin nasıl çalıştığını ve sağladığı faydaları açıklar.",
     "html": "\n<section class=\"hero hero-intro\">\n<h1>Yeni Özellik Rehberi</h1>\n<p class=\"hero-lead\">Açıklama...</p>\n</section>\n<h2>Alt Başlık</h2>\n<p>İçerik...</p>\n",
     "related": ["pika-nedir", "pika-nasil-calisir"]
   }
   ```
5. Add the URL `https://pika.tr/wiki/yeni-ozellik-rehberi` to [`wwwroot/sitemap.xml`](file:///c:/Projects/pika.web/wwwroot/sitemap.xml).

---

## 4. Slug & URL Conventions
- Lowercase kebab-case (e.g., `/wiki/customer-intelligence`).
- No IDs, GUIDs, query parameters, or file extensions in canonical URLs.
- Root: `https://pika.tr/wiki/`
- Article: `https://pika.tr/wiki/{slug}`

---

## 5. Runtime / Request Flow
1. **Request arrives** at `/wiki/` or `/wiki/{slug}`.
2. **`WikiController`** handles the route:
   - `/wiki/` -> calls `IWikiService.GetHomeViewModel()` and renders `Views/Wiki/Index.cshtml`.
   - `/wiki/{slug}` -> calls `IWikiService.GetArticleViewModel(slug)`:
     - Automatically parses `<h2>` and `<h3>` tags to inject deterministic anchors (`id="..."`) and builds a Table of Contents (TOC).
     - Renders `Views/Wiki/Article.cshtml` with SSR HTML, `TechArticle` and `BreadcrumbList` JSON-LD schemas.
     - Nonexistent slugs return a true HTTP 404.
   - `/wiki/index.html` -> 301 redirects to `/wiki/`.
