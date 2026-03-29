using Ecommerce.Base.Attributes;
using Ecommerce.Repository.Common;

namespace Ecommerce.Repository.Entities
{
    [Table("order_items")]
    public class OrderItem: BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
