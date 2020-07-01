using MyGarmin.Connectivity.Client;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public class GarminConnectionService : IGarminConnectionService
    {
        private readonly IGarminConnectionRepository garminConnectionRepository;
        private readonly HttpClient httpClient;
        private readonly IGarminClient garminClient;

        public GarminConnectionService(
            IGarminConnectionRepository garminConnectionRepository,
            IGarminClient garminClient)
        {
            this.garminConnectionRepository = garminConnectionRepository;
            this.garminClient = garminClient;
        }

        public async Task<GarminConnection> GetConnection(string username)
        {
            if (username == default)
            {
                throw new ArgumentNullException(nameof(username));
            }

            var connection = await this.garminConnectionRepository.GetConnectionByUsername(username).ConfigureAwait(false);

            if (connection == null) return null;

            return connection;
        }

        public async Task CreateConnection(GarminConnection connection)
        {
            // Check if clientId exists
            if (await this.garminConnectionRepository.ExistsConnection(connection.Username).ConfigureAwait(false))
            {
                throw new ArgumentException($"The connection with username: {connection.Username} already exists.");
            }

            // Create connection
            await this.garminConnectionRepository.CreateConnection(connection).ConfigureAwait(false);
        }

        public async Task UpdateConnection(GarminConnection connection)
        {
            // Check if clientId exists
            if (!await this.garminConnectionRepository.ExistsConnection(connection.Username).ConfigureAwait(false))
            {
                throw new ArgumentException($"The connection with username: {connection.Username} doesn't exist.");
            }

            await this.garminConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);
        }
                
        public async Task<GarminConnection> LoadData(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var connection = await this.garminConnectionRepository.GetConnectionByUsername(username).ConfigureAwait(false);

            if (connection == null)
            {
                throw new ArgumentException($"No exists a Garmin connection with username: {username}.");
            }

            await this.garminClient.GetAllActivities(connection.Username, connection.Password).ConfigureAwait(false);

            connection.LastUpdate = DateTime.UtcNow;
            connection.IsDataLoaded = true;

            await this.garminConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);

            return connection;
        }
    }
}
