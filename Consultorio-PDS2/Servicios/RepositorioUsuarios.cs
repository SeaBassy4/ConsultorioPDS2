using Filmify.Models;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Filmify.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<int> Crear(Usuario usuario);
        Task<Usuario?> ObtenerPorId(int idUsuario);
        Task<bool> VerificarCredenciales(int idUsuario, string contrasenaPlain);
    }

    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Crear(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);

            var sql = @"
        INSERT INTO Usuarios (NombreUsuario, Contrasena, Rol, EMail)
        VALUES (@NombreUsuario, @Contrasena, @Rol, @EMail);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ";

            int id = await connection.ExecuteScalarAsync<int>(sql, usuario);
            return id;
        }

        // ✔ CORREGIDO: ahora usa idUsuario correctamente
        public async Task<Usuario?> ObtenerPorId(int idUsuario)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT idUsuario,
                         NombreUsuario,
                         Contrasena,
                         Rol,
                         EMail
                  FROM Usuarios
                  WHERE idUsuario = @idUsuario",
                new { idUsuario });
        }

        // ✔ Verifica por idUsuario, no por nombre
        public async Task<bool> VerificarCredenciales(int idUsuario, string contrasenaPlain)
        {
            var usuario = await ObtenerPorId(idUsuario);
            if (usuario == null)
                return false;

            return usuario.Contrasena == contrasenaPlain;
        }
    }
}
