using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee?> GetById(long id);
        Task<Employee?> GetByEmail(string email);
        Task<long> Insert(Employee employee);
        Task<Employee?> Update(Employee employee);
        Task<Employee?> UpdateStatus(long id, string status);
    }
}
