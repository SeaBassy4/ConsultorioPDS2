using Dapper;
using System.Data.SqlClient;
using Filmify.Models;

namespace Filmify.Servicios
{
    public interface IRepositorioConsultas
    {
        Task Crear(Consulta consulta);
    }

    public class RepositorioConsultas : IRepositorioConsultas
    {
        private readonly string connectionString;

        public RepositorioConsultas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Consulta consulta)
        {
            using var connection = new SqlConnection(connectionString);
            var query = @"INSERT INTO Consultas 
                        (Motivo, Diagnostico, Tratamiento, Observaciones, FechaConsulta, IdDoctor, IdPaciente)
                        VALUES (@Motivo, @Diagnostico, @Tratamiento, @Observaciones, @FechaConsulta, @IdDoctor, @IdPaciente)";

            await connection.ExecuteAsync(query, consulta);
        }
    }
}
