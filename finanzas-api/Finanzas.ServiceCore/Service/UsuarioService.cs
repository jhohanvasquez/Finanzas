using Finanzas.ServiceCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Service
{
    public class UsuarioService
    {
        private readonly IDeudaRepository _deudaRepository;

        public UsuarioService(IDeudaRepository deudaRepository)
        {
            this._deudaRepository = deudaRepository;
        }
    }
}
