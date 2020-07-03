using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public class StravaConnectionService : IStravaConnectionService
    {
        private readonly IStravaConnectionRepository stravaConnectionRepository;
        private readonly HttpClient httpClient;
        private readonly IStravaClient stravaClient;

        public StravaConnectionService(
            IStravaConnectionRepository stravaConnectionRepository,
            IStravaClient stravaClient)
        {
            this.stravaConnectionRepository = stravaConnectionRepository;
            this.stravaClient = stravaClient;
        }

        public async Task<StravaConnection> GetConnection(string clientId)
        {
            if (clientId == default)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var user = await this.stravaConnectionRepository.GetConnectionByClientId(clientId).ConfigureAwait(false);

            if (user == null) return null;

            return user;
        }

        public async Task<Tuple<int, List<StravaConnection>>> GetConnections(List<string> filter, int rangeInit, int rangeEnd, string sort)
        {
            var connections = await this.stravaConnectionRepository.GetAllConnections().ConfigureAwait(false);

            return new Tuple<int, List<StravaConnection>>(connections.Count, connections.Take(10).ToList());
        }

        public async Task CreateConnection(StravaConnection connection)
        {
            // Check if clientId exists
            if (await this.stravaConnectionRepository.ExistsConnection(connection.ClientId).ConfigureAwait(false))
            {
                throw new ArgumentException($"The connection with clientId: {connection.ClientId} already exists.");
            }

            // Create connection
            await this.stravaConnectionRepository.CreateConnection(connection).ConfigureAwait(false);
        }

        public async Task UpdateConnection(StravaConnection connection)
        {
            // Check if clientId exists
            if (!await this.stravaConnectionRepository.ExistsConnection(connection.ClientId).ConfigureAwait(false))
            {
                throw new ArgumentException($"The connection with clientId: {connection.ClientId} doesn't exist.");
            }

            await this.stravaConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);
        }

        public async Task<Tuple<string, string>> GetTokensByClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var connection = await this.stravaConnectionRepository.GetConnectionByClientId(clientId).ConfigureAwait(false);

            if (connection == null)
            {
                throw new ArgumentException($"No exists a Strava connection with clientId: {clientId}.");
            }

            return new Tuple<string, string>(connection.Token, connection.RefreshToken);
        }

        public async Task<StravaConnection> LoadData(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var connection = await this.stravaConnectionRepository.GetConnectionByClientId(clientId).ConfigureAwait(false);

            if (connection == null)
            {
                throw new ArgumentException($"No exists a Strava connection with clientId: {clientId}.");
            }

            var athleteData = await this.stravaClient.GetAthleteData().ConfigureAwait(false);

            connection.LastUpdate = DateTime.UtcNow;
            connection.IsDataLoaded = true;

            await this.stravaConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);

            return connection;
        }
    }
}
