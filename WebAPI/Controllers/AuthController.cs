using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Resources.Auth;
using System;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class AuthController : BaseController<AuthController>
    {
        private readonly IAuthService _authService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService
            )
            : base(logger)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestResource request)
        {
            try
            {
                var result = await this._authService.LoginAsync(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
            }
        }

        [HttpPost("logout")]
        [Authorize(Policy = "RequireAnyRole")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var request = new LogoutRequestResource(this.UserId);

                var result = await this._authService.Logout(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
            }
        }
    }
}