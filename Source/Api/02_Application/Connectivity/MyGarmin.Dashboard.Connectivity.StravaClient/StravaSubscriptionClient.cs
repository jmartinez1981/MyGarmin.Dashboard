using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public class StravaSubscriptionClient : IStravaSubscriptionClient
    {
        private readonly ILogger<StravaSubscriptionClient> logger;
        private readonly HttpClient httpClient;

        public StravaSubscriptionClient(ILogger<StravaSubscriptionClient> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<long> SubscriptionCreationRequest(string clientId, string clientSecret, Uri callbackUri, string verifyToken)
        {
            var uri = this.httpClient.BaseAddress.SubscriptionCreation(clientId, clientSecret, callbackUri, verifyToken);

            this.logger.LogDebug($"Strava webhook subscription request: {uri}");

            var result = await this.httpClient.PostAsync(uri, null).ConfigureAwait(false);
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                result.EnsureSuccessStatusCode();
                var jsonContent = JsonDocument.Parse(content);
                this.logger.LogInformation($"Strava webhook subscription created successfully. SubscriptionId: {content}");

                return (long)jsonContent.RootElement.GetProperty("id").GetSingle();
            }
            catch
            {
                this.logger.LogError($"Error creating subscription. ClientId: {clientId}. Error: {content}");
                throw;
            }
        }
    }
}
