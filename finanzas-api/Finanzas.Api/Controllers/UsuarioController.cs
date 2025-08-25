using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Repositories;
using Models;

namespace Finanzas.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repo;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de usuarios.
        /// </summary>
        /// <param name="repo">Repositorio de usuarios.</param>
        public UsuarioController(UsuarioRepository repo) => _repo = repo;

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usuario">Datos del usuario a registrar.</param>
        /// <returns>Mensaje de confirmación y número de filas afectadas.</returns>
        /// <response code="200">Usuario registrado correctamente.</response>
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
        {
            using var sha256 = SHA256.Create();
            usuario.PasswordHash = System.Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(usuario.PasswordHash)));
            var result = await _repo.RegistrarUsuario(usuario);
            return Ok(new { message = "Usuario registrado", rows = result });
        }

        /// <summary>
        /// Inicia sesión de usuario validando email y contraseña.
        /// </summary>
        /// <param name="usuario">Datos de acceso del usuario (email y contraseña).</param>
        /// <returns>Mensaje de éxito y datos del usuario si la autenticación es correcta.</returns>
        /// <response code="200">Login exitoso.</response>
        /// <response code="401">Usuario o contraseña incorrectos.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            var usuarioDb = await _repo.ObtenerPorEmailAsync(usuario.Email);
            if (usuarioDb == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            using var sha256 = SHA256.Create();
            var passwordHash = System.Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(usuario.PasswordHash)));

            if (usuarioDb.PasswordHash != passwordHash)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            return Ok(new { message = "Login exitoso", usuario = usuarioDb });
        }
    }
}
