using Microsoft.AspNetCore.Mvc;

namespace Sube2.HelloMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View();
        }
    }
}
