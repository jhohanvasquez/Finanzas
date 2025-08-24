using Finanzas.ServiceCore.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Service
{
    public class UsuarioService
    {
        private readonly IUsuarioService _usuarioRepository;

        public UsuarioService(IUsuarioService usuarioRepository)
        {
            this._usuarioRepository = usuarioRepository;
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
            return _usuarioRepository.RegistrarPago(pagoInfraestructura);
        }
    }
}
