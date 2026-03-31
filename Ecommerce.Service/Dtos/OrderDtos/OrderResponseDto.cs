using Ecommerce.Base.Contants;

namespace Ecommerce.Service.Dtos.OrderDtos
{
    public class OrderResponseDto
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public decimal TotalAmount { get; set; }

        public EOrderStatus Status { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = [];
    }
}