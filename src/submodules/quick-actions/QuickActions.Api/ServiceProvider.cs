using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using QuickActions.Common.Exceptions;

namespace QuickActions.Api
{
    public static class ServiceProvider
    {
        public static IApplicationBuilder AddExceptionsHandling(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                if (exception is ResponseException responseExtensions)
                {
                    context.Response.StatusCode = responseExtensions.StatusCode;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(responseExtensions.Value));
                }
                else
                {
                    throw exception;
                }
            });

            return applicationBuilder;
        }
    }
}