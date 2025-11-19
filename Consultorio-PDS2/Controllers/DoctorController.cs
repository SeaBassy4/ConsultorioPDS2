using Consultorio_PDS2.Models;
using Filmify.Models;
using Filmify.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text.Json;


[Authorize(Roles = "Doctor")]
public class DoctorController : Controller
{
    private readonly IRepositorioPacientes repositorioPacientes;
    private readonly IRepositorioHistorialConsultas repositorioHistorial;
    private readonly IRepositorioConsultas repositorioConsultas;
    private readonly IRepositorioDoctores repositorioDoctores;
    private readonly IRepositorioPagos repositorioPagos;



    public DoctorController(
        IRepositorioPacientes repositorioPacientes,
        IRepositorioHistorialConsultas repositorioHistorial,
        IRepositorioConsultas repositorioConsultas,
        IRepositorioDoctores repositorioDoctores,
        IRepositorioPagos repositorioPagos
        )
    {
        this.repositorioPacientes = repositorioPacientes;
        this.repositorioHistorial = repositorioHistorial;
        this.repositorioConsultas = repositorioConsultas;
        this.repositorioDoctores = repositorioDoctores;
        this.repositorioPagos = repositorioPagos;
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
                //Estado = c.Estado
            });

        }

        return View(modelo);
    }


    [HttpPost]
    public async Task<IActionResult> RegistrarConsulta(RegistrarConsultaViewModel modelo)
    {

        modelo.Consulta.FechaConsulta = DateTime.Now;
        modelo.Pago.FechaPago = DateTime.Now;

        ModelState.Remove("Doctores");
        ModelState.Remove("Pacientes");

        // Recargar doctores y pacientes
        var doctores = await repositorioDoctores.ObtenerTodos();
        var pacientes = await repositorioPacientes.ObtenerTodos();

        modelo.Doctores = doctores.Select(d =>
            new SelectListItem($"{d.Nombre} {d.Apellido}", d.IdDoctor.ToString()));

        modelo.Pacientes = pacientes.Select(p =>
            new SelectListItem($"{p.Nombre} {p.Apellido}", p.IdPaciente.ToString()));


        if (!ModelState.IsValid)
        {

            Console.WriteLine("===== ERRORES DETALLADOS DE MODELSTATE =====");
            foreach (var key in ModelState.Keys)
            {
                var entry = ModelState[key];
                if (entry.Errors.Count > 0)
                {
                    Console.WriteLine($"Campo: '{key}'");
                    foreach (var error in entry.Errors)
                    {
                        Console.WriteLine($"  - Error: {error.ErrorMessage}");
                        Console.WriteLine($"  - Exception: {error.Exception?.Message}");
                    }
                    Console.WriteLine($"  - AttemptedValue: {entry.AttemptedValue}");
                }
            }
            Console.WriteLine("=============================================");

            Console.Write("Model state invalid, recargando view : ");
            var json = JsonSerializer.Serialize(modelo, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Console.WriteLine("===== MODELO COMPLETO RECIBIDO =====");
            Console.WriteLine(json);
            Console.WriteLine("====================================");


           
            // Devolver vista con ViewModel COMPLETO
            return View(modelo);
        }


        Console.Write("ejecutando metodo crear en el int id consulta");

        int idConsulta = await repositorioConsultas.Crear(modelo.Consulta);

        var pago = new Pago
        {
            IdConsulta = idConsulta,
            IdPaciente = modelo.Consulta.IdPaciente,
            Monto = modelo.Pago.Monto,
            MetodoPago = modelo.Pago.MetodoPago,
            FechaPago = DateTime.Now
        };

        await repositorioPagos.Crear(pago);

        TempData["mensaje"] = "Consulta registrada exitosamente.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> RegistrarConsulta()
    {
        var doctores = await repositorioDoctores.ObtenerTodos();
        var pacientes = await repositorioPacientes.ObtenerTodos();

        var viewModel = new RegistrarConsultaViewModel
        {
            Consulta = new Consulta(),
            Doctores = doctores.Select(d =>
                new SelectListItem($"{d.Nombre} {d.Apellido}", d.IdDoctor.ToString())),
            Pacientes = pacientes.Select(p =>
                new SelectListItem($"{p.Nombre} {p.Apellido}", p.IdPaciente.ToString()))
        };

        return View(viewModel);
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