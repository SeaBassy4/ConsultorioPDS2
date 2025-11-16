using Dapper;
using System.Data.SqlClient;
using Filmify.Models;

namespace Filmify.Servicios
{
    public interface IRepositorioDoctores
    {
        Task<IEnumerable<Doctor>> ObtenerTodos();
        Task<Doctor> ObtenerPorId(int idDoctor);
        Task Crear(Doctor doctor);
        Task Actualizar(Doctor doctor);
        Task Eliminar(int idDoctor);
    }

    public class RepositorioDoctores : IRepositorioDoctores
    {
        private readonly string connectionString;

        public RepositorioDoctores(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ Obtener todos los doctores
        public async Task<IEnumerable<Doctor>> ObtenerTodos()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Doctor>("SELECT * FROM Doctores");
        }

        // ✅ Obtener un doctor por ID
        public async Task<Doctor> ObtenerPorId(int idDoctor)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Doctor>(
                "SELECT * FROM Doctores WHERE IdDoctor = @IdDoctor", new { IdDoctor = idDoctor });
        }

       

        // ✅ Crear un nuevo doctor
        public async Task Crear(Doctor doctor)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"INSERT INTO Doctores (Nombre, Apellido, Telefono, EMail, idEspecialidad, idUsuario)
                        VALUES (@Nombre, @Apellido, @Telefono, @EMail, @IdEspecialidad, @IdUsuario)";
            await connection.ExecuteAsync(sql, doctor);
        }

        // ✅ Actualizar un doctor existente
        public async Task Actualizar(Doctor doctor)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"UPDATE Doctores 
                        SET Nombre = @Nombre, 
                            Apellido = @Apellido, 
                            Telefono = @Telefono, 
                            EMail = @EMail, 
                            idEspecialidad = @IdEspecialidad,
                            idUsuario = @IdUsuario
                        WHERE IdDoctor = @IdDoctor";
            await connection.ExecuteAsync(sql, doctor);
        }

        // ✅ Eliminar un doctor por ID
        public async Task Eliminar(int idDoctor)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM Doctores WHERE IdDoctor = @IdDoctor", new { IdDoctor = idDoctor });
        }
    }
}
