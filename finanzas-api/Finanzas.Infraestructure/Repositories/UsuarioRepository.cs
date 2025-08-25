using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using Finanzas.Infraestructure.Repositories.Configuration;
using Microsoft.Extensions.Options;
using Finanzas.Infraestructure.Interfaces;
using Dapper;

namespace Repositories
{
    public class UsuarioRepository : IUsuarioRepository
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
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                var parametros = new DynamicParameters();
                parametros.Add("@Nombre", usuario.Nombre);
                parametros.Add("@Email", usuario.Email);
                parametros.Add("@PasswordHash", usuario.PasswordHash);

                await connection.OpenAsync();

                return await connection.ExecuteAsync(
                    "sp_RegistrarUsuario",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }
        }
    }
}
