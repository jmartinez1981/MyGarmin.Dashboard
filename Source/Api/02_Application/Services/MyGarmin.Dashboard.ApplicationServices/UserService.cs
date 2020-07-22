using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        public async Task<User> GetUser(string username)
        {
            if (username == default)
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await this.userRepository.GetUserByUsername(username).ConfigureAwait(false);
            
            if (user == null) return null;

            return user;
        }

        public async Task<User> GetUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await this.userRepository.GetUserByUsernameAndPassword(username, password).ConfigureAwait(false);

            if (user == null) return null;

            return user;
        }

        public async Task<Tuple<int, List<User>>> GetUsers(List<string> filter, int rangeInit, int rangeEnd, string sort)
        {
            var users = await this.userRepository.GetAllUsers().ConfigureAwait(false);

            return new Tuple<int, List<User>>(users.Count, users.Take(10).ToList());
            //var count = await source.CountAsync();
            //var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            //return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public async Task CreateUser(User user)
        {
            // Check if username exists
            if (await this.userRepository.ExistsUser(user.Username).ConfigureAwait(false))
            {
                throw new ArgumentException($"The user with username: {user.Username} already exists.");
            }

            // Create user
            await this.userRepository.CreateUser(user).ConfigureAwait(false);
        }
    }
}
