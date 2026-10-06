using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;
using TallerMecanico.Application.Queries;

namespace TallerMecanico.Presentation.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeCreateCommandHandler _createCommandHandler;
        private readonly EmployeeUpdateCommandHandler _updateCommandHandler;
        private readonly EmployeeUpdateStatusCommandHandler _updateStatusCommandHandler;
        private readonly EmployeeGetQueryHandler _getQueryHandler;
        private readonly EmployeeListQueryHandler _listQueryHandler;

        public EmployeeController(
            EmployeeCreateCommandHandler createCommandHandler,
            EmployeeUpdateCommandHandler updateCommandHandler,
            EmployeeUpdateStatusCommandHandler updateStatusCommandHandler,
            EmployeeGetQueryHandler getQueryHandler,
            EmployeeListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _updateStatusCommandHandler = updateStatusCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeListResponse>>> GetAll()
        {
            var query = new EmployeeListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{EmployeeId:long}")]
        public async Task<ActionResult<EmployeeGetResponse>> GetById([FromRoute] EmployeeGetParams @params)
        {
            var query = new EmployeeGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Empleado con ID {@params.EmployeeId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeCreateResponse>> Create([FromBody] EmployeeCreateRequest request)
        {
            var command = new EmployeeCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { EmployeeId = response.EmployeeId }, response);
        }

        [HttpPut("{EmployeeId:long}")]
        public async Task<ActionResult<EmployeeGetResponse>> Update(
            [FromRoute] EmployeeUpdateParams @params,
            [FromBody] EmployeeUpdateRequest request)
        {
            var command = new EmployeeUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Empleado con ID {@params.EmployeeId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{EmployeeId:long}/status")]
        public async Task<ActionResult<EmployeeGetResponse>> UpdateStatus(
            [FromRoute] EmployeeUpdateStatusParams @params,
            [FromBody] EmployeeUpdateStatusRequest request)
        {
            var command = new EmployeeUpdateStatusCommand { Params = @params, Request = request };
            var response = await _updateStatusCommandHandler.UpdateStatus(command);
            if (response == null) return NotFound(new { message = $"Empleado con ID {@params.EmployeeId} no encontrado." });
            return Ok(response);
        }
    }
}
