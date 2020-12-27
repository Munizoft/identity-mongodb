using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class SetPasswordRequestResource
    {
        [JsonProperty("username")]
        [JsonRequired]
        public String UserName { get; set; }

        [JsonProperty("token")]
        [JsonRequired]
        public String Token { get; set; }

        [JsonProperty("password")]
        [JsonRequired]
        public String Password { get; set; }

        [JsonProperty("confirm_password")]
        [JsonRequired]
        public String ConfirmPassword { get; set; }
    }
}