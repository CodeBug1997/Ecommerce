using Ecommerce.Base.Contants;
using Ecommerce.Base.Exeptions;
using Ecommerce.Repository.Entities;
using Ecommerce.Repository.Repositories;
using Ecommerce.Repository.Transactions;
using Ecommerce.Service.Dtos.OrderDtos;
using System.Data;

namespace Ecommerce.Service.Services
{
    public class OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request)
        {

            if (request.Items == null || request.Items.Count == 0)
                throw new BadRequestException("Order must contain at least one item.");

            if (request.Items.Any(x => x.Quantity <= 0))
                throw new BadRequestException("Quantity must be greater than 0.");

            // Group items by product to calculate total quantity for each product
            var productGroup = request.Items.GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .ToList();

            var productIds = productGroup
               .Select(x => x.ProductId)
               .ToList();

            var products = (await _productRepository.GetByIdsAsync(productIds)).ToList();
            var productMap = products.ToDictionary(x => x.Id);

            var missingProductIds = productIds.Except(products.Select(p => p.Id)).ToList();
            if (missingProductIds.Count != 0)
                throw new NotFoundException($"Products not found: {string.Join(", ", missingProductIds)}");

            foreach (var item in productGroup)
            {
                var product = productMap[item.ProductId];

                if (product.StockQuantity < item.Quantity)
                {
                    throw new ConflictException(
                        $"Product '{product.Name}' does not have enough stock. Available: {product.StockQuantity}, Requested: {item.Quantity}");
                }
            }

            await _unitOfWork.BeginAsync();

            try
            {
                var orderItems = request.Items.Select(item =>
                {
                    var product = productMap[item.ProductId];

                    return new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        LineTotal = product.Price * item.Quantity
                    };
                }).ToList();

                var order = new Order
                {
                    CreatedAt = DateTime.UtcNow,
                    Status = EOrderStatus.Pending,
                    Items = orderItems,
                    TotalAmount = orderItems.Sum(x => x.LineTotal)
                };

                var orderId = await _orderRepository.CreateAsync(order, _unitOfWork.Transaction);
                order.Id = orderId;

                foreach (var item in order.Items)
                {
                    item.OrderId = orderId;
                }

                await _orderRepository.CreateItemsAsync(order.Items, _unitOfWork.Transaction);

                foreach (var item in productGroup)
                {
                    var updatedStock = await _productRepository.DecreaseStockAsync(
                        item.ProductId,
                        item.Quantity,
                        _unitOfWork.Transaction);
                    if (updatedStock != 1)
                    {
                        var currentProduct = await _productRepository.GetByIdAsync(item.ProductId, _unitOfWork.Transaction) ?? throw new ConflictException($"Product with id '{item.ProductId}' no longer exists.");
                        throw new ConflictException(
                            $"Product '{currentProduct.Name}' does not have enough stock. Available: {currentProduct.StockQuantity}, Requested: {item.Quantity}");
                    }
                }

                await _unitOfWork.CommitAsync();

                return MapToResponse(order)!;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<Order?> CancelOrderAsync(long id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Order id must be greater than 0");
            }

            await _unitOfWork.BeginAsync();
            try
            {
                var currentOrder = await _orderRepository.GetWithItemsAsync(id, _unitOfWork.Transaction) ?? throw new NotFoundException($"Order with id '{id}' does not exist.");

                if (currentOrder.Items == null || currentOrder.Items.Count == 0)
                    throw new BadRequestException($"Order with id: {id} has no items.");

                if (currentOrder.Status != EOrderStatus.Pending)
                    throw new ConflictException($"Only pending orders can be cancelled. Current status: {currentOrder.Status}");

                currentOrder.Status = EOrderStatus.Cancelled;
                var updateOrderResult = await _orderRepository.UpdateAsync(currentOrder, _unitOfWork.Transaction);
                if (updateOrderResult == 0)
                    throw new ConflictException($"Order with id: {id} no longer exists.");

                // Restore stock for each item
                foreach (var item in currentOrder.Items)
                {
                    var restoreStockResult = await _productRepository.IncreaseStockAsync(item.ProductId, item.Quantity, _unitOfWork.Transaction);
                    if (restoreStockResult == 0)
                    {
                        var currentProduct = await _productRepository.GetByIdAsync(item.ProductId, _unitOfWork.Transaction) ?? throw new ConflictException($"Product with id '{item.ProductId}' no longer exists.");
                        throw new ConflictException($"Failed to restore stock for product '{currentProduct.Name}'. Please check the product status.");
                    }
                }
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return await _orderRepository.GetWithItemsAsync(id);
        }

        public async Task<OrderResponseDto?> GetOrderAsync(long id)
        {
            return MapToResponse(await _orderRepository.GetWithItemsAsync(id)) ?? throw new BadRequestException($"Order with id '{id}' does not exist.");
        }

        private static OrderResponseDto? MapToResponse(Order? order)
        {
            return order == null ? null : new OrderResponseDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = [.. order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.LineTotal
                })]
            };
        }
    }
}
