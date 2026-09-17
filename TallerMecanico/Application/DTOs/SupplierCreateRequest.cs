namespace TallerMecanico.Application.DTOs
{
    public class SupplierCreateRequest
    {
        public required string Ruc { get; set; }
        public required string LegalName { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }

    }
}
