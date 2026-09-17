using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAll();
        Task<Supplier?> GetById(long id);
        Task<long> Insert(Supplier supplier);
        Task<Supplier?> Update(Supplier supplier);
        Task<Supplier?> UpdateStatus(long id, string status);
    }
}

