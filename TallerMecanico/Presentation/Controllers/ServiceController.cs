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
    public class ServiceController : ControllerBase
    {
        private readonly ServiceCreateCommandHandler _createCommandHandler;
        private readonly ServiceUpdateCommandHandler _updateCommandHandler;
        private readonly ServiceUpdateStatusCommandHandler _updateStatusCommandHandler;
        private readonly ServiceGetQueryHandler _getQueryHandler;
        private readonly ServiceListQueryHandler _listQueryHandler;

        public ServiceController(
            ServiceCreateCommandHandler createCommandHandler,
            ServiceUpdateCommandHandler updateCommandHandler,
            ServiceUpdateStatusCommandHandler updateStatusCommandHandler,
            ServiceGetQueryHandler getQueryHandler,
            ServiceListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _updateStatusCommandHandler = updateStatusCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceListResponse>>> GetAll()
        {
            var query = new ServiceListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ServiceId:long}")]
        public async Task<ActionResult<ServiceGetResponse>> GetById([FromRoute] ServiceGetParams @params)
        {
            var query = new ServiceGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Servicio con ID {@params.ServiceId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceCreateResponse>> Create([FromBody] ServiceCreateRequest request)
        {
            var command = new ServiceCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ServiceId = response.ServiceId }, response);
        }

        [HttpPut("{ServiceId:long}")]
        public async Task<ActionResult<ServiceGetResponse>> Update(
            [FromRoute] ServiceUpdateParams @params,
            [FromBody] ServiceUpdateRequest request)
        {
            var command = new ServiceUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Servicio con ID {@params.ServiceId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{ServiceId:long}/status")]
        public async Task<ActionResult<ServiceGetResponse>> UpdateStatus(
            [FromRoute] ServiceUpdateStatusParams @params,
            [FromBody] ServiceUpdateStatusRequest request)
        {
            var command = new ServiceUpdateStatusCommand { Params = @params, Request = request };
            var response = await _updateStatusCommandHandler.UpdateStatus(command);
            if (response == null) return NotFound(new { message = $"Servicio con ID {@params.ServiceId} no encontrado." });
            return Ok(response);
        }
    }
}
