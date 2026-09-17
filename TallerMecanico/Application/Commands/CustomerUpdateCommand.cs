using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class CustomerUpdateCommand
    {
        public required CustomerUpdateParams Params { get; set; }
        public required CustomerUpdateRequest Request { get; set; }
    }
}
