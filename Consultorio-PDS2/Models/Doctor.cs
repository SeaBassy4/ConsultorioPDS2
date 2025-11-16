namespace Filmify.Models
{
    public class Doctor
    {
        public int IdDoctor { get; set; }          // PK
        public string Nombre { get; set; }         // Nombre del doctor
        public string Apellido { get; set; }       // Apellido del doctor
        public string Telefono { get; set; }       // Número de teléfono
        public string EMail { get; set; }          // Correo electrónico
        public int IdEspecialidad { get; set; }    // FK a la tabla Especialidades
        public int IdUsuario { get; set; }         // FK a la tabla Usuarios
    }
}
