using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class ConfirmAccountRequestResource
    {
        [JsonProperty("username")]
        [JsonRequired]
        public String UserName { get; set; }

        [JsonProperty("password")]
        [JsonRequired]
        public String Password { get; set; }

        [JsonProperty("confirm_password")]
        [JsonRequired]
        public String ConfirmPassword { get; set; }

        [JsonProperty("token")]
        [JsonRequired]
        public String Token { get; set; }

        [JsonProperty("url")]
        [JsonRequired]
        public Uri Url { get; set; }
    }
}