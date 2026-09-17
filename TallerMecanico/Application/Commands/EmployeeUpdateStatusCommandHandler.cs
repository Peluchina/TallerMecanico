using FluentValidation;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Commands
{
    public class EmployeeUpdateStatusCommandHandler
    {
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<EmployeeUpdateStatusCommand> _validator;

        public EmployeeUpdateStatusCommandHandler(
            IEmployeeRepository repository,
            IValidator<EmployeeUpdateStatusCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<EmployeeGetResponse?> UpdateStatus(EmployeeUpdateStatusCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var updated = await _repository.UpdateStatus(command.Params.EmployeeId, command.Request.EmployeeStatus);
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
                Password = updated.Password,
                EmployeeStatus = updated.EmployeeStatus
            };
        }
    }
}
