using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class SupplierListQueryHandler
    {
        private readonly ISupplierRepository _repository;

        public SupplierListQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SupplierListResponse>> GetAll(SupplierListQuery query)
        {
            var list = await _repository.GetAll();
            return list.Select(item => new SupplierListResponse
            {
                SupplierId = item.SupplierId,
                Ruc = item.Ruc,
                LegalName = item.LegalName,
                Address = item.Address,
                Phone = item.Phone,
                SupplierStatus = item.SupplierStatus
            });
        }
    }
}
