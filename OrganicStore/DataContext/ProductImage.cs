namespace OrganicStore.DataContext
{
    public class ProductImage : BaseEntity
    {
        public required string ImageUrl { get; set; } 
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}