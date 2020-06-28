using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGarmin.Dashboard.ApplicationServices;
using System.Collections.Generic;

namespace MyGarmin.Dashboard.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == default)
            {
                return this.BadRequest();
            }

            var user = this.userService.GetUserById(id);

            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }
        [HttpGet]
        public IActionResult Get(string filter, string range, string sort)
        {
            var result = this.userService.GetUsers(new List<string>(), 0, 9, sort);

            this.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            this.HttpContext.Response.Headers.Add("Content-Range", $"users 0-9/{result.Item1}");

            return this.Ok(result.Item2);
        }
    }
}
