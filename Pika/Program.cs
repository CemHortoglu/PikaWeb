using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Pika.Configuration;
using Pika.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.AddControllersWithViews();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));
builder.Services.Configure<RecaptchaSettings>(builder.Configuration.GetSection("SiteSettings:GoogleRecaptcha"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRecaptchaService, RecaptchaService>();
builder.Services.AddScoped<ITurnstileVerifier, TurnstileVerifier>();
builder.Services.AddSingleton<IWikiService, WikiService>();
builder.Services.AddSingleton<IInternalWikiService, InternalWikiService>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/internal"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("InternalDocsAccess", policy =>
        policy.RequireAuthenticatedUser()
              .RequireRole("Admin", "InternalEngineer", "Staff"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("https://app.pika.tr", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var supportedCultures = new[] { new CultureInfo("tr"), new CultureInfo("en") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new RouteDataRequestCultureProvider
    {
        RouteDataStringKey = "culture",
        UIRouteDataStringKey = "culture"
    });
    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");

app.Use(async (context, next) =>
{
    var host = context.Request.Host.Host;
    var path = context.Request.Path.Value ?? "/";
    var query = context.Request.QueryString.Value ?? string.Empty;

    // Domain normalization: www.pika.tr -> pika.tr (301 Permanent Redirect)
    if (host.Equals("www.pika.tr", StringComparison.OrdinalIgnoreCase))
    {
        var normalizedDomainUrl = $"https://pika.tr{context.Request.PathBase}{path}{query}";
        context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
        context.Response.Headers.Location = normalizedDomainUrl;
        return;
    }

    // Legacy Route Redirections (301 Permanent Redirect)
    if (LegacyRouteMapper.TryGetRedirect(path, out var targetUrl) && targetUrl != null)
    {
        var targetWithQuery = string.IsNullOrEmpty(query) ? targetUrl : $"{targetUrl}{query}";
        context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
        context.Response.Headers.Location = targetWithQuery;
        return;
    }

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization();

app.Use(async (context, next) =>
{
    var routeData = context.GetRouteData();
    if (routeData != null && (!routeData.Values.ContainsKey("culture") || string.IsNullOrEmpty(routeData.Values["culture"]?.ToString())))
    {
        var path = context.Request.Path.Value ?? "/";
        var isEn = path.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) || path.Equals("/en", StringComparison.OrdinalIgnoreCase);
        routeData.Values["culture"] = isEn ? "en" : "tr";
    }
    await next();
});

app.UseCors("AngularClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "account",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "auth",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "contactconsent",
    pattern: "contactconsent/{action=Index}/{id?}",
    defaults: new { controller = "ContactConsent" });

app.Run();

public partial class Program { }


