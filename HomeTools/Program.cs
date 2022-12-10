using HomeTools.WindowsServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting.WindowsServices;
using NLog;
using NLog.Web;

var options = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(options);
builder.Services.AddControllersWithViews();
//builder.Services.AddHostedService<Worker>();

builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Host.UseWindowsService();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();