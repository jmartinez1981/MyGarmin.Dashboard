using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IStravaActivitiesRepository
    {
        Task<Activity> GetRouteById(long id);

        Task CreateActivity(Activity activity);

        Task CreateActivities(List<Activity> activities);

        Task DeleteActivity(long activityId);
    }
}
