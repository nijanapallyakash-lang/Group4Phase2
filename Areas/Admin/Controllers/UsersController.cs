using Microsoft.AspNetCore.Mvc;

namespace Group4Flight.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Returns the Dashboard view 
        }

        public IActionResult Manage()
        {
            return Content("Admin Manage Users Content"); // Returns Content 
        }

        public IActionResult Rights()
        {
            return Content("Admin Rights & Obligations Content"); // Returns Content 
        }
    }
}