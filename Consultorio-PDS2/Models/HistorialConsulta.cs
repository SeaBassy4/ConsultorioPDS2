namespace Filmify.Models
{
    public class HistorialConsulta
    {
        public int IdConsulta { get; set; }
        public int IdDoctor { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string Motivo { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public string Observaciones { get; set; }
        //public string Estado { get; set; }
        public string NombreDoctor { get; set; }
        public string ApellidoDoctor { get; set; }
        public string EmailDoctor { get; set; }
        public string TelefonoDoctor { get; set; }
        public string EspecialidadDoctor { get; set; }
    }
}
