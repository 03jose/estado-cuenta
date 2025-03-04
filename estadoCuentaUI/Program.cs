using estadoCuentaAPI.Services;
using estadoCuentaUI.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Leer URL de la API desde appsettings.json
var apiBaseUrl = builder.Configuration["BackendUrl"];

builder.Services.AddHttpClient("BackendAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<TarjetaService>();

var app = builder.Build();

// Pasa la configuración al layout
app.Use(async (context, next) =>
{
    var backendUrl = builder.Configuration["backendUrl"];
    context.Items["BackendUrl"] = backendUrl;
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
