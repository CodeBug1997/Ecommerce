using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Repository.Entities
{
    [Table("idempotency_orders")]
    public class IdempotencyOrder
    {
        public string IdempotencyKey { get; set; } = string.Empty;
        public long OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
