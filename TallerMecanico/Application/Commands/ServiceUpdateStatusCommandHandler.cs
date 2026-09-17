using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ServiceUpdateStatusCommandHandler
    {
        private readonly IServiceRepository _repository;
        private readonly IValidator<ServiceUpdateStatusCommand> _validator;

        public ServiceUpdateStatusCommandHandler(
            IServiceRepository repository,
            IValidator<ServiceUpdateStatusCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ServiceGetResponse?> UpdateStatus(ServiceUpdateStatusCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var updated = await _repository.UpdateStatus(command.Params.ServiceId, command.Request.ServiceStatus);
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
