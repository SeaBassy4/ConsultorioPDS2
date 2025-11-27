using Microsoft.AspNetCore.Mvc;
using Consultorio_PDS2.Models;
using Filmify.Models;
using Filmify.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text.Json;

namespace Consultorio_PDS2.Controllers
{
    public class EspecialidadesController : Controller
    {
        private readonly IRepositorioEspecialidades repositorioEspecialidades;

        public EspecialidadesController(IRepositorioEspecialidades repositorioEspecialidades)
        {
            this.repositorioEspecialidades = repositorioEspecialidades;
        }

        public async Task<IActionResult> Index()
        {
            var especialidades = await repositorioEspecialidades.ObtenerTodos();
            return View(especialidades);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Especialidad especialidad)
        {
            if (!ModelState.IsValid)
                return View(especialidad);

            await repositorioEspecialidades.Crear(especialidad);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var tieneDoctores = await repositorioEspecialidades.EspecialidadTieneDoctores(id);

            if (tieneDoctores)
            {
                TempData["Error"] = "No se puede eliminar esta especialidad porque tiene doctores asociados.";
                return RedirectToAction("Index");
            }

            await repositorioEspecialidades.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
