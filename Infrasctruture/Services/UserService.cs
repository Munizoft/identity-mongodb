using AspNetCore.Identity.MongoDbCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Munizoft.Identity.Entities;
using Munizoft.Identity.Infrastructure.Extensions;
using Munizoft.Identity.Infrastructure.Helpers;
using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Infrastructure.Validations.User;
using Munizoft.Identity.Persistence.MongoDB;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Common;
using Munizoft.Identity.Resources.User;
using Munizoft.Identity.Validations.Extensions;
using Munizoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public class UserService : BaseService<UserService>, IUserService
    {
        #region Fields
        private readonly MongoUserStore<User, Role, IdentityContext> _userStore;
        #endregion Fields

        #region Constructor
        public UserService(
            ILogger<UserService> logger,
            IMapper mapper,
            IOptions<Models.IdentityOptions> options,
            IdentityContext context)
            : base(logger, mapper, options)
        {
            _userStore = new MongoUserStore<User, Role, IdentityContext>(context, new IdentityErrorDescriber());
        }
        #endregion Constructor

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
        ///     Gey User By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserResource>> GetByIdAsync(GetByIdRequest<Guid> request)
        {
            try
            {
                var user = await _userStore.FindByIdAsync(request.Id.ToString());

                if (user == null)
                {
                    return ServiceResult<UserResource>.Fail(MessagesHelpers.USER_NOT_FOUND.ToServiceError());
                }

                var resource = _mapper.Map<User, UserResource>(user);

                return ServiceResult<UserResource>.OK(resource);
            }
            catch (Exception ex)
            {
                return ServiceResult<UserResource>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     Gey Attributes By User Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<AttributeResource>>> GetAttributesByUserIdAsync(GetByIdRequest<Guid> request)
        {
            try
            {
                if (!request.Validate<GetByIdRequestValidator<Guid>, GetByIdRequest<Guid>>().IsValid)
                {
                    var validations = request.Validate<GetByIdRequestValidator<Guid>, GetByIdRequest<Guid>>();
                    return ServiceResult<IEnumerable<AttributeResource>>.Fail(validations.ToServiceError());
                }

                var user = await _userStore.FindByIdAsync(request.Id.ToString());

                if (user == null)
                {
                    return ServiceResult<IEnumerable<AttributeResource>>.Fail(MessagesHelpers.USER_NOT_FOUND.ToServiceError());
                }

                var resource = _mapper.Map<IEnumerable<Entities.Attribute>, IEnumerable<AttributeResource>>(user.Attributes.ToArray());

                return ServiceResult<IEnumerable<AttributeResource>>.OK(resource);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<AttributeResource>>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     Gey Attributes By User Id and Type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<AttributeResource>>> GetAttributesByUserIdAndTypeAsync(GetAttributesByUserIdAndTypeRequestResource<Guid> request)
        {
            try
            {
                if (!request.Validate<GetByIdRequestValidatior<Guid>, GetAttributesByUserIdAndTypeRequestResource<Guid>>().IsValid)
                {
                    var validations = request.Validate<GetByIdRequestValidatior<Guid>, GetAttributesByUserIdAndTypeRequestResource<Guid>>();
                    return ServiceResult<IEnumerable<AttributeResource>>.Fail(validations.ToServiceError());
                }

                var user = await _userStore.FindByIdAsync(request.Id.ToString());

                if (user == null)
                {
                    return ServiceResult<IEnumerable<AttributeResource>>.Fail(MessagesHelpers.USER_NOT_FOUND.ToServiceError());
                }

                var types = user.Attributes.Where(w => w.Type == request.Type);

                var resource = _mapper.Map<IEnumerable<Entities.Attribute>, IEnumerable<AttributeResource>>(types);

                return ServiceResult<IEnumerable<AttributeResource>>.OK(resource);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<AttributeResource>>.Fail(ex.ToServiceError());
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