using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class ForgotPasswordRequestResource
    {
        [JsonProperty("username")]
        public String UserName { get; set; }

        [JsonIgnore]
        public String NormalizedUserName { get { return !String.IsNullOrEmpty(UserName) ? UserName.ToUpper() : String.Empty; } }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}