using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Resources.User;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class UsersController : BaseController<UsersController>
    {
        private readonly IUserService _userService;

        public UsersController(
            ILogger<UsersController> logger,
            IUserService userService)
            : base(logger)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.ListAsync();

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(UserCreateRequestResource request)
        {
            var result = await _userService.CreateAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Item3);
        }
    }
}