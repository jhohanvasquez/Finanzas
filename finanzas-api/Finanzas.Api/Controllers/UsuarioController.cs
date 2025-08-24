using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Repositories;
using Models;

namespace Controllers
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
    }
}
