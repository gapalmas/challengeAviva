using App.Core.Dto.Request;
using App.Core.Entities;
using App.Core.Enums;
using System.Net.Http.Json;

namespace App.IntegrationTests
{
    public class OrderEndpointsTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrderEndpointsTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "Crear orden con productos y pago en efectivo")]
        public async Task CreateOrder_ShouldReturnOk()
        {
            var products = await _client.GetFromJsonAsync<List<Product>>("/api/products");
            Assert.NotNull(products);

            var dto = new OrderRequestDto
            {
                ProductIds = products!.Take(1).Select(p => p.Id).ToList(),
                PaymentMode = PaymentMode.Cash
            };

            var response = await _client.PostAsJsonAsync("/api/orders", dto);
            response.EnsureSuccessStatusCode();

            var order = await response.Content.ReadFromJsonAsync<Order>();
            Assert.NotNull(order);
            Assert.Equal(OrderStatus.Created, order!.Status);
        }

        [Fact(DisplayName = "Obtener listado de órdenes")]
        public async Task GetOrders_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/orders");
            response.EnsureSuccessStatusCode();

            var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
            Assert.NotNull(orders);
        }

        [Fact(DisplayName = "Obtener orden por ID")]
        public async Task GetOrderById_ShouldReturnOrder()
        {
            var all = await _client.GetFromJsonAsync<List<Order>>("/api/orders");
            var id = all!.First().Id;

            var response = await _client.GetAsync($"/api/orders/{id}");
            response.EnsureSuccessStatusCode();

            var order = await response.Content.ReadFromJsonAsync<Order>();
            Assert.NotNull(order);
            Assert.Equal(id, order!.Id);
        }

        [Fact(DisplayName = "Cancelar una orden")]
        public async Task CancelOrder_ShouldChangeStatus()
        {
            var order = await CrearOrdenAsync();

            var cancel = await _client.PostAsync($"/api/orders/{order.Id}/cancel", null);
            cancel.EnsureSuccessStatusCode();

            var check = await _client.GetFromJsonAsync<Order>($"/api/orders/{order.Id}");
            Assert.Equal(OrderStatus.Cancelled, check!.Status);
        }

        [Fact(DisplayName = "Pagar una orden")]
        public async Task PayOrder_ShouldChangeStatus()
        {
            var order = await CrearOrdenAsync();

            var pay = await _client.PostAsync($"/api/orders/{order.Id}/pay", null);
            pay.EnsureSuccessStatusCode();

            var check = await _client.GetFromJsonAsync<Order>($"/api/orders/{order.Id}");
            Assert.Equal(OrderStatus.Paid, check!.Status);
        }

        private async Task<Order> CrearOrdenAsync()
        {
            var productos = await _client.GetFromJsonAsync<List<Product>>("/api/products");
            var dto = new OrderRequestDto
            {
                ProductIds = productos!.Take(1).Select(p => p.Id).ToList(),
                PaymentMode = PaymentMode.Cash
            };

            var response = await _client.PostAsJsonAsync("/api/orders", dto);
            return (await response.Content.ReadFromJsonAsync<Order>())!;
        }
    }
}
