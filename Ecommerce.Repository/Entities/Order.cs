using Ecommerce.Base.Attributes;
using Ecommerce.Base.Contants;
using Ecommerce.Repository.Common;

namespace Ecommerce.Repository.Entities
{
    [Table("orders")]
    public class Order : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public EOrderStatus Status { get; set; } = EOrderStatus.Pending;
        public List<OrderItem> Items { get; set; } = [];
    }
}
