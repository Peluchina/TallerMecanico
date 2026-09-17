using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ServiceScheduleCreateCommandValidator:AbstractValidator<ServiceScheduleCreateCommand>
    {
        public ServiceScheduleCreateCommandValidator() {
            RuleFor(x => x.Request)
               .NotNull().WithMessage("Los datos son obligatorios.");
            RuleFor(x => x.Request.CustomerId)
                .NotEmpty().WithMessage("El cliente es obligatorio.");
            RuleFor(x => x.Request.EmployeeId)
                .NotEmpty().WithMessage("El empleado es obligatorio.");
            RuleFor(x => x.Request.ServiceId)
                .NotEmpty().WithMessage("El servicio es obligatorio.");
            RuleFor(x => x.Request.ScheduledDate)
                .NotNull().WithMessage("La fecha es obligatoria.");
        }
    }
}
