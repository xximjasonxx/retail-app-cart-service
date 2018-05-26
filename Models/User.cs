using Newtonsoft.Json;

namespace CartApi.Models
{
    public class User
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("role")]
        public string Role { get; set;}
    }
}