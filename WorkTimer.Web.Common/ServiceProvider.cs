using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using QuickActions.Common.Interfaces;
using Refit;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;
using WorkTimer.Web.Common.Services;

namespace WorkTimer.Web.Common
{
    public static class ServiceProvider
    {
        public static IServiceCollection ProvideCommonServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<CookieHandler>()
                .AddTransient(sp => sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient("API"))
                .AddHttpClient("API", client => client.BaseAddress = new Uri(appSettings.ApiUri)).AddHttpMessageHandler<CookieHandler>();

            var refitSettings = new RefitSettings()
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include })
            };

            services
                .AddSingleton<IIdentity<User>>(RestService.For<IUsersIdentity>(CreateClient("UsersIdentity", services), refitSettings))
                .AddSingleton(RestService.For<IUsersIdentity>(CreateClient("UsersIdentity", services), refitSettings))
                .AddSingleton(RestService.For<IWorkPeriod>(CreateClient("WorkPeriods", services), refitSettings));

            return services;
        }

        private static HttpClient CreateClient(string prefix, IServiceCollection services)
        {
            var client = services.BuildServiceProvider().GetService<HttpClient>();
            client.BaseAddress = new Uri($"{client.BaseAddress}{prefix}");
            return client;
        }
    }
}