using MongoDB.Bson;
using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class StravaConnectionsRepository : IStravaConnectionsRepository
    {
        internal const string CollectionName = "StravaConnections";
        private readonly IMongoCollection<StravaConnection> stravaConnectionCollection;

        public StravaConnectionsRepository(
            IMongoDatabase mongoDatabase)
        {
            this.stravaConnectionCollection = mongoDatabase.GetCollection<StravaConnection>(CollectionName);
        }

        public async Task<StravaConnection> GetConnectionByClientId(string clientId)
            => await (await this.stravaConnectionCollection.FindAsync(x => x.ClientId == clientId).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public async Task<StravaConnection> GetConnectionBySubscriptionId(long subscriptionId)
            => await (await this.stravaConnectionCollection.FindAsync(x => x.WebhookSubscriptionId == subscriptionId).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task<List<StravaConnection>> GetAllConnections()
            => this.stravaConnectionCollection.Find(new BsonDocument()).ToListAsync();

        public Task CreateConnection(StravaConnection connection) => this.stravaConnectionCollection.InsertOneAsync(connection);

        public async Task<bool> ExistsConnection(string clientId)
            => await (await this.stravaConnectionCollection.FindAsync(x => x.ClientId == clientId).ConfigureAwait(false))
            .AnyAsync().ConfigureAwait(false);

        public Task UpdateConnection(StravaConnection connection)
            => this.stravaConnectionCollection.ReplaceOneAsync(x => x.ClientId == connection.ClientId, connection);
    }
}
