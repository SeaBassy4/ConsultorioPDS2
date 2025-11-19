
using Consultorio_PDS2.Models;
using Dapper;
    using Filmify.Models;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Data.SqlClient;

    namespace Filmify.Servicios
    {
        public interface IRepositorioPagos
        {
            Task<IEnumerable<Pago>> ObtenerTodos();
            Task<Pago> ObtenerPorId(int idPago);

        Task<List<PagosDelDoctorViewModel>> ObtenerPagosPorDoctor(int idDoctor);
        Task<int> Crear(Pago pago);
            Task Actualizar(Pago pago);
            Task Eliminar(int idPago);
            Task<IEnumerable<Pago>> ObtenerPorIdConsulta(int idConsulta);
        }

        public class RepositorioPagos : IRepositorioPagos
        {
            private readonly string connectionString;

            public RepositorioPagos(IConfiguration configuration)
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            // ✅ Obtener todos los pagos
            public async Task<IEnumerable<Pago>> ObtenerTodos()
            {
                using var connection = new SqlConnection(connectionString);
                return await connection.QueryAsync<Pago>("SELECT * FROM Pagos");
            }

        public async Task<List<PagosDelDoctorViewModel>> ObtenerPagosPorDoctor(int idDoctor)
        {
            var sql = @"
        SELECT 
            pg.Monto,
            pg.MetodoPago,
            pg.FechaPago,
            p.Nombre as NombrePaciente,
            p.Apellido as ApellidoPaciente
        FROM Pagos pg
        INNER JOIN Consultas c ON pg.IdConsulta = c.IdConsulta
        INNER JOIN Pacientes p ON c.IdPaciente = p.IdPaciente
        WHERE c.IdDoctor = @IdDoctor
        ORDER BY pg.FechaPago DESC";

            using var connection = new SqlConnection(connectionString);
            return (await connection.QueryAsync<PagosDelDoctorViewModel>(sql, new { IdDoctor = idDoctor })).ToList();
        }


        // ✅ Obtener un pago por ID
        public async Task<Pago> ObtenerPorId(int idPago)
            {
                using var connection = new SqlConnection(connectionString);
                return await connection.QueryFirstOrDefaultAsync<Pago>(
                    "SELECT * FROM Pagos WHERE IdPago = @IdPago", new { IdPago = idPago });
            }

            // ✅ Obtener pagos por IdConsulta
            public async Task<IEnumerable<Pago>> ObtenerPorIdConsulta(int idConsulta)
            {
                using var connection = new SqlConnection(connectionString);
                return await connection.QueryAsync<Pago>(
                    "SELECT * FROM Pagos WHERE IdConsulta = @IdConsulta", new { IdConsulta = idConsulta });
            }

            // ✅ Crear un pago y retornar el IdPago generado
            public async Task<int> Crear(Pago pago)
            {
                using var connection = new SqlConnection(connectionString);

                var sql = @"
                INSERT INTO Pagos (IdConsulta, IdPaciente, Monto, MetodoPago, FechaPago)
                VALUES (@IdConsulta, @IdPaciente, @Monto, @MetodoPago, @FechaPago);

                SELECT SCOPE_IDENTITY();";

                var id = await connection.ExecuteScalarAsync<int>(sql, pago);
                return id;
            }

            // ✅ Actualizar un pago
            public async Task Actualizar(Pago pago)
            {
                using var connection = new SqlConnection(connectionString);

                var sql = @"
                UPDATE Pagos 
                SET IdConsulta = @IdConsulta,
                    IdPaciente = @IdPaciente,
                    Monto = @Monto,
                    MetodoPago = @MetodoPago,
                    FechaPago = @FechaPago
                WHERE IdPago = @IdPago";

                await connection.ExecuteAsync(sql, pago);
            }

            // ✅ Eliminar un pago por ID
            public async Task Eliminar(int idPago)
            {
                using var connection = new SqlConnection(connectionString);

                await connection.ExecuteAsync(
                    "DELETE FROM Pagos WHERE IdPago = @IdPago",
                    new { IdPago = idPago }
                );
            }
        }
    }

