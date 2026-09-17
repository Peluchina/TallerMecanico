using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ProductScheduleCreateCommandValidator:AbstractValidator<ProductScheduleCreateCommand>
    {
        public ProductScheduleCreateCommandValidator() {
            RuleFor(x => x.Request)
               .NotNull().WithMessage("Los datos son obligatorios.");
            RuleFor(x => x.Request.ProductId)
                .NotEmpty().WithMessage("El producto es obligatorio.");
            RuleFor(x => x.Request.ServiceScheduleId)
                .NotEmpty().WithMessage("El campo es obligatorio.");
            RuleFor(x => x.Request.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad no puede ser negativa.");
            RuleFor(x => x.Request.ProductUseDate)
                .NotNull().WithMessage("La fecha es obligatoria.");

        }
    }
}
