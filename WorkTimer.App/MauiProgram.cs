using BlazorModalDialogs;
using BlazorModalDialogs.Dialogs.InputDialog;
using BlazorModalDialogs.Dialogs.MessageDialog;
using Microsoft.JSInterop;
using QuickActions.Web.Identity;
using WorkTimer.App.Services;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common;
using WorkTimer.Web.Common.Interfaces;

namespace WorkTimer.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            var appSettings = new AppSettings
            {
                ApiUri = "http://192.168.1.56:8080"
            };

#elif RELEASE
            var appSettings = new AppSettings
            {
                ApiUri = "http://127.0.0.1:8080"
            };
#endif

            builder.Services
                .AddSingleton(appSettings);

            builder.Services
                .AddTransient<ExceptionsHandler>()
                .AddSingleton<WorkPeriodsService>()
                .AddSingleton<IStorageService, StorageService>()
                .AddSingleton<SessionCookieService>(sp => new SessionStorageService(sp.GetRequiredService<IJSRuntime>(), "session-key", sp.GetRequiredService<IStorageService>()));

            builder.Services
                .AddModalDialogs(typeof(MessageDialog), typeof(InputDialog))
                .AddIdentity<User>("session-key", useCustomStorageService: true)
                .ProvideCommonServices(appSettings);

            return builder.Build();
        }
    }
}