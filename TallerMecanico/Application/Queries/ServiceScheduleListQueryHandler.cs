using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ServiceScheduleListQueryHandler
    {
        private readonly IServiceScheduleRepository _repository;

        public ServiceScheduleListQueryHandler(IServiceScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServiceScheduleListResponse>> GetAll(ServiceScheduleListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new ServiceScheduleListResponse
            {
                ServiceScheduleId = item.ServiceScheduleId,
                CustomerId = item.CustomerId,
                EmployeeId = item.EmployeeId,
                ServiceId = item.ServiceId,
                ScheduledDate = item.ScheduledDate,
                ServiceScheduleStatus = item.ServiceScheduleStatus
            });
        }
    }
}
