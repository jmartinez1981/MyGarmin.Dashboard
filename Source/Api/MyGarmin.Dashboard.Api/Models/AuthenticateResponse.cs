using MyGarmin.Dashboard.ApplicationServices.Entities;

namespace MyGarmin.Dashboard.Api.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.Firstname;
            LastName = user.Lastname;
            Username = user.Username;
            Token = token;
        }
    }
}
