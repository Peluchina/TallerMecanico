using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;
using TallerMecanico.Application.Queries;

namespace TallerMecanico.Presentation.Controllers
{
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
        public async Task<ActionResult<IEnumerable<CustomerListResponse>>> GetAll()
        {
            var query = new CustomerListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{CustomerId:long}")]
        public async Task<ActionResult<CustomerGetResponse>> GetById([FromRoute] CustomerGetParams @params)
        {
            var query = new CustomerGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Cliente con ID {@params.CustomerId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerCreateResponse>> Create([FromBody] CustomerCreateRequest request)
        {
            var command = new CustomerCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { CustomerId = response.CustomerId }, response);
        }

        [HttpPut("{CustomerId:long}")]
        public async Task<ActionResult<CustomerGetResponse>> Update(
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
