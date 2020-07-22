using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    internal class StravaImportService : IStravaImportService
    {
        private readonly IStravaAuthClient stravaAuthClient;
        private readonly IStravaConnectionService stravaConnectionService;
        private readonly IStravaAthleteRepository stravaAthleteRepository;
        private readonly IStravaActivityRepository stravaRouteRepository;

        public StravaImportService(
            IStravaAuthClient stravaAuthClient,
            IStravaConnectionService stravaConnectionService,
            IStravaAthleteRepository stravaAthleteRepository,
            IStravaActivityRepository stravaRouteRepository)
        {
            this.stravaAuthClient = stravaAuthClient;
            this.stravaConnectionService = stravaConnectionService;
            this.stravaAthleteRepository = stravaAthleteRepository;
            this.stravaRouteRepository = stravaRouteRepository;
        }

        public async Task ImportData(string clientId, string code)
        {
            await this.stravaConnectionService.UpdateCredentials(clientId, code).ConfigureAwait(false);

            var athlete = await this.GetAthlete(clientId).ConfigureAwait(false);
            await this.stravaAthleteRepository.CreateAthlete(athlete).ConfigureAwait(false);

            var activities = await this.GetActivities(athlete.Id, clientId).ConfigureAwait(false);
            await this.stravaRouteRepository.CreateActivities(activities).ConfigureAwait(false);

            await this.stravaConnectionService.MarkConnectionAsUpdated(clientId).ConfigureAwait(false);
        }

        private async Task<Athlete> GetAthlete(string clientId)
        {
            var athleteData = await this.stravaAuthClient.GetAthleteData().ConfigureAwait(false);
            var athlete = new Athlete(athleteData.Id, clientId, athleteData.Firstname, athleteData.Lastname, athleteData.ProfileImage, athleteData.MeasurementPreference);

            foreach (var shoe in athleteData.Shoes)
            {
                var shoeData = await this.stravaAuthClient.GetEquipmentDetail(shoe.Id).ConfigureAwait(false);
                athlete.AddShoeToEquipment(shoeData.GearId, shoeData.Brand, shoeData.Model, shoe.Distance);
            }

            return athlete;
        }

        private async Task<List<Activity>> GetActivities(long athleteId, string clientId)
        {
            var allActivities = new List<Activity>();

            var activities = await this.stravaAuthClient.GetActivities(athleteId).ConfigureAwait(false);

            foreach (var activity in activities)
            {
                var act = new Activity(activity.Id, clientId, activity.Name);
                act.SetGearData(activity.GearId, activity.Distance);
                act.SetElevationData(activity.ElevationGain, activity.ElevantionHigh, activity.ElevationLow);
                act.SetActivityTimeData(activity.StartDate, activity.MovingTime, activity.ElapsedTime);

                allActivities.Add(act);
            }

            return allActivities;
        }
    }
}
