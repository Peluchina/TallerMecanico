using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ProductScheduleUpdateCommand
    {
        public required ProductScheduleUpdateParams Params { get; set; }
        public required ProductScheduleUpdateRequest Request { get; set; }
    }
}
