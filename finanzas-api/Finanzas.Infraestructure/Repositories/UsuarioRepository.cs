using System.Data.SqlClient; // Remove this line to avoid ambiguity  
using Microsoft.Data.SqlClient; // Keep this line for SqlConnection usage  
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using Finanzas.Infraestructure.Repositories.Configuration;
using Microsoft.Extensions.Options;

namespace Repositories
{
    public class UsuarioRepository
    {
        private readonly ConnectionStrings _settings;
        public UsuarioRepository(IOptions<ConnectionStrings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }


        public async Task<int> RegistrarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            var connectionString = _settings.DefaultConnection;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            }
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
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
}
