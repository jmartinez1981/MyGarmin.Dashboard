using MongoDB.Bson;
using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class GarminConnectionRepository : IGarminConnectionRepository
    {
        internal const string CollectionName = "GarminConnections";
        private readonly IMongoCollection<GarminConnection> garminConnectionCollection;

        public GarminConnectionRepository(
            IMongoDatabase mongoDatabase)
        {
            this.garminConnectionCollection = mongoDatabase.GetCollection<GarminConnection>(CollectionName);
        }

        public async Task<GarminConnection> GetConnectionByUsername(string username)
            => await (await this.garminConnectionCollection.FindAsync(x => x.Username == username).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task<List<GarminConnection>> GetAllConnections()
            => this.garminConnectionCollection.Find(new BsonDocument()).ToListAsync();

        public Task CreateConnection(GarminConnection connection) => this.garminConnectionCollection.InsertOneAsync(connection);

        public async Task<bool> ExistsConnection(string username)
            => await (await this.garminConnectionCollection.FindAsync(x => x.Username == username).ConfigureAwait(false))
            .AnyAsync().ConfigureAwait(false);

        public Task UpdateConnection(GarminConnection connection)
            => this.garminConnectionCollection.ReplaceOneAsync(x => x.Username == connection.Username, connection);
    }
}
