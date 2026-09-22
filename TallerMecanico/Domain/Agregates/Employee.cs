using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Domain.Agregates
{
    public class Employee:Persona
    {
        public long EmployeeId { get; set; }
        public required string Role { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public Status EmployeeStatus { get; set; }
    }
}
