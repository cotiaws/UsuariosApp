using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsuariosApp.Domain.Interfaces.Security
{
    /// <summary>
    /// Interface para operações de autenticação JWT
    /// </summary>
    public interface IJwtBearerSecurity
    {
        /// <summary>
        /// Método para gerar um TOKEN JWT
        /// </summary>
        string GenerateToken(string user, string role);
    }
}
