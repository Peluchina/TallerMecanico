using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ProductUpdateStatusCommandHandler
    {
        private readonly IProductRespository _repository;
        private readonly IValidator<ProductUpdateStatusCommand> _validator;

        public ProductUpdateStatusCommandHandler(
            IProductRespository repository,
            IValidator<ProductUpdateStatusCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ProductGetResponse?> UpdateStatus(ProductUpdateStatusCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            string productStatus = command.Request.ProductStatus switch
            {
                "0" => "ACTIVO",
                "1" => "INACTIVO",
                _ => throw new ValidationException(
                    "El estado del producto no es válido."
                )
            };

            var updated = await _repository.UpdateStatus(
                command.Params.ProductId,
                productStatus
            );

            if (updated == null)
                return null;

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

