# Angular + API Auth Entegrasyon Handoff (ZBIM Web sonrası)

Bu doküman, ZBIM web tarafında yapılan auth entegrasyonlarını ve Angular/API ekiplerinin bundan sonra adım adım ne yapması gerektiğini özetler.

## 1) ZBIM web tarafında tamamlananlar

1. `Account/Login` server-side login akışı, kullanıcı adı/şifreyi `https://api.publish.tr/api/Users/Login` endpoint'ine gönderir.
2. Başarılı login'de dönen `accessToken`, `refreshToken`, expiration bilgileri ASP.NET auth cookie oturumuna yazılır (`StoreTokens`).
3. Kullanıcı claim'leri JWT içinden çıkarılır ve cookie principal'a eklenir.
4. Başarılı login'den sonra kullanıcı Angular uygulamasına (`Auth:AngularAppUrl`) redirect edilir.
5. Cross-site kullanım için cookie ayarı `SameSite=None`, `Secure`, `HttpOnly` olacak şekilde yapılandırılmıştır.
6. Angular origin'i için CORS policy (`https://app.publish.tr`) ve credentials desteği açıktır.
7. Auth API endpoint'leri:
   - `GET /api/auth/me` (Authorize, giriş yoksa 401)
   - `GET /api/auth/session` (AllowAnonymous, her durumda 200; `isAuthenticated` true/false)
   - `POST /api/auth/logout` (Authorize)

## 2) Angular ekibi - adım adım yapılacaklar

### Adım 1: Ortam URL ayarları
- Angular environment içinde ZBIM web API host'u net tanımlansın.
  - Örnek: `authBaseUrl = "https://publish.tr"`

### Adım 2: HttpClient credentials
- `authBaseUrl` altındaki auth çağrılarında `withCredentials: true` zorunlu olmalı.
- Öneri: bir interceptor ile otomatik ekleyin.

### Adım 3: Uygulama açılış bootstrap
- App başlarken (APP_INITIALIZER veya root effect):
  - `GET {authBaseUrl}/api/auth/session` çağırın.
  - `isAuthenticated === true` ise store'a login success dispatch edin.
  - `isAuthenticated === false` ise guest state olarak devam edin.

### Adım 4: Guard davranışı
- Protected route guard:
  1. Store'da auth state yoksa bootstrap kontrolünü beklesin.
  2. Auth true ise route'a izin versin.
  3. Auth false ise kullanıcıyı ZBIM login'e yönlendirsin:
     - `{authBaseUrl}/Account/Login?returnUrl=<current-absolute-url>`

### Adım 5: Login component revizyonu
- Angular login ekranı zorunlu değil; SSO akışında kullanılmayacaksa devre dışı bırakın.
- Kullanılacaksa:
  - NgRx subscribe sızıntılarını önlemek için `take(1)`/effect tabanlı akış kullanın.
  - Login başarılı olduğunda yine `/api/auth/session` ile state doğrulaması yapın.

### Adım 6: Token kullanım kararı
- ZBIM web `/api/auth/me` endpoint'i token payload döndürür.
- Eğer Angular başka backend'e Bearer gönderecekse bu token'ı memory-store'da tutup header'a ekleyin.
- Güvenlik için localStorage'a uzun süreli yazmaktan kaçının.

### Adım 7: Logout entegrasyonu
- Angular logout butonu:
  - `POST {authBaseUrl}/api/auth/logout` (withCredentials)
  - store temizle
  - login ekranına veya public route'a yönlendir

## 3) API ekibi - adım adım yapılacaklar

### Adım 1: Login response sözleşmesini sabitleyin
- `POST /api/Users/Login` response alanları:
  - `token.accessToken`
  - `token.refreshToken`
  - `token.expiration`
  - `token.refreshTokenExpiration`
  - `languageCode`
- Bu alan isimleri değişirse web tarafındaki parser güncellenmelidir.

### Adım 2: JWT claim sözleşmesini sabitleyin
- Aşağıdaki claim'lerden en az biri mevcut olmalı:
  - kullanıcı: `Username` veya standart name claim
  - rol: `role` / `RoleName`
  - kullanıcı id: `UserId`
- Claim isimlerini sık değiştirmeyin.

### Adım 3: CORS + credentials testleri
- Browser preflight + credential senaryosu test edilmeli.
- Özellikle prod domainlerde response header kontrolü yapılmalı.

### Adım 4: Token yaşam döngüsü
- Access token expiry kısa, refresh token expiry politika ile uyumlu olmalı.
- Refresh stratejisi varsa endpoint sözleşmesi Angular ekibine net aktarılmalı.

### Adım 5: Hata kontratı
- Login başarısızlığında öngörülebilir hata formatı dönün (örn. code/message).
- Web login sayfası generic hata gösteriyor; detaylar log tarafında izlenebilir olmalı.

## 4) UAT / kabul testi listesi

1. Kullanıcı `publish.tr/Account/Login` üzerinden login olur.
2. Login başarılı -> `app.publish.tr`'ye yönlenir.
3. Angular bootstrap `/api/auth/session` ile kullanıcıyı authenticated görür.
4. Browser yenilemede session korunur.
5. Logout sonrası `/api/auth/me` 401, `/api/auth/session` `isAuthenticated:false` döner.
6. Session timeout sonrası guard kullanıcıyı tekrar `Account/Login`'e gönderir.
