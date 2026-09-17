using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ProductScheduleUpdateStatusCommandHandler
    {
        private readonly IProductScheduleRepository _repository;
        private readonly IValidator<ProductScheduleUpdateStatusCommand> _validator;

        public ProductScheduleUpdateStatusCommandHandler(
            IProductScheduleRepository repository,
            IValidator<ProductScheduleUpdateStatusCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ProductScheduleGetResponse?> UpdateStatus(ProductScheduleUpdateStatusCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var updated = await _repository.UpdateStatus(command.Params.ProductScheduleId, command.Request.ProductScheduleStatus);
            if (updated == null) return null;

            return new ProductScheduleGetResponse
            {
                ProductScheduleId = updated.ProductScheduleId,
                ProductId = updated.ProductId,
                ServiceScheduleId = updated.ServiceScheduleId,
                Quantity = updated.Quantity,
                ProductUseDate = updated.ProductUseDate,
                ProductScheduleStatus = updated.ProductScheduleStatus
            };
        }
    }
}

