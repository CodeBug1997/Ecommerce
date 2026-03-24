namespace Ecommerce.Api.Services.Product
{
    public class ProductService: IProductService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
