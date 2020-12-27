using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class ForgotPasswordResponseResource
    {
        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}