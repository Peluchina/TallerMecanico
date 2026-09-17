namespace TallerMecanico.Application.DTOs
{
    public class ProductUpdateRequest
    {
        public required string ProductName { get; set; }
        public required long SupplierId { get; set; }
        public required string Brand { get; set; }
        public required decimal Price { get; set; }
        public required long Stock { get; set; }
    }
}
