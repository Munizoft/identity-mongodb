using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Role
{
    public class RoleCreateRequestResource
    {
        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonIgnore]
        public String NormalizedName { get { return !String.IsNullOrEmpty(Name) ? Name.ToUpper() : String.Empty; } }
    }
}