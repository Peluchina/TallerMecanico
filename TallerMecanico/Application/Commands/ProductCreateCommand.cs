using TallerMecanico.Application.DTOs;

namespace TallerMecanico.Application.Commands
{
    public class ProductCreateCommand
    {
        public required ProductCreateRequest Request { get; set; }
    }
}
