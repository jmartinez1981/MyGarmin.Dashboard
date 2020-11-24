using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public interface IStravaAuthClient
    {
        Task<AthleteInfo> GetAthleteData();

        Task<GearInfo> GetEquipmentDetail(string equipmentId);

        Task<List<ActivityInfo>> GetActivities(long athleteId);

        Task<ActivityInfo> GetActivity(long activityId);
    }
}
