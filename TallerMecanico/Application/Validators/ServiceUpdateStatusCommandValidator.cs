using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ServiceUpdateStatusCommandValidator : AbstractValidator<ServiceUpdateStatusCommand>
    {
        public ServiceUpdateStatusCommandValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Los parámetros de ruta son obligatorios.");
            RuleFor(x => x.Params.ServiceId)
                .GreaterThan(0).WithMessage("El ID del servicio debe ser mayor a 0.");
            RuleFor(x => x.Request)
                .NotNull().WithMessage("El cuerpo de la solicitud no puede estar vacío.");
        }
    }
}
