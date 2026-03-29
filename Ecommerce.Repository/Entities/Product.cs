using Ecommerce.Base.Attributes;
using Ecommerce.Repository.Common;

namespace Ecommerce.Repository.Entities
{
    [Table("products")]
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

    }
}
