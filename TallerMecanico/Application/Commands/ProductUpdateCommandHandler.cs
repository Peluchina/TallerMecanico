using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ProductUpdateCommandHandler
    {
        private readonly IProductRespository _repository;
        private readonly IValidator<ProductUpdateCommand> _validator;

        public ProductUpdateCommandHandler(
            IProductRespository repository,
            IValidator<ProductUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ProductGetResponse?> Update(ProductUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Product
            {
                ProductId = command.Params.ProductId,
                ProductName = command.Request.ProductName,
                SupplierId = command.Request.SupplierId,
                Brand = command.Request.Brand,
                Price = command.Request.Price,
                Stock = command.Request.Stock
            };

            var updated = await _repository.Update(entity);
            if (updated == null) return null;

            return new ProductGetResponse
            {
                ProductId = updated.ProductId,
                ProductName = updated.ProductName,
                SupplierId = updated.SupplierId,
                Brand = updated.Brand,
                Price = updated.Price,
                Stock = updated.Stock,
                ProductStatus = updated.ProductStatus
            };
        }
    }
}
