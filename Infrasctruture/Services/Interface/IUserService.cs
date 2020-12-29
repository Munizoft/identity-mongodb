using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Common;
using Munizoft.Identity.Resources.User;
using Munizoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public interface IUserService
    {
        /// <summary>
        ///     Create User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<UserResource>> CreateAsync(UserCreateRequestResource request);

        /// <summary>
        ///     Gey User By Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<UserResource>> GetByIdAsync(GetByIdRequest<Guid> request);

        /// <summary>
        ///     List Users
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<UserResource>>> ListAsync();

        /// <summary>
        ///     Gey Attributes By User Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<AttributeResource>>> GetAttributesByUserIdAsync(GetByIdRequest<Guid> request);

        /// <summary>
        ///     Gey Attributes By User Id and Type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<IEnumerable<AttributeResource>>> GetAttributesByUserIdAndTypeAsync(GetAttributesByUserIdAndTypeRequestResource<Guid> request);
    }
}