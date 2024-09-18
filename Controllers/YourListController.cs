using Microsoft.AspNetCore.Mvc;

namespace GameLibrary.Controllers
{
    public class YourListController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/List/YourList.cshtml"); // Provide the exact path to the view
        }
    }
}
