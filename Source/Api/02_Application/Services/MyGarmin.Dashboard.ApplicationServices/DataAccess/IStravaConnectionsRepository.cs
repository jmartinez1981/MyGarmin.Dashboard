using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IStravaConnectionsRepository
    {
        Task<bool> ExistsConnection(string clientId);

        Task<StravaConnection> GetConnectionByClientId(string clientId);

        Task<StravaConnection> GetConnectionBySubscriptionId(long subscriptionId);

        Task<List<StravaConnection>> GetAllConnections();

        Task CreateConnection(StravaConnection connection);

        Task UpdateConnection(StravaConnection connection);
    }
}
