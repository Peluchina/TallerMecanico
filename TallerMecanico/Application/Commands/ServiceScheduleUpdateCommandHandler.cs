using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ServiceScheduleUpdateCommandHandler
    {
        private readonly IServiceScheduleRepository _repository;
        private readonly IValidator<ServiceScheduleUpdateCommand> _validator;

        public ServiceScheduleUpdateCommandHandler(
            IServiceScheduleRepository repository,
            IValidator<ServiceScheduleUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ServiceScheduleGetResponse?> Update(ServiceScheduleUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new ServiceSchedule
            {
                ServiceScheduleId = command.Params.ServiceScheduleId,
                CustomerId = command.Request.CustomerId,
                EmployeeId = command.Request.EmployeeId,
                ServiceId = command.Request.ServiceId,
                ScheduledDate = command.Request.ScheduledDate
            };

            var updated = await _repository.Update(entity);
            if (updated == null) return null;

            return new ServiceScheduleGetResponse
            {
                ServiceScheduleId = updated.ServiceScheduleId,
                CustomerId = updated.CustomerId,
                EmployeeId = updated.EmployeeId,
                ServiceId = updated.ServiceId,
                ScheduledDate = updated.ScheduledDate,
                ServiceScheduleStatus = updated.ServiceScheduleStatus
            };
        }
    }
}
