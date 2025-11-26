using Consultorio_PDS2.Models;
using Filmify.Models;
using Filmify.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text.Json;



public class DoctorController : Controller
{
    private readonly IRepositorioPacientes repositorioPacientes;
    private readonly IRepositorioHistorialConsultas repositorioHistorial;
    private readonly IRepositorioConsultas repositorioConsultas;
    private readonly IRepositorioDoctores repositorioDoctores;
    private readonly IRepositorioPagos repositorioPagos;
    private readonly IRepositorioEspecialidades repositorioEspecialidades;
    private readonly IRepositorioUsuarios repositorioUsuarios;




    public DoctorController(
        IRepositorioPacientes repositorioPacientes,
        IRepositorioHistorialConsultas repositorioHistorial,
        IRepositorioConsultas repositorioConsultas,
        IRepositorioDoctores repositorioDoctores,
        IRepositorioPagos repositorioPagos,
        IRepositorioEspecialidades repositorioEspecialidades,
        IRepositorioUsuarios repositorioUsuarios

        )
    {
        this.repositorioPacientes = repositorioPacientes;
        this.repositorioHistorial = repositorioHistorial;
        this.repositorioConsultas = repositorioConsultas;
        this.repositorioDoctores = repositorioDoctores;
        this.repositorioPagos = repositorioPagos;
        this.repositorioEspecialidades = repositorioEspecialidades;
        this.repositorioUsuarios = repositorioUsuarios;
    }

    /*
    [HttpGet]
    public async Task<IActionResult> VerConsultas(int? idPaciente)
    {
        // 1. Obtener el ID del usuario logueado
        var idUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
        Console.WriteLine("idUsuario del logged user: " + idUsuario);

        // 2. Obtener el doctor basado en el usuario logueado
        var doctor = await repositorioDoctores.ObtenerPorIdUsuario(idUsuario);

        if (doctor == null)
        {
            ViewBag.Error = "No se encontró información del doctor.";
            return View(new DoctorConsultas());
        }

        Console.WriteLine("Doctor encontrado - ID: " + doctor.IdDoctor);

        // 3. Obtener pacientes de ESTE doctor
        var pacientes = await repositorioPacientes.ObtenerPorDoctor(doctor.IdDoctor);

        var modelo = new DoctorConsultas
        {
            Pacientes = pacientes.Select(p =>
                new SelectListItem($"{p.Nombre} {p.Apellido}", p.IdPaciente.ToString()))
        };

        // 4. Si se seleccionó un paciente, obtener sus consultas
        if (idPaciente.HasValue && idPaciente.Value > 0)
        {
            modelo.IdPacienteSeleccionado = idPaciente.Value;

            // Usar doctor.IdDoctor (del doctor logueado) en lugar del parámetro
            var consultas = await repositorioHistorial.ObtenerPorDoctorYPaciente(doctor.IdDoctor, idPaciente.Value);

            modelo.Consultas = consultas.Select(c => new HistorialConsulta
            {
                FechaConsulta = c.FechaConsulta,
                Motivo = c.Motivo,
                Diagnostico = c.Diagnostico,
                Tratamiento = c.Tratamiento,
                Observaciones = c.Observaciones,
                // Estado = c.Estado
            });
        }

        return View(modelo);
    }
    */

    [HttpGet]
    public async Task<IActionResult> VerConsultas(int? idPaciente)
    {
        var idUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
        var doctor = await repositorioDoctores.ObtenerPorIdUsuario(idUsuario);
        var idDoctor = doctor.IdDoctor;

        var pacientes = await repositorioPacientes.ObtenerPorDoctor(idDoctor);

        var modelo = new DoctorConsultas
        {
            IdPacienteSeleccionado = idPaciente ?? 0, 
            Pacientes = new List<SelectListItem>
                        {
                            new SelectListItem("-- Todos los pacientes --", "") 
                        }
                .Concat(pacientes.Select(p =>
                    new SelectListItem($"{p.Nombre} {p.Apellido}", p.IdPaciente.ToString())))
                .ToList()
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
            });
        }

        return View(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarConsulta(RegistrarConsultaViewModel modelo)
    {

        Console.WriteLine("=== CLAIMS DEL USUARIO ===");
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Tipo: {claim.Type}, Valor: {claim.Value}");
        }

        var idUsuario = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
        Console.WriteLine("IdUsuario logged: " + idUsuario);

        var doctor = await repositorioDoctores.ObtenerPorIdUsuario(idUsuario);

        modelo.Consulta.IdDoctor = doctor.IdDoctor;

        Console.WriteLine("IdDoctor del usuario logged: " + doctor.IdDoctor);


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



    
    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        ViewBag.Especialidades = await repositorioEspecialidades.ObtenerTodos();
        return View();
    }

    
    [HttpPost]
    public async Task<IActionResult> Crear(DoctorRegistroViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Especialidades = await repositorioEspecialidades.ObtenerTodos();
            return View(vm);
        }

        // 1️⃣ Crear usuario
        var usuario = new Usuario
        {
            NombreUsuario = vm.NombreUsuario,
            Contrasena = vm.Contrasena,
            EMail = vm.EMailUsuario,
            Rol = "Doctor"
        };

        int idUsuario = await repositorioUsuarios.Crear(usuario);

        // 2️⃣ Crear doctor asociado
        var doctor = new Doctor
        {
            Nombre = vm.Nombre,
            Apellido = vm.Apellido,
            Telefono = vm.Telefono,
            EMail = vm.EMailUsuario,
            IdEspecialidad = vm.idEspecialidad,
            IdUsuario = idUsuario
        };

        await repositorioDoctores.Crear(doctor);

        TempData["success"] = "Doctor registrado exitosamente.";
        return RedirectToAction("Crear");
    }




}