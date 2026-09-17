using FluentValidation;
using TallerMecanico.Application.Commands;
namespace TallerMecanico.Application.Validators
{
    public class CustomerCreateCommandValidator:AbstractValidator<CustomerCreateCommand>
    {
        public CustomerCreateCommandValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Los datos del cliente son obligatorios.");

            RuleFor(x => x.Request.FirstName)
                .NotEmpty()
                .WithMessage("El nombre del cliente es obligatorio.");

            RuleFor(x => x.Request.LastName)
                .NotEmpty()
                .WithMessage("El apellido del cliente es obligatorio.");

            RuleFor(x => x.Request.Dni)
                .NotEmpty()
                .WithMessage("El DNI del cliente es obligatorio.")
                .Matches(@"^\d+$")
                .WithMessage("El DNI solo debe contener números.");

            RuleFor(x => x.Request.Address)
                .NotEmpty()
                .WithMessage("La dirección del cliente es obligatoria.");

            RuleFor(x => x.Request.Phone)
                .NotEmpty()
                .WithMessage("El teléfono del cliente es obligatorio.")
                .Matches(@"^\d+$")
                .WithMessage("El teléfono solo debe contener números.");
        }
    }
}
