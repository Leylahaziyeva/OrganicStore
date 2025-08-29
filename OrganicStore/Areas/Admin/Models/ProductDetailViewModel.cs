using OrganicStore.DataContext;

namespace OrganicStore.Areas.Admin.Models
{
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public int StockQuantity { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public List<ProductImage> Images { get; set; } = [];
        public List<string> Tags { get; set; } = [];
    }
}