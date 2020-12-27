using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class EditAccountRequestResource
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonProperty("firstname")]
        public String FirstName { get; set; }

        [JsonProperty("lastname")]
        public String LastName { get; set; }
    }
}