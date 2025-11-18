using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio_PDS2.Models
{
    public class RegistrarConsultaViewModel
    {
        public Consulta Consulta { get; set; } = new Consulta();
        public IEnumerable<SelectListItem> Doctores { get; set; }
        public IEnumerable<SelectListItem> Pacientes { get; set; }
    }
}