using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class SupplierUpdateStatusCommandHandler
    {
        private readonly ISupplierRepository _repository;
        private readonly IValidator<SupplierUpdateStatusCommand> _validator;

        public SupplierUpdateStatusCommandHandler(
            ISupplierRepository repository,
            IValidator<SupplierUpdateStatusCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SupplierGetResponse?> UpdateStatus(SupplierUpdateStatusCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            string supplierStatus = command.Request.SupplierStatus switch
            {
                "0" => "ACTIVO",
                "1" => "INACTIVO",
                _ => throw new ValidationException(
                    "El estado del proveedor no es válido."
                )
            };

            var updated = await _repository.UpdateStatus(command.Params.SupplierId, supplierStatus);
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

