
using App.Core.Entities;
using App.Core.Enums;
using App.Core.Interfaces;
using App.Infrastructure.Repository;
using Moq;
using App.Application.Services;
using Req = App.Core.Dto.Request;

namespace App.UnitTests
{
    public class OrderManagerTests
    {
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly Mock<IPaymentProvider> _provider = new();
        private readonly OrderManager _manager;

        public OrderManagerTests()
        {
            _provider.Setup(p => p.Name).Returns("TestProvider");
            _provider.Setup(p => p.Supports(It.IsAny<PaymentMode>())).Returns(true);
            _provider.Setup(p => p.CalculateFee(It.IsAny<decimal>(), It.IsAny<PaymentMode>())).Returns(10);
            _provider.Setup(p => p.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

            _manager = new OrderManager(
                _productRepo.Object,
                _orderRepo.Object,
                [_provider.Object]
            );
        }

        [Fact(DisplayName = "Crear orden correctamente con proveedor válido")]
        public async Task CreateOrder_ReturnsValidOrder()
        {
            var product = new Product { Id = Guid.NewGuid(), Name = "Test", UnitPrice = 100 };
            _productRepo.Setup(p => p.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync([product]);
            _orderRepo.Setup(o => o.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            var dto = new Req.OrderRequestDto
            {
                ProductIds = [product.Id],
                PaymentMode = PaymentMode.Cash
            };

            var order = await _manager.CreateOrderAsync(dto);

            Assert.NotNull(order);
            Assert.Equal(1, order.Products.Count);
            Assert.Equal("TestProvider", order.ProviderKey);
            Assert.Equal(10, order.Fee);
            Assert.Equal(OrderStatus.Created, order.Status);
        }

        [Fact(DisplayName = "Falla si ningún proveedor soporta el método de pago")]
        public async Task CreateOrder_ThrowsIfNoProvider()
        {
            _provider.Setup(p => p.Supports(It.IsAny<PaymentMode>())).Returns(false);

            var dto = new Req.OrderRequestDto
            {
                ProductIds = [Guid.NewGuid()],
                PaymentMode = PaymentMode.Transfer
            };

            await Assert.ThrowsAsync<Exception>(() => _manager.CreateOrderAsync(dto));
        }

        [Fact(DisplayName = "Falla si proveedor retorna falso al crear orden")]
        public async Task CreateOrder_FailsIfProviderFails()
        {
            _provider.Setup(p => p.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(false);
            _productRepo.Setup(p => p.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>())).ReturnsAsync(
            [
                new Product { Id = Guid.NewGuid(), Name = "X", UnitPrice = 50 }
            ]);

            var dto = new Req.OrderRequestDto
            {
                ProductIds = [Guid.NewGuid()],
                PaymentMode = PaymentMode.Cash
            };

            await Assert.ThrowsAsync<Exception>(() => _manager.CreateOrderAsync(dto));
        }
    }

}
