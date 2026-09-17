using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class CustomerListQueryHandler
    {
        private readonly ICustomerRepository _repository;

        public CustomerListQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CustomerListResponse>> GetAll(CustomerListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new CustomerListResponse
            {
                CustomerId = item.CustomerId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Dni = item.Dni,
                Address = item.Address,
                Phone = item.Phone
            });
        }
    }
}
