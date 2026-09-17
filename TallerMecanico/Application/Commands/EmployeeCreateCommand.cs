using TallerMecanico.Application.DTOs;

namespace TallerMecanico.Application.Commands
{
    public class EmployeeCreateCommand
    {
        public required EmployeeCreateRequest Request { get; set; }
    }
}
