namespace OrganicStore.DataContext
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }
        public List<Product> Products { get; set; } = [];
        public string? ImageUrl { get; set; } 
    }
}