using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Role
{
    public class RoleResource : IKey<String>
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }
    }
}