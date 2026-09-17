using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ProductCreateCommandHandler
    {
        private readonly IProductRespository _repository;
        private readonly IValidator<ProductCreateCommand> _validator;

        public ProductCreateCommandHandler(
            IProductRespository repository,
            IValidator<ProductCreateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ProductCreateResponse> Create(ProductCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Product
            {
                ProductName = command.Request.ProductName,
                SupplierId = command.Request.SupplierId,
                Brand = command.Request.Brand,
                Price = command.Request.Price,
                Stock = command.Request.Stock
            };

            var id = await _repository.Insert(entity);

            return new ProductCreateResponse
            {
                ProductId = id
            };
        }
    }
}
