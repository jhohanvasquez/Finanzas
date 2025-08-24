using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Services;
using Repositories;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeudaController : ControllerBase
    {
        private readonly DeudaRepository _repo;
        private readonly CacheService _cache;
        public DeudaController(DeudaRepository repo, CacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Deuda deuda)
        {
            var result = await _repo.RegistrarDeuda(deuda);
            await _cache.RemoveAsync($"deudas_usuario_{deuda.UsuarioId}");
            return Ok(new { message = "Deuda registrada", rows = result });
        }

        [HttpGet("consultar/{usuarioId}")]
        public async Task<IActionResult> Consultar(int usuarioId)
        {
            var cacheKey = $"deudas_usuario_{usuarioId}";
            var cached = await _cache.GetAsync<object>(cacheKey);
            if (cached != null) return Ok(cached);

            var result = await _repo.ConsultarDeudas(usuarioId);
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
            return Ok(result);
        }
    }
}
