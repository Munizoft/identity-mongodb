using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.User;
using System;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class UsersController : BaseController<UsersController>
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion Fields

        #region Constructor
        public UsersController(
            ILogger<UsersController> logger,
            IUserService userService)
            : base(logger)
        {
            _userService = userService;
        }
        #endregion Constructor

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.ListAsync();

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var request = new GetByIdRequest<Guid>(id);

            var result = await _userService.GetByIdAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}/attributes")]
        [Authorize]
        public async Task<IActionResult> GetAttributesByUserId(Guid id)
        {
            var request = new GetByIdRequest<Guid>(id);

            var result = await _userService.GetAttributesByUserIdAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}/attributes/type/{type}")]
        [Authorize]
        public async Task<IActionResult> GetAttributesByUserIdAndType(Guid id, String type)
        {
            var request = new GetAttributesByUserIdAndTypeRequestResource<Guid>(id, type);

            var result = await _userService.GetAttributesByUserIdAndTypeAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserAsync(UserCreateRequestResource request)
        {
            var result = await _userService.CreateAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }
    }
}