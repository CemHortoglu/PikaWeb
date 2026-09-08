# PIKA PUBLIC WIKI GOVERNANCE & TAXONOMY SPECIFICATION

> **Document Status:** Active Canonical Standard  
> **Last Updated:** September 2026  
> **Version:** 1.1 (Security Boundary & Taxonomy Convergence)  
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
   - Protected by `[Authorize(Policy = "InternalDocsAccess")]` requiring platform-level global administration privileges (`SuperAdmin` or `Super Admin` role).
   - Normal tenant accounts—including tenant administrators (`Admin`, `Teknik / Admin`, `Yönetici`), operators, and users—are strictly denied access (**403 Forbidden**).
   - Anonymous requests redirect (302) to `/Account/Login`.
   - Completely absent from public JSON, public navigation, public search, and `sitemap.xml`.

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
| **High-Risk Claims** | Enterprise-grade TLS, tenant isolation, KVKK / İYS consent checks | SOC 2, ISO 27001, SAML SSO guarantees, 99.99% uptime SLA, guaranteed revenue increases | All 88 articles verified clean |
| **Engineering Leakage** | Customer-safe terms (`kuyruk durumu ve başarısız iş inceleme havuzu`) | Internal code symbols, Hangfire, MediatR, DbContext, dead-letter queues | `gonderim-operasyonu-izleme`, `sss`, `sozluk` |

---

## 3. Public Wiki Complete Inventory (88 Articles)

### Category 1: Pika'yı Tanıyın (7 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `pika-nedir` | Pika Nedir? | Prospect / Customer | **INDEX** | Pika’nın ürün olarak ne olduğunu, hangi problemi çözdüğünü ve müşteriye hangi temel değeri sunduğunu açıklar. |
| `pika-ne-degildir` | Pika Ne Değildir? | Prospect / Customer | **INDEX** | Pika’nın yanlış kategorilenmesini önlemek için CRM, BI, AI ve gönderim araçlarından farkını açıklar. |
| `pika-konumu` | Pika’nın Konumu | Prospect / Customer | **INDEX** | Pika’nın CRM, BI, kampanya aracı ve AI asistanı gibi kategorilerle ilişkisini; fakat neden bunların hiçbirine tek başına indirgenemeyeceğini açıklar. |
| `pika-nasil-calisir` | Pika Nasıl Çalışır? | Prospect / Customer | **INDEX** | Pika’nın veriden aksiyona ve ölçüme uzanan uçtan uca çalışma modelini açıklar. |
| `kimler-icin` | Pika Kimler İçin? | Prospect / Customer | **INDEX** | Pika’dan hangi tür işletmelerin ve hangi ekiplerin daha fazla değer elde edebileceğini açıklar. |
| `urun-haritasi` | Pika Ürün Haritası | Prospect / Customer | **INDEX** | Pika Knowledge Base’in tamamında kullanılacak ana ürün haritasını ve modüllerin birbirleriyle ilişkisini açıklar. |
| `bilgi-bankasi-haritasi` | Bilgi Bankası Haritası | Prospect / Customer | **INDEX** | Bu bilgi bankasının Pika’yı hangi sırayla anlattığını ve her bölümün ürün hikâyesindeki yerini gösterir. |

