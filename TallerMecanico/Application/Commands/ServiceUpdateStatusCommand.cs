using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ServiceUpdateStatusCommand
    {
        public required ServiceUpdateStatusParams Params { get; set; }
        public required ServiceUpdateStatusRequest Request { get; set; }
    }
}
