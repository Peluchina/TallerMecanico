using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ProductUpdateStatusCommand
    {
        public required ProductUpdateStatusParams Params { get; set; }
        public required ProductUpdateStatusRequest Request { get; set; }
    }
}
