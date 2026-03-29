using Ecommerce.Base.Exeptions;
using Ecommerce.Repository.Entities;
using Ecommerce.Repository.Repositories;
using Ecommerce.Service.Dtos;
using System.Data;

namespace Ecommerce.Service.Services
{
    public class OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IDbConnection connection) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IDbConnection _connection = connection;

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {

            if (request.Items == null || request.Items.Count == 0)
                throw new BadRequestException("Order must contain at least one item.");

            if (request.Items.Any(x => x.Quantity <= 0))
                throw new BadRequestException("Quantity must be greater than 0.");

            var productIds = request.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = (await _productRepository.GetByIdsAsync(productIds)).ToList();
            var productMap = products.ToDictionary(x => x.Id);

            var missingProductIds = productIds.Except(products.Select(p => p.Id)).ToList();
            if (missingProductIds.Count != 0)
                throw new NotFoundException($"Products not found: {string.Join(", ", missingProductIds)}");

            foreach (var item in request.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);

                if (product.StockQuantity < item.Quantity)
                {
                    throw new ConflictException(
                        $"Product '{product.Name}' does not have enough stock. Available: {product.StockQuantity}, Requested: {item.Quantity}");
                }
            }

            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            using var transaction = _connection.BeginTransaction();

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
                    Status = "Pending",
                    Items = orderItems,
                    TotalAmount = orderItems.Sum(x => x.LineTotal)
                };

                var orderId = await _orderRepository.CreateAsync(order, transaction);
                order.Id = orderId;

                foreach (var item in order.Items)
                {
                    item.OrderId = orderId;
                }

                await _orderRepository.CreateItemsAsync(order.Items, transaction);

                foreach (var item in order.Items)
                {
                    var updatedStock = await _productRepository.DecreaseStockAsync(
                        item.ProductId,
                        item.Quantity,
                        transaction);
                    if (updatedStock != 1)
                    {
                        var currentProduct = await _productRepository.GetByIdAsync(item.ProductId, transaction) ?? throw new ConflictException($"Product with id '{item.ProductId}' no longer exists.");
                        throw new ConflictException(
                            $"Product '{currentProduct.Name}' does not have enough stock. Available: {currentProduct.StockQuantity}, Requested: {item.Quantity}");
                    }
                }

                transaction.Commit();
                return order;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
