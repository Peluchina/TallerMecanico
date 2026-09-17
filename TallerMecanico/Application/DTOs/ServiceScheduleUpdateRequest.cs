namespace TallerMecanico.Application.DTOs
{
    public class ServiceScheduleUpdateRequest
    {
        public required long CustomerId { get; set; }
        public required long EmployeeId { get; set; }
        public required long ServiceId { get; set; }
        public required DateTime ScheduledDate { get; set; }
    }
}
