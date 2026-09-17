using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class EmployeeListQueryHandler
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeListQueryHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmployeeListResponse>> GetAll(EmployeeListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new EmployeeListResponse
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
            });
        }
    }
}
