using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public class StravaConnectionsService : IStravaConnectionsService
    {
        private readonly IStravaConnectionsRepository stravaConnectionRepository;
        private readonly IStravaTokenClient stravaTokenClient;
        private readonly IStravaSubscriptionsService stravaSubscriptionService;
        private readonly ILogger<StravaConnectionsService> logger;

        public StravaConnectionsService(
            IStravaConnectionsRepository stravaConnectionRepository,
            IStravaTokenClient stravaTokenClient,
            IStravaSubscriptionsService stravaSubscriptionService,
            ILogger<StravaConnectionsService> logger)
        {
            this.stravaConnectionRepository = stravaConnectionRepository;
            this.stravaTokenClient = stravaTokenClient;
            this.stravaSubscriptionService = stravaSubscriptionService;
            this.logger = logger;
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

            // Create webhook subscription
            var subscriptionId = await this.stravaSubscriptionService.Subscribe(connection.ClientId, connection.Secret).ConfigureAwait(false);
            connection.WebhookSubscriptionId = subscriptionId;

            // Create connection
            await this.stravaConnectionRepository.CreateConnection(connection).ConfigureAwait(false);

            this.logger.LogInformation($"Strava connection created successfully. Id: {connection.ClientId}");
        }

        public async Task<StravaConnection> GetConnectionBySubscriptionId(long subscriptionId)
        {
            if (subscriptionId == default)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            var connection = await this.stravaConnectionRepository.GetConnectionBySubscriptionId(subscriptionId).ConfigureAwait(false);

            if (connection == null)
            {
                throw new ArgumentException($"No exists a Strava connection with SubscriptionId: {subscriptionId}.");
            }

            return connection;
        }

        public async Task<Tuple<string, string>> GetTokensBySubscriptionId(long subscriptionId)
        {
            if (subscriptionId == default)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            var connection = await this.stravaConnectionRepository.GetConnectionBySubscriptionId(subscriptionId).ConfigureAwait(false);

            if (connection == null)
            {
                throw new ArgumentException($"No exists a Strava connection with subscriptionId: {subscriptionId}.");
            }

            return new Tuple<string, string>(connection.Token, connection.RefreshToken);
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

        public async Task UpdateConnection(StravaConnection connection)
        {
            // Check if clientId exists
            if (!await this.stravaConnectionRepository.ExistsConnection(connection.ClientId).ConfigureAwait(false))
            {
                throw new ArgumentException($"The connection with clientId: {connection.ClientId} doesn't exist.");
            }

            await this.stravaConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);
        }

        public async Task<StravaConnection> UpdateCredentials(string clientId, string code)
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

            var exchangeTokenInfo = await this.stravaTokenClient.GetExchangeToken(clientId, connection.Secret, code).ConfigureAwait(false);
            
            connection.Token = exchangeTokenInfo.AccessToken;
            connection.RefreshToken = exchangeTokenInfo.RefreshToken;
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            connection.TokenExpirationDate = start.AddMilliseconds(exchangeTokenInfo.ExpiresAt).ToUniversalTime();

            await this.stravaConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);

            return connection;
        }

        public async Task<StravaConnection> MarkConnectionAsUpdated(string clientId)
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

            connection.LastUpdate = DateTime.UtcNow;
            connection.IsDataLoaded = true;

            await this.stravaConnectionRepository.UpdateConnection(connection).ConfigureAwait(false);

            return connection;
        }
    }
}
