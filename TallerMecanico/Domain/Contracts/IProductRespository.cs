using TallerMecanico.Domain.Agregates;

namespace TallerMecanico.Domain.Contracts
{
    public interface IProductRespository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(long id);
        Task<long> Insert(Product product);
        Task<Product?> Update(Product product);
        Task<Product?> UpdateStatus(long id, string status);
    }
}

