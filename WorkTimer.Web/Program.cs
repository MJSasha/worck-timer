using BlazorModalDialogs;
using BlazorModalDialogs.Dialogs.InputDialog;
using BlazorModalDialogs.Dialogs.MessageDialog;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using QuickActions.Web.Identity;
using WorkTimer.Common.Models;
using WorkTimer.Web;
using WorkTimer.Web.Common;
using WorkTimer.Web.Dialogs;
using WorkTimer.Web.Dialogs.CreateOrEditUserDialog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var appSettings = builder.Configuration.GetSection("AppSettings")?.Get<AppSettings>();

builder.Services
    .AddSingleton(appSettings);

builder.Services.AddMudServices();
builder.Services.AddModalDialogs(typeof(InputDialog),
    typeof(MessageDialog),
    typeof(UsersDataDialog),
    typeof(CreateOrEditUserModal))
                .AddIdentity<User>("session-key")
                .ProvideCommonServices(appSettings);

await builder.Build().RunAsync();
