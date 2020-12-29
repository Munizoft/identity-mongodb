using AspNetCore.Identity.MongoDbCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Munizoft.Identity.Entities;
using Munizoft.Identity.Infrastructure.Extensions;
using Munizoft.Identity.Infrastructure.Helpers;
using Munizoft.Identity.Infrastructure.Managers;
using Munizoft.Identity.Infrastructure.Models;
using Munizoft.Identity.Persistence.MongoDB;
using Munizoft.Identity.Resources.Account;
using Munizoft.Identity.Resources.User;
using Munizoft.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public class AccountService : BaseService<AccountService>, IAccountService
    {
        private readonly MongoUserManager<User, Role, IdentityContext> _userManager;

        public AccountService(
            ILogger<AccountService> logger,
            IMapper mapper,
            IOptions<Models.IdentityOptions> options,
            IdentityContext context,
            IServiceProvider services)
            : base(logger, mapper, options)
        {
            var store = new MongoUserStore<User, Role, IdentityContext>(context, new IdentityErrorDescriber());

            var passwordHasher = new PasswordHasher<User>();

            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var errorDescriber = new IdentityErrorDescriber();
            var userValidators = new List<IUserValidator<User>>() { new UserValidator<User>() };
            var passwordValidators = new List<IPasswordValidator<User>>() { new PasswordValidator<User>() };

            if (services != null)
            {
                foreach (var providerName in options.Value.Tokens.ProviderMap.Keys)
                {
                    var description = options.Value.Tokens.ProviderMap[providerName];

                    var provider = (description.ProviderInstance ?? services.GetRequiredService(description.ProviderType))
                        as IUserTwoFactorTokenProvider<User>;

                    if (provider != null)
                    {
                        //RegisterTokenProvider(providerName, provider);
                    }
                }
            }

            if (options.Value.Stores.ProtectPersonalData)
            {
                if (!(store is IProtectedUserStore<User>))
                {
                    throw new InvalidOperationException("StoreNotIProtectedUserStore");
                }

                if (services.GetService<ILookupProtector>() == null)
                {
                    throw new InvalidOperationException("NoPersonalDataProtector");
                }
            }

            var userManagerLogger = new LoggerFactory().CreateLogger<UserManager<User>>();

            _userManager = new MongoUserManager<User, Role, IdentityContext>(context, store, options, passwordHasher, userValidators, passwordValidators, keyNormalizer, errorDescriber, services, userManagerLogger);

        }

        /// <summary>
        ///     Register an account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<UserResource>> RegisterAsync(RegisterRequestResource request)
        {
            try
            {
                //if (!request.Validate<RegisterRequestValidator, RegisterRequesResourcet>().IsValid)
                //{
                //    var validations = request.Validate<RegisterRequestValidator, RegisterRequest>();
                //    return ServiceResult<UserResource>.Fail(validations.ToArray());
                //}

                var user = this._mapper.Map<RegisterRequestResource, Entities.User>(request);

                var result = await _userManager.CreateAsync(user).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    if (request.Roles.Any())
                    {
                        foreach (var role in request.Roles)
                        {
                            await _userManager.AddToRoleAsync(user, role).ConfigureAwait(false);
                        }
                    }

                    if (!String.IsNullOrEmpty(request.Password))
                    {
                        var passwordResult = await _userManager.AddPasswordAsync(user, request.Password).ConfigureAwait(false);
                        if (!passwordResult.Succeeded)
                        {
                            return ServiceResult<UserResource>.Fail(passwordResult.Errors.ToServiceError());
                        }
                    }

                    var registerMapped = this._mapper.Map<Entities.User, UserResource>(user);

                    //_userManager.SaveChanges();
                    //await SendEmailConfirmation(request.Url.AbsoluteUri, userMapped);

                    return ServiceResult<UserResource>.OK(registerMapped);
                }
                else
                {
                    return ServiceResult<UserResource>.Fail(MessagesHelpers.USER_EXISTS, result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<UserResource>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     Confirm Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ConfirmAccountResponseResource>> ConfirmAccount(ConfirmAccountRequestResource request)
        {
            try
            {
                //if (!request.Validate<ConfirmAccountRequestValidator, ConfirmAccountRequest>().IsValid)
                //{
                //    var validations = request.Validate<ConfirmAccountRequestValidator, ConfirmAccountRequest>();
                //    return ServiceResult<ConfirmAccountResponse>.Fail(validations.ToArray());
                //}

                var user = await _userManager.FindByNameAsync(request.UserName.ToString()).ConfigureAwait(false);

                if (user == null)
                {
                    return ServiceResult<ConfirmAccountResponseResource>.Fail(MessagesHelpers.USER_NOT_FOUND, $"{request.UserName} not found");
                }

                var passwordErrors = await _userManager.ValidatePasswordAsync(user, request.Password).ConfigureAwait(false);
                if (passwordErrors.Errors.Any())
                {
                    return ServiceResult<ConfirmAccountResponseResource>.Fail(passwordErrors.Errors.ToServiceError());
                }

                user.EmailConfirmedDateUtc = DateTime.UtcNow;

                var token = System.Web.HttpUtility.UrlDecode(request.Token);
                //var ConfirmEmailResult = await this._userManager.ConfirmEmailAsync(user, token).ConfigureAwait(false);
                //if (ConfirmEmailResult.Succeeded)
                //{
                //    user.PasswordHash = this._userManager.PasswordHasher.HashPassword(user, request.Password);
                //    var result = await _userManager.UpdateAsync(user);
                //    if (result.Succeeded)
                //    {
                //        var response = new ConfirmAccountResponseResource()
                //        {
                //            Token = token,
                //            Username = user.UserName
                //        };

                //        return ServiceResult<ConfirmAccountResponseResource>.OK(response);
                //    }
                //    else
                //    {
                //        return ServiceResult<ConfirmAccountResponseResource>.Fail(MessagesHelpers.INVALID_PASSWORD, $"Password is invalid");
                //    }
                //}

                return ServiceResult<ConfirmAccountResponseResource>.Fail(MessagesHelpers.INVALID_TOKEN, $"Token is invalid");
            }
            catch (Exception ex)
            {
                return ServiceResult<ConfirmAccountResponseResource>.Fail(MessagesHelpers.UNKNOWN_ERROR, ex.Message);
            }
        }

        /// <summary>
        ///     Set Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<SetPasswordResponseResource>> SetPassword(SetPasswordRequestResource request)
        {
            try
            {
                //if (!request.Validate<SetPasswordRequestValidator, SetPasswordRequest>().IsValid)
                //{
                //    var validations = request.Validate<SetPasswordRequestValidator, SetPasswordRequest>();
                //    return ServiceResult<SetPasswordResponse>.Fail(validations.ToArray());
                //}

                var user = await _userManager.FindByNameAsync(request.UserName.ToString());
                if (user == null)
                {
                    return ServiceResult<SetPasswordResponseResource>.Fail(MessagesHelpers.USER_NOT_FOUND, $"{request.UserName} not found");
                }

                user.LockoutEnd = null;
                user.AccessFailedCount = 0;

                var token = System.Web.HttpUtility.UrlDecode(request.Token);
                var result = await _userManager.ResetPasswordAsync(user, token, request.Password).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    var response = new SetPasswordResponseResource()
                    {
                        Token = token,
                        Username = user.UserName
                    };

                    return ServiceResult<SetPasswordResponseResource>.OK(response);
                }

                return ServiceResult<SetPasswordResponseResource>.Fail(result.Errors.ToServiceError());
            }
            catch (Exception ex)
            {
                return ServiceResult<SetPasswordResponseResource>.Fail(MessagesHelpers.UNKNOWN_ERROR, ex.Message);
            }
        }

        /// <summary>
        ///     Change Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ChangePasswordResponseResource>> ChangePassword(ChangePasswordRequestResource request)
        {
            try
            {
                //if (!request.Validate<ChangePasswordRequestValidator, ChangePasswordRequest>().IsValid)
                //{
                //    var validations = request.Validate<ChangePasswordRequestValidator, ChangePasswordRequest>();
                //    return ServiceResult<ChangePasswordResponse>.Fail(validations.ToArray());
                //}

                var user = await _userManager.FindByIdAsync(request.UserId.ToString()).ConfigureAwait(false);
                if (user == null)
                {
                    return ServiceResult<ChangePasswordResponseResource>.Fail(MessagesHelpers.USER_NOT_FOUND, $"UserId {request.UserId} not found");
                }

                var passwordErrors = await _userManager.ValidatePasswordAsync(user, request.NewPassword).ConfigureAwait(false);
                if (passwordErrors.Errors.Any())
                {
                    return ServiceResult<ChangePasswordResponseResource>.Fail(passwordErrors.Errors.ToServiceError());
                }

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    return ServiceResult<ChangePasswordResponseResource>.OK(new ChangePasswordResponseResource());
                }

                return ServiceResult<ChangePasswordResponseResource>.Fail(result.Errors.ToServiceError());
            }
            catch (Exception ex)
            {
                return ServiceResult<ChangePasswordResponseResource>.Fail(MessagesHelpers.UNKNOWN_ERROR, ex.Message);
            }
        }

        /// <summary>
        ///     Forgot Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<ForgotPasswordResponseResource>> ForgotPassword(ForgotPasswordRequestResource request)
        {
            try
            {
                //if (!request.Validate<ForgotPasswordRequestValidator, ForgotPasswordRequest>().IsValid)
                //{
                //    var validations = request.Validate<ForgotPasswordRequestValidator, ForgotPasswordRequest>();
                //    return ServiceResult<ForgotPasswordResponse>.Fail(validations.ToArray());
                //}

                var user = await this._userManager.FindByNameAsync(request.NormalizedUserName);

                var isEmailConfirmedResult = await this._userManager.IsEmailConfirmedAsync(user);

                if (user == null)
                {
                    return ServiceResult<ForgotPasswordResponseResource>.Fail(MessagesHelpers.USER_NOT_FOUND, $"{request.UserName} not found");
                }

                //await SendForgotPassword(request.Url.AbsoluteUri, user);

                return ServiceResult<ForgotPasswordResponseResource>.OK(new ForgotPasswordResponseResource());
            }
            catch (Exception ex)
            {
                return ServiceResult<ForgotPasswordResponseResource>.Fail(MessagesHelpers.UNKNOWN_ERROR, ex.Message);
            }
        }

        /// <summary>
        ///     Edit Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<EditAccountResponseResource>> EditAccount(EditAccountRequestResource request)
        {
            try
            {
                //if (!request.Validate<EditAccountRequestValidator, EditAccountRequest>().IsValid)
                //{
                //    var validations = request.Validate<EditAccountRequestValidator, EditAccountRequest>();
                //    return ServiceResult<EditAccountResponse>.Fail(validations.ToArray());
                //}

                var user = await this._userManager.FindByIdAsync(request.UserId.ToString());

                if (user == null)
                {
                    return ServiceResult<EditAccountResponseResource>.Fail(MessagesHelpers.USER_NOT_FOUND, $"UserId: {request.UserId} not found");
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.LastUpdatedAtUtc = DateTime.UtcNow;

                var identityResult = await this._userManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    var usersMapped = this._mapper.Map<Entities.User, EditAccountResponseResource>(user);
                    return ServiceResult<EditAccountResponseResource>.OK(usersMapped);
                }

                return ServiceResult<EditAccountResponseResource>.Fail(MessagesHelpers.UNKNOWN_ERROR, "Error updating user");
            }
            catch (Exception ex)
            {
                return ServiceResult<EditAccountResponseResource>.Fail(MessagesHelpers.UNKNOWN_ERROR, ex.Message);
            }
        }
    }
}