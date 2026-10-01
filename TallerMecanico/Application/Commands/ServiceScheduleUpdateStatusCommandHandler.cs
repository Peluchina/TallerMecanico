using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class ServiceScheduleUpdateStatusCommandHandler
    {
        private readonly IServiceScheduleRepository _repository;
        private readonly IValidator<ServiceScheduleUpdateStatusCommand> _validator;

        public ServiceScheduleUpdateStatusCommandHandler(
            IServiceScheduleRepository repository,
            IValidator<ServiceScheduleUpdateStatusCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<ServiceScheduleGetResponse?> UpdateStatus(ServiceScheduleUpdateStatusCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            string serviceScheduleStatus = command.Request.ServiceScheduleStatus switch
            {
                "0" => "PENDIENTE",
                "1" => "INACTIVO",
                "2" => "REALIZADO",
                _ => throw new ValidationException(
                    "El estado de la programación no es válido."
                )
            };

            var updated = await _repository.UpdateStatus(command.Params.ServiceScheduleId, serviceScheduleStatus);
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

