using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class EmployeeUpdateStatusCommandValidator:AbstractValidator<EmployeeUpdateStatusCommand>
    {
        public EmployeeUpdateStatusCommandValidator() {
            RuleFor(x => x.Params)
               .NotNull().WithMessage("Los parámetros de ruta son obligatorios.");
            RuleFor(x => x.Params.EmployeeId)
                .GreaterThan(0).WithMessage("El ID del empleado debe ser mayor a 0.");
            RuleFor(x => x.Request)
                .NotNull().WithMessage("El cuerpo de la solicitud no puede estar vacío.");
        }
    }
}
