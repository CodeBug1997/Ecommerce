using Ecommerce.Base.Attributes;
using Ecommerce.Repository.Common;

namespace Ecommerce.Repository.Entities
{
    [Table("orders")]
    public class Order : BaseEntity
    {
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItem> Items { get; set; } = [];
    }
}