### Category 2: Müşteriyi ve Ürünü Anlayın (17 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `pika-360` | Pika 360 | Prospect / Customer | **INDEX** | Tek bir müşterinin değer, risk, iletişim erişimi ve açık fırsat bilgilerini bir araya getiren müşteri karar özetini açıklar. |
| `customer-intelligence-nedir` | Customer Intelligence ve Müşteri Analitiği | Prospect / Customer | **INDEX** | Müşteri alışveriş ritmi, değer segmentleri ve davranış eğilimlerinin nasıl analiz edildiğini açıklar. |
| `musteri-degeri-sadakat` | Müşteri Değeri ve Sadakat | Prospect / Customer | **INDEX** | Müşteri değer skoru (CVS), sadakat, risk ve davranış sinyallerinin birbirinden ayrı ama birlikte nasıl okunacağını açıklar. |
| `musteri-deger-skoru` | Müşteri Değer Skoru | Prospect / Customer | **INDEX** | Müşteri Değer Skoru'nun (CVS) 4 temel faktörünü (Monetary, Frequency, Recency, Loyalty), ağırlıklı hesaplama mantığını ve nasıl yorumlanacağını açıklar. |
| `sadakat-hedefe-yakinlik` | Sadakat ve Hedefe Yakınlık | Prospect / Customer | **INDEX** | Loyalty point/tier bilgisi ile müşterinin bir üst sadakat eşiğine yakınlığının nasıl daha akıllı aksiyonlara dönüştürülebileceğini açıklar. |
| `deger-risk-birlikte-okuma` | Değer ve Riski Birlikte Okumak | Prospect / Customer | **INDEX** | Müşteri değeri ile pasifleşme/kayıp riskini tek eksende değil birlikte değerlendirerek farklı aksiyon öncelikleri oluşturmayı açıklar. |
| `tekrar-satin-alma-analizi` | Tekrar Satın Alma Analizi | Prospect / Customer | **INDEX** | Müşterinin geçmiş alışveriş döngüsü penceresine (%80–%120 aralığı) göre zamanı yaklaşan ve geciken tekrar satın alma ihtiyaçlarının nasıl görünür hale getirildiğini açıklar. |
| `product-intelligence-nedir` | Product Intelligence Nedir? | Prospect / Customer | **INDEX** | Pika’nın ürünleri yalnız katalog kaydı değil, müşteri ihtiyacı ve ticari rol bağlamında nasıl anlamlandırdığını açıklar. |
| `playbook-sektorel-anlam` | Playbook: Sektöre Göre Ürün Dili | Prospect / Customer | **INDEX** | Playbook yöneticisinin sektör bazlı ürün dilini, versiyonlamayı ve tanım katmanını nasıl yönettiğini gösterir. |
| `need-group-product-role` | Need Group ve Product Role | Prospect / Customer | **INDEX** | Ürünün hangi müşteri ihtiyacını temsil ettiğini ve ticari/öneri sistemindeki görevini birbirinden ayırır. |
| `dinamik-siniflandirma-alanlari` | Dinamik Sınıflandırma Alanları | Operator / Admin | **NOINDEX** | Playbook’a özel alan ve seçeneklerin ürünleri daha ayrıntılı fakat kontrollü biçimde anlamlandırmak için nasıl kullanıldığını açıklar. |
| `kategori-playbook-baglantisi` | Kategori ve Playbook Bağlantısı | Operator / Admin | **NOINDEX** | Kategori binding’in Product Intelligence’da doğru alan, Need Group ve Product Role bağlamını ürünlere nasıl taşıdığını açıklar. |
| `urun-siniflandirma-workbench` | Ürün Sınıflandırma Çalışma Alanı | Operator / Admin | **NOINDEX** | Tenant Product Classification Workbench’in ürünleri filtreleme, sınıflandırma, uyarı ve onay süreçlerinde nasıl konumlandığını açıklar. |
| `master-urun-anlamlandirmalari` | Master Ürün Anlamlandırmaları | Operator / Admin | **NOINDEX** | Playbook kapsamında master ürünlere varsayılan ihtiyaç grubu, ürün rolü ve alan değerlerinin nasıl bağlandığını gösterir. |
| `review-resolution-readiness` | Review, Resolution ve Hazırlık | Operator / Admin | **NOINDEX** | Ürün sınıflandırma review akışı ile ham satış satırlarının gerçek ürünle eşleştirilmesi arasındaki farkı açıklar. |
| `product-intelligence-musteri-firsati` | Ürün Zekâsından Müşteri Fırsatına | Prospect / Customer | **INDEX** | Product Intelligence çıktılarının Pika 360, tekrar satın alma, cross-sell ve aksiyon kararlarını nasıl beslediğini açıklar. |
| `ai-musteri-ozeti` | AI Müşteri Özeti | Prospect / Customer | **INDEX** | Pika’nın ürettiği müşteri verisini daha okunur ve yorumlanabilir hale getiren AI açıklama katmanını anlatır. |

