using AspNetCore.Identity.MongoDbCore;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDbGenericRepository;
using Munizoft.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Managers
{
    public class MongoUserManager<TUser, TRole, TContext> : UserManager<TUser>
        where TUser : MongoIdentityUser<String>, new()
        where TRole : MongoIdentityRole<String>, new()
        where TContext : MongoDbContext
    {
        private readonly MongoRoleStore<Role> _roleStore;

        public MongoUserManager(
            TContext context,
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _roleStore = new MongoRoleStore<Role>(context, errors);
        }

        public async Task<IdentityResult> ValidatePasswordAsync(TUser user, string password)
        {
            return await base.ValidatePasswordAsync(user, password);
        }

        public override async Task<IdentityResult> AddToRoleAsync(TUser user, string roleName)
        {
            IList<String> roles = null;

            if (!user.Roles.Any())
            {
                roles = await GetRolesAsync(user);
            }

            var hasRole = roles.Where(x => x.Trim().ToUpper() == roleName.Trim().ToUpper());
            if (hasRole.Any())
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Role {roleName} already assigned to the user" });
            }

            var currentRoles = _roleStore.Roles.ToList();
            if (!currentRoles.Where(w => w.Name.Trim().ToUpper() == roleName.Trim().ToUpper()).Any())
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"Role {roleName} not found" });
            }

            user.Roles.Add(roleName);

            await UpdateUserAsync(user);

            return IdentityResult.Success;
        }
    }
}
