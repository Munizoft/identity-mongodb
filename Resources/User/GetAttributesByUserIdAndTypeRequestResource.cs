using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.User
{
    public class GetAttributesByUserIdAndTypeRequestResource<TKey>
    {
        [JsonProperty("id")]
        public TKey Id { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        public GetAttributesByUserIdAndTypeRequestResource(TKey id, String type)
        {
            Id = id;
            Type = type;
        }
    }
}