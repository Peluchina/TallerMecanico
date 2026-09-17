using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Domain.Agregates
{
    public class Employee
    {
        public long EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Dni { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string Role { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public Status EmployeeStatus { get; set; }
    }
}
