using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ServiceScheduleUpdateCommand
    {
        public required ServiceScheduleUpdateParams Params { get; set; }
        public required ServiceScheduleUpdateRequest Request { get; set; }
    }
}
