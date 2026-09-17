using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ProductScheduleCreateCommandHandler
    {
        private readonly IProductScheduleRepository _repository;
        private readonly IValidator<ProductScheduleCreateCommand> _validator;

        public ProductScheduleCreateCommandHandler(
            IProductScheduleRepository repository,
            IValidator<ProductScheduleCreateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ProductScheduleCreateResponse> Create(ProductScheduleCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new ProductSchedule
            {
                ProductId = command.Request.ProductId,
                ServiceScheduleId = command.Request.ServiceScheduleId,
                Quantity = command.Request.Quantity,
                ProductUseDate = command.Request.ProductUseDate
            };

            var id = await _repository.Insert(entity);

            return new ProductScheduleCreateResponse
            {
                ProductScheduleId = id
            };
        }
    }
}
