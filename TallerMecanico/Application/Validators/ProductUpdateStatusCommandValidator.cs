using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ProductUpdateStatusCommandValidator : AbstractValidator<ProductUpdateStatusCommand>
    {
        public ProductUpdateStatusCommandValidator()
        {
            RuleFor(x => x.Params)
                .NotNull().WithMessage("Los parámetros de ruta son obligatorios.");
            RuleFor(x => x.Params.ProductId)
                .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a 0.");
            RuleFor(x => x.Request)
                .NotNull().WithMessage("El cuerpo de la solicitud no puede estar vacío.");
        }
    }
}
