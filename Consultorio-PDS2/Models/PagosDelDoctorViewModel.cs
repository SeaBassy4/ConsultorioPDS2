namespace Consultorio_PDS2.Models
{
    public class PagosDelDoctorViewModel
    {
        public string Doctor { get; set; }
        public string Paciente { get; set; }
        public string ApellidoPaciente { get; set; }  // NUEVO
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }        // NUEVO
        public DateTime FechaPago { get; set; }

        // Propiedad calculada para nombre completo
        public string PacienteCompleto => $"{Paciente} {ApellidoPaciente}";
    }
}
