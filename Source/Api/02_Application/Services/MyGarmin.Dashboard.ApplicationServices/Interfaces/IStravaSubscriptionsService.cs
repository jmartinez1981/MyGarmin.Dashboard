using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    public interface IStravaSubscriptionsService
    {
        Task<long> Subscribe(string clientId, string clientSecret);

        bool IsSubscriptionCallbackValid(string verifyToken, string mode);
    }
}
