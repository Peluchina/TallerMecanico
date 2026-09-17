using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class SupplierUpdateStatusCommand
    {
        public required SupplierUpdateStatusParams Params { get; set; }
        public required SupplierUpdateStatusRequest Request { get; set; }
    }
}
