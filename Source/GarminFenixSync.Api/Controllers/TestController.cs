using Microsoft.AspNetCore.Mvc;
using MyGarmin.Connectivity.Client;
using System.Threading.Tasks;

namespace GarminFenixSync.Api
{
    [Route("api/[Controller]")]
    public class TestController : ControllerBase
    {
        private readonly IGarminClient garminClient;

        public TestController(IGarminClient garminClient)
        {
            this.garminClient = garminClient;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            await this.garminClient.Connect().ConfigureAwait(false);

            return this.Ok($"id requested: {id}");
        }
    }
}
