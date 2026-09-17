using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class CustomerGetQueryHandler
    {
        private readonly ICustomerRepository _repository;

        public CustomerGetQueryHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerGetResponse?> Get(CustomerGetQuery query)
        {
            var item = await _repository.GetById(query.Params.CustomerId);
            if (item == null) return null;

            return new CustomerGetResponse
            {
                CustomerId = item.CustomerId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Dni = item.Dni,
                Address = item.Address,
                Phone = item.Phone
            };
        }
    }
}
