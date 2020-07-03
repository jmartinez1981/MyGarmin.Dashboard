using MyGarmin.Dashboard.ApplicationServices.Entities.Garmin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IGarminConnectionRepository
    {
        Task<bool> ExistsConnection(string username);

        Task<GarminConnection> GetConnectionByUsername(string username);

        Task<List<GarminConnection>> GetAllConnections();

        Task CreateConnection(GarminConnection connection);

        Task UpdateConnection(GarminConnection connection);
    }
}
