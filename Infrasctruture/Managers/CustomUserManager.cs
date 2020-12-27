using AspNetCore.Identity.MongoDbCore;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MongoDbGenericRepository;
using Munizoft.Identity.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Munizoft.Identity.Infrastructure.Managers
{
    //public class CustomUserManager<TUser, TContext> : MongoUserOnlyStore<TUser, TContext, String, Entities.UserClaim, Entities.UserLogin, Entities.UserToken>
    //    where TUser : MongoIdentityUser<String>, new()
    //    where TContext : IMongoDbContext
    //{
    //    /// <summary>
    //    /// The <see cref="ILogger"/> used to log messages from the manager.
    //    /// </summary>
    //    /// <value>
    //    /// The <see cref="ILogger"/> used to log messages from the manager.
    //    /// </value>
    //    public virtual ILogger Logger { get; set; }

    //    private bool _disposed;

    //    protected virtual CancellationToken CancellationToken => CancellationToken.None;

    //    public Models.IdentityOptions Options { get; }

    //    /// <summary>
    //    /// The data protection purpose used for the reset password related methods.
    //    /// </summary>
    //    public const string ResetPasswordTokenPurpose = "ResetPassword";

    //    /// <summary>
    //    /// The data protection purpose used for the change phone number methods.
    //    /// </summary>
    //    public const string ChangePhoneNumberTokenPurpose = "ChangePhoneNumber";

    //    /// <summary>
    //    /// The data protection purpose used for the email confirmation related methods.
    //    /// </summary>
    //    public const string ConfirmEmailTokenPurpose = "EmailConfirmation";

    //    /// <summary>
    //    /// The <see cref="ILookupNormalizer"/> used to normalize things like user and role names.
    //    /// </summary>
    //    public ILookupNormalizer KeyNormalizer { get; set; }

    //    private readonly Dictionary<string, IUserTwoFactorTokenProvider<TUser>> _tokenProviders =
    //                                            new Dictionary<string, IUserTwoFactorTokenProvider<TUser>>();

    //    /// <summary>
    //    /// Gets or sets the persistence store the manager operates over.
    //    /// </summary>
    //    /// <value>The persistence store the manager operates over.</value>
    //    protected internal IUserStore<TUser> Store { get; set; }

    //    /// <summary>
    //    /// Gets a flag indicating whether the backing user store supports security stamps.
    //    /// </summary>
    //    /// <value>
    //    /// true if the backing user store supports user security stamps, otherwise false.
    //    /// </value>
    //    public virtual bool SupportsUserSecurityStamp
    //    {
    //        get
    //        {
    //            ThrowIfDisposed();
    //            return Store is IUserSecurityStampStore<TUser>;
    //        }
    //    }

    //    public UserValidator<TUser> UserValidator { get; } = new UserValidator<TUser>();
    //    public PasswordValidator<TUser> PasswordValidator { get; } = new PasswordValidator<TUser>();
    //    public PasswordHasher<TUser> PasswordHasher { get; } = new PasswordHasher<TUser>();

    //    public CustomUserManager(TContext context, Models.IdentityOptions options)
    //        : this(context, options, new IdentityErrorDescriber())
    //    {
    //    }

    //    public CustomUserManager(TContext context, Models.IdentityOptions options, IdentityErrorDescriber errorDescriber)
    //        : base(context, errorDescriber)
    //    {
    //        Options = options;

    //        //    if (store == null)
    //        //    {
    //        //        throw new ArgumentNullException(nameof(store));
    //        //    }

    //        //    this.Store = store;

    //        //UserValidator = new UserValidator<TUser, Guid>(this);
    //        //PasswordValidator = new MinimumLengthValidator(6);
    //        //PasswordHasher = new PasswordHasher();
    //        //ClaimsIdentityFactory = new ClaimsIdentityFactory<TUser, Guid>();

    //        //this.EmailService = emailService;
    //    }

    //    public async Task<Boolean> IsEmailConfirmedAsync(TUser user)
    //    {
    //        return user.EmailConfirmed;
    //    }

    //    public async Task<IdentityResult> ValidatePasswordAsync(TUser user, String password)
    //    {
    //        if (password == null)
    //        {
    //            throw new ArgumentNullException(nameof(password));
    //        }

    //        var errors = new List<IdentityError>();

    //        var options = Options.Password;

    //        if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
    //        {
    //            errors.Add(this.ErrorDescriber.PasswordTooShort(options.RequiredLength));
    //        }

    //        if (options.RequireNonAlphanumeric && password.All(PasswordHelpers.IsLetterOrDigit))
    //        {
    //            errors.Add(this.ErrorDescriber.PasswordRequiresNonAlphanumeric());
    //        }

    //        if (options.RequireDigit && !password.Any(PasswordHelpers.IsDigit))
    //        {
    //            errors.Add(this.ErrorDescriber.PasswordRequiresDigit());
    //        }

    //        if (options.RequireLowercase && !password.Any(PasswordHelpers.IsLower))
    //        {
    //            errors.Add(this.ErrorDescriber.PasswordRequiresLower());
    //        }

    //        if (options.RequireUppercase && !password.Any(PasswordHelpers.IsUpper))
    //        {
    //            errors.Add(this.ErrorDescriber.PasswordRequiresUpper());
    //        }

    //        if (options.RequiredUniqueChars >= 1 && password.Distinct().Count() < options.RequiredUniqueChars)
    //        {
    //            errors.Add(this.ErrorDescriber.PasswordRequiresUniqueChars(options.RequiredUniqueChars));
    //        }

    //        return
    //            errors.Count == 0
    //                ? IdentityResult.Success
    //                : IdentityResult.Failed(errors.ToArray());
    //    }

    //    public async Task<IdentityResult> ChangePasswordAsync(TUser user, String currentPassword, String newPassword)
    //    {
    //        var userResult = await this.FindByIdAsync(user.Id);

    //        if (userResult == null)
    //        {
    //            var userNotFoundError = new IdentityError() { Description = "User not found" };
    //            return IdentityResult.Failed(userNotFoundError);
    //        }

    //        var currentPasswordHash = String.Empty;

    //        if (userResult.PasswordHash != currentPasswordHash)
    //        {
    //            var currentPasswordIncorrectError = new IdentityError() { Description = "Current Password is incorrect" };
    //            return IdentityResult.Failed(currentPasswordIncorrectError);
    //        }

    //        var newPasswordHash = String.Empty;

    //        await this.SetPasswordHashAsync(userResult, newPasswordHash);

    //        return IdentityResult.Success;
    //    }

    //    /// <summary>
    //    ///     Returns a flag indicating whether the specified <paramref name="token"/> is valid for
    //    ///     the given <paramref name="user"/> and <paramref name="purpose"/>.
    //    /// </summary>
    //    /// <param name="user">The user to validate the token against.</param>
    //    /// <param name="tokenProvider">The token provider used to generate the token.</param>
    //    /// <param name="purpose">The purpose the token should be generated for.</param>
    //    /// <param name="token">The token to validate</param>
    //    /// <returns>
    //    /// The <see cref="Task"/> that represents the asynchronous operation, returning true if the <paramref name="token"/>
    //    /// is valid, otherwise false.
    //    /// </returns>
    //    public virtual async Task<bool> VerifyUserTokenAsync(TUser user, string tokenProvider, string purpose, string token)
    //    {
    //        ThrowIfDisposed();

    //        if (user == null)
    //        {
    //            throw new ArgumentNullException(nameof(user));
    //        }

    //        if (tokenProvider == null)
    //        {
    //            throw new ArgumentNullException(nameof(tokenProvider));
    //        }

    //        if (!_tokenProviders.ContainsKey(tokenProvider))
    //        {
    //            throw new NotSupportedException("FormatNoTokenProvider(nameof(TUser), tokenProvider)");
    //        }

    //        // Make sure the token is valid
    //        //var result = await _tokenProviders[tokenProvider].ValidateAsync(purpose, token, this, user);

    //        //if (!result)
    //        //{
    //        //    Logger.LogWarning(9, "VerifyUserTokenAsync() failed with purpose: {purpose} for user {userId}.", purpose, await GetUserIdAsync(user));
    //        //}

    //        return true;
    //    }

    //    public virtual async Task<IdentityResult> ResetPasswordAsync(TUser user, String token, String newPassword)
    //    {
    //        ThrowIfDisposed();

    //        if (user == null)
    //        {
    //            throw new ArgumentNullException(nameof(user));
    //        }

    //        // Make sure the token is valid and the stamp matches
    //        if (!await VerifyUserTokenAsync(user, Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token))
    //        {
    //            return IdentityResult.Failed(ErrorDescriber.InvalidToken());
    //        }
    //        var result = await UpdatePasswordHash(user, newPassword, validatePassword: true);
    //        if (!result.Succeeded)
    //        {
    //            return result;
    //        }

    //        return await UpdateUserAsync(user);
    //    }

    //    /// <summary>
    //    /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
    //    /// called before saving the user via Create or Update.
    //    /// </summary>
    //    /// <param name="user">The user</param>
    //    /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
    //    protected async Task<IdentityResult> ValidateUserAsync(TUser user)
    //    {
    //        if (SupportsUserSecurityStamp)
    //        {
    //            var stamp = await GetSecurityStampAsync(user);
    //            if (stamp == null)
    //            {
    //                throw new InvalidOperationException("Resources.NullSecurityStamp");
    //            }
    //        }

    //        var errors = new List<IdentityError>();

    //        foreach (var v in UserValidators)
    //        {
    //            var result = await v.ValidateAsync(this, user);
    //            if (!result.Succeeded)
    //            {
    //                errors.AddRange(result.Errors);
    //            }
    //        }

    //        if (errors.Count > 0)
    //        {
    //            Logger.LogWarning(13, "User {userId} validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
    //            return IdentityResult.Failed(errors.ToArray());
    //        }

    //        return IdentityResult.Success;
    //    }

    //    /// <summary>
    //    /// Called to update the user after validating and updating the normalized email/user name.
    //    /// </summary>
    //    /// <param name="user">The user.</param>
    //    /// <returns>Whether the operation was successful.</returns>
    //    protected virtual async Task<IdentityResult> UpdateUserAsync(TUser user)
    //    {
    //        var result = await ValidateUserAsync(user);
    //        if (!result.Succeeded)
    //        {
    //            return result;
    //        }
    //        await UpdateNormalizedUserNameAsync(user);
    //        await UpdateNormalizedEmailAsync(user);
    //        return await Store.UpdateAsync(user, CancellationToken);
    //    }

    //    /// <summary>
    //    ///     Updates a user's password hash.
    //    /// </summary>
    //    /// <param name="user">The user.</param>
    //    /// <param name="newPassword">The new password.</param>
    //    /// <param name="validatePassword">Whether to validate the password.</param>
    //    /// <returns>Whether the password has was successfully updated.</returns>        
    //    private async Task<IdentityResult> UpdatePasswordHash(TUser user, string newPassword, bool validatePassword = true)
    //    {
    //        if (validatePassword)
    //        {
    //            var validate = await ValidatePasswordAsync(user, newPassword);
    //            if (!validate.Succeeded)
    //            {
    //                return validate;
    //            }
    //        }

    //        var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;

    //        await this.SetPasswordHashAsync(user, hash, CancellationToken);
    //        //await this.UpdateSecurityStampInternal(user);

    //        return IdentityResult.Success;
    //    }

    //    /// <summary>
    //    ///     Throws if this class has been disposed.
    //    /// </summary>
    //    protected void ThrowIfDisposed()
    //    {
    //        if (this._disposed)
    //        {
    //            throw new ObjectDisposedException(GetType().Name);
    //        }
    //    }

    //    public virtual void Dispose()
    //    {
    //        base.Dispose();
    //        this._disposed = true;
    //    }

    //    public async Task SaveChanges(CancellationToken cancellationToken)
    //    {
    //        await base.SaveChanges(cancellationToken);
    //    }
    //}
}