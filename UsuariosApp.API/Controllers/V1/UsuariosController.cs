using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.API.Components;
using UsuariosApp.API.Entities;
using UsuariosApp.API.Repositories;

namespace UsuariosApp.API.Controllers.V1
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/[controller]")]    
    public class UsuariosController (UsuarioRepository usuarioRepository, JwtBearerComponent jwtBearerComponent, RabbitMQComponent rabbitMQComponent) : ControllerBase
    {
        [HttpPost("autenticar")] //api/usuarios/autenticar
        public async Task<IActionResult> Autenticar([FromBody] AutenticarUsuarioRequest request)
        {
            try
            {
                //Consultando o usuário através do email e da senha
                var usuario = await usuarioRepository.Obter(request.Email, request.Senha);

                //Condição de segurança: Verificar se o usuário não foi encontrado
                if (usuario == null)
                    return Unauthorized("Usuário inválido. Acesso não autorizado!");

                //Retornar sucesso e tambem os dados do usuário
                return StatusCode(200, new
                {
                    Mensagem = "Usuário autenticado com sucesso.", //Mensagem de sucesso
                    Usuario = new //Informações do usuário
                    { 
                        usuario.Id, //Id do usuário
                        usuario.Nome, //Nome do usuário
                        usuario.Email, //Email do usuário
                        Perfil = new  //Informações do perfil
                        {
                            usuario.Perfil?.Id, //Id do Perfil
                            usuario.Perfil?.Nome //Nome do perfil
                        }
                    },
                    Token = jwtBearerComponent.CreateToken(usuario.Email, usuario.Perfil?.Nome), //token JWT
                    DataExpiracao = jwtBearerComponent.GetExpiration() //data e hora de expiração
                });
            }
            catch(Exception e)
            {
                return StatusCode(500, new { e.Message });
            }
        }

        [HttpPost("criar")] //api/usuarios/criar
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
        {
            try
            {
                //Criar um usuário
                var usuario = new Usuario
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    Senha = request.Senha,
                };

                //Enviando para gravação no banco de dados
                var id = await usuarioRepository.Inserir(usuario);

                //Criando um evento 'Usuario Criado!'
                var usuarioCriado = new UsuarioCriadoEvent(usuario.Nome, usuario.Email, DateTime.Now);
                
                //Enviar os dados para a mensageria
                await rabbitMQComponent.Publish(usuarioCriado);

                //retonando status de sucesso
                return StatusCode(201, new {
                    Mensagem = "Usuário criado com sucesso.", //mensagem
                    Usuario = new //dados do usuário
                    {
                        Id = id, //id do usuário
                        usuario.Nome, //nome do usuário
                        usuario.Email, //email do usuário
                    }
                });
            }
            catch(ApplicationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }

    #region Record para definir os dados da requisição

    public record AutenticarUsuarioRequest(
            string Email, //email do usuário
            string Senha //senha do usuário
        );

    public record CriarUsuarioRequest(
            string Nome,    //nome do usuário
            string Email,   //email do usuário
            string Senha,   //senha do usuário
            string SenhaConfirmacao //confirmação da senha
        );

    #endregion
}
