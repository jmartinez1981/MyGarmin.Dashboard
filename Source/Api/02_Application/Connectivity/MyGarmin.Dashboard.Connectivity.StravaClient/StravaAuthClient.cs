using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using MyGarmin.Dashboard.Connectivity.StravaClient.Uris;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public class StravaAuthClient : IStravaAuthClient
    {
        private const int DataPerPage = 100;
        private readonly ILogger<StravaAuthClient> logger;
        private readonly HttpClient httpClient;

        public StravaAuthClient(ILogger<StravaAuthClient> logger, HttpClient httpClient)
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

        public async Task<GearInfo> GetEquipmentDetail(string equipmentId)
        {
            var uri = this.httpClient.BaseAddress.Equipment(equipmentId);
            var result = await this.httpClient.GetAsync(uri).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<GearInfo>(content);
        }

        public async Task<ActivityInfo> GetActivity(long activityId)
        {
            var uri = this.httpClient.BaseAddress.Activity(activityId);

            var result = await this.httpClient.GetAsync(uri).ConfigureAwait(false);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            return JsonSerializer.Deserialize<ActivityInfo>(content);
        }

        public async Task<List<ActivityInfo>> GetActivities(long athleteId)
        {
            var allRoutes = new List<ActivityInfo>();
            var pageNumber = 1;

            var id = athleteId.ToString(System.Globalization.CultureInfo.InvariantCulture);

            while (true)
            {
                var uri = this.httpClient.BaseAddress.Activities(id, pageNumber, DataPerPage);
                var result = await this.httpClient.GetAsync(uri).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
                var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var routes = JsonSerializer.Deserialize<List<ActivityInfo>>(content);

                if (!routes.Any())
                {
                    break;
                }

                allRoutes.AddRange(routes);
                pageNumber++;
            }

            return allRoutes;
        }
    }
}
