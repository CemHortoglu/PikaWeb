using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Pika.Models;
using Xunit;

namespace Pika.Web.Tests
{
    public class WikiSplitGenerator
    {
        [Fact]
        public void GenerateWikis()
        {
            var baseDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            var appDataDir = Path.Combine(baseDir, "App_Data");
            var sourceJsonPath = Path.Combine(appDataDir, "wiki.json");
            var publicJsonPath = Path.Combine(appDataDir, "wiki.json");
            var internalJsonPath = Path.Combine(appDataDir, "internal_wiki.json");

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            var originalJson = File.ReadAllText(sourceJsonPath);
            var originalWiki = JsonSerializer.Deserialize<WikiData>(originalJson, jsonOptions)!;

            // 1. BUILD INTERNAL WIKI
            var internalWiki = BuildInternalWiki(originalWiki);

            // 2. BUILD PUBLIC WIKI
            var publicWiki = BuildPublicWiki(originalWiki);

            // 3. EXPLICIT REGENERATION (Only when REGENERATE_WIKI=1 is provided)
            if (string.Equals(Environment.GetEnvironmentVariable("REGENERATE_WIKI"), "1", StringComparison.OrdinalIgnoreCase))
            {
                var internalJson = JsonSerializer.Serialize(internalWiki, jsonOptions);
                File.WriteAllText(internalJsonPath, internalJson);

                var publicJson = JsonSerializer.Serialize(publicWiki, jsonOptions);
                File.WriteAllText(publicJsonPath, publicJson);

                CleanStaticAppJs(baseDir, publicWiki);
            }

            // 4. TEST VALIDATION: Validate semantic structure without mutating tracked files
            Assert.True(File.Exists(internalJsonPath), "Internal wiki JSON must exist on disk");
            Assert.True(File.Exists(publicJsonPath), "Public wiki JSON must exist on disk");
            Assert.NotNull(internalWiki.Pages);
            Assert.NotEmpty(internalWiki.Pages);
            Assert.NotNull(publicWiki.Pages);
            Assert.NotEmpty(publicWiki.Pages);

            // Assert internal pages are not leaked into public wiki
            Assert.DoesNotContain("internal-mimari-genel-bakis", publicWiki.Pages.Keys);
            Assert.DoesNotContain("internal-api-mimarisi-ve-veri-kontratlari", publicWiki.Pages.Keys);

            // Assert core public wiki pages exist in generated public structure
            Assert.Contains("pika-nedir", publicWiki.Pages.Keys);
            Assert.Contains("pika-360", publicWiki.Pages.Keys);

            // Assert navigation structures are populated
            Assert.NotEmpty(internalWiki.Nav);
            Assert.NotEmpty(publicWiki.Nav);
        }

