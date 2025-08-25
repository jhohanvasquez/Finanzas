using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.ServiceCore.Interfaces
{
    public interface IUsuarioService
    {
        Task<int> RegistrarUsuario(Usuario usuario);
        Task<Usuario> LoginAsync(string email, string password);
        Task<Usuario> ObtenerPorEmailAsync(string email);

    }
}