### Category 3: Fırsat ve Karar (8 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `gunun-firsatlari-ve-karar-motoru` | Günün Fırsatları ve Karar Motoru | Prospect / Customer | **INDEX** | Günlük olarak hesaplanan tekrar satın alma, geri kazanım ve çapraz satış fırsatlarının tek ekranda nasıl yönetildiğini açıklar. |
| `segmentasyon-ve-firsatlar` | Segmentasyon ve Fırsatlar | Prospect / Customer | **INDEX** | Segmentlerin yalnız hedef kitle listesi değil, davranış ve fırsat sinyallerini yönetmenin temel katmanı olduğunu açıklar. |
| `firsat-turleri` | Fırsat Türleri | Prospect / Customer | **INDEX** | Pika’nın müşteri ve ürün verisinden çıkarabileceği temel ticari fırsat türlerini tek çerçevede açıklar. |
| `cross-sell-firsatlari` | Cross-sell / Çapraz Satış Analizi | Prospect / Customer | **INDEX** | Sepet birliktelikleri üzerinden müşterilere en uygun tamamlayıcı ürün önerilerinin nasıl tespit edildiğini açıklar. |
| `upsell-firsatlari` | Upsell / Yükseltme Bağlamı | Prospect / Customer | **INDEX** | Güncel Pika’da upsell’in Product Intelligence ürün rolleriyle nasıl tanımlandığını ve dedicated başarı-olasılığı motorundan nasıl ayrıldığını açıklar. |
| `next-best-action` | Next Best Action, Kanal ve Zaman | Prospect / Customer | **INDEX** | Müşteri için en doğru aksiyonun, iletişim kanalının ve zamanlama bağlamının deterministik kurallarla nasıl belirlendiğini açıklar. |
| `firsat-guveni-kanit` | Fırsat Güveni ve Kanıt | Prospect / Customer | **INDEX** | Pika’nın önerileri kesin gerçek gibi sunmak yerine, kanıtın gücü ve veri yeterliliğiyle birlikte değerlendirme yaklaşımını açıklar. |
| `firsattan-aksiyona-gecis` | Fırsattan Aksiyona Geçiş | Prospect / Customer | **INDEX** | Analitik sinyalin neden doğrudan mesaj anlamına gelmediğini; iş kuralı, izin, kanal, zamanlama ve insan kontrolüyle aksiyona nasıl dönüştüğünü açıklar. |

### Category 4: Aksiyon ve Otomasyon (12 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `kampanya-journey-orkestrasyonu` | Journey, Kampanya ve Orkestrasyon Katmanı | Prospect / Customer | **INDEX** | Pika’nın içgörüden aksiyona geçiş katmanını; AI kampanya asistanı, e-posta tasarımı, Journey, mağazalar ve segment şablonlarıyla birlikte açıklar. |
| `kampanya-yoneticisi-ve-kurgular` | Campaign Manager ve Kampanya Kurguları | Prospect / Customer | **INDEX** | Hedef kitle seçimi, kanal belirleme, şablon eşleme ve zamanlanmış kampanya yönetimini açıklar. |
| `journey-tasarim-tuvali` | Journey Tasarım Tuvali | Operator / Admin | **NOINDEX** | Tetikleyici, karar, bekleme, gönderim ve güncelleme adımlarının bir araya getirildiği otomasyon tasarım ekranını açıklar. |
| `journey-karar-kurallari` | Journey İçinde Karar ve Dallanma Kuralları | Operator / Admin | **NOINDEX** | Journey akışında kişilerin davranışına göre evet/hayır veya zaman aşımı üzerinden nasıl dallandırıldığını açıklar. |
| `journey-store` | Journey Store | Operator / Admin | **NOINDEX** | Hazır otomasyon Journey’lerinin kategori ve amaç bazında yeniden kullanılabildiği Journey şablon mağazasını açıklar. |
| `email-template-editor` | Email Template Editor | Operator / Admin | **NOINDEX** | Kurumsal e-posta şablonlarının bileşen bazlı olarak hazırlanabildiği tasarım ekranını açıklar. |
| `email-store` | Email Store | Operator / Admin | **NOINDEX** | Hazır e-posta şablonlarının kategori, popülerlik ve kullanım senaryolarına göre sunulduğu şablon mağazasını açıklar. |
| `pika-pilot-ai-kampanya-asistani` | Pika Pilot AI Kampanya Asistanı | Prospect / Customer | **INDEX** | Doğal dille kampanya fikri, içerik taslağı ve kanal bazlı ilk kurgunun nasıl oluşturulabildiğini gösterir. |
| `icerik-studyosu-ve-gorsel-yonetimi` | Content Studio ve İçerik Tasarımı | Prospect / Customer | **INDEX** | E-posta, SMS ve WhatsApp için kurumsal içerik şablonlarının, görsel varlıkların ve metin taslaklarının yönetimini açıklar. |
| `segment-sablonlari` | Segment Şablonları | Operator / Admin | **NOINDEX** | Tekrar kullanılan hedefleme mantıklarının parametreli ve yayınlanabilir segment şablonlarına dönüştürülebilmesini açıklar. |
| `aksiyon-calisma-alani` | Aksiyon Çalışma Alanı | Operator / Admin | **NOINDEX** | Müşteri için oluşturulan aksiyonların, kanal bilgisinin ve sonuç kayıtlarının tek alanda nasıl yönetildiğini açıklar. |
| `yayinlama-sablon-ve-yonetim` | Yayınlama, Şablonlaştırma ve Operasyonel Yönetim | Operator / Admin | **NOINDEX** | Taslak oluşturma, kontrol, yayınlama, versiyon ve tekrar kullanım mantığını ürün operasyonu açısından açıklar. |

