using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Service.Dtos.CommonDtos
{
    public abstract class IdempotencyBase
    {
        public string Key { get; set; } = string.Empty;
    }
}