using MyGarmin.Dashboard.ApplicationServices.Entities.Garmin;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public interface IGarminConnectionService
    {
        Task CreateConnection(GarminConnection connection);

        Task<GarminConnection> GetConnection(string username);

        Task<GarminConnection> LoadData(string username);
                
        Task UpdateConnection(GarminConnection connection);
    }
}
