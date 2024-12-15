using Microsoft.AspNetCore.Mvc;

namespace GameStoreMVC.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
