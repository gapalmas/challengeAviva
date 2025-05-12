using App.Core.Entities;
using Enums = App.Core.Enums;
using App.Core.Interfaces;
using App.Infrastructure.Repository;
using Req = App.Core.Dto.Request;

namespace App.Application.Services
{
    public class OrderManager(IProductRepository products, IOrderRepository orders, IEnumerable<IPaymentProvider> providers) : IOrderManager
    {
        private readonly IProductRepository _products = products;
        private readonly IOrderRepository _orders = orders;
        private readonly IEnumerable<IPaymentProvider> _providers = providers;

        public async Task<Order> CreateOrderAsync(Req.OrderRequestDto dto)
        {
            var products = await _products.GetByIdsAsync(dto.ProductIds);
            var total = products.Sum(p => p.UnitPrice);

            var provider = _providers
                .Where(p => p.Supports(dto.PaymentMode))
                .Select(p => new { Provider = p, Fee = p.CalculateFee(total, dto.PaymentMode) })
                .OrderBy(p => p.Fee)
                .FirstOrDefault()?.Provider;

            if (provider != null)
            {
                var order = new Order { Products = products.ToList(), Fee = provider.CalculateFee(total, dto.PaymentMode), ProviderKey = provider.Name };

                if (!await provider.CreateOrderAsync(order))
                    throw new Exception($"Provider {provider.Name} failed to create order");

                await _orders.AddAsync(order);
                return order;
            }

            throw new Exception("No provider supports this payment mode.");
        }

        public Task<IEnumerable<Order>> GetAllAsync() => _orders.GetAllAsync();
        public Task<Order?> GetByIdAsync(Guid id) => _orders.GetByIdAsync(id);
        public Task CancelAsync(Guid id) => _orders.UpdateStatusAsync(id, Enums.OrderStatus.Cancelled);
        public Task PayAsync(Guid id) => _orders.UpdateStatusAsync(id, Enums.OrderStatus.Paid);
    }
}
