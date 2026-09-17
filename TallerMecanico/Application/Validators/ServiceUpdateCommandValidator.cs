using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ServiceUpdateCommandValidator : AbstractValidator<ServiceUpdateCommand>
    {
        public ServiceUpdateCommandValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Los parámetros de ruta son obligatorios.");
            RuleFor(x => x.Params.ServiceId)
                .GreaterThan(0).WithMessage("El ID del servicio debe ser mayor a 0.");
            RuleFor(x => x.Request)
                .NotNull().WithMessage("Los datos del servicio son obligatorios.");
            RuleFor(x => x.Request.ServiceName)
                .NotEmpty().WithMessage("El nombre del servicio es obligatorio.");
            RuleFor(x => x.Request.Description)
                .NotEmpty().WithMessage("La descripción del servicio es obligatoria.");
            RuleFor(x => x.Request.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
            RuleFor(x => x.Request.EstimatedDuration)
                .NotNull().WithMessage("El tiempo estimado del servicio es obligatorio.");
        }
    }
}
