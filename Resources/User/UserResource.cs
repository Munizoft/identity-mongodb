using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.User
{
    public class UserResource : IKey<String>
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("firstname")]
        public String Firstname { get; set; }

        [JsonProperty("lastname")]
        public String Lastname { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonIgnore]
        public String NormalizedEmail { get { return !String.IsNullOrEmpty(Email) ? Email.ToUpper() : String.Empty; } }

        [JsonIgnore]
        public String NormalizedUsername { get { return !String.IsNullOrEmpty(Username) ? Username.ToUpper() : String.Empty; } }

    }
}