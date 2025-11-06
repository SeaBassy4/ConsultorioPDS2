using Microsoft.AspNetCore.Mvc.Rendering;
using Filmify.Models;
public class Consulta
{
    public int IdConsulta { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string Tratamiento { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public DateTime FechaConsulta { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int IdDoctor { get; set; }
    public int IdPaciente { get; set; }
}
