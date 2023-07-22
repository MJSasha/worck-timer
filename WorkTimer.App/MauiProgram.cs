﻿using BlazorModalDialogs;
using Microsoft.JSInterop;
using QuickActions.Web.Identity;
using WorkTimer.App.Services;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common;
using WorkTimer.Web.Common.Interfaces;
using WorkTimer.Web.Common.Services;

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
                ApiUri = "http://192.168.1.57:8080"
            };

#elif RELEASE
            var appSettings = new AppSettings
            {
                ApiUri = "http://127.0.0.1:8080"
            };
#endif

#if ANDROID
            //builder.Services.AddTransient<IBackgroundService, AndroidBackgroundService>();
#elif IOS
            //builder.Services.AddTransient<IBackgroundService, IosBackgroundService>();
#endif

            builder.Services
                .AddSingleton(appSettings);

            builder.Services
                .AddSingleton<LocalStorageService>()
                .AddSingleton<ExceptionsHandler>()
                .AddSingleton<IStorageService, StorageService>()
                .AddSingleton<SessionCookieService>(sp => new SessionStorageService(sp.GetRequiredService<IJSRuntime>(), "session-key", sp.GetRequiredService<IStorageService>()));

            builder.Services
                .AddModalDialogs()
                .AddIdentity<User>("session-key", useCustomStorageService: true)
                .ProvideCommonServices(appSettings);

            return builder.Build();
        }
    }
}