using MyGarmin.Dashboard.ApplicationServices.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUser(string username);

        Task<User> GetUser(string username, string password);

        Task<Tuple<int, List<User>>> GetUsers(List<string> filter, int rangeInit, int rangeEnd, string sort);

        Task CreateUser(User user);
    }
}
