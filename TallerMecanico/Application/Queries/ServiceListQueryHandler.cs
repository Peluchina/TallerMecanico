using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ServiceListQueryHandler
    {
        private readonly IServiceRepository _repository;

        public ServiceListQueryHandler(IServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServiceListResponse>> GetAll(ServiceListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new ServiceListResponse
            {
                ServiceId = item.ServiceId,
                ServiceName = item.ServiceName,
                Description = item.Description,
                Price = item.Price,
                EstimatedDuration = item.EstimatedDuration,
                ServiceStatus = item.ServiceStatus
            });
        }
    }
}
