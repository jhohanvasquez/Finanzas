using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Repositories;
using Models;
using Finanzas.Api.Service;

namespace Finanzas.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de pagos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly PagoRepository _repo;
        private readonly CacheService _cache;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de pagos.
        /// </summary>
        /// <param name="repo">Repositorio de pagos.</param>
        /// <param name="cache">Servicio de caché.</param>
        public PagoController(PagoRepository repo, CacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        /// <summary>
        /// Registra un nuevo pago para una deuda.
        /// </summary>
        /// <param name="pago">Datos del pago a registrar.</param>
        /// <returns>Mensaje de confirmación y número de filas afectadas.</returns>
        /// <response code="200">Pago registrado correctamente.</response>
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Pago pago)
        {
            var result = await _repo.RegistrarPago(pago);
            await _cache.RemoveAsync($"deudas_usuario_{pago.DeudaId}");
            return Ok(new { message = "Pago registrado", rows = result });
        }
    }
}
