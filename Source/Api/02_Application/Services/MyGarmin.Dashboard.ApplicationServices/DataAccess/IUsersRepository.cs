using MyGarmin.Dashboard.ApplicationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.DataAccess
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllUsers();

        Task<User> GetUserByUsername(string username);

        Task<bool> ExistsUser(string username);

        Task<User> GetUserByUsernameAndPassword(string username, string password);

        Task CreateUser(User user);
    }
}
