using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Domain.Agregates
{
    public class ProductSchedule
    {
        public long ProductScheduleId { get; set; }
        public required long ProductId { get; set; }
        public required long ServiceScheduleId { get; set; }
        public required long Quantity { get; set; }
        public required DateTime ProductUseDate { get; set; }
        public Status ProductScheduleStatus { get; set; }
    }
}
