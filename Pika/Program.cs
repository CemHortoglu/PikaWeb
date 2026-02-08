using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var supportedCultures = new[] { new CultureInfo("tr"), new CultureInfo("en") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    var routeCultureProvider = new RouteDataRequestCultureProvider
    {
        RouteDataStringKey = "culture",
        UIRouteDataStringKey = "culture"
    };

    options.RequestCultureProviders.Insert(0, routeCultureProvider);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization();

app.UseRouting();
app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/tr");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{culture=tr}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { culture = "tr|en" });

app.Run();
