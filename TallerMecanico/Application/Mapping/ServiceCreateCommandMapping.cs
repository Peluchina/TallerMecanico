namespace TallerMecanico.Application.Mapping
{
    public class ServiceCreateCommandMapping
    {
        public long ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required TimeOnly EstimatedDuration { get; set; }
        public required string serviceStatus { get; set; }
    }
}
