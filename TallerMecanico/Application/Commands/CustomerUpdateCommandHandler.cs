using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class CustomerUpdateCommandHandler
    {
        private readonly ICustomerRepository _repository;
        private readonly IValidator<CustomerUpdateCommand> _validator;

        public CustomerUpdateCommandHandler(
            ICustomerRepository repository,
            IValidator<CustomerUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<CustomerGetResponse?> Update(CustomerUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Customer
            {
                CustomerId = command.Params.CustomerId,
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                Dni = command.Request.Dni,
                Address = command.Request.Address,
                Phone = command.Request.Phone
            };

            var updated = await _repository.Update(entity);
            if (updated == null) return null;

            return new CustomerGetResponse
            {
                CustomerId = updated.CustomerId,
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                Dni = updated.Dni,
                Address = updated.Address,
                Phone = updated.Phone
            };
        }
    }
}
