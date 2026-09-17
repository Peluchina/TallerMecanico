using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface IServiceScheduleRepository
    {
        Task<IEnumerable<ServiceSchedule>> GetAll();
        Task<ServiceSchedule?> GetById(long id);
        Task<long> Insert(ServiceSchedule serviceSchedule);
        Task<ServiceSchedule?> Update(ServiceSchedule serviceSchedule);
        Task<ServiceSchedule?> UpdateStatus(long id, string status);
    }
}

