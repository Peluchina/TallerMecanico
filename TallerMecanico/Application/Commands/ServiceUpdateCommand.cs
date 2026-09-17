using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ServiceUpdateCommand
    {
        public required ServiceUpdateParams Params { get; set; }
        public required ServiceUpdateRequest Request { get; set; }
    }
}