### Category 5: Kanallar ve İzinler (5 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `kampanya-kanallari-ve-rol-dagilimi` | Kampanya Kanalları ve Rol Dağılımı | Prospect / Customer | **INDEX** | E-posta, SMS, WhatsApp ve Journey katmanlarının ürün içinde hangi role hizmet ettiğini açıklar. |
| `kanal-operasyonlari` | E-posta, SMS ve WhatsApp Operasyonları | Operator / Admin | **NOINDEX** | Pika’nın aksiyon katmanındaki e-posta, SMS ve WhatsApp kanallarını operasyonel ama ikincil bir uygulama katmanı olarak açıklar. |
| `izin-kanal-zamanlama` | İzin, Kanal Uygunluğu ve Zamanlama | Prospect / Customer | **INDEX** | Bir müşteriye aksiyon üretmeden önce iletişim izni, erişilebilir kanal ve doğru zamanın neden birlikte değerlendirilmesi gerektiğini açıklar. |
| `izin-optout-iys` | İzinler, Opt-out ve İYS | Prospect / Customer | **INDEX** | Kanal uygunluğu, opt-out, WhatsApp opt-in ve İYS entegrasyonunun Pika’daki rolünü; teknik uygunluk ile hukuki sorumluluk arasındaki sınırı açıklar. |
| `iletisim-listeleri-ve-opt-out` | İletişim Listeleri ve Tercih Yönetimi | Operator / Admin | **NOINDEX** | İletişim listeleri, müşteri izinleri, e-posta abonelikten çıkma (opt-out) ve engelleme listelerinin yönetimini açıklar. |

