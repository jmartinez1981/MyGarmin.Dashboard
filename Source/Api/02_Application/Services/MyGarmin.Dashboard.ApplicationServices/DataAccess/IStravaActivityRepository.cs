using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IStravaActivityRepository
    {
        Task<Activity> GetRouteById(long id);

        Task CreateActivities(List<Activity> routes);
    }
}
