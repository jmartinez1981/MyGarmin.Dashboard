using System.ComponentModel.DataAnnotations;

namespace MyGarmin.Dashboard.Api.Models
{
    public class ConnectionCrudModel
    {
        [Required]
        public string ConnectionType { get; set; }

        [Required]
        public string Id { get; set; }

        [DataType(DataType.Password)]
        public string Secret { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
