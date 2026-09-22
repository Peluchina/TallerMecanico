namespace TallerMecanico.Domain.Agregates
{
    public abstract class Persona
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Dni { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
    }
}
