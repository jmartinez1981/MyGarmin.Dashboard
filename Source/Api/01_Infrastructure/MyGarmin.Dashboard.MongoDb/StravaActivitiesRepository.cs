using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class StravaActivitiesRepository : IStravaActivitiesRepository
    {
        internal const string CollectionName = "Activities";
        private readonly IMongoCollection<Activity> activityCollection;

        public StravaActivitiesRepository(
            IMongoDatabase mongoDatabase)
        {
            this.activityCollection = mongoDatabase.GetCollection<Activity>(CollectionName);
        }


        public async Task<Activity> GetRouteById(long id)
            => await (await this.activityCollection.FindAsync(x => x.Id == id).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task CreateActivity(Activity activity) => this.activityCollection.InsertOneAsync(activity);

        public Task CreateActivities(List<Activity> activities) => this.activityCollection.InsertManyAsync(activities);

        public Task DeleteActivity(long activityId) => this.activityCollection.DeleteOneAsync(x => x.Id == activityId);
    }
}
