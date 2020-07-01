using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGarmin.Dashboard.Api.Models;
using MyGarmin.Dashboard.ApplicationServices;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class ConnectionsController : ControllerBase
    {
        private readonly IStravaConnectionService stravaConnectionService;
        private readonly IGarminConnectionService garminConnectionService;
        private readonly IConnectionService connectionService;

        public ConnectionsController(
            IStravaConnectionService stravaConnectionService,
            IGarminConnectionService garminConnectionService,
            IConnectionService connectionService)
        {
            this.stravaConnectionService = stravaConnectionService;
            this.garminConnectionService = garminConnectionService;
            this.connectionService = connectionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (id == default)
            {
                return this.BadRequest();
            }

            var connection = await this.stravaConnectionService.GetConnection(id).ConfigureAwait(false);

            if (connection == null)
            {
                return this.NotFound();
            }

            return this.Ok(connection);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string filter, string range, string sort)
        {
            var result = await this.connectionService.GetAllConnections().ConfigureAwait(false);

            this.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            this.HttpContext.Response.Headers.Add("Content-Range", $"connections 0-9/{result.Item1}");

            var connections = new List<ConnectionModel>();

            foreach (var conn in result.Item2)
            {
                if (conn.Type == ConnectionType.Garmin)
                {
                    connections.Add(new ConnectionModel()
                    {
                        ConnectionType = "Garmin",
                        Id = conn.Username,
                        Password = conn.Password,
                        IsDataLoaded = conn.IsDataLoaded,
                        LastUpdate = conn.LastUpdate
                    });
                }
                else if (conn.Type == ConnectionType.Strava)
                {
                    connections.Add(new ConnectionModel()
                    {
                        ConnectionType = "Strava",
                        Id = conn.ClientId,
                        Token = conn.Token,
                        RefreshToken = conn.RefreshToken,
                        IsDataLoaded = conn.IsDataLoaded,
                        LastUpdate = conn.LastUpdate
                    });
                }
            }

            return this.Ok(connections);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ConnectionCrudModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            if (model.ConnectionType == "strava")
            {
                var connection = new StravaConnection
                {
                    ClientId = model.Id,
                    Token = model.Token,
                    RefreshToken = model.RefreshToken,
                };

                await this.stravaConnectionService.CreateConnection(connection).ConfigureAwait(false);
            }
            else if (model.ConnectionType == "garmin")
            {
                var connection = new GarminConnection
                {
                    Username = model.Id,
                    Password = model.Password
                };

                await this.garminConnectionService.CreateConnection(connection).ConfigureAwait(false);
            }
            else
            {
                return this.BadRequest();
            }

            return this.Ok(model);
        }

        [HttpPut("LoadData/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] LoadDataModel model)
        {
            if (model == null || !ModelState.IsValid || string.IsNullOrEmpty(id))
            {
                return this.BadRequest();
            }

            if (model.Type == "strava")
            {
                var connection = await this.stravaConnectionService.LoadData(id).ConfigureAwait(false);

                var modelUpdated = new ConnectionModel()
                {
                    Id = connection.ClientId,
                    Token = connection.Token,
                    RefreshToken = connection.RefreshToken,
                    IsDataLoaded = connection.IsDataLoaded,
                    LastUpdate = connection.LastUpdate
                };

                return this.Ok(modelUpdated);
            }
            else if (model.Type == "garmin")
            {
                var connection = await this.garminConnectionService.LoadData(id).ConfigureAwait(false);

                var modelUpdated = new ConnectionModel()
                {
                    Id = connection.Username,
                    Password = connection.Password,
                    IsDataLoaded = connection.IsDataLoaded,
                    LastUpdate = connection.LastUpdate
                };

                return this.Ok(modelUpdated);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, ConnectionCrudModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return this.BadRequest();
            }

            if (model.ConnectionType == "strava")
            {
                var connection = await this.stravaConnectionService.GetConnection(id).ConfigureAwait(false);

                if (connection == null) return this.NotFound();

                var modelUpdated = new ConnectionModel()
                {
                    Id = connection.ClientId,
                    Token = connection.Token,
                    RefreshToken = connection.RefreshToken,
                    IsDataLoaded = connection.IsDataLoaded,
                    LastUpdate = connection.LastUpdate
                };
                return this.Ok(modelUpdated);
            }
            else if (model.ConnectionType == "garmin")
            {
                var connection = await this.garminConnectionService.GetConnection(id).ConfigureAwait(false);

                if (connection == null) return this.NotFound();

                var modelUpdated = new ConnectionModel()
                {
                    Id = connection.Username,
                    Password = connection.Password
                };
                return this.Ok(modelUpdated);
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}
