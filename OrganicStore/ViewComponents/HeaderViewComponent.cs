using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicStore.DataContext;
using OrganicStore.Models;

namespace OrganicStore.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public readonly AppDbContext _dbContext;

        public HeaderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var logo = await _dbContext.Logos.FirstOrDefaultAsync();
            var socialLinks = await _dbContext.SocialLinks.ToListAsync();
            var contactInfos = await _dbContext.ContactInfos.ToListAsync();

            var headerViewModel = new HeaderViewModel
            {
                LogoUrl = logo?.LogoPath,
                SocialLinks = socialLinks,
                ContactInfos = contactInfos
            };

            return View(headerViewModel);
        }
    }
}