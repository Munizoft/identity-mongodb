using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Role;
using Munizoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public interface IRoleService
    {
        /// <summary>
        ///     Create Role
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<RoleResource>> CreateAsync(RoleCreateRequestResource request);

        /// <summary>
        ///     Gey Role By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<RoleResource>> GetByIdAsync(GetByIdRequest<Guid> request);

        /// <summary>
        ///     List Roles
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<RoleResource>>> ListAsync();
    }
}
