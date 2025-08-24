using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Interfaces
{
    public interface IDeudaService
    {
        Task<IEnumerable<dynamic>> ConsultarDeudas(int? usuarioId = null);
    }
}
