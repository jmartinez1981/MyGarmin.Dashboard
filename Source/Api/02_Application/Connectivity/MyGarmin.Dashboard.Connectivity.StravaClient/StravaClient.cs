using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public class StravaClient : IStravaClient
    {
        private readonly ILogger<StravaClient> logger;
        private readonly HttpClient httpClient;

        public StravaClient(ILogger<StravaClient> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<AthleteInfo> GetAthleteData()
        {
            var uri = this.httpClient.BaseAddress.AuthenticatedAthlete();
            var result = await this.httpClient.GetAsync(uri).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            return JsonSerializer.Deserialize<AthleteInfo>(content);
        }
    }
}
