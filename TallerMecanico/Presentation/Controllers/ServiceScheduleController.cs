using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(Summary = "Listar agendamientos", Description = "Obtiene la lista de agendamientos de servicio registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de agendamientos obtenida correctamente.", typeof(IEnumerable<ServiceScheduleListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [ProducesResponseType(typeof(IEnumerable<ServiceScheduleListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ServiceScheduleListResponse>>> GetAll()
        {
            var query = new ServiceScheduleListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ServiceScheduleId:long}")]
        [SwaggerOperation(Summary = "Obtener agendamiento por ID", Description = "Obtiene los datos de un agendamiento de servicio específico por su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa. Datos del agendamiento.", typeof(ServiceScheduleGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamiento no encontrado.")]
        [ProducesResponseType(typeof(ServiceScheduleGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceScheduleGetResponse>> GetById([FromRoute] ServiceScheduleGetParams @params)
        {
            var query = new ServiceScheduleGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Agendamiento con ID {@params.ServiceScheduleId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Crear agendamiento", Description = "Registra un nuevo agendamiento de servicio en el sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Agendamiento creado correctamente.", typeof(ServiceScheduleCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un agendamiento con los mismos datos.")]
        [ProducesResponseType(typeof(ServiceScheduleCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ServiceScheduleCreateResponse>> Create([FromBody] ServiceScheduleCreateRequest request)
        {
            var command = new ServiceScheduleCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ServiceScheduleId = response.ServiceScheduleId }, response);
        }

        [HttpPut("{ServiceScheduleId:long}")]
        [SwaggerOperation(Summary = "Modificar agendamiento", Description = "Actualiza los datos de un agendamiento de servicio existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Agendamiento actualizado correctamente.", typeof(ServiceScheduleGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamiento no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto, los datos del agendamiento ya existen asociados a otro agendamiento.")]
        [ProducesResponseType(typeof(ServiceScheduleGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [SwaggerOperation(Summary = "Actualizar estado del agendamiento", Description = "Actualiza el estado (pendiente, inactivo, realizado) de un agendamiento de servicio.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del agendamiento actualizado correctamente.", typeof(ServiceScheduleGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamiento no encontrado.")]
        [ProducesResponseType(typeof(ServiceScheduleGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
