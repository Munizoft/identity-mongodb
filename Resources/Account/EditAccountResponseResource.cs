using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class EditAccountResponseResource
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("firstname")]
        public String Firstname { get; set; }

        [JsonProperty("lastname")]
        public String Lastname { get; set; }
    }
}