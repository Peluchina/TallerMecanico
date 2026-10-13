using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;
using TallerMecanico.Application.Queries;

namespace TallerMecanico.Presentation.Controllers
{
    
    [Authorize(Roles = "ADMIN,RECEPCIONISTA,MECANICO")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductScheduleController : ControllerBase
    {
        private readonly ProductScheduleCreateCommandHandler _createCommandHandler;
        private readonly ProductScheduleUpdateCommandHandler _updateCommandHandler;
        private readonly ProductScheduleUpdateStatusCommandHandler _updateStatusCommandHandler;
        private readonly ProductScheduleGetQueryHandler _getQueryHandler;
        private readonly ProductScheduleListQueryHandler _listQueryHandler;

        public ProductScheduleController(
            ProductScheduleCreateCommandHandler createCommandHandler,
            ProductScheduleUpdateCommandHandler updateCommandHandler,
            ProductScheduleUpdateStatusCommandHandler updateStatusCommandHandler,
            ProductScheduleGetQueryHandler getQueryHandler,
            ProductScheduleListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _updateStatusCommandHandler = updateStatusCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductScheduleListResponse>>> GetAll()
        {
            var query = new ProductScheduleListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ProductScheduleId:long}")]
        public async Task<ActionResult<ProductScheduleGetResponse>> GetById([FromRoute] ProductScheduleGetParams @params)
        {
            var query = new ProductScheduleGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Agendamiento de producto con ID {@params.ProductScheduleId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductScheduleCreateResponse>> Create([FromBody] ProductScheduleCreateRequest request)
        {
            var command = new ProductScheduleCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ProductScheduleId = response.ProductScheduleId }, response);
        }

        [HttpPut("{ProductScheduleId:long}")]
        public async Task<ActionResult<ProductScheduleGetResponse>> Update(
            [FromRoute] ProductScheduleUpdateParams @params,
            [FromBody] ProductScheduleUpdateRequest request)
        {
            var command = new ProductScheduleUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Agendamiento de producto con ID {@params.ProductScheduleId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{ProductScheduleId:long}/status")]
        public async Task<ActionResult<ProductScheduleGetResponse>> UpdateStatus(
            [FromRoute] ProductScheduleUpdateStatusParams @params,
            [FromBody] ProductScheduleUpdateStatusRequest request)
        {
            var command = new ProductScheduleUpdateStatusCommand { Params = @params, Request = request };
            var response = await _updateStatusCommandHandler.UpdateStatus(command);
            if (response == null) return NotFound(new { message = $"Agendamiento de producto con ID {@params.ProductScheduleId} no encontrado." });
            return Ok(response);
        }
    }
}

