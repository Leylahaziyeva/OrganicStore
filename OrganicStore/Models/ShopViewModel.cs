using OrganicStore.DataContext;

namespace OrganicStore.Models
{
    public class ShopViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
    }
}
