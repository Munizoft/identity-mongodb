using System;

namespace Munizoft.Identity.Infrastructure.Models
{
    public class JwtOptions
    {
        public String Key { get; set; }
        public String Issuer { get; set; }
        public Int32 ExpireDays { get; set; }
    }
}
