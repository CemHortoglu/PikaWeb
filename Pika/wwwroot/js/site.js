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
            "nav.solutions.journey": "Journey",
            "nav.solutions.loyalty": "Sadakat",
            "nav.solutions.campaigns": "Kampanyalar",
            "nav.solutions.segmentation": "Segmentasyon",
            "nav.solutions.analytics": "Analitik",
            "nav.solutions.integrations": "Entegrasyonlar",
            "nav.solutions.security": "Güvenlik",
            "nav.pricing": "Fiyatlandırma",
            "nav.corporate": "Kurumsal",
            "nav.pika": "Pika",
            "nav.faq": "Sık Sorulan Sorular",
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
            "sol.c0t": "Journey Orkestrasyonu",
            "sol.c0d": "Müşteri yolculuğunu tek bir canvas üzerinde kurun, test edin ve optimize edin.",
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
            "sol.c6t": "Kurumsal Güvenlik ve Uyum",
            "sol.c6d": "RBAC, denetim izleri ve gizlilik odaklı yönetişim.",
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

            "sec.h1": "Kurumsal Güvenlik ve Uyum",
            "sec.lead": "Rol bazlı yetki, denetim izleri ve güvenli entegrasyon yaklaşımıyla ölçeklenin.",
            "sec.demo": "Demo Talebi",
            "sec.contact": "Güvenlik Görüşmesi",
            "sec.midT": "Güvenlik ve uyumu operasyonun doğal parçası haline getirin",
            "sec.midD": "Erişim, denetim ve veri yönetişimi başlıklarını tek standart altında toplayın.",
            "sec.k1": "Rol bazlı yetkilendirme ve kritik aksiyon onay adımları",
            "sec.k2": "İzlenebilir işlem geçmişi ve log görünürlüğü",
            "sec.k3": "Kurumsal denetim süreçlerine uyumlu yapı",
            "sec.ctaT": "Güvenlikle birlikte planlayalım",
            "sec.ctaD": "Rol modeli, log kapsamı ve entegrasyon güvenliğini birlikte netleştirelim.",
            "sec.ctaB": "Demo Talebi",

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
            "jour.h1": "Journey Orkestrasyonu",
            "jour.chip": "Kurumsal Journey Platformu",
            "jour.lead": "Müşteri temas noktalarını tek akışta yönetin, denetleyin ve optimize edin.",
            "jour.kpi1v": "%32",
            "jour.kpi1l": "Operasyonel hızlanma",
            "jour.kpi2v": "%99,9",
            "jour.kpi2l": "Akış sürekliliği",
            "jour.kpi3v": "7/24",
            "jour.kpi3l": "Gerçek zamanlı görünürlük",
            "jour.demo": "Demo Talebi",
            "jour.next": "Kampanyalar",
            "jour.topicsT": "Kurumsal Journey Odak Alanları",
            "jour.topicsD": "Akış yönetimi, yönetişim ve ölçüm katmanlarını tek panelde görün.",
            "jour.stageT": "Journey Yaşam Döngüsü",
            "jour.stage1": "Hedef segment ve temas stratejisini tanımlayın",
            "jour.stage2": "Kanal bazlı aksiyonları tek panelde modelleyin",
            "jour.stage3": "Onay ve uyum süreçleriyle güvenli yayın alın",
            "jour.stage4": "Performansı canlı izleyip akışı optimize edin",
            "jour.f1t": "Merkezi Orkestrasyon",
            "jour.f1d": "Pazarlama, CRM ve operasyon ekiplerini aynı journey standardında hizalayın.",
            "jour.f2t": "Yönetişim Katmanı",
            "jour.f2d": "Onay adımları, rol bazlı yetki ve log kayıtları ile kurumsal denetimi güçlendirin.",
            "jour.f3t": "Ölçülebilir Sonuç",
            "jour.f3d": "Her aşamanın katkısını KPI bazında izleyerek iyileştirme döngüsü oluşturun.",
            "jour.midT": "Uçtan uca journey tasarımını kurumsal standarda taşıyın",
            "jour.midD": "Farklı ekiplerin aynı akış üzerinde çalışmasını sağlayan merkezi bir journey kurgusu oluşturun.",
            "jour.k1": "Temas noktası, kanal ve aksiyonları tek bir yolculuk haritasında toplama",
            "jour.k2": "Onay mekanizmalarıyla kontrollü yayın süreçleri",
            "jour.k3": "A/B senaryoları ile sürekli iyileştirme",
            "jour.boardT": "Yönetişim Panosu",
            "jour.boardL1": "Versiyon Yönetimi",
            "jour.boardV1": "Akış geçmişi ve geri alma desteği",
            "jour.boardL2": "Yetkilendirme",
            "jour.boardV2": "RBAC ile ekip bazlı erişim sınırları",
            "jour.boardL3": "Uyumluluk",
            "jour.boardV3": "Onay kayıtları ve denetim izi",
            "jour.boardL4": "Raporlama",
            "jour.boardV4": "Kanal, segment ve temas bazlı performans",
            "jour.ctaT": "Journey kurgunuzu birlikte tasarlayalım",
            "jour.ctaD": "Sizin müşteri yaşam döngünüze uygun örnek bir akış planlayalım.",
            "jour.ctaB": "Demo Talebi",
            "pika.h1": "Pika Platformu",
            "pika.chip": "Kurumsal Sadakat ve Kampanya Platformu",
            "pika.lead": "Pika; müşteri verisi, segmentasyon, sadakat ve kampanya orkestrasyonunu tek merkezde birleştirerek ekiplerin daha hızlı karar almasını ve sürdürülebilir büyüme üretmesini sağlar.",
            "pika.kpi1v": "%27",
            "pika.kpi1l": "Tekrar satın alma artışı",
            "pika.kpi2v": "%41",
            "pika.kpi2l": "Kampanya üretim hızlanması",
            "pika.kpi3v": "360°",
            "pika.kpi3l": "Birleşik müşteri görünümü",
            "pika.demo": "Demo Talebi",
            "pika.contact": "Bizimle İletişime Geçin",
            "pika.boardT": "Platform Kontrol Merkezi",
            "pika.boardL1": "Veri Birleştirme",
            "pika.boardV1": "Online/offline temaslardan tek profil üretimi",
            "pika.boardL2": "Segment Motoru",
            "pika.boardV2": "Davranış, değer ve yaşam döngüsü bazlı dinamik segmentler",
            "pika.boardL3": "Kampanya Orkestrasyonu",
            "pika.boardV3": "Omnichannel akışlar ve otomatik tetikleyiciler",
            "pika.boardL4": "Kurumsal Analitik",
            "pika.boardV4": "KPI, ROI ve kohort performansının canlı takibi",
            "pika.f1t": "360° Müşteri Görünümü",
            "pika.f1d": "CRM, e-ticaret, çağrı merkezi ve mağaza verilerini tek müşteri profilinde birleştirerek ekipler arası ortak bir gerçeklik oluşturur.",
            "pika.f2t": "Akıllı Segmentasyon ve Kural Motoru",
            "pika.f2d": "Sıklık, sepet değeri, ürün ilgisi ve terk etme davranışı gibi sinyalleri kullanarak doğru kitleye doğru teklifin otomatik ulaşmasını sağlar.",
            "pika.f3t": "Gerçek Zamanlı Aksiyon ve Ölçüm",
            "pika.f3d": "Müşteri aksiyonlarına milisaniye seviyesinde tepki veren senaryoları devreye alır; performansı anlık ölçerek sürekli optimizasyon sunar.",
            "pika.ctaT": "Pika ile müşteri deneyimini ölçülebilir büyümeye dönüştürün",
            "pika.ctaD": "Pazarlama, CRM, operasyon ve analitik ekiplerini aynı hedefte buluşturan kurumsal bir çalışma modeli kurun.",
            "pika.ctaL1": "Sadakat programlarını tek panelden yönetme",
            "pika.ctaL2": "Omnichannel kampanya kurgusu ve otomatik tetikleyici yönetimi",
            "pika.ctaL3": "Detaylı KPI, ROI ve segment performans karşılaştırmaları",
            "pika.ctaL4": "Rol bazlı yetkilendirme, audit log ve uyumluluk süreçleri",
            "pika.ctaB": "Platformu Canlı Görün",
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
            "loy.midT": "Kurumsal sadakat deneyimini tek panelden yönetin",
            "loy.midD": "Sadakat programınızın hedeflerini, kural setlerini ve üye deneyimini tek bir standartta yönetin.",
            "loy.k1": "Esnek puan/level modelleri ile segment bazlı kural tanımları",
            "loy.k2": "Kurumsal raporlama için tekil müşteri geçmişi ve denetim izi",
            "loy.k3": "Pazarlama ve müşteri hizmetleri ekipleri için ortak görünüm",
            "loy.ctaT": "Sadakat kurgunuzu birlikte tasarlayalım",
            "loy.ctaD": "Sektörünüze uygun puan/level modelini KPI’larla netleştirelim.",
            "loy.ctaB": "Demo Talebi",

            "camp.h1": "Kampanya Otomasyonu",
            "camp.lead": "Tetikleyiciler, kuponlar ve akışlarla operasyonu azaltın, dönüşümü artırın.",
            "camp.demo": "Demo Talebi",
            "camp.next": "Segmentasyon",
            "camp.midT": "Tüm kampanya adımlarını kurumsal bir akışta birleştirin",
            "camp.midD": "Planlama, onay ve yayın adımlarını tek bir görünümde yönetin; operasyonu azaltın.",
            "camp.k1": "Tetikleyici, zamanlama ve hedef kitleyi tek ekranda kurgulama",
            "camp.k2": "Kupon, limit ve bütçe kontrollerinde merkezi yönetişim",
            "camp.k3": "Kanal bazlı sonuçların anlık izlenmesi ve karşılaştırılması",
            "camp.ctaT": "Kampanya operasyonunu birlikte sadeleştirelim",
            "camp.ctaD": "Ekibinizin akışına uygun kurgu için örnek bir senaryo çıkaralım.",
            "camp.ctaB": "Demo Talebi",

            "seg.h1": "Segmentasyon",
            "seg.lead": "Dinamik kitleler oluşturun; doğru müşteriye doğru anda ulaşın.",
            "seg.demo": "Demo Talebi",
            "seg.next": "Analitik",
            "seg.midT": "Segmentleri canlı davranış verisiyle sürekli güncel tutun",
            "seg.midD": "Hedef kitlenizi sabit listeler yerine anlık davranışlara göre otomatik yenileyin.",
            "seg.k1": "Demografik + davranışsal filtreleri tek kurala bağlama",
            "seg.k2": "Kampanyaya hazır dinamik segment yapısı",
            "seg.k3": "BI ve CRM sistemleriyle çift yönlü veri akışı",
            "seg.ctaT": "Segment stratejinizi beraber netleştirelim",
            "seg.ctaD": "Örnek bir hedef kitleden başlayıp canlı bir kurgu tasarlayalım.",
            "seg.ctaB": "Demo Talebi",

            "ana.h1": "Analitik",
            "ana.lead": "Kampanyalarınızın gerçek katkısını görün; LTV ve kohortlarla karar alın.",
            "ana.demo": "Demo Talebi",
            "ana.next": "Entegrasyonlar",
            "ana.midT": "Karar süreçlerini net KPI görselleriyle hızlandırın",
            "ana.midD": "Tüm metrikleri tek bir yönetim ekranında birleştirerek ekipler arası ortak dil oluşturun.",
            "ana.k1": "Kampanya, kanal ve segment performansını birlikte okuma",
            "ana.k2": "Yönetim raporları için hazır gösterge setleri",
            "ana.k3": "Veriyi dış sistemlere taşıyan kurumsal raporlama altyapısı",
            "ana.ctaT": "Analitik yapınızı kurumsal seviyeye taşıyalım",
            "ana.ctaD": "Sizin KPI setinize göre örnek dashboard planı oluşturalım.",
            "ana.ctaB": "Demo Talebi",

            "int.h1": "Entegrasyonlar",
            "int.lead": "API & webhook yaklaşımıyla mevcut sistemlerinize hızlıca bağlanın.",
            "int.demo": "Demo Talebi",
            "int.contact": "Teknik Görüşme",
            "int.midT": "Mevcut teknoloji yığınınıza kontrollü ve hızlı entegre olun",
            "int.midD": "Teknik ekiplerinize uygun bir entegrasyon yol haritası ile canlıya geçiş riskini azaltın.",
            "int.k1": "API, webhook ve dosya aktarımı senaryolarını birlikte yönetme",
            "int.k2": "Yetkilendirme, loglama ve hata yönetiminde standart yaklaşım",
            "int.k3": "Test ortamından canlıya adım adım geçiş planı",
            "int.ctaT": "Entegrasyon kapsamını birlikte planlayalım",
            "int.ctaD": "Teknik ekibinizle uyumlu bir geçiş planı için demo oluşturalım.",
            "int.ctaB": "Demo Talebi",

            "faq.h1": "Sık Sorulan Sorular",
            "faq.lead": "Pika platformu, demo süreci, güvenlik ve entegrasyon hakkında en çok merak edilen soruları bu sayfada topladık.",
            "faq.q1": "Pika hangi sektörlerde kullanılabilir?",
            "faq.a1": "Perakende, finans, telekom, e-ticaret ve üyelik tabanlı tüm sektörlerde kullanılabilir. Modüler yapısı sayesinde iş ihtiyaçlarına göre ölçeklenir.",
            "faq.q2": "Demo süreci nasıl ilerliyor?",
            "faq.a2": "Demo talep formunu doldurduktan sonra ekibimiz sizinle iletişime geçer. İhtiyaç toplantısı sonrası kurumunuza özel bir canlı demo planlanır.",
            "faq.q3": "Mevcut sistemlerimizle entegrasyon mümkün mü?",
            "faq.a3": "Evet. API tabanlı mimari ile CRM, ERP, e-ticaret, çağrı merkezi ve veri ambarı sistemleriyle çift yönlü entegrasyon sağlanabilir.",
            "faq.q4": "Veri güvenliği ve yetkilendirme nasıl sağlanıyor?",
            "faq.a4": "Rol bazlı erişim, işlem logları, güvenli veri aktarımı ve kurum politikalarına uyumlu saklama prensipleri ile güvenlik katmanları uygulanır.",

            "faqpro.chip": "Pika Yardım Merkezi",
            "faqpro.title": "Sık Sorulan Sorular",
            "faqpro.lead": "Entegrasyon, güvenlik, onboarding ve operasyon süreçleriyle ilgili en sık gelen soruları tek ekranda bir araya getirdik. Hızlı cevaplar için kategori kartlarını, detaylı bilgi için aşağıdaki akordeon bloklarını kullanabilirsiniz.",
            "faqpro.stat1Value": "12+",
            "faqpro.stat1Label": "Entegrasyon bağlantısı",
            "faqpro.stat2Value": "< 7 gün",
            "faqpro.stat2Label": "Ortalama canlıya geçiş",
            "faqpro.stat3Value": "7/24",
            "faqpro.stat3Label": "İzleme ve bildirim",
            "faqpro.topicsTitle": "Öne Çıkan Konular",
            "faqpro.topicsLead": "Aradığınız başlığa göre doğrudan ilgili soruları açabilirsiniz.",
            "faqpro.topic1Title": "Entegrasyon",
            "faqpro.topic1Copy": "API, veri akışı, sistem bağlantıları",
            "faqpro.topic2Title": "Güvenlik",
            "faqpro.topic2Copy": "KVKK, yetkilendirme, denetim kayıtları",
            "faqpro.topic3Title": "Onboarding",
            "faqpro.topic3Copy": "Kurulum takvimi, ekip rolleri, eğitim",
            "faqpro.cat1Label": "Başlangıç",
            "faqpro.cat1Title": "Kurulum ve Onboarding",
            "faqpro.cat1Desc": "Kurumsal kurulum adımları, erişim rolleri ve ekip eğitim süreci ile ilgili temel sorular.",
            "faqpro.cat2Label": "Teknik",
            "faqpro.cat2Title": "Entegrasyon ve Veri",
            "faqpro.cat2Desc": "ERP, CRM, e-ticaret altyapıları ve veri ambarı bağlantılarında sık yaşanan teknik senaryolar.",
            "faqpro.cat3Label": "Operasyon",
            "faqpro.cat3Title": "Performans ve Destek",
            "faqpro.cat3Desc": "SLA, destek kanalları, sürüm yönetimi ve üretim ortamı operasyonlarıyla ilgili açıklamalar.",
            "faqpro.q1": "Pika hangi sektörlerde kullanılabilir?",
            "faqpro.a1": "Perakende, finans, telekom, e-ticaret ve üyelik tabanlı sektörlerin tamamında kullanılabilir. Modüler mimarisi sayesinde kurumunuza göre ölçeklenir.",
            "faqpro.q2": "Demo süreci nasıl ilerliyor?",
            "faqpro.a2": "Demo talebiniz sonrası satış mühendislerimiz ihtiyaç analizi yapar. Ardından kurumunuza özel senaryo seti hazırlanır ve canlı demo toplantısı planlanır.",
            "faqpro.q3": "Mevcut sistemlerimizle entegrasyon mümkün mü?",
            "faqpro.a3": "Evet. Pika; CRM, ERP, e-ticaret altyapıları, çağrı merkezi ve veri ambarı çözümleriyle API üzerinden çift yönlü entegrasyon sağlar.",
            "faqpro.q4": "Veri güvenliği ve yetkilendirme nasıl sağlanıyor?",
            "faqpro.a4": "Rol bazlı erişim, işlem logları, şifrelenmiş veri aktarımı, audit kayıtları ve kurum politikalarına uyumlu saklama süreçleri birlikte uygulanır.",
            "faqpro.q5": "Destek ekibine hangi kanallardan ve ne kadar sürede ulaşabiliriz?",
            "faqpro.a5": "Destek taleplerinizi portal, e-posta ve öncelikli hat üzerinden iletebilirsiniz. Kritik kayıtlar için 30 dakika içinde geri dönüş, standart taleplerde aynı iş günü içinde yanıt hedeflenir.",

            "search": "Ara",
            "home.faqTitle": "Sık Sorulan Sorular",
            "home.heroWelcome": "Pika'ya Hoş Geldiniz",
            "home.heroTitle": "Müşterinize <span><img src=\"/web/images/svg/your.svg\" alt=\"image\"> Özel </span> Reklam Yönetimi",
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
            "nav.solutions.journey": "Journey",
            "nav.solutions.loyalty": "Loyalty",
            "nav.solutions.campaigns": "Campaigns",
            "nav.solutions.segmentation": "Segmentation",
            "nav.solutions.analytics": "Analytics",
            "nav.solutions.integrations": "Integrations",
            "nav.solutions.security": "Security",
            "nav.pricing": "Pricing",
            "nav.corporate": "Company",
            "nav.pika": "Pika",
            "nav.faq": "Frequently Asked Questions",
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
            "sol.c0t": "Journey Orchestration",
            "sol.c0d": "Design, test, and optimize customer journeys on a single canvas.",
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
            "sol.c6t": "Enterprise Security & Compliance",
            "sol.c6d": "RBAC, audit trails, and privacy-first governance for scale.",
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

            "sec.h1": "Enterprise Security & Compliance",
            "sec.lead": "Scale with role-based access, audit trails, and a secure integration approach.",
            "sec.demo": "Request a Demo",
            "sec.contact": "Security Call",
            "sec.midT": "Make security and compliance part of daily operations",
            "sec.midD": "Align access, auditing, and governance controls in one standard model.",
            "sec.k1": "Role-based access and approval steps for sensitive actions",
            "sec.k2": "End-to-end traceability with operational logs",
            "sec.k3": "Structure aligned with enterprise compliance reviews",
            "sec.ctaT": "Plan rollout with security in mind",
            "sec.ctaD": "Align roles, logging scope, and integration controls with your governance needs.",
            "sec.ctaB": "Request a Demo",

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
            "home.heroTitle": "Ad Management <span><img src=\"/web/images/svg/your.svg\" alt=\"image\"> Tailored </span> for Your Customers",
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
            "jour.h1": "Journey Orchestration",
            "jour.chip": "Enterprise Journey Platform",
            "jour.lead": "Manage, govern, and optimize customer touchpoints in one flow.",
            "jour.kpi1v": "32%",
            "jour.kpi1l": "Operational acceleration",
            "jour.kpi2v": "99.9%",
            "jour.kpi2l": "Flow continuity",
            "jour.kpi3v": "24/7",
            "jour.kpi3l": "Real-time visibility",
            "jour.demo": "Request a Demo",
            "jour.next": "Campaigns",
            "jour.topicsT": "Enterprise Journey Focus Areas",
            "jour.topicsD": "Track orchestration, governance, and measurement layers from one panel.",
            "jour.stageT": "Journey Lifecycle",
            "jour.stage1": "Define target segments and engagement strategy",
            "jour.stage2": "Model channel actions from a single control panel",
            "jour.stage3": "Publish safely with approval and compliance checkpoints",
            "jour.stage4": "Track performance live and optimize the flow",
            "jour.f1t": "Central Orchestration",
            "jour.f1d": "Align marketing, CRM, and operations teams with one journey standard.",
            "jour.f2t": "Governance Layer",
            "jour.f2d": "Reinforce enterprise control through approval steps, RBAC, and audit logs.",
            "jour.f3t": "Measurable Outcomes",
            "jour.f3d": "Build a continuous optimization loop by tracking KPI impact at every stage.",
            "jour.midT": "Bring end-to-end journey design to enterprise standards",
            "jour.midD": "Create a centralized journey model where multiple teams collaborate in one flow.",
            "jour.k1": "Map channels, touchpoints, and actions in one journey canvas",
            "jour.k2": "Controlled publishing with approval checkpoints",
            "jour.k3": "Continuous optimization through A/B scenarios",
            "jour.boardT": "Governance Board",
            "jour.boardL1": "Version Management",
            "jour.boardV1": "Flow history and rollback support",
            "jour.boardL2": "Authorization",
            "jour.boardV2": "Team-level access boundaries with RBAC",
            "jour.boardL3": "Compliance",
            "jour.boardV3": "Approval records and auditable trail",
            "jour.boardL4": "Reporting",
            "jour.boardV4": "Performance by channel, segment, and touchpoint",
            "jour.ctaT": "Design your journey model with us",
            "jour.ctaD": "We can plan a sample flow around your customer lifecycle.",
            "jour.ctaB": "Request a Demo",
            "pika.h1": "Pika Platform",
            "pika.chip": "Enterprise Loyalty and Campaign Platform",
            "pika.lead": "Pika unifies customer data, segmentation, loyalty, and campaign orchestration in one center so teams can make faster decisions and deliver sustainable growth.",
            "pika.kpi1v": "27%",
            "pika.kpi1l": "Increase in repeat purchases",
            "pika.kpi2v": "41%",
            "pika.kpi2l": "Faster campaign production",
            "pika.kpi3v": "360°",
            "pika.kpi3l": "Unified customer view",
            "pika.demo": "Request a Demo",
            "pika.contact": "Contact Us",
            "pika.boardT": "Platform Control Center",
            "pika.boardL1": "Data Unification",
            "pika.boardV1": "Single profile creation from online and offline touchpoints",
            "pika.boardL2": "Segmentation Engine",
            "pika.boardV2": "Dynamic segments based on behavior, value, and lifecycle",
            "pika.boardL3": "Campaign Orchestration",
            "pika.boardV3": "Omnichannel flows and automated triggers",
            "pika.boardL4": "Enterprise Analytics",
            "pika.boardV4": "Live tracking of KPI, ROI, and cohort performance",
            "pika.f1t": "360° Customer View",
            "pika.f1d": "It consolidates CRM, e-commerce, call center, and in-store data into a single customer profile to create a shared source of truth across teams.",
            "pika.f2t": "Smart Segmentation and Rules Engine",
            "pika.f2d": "It uses signals such as frequency, basket value, product interest, and churn intent to automatically deliver the right offer to the right audience.",
            "pika.f3t": "Real-Time Action and Measurement",
            "pika.f3d": "It activates scenarios that react to customer behavior in milliseconds and continuously optimizes performance through real-time measurement.",
            "pika.ctaT": "Turn customer experience into measurable growth with Pika",
            "pika.ctaD": "Build an enterprise operating model that aligns marketing, CRM, operations, and analytics teams around shared goals.",
            "pika.ctaL1": "Manage loyalty programs from a single panel",
            "pika.ctaL2": "Orchestrate omnichannel campaign journeys with automated triggers",
            "pika.ctaL3": "Compare KPI, ROI, and segment performance with detailed insights",
            "pika.ctaL4": "Strengthen governance with role-based access, audit logs, and compliance flows",
            "pika.ctaB": "See the Platform Live",
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
            "loy.midT": "Manage enterprise loyalty in one consistent interface",
            "loy.midD": "Run loyalty objectives, rule sets, and member experience from a shared operational view.",
            "loy.k1": "Flexible points/tier models with segment-specific rules",
            "loy.k2": "Audit-ready customer timeline for enterprise reporting",
            "loy.k3": "Shared visibility across marketing and support teams",
            "loy.ctaT": "Let’s design your loyalty setup together",
            "loy.ctaD": "Define your points/tier model with KPI alignment.",
            "loy.ctaB": "Request a Demo",
            "camp.h1": "Campaign Automation",
            "camp.lead": "Reduce operations and increase conversion with triggers, coupons, and flows.",
            "camp.demo": "Request a Demo",
            "camp.next": "Segmentation",
            "camp.midT": "Unify campaign steps in a corporate-grade workflow",
            "camp.midD": "Handle planning, approvals, and publishing from one streamlined workspace.",
            "camp.k1": "Configure triggers, timing, and audiences in one place",
            "camp.k2": "Central governance for coupons, limits, and budget controls",
            "camp.k3": "Real-time channel performance monitoring",
            "camp.ctaT": "Simplify your campaign operations",
            "camp.ctaD": "Let's create a sample journey based on your team's flow.",
            "camp.ctaB": "Request a Demo",
            "seg.h1": "Segmentation",
            "seg.lead": "Build dynamic audiences and reach the right customer at the right moment.",
            "seg.demo": "Request a Demo",
            "seg.next": "Analytics",
            "seg.midT": "Keep audiences live with behavioral updates",
            "seg.midD": "Replace static lists with dynamic segments that update automatically.",
            "seg.k1": "Combine demographic and behavioral logic in one rule set",
            "seg.k2": "Campaign-ready dynamic audience structure",
            "seg.k3": "Bi-directional data sync with BI and CRM tools",
            "seg.ctaT": "Define your segmentation strategy",
            "seg.ctaD": "Start with one audience and design a live setup together.",
            "seg.ctaB": "Request a Demo",
            "ana.h1": "Analytics",
            "ana.lead": "See real campaign impact; make decisions with LTV and cohorts.",
            "ana.demo": "Request a Demo",
            "ana.next": "Integrations",
            "ana.midT": "Accelerate decisions with clear KPI views",
            "ana.midD": "Bring metrics into one executive view and align teams around common outcomes.",
            "ana.k1": "Read campaign, channel, and segment performance together",
            "ana.k2": "Prebuilt executive reporting indicators",
            "ana.k3": "Enterprise reporting flow to external analytics stacks",
            "ana.ctaT": "Upgrade your analytics operating model",
            "ana.ctaD": "We can map a sample dashboard with your KPI set.",
            "ana.ctaB": "Request a Demo",
            "int.h1": "Integrations",
            "int.lead": "Connect quickly to your existing systems with API & webhooks.",
            "int.demo": "Request a Demo",
            "int.contact": "Technical Meeting",
            "int.midT": "Integrate into your existing stack with controlled rollout",
            "int.midD": "Reduce go-live risk with an enterprise-ready integration path.",
            "int.k1": "Manage API, webhook, and file-based scenarios together",
            "int.k2": "Standardized auth, logging, and error handling",
            "int.k3": "Step-by-step path from sandbox to production",
            "int.ctaT": "Plan your integration scope",
            "int.ctaD": "Let's build a demo aligned with your technical architecture.",
            "int.ctaB": "Request a Demo",

            "faq.h1": "Frequently Asked Questions",
            "faq.lead": "We collected the most common questions about the Pika platform, demo process, security, and integrations on this page.",
            "faq.q1": "Which industries can use Pika?",
            "faq.a1": "It can be used in retail, finance, telecom, e-commerce, and all membership-based sectors. Its modular structure scales to business needs.",
            "faq.q2": "How does the demo process work?",
            "faq.a2": "After you submit the demo request form, our team contacts you. Following a needs meeting, we schedule a live demo tailored to your organization.",
            "faq.q3": "Is integration with our existing systems possible?",
            "faq.a3": "Yes. With API-based architecture, two-way integration can be provided with CRM, ERP, e-commerce, call center, and data warehouse systems.",
            "faq.q4": "How are data security and authorization handled?",
            "faq.a4": "Security layers are implemented with role-based access, activity logs, secure data transfer, and storage principles aligned with corporate policies.",

            "faqpro.chip": "Pika Help Center",
            "faqpro.title": "Frequently Asked Questions",
            "faqpro.lead": "We brought together the most frequently asked questions about integration, security, onboarding, and operations on a single screen. You can use the category cards for quick answers and the accordion blocks below for detailed information.",
            "faqpro.stat1Value": "12+",
            "faqpro.stat1Label": "Integration connections",
            "faqpro.stat2Value": "< 7 days",
            "faqpro.stat2Label": "Average go-live time",
            "faqpro.stat3Value": "24/7",
            "faqpro.stat3Label": "Monitoring and alerts",
            "faqpro.topicsTitle": "Highlighted Topics",
            "faqpro.topicsLead": "You can open relevant questions directly based on the topic you are looking for.",
            "faqpro.topic1Title": "Integration",
            "faqpro.topic1Copy": "API, data flow, system connections",
            "faqpro.topic2Title": "Security",
            "faqpro.topic2Copy": "Data privacy, authorization, audit logs",
            "faqpro.topic3Title": "Onboarding",
            "faqpro.topic3Copy": "Setup timeline, team roles, training",
            "faqpro.cat1Label": "Getting Started",
            "faqpro.cat1Title": "Setup and Onboarding",
            "faqpro.cat1Desc": "Core questions about enterprise setup steps, access roles, and team training process.",
            "faqpro.cat2Label": "Technical",
            "faqpro.cat2Title": "Integration and Data",
            "faqpro.cat2Desc": "Common technical scenarios for ERP, CRM, e-commerce infrastructure, and data warehouse integrations.",
            "faqpro.cat3Label": "Operations",
            "faqpro.cat3Title": "Performance and Support",
            "faqpro.cat3Desc": "Explanations about SLA, support channels, release management, and production operations.",
            "faqpro.q1": "Which industries can use Pika?",
            "faqpro.a1": "It can be used across retail, finance, telecom, e-commerce, and membership-based industries. Its modular architecture scales according to your organization.",
            "faqpro.q2": "How does the demo process work?",
            "faqpro.a2": "After your demo request, our sales engineers perform a needs analysis. Then a scenario set specific to your organization is prepared and a live demo meeting is planned.",
            "faqpro.q3": "Is integration possible with our existing systems?",
            "faqpro.a3": "Yes. Pika provides bi-directional integration via APIs with CRM, ERP, e-commerce infrastructure, call center, and data warehouse solutions.",
            "faqpro.q4": "How are data security and authorization ensured?",
            "faqpro.a4": "Role-based access, transaction logs, encrypted data transfer, audit records, and retention processes aligned with corporate policies are applied together.",
            "faqpro.q5": "Through which channels and how quickly can we reach the support team?",
            "faqpro.a5": "You can submit support requests via the portal, email, and priority line. We target a response within 30 minutes for critical records and within the same business day for standard requests.",
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

        qsa("[data-i18n-html]").forEach(el => {
            const key = el.getAttribute("data-i18n-html");
            const value = i18n[safe][key];
            if (value) el.innerHTML = value;
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
