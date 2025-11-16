using Dapper;
using System.Data;
using System.Data.SqlClient;
using Filmify.Models;

namespace Filmify.Servicios
{
    public interface IRepositorioPacientes
    {
        Task<IEnumerable<Paciente>> ObtenerPorDoctor(int idDoctor);
        Task Crear(Paciente paciente); // 👈 nuevo método
        Task<IEnumerable<Paciente>> ObtenerTodos();
    }

    public class RepositorioPacientes : IRepositorioPacientes
    {
        private readonly string connectionString;

        public RepositorioPacientes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Paciente>> ObtenerTodos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Paciente>("SELECT * FROM Pacientes");
        }

        public async Task<IEnumerable<Paciente>> ObtenerPorDoctor(int idDoctor)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Paciente>(
                "sp_PacientesPorDoctor",
                new { IdDoctor = idDoctor },
                commandType: CommandType.StoredProcedure
            );
        }

        // 👇 Nuevo método para registrar pacientes
        public async Task Crear(Paciente paciente)
        {
            using var connection = new SqlConnection(connectionString);

            var sql = @"INSERT INTO Pacientes 
                        (Nombre, Apellido, FechaNacimiento, Telefono, EMail, Direccion)
                        VALUES (@Nombre, @Apellido, @FechaNacimiento, @Telefono, @EMail, @Direccion);";

            await connection.ExecuteAsync(sql, paciente);
        }
    }
}
