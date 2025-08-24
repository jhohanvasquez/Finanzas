using Finanzas.ServiceCore.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Service
{
    public class DeudaRepository
    {
        private readonly IDeudaRepository _deudaRepository;

        public DeudaRepository(IDeudaRepository deudaRepository)
        {
            this._deudaRepository = deudaRepository;
        }
    }
}
