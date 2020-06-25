using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MyGarmin.Dashboard.Api.Middlewares.ErrorHandling
{
    internal class ExceptionMiddleware
    {
        public const string InternalServerErrorMessage = "Internal Server Error.";

        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            this.logger.LogError("Something went wrong: {Message}", exception.Message);

            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = InternalServerErrorMessage,
            }.ToString()).ConfigureAwait(false);
        }
    }
}
