using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsuariosApp.API.Components
{
    public class JwtBearerComponent (JwtSettings jwtSettings)
    {
        /// <summary>
        /// Método para retornar a data e hora de expiração do token
        /// </summary>
        public DateTime GetExpiration() => DateTime.Now.AddMinutes(jwtSettings.Expiration);

        /// <summary>
        /// Método para retornar um TOKEN JWT para um usuário autenticado
        /// </summary>
        public string CreateToken(string user, string role)
        {
            if (string.IsNullOrEmpty(jwtSettings.SecretKey))
                throw new Exception("Falha ao gerar o token.");

            //gerando a chave de assinatura criptografada para o token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //gerando os dados do usuário
            var claims = new[] {
                new Claim(ClaimTypes.Name, user), //nome do usuário autenticado
                new Claim(ClaimTypes.Role, role), //perfil do usuário autenticado
            };

            //construindo o token JWT
            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer, //Emissor do token
                audience: jwtSettings.Audience, //Destinatário do token
                claims: claims, //Dados do usuário
                expires: GetExpiration(), //Data de expiração do token
                signingCredentials: credentials //assinatura do token
                );

            //produzir e retornar o token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    /// <summary>
    /// Classe para capturar os parametros do /appsettings
    /// </summary>
    public class JwtSettings
    {
        public string? SecretKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int Expiration { get; set; }
    }
}
