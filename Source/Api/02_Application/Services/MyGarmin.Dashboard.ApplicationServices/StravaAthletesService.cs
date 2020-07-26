using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    internal class StravaAthletesService : IStravaAthletesService
    {
        private readonly IStravaAthletesRepository stravaAthleteRepository;
        private readonly IStravaAuthClient stravaAuthClient;
        private readonly ILogger<StravaAthletesService> logger;

        public StravaAthletesService(
            IStravaAthletesRepository stravaAthleteRepository,
            IStravaAuthClient stravaAuthClient,
            ILogger<StravaAthletesService> logger)
        {
            this.stravaAthleteRepository = stravaAthleteRepository;
            this.stravaAuthClient = stravaAuthClient;
            this.logger = logger;
        }

        public async Task<Athlete> CreateAthlete(string clientId)
        {
            var athlete = await this.GetAthlete(clientId).ConfigureAwait(false);

            if (athlete == null)
            {
                this.logger.LogWarning($"The athlete with clientId: {clientId} doesn't exist.");
                return null;
            }

            await this.stravaAthleteRepository.CreateAthlete(athlete).ConfigureAwait(false);

            this.logger.LogInformation($"Athlete with clientId: {clientId} created successfully.");

            return athlete;
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
    }
}
