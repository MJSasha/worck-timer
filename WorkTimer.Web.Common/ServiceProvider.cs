using Microsoft.Extensions.DependencyInjection;
using QuickActions.Common.Interfaces;
using Refit;
using WorkTimer.Common.Interfaces;
using WorkTimer.Common.Models;

namespace WorkTimer.Web.Common
{
    public static class ServiceProvider
    {
        public static IServiceCollection ProvideCommonServices(this IServiceCollection services, AppSettings appSettings)
        {
            services
                .AddSingleton<IIdentity<User>>(RestService.For<IUsersIdentity>(CreateClient("UsersIdentity", appSettings)))
                .AddSingleton(RestService.For<IUsersIdentity>(CreateClient("UsersIdentity", appSettings)))
                .AddSingleton(RestService.For<IWorkPeriod>(CreateClient("WorkPeriods", appSettings)));

            return services;
        }

        private static HttpClient CreateClient(string prefix, AppSettings appSettings)
        {
            return new HttpClient
            {
                BaseAddress = new Uri($"{appSettings.ApiUri}/{prefix}"),
                Timeout = TimeSpan.FromSeconds(appSettings.Timeout),
                MaxResponseContentBufferSize = int.MaxValue,
            };
        }
    }
}