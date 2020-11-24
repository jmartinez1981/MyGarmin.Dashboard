using MyGarmin.Dashboard.Connectivity.GarminClient.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Connectivity.Client
{
    public interface IGarminClient
    {
        Task<List<Activity>> GetAllActivities(string username, string password);
    }
}
