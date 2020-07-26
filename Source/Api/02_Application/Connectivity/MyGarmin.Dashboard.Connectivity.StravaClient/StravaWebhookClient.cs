using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public class StravaWebhookClient : IStravaWebhookClient
    {
        private readonly ILogger<StravaWebhookClient> logger;
        private readonly HttpClient httpClient;

        public StravaWebhookClient(ILogger<StravaWebhookClient> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<ActivityInfo> GetActivity(long activityId)
        {
            var uri = this.httpClient.BaseAddress.Activity(activityId);

            var result = await this.httpClient.GetAsync(uri).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<ActivityInfo>(content);
        }
    }
}
