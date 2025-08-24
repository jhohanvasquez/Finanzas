using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Repositories;
using Services;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly PagoRepository _repo;
        private readonly CacheService _cache;

        public PagoController(PagoRepository repo, CacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Pago pago)
        {
            var result = await _repo.RegistrarPago(pago);
            await _cache.RemoveAsync($"deudas_usuario_{pago.DeudaId}");
            return Ok(new { message = "Pago registrado", rows = result });
        }
    }
}
