using App.Core.Entities;
using App.Core.Enums;
using App.Core.Interfaces;
using Flurl.Http;
using CommonConst = App.Application.Helpers.Constants.CommonConstants;

namespace App.Application.Services
{
    public class CazaPagosProvider : IPaymentProvider
    {
        private readonly string ApiKey = Environment.GetEnvironmentVariable(CommonConst.ApiKey) is string value ? value : "";
        private readonly string BaseUrl = Environment.GetEnvironmentVariable(CommonConst.BaseUrlCazaPagos) is string value ? value : "";

        public string Name => CommonConst.CazaPagos;

        public bool Supports(PaymentMode mode) =>
            mode is PaymentMode.CreditCard or PaymentMode.Transfer;

        public decimal CalculateFee(decimal amount, PaymentMode mode)
        {
            return mode switch
            {
                PaymentMode.CreditCard => amount switch
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
                    OrderStatus.Created => order.Products.Any(p => p.UnitPrice > 0) ? nameof(PaymentMode.CreditCard) : nameof(PaymentMode.Transfer),
                    _ => nameof(PaymentMode.CreditCard)
                },
                products = order.Products.Select(p => new { name = p.Name, unitPrice = p.UnitPrice }).ToArray()
            };

            var response = await $"{BaseUrl}/Order"
                .WithHeader(CommonConst.xapikey, ApiKey)
                .PostJsonAsync(payload);

            return response.StatusCode == 200;
        }
    }
}
