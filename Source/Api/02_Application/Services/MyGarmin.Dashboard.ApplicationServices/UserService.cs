using MyGarmin.Dashboard.ApplicationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public class UserService : IUserService
    {

        private static List<User> Users => new List<User>
        {
            new User
            {
                Id = 1,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu",
                Password = "noa"
            },
            new User
            {
                Id = 2,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu2",
                Password = "noa"
            },
            new User
            {
                Id = 3,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu3",
                Password = "noa"
            },
            new User
            {
                Id = 4,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu4",
                Password = "noa"
            },
            new User
            {
                Id = 5,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu5",
                Password = "noa"
            },
            new User
            {
                Id = 6,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu6",
                Password = "noa"
            },
            new User
            {
                Id = 7,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu7",
                Password = "noa"
            },
            new User
            {
                Id = 8,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu8",
                Password = "noa"
            },
            new User
            {
                Id = 9,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu9",
                Password = "noa"
            },
            new User
            {
                Id = 10,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu10",
                Password = "noa"
            },
            new User
            {
                Id = 11,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu11",
                Password = "noa"
            },
            new User
            {
                Id = 12,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu12",
                Password = "noa"
            },
            new User
            {
                Id = 13,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu13",
                Password = "noa"
            },
            new User
            {
                Id = 14,
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu14",
                Password = "noa"
            }
        };

        public User GetUserById(int id)
        {
            var user = Users.SingleOrDefault(x => x.Id == id);

            if (user == null) return null;
            return user;
        }

        public User GetUser(string username, string password)
        {
            var user = Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null) return null;
            return user;
        } 

        public Tuple<int, List<User>> GetUsers(string filter, string range, string sort)
        {
            return new Tuple<int, List<User>>(Users.Count, Users.Take(10).ToList());

                //var count = await source.CountAsync();
                //var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                //return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public Tuple<int, List<User>> GetUsers(List<string> filter, int rangeInit, int rangeEnd, string sort)
        {
            return new Tuple<int, List<User>>(Users.Count, Users.Take(10).ToList());
        }
    }
}
