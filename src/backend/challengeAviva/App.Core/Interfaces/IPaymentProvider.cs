using App.Core.Entities;
using App.Core.Enums;

namespace App.Core.Interfaces
{
    public interface IPaymentProvider
    {
        string Name { get; }
        bool Supports(PaymentMode mode);
        decimal CalculateFee(decimal amount, PaymentMode mode);
        Task<bool> CreateOrderAsync(Order order);
    }
}
