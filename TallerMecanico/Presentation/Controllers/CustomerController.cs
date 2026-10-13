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
    public class CustomerController : ControllerBase
    {
        private readonly CustomerCreateCommandHandler _createCommandHandler;
        private readonly CustomerUpdateCommandHandler _updateCommandHandler;
        private readonly CustomerGetQueryHandler _getQueryHandler;
        private readonly CustomerListQueryHandler _listQueryHandler;

        public CustomerController(
            CustomerCreateCommandHandler createCommandHandler,
            CustomerUpdateCommandHandler updateCommandHandler,
            CustomerGetQueryHandler getQueryHandler,
            CustomerListQueryHandler listQueryHandler)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _getQueryHandler = getQueryHandler;
            _listQueryHandler = listQueryHandler;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar clientes", Description = "Obtiene la lista de clientes registrados.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de clientes obtenidas correctamente.", typeof(IEnumerable<CustomerListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [ProducesResponseType(typeof(IEnumerable<CustomerListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<CustomerListResponse>>> GetAll()
        {
            var query = new CustomerListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{CustomerId:long}")]
        [SwaggerOperation(Summary = "Obtener cliente por ID", Description = "Obtiene los datos de un cliente específico por su identificador.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Operación exitosa. Datos del cliente.", typeof(CustomerGetResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Cliente no encontrado.")]
        [ProducesResponseType(typeof(CustomerGetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerGetResponse>> GetById([FromRoute] CustomerGetParams @params)
        {
            var query = new CustomerGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Cliente con ID {@params.CustomerId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Crear cliente", Description = "Registra un nuevo cliente en el sistema.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Cliente creado correctamente.", typeof(CustomerCreateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un cliente con los mismos datos.")]
        [ProducesResponseType(typeof(CustomerCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CustomerCreateResponse>> Create([FromBody] CustomerCreateRequest request)
        {
            var command = new CustomerCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { CustomerId = response.CustomerId }, response);
        }

        [HttpPut("{CustomerId:long}")]
        [SwaggerOperation(Summary = "Modificar cliente", Description = "Actualiza los datos de un cliente existente.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Cliente actualizado correctamente.", typeof(CustomerUpdateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Usuario no autenticado.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Usuario sin permisos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Cliente no encontrado.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto, los datos del cliente ya existen asociados a otro cliente.")]
        [ProducesResponseType(typeof(CustomerUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CustomerUpdateResponse>> Update(
            [FromRoute] CustomerUpdateParams @params,
            [FromBody] CustomerUpdateRequest request)
        {
            var command = new CustomerUpdateCommand { Params = @params, Request = request };
            var response = await _updateCommandHandler.Update(command);
            if (response == null) return NotFound(new { message = $"Cliente con ID {@params.CustomerId} no encontrado." });
            return Ok(response);
        }
    }
}
