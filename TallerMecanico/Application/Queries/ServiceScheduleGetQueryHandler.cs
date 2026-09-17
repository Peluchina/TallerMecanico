using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ServiceScheduleGetQueryHandler
    {
        private readonly IServiceScheduleRepository _repository;

        public ServiceScheduleGetQueryHandler(IServiceScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceScheduleGetResponse?> Get(ServiceScheduleGetQuery query)
        {
            var item = await _repository.GetById(query.Params.ServiceScheduleId);
            if (item == null) return null;

            return new ServiceScheduleGetResponse
            {
                ServiceScheduleId = item.ServiceScheduleId,
                CustomerId = item.CustomerId,
                EmployeeId = item.EmployeeId,
                ServiceId = item.ServiceId,
                ScheduledDate = item.ScheduledDate,
                ServiceScheduleStatus = item.ServiceScheduleStatus
            };
        }
    }
}
