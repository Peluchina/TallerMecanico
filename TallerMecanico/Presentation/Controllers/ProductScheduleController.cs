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
        [SwaggerOperation(
            Summary = "Obtener todos los agendamientos de productos",
            Description = "Retorna la lista de todos los agendamientos de productos registrados en el sistema.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de agendamientos de productos obtenida exitosamente.", typeof(IEnumerable<ProductScheduleListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para acceder a este recurso.")]
        [ProducesResponseType(typeof(IEnumerable<ProductScheduleListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ProductScheduleListResponse>>> GetAll()
        {
            var query = new ProductScheduleListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ProductScheduleId:long}")]
        [SwaggerOperation(
            Summary = "Obtener un agendamiento de producto por ID",
            Description = "Retorna los detalles de un agendamiento de producto específico según su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Agendamiento de producto encontrado exitosamente.", typeof(ProductScheduleGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para acceder a este recurso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamiento de producto no encontrado.")]
        [ProducesResponseType(typeof(ProductScheduleGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductScheduleGetResponse>> GetById([FromRoute] ProductScheduleGetParams @params)
        {
            var query = new ProductScheduleGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Agendamiento de producto con ID {@params.ProductScheduleId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Crear un nuevo agendamiento de producto",
            Description = "Registra un nuevo agendamiento de producto en el sistema. Retorna un error 409 si ya existe un conflicto, o 400 si los datos son inválidos.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Agendamiento de producto creado exitosamente.", typeof(ProductScheduleCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos o error de validación de negocio.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para realizar esta acción.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un agendamiento de producto con los datos proporcionados.")]
        [ProducesResponseType(typeof(ProductScheduleCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductScheduleCreateResponse>> Create([FromBody] ProductScheduleCreateRequest request)
        {
            var command = new ProductScheduleCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ProductScheduleId = response.ProductScheduleId }, response);
        }

        [HttpPut("{ProductScheduleId:long}")]
        [SwaggerOperation(
            Summary = "Actualizar un agendamiento de producto",
            Description = "Actualiza los datos de un agendamiento de producto existente. Retorna 404 si no existe, 409 si hay conflicto, o 400 si los datos son inválidos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Agendamiento de producto actualizado exitosamente.", typeof(ProductScheduleGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos o error de validación de negocio.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para realizar esta acción.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamiento de producto no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto al actualizar el agendamiento de producto.")]
        [ProducesResponseType(typeof(ProductScheduleGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [SwaggerOperation(
            Summary = "Actualizar el estado de un agendamiento de producto",
            Description = "Cambia el estado de un agendamiento de producto existente. Retorna 404 si no existe o 400 si el estado es inválido.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del agendamiento de producto actualizado exitosamente.", typeof(ProductScheduleGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Estado inválido o error de validación.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "No autenticado. Se requiere un token válido.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "No autorizado. El usuario no tiene permisos para realizar esta acción.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Agendamiento de producto no encontrado.")]
        [ProducesResponseType(typeof(ProductScheduleGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
