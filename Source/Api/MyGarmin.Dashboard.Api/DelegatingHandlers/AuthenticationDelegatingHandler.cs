using Microsoft.AspNetCore.Http;
using MyGarmin.Dashboard.ApplicationServices;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api.DelegatingHandlers
{
    /// <summary>
    /// Its base on: https://stackoverflow.com/questions/56204350/how-to-refresh-a-token-using-ihttpclientfactory
    /// </summary>
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        private const string AccessToken = "17cac79c8dc9fd0c492086efbd12a8531f74cb39";
        private const string RefreshToken = "fd49bc9322481eae9becebee9e325a9efb15868e";
        private const string TokenScheme = "Bearer";
        private readonly IHttpContextAccessor httpContextAccessor;
        
        public AuthenticationDelegatingHandler(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var clientId = (string)this.httpContextAccessor.HttpContext.Request.RouteValues.Single(x => x.Key == "id").Value;
            var service = this.httpContextAccessor.HttpContext.RequestServices.GetService(typeof(IStravaConnectionService)) as IStravaConnectionService;
            var tokens = await service.GetTokensByClientId(clientId).ConfigureAwait(false);

            var token = tokens.Item1;
            request.Headers.Authorization = new AuthenticationHeaderValue(TokenScheme, token);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                token = tokens.Item2;
                request.Headers.Authorization = new AuthenticationHeaderValue(TokenScheme, token);
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }

        private static string GetToken() => AccessToken;

        private static string GetRefreshToken() => RefreshToken;
    }
}
