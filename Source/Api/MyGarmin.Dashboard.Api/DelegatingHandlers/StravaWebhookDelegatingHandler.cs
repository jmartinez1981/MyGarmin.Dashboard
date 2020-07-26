using Microsoft.AspNetCore.Http;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api.DelegatingHandlers
{
    /// <summary>
    /// Its base on: https://stackoverflow.com/questions/56204350/how-to-refresh-a-token-using-ihttpclientfactory
    /// </summary>
    public class StravaWebhookDelegatingHandler : DelegatingHandler
    {
        private const string TokenScheme = "Bearer";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IStravaTokenClient stravaTokenClient;

        public StravaWebhookDelegatingHandler(
            IHttpContextAccessor httpContextAccessor,
            IStravaTokenClient stravaTokenClient)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.stravaTokenClient = stravaTokenClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var subscriptionId = (int)this.httpContextAccessor.HttpContext.Items["subscriptionId"];
            var service = this.httpContextAccessor.HttpContext.RequestServices.GetService(typeof(IStravaConnectionsService)) as IStravaConnectionsService;
            var tokens = await service.GetTokensBySubscriptionId(subscriptionId).ConfigureAwait(false);
            
            // check if access_token is valid
            var token = tokens.Item1;
            request.Headers.Authorization = new AuthenticationHeaderValue(TokenScheme, token);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            // if access_token is not valid, use refresh_token to renew
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                var conn = await service.GetConnectionBySubscriptionId(subscriptionId).ConfigureAwait(false);
                var tokenInfo = await this.stravaTokenClient.RefreshToken(conn.ClientId, conn.Secret, conn.RefreshToken).ConfigureAwait(false);

                conn.Token = tokenInfo.AccessToken;
                conn.RefreshToken = tokenInfo.RefreshToken;
                var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                conn.TokenExpirationDate = start.AddMilliseconds(tokenInfo.ExpiresAt).ToUniversalTime();

                await service.UpdateConnection(conn).ConfigureAwait(false);

                request.Headers.Authorization = new AuthenticationHeaderValue(TokenScheme, tokenInfo.AccessToken);
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            
            return response;
        }
    }
}
