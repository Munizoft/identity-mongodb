﻿using Munizoft.Identity.Resources.Auth;
using Sole.Core.Models;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public interface IAuthService
    {
        /// <summary>
        ///     Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<LoginResponseResource>> LoginAsync(LoginRequestResource request);

        /// <summary>
        ///     Logout
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<LogoutResponseResource>> Logout(LogoutRequestResource request);
    }
}