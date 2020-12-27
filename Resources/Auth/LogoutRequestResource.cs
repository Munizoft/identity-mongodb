using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Auth
{
    public class LogoutRequestResource
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        public LogoutRequestResource(Guid userId)
        {
            UserId = userId;
        }
    }
}