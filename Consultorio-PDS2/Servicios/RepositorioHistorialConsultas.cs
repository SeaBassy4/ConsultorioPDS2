using Dapper;
using System.Data;
using Filmify.Models;
using System.Data.SqlClient;



namespace Filmify.Servicios
{
    public interface IRepositorioHistorialConsultas
    {
        Task<IEnumerable<Consulta>> ObtenerPorDoctorYPaciente(int idDoctor, int idPaciente);
        Task<IEnumerable<HistorialConsulta>> ObtenerPorPaciente(int idPaciente);
    }

    public class RepositorioHistorialConsultas : IRepositorioHistorialConsultas
    {
        private readonly string connectionString;

        public RepositorioHistorialConsultas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<HistorialConsulta>> ObtenerPorPaciente(int idPaciente)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<HistorialConsulta>(
                "sp_HistorialPaciente",
                new { IdPaciente = idPaciente },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Consulta>> ObtenerPorDoctorYPaciente(int idDoctor, int idPaciente)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Consulta>(
                "EXEC sp_ConsultasPorDoctorYPaciente @IdDoctor, @IdPaciente",
                new { IdDoctor = idDoctor, IdPaciente = idPaciente });
        }




    }
}
