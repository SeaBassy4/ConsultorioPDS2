using Microsoft.AspNetCore.Mvc;

namespace Filmify.Controllers
{
    public class LoginRegistroController : Controller
    {
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }
    }
}
