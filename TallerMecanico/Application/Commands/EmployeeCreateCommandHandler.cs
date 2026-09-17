using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class EmployeeCreateCommandHandler
    {
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<EmployeeCreateCommand> _validator;

        public EmployeeCreateCommandHandler(
            IEmployeeRepository repository,
            IValidator<EmployeeCreateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeCreateResponse> Create(EmployeeCreateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Employee
            {
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                Dni = command.Request.Dni,
                Address = command.Request.Address,
                Phone = command.Request.Phone,
                Role = command.Request.Role,
                EmailAddress = command.Request.EmailAddress,
                Password = command.Request.Password
            };

            var id = await _repository.Insert(entity);

            return new EmployeeCreateResponse
            {
                EmployeeId = id
            };
        }
    }
}
