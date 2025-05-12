using App.Core.Entities;
using App.Core.Enums;
using App.Core.Interfaces;
using Flurl.Http;
using Newtonsoft.Json;
using System.Xml;

namespace App.Application.Services
{
    public class CazaPagosProvider : IPaymentProvider
    {
        private const string ApiKey = "apikey-fj9esodija09s2";
        private const string BaseUrl = "https://app-caza-chg-aviva.azurewebsites.net";

        public string Name => "CazaPagos";

        public bool Supports(PaymentMode mode) =>
            mode is PaymentMode.Card or PaymentMode.Transfer;

        public decimal CalculateFee(decimal amount, PaymentMode mode)
        {
            return mode switch
            {
                PaymentMode.Card => amount switch
                {
                    <= 1500 => amount * 0.02m,
                    <= 5000 => amount * 0.015m,
                    _ => amount * 0.005m
                },

                PaymentMode.Transfer => amount switch
                {
                    <= 500 => 5m,
                    <= 1000 => amount * 0.025m,
                    _ => amount * 0.02m
                },

                _ => throw new NotSupportedException()
            };
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            var payload = new
            {
                method = order.Status switch
                {
                    OrderStatus.Created => order.Products.Any(p => p.UnitPrice > 0) ? "CreditCard" : "Transfer",
                    _ => "CreditCard"
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
