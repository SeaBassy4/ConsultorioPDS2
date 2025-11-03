using Filmify.Models;
using Filmify.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Filmify.Controllers
{
    public class HistorialConsultasController : Controller
    {
        private readonly IRepositorioHistorialConsultas repositorioHistorial;

        public HistorialConsultasController(IRepositorioHistorialConsultas repositorioHistorial)
        {
            this.repositorioHistorial = repositorioHistorial;
        }

        [HttpGet]
        public async Task<IActionResult> ViewHistorial(int? IdPaciente, int? IdDoctor, DateTime? FechaInicio, DateTime? FechaFin)
        {
            var consultas = new List<HistorialConsulta>();

            if (IdPaciente.HasValue)
            {
                // 🔹 Llama al SP solo con IdPaciente
                var resultado = await repositorioHistorial.ObtenerPorPaciente(IdPaciente.Value);
                consultas = resultado.ToList();

                // 🔹 Filtros adicionales aplicados en C#
                if (IdDoctor.HasValue)
                    consultas = consultas.Where(c => c.IdDoctor == IdDoctor.Value).ToList();

                if (FechaInicio.HasValue)
                    consultas = consultas.Where(c => c.FechaConsulta >= FechaInicio.Value).ToList();

                if (FechaFin.HasValue)
                    consultas = consultas.Where(c => c.FechaConsulta <= FechaFin.Value).ToList();
            }

            return View(consultas);
        }
    }
}
