using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Resources.Account
{
    public class ChangePasswordResponseResource
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        [JsonProperty("currentPassword ")]
        public String CurrentPassword { get; set; }

        [JsonProperty("newPassword")]
        public String NewPassword { get; set; }

        [JsonProperty("confirmNewPassword")]
        public String ConfirmNewPassword { get; set; }
    }
}