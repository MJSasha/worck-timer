using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using QuickActions.Web.Identity.Services;

namespace QuickActions.Web.Identity
{
    public static class ServiceProvider
    {
        public static IServiceCollection AddIdentity<T>(this IServiceCollection services, string keyName, bool useCustomStorageService = false)
        {
            services
                .AddSingleton<SessionService<T>>();

            services.AddOptions();
            services.AddAuthorizationCore();

            services.AddScoped<TokenAuthStateProvider<T>>();
            services.AddScoped<AuthenticationStateProvider, TokenAuthStateProvider<T>>();

            if (!useCustomStorageService) services.AddScoped(sp => new SessionCookieService(sp.GetRequiredService<IJSRuntime>(), keyName));

            return services;
        }
    }
}