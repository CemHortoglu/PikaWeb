const i18n = {
  tr: {
    "menu.home": "Ana Sayfa", "menu.solutions": "Çözümlerimiz", "menu.solutions.overview": "Genel Bakış", "menu.solutions.loyalty": "Sadakat Programı", "menu.solutions.campaign": "Kampanya Yönetimi", "menu.solutions.analytics": "Müşteri Analitiği", "menu.pricing": "Fiyatlandırma", "menu.corporate": "Kurumsal", "menu.contact": "İletişim", "menu.login": "Giriş", "menu.demo": "Demo Talep Et",
    "footer.about": "Pika, markaların müşteri sadakati yolculuğunu hızlandıran kurumsal bir bağlılık ve deneyim platformudur.", "footer.product": "Ürün", "footer.company": "Şirket", "footer.reach": "Bize Ulaşın", "footer.loyalty": "Sadakat Programı", "footer.campaign": "Kampanyalar", "footer.analytics": "Analitik", "footer.aboutUs": "Hakkımızda", "footer.contact": "İletişim", "footer.pricing": "Fiyatlandırma", "footer.rights": "Tüm hakları saklıdır.",
    "demo.title": "Demo Talep Formu", "demo.submit": "Talep Gönder", "demo.name": "Ad Soyad", "demo.email": "E-posta", "demo.company": "Şirket", "demo.note": "İhtiyacınızı kısaca yazın", "demo.success": "Teşekkürler! Ekibimiz kısa süre içinde size ulaşacak."
  },
  en: {
    "menu.home": "Home", "menu.solutions": "Solutions", "menu.solutions.overview": "Overview", "menu.solutions.loyalty": "Loyalty Program", "menu.solutions.campaign": "Campaign Management", "menu.solutions.analytics": "Customer Insights", "menu.pricing": "Pricing", "menu.corporate": "Corporate", "menu.contact": "Contact", "menu.login": "Login", "menu.demo": "Request Demo",
    "footer.about": "Pika is an enterprise retention and customer experience platform for growth-focused brands.", "footer.product": "Product", "footer.company": "Company", "footer.reach": "Reach us", "footer.loyalty": "Loyalty", "footer.campaign": "Campaigns", "footer.analytics": "Analytics", "footer.aboutUs": "About", "footer.contact": "Contact", "footer.pricing": "Pricing", "footer.rights": "All rights reserved.",
    "demo.title": "Request a Demo", "demo.submit": "Submit", "demo.name": "Full Name", "demo.email": "Email", "demo.company": "Company", "demo.note": "Tell us your needs", "demo.success": "Thank you! Our team will contact you soon."
  }
};

function applyLanguage(lang) {
  const dict = i18n[lang] || i18n.tr;
  document.documentElement.lang = lang;
  localStorage.setItem("pikaLang", lang);
  document.querySelectorAll("[data-i18n]").forEach(el => dict[el.dataset.i18n] && (el.textContent = dict[el.dataset.i18n]));
  document.querySelectorAll("[data-i18n-ph]").forEach(el => dict[el.dataset.i18nPh] && el.setAttribute("placeholder", dict[el.dataset.i18nPh]));
}

document.querySelectorAll("[data-lang-btn]").forEach(btn => {
  btn.addEventListener("click", () => applyLanguage(btn.dataset.langBtn));
});

applyLanguage(localStorage.getItem("pikaLang") || "tr");

document.getElementById("rdemotalep")?.addEventListener("submit", (e) => {
  e.preventDefault();
  const lang = document.documentElement.lang || "tr";
  alert(i18n[lang]["demo.success"]);
  e.target.reset();
  bootstrap.Modal.getInstance(document.getElementById("demoModal"))?.hide();
});
