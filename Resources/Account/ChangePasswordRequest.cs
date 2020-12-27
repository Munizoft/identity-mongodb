using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class ChangePasswordRequestResource
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonProperty("current_password")]
        public String CurrentPassword { get; set; }

        [JsonProperty("new_password")]
        public String NewPassword { get; set; }

        [JsonProperty("confirm_newpassword")]
        public String ConfirmNewPassword { get; set; }
    }
}