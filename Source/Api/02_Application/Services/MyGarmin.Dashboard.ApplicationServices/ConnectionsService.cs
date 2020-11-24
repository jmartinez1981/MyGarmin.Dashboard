using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    public class ConnectionsService : IConnectionsService
    {
        private readonly IGarminConnectionsRepository garminConnectionRepository;
        private readonly IStravaConnectionsRepository stravaConnectionRepository;

        public ConnectionsService(
            IGarminConnectionsRepository garminConnectionRepository,
            IStravaConnectionsRepository stravaConnectionRepository)
        {
            this.garminConnectionRepository = garminConnectionRepository;
            this.stravaConnectionRepository = stravaConnectionRepository;
        }

        public async Task<Tuple<int, List<Connection>>> GetAllConnections()
        {
            var connections = new List<Connection>();

            var stravaConnections = await this.stravaConnectionRepository.GetAllConnections().ConfigureAwait(false);
            var garminConnections = await this.garminConnectionRepository.GetAllConnections().ConfigureAwait(false);

            stravaConnections
                .ForEach(
                x => connections.Add(
                    new Connection()
                    {
                        Type = ConnectionType.Strava,
                        ClientId = x.ClientId,
                        Token = x.Token,
                        RefreshToken = x.RefreshToken,
                        IsDataLoaded = x.IsDataLoaded,
                        LastUpdate = x.LastUpdate
                    }));

            garminConnections
                .ForEach(
                x => connections.Add(
                    new Connection()
                    {
                        Type = ConnectionType.Garmin,
                        Username = x.Username,
                        Password = x.Password,
                        IsDataLoaded = x.IsDataLoaded,
                        LastUpdate = x.LastUpdate
                    }));

            var count = stravaConnections.Count + garminConnections.Count;

            return new Tuple<int, List<Connection>>(count, connections);
        }
    }
}