### Category 6: Veri ve Entegrasyon (12 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `veri-entegrasyon-genel` | Veri ve Entegrasyona Genel Bakış | Prospect / Customer | **INDEX** | Pika’nın müşteri ve ticari veriyi neden karar zincirinin başlangıcı olarak gördüğünü açıklar. |
| `pika-hangi-verileri-kullanir` | Pika Hangi Verileri Kullanır? | Prospect / Customer | **INDEX** | Excel aktarım ekranında görülen müşteri, ürün, belge ve satış alanlarını iş anlamıyla açıklar. |
| `excel-csv-aktarimi` | Excel / CSV ile Veri Aktarımı | Operator / Admin | **NOINDEX** | Mevcut satış verisinin dosya üzerinden Pika’ya alınabildiği kontrollü aktarım akışını açıklar. |
| `kolon-eslestirme` | Kolon Eşleştirme | Operator / Admin | **NOINDEX** | Firmanın kendi kolonlarının Pika’daki müşteri, ürün, belge ve satış alanlarıyla nasıl eşleştirildiğini açıklar. |
| `veri-dogrulama-kalite` | Veri Doğrulama ve Kalite Kontrolü | Operator / Admin | **NOINDEX** | Pika’nın Excel satırlarını yalnız yüklemek yerine veri geçerliliği ve işlem üretilebilirliği açısından ayrı ayrı kontrol ettiğini gösterir. |
| `ice-aktarma-sonuclari` | İçe Aktarma Sonuçları ve Satır İnceleme | Operator / Admin | **NOINDEX** | Satır bazlı durumların neden kullanıcıya açık biçimde gösterildiğini ve veri alma ile işlem üretme arasındaki farkı açıklar. |
| `fatura-siparis-neden-onemli` | Fatura ve Sipariş Verisi Neden Önemli? | Prospect / Customer | **INDEX** | Pika’nın yalnız ürün listesinden değil gerçek satın alma satırlarından neden daha fazla anlam çıkarabildiğini açıklar. |
| `satis-veri-operasyonlari` | Satış Veri Operasyonları | Operator / Admin | **NOINDEX** | Satış verisinin Pika’ya nasıl alındığını, işlendiğini ve veri kalitesi görünürlüğüyle nasıl yönetildiğini gösterir. |
| `veri-sonrasi` | Veri Pika’ya Geldikten Sonra Ne Olur? | Prospect / Customer | **INDEX** | Doğrulanmış ticari verinin müşteri ve ürün zekâsı katmanlarına nasıl bağlandığını kavramsal olarak açıklar. |
| `veri-hazirligi-guvenilirlik` | Veri Kalitesi ve Analitik Güvenilirlik | Operator / Admin | **NOINDEX** | Pika Data Quality katmanındaki gerçek issue kodlarını, severity/status, etkilenen oran, freshness, samples ve remediation yaklaşımını açıklar. |
| `ozellik-veri-gereksinimleri` | Özellik → Veri Gereksinimi Matrisi | Prospect / Customer | **INDEX** | Pika’daki ana analitik ve aksiyon kabiliyetlerinin çalışması için gereken minimum veri bağlamını ve eksik veri olduğunda ne olacağını özetler. |
| `api-entegrasyonu` | API Entegrasyonu | Prospect / Customer | **INDEX** | Düzenli veri akışı gerektiğinde Pika’nın mevcut iş sistemleriyle API üzerinden nasıl konumlandığını açıklar. |

### Category 7: Kullanım Rehberleri (8 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `gmail-kisi-aktarimi` | Kişi Aktarımı: Gmail ve Outlook | Operator / Admin | **NOINDEX** | Gmail üzerindeki hazır kişilerin seçilerek Pika rehberine aktarılabildiği kişi kazanım ekranını açıklar. |
| `kisi-listesi-ve-segmentler` | Kişi Listesi ve Segmentler | Operator / Admin | **NOINDEX** | Müşteri verisinin yalnızca kayıt değil, segment, grup ve iletişim uygunluğu bağlamıyla nasıl yönetildiğini açıklar. |
| `segment-yonetimi-ve-filtreler` | Dinamik Segment Oluşturma ve Kural Filtreleri | Operator / Admin | **NOINDEX** | Audience Manager üzerinde davranışsal, demografik ve işlem bazlı dinamik segmentlerin nasıl oluşturulacağını adım adım anlatır. |
| `kategori-yonetimi` | Kategori Yönetimi | Operator / Admin | **NOINDEX** | Ürün zekâsının temeli olan kategori yapısının ve ürün hiyerarşisinin Pika içinde nasıl yönetildiğini gösterir. |
| `kurulum-baslangic-modeli` | Kurulum ve Başlangıç Modeli | Prospect / Customer | **INDEX** | Pika’nın müşteride nasıl kademeli kurulduğunu; veri keşfinden ilk analitiklere, pilot kullanımdan düzenli entegrasyona uzanan başlangıç modelini açıklar. |
| `kullanici-roller-yetkiler` | Kullanıcılar, Roller ve Yetkiler | Operator / Admin | **NOINDEX** | Pika kullanımının farklı ekipler arasında nasıl ayrıştırılabileceğini ve yetki modelinin neden önemli olduğunu açıklar. |
| `sektorel-kullanim-ornekleri` | Sektörel Kullanım Örnekleri | Prospect / Customer | **INDEX** | Pika’nın aynı analitik ve karar zincirini farklı sektörlerde nasıl farklı problem ve ürün diliyle kullandığını açıklar. |
| `ornek-kullanim-senaryolari` | Örnek Kullanım Senaryoları | Prospect / Customer | **INDEX** | Journey, kampanya ve AI katmanlarının birlikte nasıl çalıştığını üç kısa örnek üzerinden gösterir. |

