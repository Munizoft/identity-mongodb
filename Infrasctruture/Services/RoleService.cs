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
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public class RoleService : BaseService<RoleService>, IRoleService
    {
        #region Fields
        private readonly MongoRoleStore<Role, IdentityContext> _roleStore;
        #endregion Fields

        #region Constructor
        public RoleService(
            ILogger<RoleService> logger,
            IMapper mapper,
            IOptions<Models.IdentityOptions> options,
            IdentityContext context)
            : base(logger, mapper, options)
        {
            _roleStore = new MongoRoleStore<Role, IdentityContext>(context, new IdentityErrorDescriber());
        }
        #endregion Constructor

        /// <summary>
        ///     Create Role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<RoleResource>> CreateAsync(RoleCreateRequestResource request)
        {
            try
            {
                var foundRole = await _roleStore.FindByNameAsync(request.NormalizedName);
                if (foundRole != null)
                {
                    var resource = _mapper.Map<Role, RoleResource>(foundRole);

                    return ServiceResult<RoleResource>.OK(resource);
                }

                var role = _mapper.Map<RoleCreateRequestResource, Role>(request);

                var result = await _roleStore.CreateAsync(role);

                if (result.Succeeded)
                {
                    var resource = _mapper.Map<Role, RoleResource>(role);

                    return ServiceResult<RoleResource>.OK(resource);
                }

                return ServiceResult<RoleResource>.Fail(result.Errors.ToServiceError());
            }
            catch (Exception ex)
            {
                return ServiceResult<RoleResource>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     Gey Role By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<RoleResource>> GetByIdAsync(GetByIdRequest<Guid> request)
        {
            try
            {
                var role = await _roleStore.FindByIdAsync(request.Id.ToString());

                if (role == null)
                {
                    return ServiceResult<RoleResource>.Fail(MessagesHelpers.ROLE_NOT_FOUND.ToServiceError());
                }

                var resource = _mapper.Map<Role, RoleResource>(role);

                return ServiceResult<RoleResource>.OK(resource);
            }
            catch (Exception ex)
            {
                return ServiceResult<RoleResource>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     List Roles
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<RoleResource>>> ListAsync()
        {
            try
            {
                var roles = _roleStore.Roles.ToList();

                var resource = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleResource>>(roles);

                return ServiceResult<IEnumerable<RoleResource>>.OK(resource);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<RoleResource>>.Fail(ex.ToServiceError());
            }
        }
    }
}