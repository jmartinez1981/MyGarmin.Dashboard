using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGarmin.Connectivity.Client;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System.Threading.Tasks;

namespace GarminFenixSync.Api
{
    [Route("api/[Controller]")]
    [Authorize]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IGarminClient garminClient;

        private readonly IStravaClient stravaClient;

        public TestController(IGarminClient garminClient, IStravaClient stravaClient)
        {
            this.garminClient = garminClient;
            this.stravaClient = stravaClient;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await this.stravaClient.GetAthleteData().ConfigureAwait(false);

            await this.garminClient.Connect().ConfigureAwait(false);

            return this.Ok($"id requested: {id}");
        }
    }
}
