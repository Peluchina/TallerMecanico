using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;
using TallerMecanico.Application.Queries;

namespace TallerMecanico.Presentation.Controllers
{
    [Authorize(Roles = "ADMIN,RECEPCIONISTA")]
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
        [SwaggerOperation(
            Summary = "Obtener todos los proveedores",
            Description = "Retorna la lista de todos los proveedores registrados en el sistema.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de proveedores obtenida exitosamente.", typeof(IEnumerable<SupplierListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para acceder a este recurso.")]
        [ProducesResponseType(typeof(IEnumerable<SupplierListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<SupplierListResponse>>> GetAll()
        {
            var query = new SupplierListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{SupplierId:long}")]
        [SwaggerOperation(
            Summary = "Obtener un proveedor por ID",
            Description = "Retorna los detalles de un proveedor específico según su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Proveedor encontrado exitosamente.", typeof(SupplierGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para acceder a este recurso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Proveedor no encontrado.")]
        [ProducesResponseType(typeof(SupplierGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierGetResponse>> GetById([FromRoute] SupplierGetParams @params)
        {
            var query = new SupplierGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Proveedor con ID {@params.SupplierId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Crear un nuevo proveedor",
            Description = "Registra un nuevo proveedor en el sistema. Retorna 409 si ya existe un proveedor con los mismos datos, o 400 si los datos son inválidos.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Proveedor creado exitosamente.", typeof(SupplierCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos o error de validación de negocio.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para realizar esta acción.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un proveedor con los datos proporcionados.")]
        [ProducesResponseType(typeof(SupplierCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SupplierCreateResponse>> Create([FromBody] SupplierCreateRequest request)
        {
            var command = new SupplierCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { SupplierId = response.SupplierId }, response);
        }

        [HttpPut("{SupplierId:long}")]
        [SwaggerOperation(
            Summary = "Actualizar un proveedor",
            Description = "Actualiza los datos de un proveedor existente. Retorna 404 si no existe, 409 si hay conflicto, o 400 si los datos son inválidos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Proveedor actualizado exitosamente.", typeof(SupplierGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos o error de validación de negocio.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para realizar esta acción.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Proveedor no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto al actualizar el proveedor.")]
        [ProducesResponseType(typeof(SupplierGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [SwaggerOperation(
            Summary = "Actualizar el estado de un proveedor",
            Description = "Cambia el estado de un proveedor existente. Retorna 404 si no existe o 400 si el estado es inválido.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del proveedor actualizado exitosamente.", typeof(SupplierGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Estado inválido o error de validación.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para realizar esta acción.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Proveedor no encontrado.")]
        [ProducesResponseType(typeof(SupplierGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
