using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class ProductUpdateCommand
    {
        public required ProductUpdateParams Params { get; set; }
        public required ProductUpdateRequest Request { get; set; }
    }
}
