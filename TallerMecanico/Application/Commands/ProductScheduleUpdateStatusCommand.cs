using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ProductScheduleUpdateStatusCommand
    {
        public required ProductScheduleUpdateStatusParams Params { get; set; }
        public required ProductScheduleUpdateStatusRequest Request { get; set; }
    }
}
