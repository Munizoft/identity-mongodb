﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Munizoft.Identity.Resources.User
{
    public class UserEditRequestResource : IKey<String>
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("firstname")]
        public String FirstName { get; set; }

        [JsonProperty("lastname")]
        public String LastName { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("username")]
        public String UserName { get; set; }

        [JsonIgnore]
        public String NormalizedEmail { get { return !String.IsNullOrEmpty(Email) ? Email.ToUpper() : String.Empty; } }

        [JsonIgnore]
        public String NormalizedUserName { get { return !String.IsNullOrEmpty(UserName) ? UserName.ToUpper() : String.Empty; } }

        [JsonProperty("roles")]
        public List<String> Roles { get; set; }

        public UserEditRequestResource()
        {
            Roles = new List<string>();
        }
    }
}