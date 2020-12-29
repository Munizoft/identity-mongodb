using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sole.Resources.Client;
using Sole.Services;
using System;
using System.Threading.Tasks;

namespace Munizoft.Identity.MongoDB.Controllers
{
    public class ClientsController : BaseController<ClientsController>
    {
        #region Fields
        public readonly IClientService _clientService;
        #endregion Fields

        #region Constructor
        public ClientsController(
            ILogger<ClientsController> logger,
            IClientService clientService
            )
            : base(logger)
        {
            _clientService = clientService;
        }
        #endregion Constructor

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ClientCreateResource request)
        {
            try
            {
                var result = await _clientService.CreateAsync(request);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditAsync(String id, ClientEditResource request)
        //{
        //    try
        //    {
        //        request.Id = id;

        //        var product = await _clientService.EditAsync(request);

        //        return Ok(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetsAsync(String id)
        {
            try
            {
                var result = await _clientService.GetByIdAsync(id);

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetsAsync()
        {
            try
            {
                var result = await _clientService.GetsAsync();

                if (result.Succeeded)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
