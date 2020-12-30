using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Account;
using System;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class AccountController : BaseController<AccountController>
    {
        #region Fields
        private readonly IAccountService _accountService;
        #endregion Fields

        #region Constructor
        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService
            )
            : base(logger)
        {
            _accountService = accountService;
        }
        #endregion Constructor

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestResource request)
        {
            try
            {
                var result = await this._accountService.RegisterAsync(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestResource request)
        {
            try
            {
                var result = await this._accountService.ForgotPassword(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }

        [HttpPost("Confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> Confirm(ConfirmAccountRequestResource request)
        {
            try
            {
                var result = await this._accountService.ConfirmAccount(request);
                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }

        [HttpPost("ChangePassword")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestResource request)
        {
            try
            {
                request.UserId = this.UserId;
                var result = await this._accountService.ChangePassword(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }

        [HttpPost("SetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> SetPassword(SetPasswordRequestResource request)
        {
            try
            {
                var result = await this._accountService.SetPassword(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }

        [HttpGet("Me")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<IActionResult> Me()
        {
            try
            {
                var request = new GetByIdRequest<Guid>(this.UserId);


                return BadRequest();
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }

        [HttpPut("Me")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<IActionResult> EditMe(EditAccountRequestResource request)
        {
            try
            {
                request.UserId = UserId;

                var result = await this._accountService.EditAccount(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return GenericError(ex);
            }
        }
    }
}