// wwwroot/js/site.js
(function () {
    const qs = (s, el = document) => el.querySelector(s);
    const qsa = (s, el = document) => [...el.querySelectorAll(s)];

    const setFormLoading = (form, isLoading) => {
        if (!form) return;
        const button = form.querySelector('button[type="submit"], input[type="submit"]');
        if (!button) return;

        if (isLoading) {
            if (!button.dataset.originalContent) {
                button.dataset.originalContent = button.innerHTML;
            }
            button.disabled = true;
            button.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>' + (button.dataset.loadingText || 'Gönderiliyor');
        } else {
            if (button.dataset.originalContent) {
                button.innerHTML = button.dataset.originalContent;
                delete button.dataset.originalContent;
            }
            button.disabled = false;
        }
    };

    const i18n = {
        tr: {
            "topbar.security": "Kurumsal güvenlik • KVKK uyumlu süreçler",
            "topbar.support": "Destek",
            "nav.platform": "Platform",
            "nav.products": "Ürünler",
            "nav.channels": "Kanallar",
            "nav.solutions.ecommerce": "E-Ticaret",
            "nav.solutions.retail": "Perakende",
            "nav.solutions.lifecycle": "Yaşam Döngüsü",
            "nav.solutions.crm": "CRM",
            "nav.integrations": "Entegrasyonlar",
            "nav.security": "Güvenlik",
            "nav.about": "Hakkımızda",
            "nav.blog": "Blog",
            "footer.brandDesc": "Pika; omnichannel kampanya yönetimi, segmentasyon ve AI destekli içerik üretimini tek platformda birleştirir.",
            "nav.home": "Ana Sayfa",
            "nav.solutions": "Çözümlerimiz",
            "nav.solutions.emailMarketing": "E-Posta Pazarlaması",
            "nav.solutions.smsCampaigns": "SMS Kampanyaları",
            "nav.solutions.whatsAppMessaging": "WhatsApp Mesajlaşma",
            "nav.solutions.pushNotifications": "Anlık Bildirimler",
            "nav.solutions.personalization": "Müşteri Etkileşim Yönetimi",
            "nav.solutions.templateManagement": "Müşteri Segmentasyonu ve Hedefleme",
            "nav.solutions.abTesting": "Kampanya Otomasyonu ve Zamanlama",
            "nav.solutions.reporting": "Analitik ve Raporlama",
            "nav.solutions.deliverabilityCompliance": "Teslim Edilebilirlik ve Uyumluluk",
            "nav.solutions.dataManagementEtl": "Veri Yönetimi",
            "nav.solutions.realTimeEventProcessing": "Gerçek Zamanlı Olay İşleme",
            "nav.pricing": "Fiyatlandırma",
            "nav.corporate": "Kurumsal",
            "nav.pika": "Pika",
            "nav.faq": "Sık Sorulan Sorular",
            "nav.termsOfUse": "Kullanım Şartları",
            "nav.privacyPolicy": "Gizlilik Politikası",
            "nav.contact": "İletişim",
            "nav.login": "Giriş",
            "nav.demo": "Demo Talebi",
            "nav.language": "Dil",
            "nav.career": "Kariyer",

            "career.heroTitle": "Pika'da Çalışmak",
            "career.heroDesc": "Teknolojinin geleceğini birlikte inşa edecek, tutkulu ve yenilikçi ekip arkadaşları arıyoruz. Pika ailesi olarak müşterilerimize değer katarken, kendi gelişimimizi de ön planda tutuyoruz.",
            "career.whyEyebrow": "Neden Pika?",
            "career.whyTitle": "Pika'da çalışmak neden farklı?",
            "career.whyDesc": "Sadece bir iş yeri değil, kariyer yolculuğunuzda sizi ileri taşıyacak bir ekosistem sunuyoruz.",
            "career.card1Title": "Hızla Büyüyen Teknoloji",
            "career.card1Desc": "Omnichannel pazarlama alanında öncü bir platformda, en güncel teknolojilerle çalışma fırsatı.",
            "career.card2Title": "Güçlü Ekip Kültürü",
            "career.card2Desc": "Açık iletişim, şeffaf yönetim ve birbirine destek olan, öğrenmeyi seven bir ekip.",
            "career.card3Title": "İnovasyon Odaklı",
            "career.card3Desc": "Yeni fikirler teşvik edilir. AI, otomasyon ve veri odaklı projelerde söz sahibi olursunuz.",
            "career.card4Title": "Sürekli Gelişim",
            "career.card4Desc": "Eğitim bütçesi, konferans katılımı, mentorluk programları ve kariyer planlama desteği.",
            "career.card5Title": "İş-Yaşam Dengesi",
            "career.card5Desc": "Esnek çalışma saatleri, uzaktan çalışma imkânı ve çalışan refahına önem veren politikalar.",
            "career.card6Title": "Global Vizyon",
            "career.card6Desc": "Uluslararası müşteriler, çok dilli platform ve küresel ölçekte etki yaratma fırsatı.",
            "career.valuesEyebrow": "Değerlerimiz",
            "career.valuesTitle": "Birlikte başarmanın gücüne inanıyoruz",
            "career.valuesDesc": "Pika'da her birey değerlidir. Takım ruhu, şeffaflık ve müşteri odaklılık temel değerlerimizin başında gelir.",
            "career.val1": "Şeffaflık",
            "career.val2": "Sorumluluk",
            "career.val3": "Yenilikçilik",
            "career.val4": "Müşteri Odaklılık",
            "career.quote": "\"Pika'da sadece kod yazmıyoruz; müşterilerimizin başarısını birlikte tasarlıyoruz. Her sprint, her fikir ve her satır kod bu misyonun parçası.\"",
            "career.quoteAuthor": "— Pika Ekibi",
            "career.processEyebrow": "Başvuru Süreci",
            "career.processTitle": "Nasıl başvurabilirsiniz?",
            "career.step1Title": "Formu Doldurun",
            "career.step1Desc": "Aşağıdaki başvuru formunu bilgilerinizle doldurun.",
            "career.step2Title": "İnceleme",
            "career.step2Desc": "Ekibimiz başvurunuzu dikkatle değerlendirir.",
            "career.step3Title": "Görüşme",
            "career.step3Desc": "Uygun adaylarla teknik ve kültürel uyum görüşmeleri yaparız.",
            "career.step4Title": "Hoş Geldin!",
            "career.step4Desc": "Teklif ve onboarding sürecimizle ekibimize katılırsınız.",
            "career.formTitle": "Başvuru Formu",
            "career.formDesc": "Aşağıdaki formu doldurarak bize ulaşabilirsiniz. Başvurunuz en kısa sürede değerlendirilecektir.",
            "career.position": "Başvurulan Pozisyon",
            "career.positionPh": "Pozisyon seçiniz",
            "career.cvUrl": "CV / LinkedIn Linki",
            "career.cvFile": "CV Yükle (PDF, maks. 5MB)",
            "career.apply": "Başvuruyu Gönder",

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
            "form.phonePh": "5XX XXX XX XX",
            "form.cityPh": "Şehir",
            "form.messagePh": "Kısaca ihtiyacınızı yazın...",
            "form.privacy": "Bilgileriniz gizli tutulur.",
            "form.close": "Kapat",
            "form.send": "Gönder",

            "home.heroTitle": "Müşterinize doğru anda, doğru kanaldan, doğru mesajı verin",
            "home.heroDesc": "Pika; SMS, WhatsApp, Email ve Push kanallarında kampanya yönetimi, segmentasyon, kişiselleştirme, journey otomasyonu, izin yönetimi ve raporlamayı tek platformda birleştirir. AI destekli içerik ve akış üretimiyle ekiplerin daha hızlı ve daha etkili kampanyalar oluşturmasını sağlar.",
            "home.heroCta2": "Platformu İncele",
            "home.tag1": "AI destekli kampanya üretimi",
            "home.tag2": "Transaction bazlı segmentasyon",
            "home.tag3": "Journey otomasyonu",
            "home.tag4": "IYS ve izin yönetimi",
            "home.tag5": "WhatsApp, SMS, Email, Push",
            "home.trust1Title": "Çok kanallı kampanya yönetimi",
            "home.trust1Desc": "SMS, WhatsApp, Email ve Push tek merkezde",
            "home.trust2Title": "Gerçek zamanlı tetikleyiciler",
            "home.trust2Desc": "Davranış ve işlem bazlı otomasyon akışları",
            "home.trust3Title": "İzin ve uyumluluk yönetimi",
            "home.trust3Desc": "IYS ve iletişim tercihleri kontrol altında",
            "home.trust4Title": "AI destekli operasyon",
            "home.trust4Desc": "Kampanya üretim süresini kısaltan akıllı yardımcı katman",
            "home.problemEyebrow": "Problem & Çözüm",
            "home.problemTitle": "Dağınık kampanya süreçlerini tek merkezde toplayın",
            "home.problemDesc": "Farklı kanallar, parçalı araçlar ve manuel süreçler pazarlama ekiplerini yavaşlatır. Pika, veri, kampanya, otomasyon ve raporlamayı tek çatı altında birleştirir.",
            "home.prob1Title": "Parçalı kanal yönetimi",
            "home.prob1Desc": "Her kanalda ayrı araç kullanmak ekiplerin hızını ve görünürlüğünü düşürür.",
            "home.prob2Title": "Manuel kampanya operasyonu",
            "home.prob2Desc": "İçerik üretimi, segment hazırlama ve zamanlama süreçleri ekipleri yorar.",
            "home.prob3Title": "Veriyi aksiyona çevirememe",
            "home.prob3Desc": "Transaction verisi kullanılamadığında kampanyalar geniş ama etkisiz kalır.",
            "home.sol1Title": "Tek merkezden yönetim",
            "home.sol1Desc": "Tüm ana kampanya kanallarını tek panelde yönetin.",
            "home.sol2Title": "AI ile hızlanan operasyon",
            "home.sol2Desc": "İçerik ve kampanya kurulum süreçlerini hızlandırın.",
            "home.sol3Title": "Veriye dayalı hedefleme",
            "home.sol3Desc": "Müşteri davranışına göre segmentler oluşturun ve journey'ler başlatın.",
            "home.productsEyebrow": "Ürünler",
            "home.productsTitle": "Pika'nın çekirdek ürünleri",
            "home.productsDesc": "Müşteri etkileşimi ve kampanya operasyonu için ihtiyaç duyduğunuz temel yetenekler tek platformda.",
            "home.mod1Desc": "SMS, WhatsApp, Email ve Push kampanyalarını oluşturun, zamanlayın ve yönetin.",
            "home.mod2Desc": "Transaction, davranış ve özel kurallara göre hedef kitleler oluşturun.",
            "home.mod3Desc": "Olay bazlı otomatik akışlar kurgulayın ve süreci manuel yükten kurtarın.",
            "home.mod4Desc": "Sürükle bırak tasarım ve AI destekli içerik üretimi ile kampanya hazırlığını hızlandırın.",
            "home.mod5Desc": "Kanal, kampanya ve segment performansını ölçün.",
            "home.mod6Desc": "İzin yönetimi, abonelik tercihleri ve uyumluluk süreçlerini tek yerden yönetin.",
            "home.channelsEyebrow": "Kanallar",
            "home.channelsTitle": "Tüm ana iletişim kanalları tek merkezde",
            "home.channelsDesc": "Kampanyalarınızı ve otomasyon akışlarınızı farklı araçlara bölmeden yönetin.",
            "home.ch1Desc": "Template yönetimi, kişiselleştirilmiş mesajlar, kampanya gönderimi ve süreç bazlı kullanım.",
            "home.ch2Desc": "Hızlı erişim, kritik bildirimler ve yüksek görünürlük gerektiren iletişimler.",
            "home.ch3Desc": "Görsel olarak zengin, kişiselleştirilmiş ve sürükle bırak tasarlanabilen kampanyalar.",
            "home.ch4Desc": "Gerçek zamanlı olaylara anında tepki verin ve uygulama içi etkileşimi güçlendirin.",
            "home.journeyEyebrow": "Journey Otomasyonu",
            "home.journeyTitle": "Müşteri davranışlarına göre otomatik aksiyon alın",
            "home.journeyDesc": "Pika'nın journey yapısı sayesinde, bir işlem olduğunda sonraki adımı sistem otomatik başlatır.",
            "home.j1": "Sepet hareketi sonrası otomatik hatırlatma",
            "home.j2": "Doğum gününde özel kampanya",
            "home.j3": "Segment değişiminde yeni iletişim akışı",
            "home.j4": "İşlem girişi sonrası tetiklenen kampanya",
            "home.j5": "Özel gün veya davranış bazlı otomatik süreçler",
            "home.journeyQuote": "Doğru anda doğru aksiyonu almak için manuel operasyon değil, kurallı otomasyon kullanın.",
            "home.aiEyebrow": "Yapay Zekâ",
            "home.aiTitle": "Yapay zekâ ile kampanya üretim sürecini hızlandırın",
            "home.aiDesc": "Pika'nın AI destekli yapısı, ekiplerin kampanya metni, içerik taslağı ve kampanya kurulum süreçlerini daha hızlı ve daha verimli yönetmesine yardımcı olur.",
            "home.ai1": "Kampanya metni üretimi",
            "home.ai2": "Özel gün kampanyası oluşturma",
            "home.ai3": "Segmente uygun içerik önerileri",
            "home.ai4": "Email içeriği taslağı hazırlama",
            "home.ai5": "Operasyon süresini kısaltma",
            "home.aiExample": "Özel bir gün için indirim kampanyası oluştur ve bu segmente gönder gibi komutlarla ekip operasyonunu hızlandırın.",
            "home.segEyebrow": "Segmentasyon",
            "home.segTitle": "Transaction verisini hedeflemeye dönüştürün",
            "home.segDesc": "Sadece iletişim listeleriyle değil, gerçek müşteri davranışlarıyla çalışın.",
            "home.seg1": "Mağaza bazlı filtreleme",
            "home.seg2": "Ürün bazlı segmentasyon",
            "home.seg3": "Tekrar satın alma davranışı",
            "home.seg4": "İşlem sıklığı",
            "home.seg5": "Davranış bazlı hedefleme",
            "home.seg6": "Tekrar kullanılabilir segmentler",
            "home.segQuote": "Belirli mağazadan belirli ürünü en az 3 kez alan müşterileri tek adımda segmente edin ve yeniden kullanın.",
            "home.scenariosEyebrow": "Kullanım Senaryoları",
            "home.scenariosTitle": "Pika ile neler yapabilirsiniz?",
            "home.sc1Title": "Özel gün kampanyası",
            "home.sc1Desc": "Belirli segmentlere kişiselleştirilmiş çok kanallı kampanyalar oluşturun.",
            "home.sc2Title": "Doğum günü akışı",
            "home.sc2Desc": "Özel tarihlerde otomatik başlayan teklif akışları kurun.",
            "home.sc3Title": "Sepet hatırlatma",
            "home.sc3Desc": "Davranış bazlı hatırlatma akışları ile geri dönüşü artırın.",
            "home.sc4Title": "Yeniden satın alım kampanyası",
            "home.sc4Desc": "Belirli ürün veya mağaza davranışına göre hedefleme yapın.",
            "home.sc5Title": "Özel müşteri segmentleri",
            "home.sc5Desc": "Satın alma desenine göre yüksek değerli müşteri grupları oluşturun.",
            "home.sc6Title": "Omnichannel journey yönetimi",
            "home.sc6Desc": "Email, WhatsApp, SMS ve Push aksiyonlarını aynı akışta yönetin.",
            "home.complianceEyebrow": "Entegrasyon & Uyum",
            "home.complianceTitle": "Uyumlu, ölçeklenebilir ve entegre çalışmaya hazır",
            "home.complianceDesc": "Pika; veri akışları, olay bazlı tetikleyiciler ve kurumsal süreçlerle uyumlu çalışacak şekilde modüler olarak genişletilebilir.",
            "home.comp1Title": "API entegrasyonlarına uygun yapı",
            "home.comp1Desc": "Mevcut sistemlerinizle kolayca bağlantı kurun.",
            "home.comp2Title": "Gerçek zamanlı olay işleme",
            "home.comp2Desc": "Anlık tetikleyicilerle hızlı aksiyon alın.",
            "home.comp3Title": "Veri yönetimi",
            "home.comp3Desc": "Güvenli ve düzenli veri akışı sağlayın.",
            "home.comp4Title": "İzin ve uyumluluk süreçleri",
            "home.comp4Desc": "KVKK ve IYS gereksinimlerini karşılayın.",
            "home.optionalEyebrow": "Opsiyonel Modüller",
            "home.optionalTitle": "İhtiyacınız büyüdükçe platformunuzu genişletin",
            "home.optionalDesc": "Pika, modüler yapısıyla ileri seviye çözümler ve kurumsal ihtiyaçlara göre genişletilebilir.",
            "home.optBadge": "Opsiyonel",
            "home.opt1Desc": "Telefon üzerinden otomatik sesli etkileşim ve self-service akışları.",
            "home.opt2Desc": "Yazılı ve sesli etkileşim senaryoları için genişletilebilir çözüm yapısı.",
            "home.opt3Desc": "Özel kurumsal sistemler ve veri akışlarıyla genişletilebilir mimari.",
            "home.ctaTitle": "Kampanya operasyonunuzu tek merkezden yönetin",
            "home.ctaDesc": "Pika ile müşteri verisini aksiyona dönüştürün, kampanya üretimini hızlandırın ve çok kanallı iletişimi daha akıllı hale getirin.",

            "sol.h1": "Çözümlerimiz",
            "sol.chip": "Kurumsal Mesajlaşma ve Veri Platformu",
            "sol.lead": "Kanal orkestrasyonu, kişiselleştirme ve raporlamayı tek mimaride yönetin.",
            "sol.kpi.1.title": "12 Modül",
            "sol.kpi.1.desc": "Yeni bilgi mimarisi",
            "sol.kpi.2.title": "Omnichannel",
            "sol.kpi.2.desc": "Tüm temas noktaları",
            "sol.kpi.3.title": "Data-Driven",
            "sol.kpi.3.desc": "Gerçek zamanlı karar",
            "sol.reporting": "Raporlama",
            "sol.demo": "Demo Talebi",
            "sol.map": "Çözüm Haritası",
            "sol.page.highlights": "Öne Çıkanlar",
            "sol.page.next": "Sonraki",
            "sol.ctat": "Size özel akış",
            "sol.ctad": "İş modelinize göre en doğru kurgu ve KPI setini birlikte belirleyelim.",
            "sol.ctab": "Demo Talebi",

            "sol.page.EmailMarketing.chip": "Email Marketing",
            "sol.page.EmailMarketing.title": "Email Marketing",
            "sol.page.EmailMarketing.lead": "Email kanalında yaşam döngüsü odaklı otomasyon ve içerik orkestrasyonu sağlayın.",
            "sol.page.EmailMarketing.h1": "Lifecycle otomasyonları",
            "sol.page.EmailMarketing.h2": "Dinamik segment gönderimleri",
            "sol.page.EmailMarketing.h3": "Teslimat ve tıklama analizi",
            "sol.page.SmsCampaigns.chip": "SMS Campaigns",
            "sol.page.SmsCampaigns.title": "SMS Campaigns",
            "sol.page.SmsCampaigns.lead": "Kritik mesajları doğru zamanlama ve hedeflemeyle SMS kanalında yönetin.",
            "sol.page.SmsCampaigns.h1": "Tetik bazlı SMS",
            "sol.page.SmsCampaigns.h2": "Operatör bazlı performans",
            "sol.page.SmsCampaigns.h3": "Kota ve maliyet kontrolü",
            "sol.page.WhatsAppMessaging.chip": "WhatsApp Messaging",
            "sol.page.WhatsAppMessaging.title": "WhatsApp Messaging",
            "sol.page.WhatsAppMessaging.lead": "Onaylı şablonlar ve iki yönlü konuşma akışlarıyla WhatsApp iletişimini ölçekleyin.",
            "sol.page.WhatsAppMessaging.h1": "Şablon onay yönetimi",
            "sol.page.WhatsAppMessaging.h2": "Konuşma bazlı raporlama",
            "sol.page.WhatsAppMessaging.h3": "Bot ve ajan handoff",
            "sol.page.PushNotifications.chip": "Push Notifications",
            "sol.page.PushNotifications.title": "Push Notifications",
            "sol.page.PushNotifications.lead": "Web ve mobil kanalda davranış odaklı push kurguları oluşturun.",
            "sol.page.PushNotifications.h1": "Anlık tetikleme",
            "sol.page.PushNotifications.h2": "Sessiz saat politikaları",
            "sol.page.PushNotifications.h3": "Opt-in yaşam döngüsü",
            "sol.page.Personalization.chip": "Personalization",
            "sol.page.Personalization.title": "Personalization",
            "sol.page.Personalization.lead": "Müşteri davranışı ve özniteliklerine göre gerçek zamanlı içerik kişiselleştirin.",
            "sol.page.Personalization.h1": "Dinamik öneri",
            "sol.page.Personalization.h2": "Kural + ML hibrit yaklaşım",
            "sol.page.Personalization.h3": "Kanal bazlı varyasyon",
            "sol.page.TemplateManagement.chip": "Template Management",
            "sol.page.TemplateManagement.title": "Template Management",
            "sol.page.TemplateManagement.lead": "Tüm iletişim şablonlarını sürümleyin, onaylayın ve kanal bazında yönetin.",
            "sol.page.TemplateManagement.h1": "Sürüm kontrolü",
            "sol.page.TemplateManagement.h2": "İçerik bileşen kütüphanesi",
            "sol.page.TemplateManagement.h3": "Marka uyum denetimi",
            "sol.page.ABTesting.chip": "A/B Testing",
            "sol.page.ABTesting.title": "A/B Testing",
            "sol.page.ABTesting.lead": "Mesaj, başlık, zamanlama ve kanal deneyleriyle kazanımı ölçün.",
            "sol.page.ABTesting.h1": "İstatistiksel anlamlılık",
            "sol.page.ABTesting.h2": "Holdout grupları",
            "sol.page.ABTesting.h3": "Otomatik kazanan seçimi",
            "sol.page.Reporting.chip": "Reporting",
            "sol.page.Reporting.title": "Reporting",
            "sol.page.Reporting.lead": "KPI, dönüşüm ve gelir katkısı raporlarını tek merkezde toplayın.",
            "sol.page.Reporting.h1": "Canlı dashboard",
            "sol.page.Reporting.h2": "Kohort ve funnel analizi",
            "sol.page.Reporting.h3": "Dışa aktarım ve API",
            "sol.page.DeliverabilityCompliance.chip": "Deliverability & Compliance",
            "sol.page.DeliverabilityCompliance.title": "Deliverability & Compliance",
            "sol.page.DeliverabilityCompliance.lead": "Teslimat kalitesi ve regülasyon uyumunu operasyonel olarak yönetin.",
            "sol.page.DeliverabilityCompliance.h1": "İzin/ret yönetimi",
            "sol.page.DeliverabilityCompliance.h2": "Gönderici itibarı izleme",
            "sol.page.DeliverabilityCompliance.h3": "Uyum denetim kayıtları",
            "sol.page.DataManagementEtl.chip": "Data Management ETL",
            "sol.page.DataManagementEtl.title": "Data Management ETL",
            "sol.page.DataManagementEtl.lead": "Farklı veri kaynaklarını birleştirip pazarlama aktivasyonu için standardize edin.",
            "sol.page.DataManagementEtl.h1": "Kaynak bağlayıcıları",
            "sol.page.DataManagementEtl.h2": "Dönüşüm ve zenginleştirme",
            "sol.page.DataManagementEtl.h3": "Veri kalite kontrolleri",
            "sol.page.RealTimeEventProcessing.chip": "Real-Time Event Processing",
            "sol.page.RealTimeEventProcessing.title": "Real-Time Event Processing",
            "sol.page.RealTimeEventProcessing.lead": "Müşteri olaylarını milisaniyeler içinde işleyip aksiyona dönüştürün.",
            "sol.page.RealTimeEventProcessing.h1": "Event stream işleme",
            "sol.page.RealTimeEventProcessing.h2": "Anlık tetik aksiyonları",
            "sol.page.RealTimeEventProcessing.h3": "Ölçeklenebilir olay mimarisi",
            "solutions.overview.title": "Çözümlerimiz",
            "solutions.next": "Sonraki",
            "solutions.sections.capabilities": "Yetenekler",
            "solutions.sections.workflow": "İş Akışı",
            "solutions.sections.integrations": "Entegrasyonlar",
            "solutions.sections.kpi": "KPI",
            "solutions.sections.compliance": "Uyumluluk",
            "solutions.sections.scenarios": "Senaryolar",
            "solutions.emailMarketing.title": "Email Marketing",
            "solutions.emailMarketing.subtitle": "Email kanalında yaşam döngüsü odaklı otomasyon ve içerik orkestrasyonu sağlayın.",
            "solutions.emailMarketing.summary": "Kampanya planlama, gönderim ve optimizasyon adımlarını tek akışta yönetin.",
            "solutions.emailMarketing.corp": "Kurumsal Email Marketing",
            "solutions.emailMarketing.capabilities.1": "Lifecycle otomasyonları",
            "solutions.emailMarketing.capabilities.2": "Dinamik segment gönderimleri",
            "solutions.emailMarketing.capabilities.3": "Teslimat ve tıklama analizi",
            "solutions.emailMarketing.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.emailMarketing.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.emailMarketing.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.emailMarketing.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.emailMarketing.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.emailMarketing.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.emailMarketing.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.emailMarketing.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.emailMarketing.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.smsCampaigns.title": "SMS Campaigns",
            "solutions.smsCampaigns.subtitle": "Kritik mesajları doğru zamanlama ve hedeflemeyle SMS kanalında yönetin.",
            "solutions.smsCampaigns.summary": "Yüksek ulaşılırlık gereken mesajları tek panelden yönetin.",
            "solutions.smsCampaigns.corp": "Kurumsal SMS Campaigns",
            "solutions.smsCampaigns.capabilities.1": "Tetik bazlı SMS",
            "solutions.smsCampaigns.capabilities.2": "Operatör bazlı performans",
            "solutions.smsCampaigns.capabilities.3": "Kota ve maliyet kontrolü",
            "solutions.smsCampaigns.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.smsCampaigns.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.smsCampaigns.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.smsCampaigns.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.smsCampaigns.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.smsCampaigns.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.smsCampaigns.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.smsCampaigns.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.smsCampaigns.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.whatsAppMessaging.title": "WhatsApp Messaging",
            "solutions.whatsAppMessaging.subtitle": "Onaylı şablonlar ve iki yönlü konuşma akışlarıyla WhatsApp iletişimini ölçekleyin.",
            "solutions.whatsAppMessaging.summary": "Müşteri destek ve pazarlama konuşmalarını güvenli şekilde orkestre edin.",
            "solutions.whatsAppMessaging.corp": "Kurumsal WhatsApp Messaging",
            "solutions.whatsAppMessaging.capabilities.1": "Şablon onay yönetimi",
            "solutions.whatsAppMessaging.capabilities.2": "Konuşma bazlı raporlama",
            "solutions.whatsAppMessaging.capabilities.3": "Bot ve ajan handoff",
            "solutions.whatsAppMessaging.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.whatsAppMessaging.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.whatsAppMessaging.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.whatsAppMessaging.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.whatsAppMessaging.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.whatsAppMessaging.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.whatsAppMessaging.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.whatsAppMessaging.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.whatsAppMessaging.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.pushNotifications.title": "Push Notifications",
            "solutions.pushNotifications.subtitle": "Web ve mobil kanalda davranış odaklı push kurguları oluşturun.",
            "solutions.pushNotifications.summary": "Gerçek zamanlı olaylarla kullanıcıyı doğru anda doğru ekrana yönlendirin.",
            "solutions.pushNotifications.corp": "Kurumsal Push Notifications",
            "solutions.pushNotifications.capabilities.1": "Anlık tetikleme",
            "solutions.pushNotifications.capabilities.2": "Sessiz saat politikaları",
            "solutions.pushNotifications.capabilities.3": "Opt-in yaşam döngüsü",
            "solutions.pushNotifications.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.pushNotifications.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.pushNotifications.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.pushNotifications.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.pushNotifications.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.pushNotifications.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.pushNotifications.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.pushNotifications.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.pushNotifications.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.personalization.title": "Personalization",
            "solutions.personalization.subtitle": "Müşteri davranışı ve özniteliklerine göre gerçek zamanlı içerik kişiselleştirin.",
            "solutions.personalization.summary": "Teklif, içerik ve kanal seçimlerini müşteri bağlamına göre dinamikleştirin.",
            "solutions.personalization.corp": "Kurumsal Personalization",
            "solutions.personalization.capabilities.1": "Dinamik öneri",
            "solutions.personalization.capabilities.2": "Kural + ML hibrit yaklaşım",
            "solutions.personalization.capabilities.3": "Kanal bazlı varyasyon",
            "solutions.personalization.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.personalization.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.personalization.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.personalization.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.personalization.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.personalization.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.personalization.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.personalization.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.personalization.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.templateManagement.title": "Template Management",
            "solutions.templateManagement.subtitle": "Tüm iletişim şablonlarını sürümleyin, onaylayın ve kanal bazında yönetin.",
            "solutions.templateManagement.summary": "Marka bütünlüğünü koruyan merkezi şablon operasyonu kurun.",
            "solutions.templateManagement.corp": "Kurumsal Template Management",
            "solutions.templateManagement.capabilities.1": "Sürüm kontrolü",
            "solutions.templateManagement.capabilities.2": "İçerik bileşen kütüphanesi",
            "solutions.templateManagement.capabilities.3": "Marka uyum denetimi",
            "solutions.templateManagement.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.templateManagement.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.templateManagement.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.templateManagement.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.templateManagement.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.templateManagement.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.templateManagement.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.templateManagement.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.templateManagement.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.abTesting.title": "A/B Testing",
            "solutions.abTesting.subtitle": "Mesaj, başlık, zamanlama ve kanal deneyleriyle kazanımı ölçün.",
            "solutions.abTesting.summary": "Kontrollü deneylerle karar alma hızını artırın ve riskleri azaltın.",
            "solutions.abTesting.corp": "Kurumsal A/B Testing",
            "solutions.abTesting.capabilities.1": "İstatistiksel anlamlılık",
            "solutions.abTesting.capabilities.2": "Holdout grupları",
            "solutions.abTesting.capabilities.3": "Otomatik kazanan seçimi",
            "solutions.abTesting.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.abTesting.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.abTesting.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.abTesting.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.abTesting.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.abTesting.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.abTesting.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.abTesting.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.abTesting.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.reporting.title": "Reporting",
            "solutions.reporting.subtitle": "KPI, dönüşüm ve gelir katkısı raporlarını tek merkezde toplayın.",
            "solutions.reporting.summary": "Karar vericiler için gerçek zamanlı görünürlük oluşturun.",
            "solutions.reporting.corp": "Kurumsal Reporting",
            "solutions.reporting.capabilities.1": "Canlı dashboard",
            "solutions.reporting.capabilities.2": "Kohort ve funnel analizi",
            "solutions.reporting.capabilities.3": "Dışa aktarım ve API",
            "solutions.reporting.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.reporting.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.reporting.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.reporting.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.reporting.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.reporting.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.reporting.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.reporting.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.reporting.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.deliverabilityCompliance.title": "Deliverability & Compliance",
            "solutions.deliverabilityCompliance.subtitle": "Teslimat kalitesi ve regülasyon uyumunu operasyonel olarak yönetin.",
            "solutions.deliverabilityCompliance.summary": "İtibar yönetimi ve uyumluluk kanıtlarını tek standartta toplayın.",
            "solutions.deliverabilityCompliance.corp": "Kurumsal Deliverability & Compliance",
            "solutions.deliverabilityCompliance.capabilities.1": "İzin/ret yönetimi",
            "solutions.deliverabilityCompliance.capabilities.2": "Gönderici itibarı izleme",
            "solutions.deliverabilityCompliance.capabilities.3": "Uyum denetim kayıtları",
            "solutions.deliverabilityCompliance.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.deliverabilityCompliance.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.deliverabilityCompliance.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.deliverabilityCompliance.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.deliverabilityCompliance.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.deliverabilityCompliance.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.deliverabilityCompliance.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.deliverabilityCompliance.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.deliverabilityCompliance.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.dataManagementEtl.title": "Data Management ETL",
            "solutions.dataManagementEtl.subtitle": "Farklı veri kaynaklarını birleştirip pazarlama aktivasyonu için standardize edin.",
            "solutions.dataManagementEtl.summary": "Veri kalite, dönüşüm ve zenginleştirme adımlarını otomatikleştirin.",
            "solutions.dataManagementEtl.corp": "Kurumsal Data Management ETL",
            "solutions.dataManagementEtl.capabilities.1": "Kaynak bağlayıcıları",
            "solutions.dataManagementEtl.capabilities.2": "Dönüşüm ve zenginleştirme",
            "solutions.dataManagementEtl.capabilities.3": "Veri kalite kontrolleri",
            "solutions.dataManagementEtl.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.dataManagementEtl.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.dataManagementEtl.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.dataManagementEtl.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.dataManagementEtl.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.dataManagementEtl.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.dataManagementEtl.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.dataManagementEtl.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.dataManagementEtl.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
            "solutions.realTimeEventProcessing.title": "Real-Time Event Processing",
            "solutions.realTimeEventProcessing.subtitle": "Müşteri olaylarını milisaniyeler içinde işleyip aksiyona dönüştürün.",
            "solutions.realTimeEventProcessing.summary": "Anlık sinyalleri ölçeklenebilir kurallarla işleyerek temas kalitesini yükseltin.",
            "solutions.realTimeEventProcessing.corp": "Kurumsal Real-Time Event Processing",
            "solutions.realTimeEventProcessing.capabilities.1": "Event stream işleme",
            "solutions.realTimeEventProcessing.capabilities.2": "Anlık tetik aksiyonları",
            "solutions.realTimeEventProcessing.capabilities.3": "Ölçeklenebilir olay mimarisi",
            "solutions.realTimeEventProcessing.workflow.step1": "İş hedefleri ve segment kapsamını tanımlayın.",
            "solutions.realTimeEventProcessing.workflow.step2": "Kanal, içerik ve kuralları devreye alın.",
            "solutions.realTimeEventProcessing.workflow.step3": "Sonuçları izleyip akışı sürekli optimize edin.",
            "solutions.realTimeEventProcessing.integrations": "CRM, e-ticaret ve analitik sistemleriyle entegre çalışır.",
            "solutions.realTimeEventProcessing.kpi": "Operasyon hızı, dönüşüm ve gelir katkısı KPI setinde ölçülür.",
            "solutions.realTimeEventProcessing.compliance": "Rol bazlı erişim, loglama ve izin yönetimi ile uyum sağlanır.",
            "solutions.realTimeEventProcessing.scenarios": "Kanal tetikleme, yeniden kazanım ve kişiselleştirme senaryolarını destekler.",
            "solutions.realTimeEventProcessing.repoUnspecified1": "Not: Repo’da belirtilmemiş entegrasyon ayrıntıları proje keşif aşamasında netleştirilmelidir.",
            "solutions.realTimeEventProcessing.repoUnspecified2": "Not: Repo’da belirtilmemiş sektör/regülasyon detayları canlıya geçiş öncesi tanımlanmalıdır.",
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

            "terms.chip": "Kurumsal",
            "terms.title": "Kullanım Şartları",
            "terms.lead": "ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK'a hoş geldiniz. Bu Hizmet Şartları (\"Şartlar\"), web sitemize, platformlarımıza ve tarafımızca sağlanan tüm hizmetlere (toplu olarak \"Hizmetler\") erişiminizi ve kullanımınızı düzenler. Hizmetlere erişerek veya kullanarak bu Şartlara bağlı kalmayı kabul etmiş olursunuz. Kabul etmiyorsanız, Hizmetleri derhal kullanmayı bırakmalısınız.",
            "terms.intro1": "Bu Şartlar, sizin (\"Kullanıcı\") ile ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK (\"Şirket\") arasında yasal olarak bağlayıcı bir sözleşme oluşturur.",
            "terms.intro2": "",
            "terms.s1Title": "1. Şartların Kabulü",
            "terms.s1Text": "Hizmetlere erişerek veya kullanarak aşağıdakileri temsil ve taahhüt edersiniz:",
            "terms.s1Li1": "Yetki alanınızda reşit yaşta olmanız ve bu sözleşmeye girmeye hukuken ehliyetli olmanız.",
            "terms.s1Li2": "Hizmetlere bir tüzel kişi adına erişiyorsanız, o tüzel kişiyi bu Şartlara bağlamaya yetkiniz olması.",
            "terms.s2Title": "2. Şartlarda Değişiklikler",
            "terms.s2Text": "Bu Şartları herhangi bir zamanda değiştirme veya güncelleme hakkını saklı tutarız. Değişiklikler web sitemize yayınlandığında veya size başka şekilde bildirildiğinde yürürlüğe girer. Bu tür değişikliklerden sonra Hizmetleri kullanmaya devam etmeniz güncellenmiş Şartların kabulü anlamına gelir.",
            "terms.s3Title": "3. Hizmetlerin Kullanımı",
            "terms.s3Text": "Hizmetleri geçerli tüm yasalar, düzenlemeler ve bu Şartlara uygun olarak kullanmayı kabul edersiniz. Aşağıdakiler yasaktır:",
            "terms.s3Li1": "Hizmetlerin düzgün işleyişini bozan veya engelleyen herhangi bir faaliyette bulunmak.",
            "terms.s3Li2": "Hizmetleri yasadışı, zararlı veya dolandırıcılık amacıyla kullanmak.",
            "terms.s3Li3": "Sistemlerimizin veya verilerimizin yetkisiz bölümlerine erişmeye veya müdahale etmeye çalışmak.",
            "terms.s3Li4": "Zararlı yazılım, virüs veya benzeri tehditleri Hizmetler aracılığıyla dağıtmak.",
            "terms.s3Text2": "Tek takdirimize bağlı olarak bu hükmü ihlal eden herkese karşı soruşturma başlatma ve uygun yasal işlem yapma hakkımızı saklı tutarız.",
            "terms.s4Title": "4. Hesap Sorumluluğu",
            "terms.s4Text": "Hizmetlerin belirli özelliklerini kullanmak için hesap oluşturmanız gerekiyorsa, aşağıdakileri kabul edersiniz:",
            "terms.s4Li1": "Kayıt sürecinde doğru, güncel ve eksiksiz bilgi sağlamak.",
            "terms.s4Li2": "Hesap kimlik bilgilerinizin gizliliğini korumak ve hesabınız altında gerçekleşen tüm faaliyetlerden yalnızca sizin sorumlu olmanız.",
            "terms.s4Li3": "Hesabınıza yetkisiz erişim veya kullanım durumunda derhal bizi bilgilendirmek.",
            "terms.s4Text2": "Şirket, bu yükümlülüklere uymamanızdan kaynaklanan herhangi bir kayıp veya zarardan sorumlu olmayacaktır.",
            "terms.s5Title": "5. Fikri Mülkiyet",
            "terms.s5Text1": "Hizmetler aracılığıyla sunulan tüm içerik, materyaller ve fikri mülkiyet, metin, grafikler, logolar, tasarımlar, yazılım ve ticari markalar dahil ancak bunlarla sınırlı olmamak üzere ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK veya lisans verenlerinin mülkiyetindedir.",
            "terms.s5Text2": "Size, Hizmetlere yalnızca kişisel veya yetkili iş amaçları için erişim ve kullanım için sınırlı, münhasır olmayan, devredilemez ve geri alınabilir bir lisans verilmiştir. İçeriğimizin yetkisiz kullanımı, çoğaltılması veya dağıtımı kesinlikle yasaktır ve yasal işlemlere yol açabilir.",
            "terms.s6Title": "6. Gizlilik ve Veri Koruma",
            "terms.s6Text1": "Hizmetleri kullanımınız Gizlilik Politikamıza tabidir; kişisel bilgilerinizi nasıl topladığımızı, kullandığımızı ve koruduğumuzu açıklar. Hizmetleri kullanarak, bilgilerinizin Gizlilik Politikasında açıklandığı şekilde toplanması ve kullanılmasını kabul etmiş olursunuz.",
            "terms.s6Text2Prefix": "Daha fazla bilgi için lütfen ",
            "terms.s6Link": "Gizlilik Politikası",
            "terms.s6Text2Suffix": " sayfamıza bakınız.",
            "terms.s7Title": "7. Sorumluluk Sınırlaması",
            "terms.s7Text1": "Yürürlükteki yasaların izin verdiği en geniş ölçüde, ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK, Hizmetleri kullanımınızdan doğan veya bununla bağlantılı dolaylı, arızi, özel veya sonuç olarak ortaya çıkan zararlardan sorumlu tutulamaz.",
            "terms.s7Text2": "Şirket Hizmetleri \"olduğu gibi\" ve \"mevcut olduğu şekilde\" sunar ve ticari uygunluk, belirli bir amaç için uygunluk ve ihlal etmeme garantileri dahil ancak bunlarla sınırlı olmamak üzere açık veya zımni tüm garantileri reddeder.",
            "terms.s8Title": "8. Tazminat",
            "terms.s8Text": "ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK'ı, bağlı ortaklarını ve ilgili yöneticilerini, memurlarını, çalışanlarını ve temsilcilerini aşağıdakilerden doğan herhangi bir iddia, zarar, kayıp veya yükümlülükten korumak, savunmak ve tazmin etmeyi kabul edersiniz:",
            "terms.s8Li1": "Hizmetleri kullanımınız.",
            "terms.s8Li2": "Bu Şartların ihlali.",
            "terms.s8Li3": "Üçüncü bir tarafın fikri mülkiyet veya haklarının ihlali.",
            "terms.s9Title": "9. Erişimin Askıya Alınması veya Sonlandırılması",
            "terms.s9Text": "Tek takdirimize bağlı olarak bu Şartları ihlal ettiğinizi veya Hizmetlerin bütünlüğü için risk teşkil ettiğinizi belirlememiz halinde, sebep göstermeksizin veya önceden bildirimde bulunmaksızın Hizmetlere erişiminizi herhangi bir zamanda askıya alma veya sonlandırma hakkını saklı tutarız.",
            "terms.s10Title": "10. Uygulanacak Hukuk ve Yetki",
            "terms.s10Text": "Bu Şartlar Türkiye Cumhuriyeti yasalarına tabidir ve çatışma hukuku ilkeleri dikkate alınmaksızın bu yasalara göre yorumlanır. Bu Şartlar kapsamında veya bunlarla bağlantılı ortaya çıkan anlaşmazlıklar, Ankara, Türkiye'deki mahkemelerin münhasır yargı yetkisine tabidir.",
            "terms.s11Title": "11. Ayrılabilirlik",
            "terms.s11Text": "Bu Şartların herhangi bir hükmü yetkili bir mahkeme tarafından geçersiz veya uygulanamaz bulunursa, kalan hükümler tam olarak yürürlükte kalmaya devam eder.",
            "terms.s12Title": "12. Bütün Sözleşme",
            "terms.s12Text": "Bu Şartlar, Gizlilik Politikamızla birlikte, sizin ile ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK arasında Hizmetleri kullanımınıza ilişkin tüm sözleşmeyi oluşturur ve önceki sözleşmeleri veya anlayışları geçersiz kılar.",
            "terms.s13Title": "13. İletişim Bilgileri",
            "terms.s13Text": "Bu Şartlarla ilgili herhangi bir sorunuz veya endişeniz varsa lütfen bizimle iletişime geçin:",
            "terms.contactEmail": "E-posta:",
            "terms.contactAddress": "Adres:",
            "terms.contactAddressValue": "Ostim Osb Mah. 100. Yıl Bulvarı, 55/E Kat:4, Teknopark Turkuaz Bina, 06374 Yenimahalle/Ankara",
            "terms.closing": "ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK tarafından sağlanan Hizmetleri kullanarak, bu Şartları okuduğunuzu, anladığınızı ve kabul ettiğinizi beyan etmiş olursunuz.",
            "terms.effectiveDate": "Yürürlük Tarihi:",
            "terms.effectiveDateValue": "20 Ağustos 2020",

            "privacy.chip": "Kurumsal",
            "privacy.title": "Gizlilik Politikası",
            "privacy.lead": "ARCA uygulamalarını ücretsiz ve ücretli olarak geliştirmektedir. Bu HİZMET ARCA tarafından ücretsiz sunulmakta olup, olduğu gibi kullanılmak üzere tasarlanmıştır.",
            "privacy.intro1": "Bu sayfa, hizmetimizi kullanmaya karar veren herkes için Kişisel Bilgilerin toplanması, kullanılması ve açıklanması ile ilgili politikalarımız hakkında ziyaretçileri bilgilendirmek için kullanılır.",
            "privacy.intro2": "Hizmetimizi kullanmayı seçerseniz, bu politika kapsamındaki bilgilerin toplanması ve kullanılması konusunda kabul etmiş olursunuz. Topladığımız Kişisel Bilgiler, hizmet sunmak ve iyileştirmek için kullanılmaktadır. Bilgilerinizi bu Gizlilik Politikası'nda açıklandığı şekilde hariç, hiçkimseyle kullanmayacağız veya paylaşmayacağız.",
            "privacy.intro3": "Bu Gizlilik Politikası'nda kullanılan terimler, bu Gizlilik Politikası'nda aksi tanımlanmadıkça, Kullanım Şartları ile aynı anlama sahiptir.",
            "privacy.s1Title": "1. Bilgi Toplama ve Kullanım",
            "privacy.s1Text": "Daha iyi bir deneyim için, hizmetimizi kullanırken sizden belirli kişisel olarak tanımlanabilir bilgiler sağlamanız gerekebilir. Talep ettiğimiz bilgiler cihazınızda saklanacak ve bizim tarafımızdan herhangi bir şekilde toplanmayacaktır. Uygulama, sizi tanımlamak için kullanılan bilgileri toplayabilecek üçüncü taraf hizmetleri kullanmaktadır.",
            "privacy.s2Title": "2. Log Verisi",
            "privacy.s2Text": "Hizmetimizi her kullandığınızda, uygulamada bir hata olması durumunda telefonunuzda Log Verisi adı verilen veri ve bilgi topladığımızı (üçüncü taraf ürünleri aracılığıyla) bilmenizi isteriz. Bu Log Verisi, cihazınızın İnternet Protokolü (IP) adresi, cihaz adı, işletim sistemi sürümü, hizmetimizi kullanırken uygulamanın yapılandırması, hizmeti kullandığınız zaman ve tarih ile diğer istatistikler gibi bilgileri içerebilir.",
            "privacy.s3Title": "3. Çerezler",
            "privacy.s3Text1": "Çerezler, genellikle anonim benzersiz tanımlayıcılar olarak kullanılan az miktarda veri içeren dosyalardır. Bunlar ziyaret ettiğiniz web sitelerinden tarayıcınıza gönderilir ve cihazınızın dahili belleğinde saklanır.",
            "privacy.s3Text2": "Bu Hizmet bu çerezleri açıkça kullanmaz. Ancak uygulama, bilgi toplamak ve hizmetlerini iyileştirmek için çerez kullanan üçüncü taraf kod ve kütüphaneler kullanabilir. Bu çerezleri kabul etme veya reddetme ve bir çerezin cihazınıza ne zaman gönderildiğini bilme seçeneğiniz vardır. Çerezlerimizi reddetmeyi seçerseniz, bu Hizmetin bazı bölümlerini kullanamayabilirsiniz.",
            "privacy.s4Title": "4. Hizmet Sağlayıcılar",
            "privacy.s4Text1": "Aşağıdaki nedenlerden dolayı üçüncü taraf şirketleri ve bireyleri istihdam edebiliriz:",
            "privacy.s4Li1": "Hizmetimizi kolaylaştırmak için;",
            "privacy.s4Li2": "Bizim adımıza hizmet sunmak için;",
            "privacy.s4Li3": "Hizmetle ilgili hizmetleri gerçekleştirmek için; veya",
            "privacy.s4Li4": "Hizmetimizin nasıl kullanıldığını analiz etmemize yardımcı olmak için.",
            "privacy.s4Text2": "Bu Hizmetin kullanıcılarına, bu üçüncü tarafların Kişisel Bilgilerinize erişimi olduğunu bilmenizi isteriz. Nedeni, bizim adımıza kendilerine atanan görevleri yerine getirmektir. Ancak bilgileri başka herhangi bir amaç için ifşa etmemek veya kullanmamakla yükümlüdürler.",
            "privacy.s5Title": "5. Güvenlik",
            "privacy.s5Text": "Kişisel Bilgilerinizi bize sağlama konusundaki güveninize değer veriyoruz; bu nedenle bunu ticari olarak kabul edilebilir araçlarla korumaya çalışıyoruz. Ancak unutmayınız ki İnternet üzerinden iletilen veya elektronik depolama yönteminin hiçbiri %100 güvenli ve güvenilir değildir ve mutlak güvenliğini garanti edemeyiz.",
            "privacy.s6Title": "6. Diğer Sitelere Bağlantılar",
            "privacy.s6Text": "Bu Hizmet diğer sitelere bağlantılar içerebilir. Üçüncü taraf bir bağlantıya tıklarsanız, o siteye yönlendirileceksiniz. Bu harici sitelerin bizim tarafımızdan işletilmediğini unutmayınız. Bu nedenle, bu web sitelerinin Gizlilik Politikasını incelemenizi şiddetle tavsiye ederiz. Üçüncü taraf sitelerin veya hizmetlerin içeriği, gizlilik politikaları veya uygulamaları üzerinde hiçbir kontrolümüz yoktur ve sorumluluk kabul etmeyiz.",
            "privacy.s7Title": "7. Çocukların Gizliliği",
            "privacy.s7Text1": "Bu Hizmetler 13 yaşın altındaki herkese hitap etmemektedir. 13 yaşın altındaki çocuklardan bilerek kişisel olarak tanımlanabilir bilgi toplamıyoruz. 13 yaşın altındaki bir çocuğun bize kişisel bilgi sağladığını keşfettiğimiz durumda, bunu sunucularımızdan hemen sileriz.",
            "privacy.s7Text2": "Bir ebeveyn veya vasiyseniz ve çocuğunuzun bize kişisel bilgi sağladığını biliyorsanız, gerekli işlemleri yapabilmemiz için lütfen bizimle iletişime geçiniz.",
            "privacy.s8Title": "8. Bu Gizlilik Politikasındaki Değişiklikler",
            "privacy.s8Text": "Gizlilik Politikamızı zaman zaman güncelleyebiliriz. Bu nedenle, herhangi bir değişiklik için bu sayfayı periyodik olarak incelemeniz önerilir. Yeni Gizlilik Politikasını bu sayfada yayınlayarak değişikliklerden sizi haberdar edeceğiz. Bu değişiklikler bu sayfada yayınlandıktan hemen sonra yürürlüğe girer.",
            "privacy.s9Title": "9. Google Kullanıcı Verileri",
            "privacy.s9Q1": "Eriştiğimiz Google kullanıcı verileri:",
            "privacy.s9A1": "Uygulamamız, yalnızca OAuth yetkilendirme süreci aracılığıyla açık izin verdiğinizde Google Kişiler verilerinize erişir. Özellikle uygulama, Google Kişiler listenizdeki kişi bilgilerini okuyabilir. Başka hiçbir Google kullanıcı verisine (Gmail, Drive veya Takvim gibi) erişilmez.",
            "privacy.s9Q2": "Google kullanıcı verilerinizi nasıl kullanıyoruz:",
            "privacy.s9A2": "Eriştiğimiz Google Kişiler verileri, yalnızca kullanıcı tarafından talep edildiği şekilde kişi listesi ve iletişim bilgileri ile ilgili işlevsellik sağlamak için kullanılmaktadır. Bu veriler reklamcılık, analitik, profil oluşturma veya herhangi bir ilgisiz amaç için kullanılmaz.",
            "privacy.s9Q3": "Google kullanıcı verilerinizi kimlerle paylaşıyoruz:",
            "privacy.s9A3": "Herhangi bir Google kullanıcı verisini üçüncü taraflarla paylaşmıyor, satmıyor veya ifşa etmiyoruz. Veriler yalnızca yasa veya geçerli bir yasal süreç gerektiriyorsa ifşa edilebilir.",
            "privacy.s9Q4": "Veri koruma mekanizmaları:",
            "privacy.s9A4": "Google API'leri ile tüm iletişim endüstri standardı şifrelemeyle (HTTPS/SSL) güvence altına alınmaktadır. Erişim token'ları ve kullanıcı kimlik bilgileri güvenli bir şekilde saklanır ve yetkisiz taraflara asla açığa çıkmaz. Kullanıcılar, Google Hesap İzinleri sayfası aracılığıyla uygulamanın Google verilerine erişimini istedikleri zaman iptal edebilir.",
            "privacy.s9Q5": "Veri saklama:",
            "privacy.s9A5": "Google kullanıcı verileri yalnızca talep edilen işlevselliği sağlamak için gerekli olduğu sürece saklanır. Bir kullanıcı erişimi iptal ettiğinde veya hesabını sildiğinde, ilişkili tüm veriler ve token'lar güvenli bir şekilde silinir.",
            "privacy.s10Title": "10. Bize Ulaşın",
            "privacy.s10Text": "Gizlilik Politikamız hakkında herhangi bir soru veya öneriniz varsa, bizimle iletişime geçmekten çekinmeyiniz: ",
            "privacy.lastUpdate": "Son güncelleme:",

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
            "contact.chip": "Kurumsal İletişim",
            "contact.kpi1Label": "Kritik taleplere dönüş",
            "contact.kpi2Label": "Standart geri bildirim",
            "contact.kpi3Label": "Mail, telefon, portal",
            "contact.kpi3Val": "Çok Kanal",
            "contact.kpi2Val": "Aynı Gün",
            "contact.formTitle": "Bize Yazın",
            "contact.formNamePh": "Ad Soyad",
            "contact.formEmailPh": "mail@firma.com",
            "contact.formMsgPh": "Mesajınız...",
            "contact.demo": "Demo Talebi",
            "contact.send": "Gönder",
            "contact.infoTitle": "İletişim Bilgileri",
            "contact.mail": "E-posta",
            "contact.phone": "Telefon",
            "contact.address": "Adres",
            "contact.map": "Harita alanı",
            "contact.ctaT": "Kurumsal teklif mi?",
            "contact.ctaD": "Kapsam ve entegrasyonlara göre netleştirelim.",
            "contact.ctaB": "Demo Talep Et",

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
            "price.p3f3": "Özel SLA & danışmanlık",

            "home.aiAssistantBadge": "Yeni",
            "home.aiAssistantTitle": "Yeni: Pika AI Kampanya Asistanı",
            "home.aiAssistantDesc": "Kampanya fikrinizi yazın. Pika sizin için kampanya taslağını, hedef kitle önerisini ve email template'i oluştursun.",
            "home.aiAssistantCta": "AI Kampanya Asistanını İncele →",
            "home.aiAssistantIdea": "Fikir:",
            "home.aiAssistantIdeaText": "Son 60 gündür alışveriş yapmayan müşterilere geri kazanım kampanyası oluştur.",
            "home.aiAssistantOutput": "Oluşturulanlar:",
            "home.aiOut1": "Kampanya taslağı",
            "home.aiOut2": "Hedef kitle önerisi",
            "home.aiOut3": "Email metni",
            "home.aiOut4": "Email template",

            "home.mock.send": "Gönderim",
            "home.mock.open": "Açılma",
            "home.mock.conversion": "Dönüşüm",
            "home.mock.campPerf": "Kampanya Performansı",
            "home.mock.last7": "Son 7 gün",
            "home.mock.whatsappSent": "2,340 mesaj gönderildi",
            "home.mock.aiCampaignCreated": "Kampanya oluşturuldu ✓",

            "home.flow.trigger": "Tetikleyici: Sepete ürün eklendi",
            "home.flow.condition1": "Koşul: 2 saat içinde satın alma yok",
            "home.flow.action1": "Aksiyon: WhatsApp hatırlatma gönder",
            "home.flow.wait": "Bekleme: 24 saat",
            "home.flow.action2": "Aksiyon: Email ile indirim kodu gönder",
            "home.flow.end": "Bitiş: Kampanya tamamlandı",

            "home.aiPanel.header": "Pika AI Asistan",
            "home.aiPanel.userMsg1": "Yılbaşı için VIP müşterilere özel bir WhatsApp kampanyası oluştur",
            "home.aiPanel.botDraft": "Kampanya Taslağı Hazır ✓",
            "home.aiPanel.botChannel": "📋 Kanal: WhatsApp",
            "home.aiPanel.botSegment": "👥 Segment: VIP Müşteriler (2,340 kişi)",
            "home.aiPanel.botMessage": "📝 Mesaj: \"Yeni yıla özel %20 indirim sizin için hazır!\"",
            "home.aiPanel.botTiming": "⏰ Zamanlama: Bugün 18:00",
            "home.aiPanel.userApprove": "Onaylıyorum, gönder",
            "home.aiPanel.botSent": "Kampanya gönderime alındı. ✅ Sonuçları Analytics panelinden takip edebilirsiniz.",
            "home.aiPanel.chipResults": "Sonuçları Gör",
            "home.aiPanel.chipEdit": "Düzenle",

            "home.segBuilder.title": "Segment Builder",
            "home.segBuilder.storeLabel": "Mağaza",
            "home.segBuilder.storeValue": "Kadıköy Mağaza",
            "home.segBuilder.storeCount": "1,240 müşteri",
            "home.segBuilder.productLabel": "Ürün",
            "home.segBuilder.productValue": "Spor Ayakkabı",
            "home.segBuilder.productCount": "856 müşteri",
            "home.segBuilder.behaviorLabel": "Davranış",
            "home.segBuilder.behaviorValue": "Son 30 günde ≥ 3 satın alma",
            "home.segBuilder.behaviorCount": "342 müşteri",
            "home.segBuilder.resultLabel": "Sonuç Segment",
            "home.segBuilder.resultCount": "342 müşteri",

            "nav.resources": "Kaynaklar",
            "nav.wiki": "Wiki / Bilgi Bankası",
            "nav.mega.intelligence": "Zekâ",
            "nav.mega.action": "Aksiyon",
            "nav.mega.measure": "Ölçüm",
            "nav.mega.customerIntelligenceTitle": "Customer Intelligence",
            "nav.mega.customerIntelligenceDesc": "Müşteri davranışı ve değer analizi",
            "nav.mega.productIntelligenceTitle": "Product Intelligence",
            "nav.mega.productIntelligenceDesc": "Ürün anlamlandırma ve bağlam",
            "nav.mega.pika360Title": "Pika 360",
            "nav.mega.pika360Desc": "Bütünleşik müşteri karar özeti",
            "nav.mega.opportunities": "Günün Fırsatları",
            "nav.mega.opportunitiesDesc": "Fırsat ve karar motoru",
            "nav.mega.integrationsTitle": "Entegrasyonlar",
            "nav.mega.integrationsDesc": "API ve veri bağlantıları",
            "nav.mega.campaignManagerDesc": "Kampanya oluşturma ve yönetim",
            "nav.mega.audienceManagerDesc": "Hedef kitle ve segmentasyon",
            "nav.mega.journeyManagerDesc": "Otomasyon akışları",
            "nav.mega.contentStudioDesc": "İçerik tasarım ve üretim",
            "nav.mega.reportingDesc": "Performans ölçümleme",
            "nav.mega.consentManagementDesc": "İzin ve uyumluluk yönetimi",
            "nav.mega.aiAssistantTitle": "Pika AI Kampanya Asistanı",
            "nav.mega.aiAssistantDesc": "Kampanya fikrinden hazır kampanya ve email template oluşturun.",
            "nav.mega.whatsappDesc": "Template ve kampanya gönderimi",
            "nav.mega.smsDesc": "Hızlı erişim ve bildirimler",
            "nav.mega.emailDesc": "Zengin içerikli kampanyalar",
            "nav.mega.pushDesc": "Gerçek zamanlı bildirimler",
            "nav.aiAssistant": "AI Kampanya Asistanı",
            "nav.ecommerceAi": "E-ticaret AI Kampanya",
            "nav.iysKvkk": "İYS & KVKK Uyumu",
            "nav.useCases": "Kullanım Senaryoları",
            "nav.whatsappCampaign": "WhatsApp Kampanya Yönetimi",
            "nav.emailTemplateStudio": "Email Template Studio",

            "demoPage.heroTitle": "Demo Talebi",
            "demoPage.heroDesc": "Ekibinize özel bir oturum planlayalım. İhtiyaçlarınıza göre senaryoları birlikte çalışalım ve Pika'nın kurumunuza nasıl değer katacağını canlı olarak gösterelim.",
            "demoPage.whatsIncluded": "Neler Dahil?",
            "demoPage.item1": "45 dakikalık canlı ürün sunumu",
            "demoPage.item2": "Sektörünüze özel kullanım senaryoları",
            "demoPage.item3": "Teknik entegrasyon yaklaşımı",
            "demoPage.item4": "Soru-cevap ve yol haritası önerisi",
            "demoPage.formTitle": "Talep Formu",
            "demoPage.fullName": "Ad Soyad",
            "demoPage.email": "E-posta",
            "demoPage.company": "Şirket",
            "demoPage.companyPh": "Şirket Adı",
            "demoPage.phone": "Telefon",
            "demoPage.city": "Şehir",
            "demoPage.cityPh": "Şehir",
            "demoPage.note": "Kısa İhtiyaç Notu",
            "demoPage.notePh": "Hangi modüllerle ilgileniyorsunuz?",
            "demoPage.submit": "Demo Talebini Gönder",
            "demoPage.explorePika": "Önce Pika'yı İnceleyin"
        },
        en: {
            "topbar.security": "Enterprise security • Privacy-first processes",
            "topbar.support": "Support",
            "nav.platform": "Platform",
            "nav.products": "Products",
            "nav.channels": "Channels",
            "nav.solutions.ecommerce": "E-Commerce",
            "nav.solutions.retail": "Retail",
            "nav.solutions.lifecycle": "Lifecycle",
            "nav.solutions.crm": "CRM",
            "nav.integrations": "Integrations",
            "nav.security": "Security",
            "nav.about": "About",
            "nav.blog": "Blog",
            "footer.brandDesc": "Pika unifies omnichannel campaign management, segmentation, and AI-powered content generation in a single platform.",
            "nav.home": "Home",
            "nav.solutions": "Solutions",
            "nav.solutions.emailMarketing": "Email Marketing",
            "nav.solutions.smsCampaigns": "SMS Campaigns",
            "nav.solutions.whatsAppMessaging": "WhatsApp Messaging",
            "nav.solutions.pushNotifications": "Push Notifications",
            "nav.solutions.personalization": "Journey Orchestration",
            "nav.solutions.templateManagement": "Template Management",
            "nav.solutions.abTesting": "Automation & Scheduling",
            "nav.solutions.reporting": "Reporting",
            "nav.solutions.deliverabilityCompliance": "Deliverability & Compliance",
            "nav.solutions.dataManagementEtl": "Data Management ETL",
            "nav.solutions.realTimeEventProcessing": "Real-Time Event Processing",
            "nav.pricing": "Pricing",
            "nav.corporate": "Company",
            "nav.pika": "Pika",
            "nav.faq": "Frequently Asked Questions",
            "nav.termsOfUse": "Terms of Use",
            "nav.privacyPolicy": "Privacy Policy",
            "nav.contact": "Contact",
            "nav.login": "Login",
            "nav.demo": "Request a Demo",
            "nav.language": "Language",
            "nav.career": "Careers",

            "career.heroTitle": "Work at Pika",
            "career.heroDesc": "We're looking for passionate, innovative teammates to build the future of technology together. At Pika, we value both our customers' success and our own growth.",
            "career.whyEyebrow": "Why Pika?",
            "career.whyTitle": "Why is working at Pika different?",
            "career.whyDesc": "Not just a workplace — an ecosystem that propels your career forward.",
            "career.card1Title": "Fast-Growing Technology",
            "career.card1Desc": "Work with cutting-edge technologies on a leading omnichannel marketing platform.",
            "career.card2Title": "Strong Team Culture",
            "career.card2Desc": "Open communication, transparent management, and a supportive, learning-oriented team.",
            "career.card3Title": "Innovation Focused",
            "career.card3Desc": "New ideas are encouraged. Have a voice in AI, automation, and data-driven projects.",
            "career.card4Title": "Continuous Growth",
            "career.card4Desc": "Training budget, conference attendance, mentorship programs, and career planning support.",
            "career.card5Title": "Work-Life Balance",
            "career.card5Desc": "Flexible working hours, remote work options, and employee well-being policies.",
            "career.card6Title": "Global Vision",
            "career.card6Desc": "International clients, multi-language platform, and the chance to make a global impact.",
            "career.valuesEyebrow": "Our Values",
            "career.valuesTitle": "We believe in the power of achieving together",
            "career.valuesDesc": "At Pika, every individual matters. Teamwork, transparency, and customer focus are our core values.",
            "career.val1": "Transparency",
            "career.val2": "Responsibility",
            "career.val3": "Innovation",
            "career.val4": "Customer Focus",
            "career.quote": "\"At Pika, we don't just write code; we co-design our customers' success. Every sprint, every idea, and every line of code is part of this mission.\"",
            "career.quoteAuthor": "— Pika Team",
            "career.processEyebrow": "Application Process",
            "career.processTitle": "How to apply?",
            "career.step1Title": "Fill the Form",
            "career.step1Desc": "Complete the application form below with your information.",
            "career.step2Title": "Review",
            "career.step2Desc": "Our team carefully evaluates your application.",
            "career.step3Title": "Interview",
            "career.step3Desc": "We conduct technical and cultural fit interviews with suitable candidates.",
            "career.step4Title": "Welcome!",
            "career.step4Desc": "Join our team through our offer and onboarding process.",
            "career.formTitle": "Application Form",
            "career.formDesc": "Fill out the form below to reach us. Your application will be reviewed as soon as possible.",
            "career.position": "Position Applied For",
            "career.positionPh": "Select a position",
            "career.cvUrl": "CV / LinkedIn Link",
            "career.cvFile": "Upload CV (PDF, max 5MB)",
            "career.apply": "Submit Application",

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
            "form.phonePh": "5XX XXX XX XX",
            "form.cityPh": "City",
            "form.messagePh": "Briefly describe your needs...",
            "form.privacy": "Your info stays private.",
            "form.close": "Close",
            "form.send": "Send",

            "home.heroTitle": "Send the right message, through the right channel, at the right time",
            "home.heroDesc": "Pika unifies campaign management, segmentation, personalization, journey automation, consent management, and reporting across SMS, WhatsApp, Email, and Push channels in a single platform. AI-powered content and flow generation helps teams create faster and more effective campaigns.",
            "home.heroCta2": "Explore Platform",
            "home.tag1": "AI-powered campaign generation",
            "home.tag2": "Transaction-based segmentation",
            "home.tag3": "Journey automation",
            "home.tag4": "Consent and compliance management",
            "home.tag5": "WhatsApp, SMS, Email, Push",
            "home.trust1Title": "Multi-channel campaign management",
            "home.trust1Desc": "SMS, WhatsApp, Email and Push in one place",
            "home.trust2Title": "Real-time triggers",
            "home.trust2Desc": "Behavior and transaction-based automation flows",
            "home.trust3Title": "Consent and compliance management",
            "home.trust3Desc": "IYS and communication preferences under control",
            "home.trust4Title": "AI-powered operations",
            "home.trust4Desc": "Smart assistant layer that shortens campaign production time",
            "home.problemEyebrow": "Problem & Solution",
            "home.problemTitle": "Centralize scattered campaign processes",
            "home.problemDesc": "Different channels, fragmented tools, and manual processes slow marketing teams down. Pika unifies data, campaigns, automation, and reporting under one roof.",
            "home.prob1Title": "Fragmented channel management",
            "home.prob1Desc": "Using separate tools for each channel reduces team speed and visibility.",
            "home.prob2Title": "Manual campaign operations",
            "home.prob2Desc": "Content creation, segment preparation, and scheduling processes exhaust teams.",
            "home.prob3Title": "Inability to turn data into action",
            "home.prob3Desc": "When transaction data is underutilized, campaigns are broad but ineffective.",
            "home.sol1Title": "Centralized management",
            "home.sol1Desc": "Manage all main campaign channels from a single panel.",
            "home.sol2Title": "AI-accelerated operations",
            "home.sol2Desc": "Speed up content and campaign setup processes.",
            "home.sol3Title": "Data-driven targeting",
            "home.sol3Desc": "Create segments based on customer behavior and launch journeys.",
            "home.productsEyebrow": "Products",
            "home.productsTitle": "Pika core products",
            "home.productsDesc": "Essential capabilities for customer engagement and campaign operations in a single platform.",
            "home.mod1Desc": "Create, schedule, and manage SMS, WhatsApp, Email, and Push campaigns.",
            "home.mod2Desc": "Build target audiences based on transactions, behavior, and custom rules.",
            "home.mod3Desc": "Set up event-based automated flows and remove manual overhead.",
            "home.mod4Desc": "Accelerate campaign preparation with drag-and-drop design and AI-powered content generation.",
            "home.mod5Desc": "Measure channel, campaign, and segment performance.",
            "home.mod6Desc": "Manage consent, subscription preferences, and compliance processes from one place.",
            "home.channelsEyebrow": "Channels",
            "home.channelsTitle": "All main communication channels in one place",
            "home.channelsDesc": "Manage your campaigns and automation flows without splitting across different tools.",
            "home.ch1Desc": "Template management, personalized messages, campaign delivery, and process-based usage.",
            "home.ch2Desc": "Quick access, critical notifications, and high-visibility communications.",
            "home.ch3Desc": "Visually rich, personalized, drag-and-drop campaigns.",
            "home.ch4Desc": "React instantly to real-time events and strengthen in-app engagement.",
            "home.journeyEyebrow": "Journey Automation",
            "home.journeyTitle": "Take automatic action based on customer behavior",
            "home.journeyDesc": "With Pika's journey structure, the system automatically starts the next step when a transaction occurs.",
            "home.j1": "Automatic reminder after cart activity",
            "home.j2": "Special birthday campaign",
            "home.j3": "New communication flow on segment change",
            "home.j4": "Campaign triggered after transaction entry",
            "home.j5": "Automatic processes based on special dates or behavior",
            "home.journeyQuote": "Use rule-based automation instead of manual operations to take the right action at the right time.",
            "home.aiEyebrow": "Artificial Intelligence",
            "home.aiTitle": "Accelerate campaign production with AI",
            "home.aiDesc": "Pika's AI-powered structure helps teams manage campaign copy, content drafts, and campaign setup processes faster and more efficiently.",
            "home.ai1": "Campaign copy generation",
            "home.ai2": "Special occasion campaign creation",
            "home.ai3": "Segment-appropriate content suggestions",
            "home.ai4": "Email content draft preparation",
            "home.ai5": "Reducing operational time",
            "home.aiExample": "Speed up team operations with commands like 'Create a discount campaign for Ramadan and send it to this segment'.",
            "home.segEyebrow": "Segmentation",
            "home.segTitle": "Turn transaction data into targeting",
            "home.segDesc": "Work with real customer behavior, not just contact lists.",
            "home.seg1": "Store-based filtering",
            "home.seg2": "Product-based segmentation",
            "home.seg3": "Repeat purchase behavior",
            "home.seg4": "Transaction frequency",
            "home.seg5": "Behavior-based targeting",
            "home.seg6": "Reusable segments",
            "home.segQuote": "Segment customers who purchased a specific product from a specific store at least 3 times in one step and reuse.",
            "home.scenariosEyebrow": "Use Cases",
            "home.scenariosTitle": "What can you do with Pika?",
            "home.sc1Title": "Ramadan campaign",
            "home.sc1Desc": "Create personalized multi-channel campaigns for specific segments.",
            "home.sc2Title": "Birthday flow",
            "home.sc2Desc": "Set up offer flows that start automatically on special dates.",
            "home.sc3Title": "Cart reminder",
            "home.sc3Desc": "Increase return rates with behavior-based reminder flows.",
            "home.sc4Title": "Repurchase campaign",
            "home.sc4Desc": "Target based on specific product or store behavior.",
            "home.sc5Title": "Custom customer segments",
            "home.sc5Desc": "Create high-value customer groups based on purchase patterns.",
            "home.sc6Title": "Omnichannel journey management",
            "home.sc6Desc": "Manage Email, WhatsApp, SMS, and Push actions in the same flow.",
            "home.complianceEyebrow": "Integration & Compliance",
            "home.complianceTitle": "Compliant, scalable, and ready to integrate",
            "home.complianceDesc": "Pika can be modularly extended to work with data flows, event-based triggers, and enterprise processes.",
            "home.comp1Title": "API-ready architecture",
            "home.comp1Desc": "Easily connect with your existing systems.",
            "home.comp2Title": "Real-time event processing",
            "home.comp2Desc": "Take fast action with instant triggers.",
            "home.comp3Title": "Data management",
            "home.comp3Desc": "Ensure secure and organized data flow.",
            "home.comp4Title": "Consent and compliance processes",
            "home.comp4Desc": "Meet GDPR and consent management requirements.",
            "home.optionalEyebrow": "Optional Modules",
            "home.optionalTitle": "Expand your platform as your needs grow",
            "home.optionalDesc": "Pika can be extended with advanced solutions and enterprise needs through its modular architecture.",
            "home.optBadge": "Optional",
            "home.opt1Desc": "Automated voice interaction and self-service flows over phone.",
            "home.opt2Desc": "Expandable solution structure for text and voice interaction scenarios.",
            "home.opt3Desc": "Extensible architecture with custom enterprise systems and data flows.",
            "home.ctaTitle": "Manage your campaign operations from one center",
            "home.ctaDesc": "Turn customer data into action with Pika, accelerate campaign production, and make multi-channel communication smarter.",

            "sol.h1": "Solutions",
            "sol.chip": "Enterprise Messaging and Data Platform",
            "sol.lead": "Manage channel orchestration, personalization, and reporting in one architecture.",
            "sol.kpi.1.title": "12 Modules",
            "sol.kpi.1.desc": "Modern information architecture",
            "sol.kpi.2.title": "Omnichannel",
            "sol.kpi.2.desc": "All customer touchpoints",
            "sol.kpi.3.title": "Data-Driven",
            "sol.kpi.3.desc": "Real-time decisions",
            "sol.reporting": "Reporting",
            "sol.demo": "Request a Demo",
            "sol.map": "Solution Map",
            "sol.page.highlights": "Highlights",
            "sol.page.next": "Next",
            "sol.ctat": "Tailored to you",
            "sol.ctad": "We’ll define the best setup and KPI set for your model.",
            "sol.ctab": "Request a Demo",

            "sol.page.EmailMarketing.chip": "Email Marketing",
            "sol.page.EmailMarketing.title": "Email Marketing",
            "sol.page.EmailMarketing.lead": "Run lifecycle automation and content orchestration across email touchpoints.",
            "sol.page.EmailMarketing.h1": "Lifecycle automations",
            "sol.page.EmailMarketing.h2": "Dynamic segment sends",
            "sol.page.EmailMarketing.h3": "Deliverability and click analytics",
            "sol.page.SmsCampaigns.chip": "SMS Campaigns",
            "sol.page.SmsCampaigns.title": "SMS Campaigns",
            "sol.page.SmsCampaigns.lead": "Manage critical messages in SMS with timing and audience precision.",
            "sol.page.SmsCampaigns.h1": "Trigger-based SMS",
            "sol.page.SmsCampaigns.h2": "Carrier performance insights",
            "sol.page.SmsCampaigns.h3": "Quota and cost controls",
            "sol.page.WhatsAppMessaging.chip": "WhatsApp Messaging",
            "sol.page.WhatsAppMessaging.title": "WhatsApp Messaging",
            "sol.page.WhatsAppMessaging.lead": "Scale WhatsApp communication with approved templates and two-way flows.",
            "sol.page.WhatsAppMessaging.h1": "Template approval management",
            "sol.page.WhatsAppMessaging.h2": "Conversation analytics",
            "sol.page.WhatsAppMessaging.h3": "Bot-to-agent handoff",
            "sol.page.PushNotifications.chip": "Push Notifications",
            "sol.page.PushNotifications.title": "Push Notifications",
            "sol.page.PushNotifications.lead": "Build behavior-driven push programs for web and mobile.",
            "sol.page.PushNotifications.h1": "Real-time triggers",
            "sol.page.PushNotifications.h2": "Quiet hour policies",
            "sol.page.PushNotifications.h3": "Opt-in lifecycle",
            "sol.page.Personalization.chip": "Personalization",
            "sol.page.Personalization.title": "Personalization",
            "sol.page.Personalization.lead": "Personalize content in real time using behavior and profile signals.",
            "sol.page.Personalization.h1": "Dynamic recommendations",
            "sol.page.Personalization.h2": "Rule + ML hybrid logic",
            "sol.page.Personalization.h3": "Channel-specific variations",
            "sol.page.TemplateManagement.chip": "Template Management",
            "sol.page.TemplateManagement.title": "Template Management",
            "sol.page.TemplateManagement.lead": "Version, approve, and govern templates across all communication channels.",
            "sol.page.TemplateManagement.h1": "Version control",
            "sol.page.TemplateManagement.h2": "Reusable content blocks",
            "sol.page.TemplateManagement.h3": "Brand governance",
            "sol.page.ABTesting.chip": "A/B Testing",
            "sol.page.ABTesting.title": "A/B Testing",
            "sol.page.ABTesting.lead": "Measure lift by testing message, timing, channel, and content variants.",
            "sol.page.ABTesting.h1": "Statistical confidence",
            "sol.page.ABTesting.h2": "Holdout groups",
            "sol.page.ABTesting.h3": "Automatic winner selection",
            "sol.page.Reporting.chip": "Reporting",
            "sol.page.Reporting.title": "Reporting",
            "sol.page.Reporting.lead": "Unify KPI, conversion, and revenue contribution insights in one place.",
            "sol.page.Reporting.h1": "Live dashboards",
            "sol.page.Reporting.h2": "Cohort and funnel analysis",
            "sol.page.Reporting.h3": "Export and API",
            "sol.page.DeliverabilityCompliance.chip": "Deliverability & Compliance",
            "sol.page.DeliverabilityCompliance.title": "Deliverability & Compliance",
            "sol.page.DeliverabilityCompliance.lead": "Operationalize inbox placement and compliance workflows.",
            "sol.page.DeliverabilityCompliance.h1": "Consent management",
            "sol.page.DeliverabilityCompliance.h2": "Sender reputation tracking",
            "sol.page.DeliverabilityCompliance.h3": "Audit evidence",
            "sol.page.DataManagementEtl.chip": "Data Management ETL",
            "sol.page.DataManagementEtl.title": "Data Management ETL",
            "sol.page.DataManagementEtl.lead": "Standardize data pipelines and prepare customer data for activation.",
            "sol.page.DataManagementEtl.h1": "Source connectors",
            "sol.page.DataManagementEtl.h2": "Transform and enrich",
            "sol.page.DataManagementEtl.h3": "Quality checks",
            "sol.page.RealTimeEventProcessing.chip": "Real-Time Event Processing",
            "sol.page.RealTimeEventProcessing.title": "Real-Time Event Processing",
            "sol.page.RealTimeEventProcessing.lead": "Process customer events in milliseconds and trigger immediate actions.",
            "sol.page.RealTimeEventProcessing.h1": "Event stream processing",
            "sol.page.RealTimeEventProcessing.h2": "Instant action triggers",
            "sol.page.RealTimeEventProcessing.h3": "Scalable event architecture",
            "solutions.overview.title": "Solutions",
            "solutions.next": "Next",
            "solutions.sections.capabilities": "Capabilities",
            "solutions.sections.workflow": "Workflow",
            "solutions.sections.integrations": "Integrations",
            "solutions.sections.kpi": "KPI",
            "solutions.sections.compliance": "Compliance",
            "solutions.sections.scenarios": "Scenarios",
            "solutions.emailMarketing.title": "Email Marketing",
            "solutions.emailMarketing.subtitle": "Run lifecycle automation and content orchestration across email touchpoints.",
            "solutions.emailMarketing.summary": "Manage campaign planning, delivery, and optimization in a unified flow.",
            "solutions.emailMarketing.corp": "Enterprise Email Marketing",
            "solutions.emailMarketing.capabilities.1": "Lifecycle automations",
            "solutions.emailMarketing.capabilities.2": "Dynamic segment sends",
            "solutions.emailMarketing.capabilities.3": "Deliverability and click analytics",
            "solutions.emailMarketing.workflow.step1": "Define business goals and target segment scope.",
            "solutions.emailMarketing.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.emailMarketing.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.emailMarketing.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.emailMarketing.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.emailMarketing.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.emailMarketing.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.emailMarketing.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.emailMarketing.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.smsCampaigns.title": "SMS Campaigns",
            "solutions.smsCampaigns.subtitle": "Manage critical messages in SMS with precise timing and audience targeting.",
            "solutions.smsCampaigns.summary": "Handle high-reach operational and marketing messages from a single panel.",
            "solutions.smsCampaigns.corp": "Enterprise SMS Campaigns",
            "solutions.smsCampaigns.capabilities.1": "Trigger-based SMS",
            "solutions.smsCampaigns.capabilities.2": "Carrier performance insights",
            "solutions.smsCampaigns.capabilities.3": "Quota and cost controls",
            "solutions.smsCampaigns.workflow.step1": "Define business goals and target segment scope.",
            "solutions.smsCampaigns.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.smsCampaigns.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.smsCampaigns.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.smsCampaigns.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.smsCampaigns.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.smsCampaigns.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.smsCampaigns.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.smsCampaigns.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.whatsAppMessaging.title": "WhatsApp Messaging",
            "solutions.whatsAppMessaging.subtitle": "Scale WhatsApp communication with approved templates and two-way flows.",
            "solutions.whatsAppMessaging.summary": "Orchestrate customer support and marketing conversations safely.",
            "solutions.whatsAppMessaging.corp": "Enterprise WhatsApp Messaging",
            "solutions.whatsAppMessaging.capabilities.1": "Template approval management",
            "solutions.whatsAppMessaging.capabilities.2": "Conversation analytics",
            "solutions.whatsAppMessaging.capabilities.3": "Bot and agent handoff",
            "solutions.whatsAppMessaging.workflow.step1": "Define business goals and target segment scope.",
            "solutions.whatsAppMessaging.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.whatsAppMessaging.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.whatsAppMessaging.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.whatsAppMessaging.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.whatsAppMessaging.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.whatsAppMessaging.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.whatsAppMessaging.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.whatsAppMessaging.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.pushNotifications.title": "Push Notifications",
            "solutions.pushNotifications.subtitle": "Build behavior-driven push programs for web and mobile.",
            "solutions.pushNotifications.summary": "Guide users to the right screen at the right moment with real-time events.",
            "solutions.pushNotifications.corp": "Enterprise Push Notifications",
            "solutions.pushNotifications.capabilities.1": "Real-time triggers",
            "solutions.pushNotifications.capabilities.2": "Quiet-hour policies",
            "solutions.pushNotifications.capabilities.3": "Opt-in lifecycle",
            "solutions.pushNotifications.workflow.step1": "Define business goals and target segment scope.",
            "solutions.pushNotifications.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.pushNotifications.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.pushNotifications.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.pushNotifications.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.pushNotifications.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.pushNotifications.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.pushNotifications.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.pushNotifications.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.personalization.title": "Personalization",
            "solutions.personalization.subtitle": "Personalize content in real time using behavior and profile signals.",
            "solutions.personalization.summary": "Adapt offers, content, and channels dynamically by customer context.",
            "solutions.personalization.corp": "Enterprise Personalization",
            "solutions.personalization.capabilities.1": "Dynamic recommendations",
            "solutions.personalization.capabilities.2": "Rule + ML hybrid approach",
            "solutions.personalization.capabilities.3": "Channel-specific variations",
            "solutions.personalization.workflow.step1": "Define business goals and target segment scope.",
            "solutions.personalization.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.personalization.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.personalization.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.personalization.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.personalization.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.personalization.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.personalization.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.personalization.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.templateManagement.title": "Template Management",
            "solutions.templateManagement.subtitle": "Version, approve, and govern communication templates by channel.",
            "solutions.templateManagement.summary": "Run a centralized template operation that preserves brand consistency.",
            "solutions.templateManagement.corp": "Enterprise Template Management",
            "solutions.templateManagement.capabilities.1": "Version control",
            "solutions.templateManagement.capabilities.2": "Content component library",
            "solutions.templateManagement.capabilities.3": "Brand governance checks",
            "solutions.templateManagement.workflow.step1": "Define business goals and target segment scope.",
            "solutions.templateManagement.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.templateManagement.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.templateManagement.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.templateManagement.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.templateManagement.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.templateManagement.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.templateManagement.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.templateManagement.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.abTesting.title": "A/B Testing",
            "solutions.abTesting.subtitle": "Measure lift with experiments across message, timing, and channel.",
            "solutions.abTesting.summary": "Increase decision speed and reduce risk with controlled experiments.",
            "solutions.abTesting.corp": "Enterprise A/B Testing",
            "solutions.abTesting.capabilities.1": "Statistical significance",
            "solutions.abTesting.capabilities.2": "Holdout groups",
            "solutions.abTesting.capabilities.3": "Automatic winner selection",
            "solutions.abTesting.workflow.step1": "Define business goals and target segment scope.",
            "solutions.abTesting.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.abTesting.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.abTesting.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.abTesting.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.abTesting.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.abTesting.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.abTesting.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.abTesting.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.reporting.title": "Reporting",
            "solutions.reporting.subtitle": "Unify KPI, conversion, and revenue contribution reporting.",
            "solutions.reporting.summary": "Create real-time visibility for decision makers.",
            "solutions.reporting.corp": "Enterprise Reporting",
            "solutions.reporting.capabilities.1": "Live dashboards",
            "solutions.reporting.capabilities.2": "Cohort and funnel analysis",
            "solutions.reporting.capabilities.3": "Export and API",
            "solutions.reporting.workflow.step1": "Define business goals and target segment scope.",
            "solutions.reporting.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.reporting.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.reporting.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.reporting.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.reporting.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.reporting.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.reporting.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.reporting.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.deliverabilityCompliance.title": "Deliverability & Compliance",
            "solutions.deliverabilityCompliance.subtitle": "Operationalize inbox placement and regulatory compliance.",
            "solutions.deliverabilityCompliance.summary": "Manage sender reputation and compliance evidence in one standard.",
            "solutions.deliverabilityCompliance.corp": "Enterprise Deliverability & Compliance",
            "solutions.deliverabilityCompliance.capabilities.1": "Consent management",
            "solutions.deliverabilityCompliance.capabilities.2": "Sender reputation tracking",
            "solutions.deliverabilityCompliance.capabilities.3": "Compliance audit logs",
            "solutions.deliverabilityCompliance.workflow.step1": "Define business goals and target segment scope.",
            "solutions.deliverabilityCompliance.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.deliverabilityCompliance.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.deliverabilityCompliance.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.deliverabilityCompliance.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.deliverabilityCompliance.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.deliverabilityCompliance.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.deliverabilityCompliance.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.deliverabilityCompliance.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.dataManagementEtl.title": "Data Management ETL",
            "solutions.dataManagementEtl.subtitle": "Standardize data from multiple sources for activation.",
            "solutions.dataManagementEtl.summary": "Automate quality, transformation, and enrichment workflows.",
            "solutions.dataManagementEtl.corp": "Enterprise Data Management ETL",
            "solutions.dataManagementEtl.capabilities.1": "Source connectors",
            "solutions.dataManagementEtl.capabilities.2": "Transform and enrich",
            "solutions.dataManagementEtl.capabilities.3": "Data quality checks",
            "solutions.dataManagementEtl.workflow.step1": "Define business goals and target segment scope.",
            "solutions.dataManagementEtl.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.dataManagementEtl.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.dataManagementEtl.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.dataManagementEtl.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.dataManagementEtl.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.dataManagementEtl.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.dataManagementEtl.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.dataManagementEtl.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
            "solutions.realTimeEventProcessing.title": "Real-Time Event Processing",
            "solutions.realTimeEventProcessing.subtitle": "Process customer events in milliseconds and trigger actions.",
            "solutions.realTimeEventProcessing.summary": "Use scalable rules on live signals to improve engagement quality.",
            "solutions.realTimeEventProcessing.corp": "Enterprise Real-Time Event Processing",
            "solutions.realTimeEventProcessing.capabilities.1": "Event stream processing",
            "solutions.realTimeEventProcessing.capabilities.2": "Instant action triggers",
            "solutions.realTimeEventProcessing.capabilities.3": "Scalable event architecture",
            "solutions.realTimeEventProcessing.workflow.step1": "Define business goals and target segment scope.",
            "solutions.realTimeEventProcessing.workflow.step2": "Activate channel, content, and rule configuration.",
            "solutions.realTimeEventProcessing.workflow.step3": "Monitor outcomes and continuously optimize the flow.",
            "solutions.realTimeEventProcessing.integrations": "Works with CRM, e-commerce, and analytics systems.",
            "solutions.realTimeEventProcessing.kpi": "Operational speed, conversion, and revenue contribution are tracked as KPIs.",
            "solutions.realTimeEventProcessing.compliance": "Compliance is ensured through RBAC, logging, and consent governance.",
            "solutions.realTimeEventProcessing.scenarios": "Supports trigger-based messaging, reactivation, and personalization scenarios.",
            "solutions.realTimeEventProcessing.repoUnspecified1": "Note: Integration details not specified in the repo should be clarified during technical discovery.",
            "solutions.realTimeEventProcessing.repoUnspecified2": "Note: Industry/regulatory specifics not specified in the repo should be defined before go-live.",
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

            "terms.chip": "Corporate",
            "terms.title": "Terms of Service",
            "terms.lead": "Welcome to ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK. These Terms of Service (\"Terms\") govern your access to and use of our website, platforms, and any services provided by us (collectively, the \"Services\"). By accessing or using the Services, you agree to be bound by these Terms. If you do not agree, you must immediately cease using the Services.",
            "terms.intro1": "These Terms constitute a legally binding agreement between you (\"the User\") and ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK (\"the Company\").",
            "terms.intro2": "",
            "terms.s1Title": "1. Acceptance of Terms",
            "terms.s1Text": "By accessing or using the Services, you represent and warrant that:",
            "terms.s1Li1": "You are at least the age of majority in your jurisdiction and legally capable of entering into this agreement.",
            "terms.s1Li2": "If you are accessing the Services on behalf of a legal entity, you have the authority to bind that entity to these Terms.",
            "terms.s2Title": "2. Changes to Terms",
            "terms.s2Text": "We reserve the right to modify or update these Terms at any time. Changes will take effect upon being posted to our website or otherwise communicated to you. Your continued use of the Services after such modifications constitutes acceptance of the updated Terms.",
            "terms.s3Title": "3. Use of Services",
            "terms.s3Text": "You agree to use the Services in compliance with all applicable laws, regulations, and these Terms. You are prohibited from:",
            "terms.s3Li1": "Engaging in any activity that disrupts or interferes with the proper functioning of the Services.",
            "terms.s3Li2": "Using the Services for any unlawful, harmful, or fraudulent purpose.",
            "terms.s3Li3": "Attempting to access or manipulate unauthorized parts of our systems or data.",
            "terms.s3Li4": "Distributing malicious software, viruses, or similar threats through the Services.",
            "terms.s3Text2": "We reserve the right to investigate and take appropriate legal action against anyone who, in our sole discretion, violates this provision.",
            "terms.s4Title": "4. Account Responsibility",
            "terms.s4Text": "If you are required to create an account to use certain features of the Services, you agree to:",
            "terms.s4Li1": "Provide accurate, up-to-date, and complete information during the registration process.",
            "terms.s4Li2": "Maintain the confidentiality of your account credentials and be solely responsible for all activities that occur under your account.",
            "terms.s4Li3": "Notify us immediately of any unauthorized access or use of your account.",
            "terms.s4Text2": "The Company will not be liable for any loss or damage arising from your failure to comply with these obligations.",
            "terms.s5Title": "5. Intellectual Property",
            "terms.s5Text1": "All content, materials, and intellectual property made available through the Services, including but not limited to text, graphics, logos, designs, software, and trademarks, are the property of ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK or its licensors.",
            "terms.s5Text2": "You are granted a limited, non-exclusive, non-transferable, and revocable license to access and use the Services solely for personal or authorized business purposes. Any unauthorized use, reproduction, or distribution of our content is strictly prohibited and may result in legal action.",
            "terms.s6Title": "6. Privacy and Data Protection",
            "terms.s6Text1": "Your use of the Services is subject to our Privacy Policy, which outlines how we collect, use, and protect your personal information. By using the Services, you agree to our collection and use of your information as described in the Privacy Policy.",
            "terms.s6Text2Prefix": "For more information, please refer to our ",
            "terms.s6Link": "Privacy Policy",
            "terms.s6Text2Suffix": ".",
            "terms.s7Title": "7. Limitation of Liability",
            "terms.s7Text1": "To the fullest extent permitted by law, ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK shall not be held liable for any indirect, incidental, special, or consequential damages arising out of or in connection with your use of the Services.",
            "terms.s7Text2": "The Company provides the Services on an \"as is\" and \"as available\" basis and disclaims all warranties, express or implied, including but not limited to warranties of merchantability, fitness for a particular purpose, and non-infringement.",
            "terms.s8Title": "8. Indemnification",
            "terms.s8Text": "You agree to indemnify, defend, and hold harmless ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK, its affiliates, and their respective directors, officers, employees, and agents from any claims, damages, losses, or liabilities arising out of:",
            "terms.s8Li1": "Your use of the Services.",
            "terms.s8Li2": "Your violation of these Terms.",
            "terms.s8Li3": "Your infringement of any intellectual property or rights of a third party.",
            "terms.s9Title": "9. Suspension or Termination of Access",
            "terms.s9Text": "We reserve the right to suspend or terminate your access to the Services at any time, with or without cause, and without prior notice, if we determine, in our sole discretion, that you have violated these Terms or pose a risk to the integrity of the Services.",
            "terms.s10Title": "10. Governing Law and Jurisdiction",
            "terms.s10Text": "These Terms are governed by and construed in accordance with the laws of the Republic of Türkiye, without regard to its conflict of laws principles. Any disputes arising under or in connection with these Terms shall be subject to the exclusive jurisdiction of the courts located in Ankara, Türkiye.",
            "terms.s11Title": "11. Severability",
            "terms.s11Text": "If any provision of these Terms is found to be invalid or unenforceable by a court of competent jurisdiction, the remaining provisions will remain in full force and effect.",
            "terms.s12Title": "12. Entire Agreement",
            "terms.s12Text": "These Terms, together with our Privacy Policy, constitute the entire agreement between you and ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK regarding your use of the Services and supersede any prior agreements or understandings.",
            "terms.s13Title": "13. Contact Information",
            "terms.s13Text": "If you have any questions or concerns regarding these Terms, please contact us at:",
            "terms.contactEmail": "Email:",
            "terms.contactAddress": "Address:",
            "terms.contactAddressValue": "Ostim Osb Mah. 100. Yıl Bulvarı, 55/E Kat:4, Teknopark Turkuaz Bina, 06374 Yenimahalle/Ankara",
            "terms.closing": "By using the Services provided by ARCA YAZILIM BİLİŞİM EĞİTİM DANIŞMANLIK, you acknowledge that you have read, understood, and agreed to these Terms.",
            "terms.effectiveDate": "Effective Date:",
            "terms.effectiveDateValue": "August 20, 2020",

            "privacy.chip": "Corporate",
            "privacy.title": "Privacy Policy",
            "privacy.lead": "ARCA built apps as free and paid. This SERVICE is provided by ARCA at no cost and is intended for use as is.",
            "privacy.intro1": "This page is used to inform visitors regarding our policies with the collection, use, and disclosure of Personal Information if anyone decided to use our Service.",
            "privacy.intro2": "If you choose to use our Service, then you agree to the collection and use of information in relation to this policy. The Personal Information that we collect is used for providing and improving the Service. We will not use or share your information with anyone except as described in this Privacy Policy.",
            "privacy.intro3": "The terms used in this Privacy Policy have the same meanings as in our Terms and Conditions, which is accessible at ARCA apps unless otherwise defined in this Privacy Policy.",
            "privacy.s1Title": "1. Information Collection and Use",
            "privacy.s1Text": "For a better experience, while using our Service, we may require you to provide us with certain personally identifiable information. The information that we request will be retained on your device and is not collected by us in any way. The app does use third party services that may collect information used to identify you.",
            "privacy.s2Title": "2. Log Data",
            "privacy.s2Text": "We wish to inform you that whenever you use our Service, in a case of an error in the app we collect data and information (through third party products) on your phone called Log Data. This Log Data may include information such as your device Internet Protocol (\"IP\") address, device name, operating system version, the configuration of the app when utilizing our Service, the time and date of your use of the Service, and other statistics.",
            "privacy.s3Title": "3. Cookies",
            "privacy.s3Text1": "Cookies are files with a small amount of data that are commonly used as anonymous unique identifiers. These are sent to your browser from the websites that you visit and are stored on your device's internal memory.",
            "privacy.s3Text2": "This Service does not use these \"cookies\" explicitly. However, the app may use third party code and libraries that use \"cookies\" to collect information and improve their services. You have the option to either accept or refuse these cookies and know when a cookie is being sent to your device. If you choose to refuse our cookies, you may not be able to use some portions of this Service.",
            "privacy.s4Title": "4. Service Providers",
            "privacy.s4Text1": "We may employ third-party companies and individuals due to the following reasons:",
            "privacy.s4Li1": "To facilitate our Service;",
            "privacy.s4Li2": "To provide the Service on our behalf;",
            "privacy.s4Li3": "To perform Service-related services; or",
            "privacy.s4Li4": "To assist us in analyzing how our Service is used.",
            "privacy.s4Text2": "We wish to inform users of this Service that these third parties have access to your Personal Information. The reason is to perform the tasks assigned to them on our behalf. However, they are obligated not to disclose or use the information for any other purpose.",
            "privacy.s5Title": "5. Security",
            "privacy.s5Text": "We value your trust in providing us your Personal Information, thus we are striving to use commercially acceptable means of protecting it. However, please note that no method of transmission over the internet, or method of electronic storage is 100% secure and reliable, and we cannot guarantee its absolute security.",
            "privacy.s6Title": "6. Links to Other Sites",
            "privacy.s6Text": "This Service may contain links to other sites. If you click on a third-party link, you will be directed to that site. Please note that these external sites are not operated by us. Therefore, we strongly advise you to review the Privacy Policy of these websites. We have no control over and assume no responsibility for the content, privacy policies, or practices of any third-party sites or services.",
            "privacy.s7Title": "7. Children's Privacy",
            "privacy.s7Text1": "These Services do not address anyone under the age of 13. We do not knowingly collect personally identifiable information from children under 13. In the case we discover that a child under 13 has provided us with personal information, we immediately delete this from our servers.",
            "privacy.s7Text2": "If you are a parent or guardian and you are aware that your child has provided us with personal information, please contact us so that we may take the necessary actions.",
            "privacy.s8Title": "8. Changes to This Privacy Policy",
            "privacy.s8Text": "We may update our Privacy Policy from time to time. Thus, you are advised to review this page periodically for any changes. We will notify you of any changes by posting the new Privacy Policy on this page. These changes are effective immediately after they are posted on this page.",
            "privacy.s9Title": "9. Google User Data",
            "privacy.s9Q1": "What Google user data we access:",
            "privacy.s9A1": "Our application accesses your Google Contacts data only after you grant explicit permission through the OAuth authorization process. Specifically, the app may read contact information from your Google Contacts list. No other Google user data (such as Gmail, Drive, or Calendar) is accessed.",
            "privacy.s9Q2": "How we use your Google user data:",
            "privacy.s9A2": "The Google Contacts data we access is used solely to provide contacts-related functionality — such as reading contact lists and information as requested by the user. This data is not used for advertising, analytics, profiling, or any unrelated purpose.",
            "privacy.s9Q3": "Who we share your Google user data with:",
            "privacy.s9A3": "We do not share, sell, or disclose any Google user data to third parties. Data may only be disclosed if required by law or a valid legal process.",
            "privacy.s9Q4": "Data protection mechanisms:",
            "privacy.s9A4": "All communication with Google APIs is secured using industry-standard encryption (HTTPS/SSL). Access tokens and user credentials are stored securely and are never exposed to unauthorized parties. Users can revoke the app's access to their Google data at any time through their Google Account Permissions page.",
            "privacy.s9Q5": "Data retention:",
            "privacy.s9A5": "Google user data is stored only as long as necessary to provide the requested functionality. When a user revokes access or deletes their account, all associated data and tokens are securely deleted.",
            "privacy.s10Title": "10. Contact Us",
            "privacy.s10Text": "If you have any questions or suggestions about our Privacy Policy, please do not hesitate to contact us at ",
            "privacy.lastUpdate": "Last update:",

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
            "contact.chip": "Enterprise Contact",
            "contact.kpi1Label": "Response for critical requests",
            "contact.kpi2Label": "Standard response",
            "contact.kpi3Label": "Email, phone, portal",
            "contact.kpi3Val": "Multi-channel",
            "contact.kpi2Val": "Same Day",
            "contact.formNamePh": "Full name",
            "contact.formEmailPh": "name@company.com",
            "contact.formMsgPh": "Your message...",
            "contact.ctaB": "Request a Demo",

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

            "home.aiAssistantBadge": "New",
            "home.aiAssistantTitle": "New: Pika AI Campaign Assistant",
            "home.aiAssistantDesc": "Describe your campaign idea. Pika generates the campaign draft, audience recommendation, and email template for you.",
            "home.aiAssistantCta": "Explore AI Campaign Assistant →",
            "home.aiAssistantIdea": "Idea:",
            "home.aiAssistantIdeaText": "Create a win-back campaign for customers who haven't purchased in the last 60 days.",
            "home.aiAssistantOutput": "Generated:",
            "home.aiOut1": "Campaign draft",
            "home.aiOut2": "Audience recommendation",
            "home.aiOut3": "Email copy",
            "home.aiOut4": "Email template",

            "home.mock.send": "Sends",
            "home.mock.open": "Opens",
            "home.mock.conversion": "Conversion",
            "home.mock.campPerf": "Campaign Performance",
            "home.mock.last7": "Last 7 days",
            "home.mock.whatsappSent": "2,340 messages sent",
            "home.mock.aiCampaignCreated": "Campaign created ✓",

            "nav.resources": "Resources",
            "nav.wiki": "Wiki / Knowledge Base",
            "nav.mega.intelligence": "Intelligence",
            "nav.mega.action": "Action",
            "nav.mega.measure": "Measure",
            "nav.mega.customerIntelligenceTitle": "Customer Intelligence",
            "nav.mega.customerIntelligenceDesc": "Customer behavior and value analytics",
            "nav.mega.productIntelligenceTitle": "Product Intelligence",
            "nav.mega.productIntelligenceDesc": "Product contextualization & roles",
            "nav.mega.pika360Title": "Pika 360",
            "nav.mega.pika360Desc": "Unified customer decision overview",
            "nav.mega.opportunities": "Daily Opportunities",
            "nav.mega.opportunitiesDesc": "Opportunity and decision engine",
            "nav.mega.integrationsTitle": "Integrations",
            "nav.mega.integrationsDesc": "API and data connectivity",
            "nav.mega.campaignManagerDesc": "Campaign creation and management",
            "nav.mega.audienceManagerDesc": "Audience and segmentation",
            "nav.mega.journeyManagerDesc": "Automation flows",
            "nav.mega.contentStudioDesc": "Content design and production",
            "nav.mega.reportingDesc": "Performance measurement",
            "nav.mega.consentManagementDesc": "Consent and compliance management",
            "nav.mega.aiAssistantTitle": "Pika AI Campaign Assistant",
            "nav.mega.aiAssistantDesc": "Turn campaign ideas into ready-to-send campaigns and email templates.",
            "nav.mega.whatsappDesc": "Template and campaign delivery",
            "nav.mega.smsDesc": "Quick access and notifications",
            "nav.mega.emailDesc": "Rich content campaigns",
            "nav.mega.pushDesc": "Real-time notifications",
            "nav.aiAssistant": "AI Campaign Assistant",
            "nav.ecommerceAi": "E-commerce AI Campaign",
            "nav.iysKvkk": "IYS & KVKK Compliance",
            "nav.useCases": "Use Cases",
            "nav.whatsappCampaign": "WhatsApp Campaign Management",
            "nav.emailTemplateStudio": "Email Template Studio",

            "home.aiAssistantBadge": "New",
            "home.aiAssistantTitle": "New: Pika AI Campaign Assistant",
            "home.aiAssistantDesc": "Describe your campaign idea. Pika generates the campaign draft, audience recommendation, and email template for you.",
            "home.aiAssistantCta": "Explore AI Campaign Assistant →",
            "home.aiAssistantIdea": "Idea:",
            "home.aiAssistantIdeaText": "Create a win-back campaign for customers who haven't purchased in the last 60 days.",
            "home.aiAssistantOutput": "Generated:",
            "home.aiOut1": "Campaign draft",
            "home.aiOut2": "Audience recommendation",
            "home.aiOut3": "Email copy",
            "home.aiOut4": "Email template",

            "home.mock.send": "Sends",
            "home.mock.open": "Opens",
            "home.mock.conversion": "Conversion",
            "home.mock.campPerf": "Campaign Performance",
            "home.mock.last7": "Last 7 days",
            "home.mock.whatsappSent": "2,340 messages sent",
            "home.mock.aiCampaignCreated": "Campaign created ✓",

            "home.flow.trigger": "Trigger: Product added to cart",
            "home.flow.condition1": "Condition: No purchase within 2 hours",
            "home.flow.action1": "Action: Send WhatsApp reminder",
            "home.flow.wait": "Wait: 24 hours",
            "home.flow.action2": "Action: Send discount code via Email",
            "home.flow.end": "End: Campaign completed",

            "home.aiPanel.header": "Pika AI Assistant",
            "home.aiPanel.userMsg1": "Create a WhatsApp campaign exclusively for VIP customers for New Year",
            "home.aiPanel.botDraft": "Campaign Draft Ready ✓",
            "home.aiPanel.botChannel": "📋 Channel: WhatsApp",
            "home.aiPanel.botSegment": "👥 Segment: VIP Customers (2,340 people)",
            "home.aiPanel.botMessage": "📝 Message: \"Your exclusive 20% New Year discount is ready!\"",
            "home.aiPanel.botTiming": "⏰ Timing: Today at 18:00",
            "home.aiPanel.userApprove": "Approved, send it",
            "home.aiPanel.botSent": "Campaign queued for delivery. ✅ Track results from the Analytics panel.",
            "home.aiPanel.chipResults": "View Results",
            "home.aiPanel.chipEdit": "Edit",

            "home.segBuilder.title": "Segment Builder",
            "home.segBuilder.storeLabel": "Store",
            "home.segBuilder.storeValue": "Kadıköy Store",
            "home.segBuilder.storeCount": "1,240 customers",
            "home.segBuilder.productLabel": "Product",
            "home.segBuilder.productValue": "Sports Shoes",
            "home.segBuilder.productCount": "856 customers",
            "home.segBuilder.behaviorLabel": "Behavior",
            "home.segBuilder.behaviorValue": "≥ 3 purchases in last 30 days",
            "home.segBuilder.behaviorCount": "342 customers",
            "home.segBuilder.resultLabel": "Result Segment",
            "home.segBuilder.resultCount": "342 customers",

            "nav.resources": "Resources",
            "nav.wiki": "Wiki / Knowledge Base",
            "nav.mega.intelligence": "Intelligence",
            "nav.mega.action": "Action",
            "nav.mega.measure": "Measure",
            "nav.mega.customerIntelligenceTitle": "Customer Intelligence",
            "nav.mega.customerIntelligenceDesc": "Customer behavior and value analytics",
            "nav.mega.productIntelligenceTitle": "Product Intelligence",
            "nav.mega.productIntelligenceDesc": "Product contextualization & roles",
            "nav.mega.pika360Title": "Pika 360",
            "nav.mega.pika360Desc": "Unified customer decision overview",
            "nav.mega.opportunities": "Daily Opportunities",
            "nav.mega.opportunitiesDesc": "Opportunity and decision engine",
            "nav.mega.integrationsTitle": "Integrations",
            "nav.mega.integrationsDesc": "API and data connectivity",
            "nav.mega.campaignManagerDesc": "Campaign creation and management",
            "nav.mega.audienceManagerDesc": "Audience and segmentation",
            "nav.mega.journeyManagerDesc": "Automation flows",
            "nav.mega.contentStudioDesc": "Content design and production",
            "nav.mega.reportingDesc": "Performance measurement",
            "nav.mega.consentManagementDesc": "Consent and compliance management",
            "nav.mega.aiAssistantTitle": "Pika AI Campaign Assistant",
            "nav.mega.aiAssistantDesc": "Turn campaign ideas into ready-to-send campaigns and email templates.",
            "nav.mega.whatsappDesc": "Template and campaign delivery",
            "nav.mega.smsDesc": "Quick access and notifications",
            "nav.mega.emailDesc": "Rich content campaigns",
            "nav.mega.pushDesc": "Real-time notifications",
            "nav.aiAssistant": "AI Campaign Assistant",
            "nav.ecommerceAi": "E-commerce AI Campaign",
            "nav.iysKvkk": "IYS & KVKK Compliance",
            "nav.useCases": "Use Cases",
            "nav.whatsappCampaign": "WhatsApp Campaign Management",
            "nav.emailTemplateStudio": "Email Template Studio",

            "demoPage.heroTitle": "Request a Demo",
            "demoPage.heroDesc": "Let's plan a session tailored to your team. We'll work through scenarios for your needs and show live how Pika can add value to your organization.",
            "demoPage.whatsIncluded": "What's Included?",
            "demoPage.item1": "45-minute live product presentation",
            "demoPage.item2": "Industry-specific use cases",
            "demoPage.item3": "Technical integration approach",
            "demoPage.item4": "Q&A and roadmap recommendations",
            "demoPage.formTitle": "Request Form",
            "demoPage.fullName": "Full Name",
            "demoPage.email": "Email",
            "demoPage.company": "Company",
            "demoPage.companyPh": "Company Name",
            "demoPage.phone": "Phone",
            "demoPage.city": "City",
            "demoPage.cityPh": "City",
            "demoPage.note": "Brief Needs Note",
            "demoPage.notePh": "Which modules are you interested in?",
            "demoPage.submit": "Submit Demo Request",
            "demoPage.explorePika": "Explore Pika First",

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
        return getPathLang() || document.documentElement.getAttribute("data-lang") || "tr";
    }

    function setLang(lang, syncUrl = false) {
        const safe = (lang === "en" || lang === "tr") ? lang : "tr";
        localStorage.setItem("pika_lang", safe);
        document.documentElement.setAttribute("lang", safe);
        document.documentElement.setAttribute("data-lang", safe);

        const label = qs("#langLabel");
        if (label) label.textContent = safe.toUpperCase();
        qsa("[data-lang-label]").forEach(el => {
            el.textContent = safe.toUpperCase();
        });

        if (syncUrl) {
            const alternate = document.querySelector(`link[rel="alternate"][hreflang="${safe}"]`)?.getAttribute("href");
            if (alternate) {
                window.location.assign(alternate);
                return;
            }
            const fallback = safe === "en" ? "/en/" : "/";
            window.location.assign(fallback);
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

    function getTurnstileToken(container) {
        if (!container || typeof turnstile === "undefined") return "";
        return turnstile.getResponse(container) || "";
    }

    function renderTurnstile(container) {
        if (!container || typeof turnstile === "undefined") return;
        if (container.dataset.turnstileRendered) return;
        const sitekey = container.dataset.sitekey;
        if (!sitekey) return;
        turnstile.render(container, { sitekey, theme: container.dataset.theme || "auto" });
        container.dataset.turnstileRendered = "1";
    }

    function resetTurnstile(container) {
        if (!container || typeof turnstile === "undefined") return;
        turnstile.reset(container);
    }

    // Demo modal submit
    function bindDemoForm() {
        const form = qs("#demoRequestForm");
        if (!form) return;

        const modal = document.getElementById("demoModal");
        const tsWidget = form.querySelector(".cf-turnstile");

        if (modal && tsWidget) {
            modal.addEventListener("shown.bs.modal", () => renderTurnstile(tsWidget));
        }

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

            setFormLoading(form, true);

            setFormLoading(form, true);

            setFormLoading(form, true);

            const lang = getLang();
            const phoneRaw = (form.phone?.value || "").replace(/\D/g, "");
            const payload = {
                firstName: form.firstName?.value?.trim() || "",
                lastName: form.lastName?.value?.trim() || "",
                email: form.email?.value?.trim() || "",
                phone: "+90 " + phoneRaw,
                companyName: form.companyName?.value?.trim() || "",
                website: form.website?.value?.trim() || "",
                city: form.city?.value?.trim() || "",
                message: form.message?.value?.trim() || "",
                recaptchaToken: form.querySelector('input[name="g-recaptcha-response"]')?.value || "",
                lang
            };

            if (tsWidget) {
                const token = getTurnstileToken(tsWidget);
                if (!token) {
                    showResult(false, lang === "en" ? "Please complete the captcha." : "Lütfen captcha doğrulamasını tamamlayın.");
                    setFormLoading(form, false);
                    return;
                }
                payload.cfTurnstileResponse = token;
            }

            if (!payload.firstName || !payload.lastName || !payload.email || !phoneRaw || !payload.companyName) {
                showResult(false, lang === "en" ? "Please fill required fields." : "Lütfen zorunlu alanları doldurun.");
                setFormLoading(form, false);
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
                    resetTurnstile(tsWidget);
                } else {
                    showResult(false, data.message || (lang === "en" ? "Something went wrong." : "Bir hata oluştu."));
                }
            } catch {
                showResult(false, lang === "en" ? "Network error." : "Ağ hatası.");
            } finally {
                setFormLoading(form, false);
                delete form.dataset.recaptchaPassed;
            }
        });
    }

    // Demo page form submit
    function bindPageDemoForm() {
        const form = qs("#pageDemoRequestForm");
        if (!form) return;

        const tsWidget = form.querySelector(".cf-turnstile");
        if (tsWidget) {
            if (typeof turnstile !== "undefined") {
                renderTurnstile(tsWidget);
            } else {
                window.addEventListener("load", () => renderTurnstile(tsWidget));
            }
        }

        const result = qs("#pageDemoResult");
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
            const phoneRaw = (form.phone?.value || "").replace(/\D/g, "");
            const payload = {
                firstName: form.firstName?.value?.trim() || "",
                lastName: "",
                email: form.email?.value?.trim() || "",
                phone: "+90 " + phoneRaw,
                companyName: form.companyName?.value?.trim() || "",
                website: "",
                city: form.city?.value?.trim() || "",
                message: form.message?.value?.trim() || "",
                recaptchaToken: form.querySelector('input[name="g-recaptcha-response"]')?.value || "",
                lang
            };

            if (tsWidget) {
                const token = getTurnstileToken(tsWidget);
                if (!token) {
                    showResult(false, lang === "en" ? "Please complete the captcha." : "Lütfen captcha doğrulamasını tamamlayın.");
                    return;
                }
                payload.cfTurnstileResponse = token;
            }

            if (!payload.firstName || !payload.email || !phoneRaw || !payload.companyName) {
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
                    resetTurnstile(tsWidget);
                } else {
                    showResult(false, data.message || (lang === "en" ? "Something went wrong." : "Bir hata oluştu."));
                }
            } catch {
                showResult(false, lang === "en" ? "Network error." : "Ağ hatası.");
            } finally {
                setFormLoading(form, false);
                delete form.dataset.recaptchaPassed;
            }
        });
    }


    function bindContactForm() {
        const form = qs("#contactForm");
        if (!form) return;

        const result = qs("#contactResult");
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
                fullName: form.fullName?.value?.trim() || "",
                email: form.email?.value?.trim() || "",
                message: form.message?.value?.trim() || "",
                recaptchaToken: form.querySelector('input[name="g-recaptcha-response"]')?.value || "",
                lang
            };

            if (!payload.fullName || !payload.email || !payload.message) {
                showResult(false, lang === "en" ? "Please fill required fields." : "Lütfen zorunlu alanları doldurun.");
                return;
            }

            try {
                const res = await fetch(`/${lang}/lead/contact`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });
                const data = await res.json();
                if (res.ok && data.ok) {
                    showResult(true, data.message || (lang === "en" ? "Message sent." : "Mesaj gönderildi."));
                    form.reset();
                } else {
                    showResult(false, data.message || (lang === "en" ? "Something went wrong." : "Bir hata oluştu."));
                }
            } catch {
                showResult(false, lang === "en" ? "Network error." : "Ağ hatası.");
            } finally {
                setFormLoading(form, false);
                delete form.dataset.recaptchaPassed;
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

    // Mobile menu accordion toggle
    function bindMobileMenu() {
        document.querySelectorAll(".mobile-navbar .mobile-menu-list > a").forEach(function (link) {
            var parent = link.parentElement;
            if (!parent.querySelector(".mobile-menu-items")) return;
            link.addEventListener("click", function (e) {
                e.preventDefault();
                // close siblings
                parent.parentElement.querySelectorAll(".mobile-menu-list.active").forEach(function (sib) {
                    if (sib !== parent) sib.classList.remove("active");
                });
                parent.classList.toggle("active");
            });
        });
    }

    function bindCareerForm() {
        const form = qs("#careerForm");
        if (!form) return;

        const result = qs("#careerResult");
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
            const fullName = form.fullName?.value?.trim() || "";
            const email = form.email?.value?.trim() || "";
            const phone = form.phone?.value?.trim() || "";
            const position = form.position?.value?.trim() || "";

            if (!fullName || !email || !phone || !position) {
                showResult(false, lang === "en" ? "Please fill required fields." : "Lütfen zorunlu alanları doldurun.");
                return;
            }

            const cvFileInput = form.querySelector('input[name="cvFile"]');
            const cvFile = cvFileInput?.files?.[0];
            if (cvFile) {
                if (!cvFile.name.toLowerCase().endsWith('.pdf')) {
                    showResult(false, lang === "en" ? "Only PDF files are allowed." : "Sadece PDF dosyası yükleyebilirsiniz.");
                    return;
                }
                if (cvFile.size > 5 * 1024 * 1024) {
                    showResult(false, lang === "en" ? "CV file must be under 5 MB." : "CV dosyası en fazla 5 MB olabilir.");
                    return;
                }
            }

            setFormLoading(form, true);

            const fd = new FormData();
            fd.append("fullName", fullName);
            fd.append("email", email);
            fd.append("phone", phone);
            fd.append("position", position);
            fd.append("cvUrl", form.cvUrl?.value?.trim() || "");
            fd.append("message", form.message?.value?.trim() || "");
            fd.append("recaptchaToken", form.querySelector('input[name="g-recaptcha-response"]')?.value || "");
            fd.append("lang", lang);
            if (cvFile) {
                fd.append("cvFile", cvFile);
            }

            try {
                const res = await fetch(`/${lang}/lead/career`, {
                    method: "POST",
                    body: fd
                });
                const data = await res.json();
                if (res.ok && data.ok) {
                    showResult(true, data.message || (lang === "en" ? "Application sent." : "Başvuru gönderildi."));
                    form.reset();
                } else {
                    showResult(false, data.message || (lang === "en" ? "Something went wrong." : "Bir hata oluştu."));
                }
            } catch {
                showResult(false, lang === "en" ? "Network error." : "Ağ hatası.");
            } finally {
                setFormLoading(form, false);
            }
        });
    }

    // Phase 8: Living Hero Cockpit Staged Sequence
    function bindHeroLivingCockpit() {
        const wrap = document.getElementById("hero-cockpit-wrap");
        if (!wrap) return;

        const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
        if (prefersReducedMotion) return;

        const liveBadge = document.getElementById("hero-live-badge");
        const liveText = document.getElementById("hero-live-text");
        const statOpps = document.getElementById("hero-stat-opportunities");
        const focusRow = document.getElementById("hero-focus-row");
        const actionTray = document.getElementById("hero-action-tray");
        const isTr = document.documentElement.lang === "tr" || window.location.pathname.startsWith("/tr");

        let currentStage = 0;
        let isPaused = false;
        let timer = null;

        const setStage = (stage) => {
            currentStage = stage;

            // Reset all staged elements
            liveBadge?.classList.remove("is-alert");
            if (liveText) liveText.textContent = isTr ? "Canlı Akış" : "Live Stream";
            statOpps?.classList.remove("is-highlighted");
            focusRow?.classList.remove("is-highlighted");
            actionTray?.classList.remove("is-open");

            if (stage === 1) {
                // Stage 1: Neutral
            } else if (stage === 2) {
                // Stage 2: Detection indicator alert
                liveBadge?.classList.add("is-alert");
                if (liveText) liveText.textContent = isTr ? "Yeni Fırsat Tespit Edildi (+₺650)" : "New Opportunity Detected (+$650)";
            } else if (stage === 3) {
                // Stage 3: Opportunity stat highlighted
                liveBadge?.classList.add("is-alert");
                if (liveText) liveText.textContent = isTr ? "Yeni Fırsat Tespit Edildi (+₺650)" : "New Opportunity Detected (+$650)";
                statOpps?.classList.add("is-highlighted");
            } else if (stage === 4) {
                // Stage 4: Top row focused
                liveBadge?.classList.add("is-alert");
                if (liveText) liveText.textContent = isTr ? "Yeni Fırsat Tespit Edildi (+₺650)" : "New Opportunity Detected (+$650)";
                statOpps?.classList.add("is-highlighted");
                focusRow?.classList.add("is-highlighted");
            } else if (stage === 5) {
                // Stage 5: Action tray open
                liveBadge?.classList.add("is-alert");
                if (liveText) liveText.textContent = isTr ? "Aksiyon Hazır: WhatsApp" : "Action Ready: WhatsApp";
                statOpps?.classList.add("is-highlighted");
                focusRow?.classList.add("is-highlighted");
                actionTray?.classList.add("is-open");
            }
        };

        const tick = () => {
            if (isPaused) return;
            const nextStage = (currentStage % 5) + 1;
            setStage(nextStage);

            // Longer pause at stage 5 (actions ready) and stage 1 (neutral)
            const duration = (nextStage === 5) ? 5500 : (nextStage === 1) ? 4000 : 2500;
            timer = setTimeout(tick, duration);
        };

        // Pause on interaction
        wrap.addEventListener("mouseenter", () => { isPaused = true; });
        wrap.addEventListener("mouseleave", () => {
            isPaused = false;
            clearTimeout(timer);
            timer = setTimeout(tick, 2000);
        });

        // IntersectionObserver for visibility
        if ("IntersectionObserver" in window) {
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        isPaused = false;
                        clearTimeout(timer);
                        timer = setTimeout(tick, 1800);
                    } else {
                        isPaused = true;
                        clearTimeout(timer);
                    }
                });
            }, { threshold: 0.2 });
            observer.observe(wrap);
        } else {
            timer = setTimeout(tick, 2000);
        }

        // Filter chips
        const chips = wrap.querySelectorAll(".ph-cockpit-filter-chip");
        chips.forEach(chip => {
            chip.addEventListener("click", () => {
                chips.forEach(c => c.classList.remove("active"));
                chip.classList.add("active");
            });
        });
    }

    // Phase 8: Flagship Günün Fırsatları 5-Step Storytelling Console
    function bindOpportunityStoryWalkthrough() {
        const container = document.getElementById("opportunity-storyteller");
        if (!container) return;

        const stepBtns = container.querySelectorAll(".ph-story-step-btn");
        const panes = container.querySelectorAll(".ph-story-pane");
        const prevBtn = document.getElementById("story-prev-btn");
        const nextBtn = document.getElementById("story-next-btn");

        if (!stepBtns.length || !panes.length) return;

        let activeStep = 1;
        let isPaused = false;
        let rotationTimer = null;
        const totalSteps = stepBtns.length;

        const goToStep = (stepNum) => {
            activeStep = ((stepNum - 1 + totalSteps) % totalSteps) + 1;

            stepBtns.forEach(btn => {
                const step = parseInt(btn.getAttribute("data-story-step"), 10);
                const isActive = step === activeStep;
                btn.classList.toggle("active", isActive);
                btn.setAttribute("aria-selected", isActive ? "true" : "false");
            });

            panes.forEach(pane => {
                const step = parseInt(pane.getAttribute("data-story-pane"), 10);
                pane.classList.toggle("active", step === activeStep);
            });
        };

        stepBtns.forEach(btn => {
            btn.addEventListener("click", () => {
                const step = parseInt(btn.getAttribute("data-story-step"), 10);
                if (!isNaN(step)) {
                    goToStep(step);
                    isPaused = true; // pause auto rotation when user clicks
                }
            });
        });

        prevBtn?.addEventListener("click", () => {
            goToStep(activeStep - 1);
            isPaused = true;
        });

        nextBtn?.addEventListener("click", () => {
            goToStep(activeStep + 1);
            isPaused = true;
        });

        // Interactive action choice selection in Step 3
        const choices = container.querySelectorAll(".ph-story-choice");
        choices.forEach(choice => {
            choice.addEventListener("click", () => {
                choices.forEach(c => c.classList.remove("selected"));
                choice.classList.add("selected");
            });
        });

        // Auto walkthrough progression
        const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
        if (!prefersReducedMotion) {
            const startAutoRotation = () => {
                if (rotationTimer) clearInterval(rotationTimer);
                rotationTimer = setInterval(() => {
                    if (!isPaused) {
                        goToStep(activeStep + 1);
                    }
                }, 6000);
            };

            container.addEventListener("mouseenter", () => { isPaused = true; });
            container.addEventListener("mouseleave", () => { isPaused = false; });

            if ("IntersectionObserver" in window) {
                const observer = new IntersectionObserver((entries) => {
                    entries.forEach(entry => {
                        if (entry.isIntersecting) {
                            startAutoRotation();
                        } else {
                            if (rotationTimer) clearInterval(rotationTimer);
                        }
                    });
                }, { threshold: 0.25 });
                observer.observe(container);
            } else {
                startAutoRotation();
            }
        }
    }

    // Phase 8: 6-Stage Connected Pipeline Timeline Progression
    function bindPipelineProgression() {
        const grid = document.getElementById("pipeline-grid");
        if (!grid) return;

        const cards = grid.querySelectorAll(".ph-pipeline-card");
        if (!cards.length) return;

        cards.forEach((card, idx) => {
            card.addEventListener("click", () => {
                cards.forEach(c => c.classList.remove("is-active"));
                card.classList.add("is-active");
            });
        });

        const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
        if (!prefersReducedMotion && "IntersectionObserver" in window) {
            let activeIdx = 0;
            let interval = null;

            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        if (!interval) {
                            interval = setInterval(() => {
                                cards.forEach(c => c.classList.remove("is-active"));
                                cards[activeIdx].classList.add("is-active");
                                activeIdx = (activeIdx + 1) % cards.length;
                            }, 3200);
                        }
                    } else {
                        if (interval) {
                            clearInterval(interval);
                            interval = null;
                        }
                    }
                });
            }, { threshold: 0.3 });
            observer.observe(grid);
        }
    }

    // Phase 8: Product Showcase Tabs (6 Core Platform Capabilities)
    function bindProductShowcaseTabs() {
        const tabBtns = document.querySelectorAll(".ph-showcase-btn");
        if (!tabBtns.length) return;

        tabBtns.forEach(btn => {
            btn.addEventListener("click", () => {
                const targetId = btn.getAttribute("data-showcase-tab");
                if (!targetId) return;

                tabBtns.forEach(b => {
                    b.classList.remove("active");
                    b.setAttribute("aria-selected", "false");
                });
                btn.classList.add("active");
                btn.setAttribute("aria-selected", "true");

                const panes = document.querySelectorAll(".ph-showcase-pane");
                panes.forEach(pane => {
                    if (pane.id === targetId) {
                        pane.classList.add("active");
                    } else {
                        pane.classList.remove("active");
                    }
                });
            });
        });
    }

    // Phase 8: Channel Execution Switcher Tabs
    function bindChannelSwitcher() {
        const tabBtns = document.querySelectorAll(".ph-channel-tab-btn");
        if (!tabBtns.length) return;

        tabBtns.forEach(btn => {
            btn.addEventListener("click", () => {
                const targetId = btn.getAttribute("data-channel-tab");
                if (!targetId) return;

                tabBtns.forEach(b => {
                    b.classList.remove("active");
                    b.setAttribute("aria-selected", "false");
                });
                btn.classList.add("active");
                btn.setAttribute("aria-selected", "true");

                const panes = document.querySelectorAll(".ph-channel-pane");
                panes.forEach(pane => {
                    if (pane.id === targetId) {
                        pane.classList.add("active");
                    } else {
                        pane.classList.remove("active");
                    }
                });
            });
        });
    }

    // Boot
    document.addEventListener("DOMContentLoaded", () => {
        bindLangButtons();
        setLang(getLang());
        bindDemoForm();
        bindPageDemoForm();
        bindContactForm();
        bindCareerForm();
        bindHeroLivingCockpit();
        bindOpportunityStoryWalkthrough();
        bindPipelineProgression();
        bindProductShowcaseTabs();
        bindChannelSwitcher();
        initAOS();
        initSwiper();
        bindHeader();
        showToastIfAny();
        bindPreloader();
        bindMobileMenu();
    });
})();


