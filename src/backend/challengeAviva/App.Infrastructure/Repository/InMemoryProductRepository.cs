using App.Core.Entities;

namespace App.Infrastructure.Repository
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static readonly List<Product> _products =
        [
            new() { Id = Guid.NewGuid(), Name = "Producto A", UnitPrice = 100 },
            new() { Id = Guid.NewGuid(), Name = "Producto B", UnitPrice = 150 }
        ];

        public static List<Product> Products => _products;

        //public Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids) => Task.FromResult(_products.Where(p => ids.Contains(p.Id)));

        public Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return Task.FromResult(ids.Any()
                ? Products.Where(p => ids.Contains(p.Id))
                : Products.AsEnumerable());
        }
    }
}
