using Microsoft.AspNetCore.Mvc;
using Filmify.Models;
using Filmify.Servicios;

namespace Filmify.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public UsuariosController(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            try
            {
                await repositorioUsuarios.Crear(usuario);

                TempData["MensajeExito"] = $"El usuario {usuario.NombreUsuario} fue creado exitosamente.";

                // ✅ Redirect to Usuarios/Index (or Home/Index)
                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", $"Error al crear el usuario: {ex.Message}");
                return View(usuario);
            }
        }

    }
}