using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Application.DTOs
{
    public class ServiceScheduleGetResponse
    {
        public long ServiceScheduleId { get; set; }
        public required long CustomerId { get; set; }
        public required long EmployeeId { get; set; }
        public required long ServiceId { get; set; }
        public required DateTime ScheduledDate { get; set; }
        public  StatusSchedule ServiceScheduleStatus { get; set; }
    }
}
