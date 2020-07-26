using Microsoft.AspNetCore.Mvc;
using MyGarmin.Connectivity.Client;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System.Threading.Tasks;

namespace GarminFenixSync.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IGarminClient garminClient;
        private readonly IStravaAuthClient stravaClient;
        private readonly IUsersService userService;

        public TestController(IGarminClient garminClient, IStravaAuthClient stravaClient, IUsersService userService)
        {
            this.garminClient = garminClient;
            this.stravaClient = stravaClient;
            this.userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = new User()
            {
                Firstname = "Jose",
                Lastname = "Martinez Fuentes",
                Username = "jomafu",
                Password = "1234"
            };

            await this.userService.CreateUser(user).ConfigureAwait(false);
            //var data = await this.stravaClient.GetAthleteData().ConfigureAwait(false);

            //await this.garminClient.Connect().ConfigureAwait(false);

            return this.Ok($"id requested: {id}");
        }
    }
}
