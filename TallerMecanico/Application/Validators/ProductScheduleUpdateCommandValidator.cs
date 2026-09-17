using FluentValidation;
using TallerMecanico.Application.Commands;

namespace TallerMecanico.Application.Validators
{
    public class ProductScheduleUpdateCommandValidator:AbstractValidator<ProductScheduleUpdateCommand>
    {
        public ProductScheduleUpdateCommandValidator() {
            RuleFor(x => x.Params)
              .NotNull().WithMessage("Los parametros de ruta son obligatorios.");
            RuleFor(x => x.Params.ProductScheduleId)
                .GreaterThan(0).WithMessage("El identificador debe ser mayor a 0.");
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
