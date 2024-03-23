using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using QuickActions.Api.Identity.Services;
using QuickActions.Common.Data;

namespace QuickActions.Api.Identity
{
    public static class ServiceProvider
    {
        /// <summary>
        /// <strong>IMPORTANT:</strong> add <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-7.0">IHttpContextAccessor</see> to DI
        /// </summary>
        public static IServiceCollection AddIdentity<T>(this IServiceCollection services, string keyName, int sessionLifeTime = 120, Func<Session<T>, string[], bool> rolesChecker = null)
        {
            services.AddSingleton(sp => new SessionsService<T>(
                sp.GetRequiredService<IHttpContextAccessor>(),
                keyName,
                sessionLifeTime,
                rolesChecker));

            return services;
        }
    }
}