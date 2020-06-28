using MyGarmin.Dashboard.ApplicationServices.Entities;
using System;
using System.Collections.Generic;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public interface IUserService
    {
        User GetUserById(int id);

        User GetUser(string username, string password);

        Tuple<int, List<User>> GetUsers(List<string> filter, int rangeInit, int rangeEnd, string sort);
    }
}
