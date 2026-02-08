// wwwroot/js/site.js
(function () {
    const qs = (s, el = document) => el.querySelector(s);
    const qsa = (s, el = document) => [...el.querySelectorAll(s)];

    const i18n = {
        tr: {
            "topbar.security": "Kurumsal güvenlik • KVKK uyumlu süreçler",
            "topbar.support": "Destek:",
            "nav.home": "Ana Sayfa",
            "nav.solutions": "Çözümlerimiz",
            "nav.solutions.overview": "Genel Bakış",
            "nav.solutions.loyalty": "Sadakat",
            "nav.solutions.campaigns": "Kampanyalar",
            "nav.solutions.segmentation": "Segmentasyon",
            "nav.solutions.analytics": "Analitik",
            "nav.solutions.integrations": "Entegrasyonlar",
            "nav.pricing": "Fiyatlandırma",
            "nav.corporate": "Kurumsal",
            "nav.contact": "İletişim",
            "nav.login": "Giriş",
            "nav.demo": "Demo Talebi",
            "nav.language": "Dil",

            "footer.desc": "Pika ile sadakat programınızı yönetin, kampanyaları otomatikleştirin ve veriyi aksiyona dönüştürün.",
            "footer.product": "Ürün",
            "footer.company": "Şirket",
            "footer.security": "Güvenlik",
            "footer.kvkk": "KVKK",
            "footer.terms": "Kullanım Şartları",
            "footer.ctaTitle": "Daha fazla gelir, daha yüksek sadakat",
            "footer.ctaDesc": "Demo ile 15 dakikada ihtiyacınıza uygun akışı kuralım.",
            "footer.contactSales": "Satış ile Görüş",
            "footer.rights": "Tüm hakları saklıdır.",
            "footer.made": "Kurumsal sadakat ürünleri için tasarlandı.",

            "demo.title": "Demo Talebi",
            "demo.subtitle": "Kısa bilgileri bırakın, ekibimiz sizinle iletişime geçsin.",
            "form.fullName": "Ad Soyad",
            "form.company": "Şirket",
            "form.email": "E-posta",
            "form.phone": "Telefon",
            "form.message": "Mesaj",
            "form.fullNamePh": "Ad Soyad",
            "form.companyPh": "Şirket",
            "form.emailPh": "mail@firma.com",
            "form.phonePh": "+90 ...",
            "form.messagePh": "Kısaca ihtiyacınızı yazın...",
            "form.privacy": "Bilgileriniz gizli tutulur.",
            "form.close": "Kapat",
            "form.send": "Gönder",

            "home.badge": "Kurumsal Sadakat & Kampanya Otomasyonu",
            "home.h1": "Sadakati ölçülebilir büyümeye çevirin.",
            "home.lead": "Segmentasyon, kampanya akışları ve analitik ile müşteriyi doğru anda doğru teklifle yakalayın.",
            "home.cta1": "Demo Talebi",
            "home.cta2": "Çözümleri İncele",
            "home.trust1": "KVKK Odaklı",
            "home.trust2": "Hızlı Kurulum",
            "home.trust3": "Ölçülebilir ROI",
            "home.logosTitle": "Birçok ekip Pika ile büyüyor",
            "home.kpi1Title": "Dönüşüm",
            "home.kpi2Title": "Tekrar Satın Alma",
            "home.f1Title": "Akış Bazlı Kampanyalar",
            "home.f1Desc": "Koşullara göre tetiklenen otomasyonlar: hoş geldin, terk edilen sepet, geri kazanım.",
            "home.f2Title": "Sadakat Programları",
            "home.f2Desc": "Puan, seviye, ödül kataloğu ve kişiselleştirilmiş avantajlarla bağlılık oluşturun.",
            "home.f3Title": "Analitik & İçgörü",
            "home.f3Desc": "Kohortlar, LTV, kampanya performansı ve kanal bazlı katkıyı tek panelde görün.",
            "home.learnMore": "Detay",
            "home.blockTitle": "Kampanyadan sadakate: uçtan uca büyüme akışı",
            "home.blockDesc": "Pika; veri, segment ve aksiyonu aynı yerde toplar. Ekipler daha hızlı iterasyon yapar, daha az operasyonla daha çok sonuç alır.",
            "home.check1": "Hazır şablonlar ve hızlı devreye alma",
            "home.check2": "Gelişmiş segment filtreleri ve kural motoru",
            "home.check3": "Gerçek zamanlı raporlama, export ve API",
            "home.seePricing": "Fiyatları Gör",
            "home.requestDemo": "Demo İste",
            "home.testTitle": "Ekiplerin sevdiği deneyim",
            "home.testDesc": "Kurumsal görünüm, net kullanıcı akışları, yüksek dönüşüm odaklı UI.",
            "home.q1": "“Kampanya üretim süremiz ciddi azaldı. Operasyon yerine stratejiye zaman ayırıyoruz.”",
            "home.q2": "“Segmentasyon ve analitik aynı yerde olunca kararlar hızlandı, sonuçlar netleşti.”",
            "home.q3": "“Kurumsal görünüm ve kullanım kolaylığı, iç paydaşlarda güven oluşturdu.”",
            "home.ctaTitle": "Demo ile ihtiyaçlarınıza uygun akışı birlikte kuralım",
            "home.ctaDesc": "15 dakikada hedeflerinizi çıkaralım, kurulum planını netleştirelim.",
            "home.ctaBtn": "Demo Talebi",

            "sol.h1": "Çözümlerimiz",
            "sol.lead": "Sadakat, kampanya, segmentasyon ve analitiği tek çatı altında birleştirin.",
            "sol.integrations": "Entegrasyonlar",
            "sol.demo": "Demo Talebi",
            "sol.c1t": "Sadakat Yönetimi",
            "sol.c1d": "Puan/level, ödül kataloğu, özel avantajlar ve otomatik kurallar.",
            "sol.c2t": "Kampanya Otomasyonu",
            "sol.c2d": "Tetikleyiciler, akışlar, kuponlar ve kanal bazlı gönderimler.",
            "sol.c3t": "Segmentasyon",
            "sol.c3d": "Davranışsal ve demografik filtrelerle dinamik kitleler.",
            "sol.c4t": "Analitik",
            "sol.c4d": "LTV, kohort, kampanya katkısı ve funnel raporları.",
            "sol.c5t": "Entegrasyonlar",
            "sol.c5d": "API, webhook ve hazır bağlayıcılarla hızlı entegrasyon.",
            "sol.ctat": "Size özel akış",
            "sol.ctad": "İş modelinize göre en doğru kurgu ve KPI setini birlikte belirleyelim.",
            "sol.ctab": "Demo Talebi",

            "price.h1": "Fiyatlandırma",
            "price.lead": "İhtiyacınıza göre ölçeklenen planlar. Kurumsal için özel sözleşme seçenekleri.",
            "price.best": "En Popüler",
            "price.custom": "Özel",
            "price.choose": "Teklif Al",
            "price.contact": "Satış ile Görüş",
            "price.compare": "Plan Karşılaştırma",
            "price.feature": "Özellik",
            "price.row1": "Segmentasyon",
            "price.row2": "Otomasyon Akışları",
            "price.row3": "Gelişmiş Analitik",
            "price.row4": "SSO / RBAC",
            "price.note": "Not: Fiyatlar örnek amaçlıdır; gerçek fiyatlar kullanım ve kapsamla netleşir.",

            "corp.h1": "Kurumsal",
            "corp.lead": "Güven, sürdürülebilirlik ve ölçülebilir büyüme için tasarlanmış bir sadakat altyapısı.",
            "corp.demo": "Demo Talebi",
            "corp.contact": "İletişime Geç",
            "corp.s1": "Uptime hedefi",
            "corp.s2": "Rol bazlı yetki",
            "corp.s3": "Entegrasyon mimarisi",
            "corp.s4": "Uyum odaklı",
            "corp.m1t": "Misyon",
            "corp.m1d": "Müşteri sadakatini şeffaf metriklerle yönetilebilir hale getirmek.",
            "corp.m2t": "Değerler",
            "corp.m2d": "Güven, kullanıcı deneyimi, veri sorumluluğu ve süreklilik.",
            "corp.m3t": "Güvenlik",
            "corp.m3d": "Kurumsal erişim kontrolü, denetim izleri ve güvenli entegrasyon modeli.",
            "corp.ctaT": "Kurumsal ihtiyaçlar için birlikte planlayalım",
            "corp.ctaD": "Kapsam, entegrasyon ve KPI setine göre yol haritası çıkaralım.",
            "corp.ctaB": "Demo Talebi",

            "contact.h1": "İletişim",
            "contact.lead": "Sorunuz mu var? Satış ve teknik ekiplerimizle hızlıca bağlantı kurun.",
            "contact.formTitle": "Bize Yazın",
            "contact.demo": "Demo Talebi",
            "contact.send": "Gönder",
            "contact.infoTitle": "İletişim Bilgileri",
            "contact.mail": "E-posta",
            "contact.phone": "Telefon",
            "contact.address": "Adres",
            "contact.map": "Harita alanı",
            "contact.ctaT": "Kurumsal teklif mi?",
            "contact.ctaD": "Kapsam ve entegrasyonlara göre netleştirelim.",
            "contact.ctaB": "Fiyatlandırma",

            "login.title": "Kurumsal Giriş",
            "login.subtitle": "Panelinize erişmek için bilgilerinizi girin.",
            "login.password": "Şifre",
            "login.remember": "Beni hatırla",
            "login.forgot": "Şifremi unuttum",
            "login.signin": "Giriş Yap",
            "login.or": "veya",
            "login.contactSales": "Kurumsal teklif al",
            "login.note": "Giriş denemeleri loglanır ve güvenlik politikalarına tabidir.",

            // Solutions subpages (basic)
            "loy.h1": "Sadakat Yönetimi",
            "loy.lead": "Puan, seviye ve ödüllerle müşteri bağlılığını sürdürülebilir hale getirin.",
            "loy.demo": "Demo Talebi",
            "loy.pricing": "Fiyatlandırma",
            "loy.m1t": "Ödül Kataloğu",
            "loy.m1d": "Esnek ödül kurgusu",
            "loy.m2t": "Seviyeler",
            "loy.m2d": "Tier bazlı avantajlar",
            "loy.m3t": "Kurallar",
            "loy.m3d": "Otomatik puanlama",
            "loy.f1t": "Puan & Harcama",
            "loy.f1d": "Alışverişe, kategoriye veya aksiyona göre puan kazanımı ve harcama.",
            "loy.f2t": "Üyelik Deneyimi",
            "loy.f2d": "Müşterinin puan/level durumunu net gösteren, kurumsal UI.",
            "loy.f3t": "Suistimal Önleme",
            "loy.f3d": "Kural limitleri, denetim izleri ve rol bazlı yetkilendirme.",
            "loy.ctaT": "Sadakat kurgunuzu birlikte tasarlayalım",
            "loy.ctaD": "Sektörünüze uygun puan/level modelini KPI’larla netleştirelim.",
            "loy.ctaB": "Demo Talebi",

            "camp.h1": "Kampanya Otomasyonu",
            "camp.lead": "Tetikleyiciler, kuponlar ve akışlarla operasyonu azaltın, dönüşümü artırın.",
            "camp.demo": "Demo Talebi",
            "camp.next": "Segmentasyon",
            "camp.f1t": "Tetikleyiciler",
            "camp.f1d": "Event bazlı akışlar: kayıt, ilk alışveriş, terk edilen sepet, geri kazanım.",
            "camp.f2t": "Kupon & Teklif",
            "camp.f2d": "Kural bazlı kupon üretimi, limitler, süreler ve kullanım koşulları.",
            "camp.f3t": "Kanal Yönetimi",
            "camp.f3d": "E-posta/SMS/push gibi kanallarda planlama ve performans takibi.",

            "seg.h1": "Segmentasyon",
            "seg.lead": "Dinamik kitleler oluşturun; doğru müşteriye doğru anda ulaşın.",
            "seg.demo": "Demo Talebi",
            "seg.next": "Analitik",
            "seg.f1t": "Kural Motoru",
            "seg.f1d": "Davranış, sıklık, sepet, kategori ve zaman pencereleriyle filtreler.",
            "seg.c1": "Dinamik güncellenen segmentler",
            "seg.c2": "Kohort ve LTV bazlı hedefleme",
            "seg.c3": "Export / API ile dış sistemler",
            "seg.f2t": "Örnek Kullanım",
            "seg.f2d": "“Son 30 günde 2+ alışveriş yapan ama 14 gündür pasif olan” gibi hedefler.",

            "ana.h1": "Analitik",
            "ana.lead": "Kampanyalarınızın gerçek katkısını görün; LTV ve kohortlarla karar alın.",
            "ana.demo": "Demo Talebi",
            "ana.next": "Entegrasyonlar",
            "ana.f1t": "KPI Paneli",
            "ana.f1d": "Dönüşüm, tekrar satın alma, sepet ve kanal bazlı katkı.",
            "ana.f2t": "Kohort",
            "ana.f2d": "Zaman içinde davranış değişimini izleyin, retention’ı görün.",
            "ana.f3t": "Raporlama",
            "ana.f3d": "CSV export, API ile BI/warehouse akışları.",

            "int.h1": "Entegrasyonlar",
            "int.lead": "API & webhook yaklaşımıyla mevcut sistemlerinize hızlıca bağlanın.",
            "int.demo": "Demo Talebi",
            "int.contact": "Teknik Görüşme",
            "int.f1t": "REST API",
            "int.f1d": "Müşteri, işlem ve etkinlik verilerini güvenli şekilde gönderin.",
            "int.f2t": "Webhook",
            "int.f2d": "Kampanya ve sadakat aksiyonlarını sisteminize geri bildirin.",
            "int.c1": "Event tabanlı mimari",
            "int.c2": "Retry & signature (önerilir)",
            "int.c3": "Dokümantasyon & örnek payload",

            "search": "Ara",
            "home.faqTitle": "Sık Sorulan Sorular",
            "home.heroWelcome": "Pika'ya Hoş Geldiniz",
            "home.heroTitle": "Müşterinize Özel Reklam Yönetimi",
            "home.heroDesc": "Pika Analysis olarak online mağazaların reklam yönetimini gelişmiş otomasyonla dönüştürüyoruz. E-ticaret ekiplerinin büyümeye odaklanabilmesi için süreci sadeleştiriyor ve performansı artırıyoruz.",
            "home.heroBadge": "Tek Tıkla Bir Adım Önde",
            "home.card1": "Şirketinizi Verimlilikle Büyütün",
            "home.card2": "Otomasyonla Karlılığı Artırın",
            "home.card2Desc": "Akıllı çözümlerimizle kampanya yönetimini hızlandırın, maliyeti azaltın.",
            "home.extraProfit": "Ekstra Kâr",
            "home.card3": "Müşteri İhtiyaçlarını Önceleyin",
            "home.highlight": "Öne Çıkın",
            "home.aiTitle": "Yapay Zeka Desteği ile Rakiplerinizin Önüne Geçin",
            "home.aiDesc": "Yapay zeka destekli içgörü ve raporlamalar sayesinde hızlı, ölçülebilir ve sürdürülebilir büyüme sağlayın.",
            "home.ai1": "Yapay Zeka Desteği",
            "home.ai2": "Kişiselleştirilmiş Kampanya",
            "home.ai3": "BI Desteği",
            "home.ai4": "Kapsamlı Raporlama",
            "home.faqSub": "S.S.S.",
            "home.faqDesc": "Pika Analysis çözümleri ile ilgili sık sorulan soruların kısa cevaplarını aşağıda bulabilirsiniz.",
            "home.faqQ1": "Pika Analysis nedir?",
            "home.faqA1": "Pika, e-ticaret ekipleri için reklam ve kampanya otomasyonu sağlayan bir platformdur.",
            "home.faqQ2": "Nasıl otomasyon sağlıyor?",
            "home.faqA2": "Veri odaklı kurallar, segmentasyon ve hazır akışlarla manuel işleri azaltır.",
            "home.faqQ3": "Hangi platformlarla entegre olur?",
            "home.faqA3": "Popüler reklam ve e-ticaret altyapılarıyla API üzerinden entegre çalışır.",
            "form.lastNamePh": "Soyad",
            "form.websitePh": "Web sitesi",
            "login.passwordPh": "••••••••",
            "login.secure": "Güvenli",

            "sol.m1t": "Sadakat",
            "sol.m1d": "Puan, seviye, ödül",
            "sol.m2t": "Kampanya",
            "sol.m2d": "Akış & otomasyon",
            "sol.m3t": "Segment",
            "sol.m3d": "Kural motoru",
            "sol.m4t": "Analitik",
            "sol.m4d": "KPI & içgörü",
            "price.p1t": "Start",
            "price.p1d": "Küçük ekipler için hızlı başlangıç.",
            "price.p1f1": "Temel segmentasyon",
            "price.p1f2": "Kampanya şablonları",
            "price.p1f3": "Standart raporlar",
            "price.p2t": "Growth",
            "price.p2d": "Büyüyen ekipler için otomasyon ve analitik.",
            "price.p2f1": "Gelişmiş segment & dinamik kitle",
            "price.p2f2": "Otomasyon akışları",
            "price.p2f3": "Kohort & kampanya performansı",
            "price.p2f4": "Webhook/API erişimi",
            "price.p3t": "Enterprise",
            "price.p3d": "Kurumsal SLA, güvenlik ve özel entegrasyonlar.",
            "price.p3f1": "SSO / RBAC",
            "price.p3f2": "Özel raporlar & BI entegrasyonu",
            "price.p3f3": "Özel SLA & danışmanlık"
        },
        en: {
            "topbar.security": "Enterprise security • Privacy-first processes",
            "topbar.support": "Support:",
            "nav.home": "Home",
            "nav.solutions": "Solutions",
            "nav.solutions.overview": "Overview",
            "nav.solutions.loyalty": "Loyalty",
            "nav.solutions.campaigns": "Campaigns",
            "nav.solutions.segmentation": "Segmentation",
            "nav.solutions.analytics": "Analytics",
            "nav.solutions.integrations": "Integrations",
            "nav.pricing": "Pricing",
            "nav.corporate": "Company",
            "nav.contact": "Contact",
            "nav.login": "Login",
            "nav.demo": "Request a Demo",
            "nav.language": "Language",

            "footer.desc": "Manage loyalty, automate campaigns, and turn data into action with Pika.",
            "footer.product": "Product",
            "footer.company": "Company",
            "footer.security": "Security",
            "footer.kvkk": "Privacy",
            "footer.terms": "Terms",
            "footer.ctaTitle": "More revenue, higher loyalty",
            "footer.ctaDesc": "In 15 minutes, we’ll map the right flow for your needs.",
            "footer.contactSales": "Talk to Sales",
            "footer.rights": "All rights reserved.",
            "footer.made": "Designed for enterprise loyalty teams.",

            "demo.title": "Request a Demo",
            "demo.subtitle": "Leave a few details and our team will contact you.",
            "form.fullName": "Full name",
            "form.company": "Company",
            "form.email": "Email",
            "form.phone": "Phone",
            "form.message": "Message",
            "form.fullNamePh": "Full name",
            "form.companyPh": "Company",
            "form.emailPh": "name@company.com",
            "form.phonePh": "+1 ...",
            "form.messagePh": "Briefly describe your needs...",
            "form.privacy": "Your info stays private.",
            "form.close": "Close",
            "form.send": "Send",

            "home.badge": "Enterprise Loyalty & Campaign Automation",
            "home.h1": "Turn loyalty into measurable growth.",
            "home.lead": "Use segmentation, automated flows and analytics to reach customers at the right moment.",
            "home.cta1": "Request a Demo",
            "home.cta2": "Explore Solutions",
            "home.trust1": "Privacy-first",
            "home.trust2": "Fast rollout",
            "home.trust3": "Measurable ROI",
            "home.logosTitle": "Teams grow with Pika",
            "home.kpi1Title": "Conversion",
            "home.kpi2Title": "Repeat Purchase",
            "home.f1Title": "Flow-based Campaigns",
            "home.f1Desc": "Triggered automations: welcome, cart abandonment, winback.",
            "home.f2Title": "Loyalty Programs",
            "home.f2Desc": "Points, tiers, rewards and personalized benefits.",
            "home.f3Title": "Analytics & Insights",
            "home.f3Desc": "Cohorts, LTV, campaign performance in one dashboard.",
            "home.learnMore": "Details",
            "home.blockTitle": "From campaigns to loyalty: end-to-end growth",
            "home.blockDesc": "Pika unifies data, segments and actions. Teams iterate faster with less ops.",
            "home.check1": "Templates & quick onboarding",
            "home.check2": "Advanced segmentation & rule engine",
            "home.check3": "Real-time reporting, export and API",
            "home.seePricing": "See Pricing",
            "home.requestDemo": "Request Demo",
            "home.testTitle": "A UX teams love",
            "home.testDesc": "Enterprise look, clear flows, conversion-oriented UI.",
            "home.q1": "“Our campaign build time dropped significantly. We focus on strategy now.”",
            "home.q2": "“Having segmentation and analytics together accelerated decisions.”",
            "home.q3": "“Enterprise feel and usability built stakeholder confidence.”",
            "home.ctaTitle": "Let’s design the right flow in a demo",
            "home.ctaDesc": "In 15 minutes we’ll map goals and outline a rollout plan.",
            "home.ctaBtn": "Request a Demo",

            "sol.h1": "Solutions",
            "sol.lead": "Unify loyalty, campaigns, segmentation and analytics in one platform.",
            "sol.integrations": "Integrations",
            "sol.demo": "Request a Demo",
            "sol.c1t": "Loyalty Management",
            "sol.c1d": "Points/tiers, reward catalog, benefits and automated rules.",
            "sol.c2t": "Campaign Automation",
            "sol.c2d": "Triggers, flows, coupons and multi-channel delivery.",
            "sol.c3t": "Segmentation",
            "sol.c3d": "Dynamic audiences with behavioral & demographic filters.",
            "sol.c4t": "Analytics",
            "sol.c4d": "LTV, cohorts, contribution and funnel reports.",
            "sol.c5t": "Integrations",
            "sol.c5d": "Fast integration via API, webhooks and connectors.",
            "sol.ctat": "Tailored to you",
            "sol.ctad": "We’ll define the best setup and KPI set for your model.",
            "sol.ctab": "Request a Demo",

            "price.h1": "Pricing",
            "price.lead": "Plans that scale. Enterprise contracts available.",
            "price.best": "Most Popular",
            "price.custom": "Custom",
            "price.choose": "Get Quote",
            "price.contact": "Talk to Sales",
            "price.compare": "Compare Plans",
            "price.feature": "Feature",
            "price.row1": "Segmentation",
            "price.row2": "Automation Flows",
            "price.row3": "Advanced Analytics",
            "price.row4": "SSO / RBAC",
            "price.note": "Note: Prices are illustrative; final pricing depends on scope.",

            "corp.h1": "Company",
            "corp.lead": "Built for trust, sustainability and measurable growth.",
            "corp.demo": "Request a Demo",
            "corp.contact": "Contact",
            "corp.s1": "Uptime target",
            "corp.s2": "Role-based access",
            "corp.s3": "Integration architecture",
            "corp.s4": "Privacy-first",
            "corp.m1t": "Mission",
            "corp.m1d": "Make loyalty measurable and manageable.",
            "corp.m2t": "Values",
            "corp.m2d": "Trust, UX, data responsibility, continuity.",
            "corp.m3t": "Security",
            "corp.m3d": "Enterprise access control, audit trails and secure integrations.",
            "corp.ctaT": "Let’s plan your enterprise rollout",
            "corp.ctaD": "We’ll define scope, integrations and KPI roadmap.",
            "corp.ctaB": "Request a Demo",

            "contact.h1": "Contact",
            "contact.lead": "Questions? Reach sales or technical teams quickly.",
            "contact.formTitle": "Send us a message",
            "contact.demo": "Request a Demo",
            "contact.send": "Send",
            "contact.infoTitle": "Contact details",
            "contact.mail": "Email",
            "contact.phone": "Phone",
            "contact.address": "Address",
            "contact.map": "Map area",
            "contact.ctaT": "Need an enterprise quote?",
            "contact.ctaD": "We’ll tailor by scope and integrations.",
            "contact.ctaB": "Pricing",

            "login.title": "Enterprise Login",
            "login.subtitle": "Enter your credentials to access your panel.",
            "login.password": "Password",
            "login.remember": "Remember me",
            "login.forgot": "Forgot password",
            "login.signin": "Sign in",
            "login.or": "or",
            "login.contactSales": "Get an enterprise quote",
            "login.note": "Login attempts are logged and subject to security policies.",

            "search": "Search",
            "home.faqTitle": "Frequently Asked Questions",
            "home.heroWelcome": "Welcome to Pika",
            "home.heroTitle": "Custom Ad Management for Your Customers",
            "home.heroDesc": "At Pika Analysis, we transform online store advertising with advanced automation so e-commerce teams can focus on growth.",
            "home.heroBadge": "One Click Ahead",
            "home.card1": "Grow Your Business Efficiently",
            "home.card2": "Increase Profitability with Automation",
            "home.card2Desc": "Speed up campaign management and reduce costs with smart solutions.",
            "home.extraProfit": "Extra Profit",
            "home.card3": "Prioritize Customer Needs",
            "home.highlight": "Stand Out",
            "home.aiTitle": "Get Ahead of Competitors with AI Support",
            "home.aiDesc": "Achieve fast, measurable, and sustainable growth with AI-powered insights and reporting.",
            "home.ai1": "AI Support",
            "home.ai2": "Personalized Campaigns",
            "home.ai3": "BI Support",
            "home.ai4": "Comprehensive Reporting",
            "home.faqSub": "FAQ",
            "home.faqDesc": "Find quick answers to frequently asked questions about Pika Analysis solutions.",
            "home.faqQ1": "What is Pika Analysis?",
            "home.faqA1": "Pika is a platform that provides ad and campaign automation for e-commerce teams.",
            "home.faqQ2": "How does it automate?",
            "home.faqA2": "It reduces manual tasks with data-driven rules, segmentation, and ready-to-use flows.",
            "home.faqQ3": "Which platforms does it integrate with?",
            "home.faqA3": "It integrates via API with popular advertising and e-commerce infrastructures.",
            "form.lastNamePh": "Last name",
            "form.websitePh": "Website",
            "login.passwordPh": "••••••••",
            "login.secure": "Secure",

            "sol.m1t": "Loyalty",
            "sol.m1d": "Points, tiers, rewards",
            "sol.m2t": "Campaign",
            "sol.m2d": "Flows & automation",
            "sol.m3t": "Segment",
            "sol.m3d": "Rule engine",
            "sol.m4t": "Analytics",
            "sol.m4d": "KPI & insight",
            "price.p1t": "Start",
            "price.p1d": "Quick start for small teams.",
            "price.p1f1": "Basic segmentation",
            "price.p1f2": "Campaign templates",
            "price.p1f3": "Standard reports",
            "price.p2t": "Growth",
            "price.p2d": "Automation and analytics for growing teams.",
            "price.p2f1": "Advanced segmentation & dynamic audiences",
            "price.p2f2": "Automation flows",
            "price.p2f3": "Cohort & campaign performance",
            "price.p2f4": "Webhook/API access",
            "price.p3t": "Enterprise",
            "price.p3d": "Enterprise SLA, security, and custom integrations.",
            "price.p3f1": "SSO / RBAC",
            "price.p3f2": "Custom reports & BI integration",
            "price.p3f3": "Custom SLA & consulting",
            "loy.h1": "Loyalty Management",
            "loy.lead": "Build sustainable customer loyalty with points, tiers, and rewards.",
            "loy.demo": "Request a Demo",
            "loy.pricing": "Pricing",
            "loy.m1t": "Reward Catalog",
            "loy.m1d": "Flexible reward design",
            "loy.m2t": "Tiers",
            "loy.m2d": "Tier-based benefits",
            "loy.m3t": "Rules",
            "loy.m3d": "Automatic scoring",
            "loy.f1t": "Points & Redemption",
            "loy.f1d": "Earn and redeem points by purchase, category, or action.",
            "loy.f2t": "Membership Experience",
            "loy.f2d": "Enterprise UI showing points/tier status clearly.",
            "loy.f3t": "Fraud Prevention",
            "loy.f3d": "Rule limits, audit trails, and role-based authorization.",
            "loy.ctaT": "Let’s design your loyalty setup together",
            "loy.ctaD": "Define your points/tier model with KPI alignment.",
            "loy.ctaB": "Request a Demo",
            "camp.h1": "Campaign Automation",
            "camp.lead": "Reduce operations and increase conversion with triggers, coupons, and flows.",
            "camp.demo": "Request a Demo",
            "camp.next": "Segmentation",
            "camp.f1t": "Triggers",
            "camp.f1d": "Event-based flows: signup, first purchase, abandoned cart, winback.",
            "camp.f2t": "Coupon & Offer",
            "camp.f2d": "Rule-based coupon generation, limits, durations, and conditions.",
            "camp.f3t": "Channel Management",
            "camp.f3d": "Planning and performance tracking across email/SMS/push.",
            "seg.h1": "Segmentation",
            "seg.lead": "Build dynamic audiences and reach the right customer at the right moment.",
            "seg.demo": "Request a Demo",
            "seg.next": "Analytics",
            "seg.f1t": "Rule Engine",
            "seg.f1d": "Filters by behavior, frequency, cart, category, and time windows.",
            "seg.c1": "Dynamically updated segments",
            "seg.c2": "Cohort and LTV-based targeting",
            "seg.c3": "External systems via export/API",
            "seg.f2t": "Example Use Case",
            "seg.f2d": "Targets like: bought 2+ times in 30 days but inactive for 14 days.",
            "ana.h1": "Analytics",
            "ana.lead": "See real campaign impact; make decisions with LTV and cohorts.",
            "ana.demo": "Request a Demo",
            "ana.next": "Integrations",
            "ana.f1t": "KPI Dashboard",
            "ana.f1d": "Conversion, repeat purchase, basket and channel contribution.",
            "ana.f2t": "Cohort",
            "ana.f2d": "Track behavior change over time and monitor retention.",
            "ana.f3t": "Reporting",
            "ana.f3d": "CSV export and BI/warehouse flows via API.",
            "int.h1": "Integrations",
            "int.lead": "Connect quickly to your existing systems with API & webhooks.",
            "int.demo": "Request a Demo",
            "int.contact": "Technical Meeting",
            "int.f1t": "REST API",
            "int.f1d": "Send customer, transaction and event data securely.",
            "int.f2t": "Webhook",
            "int.f2d": "Send campaign and loyalty actions back to your systems.",
            "int.c1": "Event-driven architecture",
            "int.c2": "Retry & signature (recommended)",
            "int.c3": "Documentation & sample payload"
        }
    };

    function getPathLang() {
        const part = window.location.pathname.split("/").filter(Boolean)[0];
        return part === "tr" || part === "en" ? part : null;
    }

    function getLang() {
        return getPathLang() || localStorage.getItem("pika_lang") || document.documentElement.getAttribute("data-lang") || "tr";
    }

    function setLang(lang, syncUrl = false) {
        const safe = i18n[lang] ? lang : "tr";
        localStorage.setItem("pika_lang", safe);
        document.documentElement.setAttribute("lang", safe);
        document.documentElement.setAttribute("data-lang", safe);

        const label = qs("#langLabel");
        if (label) label.textContent = safe.toUpperCase();
        qsa("[data-lang-label]").forEach(el => {
            el.textContent = safe.toUpperCase();
        });

        qsa("[data-i18n]").forEach(el => {
            const key = el.getAttribute("data-i18n");
            const value = i18n[safe][key];
            if (value) el.textContent = value;
        });

        qsa("[data-i18n-placeholder]").forEach(el => {
            const key = el.getAttribute("data-i18n-placeholder");
            const value = i18n[safe][key];
            if (value) el.setAttribute("placeholder", value);
        });

        if (syncUrl) {
            const pathLang = getPathLang();
            if (pathLang !== safe) {
                const parts = window.location.pathname.split("/").filter(Boolean);
                const rest = pathLang ? parts.slice(1) : parts;
                const nextPath = `/${safe}${rest.length ? `/${rest.join("/")}` : ""}`;
                window.location.assign(`${nextPath}${window.location.search}${window.location.hash}`);
            }
        }
    }

    // Language buttons
    function bindLangButtons() {
        qsa("[data-lang-set]").forEach(btn => {
            btn.addEventListener("click", () => {
                setLang(btn.getAttribute("data-lang-set"), true);
            });
        });
    }

    // Demo modal submit
    function bindDemoForm() {
        const form = qs("#demoRequestForm");
        if (!form) return;

        const result = qs("#demoResult");
        const showResult = (ok, msg) => {
            if (!result) return;
            result.classList.remove("d-none");
            result.classList.toggle("pika-alert--ok", !!ok);
            result.classList.toggle("pika-alert--err", !ok);
            result.textContent = msg;
        };

        form.addEventListener("submit", async (e) => {
            e.preventDefault();

            const lang = getLang();
            const payload = {
                firstName: form.firstName?.value?.trim() || "",
                lastName: form.lastName?.value?.trim() || "",
                email: form.email?.value?.trim() || "",
                phone: form.phone?.value?.trim() || "",
                companyName: form.companyName?.value?.trim() || "",
                website: form.website?.value?.trim() || "",
                message: form.message?.value?.trim() || "",
                lang
            };

            if (!payload.firstName || !payload.lastName || !payload.email || !payload.phone || !payload.companyName || !payload.website) {
                showResult(false, lang === "en" ? "Please fill required fields." : "Lütfen zorunlu alanları doldurun.");
                return;
            }

            try {
                const res = await fetch(`/${lang}/lead/demo-request`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
                const data = await res.json();
                if (res.ok && data.ok) {
                    showResult(true, data.message || (lang === "en" ? "Request received." : "Talebiniz alındı."));
                    form.reset();
                } else {
                    showResult(false, data.message || (lang === "en" ? "Something went wrong." : "Bir hata oluştu."));
                }
            } catch {
                showResult(false, lang === "en" ? "Network error." : "Ağ hatası.");
            }
        });
    }

    // AOS init
    function initAOS() {
        if (window.AOS) AOS.init({ duration: 650, once: true, offset: 80 });
    }

    // Swiper init
    function initSwiper() {
        const el = document.getElementById("testimonialsSwiper");
        if (!el || !window.Swiper) return;

        const swiper = new Swiper("#testimonialsSwiper", {
            slidesPerView: 1,
            spaceBetween: 18,
            breakpoints: { 992: { slidesPerView: 3 } }
        });

        const prev = document.getElementById("swiperPrev");
        const next = document.getElementById("swiperNext");
        if (prev) prev.addEventListener("click", () => swiper.slidePrev());
        if (next) next.addEventListener("click", () => swiper.slideNext());
    }


    function hidePreloader() {
        const preloader = document.getElementById("preloader");
        if (!preloader) return;
        preloader.classList.add("is-hidden");
        document.body.classList.remove("preloading");
        setTimeout(() => preloader.remove(), 400);
    }

    function bindPreloader() {
        document.body.classList.add("preloading");

        if (document.readyState === "complete") {
            hidePreloader();
        } else {
            window.addEventListener("load", hidePreloader, { once: true });
            setTimeout(hidePreloader, 3000);
        }
    }

    // Header shrink on scroll
    function bindHeader() {
        const header = document.querySelector(".pika-header");
        if (!header) return;
        const onScroll = () => header.classList.toggle("pika-header--scrolled", window.scrollY > 10);
        window.addEventListener("scroll", onScroll, { passive: true });
        onScroll();
    }

    // Optional toast
    function showToastIfAny() {
        const el = document.querySelector(".pika-toast");
        if (!el) return;
        const msg = el.getAttribute("data-toast");
        if (!msg) return;

        const toast = document.createElement("div");
        toast.className = "pika-toast-ui";
        toast.innerHTML = `<div class="pika-toast-inner"><i class="fa-solid fa-circle-info me-2"></i>${msg}</div>`;
        document.body.appendChild(toast);

        setTimeout(() => toast.classList.add("show"), 50);
        setTimeout(() => toast.classList.remove("show"), 3500);
        setTimeout(() => toast.remove(), 4200);
    }

    // Boot
    document.addEventListener("DOMContentLoaded", () => {
        bindLangButtons();
        setLang(getLang());
        bindDemoForm();
        initAOS();
        initSwiper();
        bindHeader();
        showToastIfAny();
        bindPreloader();
    });
})();
