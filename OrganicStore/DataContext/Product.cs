using Microsoft.EntityFrameworkCore;

namespace OrganicStore.DataContext
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        [Precision(18, 2)]
        public required decimal Price { get; set; }

        [Precision(18, 2)]
        public decimal Weight { get; set; }
        public int StockQuantity { get; set; } = 0;
        public string? CoverImageUrl { get; set; } 
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductImage> Images { get; set; } = [];
        public List<ProductTag> ProductTags { get; set; } = [];
    }
}