using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Models;
using System;

namespace Repositories
{
    public class PagoRepository
    {
        private readonly IConfiguration _config;
        public PagoRepository(IConfiguration config) { _config = config; }

        public async Task<int> RegistrarPago(Pago pago)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            }
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteAsync("sp_RegistrarPago",
                new { pago.DeudaId, pago.MontoPago, pago.MetodoPago },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
