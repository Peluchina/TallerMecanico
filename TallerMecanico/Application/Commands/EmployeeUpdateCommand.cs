using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class EmployeeUpdateCommand
    {
        public required EmployeeUpdateParams Params { get; set; }
        public required EmployeeUpdateRequest Request { get; set; }
    }
}
