using Microsoft.AspNetCore.Mvc;

namespace GameStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
