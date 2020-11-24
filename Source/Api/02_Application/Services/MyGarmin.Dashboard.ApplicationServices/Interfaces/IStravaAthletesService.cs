using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    internal interface IStravaAthletesService
    {
        Task<Athlete> CreateAthlete(string clientId);
    }
}