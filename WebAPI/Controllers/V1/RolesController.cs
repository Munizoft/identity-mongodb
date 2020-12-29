using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Munizoft.Identity.Infrastructure.Services;
using Munizoft.Identity.Resources;
using Munizoft.Identity.Resources.Role;
using System;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class RolesController : BaseController<RolesController>
    {
        #region Fields
        private readonly IRoleService _roleService;
        #endregion Fields

        #region Constructor
        public RolesController(
            ILogger<RolesController> logger,
            IRoleService roleService
            )
            : base(logger)
        {
            _roleService = roleService;
        }
        #endregion Constructor

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.ListAsync();

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRoles(Guid id)
        {
            var request = new GetByIdRequest<Guid>(id);

            var result = await _roleService.GetByIdAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(RoleCreateRequestResource request)
        {
            var result = await _roleService.CreateAsync(request);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Errors);
        }
    }
}