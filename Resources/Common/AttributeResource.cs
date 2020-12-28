using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Common
{
    public class AttributeResource
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("key")]
        public String Key { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("value")]
        public String Value { get; set; }
    }
}