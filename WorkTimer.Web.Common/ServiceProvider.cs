using Microsoft.Extensions.DependencyInjection;
using Refit;
using WorkTimer.Common.Interfaces;

namespace WorkTimer.Web.Common
{
    public static class ServiceProvider
    {
        public static IServiceCollection ProvideCommonServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSingleton(RestService.For<IWorkPeriod>(new HttpClient
            {
                BaseAddress = new Uri($"{appSettings.ApiUri}/WorkPeriods"),
                Timeout = TimeSpan.FromSeconds(appSettings.Timeout),
                MaxResponseContentBufferSize = int.MaxValue,
            }));

            return services;
        }
    }
}