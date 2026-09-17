namespace TallerMecanico.Application.DTOs
{
    public class ServiceCreateRequest
    {
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required TimeOnly EstimatedDuration { get; set; }
    }
}
