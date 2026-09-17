using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface IProductScheduleRepository
    {
        Task<IEnumerable<ProductSchedule>> GetAll();
        Task<ProductSchedule?> GetById(long id);
        Task<long> Insert(ProductSchedule productSchedule);
        Task<ProductSchedule?> Update(ProductSchedule productSchedule);
        Task<ProductSchedule?> UpdateStatus(long id, string status);
    }
}

