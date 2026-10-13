using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;
using TallerMecanico.Application.Queries;

namespace TallerMecanico.Presentation.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "ADMIN,RECEPCIONISTA")]
        [SwaggerOperation(Summary = "Listar empleados", Description = "Obtiene la lista de empleados registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de empleados obtenidas correctamente.", typeof(IEnumerable<EmployeeListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<EmployeeListResponse>>> GetAll()
        {
            var query = new EmployeeListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{EmployeeId:long}")]
        [Authorize(Roles = "ADMIN,RECEPCIONISTA")]
        [SwaggerOperation(Summary = "Obtener empleado por ID", Description = "Obtiene los datos de un empleado específico por su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa. Datos del empleado.", typeof(EmployeeGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empleado no encontrado.")]
        [ProducesResponseType(typeof(EmployeeGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeGetResponse>> GetById([FromRoute] EmployeeGetParams @params)
        {
            var query = new EmployeeGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Empleado con ID {@params.EmployeeId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Crear empleado", Description = "Registra un nuevo empleado en el sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Empleado creado correctamente.", typeof(EmployeeCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un empleado con los mismos datos.")]
        [ProducesResponseType(typeof(EmployeeCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeCreateResponse>> Create([FromBody] EmployeeCreateRequest request)
        {
            var command = new EmployeeCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { EmployeeId = response.EmployeeId }, response);
        }

        [HttpPut("{EmployeeId:long}")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Modificar empleado", Description = "Actualiza los datos de un empleado existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Empleado actualizado correctamente.", typeof(EmployeeUpdateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empleado no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto, los datos del empleado ya existen asociados a otro empleado.")]
        [ProducesResponseType(typeof(EmployeeUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EmployeeUpdateResponse>> Update(
            [FromRoute] EmployeeUpdateParams @params,
            [FromBody] EmployeeUpdateRequest request)
        {
            var command = new EmployeeUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Empleado con ID {@params.EmployeeId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{EmployeeId:long}/status")]
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Summary = "Actualizar estado del empleado", Description = "Actualiza el estado (activo/inactivo) de un empleado.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del empleado actualizado correctamente.", typeof(EmployeeGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Empleado no encontrado.")]
        [ProducesResponseType(typeof(EmployeeGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
