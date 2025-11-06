using Microsoft.AspNetCore.Mvc.Rendering;
using Filmify.Models;

namespace Filmify.Models
{
    public class DoctorConsultas
    {
        public int IdPacienteSeleccionado { get; set; }
        public IEnumerable<SelectListItem> Pacientes { get; set; }

        public IEnumerable<HistorialConsulta> Consultas { get; set; }
    }
}
