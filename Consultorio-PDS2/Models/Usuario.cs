using System.ComponentModel.DataAnnotations;

namespace Filmify.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        public string Rol { get; set; }

    }
}