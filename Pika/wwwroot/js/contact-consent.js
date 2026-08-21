(function () {
    "use strict";

    const form = document.getElementById("contactConsentForm");
    if (!form) return;

    const resultBox = document.getElementById("consentResult");
    const submitBtn = form.querySelector(".consent-submit");
    const turnstileWidget = form.querySelector(".cf-turnstile");

    function showResult(ok, message) {
        if (!resultBox) return;
        resultBox.textContent = message;
        resultBox.classList.remove("d-none", "consent-alert--ok", "consent-alert--err");
        resultBox.classList.add(ok ? "consent-alert--ok" : "consent-alert--err");
    }

    function clearResult() {
        if (!resultBox) return;
        resultBox.textContent = "";
        resultBox.classList.add("d-none");
        resultBox.classList.remove("consent-alert--ok", "consent-alert--err");
    }

    function setLoading(isLoading) {
        if (!submitBtn) return;
        submitBtn.disabled = isLoading;
        submitBtn.textContent = isLoading ? "Gönderiliyor..." : "Gönder";
    }

    function getTurnstileResponse() {
        if (!turnstileWidget) return "";
        if (typeof window.turnstile === "undefined") return "";
        try {
            return window.turnstile.getResponse(turnstileWidget) || "";
        } catch (_) {
            return "";
        }
    }

    function resetTurnstile() {
        if (!turnstileWidget) return;
        if (typeof window.turnstile === "undefined") return;
        try {
            window.turnstile.reset(turnstileWidget);
        } catch (_) { /* noop */ }
    }

    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        clearResult();

        const turnstileToken = getTurnstileResponse();
        if (turnstileWidget && !turnstileToken) {
            showResult(false, "Lütfen doğrulamayı tamamlayın.");
            return;
        }

        const formData = new FormData();
        formData.append("__RequestVerificationToken",
            form.querySelector('input[name="__RequestVerificationToken"]')?.value || "");
        formData.append("Token", form.querySelector('input[name="Token"]')?.value || "");
        formData.append("EmailConsent", form.querySelector('input[name="EmailConsent"]')?.checked ? "true" : "false");
        formData.append("SmsConsent", form.querySelector('input[name="SmsConsent"]')?.checked ? "true" : "false");
        formData.append("CfTurnstileResponse", turnstileToken);

        setLoading(true);
        try {
            const response = await fetch(form.action, {
                method: "POST",
                body: formData,
                credentials: "same-origin"
            });

            let data = null;
            try {
                data = await response.json();
            } catch (_) { /* non-json */ }

            if (response.ok && data && data.ok) {
                showResult(true, data.message || "Tercihleriniz kaydedildi. Teşekkür ederiz.");
                if (submitBtn) {
                    submitBtn.disabled = true;
                    submitBtn.textContent = "Kaydedildi";
                }
            } else {
                const msg = (data && data.message) || "İşlem tamamlanamadı. Lütfen tekrar deneyin.";
                showResult(false, msg);
                resetTurnstile();
                setLoading(false);
            }
        } catch (err) {
            showResult(false, "Ağ hatası. Lütfen tekrar deneyin.");
            resetTurnstile();
            setLoading(false);
        }
    });
})();
