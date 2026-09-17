using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ProductGetQueryHandler
    {
        private readonly IProductRespository _repository;

        public ProductGetQueryHandler(IProductRespository repository)
        {
            _repository = repository;
        }

        public async Task<ProductGetResponse?> Get(ProductGetQuery query)
        {
            var item = await _repository.GetById(query.Params.ProductId);
            if (item == null) return null;

            return new ProductGetResponse
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                SupplierId = item.SupplierId,
                Brand = item.Brand,
                Price = item.Price,
                Stock = item.Stock,
                ProductStatus = item.ProductStatus
            };
        }
    }
}
