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

        public async Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            var connectionString = _settings.DefaultConnection;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<Usuario>(
                    "SELECT UsuarioId, Nombre, Email, PasswordHash FROM Usuario WHERE Email = @Email",
                    new { Email = email }
                );
            }
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

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            var connectionString = _settings.DefaultConnection;
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada.");

            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Buscar el usuario por email
                var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(
                    "SELECT UsuarioId, Nombre, Email, PasswordHash FROM Usuario WHERE Email = @Email",
                    new { Email = email }
                );

                if (usuario == null)
                    return null;

                // Hashear la contraseña recibida para comparar
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var passwordHash = Convert.ToBase64String(
                        sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
                    );

                    if (usuario.PasswordHash != passwordHash)
                        return null;
                }

                return usuario;
            }
        }

    }
}
