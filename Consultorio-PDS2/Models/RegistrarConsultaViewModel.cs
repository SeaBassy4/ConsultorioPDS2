using Filmify.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Consultorio_PDS2.Models
{
    public class RegistrarConsultaViewModel
    {
        public Consulta Consulta { get; set; } = new Consulta();

        // Campos nuevos para el pago
        public Pago Pago { get; set; } = new Pago();
        public IEnumerable<SelectListItem> Doctores { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Pacientes { get; set; } = new List<SelectListItem>();
    }
}