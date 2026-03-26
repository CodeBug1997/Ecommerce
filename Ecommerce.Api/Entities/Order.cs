using Ecommerce.Api.Contants;

namespace Ecommerce.Api.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus? Status { get; set; }
    }
}
