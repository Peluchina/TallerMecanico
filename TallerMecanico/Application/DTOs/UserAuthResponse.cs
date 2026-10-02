namespace TallerMecanico.Application.DTOs
{
    public class UserAuthResponse
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public string? Picture { get; set; }
        public required string Token { get; set; }
    }
}
