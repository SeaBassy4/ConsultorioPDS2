using Filmify.Servicios;
using Filmify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class DoctorController : Controller
{
    private readonly IRepositorioPacientes repositorioPacientes;
    private readonly IRepositorioHistorialConsultas repositorioHistorial;
    private readonly IRepositorioConsultas repositorioConsultas;
    private readonly IRepositorioPacientes _repositorioPacientes;


    public DoctorController(
        IRepositorioPacientes repositorioPacientes,
        IRepositorioHistorialConsultas repositorioHistorial,
        IRepositorioConsultas repositorioConsultas
        )
    {
        this.repositorioPacientes = repositorioPacientes;
        this.repositorioHistorial = repositorioHistorial;
        this.repositorioConsultas = repositorioConsultas;
    }

    [HttpGet]
    public async Task<IActionResult> VerConsultas(int idDoctor, int? idPaciente)
    {
        var pacientes = await repositorioPacientes.ObtenerPorDoctor(idDoctor);

        var modelo = new DoctorConsultas
        {
            Pacientes = pacientes.Select(p =>
                new SelectListItem($"{p.Nombre} {p.Apellido}", p.IdPaciente.ToString()))
        };

        if (idPaciente.HasValue)
        {
            modelo.IdPacienteSeleccionado = idPaciente.Value;
            var consultas = await repositorioHistorial.ObtenerPorDoctorYPaciente(idDoctor, idPaciente.Value);
            modelo.Consultas = consultas.Select(c => new HistorialConsulta
            {
                FechaConsulta = c.FechaConsulta,
                Motivo = c.Motivo,
                Diagnostico = c.Diagnostico,
                Tratamiento = c.Tratamiento,
                Observaciones = c.Observaciones,
                Estado = c.Estado
            }) ;

        }

        return View(modelo);
    }


    [HttpPost]
    public async Task<IActionResult> RegistrarConsulta(Consulta consulta)
    {
        if (!ModelState.IsValid)
            return View(consulta);

        consulta.FechaConsulta = DateTime.Now;
        await repositorioConsultas.Crear(consulta);

        TempData["mensaje"] = "Consulta registrada exitosamente.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult RegistrarConsulta()
    {
        return View();
    }



    [HttpGet]
    public IActionResult RegistrarPaciente()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> RegistrarPaciente(Paciente paciente)
    {
        if (!ModelState.IsValid)
        {
            return View(paciente);
        }

        await repositorioPacientes.Crear(paciente);

        TempData["Mensaje"] = "Paciente registrado correctamente ✅";
        return RedirectToAction("RegistrarPaciente");
    }


}
