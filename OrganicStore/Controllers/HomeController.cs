using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicStore.DataContext;
using OrganicStore.Models;

namespace OrganicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
           var products = _dbContext.Products
                .Include(p => p.Category!)
                .Include(p => p.ProductTags!).ThenInclude(pt => pt.Tag)
                .ToList();

           var categories = _dbContext.Categories.ToList();
           var tags = _dbContext.Tags.ToList();

            var homeViewModel = new HomeViewModel
            {
                Products = products,
                Categories = categories,
                Tags = tags
            };

            return View(homeViewModel);
        }    
    }
}
