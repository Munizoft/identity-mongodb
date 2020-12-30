using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using Sole.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Munizoft.Identity.Entities
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<String>, ICreatedAt, ILastUpdatedAt, IDeletedAt
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }

        [NotMapped]
        public String DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName))
                    return String.Empty;

                if (!String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName))
                    return FirstName;

                if (!String.IsNullOrEmpty(FirstName) && !String.IsNullOrEmpty(LastName))
                    return $"{FirstName} {LastName}";

                return String.Empty;
            }
        }

        public String Timezone { get; set; }

        public DateTime? EmailConfirmedDateUtc { get; set; }

        public ICollection<Attribute> Attributes { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAtLocal { get { return CreatedAtUtc.ToLocalTime(); } }

        public DateTime LastUpdatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAtLocal
        {
            get
            {
                return LastUpdatedAtUtc.ToLocalTime();
            }
        }

        public DateTime? DeletedAtUtc { get; set; }
        public DateTime? DeletedAtLocal
        {
            get
            {
                if (DeletedAtUtc.HasValue)
                    return DeletedAtUtc.Value.ToLocalTime();
                return null;
            }
        }

        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public User(String email)
            : this()
        {
            this.UserName = email;
            this.Email = email;
            this.NormalizedEmail = email.ToUpper();
            this.NormalizedUserName = email.ToUpper();
        }

        public User(String username, String email = "")
            : this()
        {
            this.UserName = username;
            this.Email = !String.IsNullOrEmpty(email) ? email : username;
            this.NormalizedEmail = Email.ToUpper();
            this.NormalizedUserName = UserName.ToUpper();
        }
    }
}