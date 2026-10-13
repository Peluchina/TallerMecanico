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
    public class ProductController : ControllerBase
    {
        private readonly ProductCreateCommandHandler _createCommandHandler;
        private readonly ProductUpdateCommandHandler _updateCommandHandler;
        private readonly ProductUpdateStatusCommandHandler _updateStatusCommandHandler;
        private readonly ProductGetQueryHandler _getQueryHandler;
        private readonly ProductListQueryHandler _listQueryHandler;

        public ProductController(
            ProductCreateCommandHandler createCommandHandler,
            ProductUpdateCommandHandler updateCommandHandler,
            ProductUpdateStatusCommandHandler updateStatusCommandHandler,
            ProductGetQueryHandler getQueryHandler,
            ProductListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _updateStatusCommandHandler = updateStatusCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar productos", Description = "Obtiene la lista de productos registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de productos obtenida correctamente.", typeof(IEnumerable<ProductListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [ProducesResponseType(typeof(IEnumerable<ProductListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ProductListResponse>>> GetAll()
        {
            var query = new ProductListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ProductId:long}")]
        [SwaggerOperation(Summary = "Obtener producto por ID", Description = "Obtiene los datos de un producto específico por su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa. Datos del producto.", typeof(ProductGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado.")]
        [ProducesResponseType(typeof(ProductGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductGetResponse>> GetById([FromRoute] ProductGetParams @params)
        {
            var query = new ProductGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Producto con ID {@params.ProductId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Crear producto", Description = "Registra un nuevo producto en el sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Producto creado correctamente.", typeof(ProductCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un producto con los mismos datos.")]
        [ProducesResponseType(typeof(ProductCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductCreateResponse>> Create([FromBody] ProductCreateRequest request)
        {
            var command = new ProductCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ProductId = response.ProductId }, response);
        }

        [HttpPut("{ProductId:long}")]
        [SwaggerOperation(Summary = "Modificar producto", Description = "Actualiza los datos de un producto existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto actualizado correctamente.", typeof(ProductGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto, los datos del producto ya existen asociados a otro producto.")]
        [ProducesResponseType(typeof(ProductGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductGetResponse>> Update(
            [FromRoute] ProductUpdateParams @params,
            [FromBody] ProductUpdateRequest request)
        {
            var command = new ProductUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Producto con ID {@params.ProductId} no encontrado." });
            return Ok(response);
        }

        [HttpPatch("{ProductId:long}/status")]
        [SwaggerOperation(Summary = "Actualizar estado del producto", Description = "Actualiza el estado (activo/inactivo) de un producto.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del producto actualizado correctamente.", typeof(ProductGetResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado.")]
        [ProducesResponseType(typeof(ProductGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductGetResponse>> UpdateStatus(
            [FromRoute] ProductUpdateStatusParams @params,
            [FromBody] ProductUpdateStatusRequest request)
        {
            var command = new ProductUpdateStatusCommand { Params = @params, Request = request };
            var response = await _updateStatusCommandHandler.UpdateStatus(command);
            if (response == null) return NotFound(new { message = $"Producto con ID {@params.ProductId} no encontrado." });
            return Ok(response);
        }
    }
}

