namespace TallerMecanico.Application.Mapping
{
    public class ProductCreateCommandMapping
    {
        public long ProductId { get; set; }
        public required string ProductName { get; set; }
        public required long SupplierId { get; set; }
        public required string Brand { get; set; }
        public required decimal Price { get; set; }
        public required long Stock { get; set; }
        public required string ProductStatus { get; set; }
    }
}
