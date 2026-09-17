using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class SupplierUpdateCommand
    {
        public required SupplierUpdateParams Params { get; set; }
        public required SupplierUpdateRequest Request { get; set; }
    }
}
