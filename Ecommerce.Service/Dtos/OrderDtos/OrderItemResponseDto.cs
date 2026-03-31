namespace Ecommerce.Service.Dtos.OrderDtos
{
    public class OrderItemResponseDto
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
