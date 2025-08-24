using Finanzas.ServiceCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Service
{
    public class PagoService
    {
        private readonly IDeudaRepository _deudaRepository;

        public PagoService(IDeudaRepository deudaRepository)
        {
            this._deudaRepository = deudaRepository;
        }
    }
}