### Category 8: Ölçüm ve Analitik (13 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `bi-kokpit` | BI Kokpit | Prospect / Customer | **INDEX** | Yönetim seviyesinde karar vermeye hazır temel müşteri ve satış özetlerinin nasıl sunulduğunu açıklar. |
| `satis-kanali-performansi` | Satış Kanalı Performansı | Prospect / Customer | **INDEX** | Pika BI’ın mağaza, web, e-ticaret gibi satış kanallarını revenue, basket, customer, repeat rate ve overlap metrikleriyle nasıl karşılaştırdığını açıklar. |
| `magaza-performansi` | Mağaza Performansı | Prospect / Customer | **INDEX** | Pika BI mağaza performansında ciro, müşteri, sepet, tekrar oranı, dönem karşılaştırması ve veri kapsama metriklerini açıklar. |
| `magaza-performans-skoru` | Mağaza Karşılaştırması ve Sıralamalar | Prospect / Customer | **INDEX** | Mağazaların ciro, büyüme, sepet büyüklüğü ve müşteri sadakati açısından nasıl karşılaştırıldığını açıklar. |
| `magaza-musteri-davranisi` | Mağaza Bazlı Müşteri Davranışı | Prospect / Customer | **INDEX** | Mağazaların yalnız satış rakamlarıyla değil, müşteri sıklığı, sepet, tekrar satın alma ve segment yapısıyla nasıl okunabileceğini açıklar. |
| `magaza-firsat-alanlari` | Mağaza Bazlı Fırsat Alanları | Prospect / Customer | **INDEX** | Mağaza performansındaki zayıf veya güçlü sinyallerin aksiyon fırsatlarına nasıl dönüştürülebileceğini açıklar. |
| `urun-kategori-performansi` | Ürün ve Kategori Performansı | Prospect / Customer | **INDEX** | Pika BI’daki ürün/kategori performansının doğrulanmış metriklerini, dönem karşılaştırmasını ve veri kalitesi göstergelerini açıklar. |
| `kampanya-performansi-ve-olcumleme` | Kampanya Performansı ve Ölçümleme Mantığı | Prospect / Customer | **INDEX** | Gönderim ve etkileşim sonuçlarını, kampanya sonrası satış ilişkilendirmesini ve attribution ile nedensellik arasındaki sınırı açıklar. |
| `omnichannel-performansi` | İletişim Kanallarını Birlikte Okumak | Prospect / Customer | **INDEX** | E-posta, SMS ve WhatsApp iletişim operasyonlarının tek bir satış-kanalı BI ekranıyla karıştırılmadan nasıl birlikte değerlendirileceğini açıklar. |
| `teslimat-konsolu` | Görevler ve Teslimat Takibi | Operator / Admin | **NOINDEX** | Gönderim operasyonlarının durumunu, kampanya teslimat süreçlerini ve operasyonel görünürlüğü müşteri odaklı olarak açıklar. |
| `basarisiz-yeniden-deneme` | Başarısız Gönderimler ve İletişim Güvenliği | Operator / Admin | **NOINDEX** | Geçici ve kalıcı gönderim engellerinin nasıl ayrıldığını, izin ve opt-out güvenliğini açıklar. |
| `gonderim-operasyonu-izleme` | Gönderim Operasyonu ve İzleme | Operator / Admin | **NOINDEX** | Gönderimlerin durumunu, kanala iletilme sürecini, bekleyen veya tamamlanan işlerin operasyonel görünürlüğünü açıklar. |
| `olcum-ogrenme-dongusu` | Ölçüm ve Öğrenme Döngüsü | Prospect / Customer | **INDEX** | Pika’nın veri → karar → aksiyon → sonuç zincirini nasıl kapattığını ve sonuçların sonraki kararları neden beslemesi gerektiğini açıklar. |

