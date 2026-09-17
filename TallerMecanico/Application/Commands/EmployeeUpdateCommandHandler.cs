using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class EmployeeUpdateCommandHandler
    {
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<EmployeeUpdateCommand> _validator;

        public EmployeeUpdateCommandHandler(
            IEmployeeRepository repository,
            IValidator<EmployeeUpdateCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeGetResponse?> Update(EmployeeUpdateCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var entity = new Employee
            {
                EmployeeId = command.Params.EmployeeId,
                FirstName = command.Request.FirstName,
                LastName = command.Request.LastName,
                Dni = command.Request.Dni,
                Address = command.Request.Address,
                Phone = command.Request.Phone,
                Role = command.Request.Role,
                EmailAddress = command.Request.EmailAddress,
                Password = command.Request.Password
            };

            var updated = await _repository.Update(entity);
            if (updated == null) return null;

            return new EmployeeGetResponse
            {
                EmployeeId = updated.EmployeeId,
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                Dni = updated.Dni,
                Address = updated.Address,
                Phone = updated.Phone,
                Role = updated.Role,
                EmailAddress = updated.EmailAddress,
                Password = updated.Password
            };
        }
    }
}
