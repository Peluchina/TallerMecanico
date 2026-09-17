using TallerMecanico.Application.DTOs;

namespace TallerMecanico.Application.Commands
{
    public class ProductScheduleCreateCommand
    {
        public required ProductScheduleCreateRequest Request { get; set; }
    }
}
