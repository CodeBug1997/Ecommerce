using Ecommerce.Repository.Common;
using Ecommerce.Repository.Entities;
using Ecommerce.Service.Dtos.OrderDtos;
using Ecommerce.Service.Services;

namespace Ecommerce.Service.Idempotency
{
    public class IdempotencyOrderServiceDecorator(
            IOrderService inner,
            IIdempotencyRepository<IdempotencyOrder> repo) : IOrderService
    {
        private readonly IOrderService _inner = inner;
        private readonly IIdempotencyRepository<IdempotencyOrder> _repo = repo;

        public async Task<Order?> CancelOrderAsync(long id)
        {
            return await _inner.CancelOrderAsync(id);
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Key))
                return await _inner.CreateOrderAsync(request);

            var existing = await _repo.GetAsync(request.Key);

            if (existing != null)
            {
                var existingOrder = await _inner.GetOrderAsync(existing.OrderId);
                if (existingOrder != null)
                    return existingOrder;
            }

            var response = await _inner.CreateOrderAsync(request);

            return response;
        }

        public async Task<OrderResponseDto?> GetOrderAsync(long id)
        {
            return await _inner.GetOrderAsync(id);
        }
    }
}
