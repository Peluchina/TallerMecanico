using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ServiceUpdateCommandHandler
    {
        private readonly IServiceRepository _repository;
        private readonly IValidator<ServiceUpdateCommand> _validator;

        public ServiceUpdateCommandHandler(
            IServiceRepository repository,
            IValidator<ServiceUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ServiceGetResponse?> Update(ServiceUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Service
            {
                ServiceId = command.Params.ServiceId,
                ServiceName = command.Request.ServiceName,
                Description = command.Request.Description,
                Price = command.Request.Price,
                EstimatedDuration = command.Request.EstimatedDuration
            };

            var updated = await _repository.Update(entity);
            if (updated == null) return null;

            return new ServiceGetResponse
            {
                ServiceId = updated.ServiceId,
                ServiceName = updated.ServiceName,
                Description = updated.Description,
                Price = updated.Price,
                EstimatedDuration = updated.EstimatedDuration,
                ServiceStatus = updated.ServiceStatus
            };
        }
    }
}
