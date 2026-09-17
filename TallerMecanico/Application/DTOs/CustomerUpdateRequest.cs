namespace TallerMecanico.Application.DTOs
{
    public class CustomerUpdateRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Dni { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
    }
}
