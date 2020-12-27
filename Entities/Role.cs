using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;

namespace Munizoft.Identity.Entities
{
    [CollectionName("Roles")]
    public class Role : MongoIdentityRole<String>
    {
        public Role()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Role(string roleName) 
            : base(roleName)
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
