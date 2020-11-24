using System.Threading.Tasks;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;

namespace MyGarmin.Dashboard.ApplicationServices
{
    internal class StravaImportService : IStravaImportService
    {
        private readonly IStravaConnectionsService stravaConnectionService;
        private readonly IStravaAthletesService stravaAthletesService;
        private readonly IStravaActivitiesService stravaActivitiesService;

        public StravaImportService(
            IStravaConnectionsService stravaConnectionService,
            IStravaAthletesService stravaAthletesService,
            IStravaActivitiesService stravaActivitiesService)
        {
            this.stravaConnectionService = stravaConnectionService;
            this.stravaAthletesService = stravaAthletesService;
            this.stravaActivitiesService = stravaActivitiesService;
        }

        public async Task ImportData(string clientId, string code)
        {
            await this.stravaConnectionService.UpdateCredentials(clientId, code).ConfigureAwait(false);

            var athlete = await this.stravaAthletesService.CreateAthlete(clientId).ConfigureAwait(false);

            await this.stravaActivitiesService.CreateActivities(athlete.Id, clientId).ConfigureAwait(false);
            
            await this.stravaConnectionService.MarkConnectionAsUpdated(clientId).ConfigureAwait(false);
        }
    }
}
