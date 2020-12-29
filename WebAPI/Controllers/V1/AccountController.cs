using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munizoft.Identity.Infrastructure.Helpers;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Account;
using System;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        private readonly IAccountService _accountService;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService
            )
            : base(logger)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<object> Register(RegisterRequestResource request)
        {
            try
            {
                var result = await this._accountService.RegisterAsync(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }
                else if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<object> ForgotPassword(ForgotPasswordRequestResource request)
        {
            try
            {
                var result = await this._accountService.ForgotPassword(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }
                else if (!result.Succeeded)
                {
                    return BadRequest(result);
                }

                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }

        [HttpPost("Confirm")]
        [AllowAnonymous]
        public async Task<object> Confirm(ConfirmAccountRequestResource request)
        {
            try
            {
                var result = await this._accountService.ConfirmAccount(request);
                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }
                else if (!result.Succeeded)
                {
                    return BadRequest(result);
                }

                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }

        [HttpPost("ChangePassword")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<object> ChangePassword(ChangePasswordRequestResource request)
        {
            try
            {
                request.UserId = this.UserId;
                var result = await this._accountService.ChangePassword(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }

        [HttpPost("SetPassword")]
        [AllowAnonymous]
        public async Task<object> SetPassword(SetPasswordRequestResource request)
        {
            try
            {
                var result = await this._accountService.SetPassword(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }
                else if (!result.Succeeded)
                {
                    return BadRequest(result);
                }

                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }

        [HttpGet("Me")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<Object> Me()
        {
            try
            {
                var request = new GetByIdRequest<Guid>(this.UserId);

                //var serviceResult = await this._accountService.GetUserByIdAsync(request);

                //if (serviceResult.Succeeded)
                //{
                //    if (serviceResult.Data == null)
                //        return NotFound();

                //    return Ok(serviceResult);
                //}

                return BadRequest();
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }

        [HttpPut("Me")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<Object> EditMe(EditAccountRequestResource request)
        {
            try
            {
                request.UserId = UserId;

                var result = await this._accountService.EditAccount(request);

                if (result.Succeeded)
                {
                    if (result.Data == null)
                        return NotFound();

                    return Ok(result.Data);
                }
                else if (!result.Succeeded)
                {
                    return BadRequest(result);
                }

                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
            catch (Exception ex)
            {
                return new ApplicationException(MessagesHelpers.UNKNOWN_ERROR);
            }
        }
    }
}