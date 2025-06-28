using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.API.Contexts;
using UsuariosApp.API.Helpers;
using UsuariosApp.API.Models;

namespace UsuariosApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController (DataContext context, JwtTokenHelper jwtHelper) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public IActionResult Post([FromBody] AuthRequest request)
        {
            var usuario = context
                            .Set<Usuario>()
                            .SingleOrDefault(u => u.Email.Equals(request.Email) 
                                               && u.Senha.Equals(CryptoHelper.GetSHA256(request.Senha)));

            if (usuario == null)
                return Unauthorized("Acesso não autorizado.");

            var response = new AuthResponse(
                Id: usuario.Id,
                Nome: usuario.Nome,
                Email: usuario.Email,
                DataHoraAcesso: DateTime.UtcNow,
                DataHoraExpiracao: DateTime.UtcNow.AddHours(1),
                AccessToken: jwtHelper.GenerateToken(usuario.Email)
            );

            return Ok(response);
        }
    }

    #region Records

    public record AuthRequest(
        string Email,
        string Senha
        );

    public record AuthResponse(
        Guid Id,
        string Nome,
        string Email,
        DateTime DataHoraAcesso,
        DateTime DataHoraExpiracao,
        string AccessToken
        );

    #endregion
}