        private static WikiData BuildInternalWiki(WikiData original)
        {
            var internalWiki = new WikiData();

            // Internal Navigation categories
            internalWiki.Nav = new List<List<object>>
            {
                new List<object> { "Mimari ve Servis Sınırları", new List<string> {
                    "internal-mimari-genel-bakis",
                    "internal-api-mimarisi-ve-veri-kontratlari"
                }},
                new List<object> { "Delivery & Worker Mimarisi", new List<string> {
                    "internal-teslimat-konsolu-ve-worker-mimarisi",
                    "internal-retry-politikasi-ve-hata-yonetimi",
                    "internal-gonderim-izleme-ve-telemetri"
                }},
                new List<object> { "Veri & BI Dahili Motorları", new List<string> {
                    "internal-veri-kalitesi-ve-anomali-denetimi",
                    "internal-musteri-deger-skoru-algoritmasi",
                    "internal-magaza-metrik-hesaplama-mimarisi",
                    "internal-cross-sell-sepet-analizi-motoru"
                }},
                new List<object> { "Product Intelligence Motoru", new List<string> {
                    "internal-product-intelligence-resolution-mimarisi"
                }},
                new List<object> { "CCE Karar & Fırsat Motoru", new List<string> {
                    "internal-cce-karar-motoru-mimarisi"
                }},
                new List<object> { "AI Altyapısı & Prompt Yönetimi", new List<string> {
                    "internal-ai-mimarisi-ve-prompt-yonetimi"
                }},
                new List<object> { "Güvenlik, Operasyon & Sürüm", new List<string> {
                    "internal-guvenlik-ve-yetkilendirme-mimarisi",
                    "internal-operasyon-ve-runbook",
                    "internal-surum-ve-gecis-notlari"
                }}
            };

            // Article 1: internal-mimari-genel-bakis
            internalWiki.Pages["internal-mimari-genel-bakis"] = new WikiPage
            {
                Slug = "internal-mimari-genel-bakis",
                Section = "Mimari ve Servis Sınırları",
                Title = "Pika Platform Mimarisi ve Servis Sınırları",
                Summary = "Pika'nın monolitik web katmanı, arka plan işleyicileri (Hangfire/Worker), veri tabanı modelleri ve servis sınırlarını açıklar.",
                Related = new List<string> { "internal-api-mimarisi-ve-veri-kontratlari", "internal-teslimat-konsolu-ve-worker-mimarisi", "internal-guvenlik-ve-yetkilendirme-mimarisi" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Pika Platform Mimarisi ve Servis Sınırları</h1>
<p class=""hero-lead"">Bu doküman Pika'nın çekirdek mimarisini, servis katmanlarını, veri akış pipeline'larını ve bileşenler arası bağımlılıkları tanımlar.</p>
</div>
</section>

<div class=""lead-note"">
<span class=""eyebrow"">Mühendislik Kapsamı</span>
<p>Pika mimarisi, yüksek hacimli B2B/B2C müşteri ve satış hareketlerini normalize eden, analitik zekâ modellerini hesaplayan ve çok kanallı iletişim operasyonlarını orkestre eden dağıtık modüllerden oluşur.</p>
</div>

<h2>1. Çekirdek Katmanlar ve Servis Sınırları</h2>
<p>Platform 4 ana mimari katmana ayrılmıştır:</p>
<div class=""feature-grid"">
<article class=""feature-card"">
<div class=""feature-icon"">WEB</div>
<h3>Web &amp; API Gateway</h3>
<p>ASP.NET Core tabanlı public MVC, Angular SPA host ve REST Ingestion API endpoint'leri. Cookie ve Bearer JWT hibrit kimlik doğrulama sağlar.</p>
</article>
<article class=""feature-card"">
<div class=""feature-icon"">ENG</div>
<h3>Analitik &amp; Karar Motorları</h3>
<p>Customer Intelligence, Product Intelligence, CCE (Customer Context Engine) ve BI snapshot üreticileri. Batch ve event-driven çalışır.</p>
</article>
<article class=""feature-card"">
<div class=""feature-icon"">WRK</div>
<h3>Delivery &amp; Background Workers</h3>
<p>Hangfire ve IHostedService tabanlı dağıtık worker kuyrukları. Kampanya gönderimleri, webhook dispatcher ve senkronizasyon işlerini yürütür.</p>
</article>
<article class=""feature-card"">
<div class=""feature-icon"">DB</div>
<h3>Veri Katmanı &amp; Snapshot Store</h3>
<p>İlişkisel operasyonel veri tabanı (PostgreSQL / SQL Server), read-model snapshot'ları ve audit event logları.</p>
</article>
</div>

<h2>2. Veri Akış Pipeline'ı</h2>
<div class=""story-flow"">
<div class=""story-step""><div class=""story-num"">1</div><div class=""story-content""><h3>Ingestion (Veri Alımı)</h3><p>Excel/CSV yüklemeleri veya Ingestion API üzerinden ham veri alınır. Satır düzeyinde şema validasyonu yapılır.</p></div></div>
<div class=""story-step""><div class=""story-num"">2</div><div class=""story-content""><h3>Normalization &amp; Resolution</h3><p>Müşteri tekilleştirme (hash/phone/email) ve ürün master eşleştirme motoru devreye girer.</p></div></div>
<div class=""story-step""><div class=""story-num"">3</div><div class=""story-content""><h3>Analytics Engine Execution</h3><p>Ritim, segment, frekans, RFM ve Customer Value Score hesaplama job'ları çalışır.</p></div></div>
<div class=""story-step""><div class=""story-num"">4</div><div class=""story-content""><h3>CCE Karar Ağacı</h3><p>Next Best Action, Next Best Channel ve Günün Fırsatları öncelik sıralaması yapılır.</p></div></div>
<div class=""story-step""><div class=""story-num"">5</div><div class=""story-content""><h3>Dispatcher &amp; Provider Gateways</h3><p>İzin/opt-out kontrolleri geçilerek ilgili SMS, WhatsApp veya E-posta sağlayıcısına iletilir.</p></div></div>
</div>
"
            };

            // Article 2: internal-api-mimarisi-ve-veri-kontratlari
            internalWiki.Pages["internal-api-mimarisi-ve-veri-kontratlari"] = new WikiPage
            {
                Slug = "internal-api-mimarisi-ve-veri-kontratlari",
                Section = "Mimari ve Servis Sınırları",
                Title = "Ingestion API Mimarisi, Rate Limiting ve Idempotency Kontratları",
                Summary = "API veri alım uç noktaları, batch ingestion formatları, HMAC imzalama, rate limiting algoritmaları ve idempotency key yönetimini açıklar.",
                Related = new List<string> { "internal-mimari-genel-bakis", "internal-veri-kalitesi-ve-anomali-denetimi" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Ingestion API Mimarisi ve Veri Kontratları</h1>
<p class=""hero-lead"">Pika'nın veri entegrasyonu için sunduğu dahili API mimarisi, idempotency standartları ve hata kodları.</p>
</div>
</section>

<h2>1. Ingestion Endpoint Mimarisi</h2>
<p>Veri alımı <code>/api/v1/ingest/</code> rotası altında asenkron kuyruğa yazma modeliyle çalışır. Gelen payload doğrudan veri tabanına senkron yazılmaz; buffer kuyruğuna alınarak 202 Accepted yanıtı dönülür.</p>

<h2>2. Idempotency Kontratı</h2>
<p>Tüm batch gönderimlerinde <code>X-Idempotency-Key</code> header'ı zorunludur. Anahtar 24 saat boyunca Redis/MemoryCache üzerinde saklanır. Aynı anahtarla gelen mükerrer istekler işlemi yeniden başlatmaz, ilk işlemin sonucunu ve durum kodunu döner.</p>

<h2>3. Rate Limiting ve Throttling</h2>
<p>Tenant başına token bucket algoritması ile 1000 req/min varsayılan limit uygulanır. Aşım durumunda <code>429 Too Many Requests</code> ve <code>Retry-After</code> header'ı döner.</p>
"
            };

            // Article 3: internal-teslimat-konsolu-ve-worker-mimarisi
            internalWiki.Pages["internal-teslimat-konsolu-ve-worker-mimarisi"] = new WikiPage
            {
                Slug = "internal-teslimat-konsolu-ve-worker-mimarisi",
                Section = "Delivery & Worker Mimarisi",
                Title = "Delivery Workers, Job/Attempt/Event Modeli ve Dispatcher Mimarisi",
                Summary = "Gönderim işlerinin arka plan worker havuzları, Job, Attempt ve Event durum makineleri, Dead-letter kuyruğu ve Dispatcher orkestrasyonunu detaylandırır.",
                Related = new List<string> { "internal-retry-politikasi-ve-hata-yonetimi", "internal-gonderim-izleme-ve-telemetri" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Delivery Workers, Job/Attempt/Event Modeli ve Dispatcher Mimarisi</h1>
<p class=""hero-lead"">Pika'nın mesaj ve kampanya gönderim altyapısının çekirdeğini oluşturan asenkron worker ve durum makinesi mimarisi.</p>
</div>
</section>

<h2>1. Job, Attempt ve Event Modeli</h2>
<p>Pika delivery mimarisinde her gönderim 3 temel varlıkla temsil edilir:</p>
<div class=""role-table"">
<div class=""role-row role-head""><div>Varlık</div><div>Açıklama</div></div>
<div class=""role-row""><div><strong>DeliveryJob</strong></div><div>Mesajın ana iş kaydıdır. Hedef kişi, kanal, şablon, tenant ve genel durum bilgisini (Pending, Processing, Completed, Failed, DeadLetter) tutar.</div></div>
<div class=""role-row""><div><strong>DeliveryAttempt</strong></div><div>Her sağlayıcı çağırma denemesini temsil eder. AttemptIndex, RequestTimestamp, ResponseStatusCode, RawProviderCode ve ExecutionDuration alanlarını içerir.</div></div>
<div class=""role-row""><div><strong>DeliveryEvent</strong></div><div>Gönderim sonrasında sağlayıcıdan webhook veya polling ile gelen durum olaylarıdır (Delivered, Bounced, Rejected, Clicked, Opened, OptedOut).</div></div>
</div>

<h2>2. Dispatcher Orkestrasyonu</h2>
<p>Dispatcher servisi Hangfire recurring job ve memory queue consumer'ları üzerinden çalışır:</p>
<ul>
<li><strong>Kanal Uygunluk Filtresi:</strong> Gönderim anında IYS/Opt-out veritabanı sorgulanır.</li>
<li><strong>Rate-Limiter:</strong> Sağlayıcı API sınırına göre (örn. SMS gateway için 50 req/sec) throttle uygulanır.</li>
<li><strong>Payload Builder:</strong> Müşteri değişkenleri dinamik şablona enjekte edilir.</li>
<li><strong>Audit Log:</strong> Gönderilen her paket şifrelenmiş audit loguna yazılır.</li>
</ul>

<h2>3. Dead-Letter Kuyruk Mekanizması</h2>
<p>Maksimum deneme sayısını (varsayılan: 5) aşan veya kalıcı doğrulama hatası alan job'lar <code>DeadLetter</code> tablosuna taşınır. Burada operatör müdahalesi veya otomatik dead-letter replay kuralı beklenir.</p>
"
            };

            // Article 4: internal-retry-politikasi-ve-hata-yonetimi
            internalWiki.Pages["internal-retry-politikasi-ve-hata-yonetimi"] = new WikiPage
            {
                Slug = "internal-retry-politikasi-ve-hata-yonetimi",
                Section = "Delivery & Worker Mimarisi",
                Title = "Retry Politikası, Exponential Backoff ve Hata Normalizasyonu",
                Summary = "Geçici ve kalıcı sağlayıcı hata kodlarının sınıflandırılması, exponential backoff katsayıları, jitter hesaplaması ve devre kesici (circuit breaker) politikası.",
                Related = new List<string> { "internal-teslimat-konsolu-ve-worker-mimarisi", "internal-gonderim-izleme-ve-telemetri" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Retry Politikası, Exponential Backoff ve Hata Normalizasyonu</h1>
<p class=""hero-lead"">Pika'nın sağlayıcı hatalarını yönetme stratejisi, backoff süreleri ve circuit breaker mekanizması.</p>
</div>
</section>

<h2>1. Hata Sınıflandırma Matrisi</h2>
<div class=""role-table"">
<div class=""role-row role-head""><div>Hata Tipi</div><div>Sağlayıcı / HTTP Kodu</div><div>Uygulanan Politika</div></div>
<div class=""role-row""><div><strong>Geçici Ağ / Gateway</strong></div><div>429, 500, 502, 503, 504, Timeout</div><div>Exponential backoff ile tekrar denenir (1dk, 5dk, 15dk, 1saat, 4saat).</div></div>
<div class=""role-row""><div><strong>Kalıcı İstemci Hatası</strong></div><div>400, 401, 403, 404, Geçersiz Numara/Email</div><div>Tekrar denenmez; anında PermanentFailed durumuna alınır.</div></div>
<div class=""role-row""><div><strong>İzin / Opt-Out</strong></div><div>IYS_REJECTED, BLACKLISTED, UNSUBSCRIBED</div><div>Asla tekrar denenmez; kullanıcı suppression listesine kaydedilir.</div></div>
</div>

<h2>2. Exponential Backoff &amp; Jitter Formülü</h2>
<p>Yeniden deneme gecikmesi <code>Delay = Min(MaxDelay, BaseDelay * 2^AttemptIndex) + RandomJitter</code> formülü ile hesaplanır. Jitter çakışmaları ve sağlayıcıya ani yük bindirmeyi engeller.</p>
"
            };

            // Article 5: internal-gonderim-izleme-ve-telemetri
            internalWiki.Pages["internal-gonderim-izleme-ve-telemetri"] = new WikiPage
            {
                Slug = "internal-gonderim-izleme-ve-telemetri",
                Section = "Delivery & Worker Mimarisi",
                Title = "Dağıtık Gönderim Telemetrisi ve Sağlık Kontrolleri",
                Summary = "Kuyruk gecikme metrikleri, Hangfire worker havuzu izleme, Prometheus/OpenTelemetry sayaçları ve alarm eşikleri.",
                Related = new List<string> { "internal-teslimat-konsolu-ve-worker-mimarisi", "internal-operasyon-ve-runbook" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Dağıtık Gönderim Telemetrisi ve Sağlık Kontrolleri</h1>
<p class=""hero-lead"">Worker havuzlarının yük dağılımı, kuyruk boyutu alarmları ve telemetri mimarisi.</p>
</div>
</section>

<h2>1. Temel Telemetri Metrikleri</h2>
<ul>
<li><strong>pika_delivery_queue_length:</strong> Kuyrukta bekleyen iş sayısı. &gt; 5000 uyarısı tetiklenir.</li>
<li><strong>pika_delivery_latency_seconds:</strong> İşin oluşturulması ile ilk attempt arasındaki süre.</li>
<li><strong>pika_provider_error_rate:</strong> Son 5 dakikadaki sağlayıcı hata oranı. %5 üzeri circuit breaker tetikler.</li>
</ul>
"
            };

            // Article 6: internal-veri-kalitesi-ve-anomali-denetimi
            internalWiki.Pages["internal-veri-kalitesi-ve-anomali-denetimi"] = new WikiPage
            {
                Slug = "internal-veri-kalitesi-ve-anomali-denetimi",
                Section = "Veri & BI Dahili Motorları",
                Title = "Data Quality Kural Motoru, Severity Hesaplamaları ve Freshness Scheduler",
                Summary = "DQ Engine'in 24 kural kodu, severity ağırlıkları, freshness kontrol periyotları ve anomali tespiti.",
                Related = new List<string> { "internal-musteri-deger-skoru-algoritmasi", "internal-magaza-metrik-hesaplama-mimarisi" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Data Quality Kural Motoru ve Anomali Denetimi</h1>
<p class=""hero-lead"">Veri bütünlüğünü, şema uygunluğunu ve freshness parametrelerini denetleyen dahili motor.</p>
</div>
</section>

<h2>1. DQ Kural Kataloğu ve Kodları</h2>
<div class=""role-table"">
<div class=""role-row role-head""><div>Kural Kodu</div><div>Severity</div><div>Açıklama</div></div>
<div class=""role-row""><div><strong>DQ-CUST-001</strong></div><div>Critical</div><div>Geçersiz telefon/email formatı (Regex doğrulaması başarısız).</div></div>
<div class=""role-row""><div><strong>DQ-ORD-002</strong></div><div>High</div><div>Tarih gelecekte veya 5 yıldan eski olan fatura satırları.</div></div>
<div class=""role-row""><div><strong>DQ-PROD-003</strong></div><div>Medium</div><div>Kategori veya Need Group atanmamış aktif ürünler.</div></div>
<div class=""role-row""><div><strong>DQ-FRESH-004</strong></div><div>High</div><div>Son 48 saatte yeni satış hareketi gelmemiş aktif tenantlar.</div></div>
</div>
"
            };

            // Article 7: internal-musteri-deger-skoru-algoritmasi
            internalWiki.Pages["internal-musteri-deger-skoru-algoritmasi"] = new WikiPage
            {
                Slug = "internal-musteri-deger-skoru-algoritmasi",
                Section = "Veri & BI Dahili Motorları",
                Title = "Customer Value Score Katsayı Matrisi ve Normalizasyon Formülleri",
                Summary = "Customer Value Score'un 0-100 ölçeğindeki deterministik ağırlık katsayıları, percentile normalizasyonu ve logaritmik harcama skoru formülü.",
                Related = new List<string> { "internal-cce-karar-motoru-mimarisi", "internal-veri-kalitesi-ve-anomali-denetimi" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Customer Value Score Algoritması ve Katsayı Matrisi</h1>
<p class=""hero-lead"">Müşteri değer skorunun arkasındaki matematiksel ağırlıklar ve normalizasyon modelleri.</p>
</div>
</section>

<h2>1. Deterministik Katsayı Ağırlıkları</h2>
<p>Skor 4 ana bileşenin ağırlıklı toplamından oluşur:</p>
<ul>
<li><strong>Monetary Score (Ağırlık: 0.40):</strong> Logaritmik ciro normalizasyonu <code>Min(100, Ln(TotalSpend + 1) / Ln(MaxTargetSpend) * 100)</code>.</li>
<li><strong>Frequency Score (Ağırlık: 0.25):</strong> Sipariş sıklığı ve aktif dönem sayısı.</li>
<li><strong>Recency &amp; Rhythm (Ağırlık: 0.20):</strong> Son alışverişin beklenen ritme oranı.</li>
<li><strong>Loyalty &amp; Retention (Ağırlık: 0.15):</strong> Müşteri yaşam döngüsü süresi ve sepet çeşitliliği.</li>
</ul>
"
            };

            // Article 8: internal-magaza-metrik-hesaplama-mimarisi
            internalWiki.Pages["internal-magaza-metrik-hesaplama-mimarisi"] = new WikiPage
            {
                Slug = "internal-magaza-metrik-hesaplama-mimarisi",
                Section = "Veri & BI Dahili Motorları",
                Title = "BI Snapshot Aggregation ve Mağaza Metrik Hesaplama Mimarisi",
                Summary = "Mağaza ciro, büyüme, sepet, repeat rate ve kapsama hesaplamalarının snapshot pipeline'ı ve aggregate tabloları.",
                Related = new List<string> { "internal-musteri-deger-skoru-algoritmasi", "internal-cross-sell-sepet-analizi-motoru" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>BI Snapshot Aggregation ve Mağaza Metrik Mimarisi</h1>
<p class=""hero-lead"">BI panolarının gerçek zamanlı sorgu yükünü optimize eden gecelik snapshot ve materialization akışları.</p>
</div>
</section>

<h2>1. Snapshot Üretim Süreci</h2>
<p>Her gece 02:00'de Hangfire job'ı önceki günün satış hareketlerini tenant bazında normalize eder ve <code>StorePerformanceSnapshot</code> tablosuna yazar.</p>
"
            };

            // Article 9: internal-cross-sell-sepet-analizi-motoru
            internalWiki.Pages["internal-cross-sell-sepet-analizi-motoru"] = new WikiPage
            {
                Slug = "internal-cross-sell-sepet-analizi-motoru",
                Section = "Veri & BI Dahili Motorları",
                Title = "Cross-Sell Birliktelik Analizi Algoritması (Support, Confidence, Lift)",
                Summary = "Sepet birliktelik kural motoru, Apriori tabanlı hesaplama parametreleri, transaction windowing ve minimum eşik değerleri.",
                Related = new List<string> { "internal-magaza-metrik-hesaplama-mimarisi", "internal-product-intelligence-resolution-mimarisi" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Cross-Sell Birliktelik Analizi Motoru</h1>
<p class=""hero-lead"">Çapraz satış önerilerini üreten sepet birliktelik motorunun çalışma prensipleri.</p>
</div>
</section>

<h2>1. Metrik Tanımları ve Eşik Değerleri</h2>
<ul>
<li><strong>Support (Destek):</strong> <code>Count(A ∩ B) / TotalBaskets</code> (Minimum eşik: 0.01).</li>
<li><strong>Confidence (Güven):</strong> <code>Count(A ∩ B) / Count(A)</code> (Minimum eşik: 0.15).</li>
<li><strong>Lift (Kaldıraç):</strong> <code>Confidence(A → B) / Support(B)</code> (Sadece Lift &gt; 1.2 olan kurallar önerilir).</li>
</ul>
"
            };

            // Article 10: internal-product-intelligence-resolution-mimarisi
            internalWiki.Pages["internal-product-intelligence-resolution-mimarisi"] = new WikiPage
            {
                Slug = "internal-product-intelligence-resolution-mimarisi",
                Section = "Product Intelligence Motoru",
                Title = "Product Intelligence Resolution Engine ve Master Product Eşleştirme Pipeline'ı",
                Summary = "Ham ürün isimlerinin temizlenmesi, alias eşleme, fuzzy matching, playbook bağlama ve readiness kontrol aşamaları.",
                Related = new List<string> { "internal-mimari-genel-bakis", "internal-cross-sell-sepet-analizi-motoru" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Product Intelligence Resolution Engine</h1>
<p class=""hero-lead"">Ham ürün verisini sektörel playbook ve rol tanımlarıyla zenginleştiren çözümleme pipeline'ı.</p>
</div>
</section>

<h2>1. Resolution Pipeline Aşamaları</h2>
<ol>
<li><strong>Text Sanitization:</strong> Özel karakter temizliği, küçük harf ve varyant kod ayıklama.</li>
<li><strong>Exact &amp; Alias Matching:</strong> Tenant master ürün alias tablosunda bire bir arama.</li>
<li><strong>Trigram Similarity:</strong> Benzerlik &gt; 0.85 ise otomatik öneri havuzuna atama.</li>
<li><strong>Readiness Gating:</strong> Need Group veya Product Role eksikse analitik modele dahil edilmez.</li>
</ol>
"
            };

            // Article 11: internal-cce-karar-motoru-mimarisi
            internalWiki.Pages["internal-cce-karar-motoru-mimarisi"] = new WikiPage
            {
                Slug = "internal-cce-karar-motoru-mimarisi",
                Section = "CCE Karar & Fırsat Motoru",
                Title = "Customer Context Engine (CCE) Kural Ağaçları ve Arbitrasyon Ağırlıkları",
                Summary = "Next Best Action, Next Best Channel ve Next Best Time sinyallerinin deterministik arbitrasyon kuralları, çakışma çözümleri ve öncelik sıralaması.",
                Related = new List<string> { "internal-musteri-deger-skoru-algoritmasi", "internal-ai-mimarisi-ve-prompt-yonetimi" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Customer Context Engine (CCE) Mimarisi</h1>
<p class=""hero-lead"">Müşteri fırsatlarını ve aksiyon önerilerini üreten deterministik karar motoru.</p>
</div>
</section>

<h2>1. Karar Ağacı Öncelik Kademesi</h2>
<p>CCE motoru önerileri şu hiyerarşik sırayla değerlendirir:</p>
<ol>
<li><strong>Risk Engelleri (Kritik):</strong> Son 7 günde iletişim kurulmuşsa veya opt-out varsa aksiyon baskılanır (Frequency Capping).</li>
<li><strong>Geri Kazanım (Churn Risk):</strong> Yüksek değerli pasifleşme riski taşıyan müşteriler en üst önceliği alır.</li>
<li><strong>Tekrar Satın Alma (Rhythm):</strong> Tüketim döngüsü penceresine (%80 - %120 aralığı) girmiş ürünler.</li>
<li><strong>Cross-Sell / Tamamlayıcı:</strong> Sepet birliktelik skoru yüksek fırsatlar.</li>
</ol>
"
            };

            // Article 12: internal-ai-mimarisi-ve-prompt-yonetimi
            internalWiki.Pages["internal-ai-mimarisi-ve-prompt-yonetimi"] = new WikiPage
            {
                Slug = "internal-ai-mimarisi-ve-prompt-yonetimi",
                Section = "AI Altyapısı & Prompt Yönetimi",
                Title = "LLM Provider Routing, Prompt Şablonları ve Fallback Mekanizmaları",
                Summary = "AI Kampanya Asistanı ve AI Müşteri Özeti için model yönlendirme (OpenAI / Anthropic / Gemini), prompt versiyonlama, token limitleri ve güvenlik filtreleri.",
                Related = new List<string> { "internal-cce-karar-motoru-mimarisi", "internal-mimari-genel-bakis" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>LLM Provider Routing ve Prompt Yönetimi</h1>
<p class=""hero-lead"">Pika AI servislerinin model entegrasyonu, prompt mühendisliği ve hata toleransı mimarisi.</p>
</div>
</section>

<h2>1. Model Routing &amp; Fallback</h2>
<p>AI istekleri birincil olarak hızlı model havuzuna yönlendirilir. Yanıt süresi 8 saniyeyi aşarsa veya rate limit hatası alınırsa ikincil sağlayıcı devreye girer.</p>

<h2>2. Prompt Güvenliği ve İzolasyon</h2>
<p>Müşterinin kişisel verileri (TCKN, ham telefon, açık isim) LLM payload'ına asla eklenmez. Yalnızca tokenize edilmiş anonim davranış metrikleri prompt'a parametre olarak verilir.</p>
"
            };

            // Article 13: internal-guvenlik-ve-yetkilendirme-mimarisi
            internalWiki.Pages["internal-guvenlik-ve-yetkilendirme-mimarisi"] = new WikiPage
            {
                Slug = "internal-guvenlik-ve-yetkilendirme-mimarisi",
                Section = "Güvenlik, Operasyon & Sürüm",
                Title = "Kimlik Doğrulama, Cookie Güvenliği, Tenant İzolasyonu ve RBAC Yetki Ağacı",
                Summary = "ASP.NET Core Cookie kimlik doğrulaması, JWT claim dönüşümleri, cross-tenant veri izolasyonu ve rol/yetki matrisi.",
                Related = new List<string> { "internal-mimari-genel-bakis", "internal-operasyon-ve-runbook" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Kimlik Doğrulama, Tenant İzolasyonu ve RBAC</h1>
<p class=""hero-lead"">Pika'nın güvenlik, kimlik doğrulama ve veri izolasyonu mimarisi.</p>
</div>
</section>

<h2>1. Cookie ve Token Mimarisi</h2>
<p>Sistem HttpOnly, Secure, SameSite=None cookie yapısı ve JWT claim mapping kullanır. Token yenileme arka planda sliding expiration ile yönetilir.</p>

<h2>2. Tenant İzolasyonu</h2>
<p>Tüm Entity Framework sorgularında ve repository katmanında <code>TenantId</code> global query filter zorunludur. Çapraz tenant veri sızıntısı mimari düzeyde engellenmiştir.</p>
"
            };

            // Article 14: internal-operasyon-ve-runbook
            internalWiki.Pages["internal-operasyon-ve-runbook"] = new WikiPage
            {
                Slug = "internal-operasyon-ve-runbook",
                Section = "Güvenlik, Operasyon & Sürüm",
                Title = "Dağıtım Prosedürleri, Ortam Konfigürasyonları ve Hata Çözüm Runbook'ları",
                Summary = "CI/CD pipeline'ları, veritabanı migration adımları, Cloudflare önbellek yönetimi ve sık karşılaşılan üretim ortamı hata senaryoları.",
                Related = new List<string> { "internal-gonderim-izleme-ve-telemetri", "internal-surum-ve-gecis-notlari" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Operasyonel Runbook ve Dağıtım Prosedürleri</h1>
<p class=""hero-lead"">Sistem yönetimi, canlı ortam bakımı ve arıza müdahale kılavuzları.</p>
</div>
</section>

<h2>1. Dağıtım ve Migration Prosedürü</h2>
<ol>
<li>Migration script'leri staging ortamında test edilir.</li>
<li>Web uygulaması Blue/Green slot geçişiyle sıfır kesintiyle güncellenir.</li>
<li>Hangfire worker servisleri kademeli olarak yeniden başlatılır.</li>
</ol>
"
            };

            // Article 15: internal-surum-ve-gecis-notlari
            internalWiki.Pages["internal-surum-ve-gecis-notlari"] = new WikiPage
            {
                Slug = "internal-surum-ve-gecis-notlari",
                Section = "Güvenlik, Operasyon & Sürüm",
                Title = "Sürüm Geçiş Notları, Veritabanı Migrasyonları ve Mühendislik Backlog Durumu",
                Summary = "Faz geçişleri, veritabanı şema değişiklik geçmişi ve teknik borç / mimari backlog durum özeti.",
                Related = new List<string> { "internal-operasyon-ve-runbook", "internal-mimari-genel-bakis" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Dahili Mühendislik Dokümantasyonu · Gizli</div>
<h1>Sürüm Geçiş Notları ve Mühendislik Durumu</h1>
<p class=""hero-lead"">Pika'nın versiyonlama geçmişi, faz 9.5 wiki ayrıştırması ve mimari kararlar günlüğü.</p>
</div>
</section>

<h2>1. Faz 9.5 Wiki Ayrıştırma Kararı</h2>
<p>Public müşteri dokümantasyonu ile dahili mühendislik detayları kesin sınırlarla ayrılmıştır. Tüm delivery worker, retry internals, formül katsayıları ve prompt mimarisi bu güvenli dahili alana taşınmıştır.</p>
"
            };

            return internalWiki;
        }

        private static WikiData BuildPublicWiki(WikiData original)
        {
            var publicWiki = new WikiData();

            // 9 Clean Public Categories
            publicWiki.Nav = new List<List<object>>
            {
                new List<object> { "Pika'yı Tanıyın", new List<string> {
                    "pika-nedir",
                    "pika-ne-degildir",
                    "pika-konumu",
                    "pika-nasil-calisir",
                    "kimler-icin",
                    "urun-haritasi",
                    "bilgi-bankasi-haritasi"
                }},
                new List<object> { "Müşteriyi ve Ürünü Anlayın", new List<string> {
                    "pika-360",
                    "customer-intelligence-nedir",
                    "musteri-degeri-sadakat",
                    "musteri-deger-skoru",
                    "sadakat-hedefe-yakinlik",
                    "deger-risk-birlikte-okuma",
                    "tekrar-satin-alma-analizi",
                    "product-intelligence-nedir",
                    "playbook-sektorel-anlam",
                    "need-group-product-role",
                    "dinamik-siniflandirma-alanlari",
                    "kategori-playbook-baglantisi",
                    "urun-siniflandirma-workbench",
                    "master-urun-anlamlandirmalari",
                    "review-resolution-readiness",
                    "product-intelligence-musteri-firsati",
                    "ai-musteri-ozeti"
                }},
                new List<object> { "Fırsat ve Karar", new List<string> {
                    "gunun-firsatlari-ve-karar-motoru",
                    "segmentasyon-ve-firsatlar",
                    "firsat-turleri",
                    "cross-sell-firsatlari",
                    "upsell-firsatlari",
                    "next-best-action",
                    "firsat-guveni-kanit",
                    "firsattan-aksiyona-gecis"
                }},
                new List<object> { "Aksiyon ve Otomasyon", new List<string> {
                    "kampanya-journey-orkestrasyonu",
                    "kampanya-yoneticisi-ve-kurgular",
                    "journey-tasarim-tuvali",
                    "journey-karar-kurallari",
                    "journey-store",
                    "email-template-editor",
                    "email-store",
                    "pika-pilot-ai-kampanya-asistani",
                    "icerik-studyosu-ve-gorsel-yonetimi",
                    "segment-sablonlari",
                    "aksiyon-calisma-alani",
                    "yayinlama-sablon-ve-yonetim"
                }},
                new List<object> { "Kanallar ve İzinler", new List<string> {
                    "kampanya-kanallari-ve-rol-dagilimi",
                    "kanal-operasyonlari",
                    "izin-kanal-zamanlama",
                    "izin-optout-iys",
                    "iletisim-listeleri-ve-opt-out"
                }},
                new List<object> { "Veri ve Entegrasyon", new List<string> {
                    "veri-entegrasyon-genel",
                    "pika-hangi-verileri-kullanir",
                    "excel-csv-aktarimi",
                    "kolon-eslestirme",
                    "veri-dogrulama-kalite",
                    "ice-aktarma-sonuclari",
                    "fatura-siparis-neden-onemli",
                    "satis-veri-operasyonlari",
                    "veri-sonrasi",
                    "veri-hazirligi-guvenilirlik",
                    "ozellik-veri-gereksinimleri",
                    "api-entegrasyonu"
                }},
                new List<object> { "Kullanım Rehberleri", new List<string> {
                    "gmail-kisi-aktarimi",
                    "kisi-listesi-ve-segmentler",
                    "segment-yonetimi-ve-filtreler",
                    "kategori-yonetimi",
                    "kurulum-baslangic-modeli",
                    "kullanici-roller-yetkiler",
                    "sektorel-kullanim-ornekleri",
                    "ornek-kullanim-senaryolari"
                }},
                new List<object> { "Ölçüm ve Analitik", new List<string> {
                    "bi-kokpit",
                    "satis-kanali-performansi",
                    "magaza-performansi",
                    "magaza-performans-skoru",
                    "magaza-musteri-davranisi",
                    "magaza-firsat-alanlari",
                    "urun-kategori-performansi",
                    "kampanya-performansi-ve-olcumleme",
                    "omnichannel-performansi",
                    "teslimat-konsolu",
                    "basarisiz-yeniden-deneme",
                    "gonderim-operasyonu-izleme",
                    "olcum-ogrenme-dongusu"
                }},
                new List<object> { "SSS ve Kaynaklar", new List<string> {
                    "ihtiyac-haritasi",
                    "sss",
                    "sozluk",
                    "en-iyi-uygulamalar",
                    "ai-rolu-guven-siniri"
                }}
            };

            // Copy existing pages as baseline
            foreach (var kvp in original.Pages)
            {
                publicWiki.Pages[kvp.Key] = new WikiPage
                {
                    Slug = kvp.Key,
                    Section = kvp.Value.Section,
                    Title = kvp.Value.Title,
                    Summary = kvp.Value.Summary,
                    Html = kvp.Value.Html,
                    Related = kvp.Value.Related != null ? new List<string>(kvp.Value.Related) : new List<string>()
                };
            }

            // Update sections to match the new 9 categories
            UpdatePublicPageSections(publicWiki);

            // SANITIZE & REWRITE SPLIT ARTICLES FOR CUSTOMER SAFETY
            SanitizeSplitArticles(publicWiki);

            // ADD NEW CUSTOMER USAGE GUIDES
            AddNewCustomerGuides(publicWiki);

            return publicWiki;
        }

        private static void UpdatePublicPageSections(WikiData wiki)
        {
            foreach (var navGroup in wiki.Nav)
            {
                var sectionTitle = navGroup[0]?.ToString() ?? "";
                if (navGroup[1] is List<string> slugs)
                {
                    foreach (var s in slugs)
                    {
                        if (wiki.Pages.TryGetValue(s, out var page))
                        {
                            page.Section = sectionTitle;
                        }
                    }
                }
            }
        }

        private static void SanitizeSplitArticles(WikiData wiki)
        {
            // 1. teslimat-konsolu -> Görevler ve Teslimat Takibi
            if (wiki.Pages.TryGetValue("teslimat-konsolu", out var tk))
            {
                tk.Title = "Görevler ve Teslimat Takibi";
                tk.Summary = "Gönderim operasyonlarının durumunu, kampanya teslimat süreçlerini ve operasyonel görünürlüğü müşteri odaklı olarak açıklar.";
                tk.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Ölçüm ve Analitik</div>
<h1>Görevler ve Teslimat Takibi</h1>
<p class=""hero-lead"">Pika'da bir kampanya veya Journey yayınlandığında gönderim süreçleri şeffaf bir şekilde izlenir. Teslimat Takibi ekranı, her bir iletişimin durumunu ve operasyonel güvenilirliğini kullanıcılara sunar.</p>
</div>
</section>

<div class=""lead-note"">
<span class=""eyebrow"">Temel Amaç</span>
<p>Kullanıcıların hangi kampanyanın ne aşamada olduğunu, kaç alıcıya başarıyla ulaştığını ve olası gönderim engellerini tek ekrandan görmesini sağlamaktır.</p>
</div>

<h2>Gönderim Durumları Ne Anlama Gelir?</h2>
<div class=""role-table"">
<div class=""role-row role-head""><div>Durum</div><div>Anlamı</div></div>
<div class=""role-row""><div><strong>Bekliyor (Queued)</strong></div><div>Gönderim sıraya alınmış ve zamanlanmış saatini beklemektedir.</div></div>
<div class=""role-row""><div><strong>Gönderiliyor (Processing)</strong></div><div>İlgili kanal üzerinden alıcılara iletim devam etmektedir.</div></div>
<div class=""role-row""><div><strong>Teslim Edildi (Delivered)</strong></div><div>Mesaj alıcının cihazına veya e-posta sunucusuna başarıyla ulaşmıştır.</div></div>
<div class=""role-row""><div><strong>Engellendi / Uygun Değil</strong></div><div>İletişim izni bulunmayan veya kanal tercihi kapalı olan kullanıcılara gönderim yapılmamıştır.</div></div>
</div>

<h2>Operasyonel Güvenilirlik</h2>
<p>Pika, geçici bağlantı sorunlarında gönderimi otomatik olarak koruma altına alır ve kullanıcının manuel müdahalesine gerek kalmadan süreci yönetir. Kalıcı izin engellerinde ise müşterinin tercihine saygı duyularak gönderim durdurulur.</p>
";
            }

            // 2. basarisiz-yeniden-deneme -> Başarısız Gönderimler ve İletişim Güvenliği
            if (wiki.Pages.TryGetValue("basarisiz-yeniden-deneme", out var byd))
            {
                byd.Title = "Başarısız Gönderimler ve İletişim Güvenliği";
                byd.Summary = "Geçici ve kalıcı gönderim engellerinin nasıl ayrıldığını, izin ve opt-out güvenliğini açıklar.";
                byd.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Ölçüm ve Analitik</div>
<h1>Başarısız Gönderimler ve İletişim Güvenliği</h1>
<p class=""hero-lead"">Her başarısız gönderim aynı nedene dayanmaz. Pika, geçici teknik aksaklıklar ile kalıcı müşteri tercihlerini birbirinden ayırarak hem teslimat kalitesini hem de müşteri güvenini korur.</p>
</div>
</section>

<h2>Geçici ve Kalıcı Hata Farkı</h2>
<div class=""feature-grid"">
<article class=""feature-card"">
<h3>Geçici Durumlar</h3>
<p>Alıcı sunucusunun anlık meşgul olması veya geçici şebeke kesintileridir. Pika bu durumları güvenli aralıklarla yeniden deneyerek teslimat oranını maksimize eder.</p>
</article>
<article class=""feature-card"">
<h3>Kalıcı Engeller</h3>
<p>Geçersiz e-posta adresi, eksik telefon numarası veya kullanıcının opt-out bildirmesi gibi durumlardır. Bu iletiler tekrar denenmez ve raporlarda açıkça listelenir.</p>
</article>
</div>
";
            }

            // 3. musteri-deger-skoru -> Müşteri Değer Skoru
            if (wiki.Pages.TryGetValue("musteri-deger-skoru", out var mds))
            {
                mds.Title = "Müşteri Değer Skoru";
                mds.Summary = "Müşteri Değer Skoru'nun hangi iş sinyallerini (ciro, sıklık, alışveriş ritmi) kullandığını ve nasıl yorumlanacağını açıklar.";
                mds.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Müşteriyi ve Ürünü Anlayın</div>
<h1>Müşteri Değer Skoru</h1>
<p class=""hero-lead"">Müşteri Değer Skoru, müşterinin işletmeniz için oluşturduğu toplam ticari değeri ve ilişki gücünü 0–100 arasında anlaşılır bir göstergeye dönüştürür.</p>
</div>
</section>

<h2>Hangi Sinyaller Kullanılır?</h2>
<div class=""role-table"">
<div class=""role-row role-head""><div>Sinyal</div><div>İş Anlamı</div></div>
<div class=""role-row""><div><strong>Toplam Harcama (Monetary)</strong></div><div>Müşterinin toplam ciroya katkısı ve sepet büyüklüğü.</div></div>
<div class=""role-row""><div><strong>Sipariş Sıklığı (Frequency)</strong></div><div>Alışveriş yapma periyodu ve sipariş adedi.</div></div>
<div class=""role-row""><div><strong>Alışveriş Ritmi (Rhythm)</strong></div><div>Müşterinin kendi geçmişindeki alışveriş aralığına sadakati.</div></div>
<div class=""role-row""><div><strong>İlişki Süresi (Recency)</strong></div><div>En son alışverişten bu yana geçen süre ve aktiflik durumu.</div></div>
</div>

<h2>Skor Nasıl Okunmalıdır?</h2>
<p>Yüksek skorlu müşteriler sadık ve yüksek katkı sağlayan çekirdek kitleyi temsil eder. Skorun düşüş eğilimine girmesi, müşterinin ilgisinin azaldığını gösteren erken bir uyarı sinyalidir.</p>
";
            }

            // 4. magaza-performans-skoru -> Mağaza Karşılaştırması ve Sıralamalar
            if (wiki.Pages.TryGetValue("magaza-performans-skoru", out var mps))
            {
                mps.Title = "Mağaza Karşılaştırması ve Sıralamalar";
                mps.Summary = "Mağazaların ciro, büyüme, sepet büyüklüğü ve müşteri sadakati açısından nasıl karşılaştırıldığını açıklar.";
                mps.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Ölçüm ve Analitik</div>
<h1>Mağaza Karşılaştırması ve Sıralamalar</h1>
<p class=""hero-lead"">Pika BI, mağazalarınızı tek bir yüzeysel puana sıkıştırmak yerine; ciro, müşteri büyümesi ve tekrar alışveriş oranı gibi somut eksenlerde karşılaştırır.</p>
</div>
</section>

<h2>Karşılaştırma Eksenleri</h2>
<ul>
<li><strong>Ciro ve Satış Hacmi:</strong> Toplam satış katkısı.</li>
<li><strong>Müşteri Kazanımı:</strong> Yeni müşteri edinme performansı.</li>
<li><strong>Tekrar Alışveriş Oranı:</strong> Mağazaya gelen müşterilerin yeniden gelme sıklığı.</li>
<li><strong>Ortalama Sepet:</strong> Fatura başına ürün adedi ve tutarı.</li>
</ul>
";
            }

            // 5. cross-sell-firsatlari -> Cross-sell / Çapraz Satış Analizi
            if (wiki.Pages.TryGetValue("cross-sell-firsatlari", out var csf))
            {
                csf.Title = "Cross-sell / Çapraz Satış Analizi";
                csf.Summary = "Sepet birliktelikleri üzerinden müşterilere en uygun tamamlayıcı ürün önerilerinin nasıl tespit edildiğini açıklar.";
                csf.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Fırsat ve Karar</div>
<h1>Cross-sell / Çapraz Satış Analizi</h1>
<p class=""hero-lead"">Pika Cross-sell analizi, müşterilerinizin geçmiş sepet birlikteliklerini inceleyerek birbiriyle doğal olarak eşleşen ürün çiftlerini ve fırsatları görünür kılar.</p>
</div>
</section>

<h2>Birliktelik Mantığı</h2>
<p>Hangi ürünlerin aynı sepette veya yakın zaman aralıklarında birlikte alındığı analiz edilir. Böylece belirli bir ürünü alan müşteriye ilgisiz bir teklif yerine, gerçekten ihtiyaç duyabileceği tamamlayıcı ürünler önerilir.</p>
";
            }

            // 6. next-best-action -> Next Best Action, Kanal ve Zaman
            if (wiki.Pages.TryGetValue("next-best-action", out var nba))
            {
                nba.Title = "Next Best Action, Kanal ve Zaman";
                nba.Summary = "Müşteri için en doğru aksiyonun, en uygun iletişim kanalının ve zamanlamanın nasıl belirlendiğini açıklar.";
                nba.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Fırsat ve Karar</div>
<h1>Next Best Action, Kanal ve Zaman</h1>
<p class=""hero-lead"">Pika, müşteri için sadece 'bir mesaj göndermek' yerine; hangi müşteriye, hangi teklifle, hangi kanaldan ve ne zaman ulaşılması gerektiğini kararlaştırmaya destek olur.</p>
</div>
</section>

<h2>Karar Boyutları</h2>
<div class=""feature-grid"">
<article class=""feature-card"">
<h3>Next Best Action (Hangi Aksiyon?)</h3>
<p>Müşterinin durumuna göre tekrar satın alma hatırlatması, hoş geldin teklifi, geri kazanım veya çapraz satış seçeneği.</p>
</article>
<article class=""feature-card"">
<h3>Next Best Channel (Hangi Kanal?)</h3>
<p>Müşterinin izin verdiği ve etkileşim olasılığı en yüksek kanal (E-posta, SMS veya WhatsApp).</p>
</article>
<article class=""feature-card"">
<h3>Next Best Time (Hangi Zaman?)</h3>
<p>Müşterinin alışveriş ve mesaj açma alışkanlıklarına göre en uygun gün ve saat aralığı.</p>
</article>
</div>
";
            }

            // 7. ai-rolu-guven-siniri -> AI'ın Rolü ve Güven Sınırı
            if (wiki.Pages.TryGetValue("ai-rolu-guven-siniri", out var air))
            {
                air.Title = "AI'ın Rolü ve Güven Sınırı";
                air.Summary = "Pika'da yapay zekânın karar verici değil, analitik hesaplamaları açıklayan ve içerik üreten güvenilir bir yardımcı olduğunu açıklar.";
                air.Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">SSS ve Kaynaklar</div>
<h1>AI'ın Rolü ve Güven Sınırı</h1>
<p class=""hero-lead"">Pika'da AI sihirli bir kara kutu değildir. Müşteri değeri, satın alma ritmi ve risk skorları belirlenmiş matematiksel kurallarla hesaplanır; AI ise bu verileri kullanıcı için anlaşılır metinlere ve yaratıcı kampanya fikirlerine dönüştürür.</p>
</div>
</section>

<h2>Rol Dağılımı</h2>
<div class=""role-table"">
<div class=""role-row role-head""><div>Katman</div><div>Sorumluluk</div></div>
<div class=""role-row""><div><strong>Sistem &amp; Analitik</strong></div><div>Veri doğrulama, ciro hesaplama, müşteri ritmi, sepet birlikteliği.</div></div>
<div class=""role-row""><div><strong>Pika AI</strong></div><div>Müşteri özetini yorumlama, kampanya metni ve e-posta şablonu taslağı üretme.</div></div>
<div class=""role-row""><div><strong>İnsan &amp; Yönetici</strong></div><div>Son karar, onay, bütçe ve yayına alma kontrolü.</div></div>
</div>
";
            }
        }

        private static void AddNewCustomerGuides(WikiData wiki)
        {
            // 1. gunun-firsatlari-ve-karar-motoru
            wiki.Pages["gunun-firsatlari-ve-karar-motoru"] = new WikiPage
            {
                Slug = "gunun-firsatlari-ve-karar-motoru",
                Section = "Fırsat ve Karar",
                Title = "Günün Fırsatları ve Karar Motoru",
                Summary = "Günlük olarak hesaplanan tekrar satın alma, geri kazanım ve çapraz satış fırsatlarının tek ekranda nasıl yönetildiğini açıklar.",
                Related = new List<string> { "segmentasyon-ve-firsatlar", "firsat-turleri", "next-best-action", "kampanya-journey-orkestrasyonu" },
                Html = @"
<section class=""hero hero-dark"">
<div class=""hero-copy wide"">
<div class=""hero-kicker"">Fırsat ve Karar</div>
<h1>Günün Fırsatları ve Karar Motoru</h1>
<p class=""hero-lead"">Günün Fırsatları, işletmenizin veri tabanındaki binlerce müşteri hareketini tarayarak bugün aksiyon alınması gereken en öncelikli fırsatları önünüze getirir.</p>
</div>
</section>

<figure class=""screen-figure"">
<img src=""/wiki/assets/images/img_gunun-firsatlari.png"" alt=""Pika Günün Fırsatları Ekranı"">
<figcaption>Günün Fırsatları: Öncelikli müşteri grupları, fırsat nedenleri ve tek tıkla aksiyona dönüştürme butonları.</figcaption>
</figure>

<h2>Nasıl Çalışır?</h2>
<p>Her gün yenilenen analizlerle, tüketim periyodu yaklaşanlar, sepetinde çapraz satış potansiyeli olanlar ve pasifleşme riski taşıyan sadık müşteriler listelenir. Kullanıcı bu kitleyi doğrudan bir kampanyaya veya Journey akışına yönlendirebilir.</p>
"
            };

            // 2. customer-intelligence-nedir
            wiki.Pages["customer-intelligence-nedir"] = new WikiPage
            {
                Slug = "customer-intelligence-nedir",
                Section = "Müşteriyi ve Ürünü Anlayın",
                Title = "Customer Intelligence ve Müşteri Analitiği",
                Summary = "Müşteri alışveriş ritmi, değer segmentleri ve davranış eğilimlerinin nasıl analiz edildiğini açıklar.",
                Related = new List<string> { "pika-360", "musteri-degeri-sadakat", "tekrar-satin-alma-analizi", "ai-musteri-ozeti" },
                Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Müşteriyi ve Ürünü Anlayın</div>
<h1>Customer Intelligence ve Müşteri Analitiği</h1>
<p class=""hero-lead"">Customer Intelligence, ham müşteri listelerini yaşayan ve anlamlı ticari segmentlere dönüştüren Pika'nın temel analitik katmanıdır.</p>
</div>
</section>

<h2>Analitik Göstergeler</h2>
<p>Müşterinin hangi aralıklarla alışveriş yaptığı, sepet büyüklüğü trendi, tercih ettiği mağazalar ve kanallar tek bir bütünleşik profilde toplanır.</p>
"
            };

            // 3. kampanya-yoneticisi-ve-kurgular
            wiki.Pages["kampanya-yoneticisi-ve-kurgular"] = new WikiPage
            {
                Slug = "kampanya-yoneticisi-ve-kurgular",
                Section = "Aksiyon ve Otomasyon",
                Title = "Campaign Manager ve Kampanya Kurguları",
                Summary = "Hedef kitle seçimi, kanal belirleme, şablon eşleme ve zamanlanmış kampanya yönetimini açıklar.",
                Related = new List<string> { "kampanya-journey-orkestrasyonu", "pika-pilot-ai-kampanya-asistani", "icerik-studyosu-ve-gorsel-yonetimi" },
                Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Aksiyon ve Otomasyon</div>
<h1>Campaign Manager ve Kampanya Kurguları</h1>
<p class=""hero-lead"">Campaign Manager, fırsat olarak belirlenen kitlelere yönelik çok kanallı pazarlama kampanyalarının tasarlandığı ve yönetildiği merkezdir.</p>
</div>
</section>

<h2>Kampanya Adımları</h2>
<ol>
<li><strong>Hedef Kitle:</strong> Hazır segmentlerden veya fırsat listesinden kitle seçilir.</li>
<li><strong>Kanal Seçimi:</strong> E-posta, SMS, WhatsApp veya Push bildirim belirlenir.</li>
<li><strong>İçerik:</strong> Content Studio şablonları veya AI asistanı ile hazırlanan mesaj yüklenir.</li>
<li><strong>Zamanlama:</strong> Anında gönderim veya geleceğe yönelik zamanlama yapılır.</li>
</ol>
"
            };

            // 4. icerik-studyosu-ve-gorsel-yonetimi
            wiki.Pages["icerik-studyosu-ve-gorsel-yonetimi"] = new WikiPage
            {
                Slug = "icerik-studyosu-ve-gorsel-yonetimi",
                Section = "Aksiyon ve Otomasyon",
                Title = "Content Studio ve İçerik Tasarımı",
                Summary = "E-posta, SMS ve WhatsApp için kurumsal içerik şablonlarının, görsel varlıkların ve metin taslaklarının yönetimini açıklar.",
                Related = new List<string> { "email-template-editor", "email-store", "pika-pilot-ai-kampanya-asistani" },
                Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Aksiyon ve Otomasyon</div>
<h1>Content Studio ve İçerik Tasarımı</h1>
<p class=""hero-lead"">Content Studio; tüm kanallarınız için görsel, metin ve dinamik kişiselleştirme bloklarını tek çatı altında hazırlamanızı sağlar.</p>
</div>
</section>

<figure class=""screen-figure"">
<img src=""/wiki/assets/images/img_email-template-editor_17.png"" alt=""Pika Content Studio ve Email Editörü"">
<figcaption>Content Studio: Sürükle bırak bileşenler, responsive önizleme ve görsel kütüphanesi.</figcaption>
</figure>
"
            };

            // 5. iletisim-listeleri-ve-opt-out
            wiki.Pages["iletisim-listeleri-ve-opt-out"] = new WikiPage
            {
                Slug = "iletisim-listeleri-ve-opt-out",
                Section = "Kanallar ve İzinler",
                Title = "İletişim Listeleri ve Tercih Yönetimi",
                Summary = "İletişim listeleri, müşteri izinleri, e-posta abonelikten çıkma (opt-out) ve engelleme listelerinin yönetimini açıklar.",
                Related = new List<string> { "izin-optout-iys", "izin-kanal-zamanlama", "kisi-listesi-ve-segmentler" },
                Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Kanallar ve İzinler</div>
<h1>İletişim Listeleri ve Tercih Yönetimi</h1>
<p class=""hero-lead"">Pika, müşteri tercihlerine ve yasal izinlere tam uyum sağlayacak şekilde iletişim listelerini ve abonelikten ayrılma süreçlerini otomatik yönetir.</p>
</div>
</section>

<h2>Abonelikten Çıkma ve Tercih Yönetimi</h2>
<p>Gönderilen e-postalarda yer alan tek tıkla abonelikten çıkma bağlantıları, müşterinin tercihini anında iletişim listesine işler ve gelecekteki gönderimlerde ilgili kanal otomatik olarak pasife alınır.</p>
"
            };

            // 6. segment-yonetimi-ve-filtreler
            wiki.Pages["segment-yonetimi-ve-filtreler"] = new WikiPage
            {
                Slug = "segment-yonetimi-ve-filtreler",
                Section = "Kullanım Rehberleri",
                Title = "Dinamik Segment Oluşturma ve Kural Filtreleri",
                Summary = "Audience Manager üzerinde davranışsal, demografik ve işlem bazlı dinamik segmentlerin nasıl oluşturulacağını adım adım anlatır.",
                Related = new List<string> { "kisi-listesi-ve-segmentler", "segment-sablonlari", "segmentasyon-ve-firsatlar" },
                Html = @"
<section class=""hero hero-intro"">
<div class=""hero-copy"">
<div class=""hero-kicker"">Kullanım Rehberleri</div>
<h1>Dinamik Segment Oluşturma ve Kural Filtreleri</h1>
<p class=""hero-lead"">Audience Manager, statik excel listeleri yerine kurallara göre otomatik güncellenen dinamik kitleler oluşturmanıza olanak tanır.</p>
</div>
</section>

<figure class=""screen-figure"">
<img src=""/wiki/assets/images/img_kisi-listesi-ve-segmentler_3.png"" alt=""Pika Segment Yönetimi ve Filtre Ekranı"">
<figcaption>Audience Manager: Kural bazlı filtreleme ve dinamik segment büyüklüğü önizlemesi.</figcaption>
</figure>

<h2>Adım Adım Segment Oluşturma</h2>
<ol>
<li><strong>Kural Ekle:</strong> Son alışveriş tarihi, toplam ciro, kategori ilgisi veya mağaza filtresi seçilir.</li>
<li><strong>Koşulları Birleştir:</strong> VE / VEYA mantığı ile kurallar zenginleştirilir.</li>
<li><strong>Önizle ve Kaydet:</strong> Kurala uyan canlı müşteri sayısı anlık hesaplanır ve segment kaydedilir.</li>
</ol>
"
            };
        }

        private static void CleanStaticAppJs(string baseDir, WikiData publicWiki)
        {
            var appJsPath = Path.Combine(baseDir, "wwwroot", "wiki", "assets", "app.js");
            if (!File.Exists(appJsPath)) return;

            var jsonOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            var publicJson = JsonSerializer.Serialize(publicWiki, jsonOptions);

            var cleanAppJs = "// Pika Public Knowledge Base Client Script\nconst DATA = " + publicJson + ";\n" + @"
const LANG_LOCALE = { tr: 'tr-TR', en: 'en-US' };
let lang = localStorage.getItem('pikaLanguage') || 'tr';
const pages = DATA.pages;
const navSpec = DATA.nav;
const pageOrder = navSpec.flatMap(x => x[1]);

function pageTitle(id) { return pages[id] ? pages[id].title : id; }
function sectionTitle(s) { return s || ''; }
function current() { return location.pathname.replace(/^\/wiki\/?/, '') || 'pika-nedir'; }

// Public search helper
function searchPublicDocs(q) {
    q = q.trim().toLowerCase();
    if (!q) return [];
    return Object.entries(pages).filter(([id, p]) => {
        const plain = (p.title + ' ' + p.summary + ' ' + (p.html || '').replace(/<[^>]+>/g, ' ')).toLowerCase();
        return plain.includes(q) || id.toLowerCase().includes(q);
    }).slice(0, 10).map(([id, p]) => ({ id, title: p.title, summary: p.summary }));
}
";
            File.WriteAllText(appJsPath, cleanAppJs);
        }
    }
}
