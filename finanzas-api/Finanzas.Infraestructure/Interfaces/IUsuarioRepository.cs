using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzas.Infraestructure.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<int> RegistrarUsuario(Usuario usuario);
        Task<Usuario> ObtenerPorEmailAsync(string email);
        Task<Usuario> LoginAsync(string email, string password);

    }
}
