namespace Ecommerce.Service.Dtos.ProductDtos
{
    public class ProductResponseDto
    {
        public long id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public ProductResponseDto(long id, string name, decimal price, int stockQuantity)
        {
            this.id = id;
            Name = name;
            Price = price;
            StockQuantity = stockQuantity;
        }
    }
}
