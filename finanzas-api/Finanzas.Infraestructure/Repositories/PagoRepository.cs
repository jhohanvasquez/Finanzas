using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using Finanzas.Infraestructure.Repositories.Configuration;
using Microsoft.Extensions.Options;
using Finanzas.Infraestructure.Interfaces;

namespace Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly ConnectionStrings _settings;
        public PagoRepository(IOptions<ConnectionStrings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<int> RegistrarPago(Pago pago)
        {
            var connectionString = _settings.DefaultConnection;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            }

            // Fix for CS8370: Replace "using var" with explicit "using" block  
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                // Fix for CS0104: Specify the fully qualified name for SqlConnection  
                return await connection.ExecuteAsync("sp_RegistrarPago",
                    new { pago.DeudaId, pago.MontoPago, pago.MetodoPago },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
