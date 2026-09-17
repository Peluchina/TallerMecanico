using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ServiceGetQueryHandler
    {
        private readonly IServiceRepository _repository;

        public ServiceGetQueryHandler(IServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceGetResponse?> Get(ServiceGetQuery query)
        {
            var item = await _repository.GetById(query.Params.ServiceId);
            if (item == null) return null;

            return new ServiceGetResponse
            {
                ServiceId = item.ServiceId,
                ServiceName = item.ServiceName,
                Description = item.Description,
                Price = item.Price,
                EstimatedDuration = item.EstimatedDuration,
                ServiceStatus = item.ServiceStatus
            };
        }
    }
}
