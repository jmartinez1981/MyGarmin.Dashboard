using MyGarmin.Dashboard.ApplicationServices.Entities;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IStravaAthletesRepository
    {
        Task<Athlete> GetAthleteById(long id);

        Task CreateAthlete(Athlete athlete);

        Task UpdateAthlete(Athlete athlete);
    }
}
