using App.Core.Entities;
using Enums = App.Core.Enums;

namespace App.Infrastructure.Repository
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = [];

        public Task AddAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Order>> GetAllAsync() => Task.FromResult<IEnumerable<Order>>(_orders);
        public Task<Order?> GetByIdAsync(Guid id) => Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));

        public Task UpdateStatusAsync(Guid id, Enums.OrderStatus status)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null) order.Status = status;
            return Task.CompletedTask;
        }
    }
}
