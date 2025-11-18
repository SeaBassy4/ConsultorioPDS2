using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Filmify.Servicios;
using Filmify.Models;
using Microsoft.AspNetCore.Authorization;


namespace Filmify.Controllers
{
    
    public class LoginRegistroController : Controller
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public LoginRegistroController(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        // -------------------- LOGIN --------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(int idUsuario, string contrasena)
        {
            if (idUsuario <= 0 || string.IsNullOrWhiteSpace(contrasena))
            {
                ViewBag.Error = "Ingrese ID de usuario y contraseña.";
                return View();
            }

            var usuario = await repositorioUsuarios.ObtenerPorId(idUsuario);

            if (usuario == null || usuario.Contrasena != contrasena)
            {
                ViewBag.Error = "ID o contraseña incorrectos.";
                return View();
            }

            // Crear claims
            var claims = new List<Claim>
            {
                  new Claim("IdUsuario", usuario.idUsuario.ToString()),
                  new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                  new Claim(ClaimTypes.Role, usuario.Rol)
             };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            return RedirectToAction("Index", "Home");
        }

        // -------------------- REGISTRO --------------------

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            await repositorioUsuarios.Crear(usuario);

            TempData["mensaje"] = "Usuario registrado exitosamente.";
            return RedirectToAction("Login");
        }

        // -------------------- LOGOUT --------------------
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // -------------------- ACCESS DENIED --------------------
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
