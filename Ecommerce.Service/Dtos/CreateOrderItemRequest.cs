using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Service.Dtos
{
    public class CreateOrderItemRequest
    {
        [Range(1, long.MaxValue)]
        public long ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
