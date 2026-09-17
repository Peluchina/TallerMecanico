using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ServiceCreateCommandHandler
    {
        private readonly IServiceRepository _repository;
        private readonly IValidator<ServiceCreateCommand> _validator;

        public ServiceCreateCommandHandler(
            IServiceRepository repository,
            IValidator<ServiceCreateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ServiceCreateResponse> Create(ServiceCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Service
            {
                ServiceName = command.Request.ServiceName,
                Description = command.Request.Description,
                Price = command.Request.Price,
                EstimatedDuration = command.Request.EstimatedDuration
            };

            var id = await _repository.Insert(entity);

            return new ServiceCreateResponse
            {
                ServiceId = id
            };
        }
    }
}
