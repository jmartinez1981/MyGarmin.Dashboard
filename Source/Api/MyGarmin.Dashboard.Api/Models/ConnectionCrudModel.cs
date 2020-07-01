using System.ComponentModel.DataAnnotations;

namespace MyGarmin.Dashboard.Api.Models
{
    public class ConnectionCrudModel
    {
        [Required]
        public string ConnectionType { get; set; }

        public string Id { get; set; }

        [DataType(DataType.Password)]
        public string Token { get; set; }

        [DataType(DataType.Password)]
        public string RefreshToken { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
