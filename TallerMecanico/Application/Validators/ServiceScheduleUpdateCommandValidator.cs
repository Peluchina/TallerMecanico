using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ServiceScheduleUpdateCommandValidator:AbstractValidator<ServiceScheduleUpdateCommand>
    {
        public ServiceScheduleUpdateCommandValidator() {
            RuleFor(x => x.Params)
              .NotNull().WithMessage("Los parametros de ruta son obligatorios.");
            RuleFor(x => x.Params.ServiceScheduleId)
                .GreaterThan(0).WithMessage("El identificador debe ser mayor a 0.");
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
