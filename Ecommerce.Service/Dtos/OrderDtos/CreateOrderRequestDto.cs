using Ecommerce.Service.Dtos.CommonDtos;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Service.Dtos.OrderDtos
{
    public class CreateOrderRequestDto : IdempotencyBase
    {
        [Required]
        [MinLength(1)]
        public List<CreateOrderItemRequestDto> Items { get; set; } = [];
    }
}