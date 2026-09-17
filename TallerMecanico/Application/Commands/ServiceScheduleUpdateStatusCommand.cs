using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ServiceScheduleUpdateStatusCommand
    {
        public required ServiceScheduleUpdateStatusParams Params { get; set; }
        public required ServiceScheduleUpdateStatusRequest Request { get; set; }
    }
}