### Category 9: SSS ve Kaynaklar (5 Articles)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `ihtiyac-haritasi` | Hangi İhtiyacım Varsa Pika’da Nereye Bakmalıyım? | Prospect / Customer | **INDEX** | İş sorusundan doğru Pika özelliğine hızlı geçiş sağlayan karar haritasıdır. |
| `sss` | Sık Sorulan Sorular | Prospect / Customer | **INDEX** | Müşteri, satış, kurulum ve operasyon ekiplerinin Pika hakkında en sık soracağı ürün, veri, AI, BI, kanal ve ölçüm sorularını yanıtlar. |
| `sozluk` | Pika Sözlüğü | Prospect / Customer | **INDEX** | Knowledge Base boyunca kullanılan ürün, veri, BI, Product Intelligence, kampanya ve delivery terimlerini ortak bir dille açıklar. |
| `en-iyi-uygulamalar` | En İyi Uygulamalar | Prospect / Customer | **INDEX** | Pika’yı yüksek veri kalitesi, müşteri saygısı ve ölçülebilir sonuçlarla kullanmak için temel çalışma ilkelerini toplar. |
| `ai-rolu-guven-siniri` | AI'ın Rolü ve Güven Sınırı | Prospect / Customer | **INDEX** | Pika'da yapay zekânın karar verici değil, analitik hesaplamaları açıklayan ve içerik üreten güvenilir bir yardımcı olduğunu açıklar. |

### Off-Navigation Technical Page (1 Article)
| Slug | Title | Audience | Classification | Rationale |
| :--- | :--- | :--- | :--- | :--- |
| `gonderim-son-katman` | Gönderim: Karar Zincirinin Son Katmanı | Operator / Technical | **NOINDEX** | Pika’da e-posta, SMS ve WhatsApp gönderiminin müşteri zekâsından sonra gelen uygulama katmanı olduğunu ve gönderim ön koşullarını açıklar. |

---

## 4. Quarantined Internal Engineering Wiki (15 Articles)

The following articles contain proprietary engineering designs, database entity relationships, worker architectures, and deployment pipelines. They are quarantined under `/internal/wiki/` and served via `InternalWikiController` requiring platform global administration privileges with policy `InternalDocsAccess` (`[Authorize(Policy = "InternalDocsAccess")]`):

1. `internal-ai-mimarisi-ve-prompt-yonetimi`: LLM Provider Routing, Prompt Şablonları ve Fallback Mekanizmaları - AI Kampanya Asistanı ve AI Müşteri Özeti için model yönlendirme (OpenAI / Anthropic / Gemini), prompt versiyonlama, token limitleri ve güvenlik filtreleri.
2. `internal-api-mimarisi-ve-veri-kontratlari`: Ingestion API Mimarisi, Rate Limiting ve Idempotency Kontratları - API veri alım uç noktaları, batch ingestion formatları, HMAC imzalama, rate limiting algoritmaları ve idempotency key yönetimini açıklar.
3. `internal-cce-karar-motoru-mimarisi`: Customer Context Engine (CCE) Kural Ağaçları ve Arbitrasyon Ağırlıkları - Next Best Action, Next Best Channel ve Next Best Time sinyallerinin deterministik arbitrasyon kuralları, çakışma çözümleri ve öncelik sıralaması.
4. `internal-cross-sell-sepet-analizi-motoru`: Cross-Sell Birliktelik Analizi Algoritması (Support, Confidence, Lift) - Sepet birliktelik kural motoru, Apriori tabanlı hesaplama parametreleri, transaction windowing ve minimum eşik değerleri.
5. `internal-gonderim-izleme-ve-telemetri`: Dağıtık Gönderim Telemetrisi ve Sağlık Kontrolleri - Kuyruk gecikme metrikleri, Hangfire worker havuzu izleme, Prometheus/OpenTelemetry sayaçları ve alarm eşikleri.
6. `internal-guvenlik-ve-yetkilendirme-mimarisi`: Kimlik Doğrulama, Cookie Güvenliği, Tenant İzolasyonu ve RBAC Yetki Ağacı - ASP.NET Core Cookie kimlik doğrulaması, JWT claim dönüşümleri, cross-tenant veri izolasyonu ve rol/yetki matrisi.
7. `internal-magaza-metrik-hesaplama-mimarisi`: BI Snapshot Aggregation ve Mağaza Metrik Hesaplama Mimarisi - Mağaza ciro, büyüme, sepet, repeat rate ve kapsama hesaplamalarının snapshot pipeline'ı ve aggregate tabloları.
8. `internal-mimari-genel-bakis`: Pika Platform Mimarisi ve Servis Sınırları - Pika'nın monolitik web katmanı, arka plan işleyicileri (Hangfire/Worker), veri tabanı modelleri ve servis sınırlarını açıklar.
9. `internal-musteri-deger-skoru-algoritmasi`: Customer Value Score Katsayı Matrisi ve Normalizasyon Formülleri - Customer Value Score'un 0-100 ölçeğindeki deterministik ağırlık katsayıları, percentile normalizasyonu ve logaritmik harcama skoru formülü.
10. `internal-operasyon-ve-runbook`: Dağıtım Prosedürleri, Ortam Konfigürasyonları ve Hata Çözüm Runbook'ları - CI/CD pipeline'ları, veritabanı migration adımları, Cloudflare önbellek yönetimi ve sık karşılaşılan üretim ortamı hata senaryoları.
11. `internal-product-intelligence-resolution-mimarisi`: Product Intelligence Resolution Engine ve Master Product Eşleştirme Pipeline'ı - Ham ürün isimlerinin temizlenmesi, alias eşleme, fuzzy matching, playbook bağlama ve readiness kontrol aşamaları.
12. `internal-retry-politikasi-ve-hata-yonetimi`: Retry Politikası, Exponential Backoff ve Hata Normalizasyonu - Geçici ve kalıcı sağlayıcı hata kodlarının sınıflandırılması, exponential backoff katsayıları, jitter hesaplaması ve devre kesici (circuit breaker) politikası.
13. `internal-surum-ve-gecis-notlari`: Sürüm Geçiş Notları, Veritabanı Migrasyonları ve Mühendislik Backlog Durumu - Faz geçişleri, veritabanı şema değişiklik geçmişi ve teknik borç / mimari backlog durum özeti.
14. `internal-teslimat-konsolu-ve-worker-mimarisi`: Delivery Workers, Job/Attempt/Event Modeli ve Dispatcher Mimarisi - Gönderim işlerinin arka plan worker havuzları, Job, Attempt ve Event durum makineleri, Dead-letter kuyruğu ve Dispatcher orkestrasyonunu detaylandırır.
15. `internal-veri-kalitesi-ve-anomali-denetimi`: Data Quality Kural Motoru, Severity Hesaplamaları ve Freshness Scheduler - DQ Engine'in 24 kural kodu, severity ağırlıkları, freshness kontrol periyotları ve anomali tespiti.

