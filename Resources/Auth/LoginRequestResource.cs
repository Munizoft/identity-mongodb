using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Auth
{
    public class LoginRequestResource
    {
        [JsonProperty("username")]
        public String UserName { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        [JsonProperty("user_agent")]
        public String UserAgent { get; set; }

        [JsonProperty("ipv4")]
        public String IPv4 { get; set; }

        [JsonProperty("ipv6")]
        public String IPv6 { get; set; }

        [JsonProperty("browser")]
        public String Browser { get; set; }

        [JsonProperty("browser_version")]
        public String BrowserVersion { get; set; }

        [JsonProperty("platform")]
        public String Platform { get; set; }

        [JsonProperty("platform_version")]
        public String PlatformVersion { get; set; }

        [JsonProperty("device")]
        public String Device { get; set; }

        [JsonProperty("device_version")]
        public String DeviceVersion { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}