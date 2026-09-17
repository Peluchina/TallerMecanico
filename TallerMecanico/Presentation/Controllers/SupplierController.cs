using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;
using TallerMecanico.Application.Queries;

namespace TallerMecanico.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierCreateCommandHandler _createCommandHandler;
        private readonly SupplierUpdateCommandHandler _updateCommandHandler;
        private readonly SupplierUpdateStatusCommandHandler _updateStatusCommandHandler;
        private readonly SupplierGetQueryHandler _getQueryHandler;
        private readonly SupplierListQueryHandler _listQueryHandler;

        public SupplierController(
            SupplierCreateCommandHandler createCommandHandler,
            SupplierUpdateCommandHandler updateCommandHandler,
            SupplierUpdateStatusCommandHandler updateStatusCommandHandler,
            SupplierGetQueryHandler getQueryHandler,
            SupplierListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _updateStatusCommandHandler = updateStatusCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierListResponse>>> GetAll()
        {
            var query = new SupplierListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{SupplierId:long}")]
        public async Task<ActionResult<SupplierGetResponse>> GetById([FromRoute] SupplierGetParams @params)
        {
            var query = new SupplierGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Proveedor con ID {@params.SupplierId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierCreateResponse>> Create([FromBody] SupplierCreateRequest request)
        {
            var command = new SupplierCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { SupplierId = response.SupplierId }, response);
        }

        [HttpPut("{SupplierId:long}")]
        public async Task<ActionResult<SupplierGetResponse>> Update(
            [FromRoute] SupplierUpdateParams @params,
            [FromBody] SupplierUpdateRequest request)
        {
            var command = new SupplierUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Proveedor con ID {@params.SupplierId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{SupplierId:long}/status")]
        public async Task<ActionResult<SupplierGetResponse>> UpdateStatus(
            [FromRoute] SupplierUpdateStatusParams @params,
            [FromBody] SupplierUpdateStatusRequest request)
        {
            var command = new SupplierUpdateStatusCommand { Params = @params, Request = request };
            var response = await _updateStatusCommandHandler.UpdateStatus(command);
            if (response == null) return NotFound(new { message = $"Proveedor con ID {@params.SupplierId} no encontrado." });
            return Ok(response);
        }
    }
}

