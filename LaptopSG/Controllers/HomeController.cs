using Microsoft.AspNetCore.Mvc;
namespace SportsStore.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ViewBag.Message = "Home Page";
            TempData["Message"] = "This is the home page!";
            return View();
        }

    }
}