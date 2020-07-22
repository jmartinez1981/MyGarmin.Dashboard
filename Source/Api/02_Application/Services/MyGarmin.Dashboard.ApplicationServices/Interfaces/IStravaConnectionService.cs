using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    public interface IStravaConnectionService
    {
        Task CreateConnection(StravaConnection connection);

        Task<StravaConnection> GetConnection(string clientId);

        Task<Tuple<int, List<StravaConnection>>> GetConnections(List<string> filter, int rangeInit, int rangeEnd, string sort);

        Task UpdateConnection(StravaConnection connection);

        Task<StravaConnection> UpdateCredentials(string clientId, string code);

        Task<Tuple<string, string>> GetTokensByClientId(string clientId);

        Task<StravaConnection> MarkConnectionAsUpdated(string clientId);
    }
}
