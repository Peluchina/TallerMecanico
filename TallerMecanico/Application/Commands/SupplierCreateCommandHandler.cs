using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class SupplierCreateCommandHandler
    {
        private readonly ISupplierRepository _repository;
        private readonly IValidator<SupplierCreateCommand> _validator;

        public SupplierCreateCommandHandler(
            ISupplierRepository repository,
            IValidator<SupplierCreateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SupplierCreateResponse> Create(SupplierCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Supplier
            {
                Ruc = command.Request.Ruc,
                LegalName = command.Request.LegalName,
                Address = command.Request.Address,
                Phone = command.Request.Phone
            };

            var id = await _repository.Insert(entity);

            return new SupplierCreateResponse
            {
                SupplierId = id
            };
        }
    }
}
