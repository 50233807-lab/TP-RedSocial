using Microsoft.AspNetCore.Mvc;

namespace TPMOGUE.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login", "Usuario");
        }
    }
}