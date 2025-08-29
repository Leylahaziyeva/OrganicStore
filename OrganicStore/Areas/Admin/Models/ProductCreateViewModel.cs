using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrganicStore.Areas.Admin.Models
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public int StockQuantity { get; set; }

        public IFormFile CoverImageFile { get; set; } = null!;
        public IFormFile[] ImageFiles { get; set; } = [];

        public int CategoryId { get; set; }
        public List<SelectListItem> CategorySelectListItems { get; set; } = [];
        public int[] TagIds { get; set; } = [];
        public List<SelectListItem> TagSelectListItems { get; set; } = [];
    }
}