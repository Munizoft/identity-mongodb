using Newtonsoft.Json;
using System;

namespace Munizoft.Identity.Infrastructure.Models
{
    public class ServiceError
    {
        [JsonProperty("code")]
        public String Code { get; private set; }

        [JsonProperty("message")]
        public String Message { get; private set; }

        [JsonProperty("description")]
        public String Description { get; private set; }

        public ServiceError(String message)
            : this(String.Empty, message, String.Empty)
        {

        }

        public ServiceError(String message, String description)
           : this(String.Empty, message, description)
        {

        }

        public ServiceError(String code, String message, String description)
        {
            Code = code;
            Message = message;
            Description = description;
        }
    }
}