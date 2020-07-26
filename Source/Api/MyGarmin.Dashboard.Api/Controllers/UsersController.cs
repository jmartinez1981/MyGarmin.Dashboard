using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGarmin.Dashboard.Api.Models;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService userService;

        public UsersController(IUsersService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (id == default)
            {
                return this.BadRequest();
            }

            var user = await this.userService.GetUser(id).ConfigureAwait(false);

            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> Get(string filter, string range, string sort)
        {
            var result = await this.userService.GetUsers(new List<string>(), 0, 9, sort).ConfigureAwait(false);

            this.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            this.HttpContext.Response.Headers.Add("Content-Range", $"users 0-9/{result.Item1}");

            var users = new List<UserModel>();

            result.Item2.ForEach(x => users.Add(new UserModel { Id = x.Username, Firstname = x.Firstname, Lastname = x.Lastname }));
            
            return this.Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserCreationModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }
            
            var user = new User
            {
                Username = model.Id,
                Password = model.Password,
                Firstname = model.Firstname,
                Lastname = model.Lastname
            };

            await this.userService.CreateUser(user).ConfigureAwait(false);

            return this.Ok(model);
        }
    }
}
