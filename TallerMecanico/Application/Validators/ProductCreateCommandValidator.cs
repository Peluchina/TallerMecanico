using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ProductCreateCommandValidator: AbstractValidator<ProductCreateCommand>
    {
        public ProductCreateCommandValidator() {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Los datos del producto son obligatorios.");
            RuleFor(x => x.Request.ProductName)
                .NotEmpty()
                .WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(500)
                .WithMessage("El nombre del producto no puede superar los 150 caracteres.");
            RuleFor(x => x.Request.SupplierId)
                .NotEmpty()
                .WithMessage("El proveedor es obligatorio.");
            RuleFor(x => x.Request.Brand)
                .NotEmpty()
                .WithMessage("La marca del producto es obligatorio.")
                .MaximumLength(500)
                .WithMessage("La marca del producto no puede superar los 150 caracteres.");
            RuleFor(x => x.Request.Price)
                .GreaterThan(0)
                .WithMessage("El precio debe ser mayor a 0.");
            RuleFor(x => x.Request.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("La cantidad no puede ser negativa.");
        }
    }
}
