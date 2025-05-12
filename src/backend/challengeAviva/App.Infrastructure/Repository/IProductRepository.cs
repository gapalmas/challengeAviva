using App.Core.Entities;

namespace App.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
    }
}
