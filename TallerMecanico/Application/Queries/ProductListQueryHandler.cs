using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ProductListQueryHandler
    {
        private readonly IProductRespository _repository;

        public ProductListQueryHandler(IProductRespository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductListResponse>> GetAll(ProductListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new ProductListResponse
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                SupplierId = item.SupplierId,
                Brand = item.Brand,
                Price = item.Price,
                Stock = item.Stock,
                ProductStatus = item.ProductStatus
            });
        }
    }
}
