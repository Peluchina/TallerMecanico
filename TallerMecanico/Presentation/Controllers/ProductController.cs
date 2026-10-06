using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<ProductListResponse>>> GetAll()
        {
            var query = new ProductListQuery();
            var response = await _listQueryHandler.GetAll(query);
            return Ok(response);
        }

        [HttpGet("{ProductId:long}")]
        public async Task<ActionResult<ProductGetResponse>> GetById([FromRoute] ProductGetParams @params)
        {
            var query = new ProductGetQuery { Params = @params };
            var response = await _getQueryHandler.Get(query);
            if (response == null) return NotFound(new { message = $"Producto con ID {@params.ProductId} no encontrado." });
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductCreateResponse>> Create([FromBody] ProductCreateRequest request)
        {
            var command = new ProductCreateCommand { Request = request };
            var response = await _createCommandHandler.Create(command);
            return CreatedAtAction(nameof(GetById), new { ProductId = response.ProductId }, response);
        }

        [HttpPut("{ProductId:long}")]
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

