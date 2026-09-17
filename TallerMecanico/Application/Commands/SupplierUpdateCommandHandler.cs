using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class SupplierUpdateCommandHandler
    {
        private readonly ISupplierRepository _repository;
        private readonly IValidator<SupplierUpdateCommand> _validator;

        public SupplierUpdateCommandHandler(
            ISupplierRepository repository,
            IValidator<SupplierUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SupplierGetResponse?> Update(SupplierUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Supplier
            {
                SupplierId = command.Params.SupplierId,
                Ruc = command.Request.Ruc,
                LegalName = command.Request.LegalName,
                Address = command.Request.Address,
                Phone = command.Request.Phone
            };

            var updated = await _repository.Update(entity);
            if (updated == null) return null;

            return new SupplierGetResponse
            {
                SupplierId = updated.SupplierId,
                Ruc = updated.Ruc,
                LegalName = updated.LegalName,
                Address = updated.Address,
                Phone = updated.Phone,
                SupplierStatus = updated.SupplierStatus
            };
        }
    }
}
