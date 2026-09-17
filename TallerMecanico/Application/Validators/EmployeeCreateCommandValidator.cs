using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
    {
        public EmployeeCreateCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Los datos del empleado son obligatorios.");
            RuleFor(x => x.Request.FirstName)
                .NotEmpty()
                .WithMessage("El nombre del empleado es obligatorio.");
            RuleFor(x => x.Request.LastName)
                .NotEmpty().
                WithMessage("El apellido del empleado es obligatorio.");
            RuleFor(x => x.Request.Dni)
                .NotEmpty()
                .WithMessage("El DNI del empleado es obligatorio.")
                .Matches(@"^\d+$")
                .WithMessage("El DNI solo debe contener números.");
            RuleFor(x => x.Request.Address)
                .NotEmpty()
                .WithMessage("La dirección del empleado es obligatoria.");
            RuleFor(x => x.Request.Phone)
                .NotEmpty()
                .WithMessage("El teléfono del empleado es obligatorio.")
                .Matches(@"^\d+$")
                .WithMessage("El teléfono solo debe contener números.");
            RuleFor(x => x.Request.Role)
                .NotEmpty()
                .WithMessage("El rol del empleado es obligatorio.");
            RuleFor(x => x.Request.EmailAddress)
                .NotEmpty()
                .WithMessage("El correo electrónico del empleado es obligatorio.");
            RuleFor(x => x.Request.Password)
                .NotEmpty()
                .WithMessage("La contraseña del empleado es obligatoria.");
        }
    }
}
