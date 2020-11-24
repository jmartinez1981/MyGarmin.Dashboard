using System;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Connectivity.StravaClient
{
    public interface IStravaSubscriptionClient
    {
        Task<long> SubscriptionCreationRequest(string clientId, string clientSecret, Uri callbackUri, string verifyToken);
    }
}
