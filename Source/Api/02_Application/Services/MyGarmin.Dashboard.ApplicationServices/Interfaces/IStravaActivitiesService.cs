using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    public interface IStravaActivitiesService
    {
        Task CreateActivity(long activityId, long subscriptionId);

        Task<int> CreateActivities(long athleteId, string clientId);

        Task DeleteActivity(long activityId, long subscriptionId);
    }
}