using App.Core.Entities;
using App.Core.Enums;
using App.Core.Interfaces;
using Flurl.Http;

namespace App.Application.Services
{
    public class CazaPagosProvider : IPaymentProvider
    {
        private const string ApiKey = "apikey-fj9esodija09s2";
        private const string BaseUrl = "https://app-cazapagos-chg-aviva.azurewebsites.net";

        public string Name => "CazaPagos";

        public bool Supports(PaymentMode mode) =>
            mode is PaymentMode.Card or PaymentMode.Transfer;

        public decimal CalculateFee(decimal amount, PaymentMode mode) =>
            mode switch
            {
                PaymentMode.Card => amount * 0.025m,
                PaymentMode.Transfer => amount * 0.01m,
                _ => throw new NotSupportedException()
            };

        public async Task<bool> CreateOrderAsync(Order order)
        {
            var payload = new
            {
                method = order.Status switch
                {
                    OrderStatus.Created => order.Products.Any(p => p.UnitPrice > 0) ? "Card" : "Transfer",
                    _ => "Card"
                },
                products = order.Products.Select(p => new { name = p.Name, unitPrice = p.UnitPrice }).ToArray()
            };

            var response = await $"{BaseUrl}/Order"
                .WithHeader("x-api-key", ApiKey)
                .PostJsonAsync(payload);

            return response.StatusCode == 200;
        }
    }
}
