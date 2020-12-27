using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Auth
{
    public class LoginResponseResource
    {
        //[JsonProperty("user_id")]
        [JsonIgnore]
        public String UserId { get; set; }

        //[JsonProperty("username")]
        [JsonIgnore]
        public String Username { get; set; }

        [JsonProperty("access_token")]
        public String AccessToken { get; set; }

        [JsonProperty("expiresIn")]
        public DateTime ExpiresIn { get; set; }
    }
}