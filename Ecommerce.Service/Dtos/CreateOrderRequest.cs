using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Service.Dtos
{
    public class CreateOrderRequest
    {
        [Required]
        [MinLength(1)]
        public List<CreateOrderItemRequest> Items { get; set; } = [];
    }
}
