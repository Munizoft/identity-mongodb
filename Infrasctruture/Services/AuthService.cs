using AspNetCore.Identity.MongoDbCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Munizoft.Identity.Entities;
using Munizoft.Identity.Infrastructure.Helpers;
using Munizoft.Identity.Infrastructure.Managers;
using Munizoft.Identity.Infrastructure.Validations;
using Munizoft.Identity.Infrastructure.Validations.Auth;
using Munizoft.Identity.Persistence.MongoDB;
using Munizoft.Identity.Resources.Auth;
using Munizoft.Identity.Validations.Extensions;
using Sole.Core.Extensions;
using Sole.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Services
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        #region Fields
        private readonly MongoUserManager<User, Role, IdentityContext> _userManager;
        private readonly IOptions<Models.JwtOptions> _jwtOptions;
        #endregion Fields

        #region Constructor
        public AuthService(
            ILogger<AuthService> logger,
            IMapper mapper,
            IOptions<Models.IdentityOptions> options,
            IOptions<Models.JwtOptions> jwtOptions,
            IdentityContext context,
            IServiceProvider services)
            : base(logger, mapper, options)
        {
            _jwtOptions = jwtOptions;
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
        #endregion Constructor

        /// <summary>
        ///     Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LoginResponseResource>> LoginAsync(LoginRequestResource request)
        {
            try
            {
                if (!request.Validate<LoginRequestValidator, LoginRequestResource>().IsValid)
                {
                    var validations = request.Validate<LoginRequestValidator, LoginRequestResource>();
                    return ServiceResult<LoginResponseResource>.Fail(validations.ToServiceError());
                }

                var user = await this._userManager.FindByNameAsync(request.UserName);

                if (user == null)
                {
                    return ServiceResult<LoginResponseResource>.Fail(MessagesHelpers.USER_OR_PASSWORD_INVALID.ToServiceError()); ;
                }

                var passwordValid = await this._userManager.CheckPasswordAsync(user, request.Password);

                if (!passwordValid)
                {
                    return ServiceResult<LoginResponseResource>.Fail(MessagesHelpers.USER_OR_PASSWORD_INVALID.ToServiceError()); ;
                }

                var roles = await _userManager.GetRolesAsync(user);

                var loginResponse = await JwtHelpers.GenerateToken(_jwtOptions.Value, user, roles.ToList());

                return ServiceResult<LoginResponseResource>.OK(loginResponse);
            }
            catch (Exception ex)
            {
                return ServiceResult<LoginResponseResource>.Fail(ex.ToServiceError());
            }
        }

        /// <summary>
        ///     Logout
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult<LogoutResponseResource>> Logout(LogoutRequestResource request)
        {
            try
            {
                if (!request.Validate<LogoutRequestValidator, LogoutRequestResource>().IsValid)
                {
                    var validations = request.Validate<LogoutRequestValidator, LogoutRequestResource>();
                    return ServiceResult<LogoutResponseResource>.Fail(validations.ToServiceError());
                }

                var user = await _userManager.FindByIdAsync(request.UserId.ToString());

                if (user == null)
                {
                    return ServiceResult<LogoutResponseResource>.Fail(MessagesHelpers.USER_NOT_FOUND.ToServiceError());
                }

                //var lastLogin = this._unitOfWork.UserLoginHistoryRepository.Find(x => x.UserId == user.Id && x.LogOutDateUtc == null).OrderBy(o => o.LogInDateUtc).FirstOrDefault();

                //lastLogin.LogOutDateUtc = DateTime.UtcNow;

                return ServiceResult<LogoutResponseResource>.OK(null);
            }
            catch (Exception ex)
            {
                return ServiceResult<LogoutResponseResource>.Fail(ex.ToServiceError());
            }
        }

        private async Task<ServiceResult<T>> checkIfUsercanLogin<T>(Entities.User user, String url)
        {
            if (user == null)
            {
                return ServiceResult<T>.Fail(MessagesHelpers.USER_NOT_FOUND.ToServiceError());
            }

            //if (!user.Active)
            //{
            //    return (false, MessagesHelpers.USER_INACTIVE);
            //}

            //if (await _signInManager.IsLockedOut(user))
            //{
            //    await SendUserLockout(url, user);

            //    return (false, MessagesHelpers.USER_LOCKEDOUT);
            //}

            //if (!await _signInManager.HasSetUpPassword(user))
            //{
            //    return (false, MessagesHelpers.USER_PASSWORD_NOT_SETUP);
            //}

            //if (!await _signInManager.HasEmailConfirmed(user))
            //{
            //    return (false, MessagesHelpers.USER_NOT_CONFIRMED);
            //}

            return ServiceResult<T>.OK(Activator.CreateInstance<T>());
        }
    }
}