using Consultorio_PDS2.Models;
using Dapper;
using System.Data.SqlClient;
using Filmify.Models;

namespace Filmify.Servicios
{
    public interface IRepositorioEspecialidades
    {
        Task<IEnumerable<Especialidad>> ObtenerTodos();
    }
    public class RepositorioEspecialidades : IRepositorioEspecialidades
    {
        private readonly string connectionString;

        public RepositorioEspecialidades(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<Especialidad>> ObtenerTodos()
        {
            using var connection = new SqlConnection(connectionString);
            var sql = "SELECT idEspecialidad, Nombre FROM Especialidad";
            return await connection.QueryAsync<Especialidad>(sql);
        }

    }
}



