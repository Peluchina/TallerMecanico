using TallerMecanico.Application.DTOs;

namespace TallerMecanico.Application.Commands
{
    public class CustomerCreateCommand
    {
        public required CustomerCreateRequest Request { get; set; }
    }
}
