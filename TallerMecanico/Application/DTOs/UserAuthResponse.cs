namespace TallerMecanico.Application.DTOs
{
    public class UserAuthResponse
    {
        public long EmployeeId { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public string? Picture { get; set; }
        public required string Token { get; set; }
    }
}
