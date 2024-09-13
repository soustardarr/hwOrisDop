using Microsoft.AspNetCore.Mvc;

namespace HomeWork.Redis.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
