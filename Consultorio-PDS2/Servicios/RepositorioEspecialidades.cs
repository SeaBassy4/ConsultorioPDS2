using Consultorio_PDS2.Models;
using Dapper;
using System.Data.SqlClient;
using Filmify.Models;

namespace Filmify.Servicios
{
    public interface IRepositorioEspecialidades
    {
        Task<IEnumerable<Especialidad>> ObtenerTodos();
        Task Crear(Especialidad especialidad);
        Task<bool> EspecialidadTieneDoctores(int idEspecialidad);
        Task Eliminar(int idEspecialidad);
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



        public async Task Crear(Especialidad especialidad)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"INSERT INTO Especialidad (Nombre)
                    VALUES (@Nombre)";
            await connection.ExecuteAsync(sql, especialidad);
        }

        public async Task<bool> EspecialidadTieneDoctores(int idEspecialidad)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"SELECT COUNT(*) FROM Doctores WHERE idEspecialidad = @idEspecialidad";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { idEspecialidad });
            return count > 0;
        }

        public async Task Eliminar(int idEspecialidad)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"DELETE FROM Especialidad WHERE idEspecialidad = @idEspecialidad";
            await connection.ExecuteAsync(sql, new { idEspecialidad });
        }






    }
}



