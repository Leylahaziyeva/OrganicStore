using Microsoft.AspNetCore.Mvc;

namespace OrganicStore.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
