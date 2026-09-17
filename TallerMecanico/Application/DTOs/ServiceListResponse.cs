using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Application.DTOs
{
    public class ServiceListResponse
    {
        public long ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required TimeOnly EstimatedDuration { get; set; }
        public Status ServiceStatus { get; set; }
    }
}
