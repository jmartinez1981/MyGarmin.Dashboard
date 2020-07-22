using MyGarmin.Dashboard.Connectivity.StravaClient.Data;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public interface IStravaTokenClient
    {
        Task<ExchangeTokenInfo> GetExchangeToken(string clientId, string clientSecret, string code);
    }
}
