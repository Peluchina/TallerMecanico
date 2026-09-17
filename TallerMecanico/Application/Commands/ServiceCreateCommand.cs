using TallerMecanico.Application.DTOs;

namespace TallerMecanico.Application.Commands
{
    public class ServiceCreateCommand
    {
        public required ServiceCreateRequest Request { get; set; }
    }
}
