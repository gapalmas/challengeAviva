using App.Core.Enums;

namespace App.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<Product> Products { get; set; } = new();
        public decimal Fee { get; set; }
        public string ProviderKey { get; set; } = string.Empty;
        public PaymentMode PaymentMode { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal Total => Products.Sum(p => p.UnitPrice);
        public decimal GrandTotal => Total + Fee;
    }
}
