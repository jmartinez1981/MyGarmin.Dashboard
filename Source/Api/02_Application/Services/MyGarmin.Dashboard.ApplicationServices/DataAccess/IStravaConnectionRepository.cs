using MyGarmin.Dashboard.ApplicationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IStravaConnectionRepository
    {
        Task<bool> ExistsConnection(string clientId);

        Task<StravaConnection> GetConnectionByClientId(string clientId);

        Task<List<StravaConnection>> GetAllConnections();

        Task CreateConnection(StravaConnection connection);

        Task UpdateConnection(StravaConnection connection);
    }
}
