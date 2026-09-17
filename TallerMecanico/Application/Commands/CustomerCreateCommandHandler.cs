using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Mapping;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class CustomerCreateCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IValidator<CustomerCreateCommand> _validator;

        public CustomerCreateCommandHandler(
            ICustomerRepository customerRepository,
            IValidator<CustomerCreateCommand> validator)
        {
            _customerRepository = customerRepository;
            _validator = validator;
        }

        public async Task<CustomerCreateResponse> Create(CustomerCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var customer = new Customer
            {
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                Dni = command.Request.Dni,
                Address = command.Request.Address,
                Phone = command.Request.Phone
            };

            //var customer=command.ToEntity();

            var id = await _customerRepository.Insert(customer);

            return new CustomerCreateResponse
            {
                CustomerId = id
            };
        }
    }
}

