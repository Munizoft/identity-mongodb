using AspNetCore.Identity.MongoDbCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Munizoft.Identity.Entities;
using Munizoft.Identity.Infrastructure.Extensions;
using Munizoft.Identity.Infrastructure.Helpers;
using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Persistence.MongoDB;
using Munizoft.Identity.Resources.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public class UserService : BaseService<UserService>, IUserService
    {
        private readonly MongoUserStore<User, Role, IdentityContext> _userStore;

        public UserService(
            ILogger<UserService> logger,
            IMapper mapper,
            IOptions<Models.IdentityOptions> options,
            IdentityContext context)
            : base(logger, mapper, options)
        {
            _userStore = new MongoUserStore<User, Role, IdentityContext>(context, new IdentityErrorDescriber());
        }

        /// <summary>
        ///     Create User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserResource>> CreateAsync(UserCreateRequestResource request)
        {
            User user = null;

            try
            {
                var foundUsername = await _userStore.FindByNameAsync(request.NormalizedUserName);

                if (foundUsername != null)
                {
                    return ServiceResult<UserResource>.Fail(MessagesHelpers.USER_EXISTS.ToServiceError());
                }

                var foundEmail = await _userStore.FindByEmailAsync(request.NormalizedEmail);

                if (foundEmail != null)
                {
                    return ServiceResult<UserResource>.Fail(MessagesHelpers.USER_EXISTS.ToServiceError());
                }

                user = _mapper.Map<UserCreateRequestResource, User>(request);

                user.Id = Guid.NewGuid().ToString();

                var result = await _userStore.CreateAsync(user);

                if (request.Roles.Any())
                {
                    foreach (var role in request.Roles)
                    {
                        //user.Roles.Add(role.ToUpper());
                        await _userStore.AddToRoleAsync(user, role);
                    }
                }

                if (result.Succeeded)
                {
                    var resource = _mapper.Map<User, UserResource>(user);

                    return ServiceResult<UserResource>.OK(resource);
                }

                return ServiceResult<UserResource>.Fail(result.Errors.ToServiceError());
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    await _userStore.DeleteAsync(user);
                }

                return ServiceResult<UserResource>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     List Users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<UserResource>>> ListAsync()
        {
            try
            {
                var users = _userStore.Users.ToList();

                var resource = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

                return ServiceResult<IEnumerable<UserResource>>.OK(resource);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<UserResource>>.Fail(ex.ToServiceError());
            }
        }
    }
}