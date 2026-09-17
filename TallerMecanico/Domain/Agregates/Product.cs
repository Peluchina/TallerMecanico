using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Domain.Agregates
{
    public class Product
    {
        public long ProductId { get; set; }
        public required string ProductName { get; set; }
        public required long SupplierId { get; set; }
        public required string Brand { get; set; }
        public required decimal Price { get; set; }
        public required long Stock { get; set; }
        public Status ProductStatus { get; set; }
    }
}
