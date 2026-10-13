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
        [SwaggerOperation(Summary = "Listar servicios", Description = "Obtiene la lista de servicios registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de servicios obtenida correctamente.", typeof(IEnumerable<ServiceListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [ProducesResponseType(typeof(IEnumerable<ServiceListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ServiceListResponse>>> GetAll()
        {
            var query = new ServiceListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ServiceId:long}")]
        [SwaggerOperation(Summary = "Obtener servicio por ID", Description = "Obtiene los datos de un servicio específico por su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa. Datos del servicio.", typeof(ServiceGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Servicio no encontrado.")]
        [ProducesResponseType(typeof(ServiceGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceGetResponse>> GetById([FromRoute] ServiceGetParams @params)
        {
            var query = new ServiceGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Servicio con ID {@params.ServiceId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Crear servicio", Description = "Registra un nuevo servicio en el sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Servicio creado correctamente.", typeof(ServiceCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un servicio con los mismos datos.")]
        [ProducesResponseType(typeof(ServiceCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ServiceCreateResponse>> Create([FromBody] ServiceCreateRequest request)
        {
            var command = new ServiceCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ServiceId = response.ServiceId }, response);
        }

        [HttpPut("{ServiceId:long}")]
        [SwaggerOperation(Summary = "Modificar servicio", Description = "Actualiza los datos de un servicio existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Servicio actualizado correctamente.", typeof(ServiceGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Servicio no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto, los datos del servicio ya existen asociados a otro servicio.")]
        [ProducesResponseType(typeof(ServiceGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [SwaggerOperation(Summary = "Actualizar estado del servicio", Description = "Actualiza el estado (activo/inactivo) de un servicio.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del servicio actualizado correctamente.", typeof(ServiceGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Servicio no encontrado.")]
        [ProducesResponseType(typeof(ServiceGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
