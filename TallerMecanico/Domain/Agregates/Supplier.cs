using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Domain.Agregates
{
    public class Supplier
    {
        public long SupplierId { get; set; }
        public required string Ruc { get; set; }
        public required string LegalName { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public Status SupplierStatus { get; set; }
    }
}

