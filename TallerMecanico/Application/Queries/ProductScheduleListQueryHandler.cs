using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ProductScheduleListQueryHandler
    {
        private readonly IProductScheduleRepository _repository;

        public ProductScheduleListQueryHandler(IProductScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductScheduleListResponse>> GetAll(ProductScheduleListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new ProductScheduleListResponse
            {
                ProductScheduleId = item.ProductScheduleId,
                ProductId = item.ProductId,
                ServiceScheduleId = item.ServiceScheduleId,
                Quantity = item.Quantity,
                ProductUseDate = item.ProductUseDate,
                ProductScheduleStatus = item.ProductScheduleStatus
            });
        }
    }
}
