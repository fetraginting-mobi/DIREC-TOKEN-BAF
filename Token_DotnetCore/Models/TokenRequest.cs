using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Token_Ifinsys.Models
{
    public class TokenRequest
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        //[Required]
        //[JsonProperty("moduleName")]
        //public string ModuleName { get; set; }

        //[Required]
        //[JsonProperty("imei")]
        //public string Imei { get; set; }
    }
}
