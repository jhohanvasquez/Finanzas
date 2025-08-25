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
        public Task<int> RegistrarUsuario(Usuario usuario)
        {
            return _usuarioRepository.RegistrarUsuario(usuario);
        }

        public Task<Usuario> LoginAsync(string email, string password)
        {
            return _usuarioRepository.LoginAsync(email, password);
        }

        public Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            return _usuarioRepository.ObtenerPorEmailAsync(email);
        }
    }
}
