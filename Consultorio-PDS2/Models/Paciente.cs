using Consultorio_PDS2.Validaciones;
using System.ComponentModel.DataAnnotations;


namespace Filmify.Models
{
    public class Paciente
    {
        public int IdPaciente { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Telefono10Digitos]
        public string Telefono { get; set; }
        [EmailValido]
        public string EMail { get; set; }
        [Required]
        public string Direccion { get; set; }
    }
}
