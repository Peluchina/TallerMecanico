using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAll();
        Task<Service?> GetById(long id);
        Task<long> Insert(Service service);
        Task<Service?> Update(Service service);
        Task<Service?> UpdateStatus(long id, string status);
    }
}
