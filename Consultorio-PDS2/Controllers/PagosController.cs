using Filmify.Models;
using Filmify.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Consultorio_PDS2.Controllers
{
    public class PagosController: Controller
    {
        private readonly IRepositorioPagos repositorioPagos;
        private readonly IRepositorioDoctores repositorioDoctores;

        public PagosController(IRepositorioPagos repositorioPagos, IRepositorioDoctores repositorioDoctores)

        {
            this.repositorioPagos = repositorioPagos;
            this.repositorioDoctores = repositorioDoctores;
        }

        public async Task<IActionResult> PagosDelDoctor()
        {
            // 1. Obtener el ID del usuario logueado
            var idUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
            Console.WriteLine("idUsuario del logged user: " + idUsuario);

            // 2. Obtener el doctor basado en el usuario logueado
            var doctor = await repositorioDoctores.ObtenerPorIdUsuario(idUsuario);

            if (doctor == null)
            {
                ViewBag.Error = "No se encontró información del doctor.";
                return View(new DoctorConsultas());
            }

            Console.WriteLine("Doctor encontrado - ID: " + doctor.IdDoctor);

            var pagos = await repositorioPagos.ObtenerPagosPorDoctor(doctor.IdDoctor);
            return View(pagos);
        }
    }
}