---

## 5. Governance Enforcement & Automated Regression Testing

To prevent regression or accidental drift, the following automated tests in `Pika.Web.Tests/WikiTests.cs` and `Pika.Web.Tests/WikiSplitGenerator.cs` enforce this specification:

- `AllSitemapWikiUrls_Return200OK`: Asserts exactly 59 Wiki URLs in `sitemap.xml` (root + 58 INDEX articles) and verifies all 59 return 200 OK without truncation.
- `InternalWikiSecurity_AnonymousUser_RedirectsToLogin`: Asserts anonymous access to `/internal/wiki/` and internal articles redirects to `/Account/Login`.
- `InternalWikiSecurity_TenantUser_Returns403Forbidden`: Asserts authenticated tenant users (`User`, `Operator`) receive 403 Forbidden on internal wiki routes.
- `InternalWiki_TenantAdmin_CannotAccessInternalDocumentation`: Asserts tenant administrators carrying `Admin`, `Teknik / Admin`, or `Yönetici` roles are strictly forbidden (403 Forbidden) from accessing internal routes.
- `InternalWikiSecurity_PrivilegedSuperAdmin_Returns200OK`: Asserts platform global administrators (`SuperAdmin`, `Super Admin`) with `InternalDocsAccess` receive 200 OK on internal wiki routes.
- `AccountJwt_ContractMapsToInternalDocsAccess`: Proves upstream JWT role claims mapped by `AccountController` are evaluated by `InternalDocsAccess` policy.
- `GovernanceDocument_MatchesGeneratedWikiData`: Programmatically parses `WIKI_PUBLIC_GOVERNANCE.md` and validates 1-to-1 slug, title, and indexability parity against `wiki.json`.
- `Generator_IsIdempotent_SecondRunProducesZeroDrift`: Proves in-memory second-run generation produces identical serialized data with zero drift.
- `ClaimSafety_PublicWikiContainsNoProhibitedTerms`: Verifies complete absence of compliance absolutes, guarantees, unauthorized streaming claims, or fake predictions across all public articles.
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

