using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MyGarmin.Dashboard.Api.Middlewares.RequestAndResponse
{
    internal class RequestResponseMiddleware : IDisposable
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestResponseMiddleware> logger;
        private StreamReader responseStreamReader;

        public RequestResponseMiddleware(RequestDelegate next, ILogger<RequestResponseMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = await FormatRequest(context.Request).ConfigureAwait(false);

            this.logger.LogInformation($"Request: {request}");

            var originalBody = context.Response.Body;

            using (var responseBodyStream = new MemoryStream())
            {
                context.Response.Body = responseBodyStream;

                // Continue down the Middleware pipeline, eventually returning to this class
                await this.next(context).ConfigureAwait(false);

                var response = await this.FormatResponse(context.Response).ConfigureAwait(false);

                this.logger.LogInformation($"Response: {response}");

                await responseBodyStream.CopyToAsync(originalBody).ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.responseStreamReader?.Dispose();
            }
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();

            // We create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength, System.Globalization.CultureInfo.InvariantCulture)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            // Assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            this.responseStreamReader = new StreamReader(response.Body);

            var text = await this.responseStreamReader.ReadToEndAsync().ConfigureAwait(false);

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }
}
