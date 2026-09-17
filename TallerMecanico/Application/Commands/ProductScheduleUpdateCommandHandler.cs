using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ProductScheduleUpdateCommandHandler
    {
        private readonly IProductScheduleRepository _repository;
        private readonly IValidator<ProductScheduleUpdateCommand> _validator;

        public ProductScheduleUpdateCommandHandler(
            IProductScheduleRepository repository,
            IValidator<ProductScheduleUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ProductScheduleGetResponse?> Update(ProductScheduleUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new ProductSchedule
            {
                ProductScheduleId = command.Params.ProductScheduleId,
                ProductId = command.Request.ProductId,
                ServiceScheduleId = command.Request.ServiceScheduleId,
                Quantity = command.Request.Quantity,
                ProductUseDate = command.Request.ProductUseDate
            };

            var updated = await _repository.Update(entity);
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
