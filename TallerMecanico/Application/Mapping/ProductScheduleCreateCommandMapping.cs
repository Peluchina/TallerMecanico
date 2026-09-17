namespace TallerMecanico.Application.Mapping
{
    public class ProductScheduleCreateCommandMapping
    {
        public long ProductScheduleId { get; set; }
        public required long ProductId { get; set; }
        public required long ServiceScheduleId { get; set; }
        public required long Quantity { get; set; }
        public required DateTime ProductUseDate { get; set; }
        public required string ProductScheduleStatus { get; set; }
    }
}
