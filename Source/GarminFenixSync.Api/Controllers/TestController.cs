using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GarminFenixSync.Api
{
    [Route("api/[Controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return this.Ok($"id requested: {id}");
        }
    }
}
