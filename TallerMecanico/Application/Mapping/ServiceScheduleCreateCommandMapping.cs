namespace TallerMecanico.Application.Mapping
{
    public class ServiceScheduleCreateCommandMapping
    {
        public long ServiceScheduleId { get; set; }
        public required long CustomerId { get; set; }
        public required long EmployeeId { get; set; }
        public required long ServiceId { get; set; }
        public required DateTime ScheduledDate { get; set; }
        public required string ServiceScheduleStatus { get; set; }

    }
}
