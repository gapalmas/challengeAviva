using App.Core.Enums;

namespace App.Core.Dto.Request
{
    public class OrderRequestDto
    {
        public List<Guid> ProductIds { get; set; } = new();
        public PaymentMode PaymentMode { get; set; }
    }
}
