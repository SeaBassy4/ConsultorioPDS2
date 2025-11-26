namespace Filmify.Models
{
    public class DoctorRegistroViewModel
    {
        // Usuario
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string EMailUsuario { get; set; }

        // Doctor
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }

        public int idEspecialidad { get; set; }
    }
}
