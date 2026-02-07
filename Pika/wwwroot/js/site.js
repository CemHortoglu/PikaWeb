const i18n = {
  tr: {
    "menu.home": "Ana Sayfa",
    "menu.solutions": "Çözümlerimiz",
    "menu.solutions.overview": "Çözümler Genel Bakış",
    "menu.solutions.loyalty": "Sadakat Programı",
    "menu.solutions.campaign": "Kampanya Otomasyonu",
    "menu.solutions.analytics": "Analitik ve İçgörü",
    "menu.pricing": "Fiyatlandırma",
    "menu.corporate": "Kurumsal",
    "menu.contact": "İletişim",
    "menu.login": "Giriş",
    "menu.demo": "Demo Talebi",
    "footer.about": "Pika, markaların müşteri sadakati yolculuğunu hızlandıran kurumsal bir bağlılık ve deneyim platformudur.",
    "footer.product": "Ürün",
    "footer.company": "Şirket",
    "footer.reach": "Bize Ulaşın",
    "footer.loyalty": "Sadakat Programı",
    "footer.campaign": "Kampanyalar",
    "footer.analytics": "Analitik",
    "footer.aboutUs": "Hakkımızda",
    "footer.contact": "İletişim",
    "footer.pricing": "Fiyatlandırma",
    "footer.rights": "Tüm hakları saklıdır.",
    "demo.title": "Demo Talep Formu",
    "demo.submit": "Talep Gönder",
    "demo.name": "Ad Soyad",
    "demo.email": "E-posta",
    "demo.company": "Şirket",
    "demo.note": "İhtiyacınızı kısaca yazın"
  },
  en: {
    "menu.home": "Home",
    "menu.solutions": "Solutions",
    "menu.solutions.overview": "Solutions Overview",
    "menu.solutions.loyalty": "Loyalty Program",
    "menu.solutions.campaign": "Campaign Automation",
    "menu.solutions.analytics": "Analytics & Insights",
    "menu.pricing": "Pricing",
    "menu.corporate": "Corporate",
    "menu.contact": "Contact",
    "menu.login": "Login",
    "menu.demo": "Request Demo",
    "footer.about": "Pika is an enterprise loyalty and experience platform accelerating customer retention growth.",
    "footer.product": "Product",
    "footer.company": "Company",
    "footer.reach": "Contact",
    "footer.loyalty": "Loyalty Program",
    "footer.campaign": "Campaigns",
    "footer.analytics": "Analytics",
    "footer.aboutUs": "About",
    "footer.contact": "Contact",
    "footer.pricing": "Pricing",
    "footer.rights": "All rights reserved.",
    "demo.title": "Demo Request Form",
    "demo.submit": "Submit Request",
    "demo.name": "Full Name",
    "demo.email": "Email",
    "demo.company": "Company",
    "demo.note": "Briefly tell us your needs"
  }
};

function applyLanguage(lang) {
  document.documentElement.setAttribute('lang', lang);
  document.querySelectorAll('[data-i18n]').forEach((el) => {
    const key = el.dataset.i18n;
    if (i18n[lang][key]) el.textContent = i18n[lang][key];
  });
  document.querySelectorAll('[data-i18n-ph]').forEach((el) => {
    const key = el.dataset.i18nPh;
    if (i18n[lang][key]) el.setAttribute('placeholder', i18n[lang][key]);
  });
  document.querySelectorAll('[data-lang-btn]').forEach((btn) => {
    btn.classList.toggle('active', btn.dataset.langBtn === lang);
  });
}

document.querySelectorAll('[data-lang-btn]').forEach((btn) => {
  btn.addEventListener('click', () => applyLanguage(btn.dataset.langBtn));
});

applyLanguage('tr');

document.querySelectorAll('#demoModal form').forEach((form) => {
  form.addEventListener('submit', (e) => {
    e.preventDefault();
    alert('Teşekkürler! Ekibimiz en kısa sürede sizinle iletişime geçecek.');
    form.reset();
    bootstrap.Modal.getInstance(document.getElementById('demoModal'))?.hide();
  });
});
