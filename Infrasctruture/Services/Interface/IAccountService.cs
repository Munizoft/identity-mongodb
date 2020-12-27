using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Resources.Account;
using Munizoft.Identity.Resources.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public interface IAccountService
    {
        /// <summary>
        ///     Register an account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<UserResource>> RegisterAsync(RegisterRequestResource request);

        /// <summary>
        ///     Confirm Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<ConfirmAccountResponseResource>> ConfirmAccount(ConfirmAccountRequestResource request);

        /// <summary>
        ///     Set Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<SetPasswordResponseResource>> SetPassword(SetPasswordRequestResource request);

        /// <summary>
        ///     Change Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<ChangePasswordResponseResource>> ChangePassword(ChangePasswordRequestResource request);

        /// <summary>
        ///     Forgot Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<ForgotPasswordResponseResource>> ForgotPassword(ForgotPasswordRequestResource request);

        /// <summary>
        ///     Edit Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<EditAccountResponseResource>> EditAccount(EditAccountRequestResource request);
    }
}