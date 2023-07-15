using BlazorModalDialogs;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WorkTimer.Web;
using WorkTimer.Web.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.GetSection("AppSettings")?.Get<AppSettings>();

builder.Services
    .AddSingleton(appSettings);

builder.Services.AddModalDialogs();

await builder.Build().RunAsync();
