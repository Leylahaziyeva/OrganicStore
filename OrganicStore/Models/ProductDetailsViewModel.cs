using OrganicStore.DataContext;

namespace OrganicStore.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } = null!;
        public List<Category> Categories { get; set; } = new();
        public List<Product> RelatedProducts { get; set; } = new();     
    }
}