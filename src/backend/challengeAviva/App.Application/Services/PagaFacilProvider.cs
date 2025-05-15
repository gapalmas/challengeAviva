using App.Core.Entities;
using App.Core.Enums;
using App.Core.Interfaces;
using Flurl.Http;
using CommonConst = App.Application.Helpers.Constants.CommonConstants;

namespace App.Application.Services
{
    public class PagaFacilProvider : IPaymentProvider
    {
        private readonly string ApiKey = Environment.GetEnvironmentVariable(CommonConst.ApiKey) is string value ? value : "";
        private readonly string BaseUrl = Environment.GetEnvironmentVariable(CommonConst.BaseUrlPagaFacil) is string value ? value : "";

        public string Name => CommonConst.PagaFacil;

        public bool Supports(PaymentMode mode) =>
            mode is PaymentMode.Cash or PaymentMode.CreditCard;

        public decimal CalculateFee(decimal amount, PaymentMode mode) =>
            mode switch
            {
                PaymentMode.Cash => 15m,              // 15 MXN
                PaymentMode.CreditCard => amount * 0.01m,   // 1%
                _ => throw new NotSupportedException()
            };

        public async Task<bool> CreateOrderAsync(Order order)
        {
            var payload = new
            {
                method = order.Status switch
                {
                    OrderStatus.Created => order.Products.Any(p => p.UnitPrice > 0) ? nameof(PaymentMode.Cash) : nameof(PaymentMode.CreditCard),
                    _ => nameof(PaymentMode.Cash)
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
