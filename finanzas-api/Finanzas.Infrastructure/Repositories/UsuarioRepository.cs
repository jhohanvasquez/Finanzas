using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Models;
using System;

namespace Repositories
{
    public class UsuarioRepository
    {
        private readonly IConfiguration _config;

        public UsuarioRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> RegistrarUsuario(Usuario usuario)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            }
            using var connection = new SqlConnection(connectionString);
            var parametros = new
            {
                usuario.Nombre,
                usuario.Email,
                usuario.PasswordHash
            };
            return await connection.ExecuteAsync("sp_RegistrarUsuario", parametros, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
