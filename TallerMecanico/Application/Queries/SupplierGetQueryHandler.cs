using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;

namespace TallerMecanico.Application.Queries
{
    public class SupplierGetQueryHandler
    {
        private readonly ISupplierRepository _repository;

        public SupplierGetQueryHandler(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<SupplierGetResponse?> Get(SupplierGetQuery query)
        {
            var item = await _repository.GetById(query.Params.SupplierId);
            if (item == null) return null;

            return new SupplierGetResponse
            {
                SupplierId = item.SupplierId,
                Ruc = item.Ruc,
                LegalName = item.LegalName,
                Address = item.Address,
                Phone = item.Phone,
                SupplierStatus = item.SupplierStatus
            };
        }
    }
}
