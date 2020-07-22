using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class StravaActivityRepository : IStravaActivityRepository
    {
        internal const string CollectionName = "Activities";
        private readonly IMongoCollection<Activity> athleteCollection;

        public StravaActivityRepository(
            IMongoDatabase mongoDatabase)
        {
            this.athleteCollection = mongoDatabase.GetCollection<Activity>(CollectionName);
        }


        public async Task<Activity> GetRouteById(long id)
            => await (await this.athleteCollection.FindAsync(x => x.Id == id).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task CreateActivities(List<Activity> routes)
        {
            return this.athleteCollection.InsertManyAsync(routes);
        }
    }
}
