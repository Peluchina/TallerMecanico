using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class EmployeeGetQueryHandler
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeGetQueryHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmployeeGetResponse?> Get(EmployeeGetQuery query)
        {
            var item = await _repository.GetById(query.Params.EmployeeId);
            if (item == null) return null;

            return new EmployeeGetResponse
            {
                EmployeeId = item.EmployeeId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Dni = item.Dni,
                Address = item.Address,
                Phone = item.Phone,
                Role = item.Role,
                EmailAddress = item.EmailAddress,
                Password = item.Password,
                EmployeeStatus = item.EmployeeStatus
            };
        }
    }
}
