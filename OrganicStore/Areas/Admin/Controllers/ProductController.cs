using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrganicStore.Areas.Admin.Data;
using OrganicStore.Areas.Admin.Models;
using OrganicStore.DataContext;

namespace OrganicStore.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categorySelectListItems = await GetCategorySelectListItems();
            var tagSelectListItems = await GetTagSelectListItems();

            var productCreateViewModel = new ProductCreateViewModel
            {
                CategorySelectListItems = categorySelectListItems,
                TagSelectListItems = tagSelectListItems,
                StockQuantity = 0,
                Weight = 0
            };

            return View(productCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CategorySelectListItems = await GetCategorySelectListItems();
                model.TagSelectListItems = await GetTagSelectListItems();
                return View(model);
            }

            if (!Directory.Exists(PathConstants.ProductPath))
                Directory.CreateDirectory(PathConstants.ProductPath);

            if (!model.CoverImageFile.IsImage() || !model.CoverImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("CoverImageFile", "Invalid cover image");
                model.CategorySelectListItems = await GetCategorySelectListItems();
                model.TagSelectListItems = await GetTagSelectListItems();
                return View(model);
            }

            var coverImageName = await model.CoverImageFile.GenerateFileAsync(PathConstants.ProductPath);

            var productImages = new List<ProductImage>
            {
                new ProductImage { ImageUrl = coverImageName, IsMain = true }
            };

            if (model.ImageFiles != null && model.ImageFiles.Length > 0)
            {
                foreach (var imageFile in model.ImageFiles)
                {
                    if (!imageFile.IsImage() || !imageFile.IsAllowedSize(1))
                    {
                        ModelState.AddModelError("ImageFiles", "Invalid image");
                        model.CategorySelectListItems = await GetCategorySelectListItems();
                        model.TagSelectListItems = await GetTagSelectListItems();
                        return View(model);
                    }

                    var imageName = await imageFile.GenerateFileAsync(PathConstants.ProductPath);
                    productImages.Add(new ProductImage { ImageUrl = imageName, IsMain = false });
                }
            }

            var productTags = model.TagIds.Select(tagId => new ProductTag { TagId = tagId }).ToList();

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Weight = model.Weight,
                StockQuantity = model.StockQuantity,
                CoverImageUrl = coverImageName,
                CategoryId = model.CategoryId,
                Images = productImages,
                ProductTags = productTags
            };

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var product = await _dbContext.Products
                .Include(x => x.Images)
                .Include(x => x.ProductTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            var categorySelectListItems = await GetCategorySelectListItems();
            var tagSelectListItems = await GetTagSelectListItems();

            var productUpdateViewModel = new ProductUpdateViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                StockQuantity = product.StockQuantity,
                CoverImageUrl = product.CoverImageUrl,
                Images = product.Images,
                CategoryId = product.CategoryId,
                CategorySelectListItems = categorySelectListItems,
                TagIds = product.ProductTags.Select(x => x.TagId).ToArray(),
                TagSelectListItems = tagSelectListItems,
                ProductTags = product.ProductTags
            };

            return View(productUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateViewModel model)
        {
            var product = await _dbContext.Products
                .Include(x => x.Images)
                .Include(x => x.ProductTags)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model = await GetProductUpdateViewModel(product);
                return View(model);
            }

            if (!Directory.Exists(PathConstants.ProductPath))
                Directory.CreateDirectory(PathConstants.ProductPath);

            if (model.CoverImageFile != null)
            {
                if (!model.CoverImageFile.IsImage() || !model.CoverImageFile.IsAllowedSize(1))
                {
                    ModelState.AddModelError("CoverImageFile", "Invalid cover image");
                    model = await GetProductUpdateViewModel(product);
                    return View(model);
                }

                if (!string.IsNullOrEmpty(product.CoverImageUrl))
                {
                    var oldPath = Path.Combine(PathConstants.ProductPath, product.CoverImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var coverName = await model.CoverImageFile.GenerateFileAsync(PathConstants.ProductPath);
                product.CoverImageUrl = coverName;

                var oldMain = product.Images.FirstOrDefault(x => x.IsMain);
                if (oldMain != null) oldMain.ImageUrl = coverName;
                else
                {
                    product.Images.Add(new ProductImage { ImageUrl = coverName, IsMain = true });
                }
            }

            if (model.ImageFiles != null && model.ImageFiles.Length > 0)
            {
                foreach (var imageFile in model.ImageFiles)
                {
                    if (!imageFile.IsImage() || !imageFile.IsAllowedSize(1))
                    {
                        ModelState.AddModelError("ImageFiles", "Invalid image file");
                        model = await GetProductUpdateViewModel(product);
                        return View(model);
                    }

                    var imageName = await imageFile.GenerateFileAsync(PathConstants.ProductPath);
                    product.Images.Add(new ProductImage { ImageUrl = imageName, IsMain = false });
                }
            }

            product.ProductTags.Clear();
            foreach (var tagId in model.TagIds)
            {
                product.ProductTags.Add(new ProductTag { ProductId = product.Id, TagId = tagId });
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Weight = model.Weight;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return Json(new { success = true });
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            var productDetailViewModel = new ProductDetailViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                StockQuantity = product.StockQuantity,
                CoverImageUrl = product.CoverImageUrl,
                CategoryName = product.Category?.Name,
                Images = product.Images,
                Tags = product.ProductTags.Select(pt => pt.Tag.Name).ToList()
            };

            return View(productDetailViewModel);
        }

        public async Task<List<SelectListItem>> GetCategorySelectListItems()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();
        }

        public async Task<List<SelectListItem>> GetTagSelectListItems()
        {
            var tags = await _dbContext.Tags.ToListAsync();
            return tags
                .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
                .ToList();
        }

        public async Task<ProductUpdateViewModel> GetProductUpdateViewModel(Product product)
        {
            var categorySelectListItems = await GetCategorySelectListItems();
            var tagSelectListItems = await GetTagSelectListItems();

            ProductUpdateViewModel model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Weight = product.Weight,
                StockQuantity = product.StockQuantity,
                CoverImageUrl = product.CoverImageUrl,
                Images = product.Images,
                CategoryId = product.CategoryId,
                CategorySelectListItems = categorySelectListItems,
                TagIds = product.ProductTags.Select(x => x.TagId).ToArray(),
                TagSelectListItems = tagSelectListItems,
                ProductTags = product.ProductTags
            };

            return model;
        }

    }
}