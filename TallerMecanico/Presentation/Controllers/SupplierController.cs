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
        [SwaggerOperation(Summary = "Listar proveedores", Description = "Obtiene la lista de proveedores registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de proveedores obtenida correctamente.", typeof(IEnumerable<SupplierListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
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
        [SwaggerOperation(Summary = "Obtener proveedor por ID", Description = "Obtiene los datos de un proveedor específico por su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa. Datos del proveedor.", typeof(SupplierGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
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
        [SwaggerOperation(Summary = "Crear proveedor", Description = "Registra un nuevo proveedor en el sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Proveedor creado correctamente.", typeof(SupplierCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un proveedor con los mismos datos.")]
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
        [SwaggerOperation(Summary = "Modificar proveedor", Description = "Actualiza los datos de un proveedor existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Proveedor actualizado correctamente.", typeof(SupplierGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Proveedor no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto, los datos del proveedor ya existen asociados a otro proveedor.")]
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
        [SwaggerOperation(Summary = "Actualizar estado del proveedor", Description = "Actualiza el estado (activo/inactivo) de un proveedor.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del proveedor actualizado correctamente.", typeof(SupplierGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
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

