using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ServiceScheduleCreateCommandHandler
    {
        private readonly IServiceScheduleRepository _repository;
        private readonly IValidator<ServiceScheduleCreateCommand> _validator;

        public ServiceScheduleCreateCommandHandler(
            IServiceScheduleRepository repository,
            IValidator<ServiceScheduleCreateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ServiceScheduleCreateResponse> Create(ServiceScheduleCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new ServiceSchedule
            {
                CustomerId = command.Request.CustomerId,
                EmployeeId = command.Request.EmployeeId,
                ServiceId = command.Request.ServiceId,
                ScheduledDate = command.Request.ScheduledDate
            };

            var id = await _repository.Insert(entity);

            return new ServiceScheduleCreateResponse
            {
                ServiceScheduleId = id
            };
        }
    }
}
