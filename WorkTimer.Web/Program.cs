using BlazorModalDialogs;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QuickActions.Web.Identity;
using WorkTimer.Common.Models;
using WorkTimer.Web;
using WorkTimer.Web.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.GetSection("AppSettings")?.Get<AppSettings>();

builder.Services
    .AddSingleton(appSettings);

builder.Services.AddMudServices();
builder.Services.AddModalDialogs()
                .AddIdentity<User>("session-key")
                .ProvideCommonServices(appSettings);

await builder.Build().RunAsync();
