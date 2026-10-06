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
    public class ServiceScheduleController : ControllerBase
    {
        private readonly ServiceScheduleCreateCommandHandler _createCommandHandler;
        private readonly ServiceScheduleUpdateCommandHandler _updateCommandHandler;
        private readonly ServiceScheduleUpdateStatusCommandHandler _updateStatusCommandHandler;
        private readonly ServiceScheduleGetQueryHandler _getQueryHandler;
        private readonly ServiceScheduleListQueryHandler _listQueryHandler;

        public ServiceScheduleController(
            ServiceScheduleCreateCommandHandler createCommandHandler,
            ServiceScheduleUpdateCommandHandler updateCommandHandler,
            ServiceScheduleUpdateStatusCommandHandler updateStatusCommandHandler,
            ServiceScheduleGetQueryHandler getQueryHandler,
            ServiceScheduleListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _updateStatusCommandHandler = updateStatusCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceScheduleListResponse>>> GetAll()
        {
            var query = new ServiceScheduleListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ServiceScheduleId:long}")]
        public async Task<ActionResult<ServiceScheduleGetResponse>> GetById([FromRoute] ServiceScheduleGetParams @params)
        {
            var query = new ServiceScheduleGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Agendamiento con ID {@params.ServiceScheduleId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceScheduleCreateResponse>> Create([FromBody] ServiceScheduleCreateRequest request)
        {
            var command = new ServiceScheduleCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ServiceScheduleId = response.ServiceScheduleId }, response);
        }

        [HttpPut("{ServiceScheduleId:long}")]
        public async Task<ActionResult<ServiceScheduleGetResponse>> Update(
            [FromRoute] ServiceScheduleUpdateParams @params,
            [FromBody] ServiceScheduleUpdateRequest request)
        {
            var command = new ServiceScheduleUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Agendamiento con ID {@params.ServiceScheduleId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{ServiceScheduleId:long}/status")]
        public async Task<ActionResult<ServiceScheduleGetResponse>> UpdateStatus(
            [FromRoute] ServiceScheduleUpdateStatusParams @params,
            [FromBody] ServiceScheduleUpdateStatusRequest request)
        {
            var command = new ServiceScheduleUpdateStatusCommand { Params = @params, Request = request };
            var response = await _updateStatusCommandHandler.UpdateStatus(command);
            if (response == null) return NotFound(new { message = $"Agendamiento con ID {@params.ServiceScheduleId} no encontrado." });
            return Ok(response);
        }
    }
}

