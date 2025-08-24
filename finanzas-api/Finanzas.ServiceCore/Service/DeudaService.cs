using Finanzas.ServiceCore.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Service
{
    public class DeudaService : IDeudaService
    {
        private readonly IDeudaRepository _deudaRepository;

        public DeudaService(IDeudaRepository deudaRepository)
        {
            this._deudaRepository = deudaRepository;
        }

        public Task<IEnumerable<dynamic>> ConsultarDeudas(int? usuarioId = null)
        {
            return _deudaRepository.ConsultarDeudas(usuarioId);
        }

    }
}
