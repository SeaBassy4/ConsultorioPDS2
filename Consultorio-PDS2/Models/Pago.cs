namespace Filmify.Models
{
    public class Pago
    {
        public int IdPago { get; set; }
        public int IdConsulta { get; set; }
        public int IdPaciente { get; set; }
        public double Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
