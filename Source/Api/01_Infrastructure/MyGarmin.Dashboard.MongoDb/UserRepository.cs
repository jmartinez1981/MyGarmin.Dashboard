using MongoDB.Bson;
using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class UserRepository : IUserRepository
    {
        internal const string CollectionName = "Users";
        private readonly IMongoCollection<User> userCollection;

        public UserRepository(
            IMongoDatabase mongoDatabase)
        {
            this.userCollection = mongoDatabase.GetCollection<User>(CollectionName);
        }

        public Task<List<User>> GetAllUsers()
            => this.userCollection.Find(new BsonDocument()).ToListAsync();

        public async Task<User> GetUserByUsername(string username)
            => await (await this.userCollection.FindAsync(x => x.Username == username).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public async Task<bool> ExistsUser(string username)
            => await (await this.userCollection.FindAsync(x => x.Username == username).ConfigureAwait(false))
            .AnyAsync().ConfigureAwait(false);

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        => await (await this.userCollection.FindAsync(x => x.Username == username && x.Password == password).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task CreateUser(User user)
        {
            return this.userCollection.InsertOneAsync(user);
        }
    }
}
