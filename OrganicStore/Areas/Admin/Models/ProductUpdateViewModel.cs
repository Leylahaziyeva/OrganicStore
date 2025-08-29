using Microsoft.AspNetCore.Mvc.Rendering;
using OrganicStore.DataContext;

namespace OrganicStore.Areas.Admin.Models
{
    public class ProductUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public int StockQuantity { get; set; }

        public string? CoverImageUrl { get; set; }
        public IFormFile? CoverImageFile { get; set; }
        public IFormFile[]? ImageFiles { get; set; }
        public List<ProductImage> Images { get; set; } = [];

        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
        public int[] TagIds { get; set; } = [];
        public List<SelectListItem> TagSelectListItems { get; set; } = [];
        public List<ProductTag> ProductTags { get; set; } = [];
    }
}