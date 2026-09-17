namespace TallerMecanico.Application.Mapping
{
    public class SupplierCreateCommandMapping
    {
        public required long SupplierId { get; set; }
        public required string Ruc { get; set; }
        public required string LegalName { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string SupplierStatus { get; set; }
    }
}
