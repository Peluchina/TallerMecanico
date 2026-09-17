using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class ProductScheduleGetQueryHandler
    {
        private readonly IProductScheduleRepository _repository;

        public ProductScheduleGetQueryHandler(IProductScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductScheduleGetResponse?> Get(ProductScheduleGetQuery query)
        {
            var item = await _repository.GetById(query.Params.ProductScheduleId);
            if (item == null) return null;

            return new ProductScheduleGetResponse
            {
                ProductScheduleId = item.ProductScheduleId,
                ProductId = item.ProductId,
                ServiceScheduleId = item.ServiceScheduleId,
                Quantity = item.Quantity,
                ProductUseDate = item.ProductUseDate,
                ProductScheduleStatus = item.ProductScheduleStatus
            };
        }
    }
}
