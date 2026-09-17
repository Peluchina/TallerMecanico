using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ServiceScheduleUpdateStatusCommandValidator:AbstractValidator<ServiceScheduleUpdateStatusCommand>
    {
        public ServiceScheduleUpdateStatusCommandValidator() {
            RuleFor(x => x.Params)
               .NotNull().WithMessage("Los parámetros de ruta son obligatorios.");
            RuleFor(x => x.Params.ServiceScheduleId)
                .GreaterThan(0).WithMessage("El ID debe ser mayor a 0.");
            RuleFor(x => x.Request)
                .NotNull().WithMessage("El cuerpo de la solicitud no puede estar vacío.");
        }
    }
}
