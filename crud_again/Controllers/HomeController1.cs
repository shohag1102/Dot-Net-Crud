using Microsoft.AspNetCore.Mvc;

namespace crud_again.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
