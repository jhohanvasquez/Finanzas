using Finanzas.Infraestructure.Interfaces;
using Finanzas.ServiceCore.Interfaces;
using Models;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Service
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;

        public PagoService(IPagoRepository pagoRepository)
        {
            this._pagoRepository = pagoRepository;
        }

        public Task<int> RegistrarPago(Pago pago)
        {
            var pagoInfraestructura = new Pago
            {
                PagoId = pago.PagoId,
                DeudaId = pago.DeudaId,
                MontoPago = pago.MontoPago,
                MetodoPago = pago.MetodoPago
            };

            return _pagoRepository.RegistrarPago(pagoInfraestructura);
        }
    }
}
