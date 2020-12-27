using Munizoft.Identity.Resources.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Munizoft.Identity.Resources.Account
{
    public class RegisterRequestResource
    {
        [JsonProperty("firstname")]
        public String FirstName { get; set; }

        [JsonProperty("lastname")]
        public String LastName { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("username")]
        public String UserName { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        [JsonIgnore]
        public String NormalizedEmail { get { return !String.IsNullOrEmpty(Email) ? Email.ToUpper() : String.Empty; } }

        [JsonIgnore]
        public String NormalizedUserName { get { return !String.IsNullOrEmpty(UserName) ? UserName.ToUpper() : String.Empty; } }

        [JsonProperty("attributes")]
        public List<AttributeCreateRequestResource> Attributes { get; set; }

        [JsonProperty("roles")]
        public List<String> Roles { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        public RegisterRequestResource()
        {
            Roles = new List<String>();
        }
    }
}