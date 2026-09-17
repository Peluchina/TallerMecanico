using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Domain.Agregates
{
    public class ServiceSchedule
    {
        public long ServiceScheduleId { get; set; }
        public required long CustomerId { get; set; }
        public required long EmployeeId { get; set; }
        public required long ServiceId { get; set; }
        public required DateTime ScheduledDate { get; set; }
        public StatusSchedule ServiceScheduleStatus { get; set; }
    }
}
