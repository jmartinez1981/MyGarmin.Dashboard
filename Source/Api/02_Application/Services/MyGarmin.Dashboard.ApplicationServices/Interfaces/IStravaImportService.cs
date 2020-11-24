using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    public interface IStravaImportService
    {
        Task ImportData(string clientId, string code);
    }
}