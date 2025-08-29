using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicStore.Areas.Admin.Data;
using OrganicStore.DataContext;

namespace OrganicStore.Areas.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existCategory = await _dbContext.Categories.AnyAsync(c => c.Name == category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "Category with this name already exists.");
                return View(category);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (!Directory.Exists(PathConstants.CategoryPath))
                    Directory.CreateDirectory(PathConstants.CategoryPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(PathConstants.CategoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                category.ImageUrl = fileName;
            }

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var category = _dbContext.Categories.Find(id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var dbCategory = await _dbContext.Categories.FindAsync(id);
                return View(dbCategory);
            }

            var existCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existCategory == null) return NotFound();

            var hasNewName = await _dbContext.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id);
            if (hasNewName)
            {
                ModelState.AddModelError("Name", "Category with this name already exists.");
                return View(existCategory);
            }

            existCategory.Name = category.Name;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (!Directory.Exists(PathConstants.CategoryPath))
                    Directory.CreateDirectory(PathConstants.CategoryPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(PathConstants.CategoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(existCategory.ImageUrl))
                {
                    var oldImagePath = Path.Combine(PathConstants.CategoryPath, existCategory.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                existCategory.ImageUrl = fileName;
            }

            _dbContext.Categories.Update(existCategory);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();

            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var imagePath = Path.Combine(PathConstants.CategoryPath, category.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();

            return Json(new { IsDeleted = true });
        }
    }
}