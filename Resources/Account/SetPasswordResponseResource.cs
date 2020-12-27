using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class SetPasswordResponseResource
    {
        [JsonProperty("Username")]

        public String Username { get; set; }

        [JsonProperty("token")]

        public String Token { get; set; }
    }
}
