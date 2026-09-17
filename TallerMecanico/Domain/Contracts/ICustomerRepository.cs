using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer?> GetById(long id);
        Task<long> Insert(Customer customer);
        Task<Customer?> Update(Customer customer);
    }
}
