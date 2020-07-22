using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyGarmin.Dashboard.Api.Models;
using MyGarmin.Dashboard.ApplicationServices.Entities;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Common.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IOptions<AuthenticationSettings> authSettings;

        public AuthController(IUserService userService, IOptions<AuthenticationSettings> authSettings)
        {
            this.userService = userService;
            this.authSettings = authSettings;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return this.BadRequest(new { statusText = "Username or password is empty" });
            }

            var user = await this.userService.GetUser(model.Username, model.Password).ConfigureAwait(false);

            if (user == null)
                return this.BadRequest(new { statusText = "Username or password is incorrect" });

            var token = generateJwtToken(user);

            return this.Ok(new AuthenticateResponse(user, token));
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.authSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
