namespace Ecommerce.Service.Services
{
    public class ProductService: IProductService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
