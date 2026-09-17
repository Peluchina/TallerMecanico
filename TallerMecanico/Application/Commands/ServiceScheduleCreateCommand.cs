using TallerMecanico.Application.DTOs;

namespace TallerMecanico.Application.Commands
{
    public class ServiceScheduleCreateCommand
    {
        public required ServiceScheduleCreateRequest Request { get; set; }
    }
}
