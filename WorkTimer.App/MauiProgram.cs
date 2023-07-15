﻿using BlazorModalDialogs;
using Microsoft.Extensions.Configuration;
using WorkTimer.Web.Common;

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
                ApiUri = "http://127.0.0.1:8080/"
            };

#elif RELEASE
            var appSettings = new AppSettings
            {
                ApiUri = "http://127.0.0.1:8080/"
            };
#endif

            builder.Services
                .AddSingleton(appSettings);

            builder.Services.AddModalDialogs();

            return builder.Build();
        }
    }
}