using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuariosApp.Domain.Entities;

namespace UsuariosApp.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Método para salvar um novo usuário no banco de dados.
        /// </summary>
        void Add(Usuario usuario);

        /// <summary>
        /// Método para verificar se existe um email cadastrado no banco.
        /// </summary>
        bool VerifyEmailExists(string email);

        /// <summary>
        /// Método para retornar 1 usuário através do email e senha.
        /// </summary>
        Usuario? GetByEmailAndSenha(string email, string senha);

        /// <summary>
        /// Método para retornar 1 usuário através do email
        /// </summary>
        Usuario? GetByEmail(string email);
    }
}
