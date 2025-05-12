using App.Core.Dto.Request;
using App.Core.Entities;

namespace App.Core.Interfaces
{
    public interface IOrderManager
    {
        Task<Order> CreateOrderAsync(OrderRequestDto dto);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task CancelAsync(Guid id);
        Task PayAsync(Guid id);
    }
}
