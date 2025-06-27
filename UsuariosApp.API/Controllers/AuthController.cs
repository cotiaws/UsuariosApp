using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public IActionResult Post([FromBody] AuthRequest request)
        {
            return Ok();
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
