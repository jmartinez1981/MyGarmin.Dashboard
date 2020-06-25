using System.Threading.Tasks;

namespace MyGarmin.Connectivity.Client
{
    public interface IGarminClient
    {
        Task Connect();
    }
}
