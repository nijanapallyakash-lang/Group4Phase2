using Microsoft.AspNetCore.Mvc;

namespace Group4Flight.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return Content("Client Flight Search Content"); // Returns Content 
        }
    }
}