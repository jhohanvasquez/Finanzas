using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Repositories;
using Models;

namespace Finanzas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repo;
        public UsuarioController(UsuarioRepository repo) => _repo = repo;

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
        {
            using var sha256 = SHA256.Create();
            usuario.PasswordHash = System.Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(usuario.PasswordHash)));
            var result = await _repo.RegistrarUsuario(usuario);
            return Ok(new { message = "Usuario registrado", rows = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            // Obtener el usuario por email
            var usuarioDb = await _repo.ObtenerPorEmailAsync(usuario.Email);
            if (usuarioDb == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            // Hashear la contraseña recibida para comparar
            using var sha256 = SHA256.Create();
            var passwordHash = System.Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(usuario.PasswordHash)));

            if (usuarioDb.PasswordHash != passwordHash)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            // Puedes devolver datos del usuario o un token si implementas JWT
            return Ok(new { message = "Login exitoso", usuario = usuarioDb });
        }

    }
}
