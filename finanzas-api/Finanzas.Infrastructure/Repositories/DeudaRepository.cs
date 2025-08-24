using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Models;
using System;

namespace Repositories
{
    public class DeudaRepository
    {
        private readonly IConfiguration _config;
        public DeudaRepository(IConfiguration config) { _config = config; }

        public async Task<int> RegistrarDeuda(Deuda deuda)
        {
            if (deuda.MontoTotal <= 0)
                throw new System.Exception("No se pueden registrar deudas con valores negativos.");

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return await connection.ExecuteAsync("sp_RegistrarDeuda",
                new { deuda.UsuarioId, deuda.MontoTotal, deuda.Descripcion },
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<dynamic>> ConsultarDeudas(int? usuarioId = null)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            }
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync("sp_ConsultarDeudas",
                new { PersonaId = usuarioId },
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
