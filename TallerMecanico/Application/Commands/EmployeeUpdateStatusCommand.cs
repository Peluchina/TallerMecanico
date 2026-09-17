using TallerMecanico.Application.DTOs;
using TallerMecanico.Application.Parameters;

namespace TallerMecanico.Application.Commands
{
    public class EmployeeUpdateStatusCommand
    {
        public required EmployeeUpdateStatusParams Params { get; set; }
        public required EmployeeUpdateStatusRequest Request {  get; set; }  
    }
}
