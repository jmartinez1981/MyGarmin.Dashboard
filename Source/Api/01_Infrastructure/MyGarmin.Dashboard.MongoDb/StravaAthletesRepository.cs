using MongoDB.Driver;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.MongoDb
{
    internal class StravaAthletesRepository : IStravaAthletesRepository
    {
        internal const string CollectionName = "Athletes";
        private readonly IMongoCollection<Athlete> athleteCollection;

        public StravaAthletesRepository(
            IMongoDatabase mongoDatabase)
        {
            this.athleteCollection = mongoDatabase.GetCollection<Athlete>(CollectionName);
        }


        public async Task<Athlete> GetAthleteById(long id)
            => await (await this.athleteCollection.FindAsync(x => x.Id == id).ConfigureAwait(false))
            .FirstOrDefaultAsync().ConfigureAwait(false);

        public Task CreateAthlete(Athlete athlete)
        {
            return this.athleteCollection.InsertOneAsync(athlete);
        }

        public Task UpdateAthlete(Athlete athlete)
            => this.athleteCollection.ReplaceOneAsync(x => x.Id == athlete.Id, athlete);
    }
}
