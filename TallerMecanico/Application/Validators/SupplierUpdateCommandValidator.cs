using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class SupplierUpdateCommandValidator:AbstractValidator<SupplierUpdateCommand>
    {
        public SupplierUpdateCommandValidator() {
            RuleFor(x => x.Request)
                .NotNull().WithMessage("Los datos del proveedor son obligatorios.");
            RuleFor(x => x.Request.Ruc)
                .NotNull()
                .WithMessage("El RUC del proveedor es obligatorio.")
                .Matches(@"^\d+$")
                .WithMessage("El RUC solo debe contener números.");
            RuleFor(x => x.Request.LegalName)
                .NotNull().WithMessage("La Razon Social del proveedor es obligatorio.");
            RuleFor(x => x.Request.Address)
                .NotNull().WithMessage("La direccion del proveedor es obligatorio.");
            RuleFor(x => x.Request.Phone)
                .NotNull()
                .WithMessage("El telefono del proveedor es obligatorio.")
                .Matches(@"^\d+$")
                .WithMessage("El telefono solo debe contener números.");
        }
    }
}
