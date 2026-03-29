namespace Ecommerce.Repository.Common
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
