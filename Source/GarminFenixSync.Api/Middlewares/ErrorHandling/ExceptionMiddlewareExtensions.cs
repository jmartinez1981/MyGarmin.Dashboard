using Microsoft.AspNetCore.Builder;

namespace GarminFenixSync.Api.Middlewares.ErrorHandling
{
    internal static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
