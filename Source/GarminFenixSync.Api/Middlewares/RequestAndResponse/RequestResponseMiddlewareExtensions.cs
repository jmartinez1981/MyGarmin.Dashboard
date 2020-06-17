using Microsoft.AspNetCore.Builder;

namespace GarminFenixSync.Api.Middlewares.RequestAndResponse
{
    internal static class RequestResponseMiddlewareExtensions
    {
        public static void ConfigureRequestResponseLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseMiddleware>();
        }
    }
}
