using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public class StravaTokenClient : IStravaTokenClient
    {
        private readonly ILogger<StravaTokenClient> logger;
        private readonly HttpClient httpClient;

        public StravaTokenClient(ILogger<StravaTokenClient> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<ExchangeTokenInfo> RefreshToken(string clientId, string clientSecret, string refreshToken)
        {
            var uri = this.httpClient.BaseAddress.RefreshAuthToken(clientId, clientSecret, refreshToken);
            var result = await this.httpClient.PostAsync(uri, null).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<ExchangeTokenInfo>(content);
        }

        public async Task<ExchangeTokenInfo> GetExchangeToken(string clientId, string clientSecret, string code)
        {
            var uri = this.httpClient.BaseAddress.ExchangeAuthToken(clientId, clientSecret, code);
            var result = await this.httpClient.PostAsync(uri, null).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            return JsonSerializer.Deserialize<ExchangeTokenInfo>(content);
        }
    }
}
