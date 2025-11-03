using Filmify.Models;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Filmify.Servicios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(@"
                INSERT INTO Usuarios (NombreUsuario, Contrasena, Rol, EMail)
                VALUES (@NombreUsuario, @Contrasena, @Rol, @EMail);", usuario);
        }
    }

    public interface IRepositorioUsuarios
    {
        Task Crear(Usuario usuario);
    }
}