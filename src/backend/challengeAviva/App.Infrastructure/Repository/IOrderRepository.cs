using App.Core.Entities;
using App.Core.Enums;

namespace App.Infrastructure.Repository
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task UpdateStatusAsync(Guid id, OrderStatus status);
    }
}
