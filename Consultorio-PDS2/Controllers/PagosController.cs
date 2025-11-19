using Filmify.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio_PDS2.Controllers
{
    public class PagosController: Controller
    {
        private readonly IRepositorioPagos repositorioPagos;

        public PagosController(IRepositorioPagos repositorioPagos)
        {
            this.repositorioPagos = repositorioPagos;
        }

        public async Task<IActionResult> PagosDelDoctor()
        {
            // Aquí obtienes el ID del doctor logueado
            // Por ahora lo hardcodeamos para probar
            int idDoctor = 1; // Cambia esto por cómo obtienes el ID del doctor logueado

            var pagos = await repositorioPagos.ObtenerPagosPorDoctor(idDoctor);
            return View(pagos);
        }
    }
}
