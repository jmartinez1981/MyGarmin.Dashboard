using MongoDB.Bson;
using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class StravaConnectionRepository : IStravaConnectionRepository
    {
        internal const string CollectionName = "StravaConnections";
        private readonly IMongoCollection<StravaConnection> stravaConnectionCollection;

        public StravaConnectionRepository(
            IMongoDatabase mongoDatabase)
        {
            this.stravaConnectionCollection = mongoDatabase.GetCollection<StravaConnection>(CollectionName);
        }

        public async Task<StravaConnection> GetConnectionByClientId(string clientId)
            => await (await this.stravaConnectionCollection.FindAsync(x => x.ClientId == clientId).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task<List<StravaConnection>> GetAllConnections()
            => this.stravaConnectionCollection.Find(new BsonDocument()).ToListAsync();

        public Task CreateConnection(StravaConnection connection) => this.stravaConnectionCollection.InsertOneAsync(connection);

        public async Task<bool> ExistsConnection(string clientId)
            => await (await this.stravaConnectionCollection.FindAsync(x => x.ClientId == clientId).ConfigureAwait(false))
            .AnyAsync().ConfigureAwait(false);
    }
}
