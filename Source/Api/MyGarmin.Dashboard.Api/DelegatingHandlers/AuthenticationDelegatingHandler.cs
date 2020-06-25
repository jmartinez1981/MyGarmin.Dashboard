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

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue(TokenScheme, token);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                token = GetRefreshToken();
                request.Headers.Authorization = new AuthenticationHeaderValue(TokenScheme, token);
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }

        private static string GetToken() => AccessToken;

        private static string GetRefreshToken() => RefreshToken;
    }
}
