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
    public class StravaConnectionsController : ControllerBase
    {
        private readonly IStravaConnectionService stravaConnectionService;

        public StravaConnectionsController(IStravaConnectionService stravaConnectionService)
        {
            this.stravaConnectionService = stravaConnectionService;
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
            var result = await this.stravaConnectionService.GetConnections(new List<string>(), 0, 9, sort).ConfigureAwait(false);

            this.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            this.HttpContext.Response.Headers.Add("Content-Range", $"stravaConnections 0-9/{result.Item1}");

            var connections = new List<StravaConnectionModel>();

            result.Item2.ForEach(x => connections.Add(new StravaConnectionModel { Id = x.ClientId, Token = x.Token, RefreshToken = x.RefreshToken, IsDataLoaded = x.IsDataLoaded, LastUpdate = x.LastUpdate }));
            
            return this.Ok(connections);
        }

        [HttpPost]
        public async Task<IActionResult> Post(StravaConnectionCreationModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var connection = new StravaConnection
            {
                ClientId = model.Id,
                Token = model.Token,
                RefreshToken = model.RefreshToken,
            };

            await this.stravaConnectionService.CreateConnection(connection).ConfigureAwait(false);

            return this.Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, StravaConnectionUpdateModel model)
        {
            await this.stravaConnectionService.LoadData(id).ConfigureAwait(false);

            await Task.Delay(5000).ConfigureAwait(false);

            return this.Ok();
        }
    }
}
