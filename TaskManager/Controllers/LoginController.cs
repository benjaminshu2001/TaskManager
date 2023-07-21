using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Home/Login.cshtml");
        }
    }
}
