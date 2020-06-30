using MyGarmin.Dashboard.ApplicationServices.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public interface IStravaConnectionService
    {
        Task CreateConnection(StravaConnection connection);

        Task<StravaConnection> GetConnection(string clientId);

        Task<Tuple<int, List<StravaConnection>>> GetConnections(List<string> filter, int rangeInit, int rangeEnd, string sort);

        Task LoadData(string clientId);

        Task<Tuple<string, string>> GetTokensByClientId(string clientId);
    }
}
