using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public interface IStravaClient
    {
        Task<AthleteInfo> GetAthleteData();
    }
}
