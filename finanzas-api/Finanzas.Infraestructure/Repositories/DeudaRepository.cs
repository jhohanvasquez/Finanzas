using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using Finanzas.Infraestructure.Repositories.Configuration;
using Microsoft.Extensions.Options;

namespace Repositories
{
    public class DeudaRepository
    {
        private readonly ConnectionStrings _settings;
        public DeudaRepository(IOptions<ConnectionStrings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<int> RegistrarDeuda(Deuda deuda)
        {
            if (deuda == null)
                throw new ArgumentNullException(nameof(deuda), "La deuda no puede ser nula.");
            if (deuda.MontoTotal <= 0)
                throw new System.Exception("No se pueden registrar deudas con valores negativos.");

            using (var connection = new System.Data.SqlClient.SqlConnection(_settings.DefaultConnection))
            {
                return await connection.ExecuteAsync("sp_RegistrarDeuda",
                    new { deuda.UsuarioId, deuda.MontoTotal, deuda.Descripcion },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<dynamic>> ConsultarDeudas(int? usuarioId = null)
        {
            if (usuarioId == null)
                throw new ArgumentNullException(nameof(usuarioId), "El ID del usuario no puede ser nulo.");
            var connectionString = _settings.DefaultConnection;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            }
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                return await connection.QueryAsync("sp_ConsultarDeudas",
                    new { PersonaId = usuarioId },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
