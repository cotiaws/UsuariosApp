using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace UsuariosApp.Tests
{
    public class UsuariosTest
    {
        private readonly HttpClient _httpClient;
        private readonly Faker _faker;

        private const string _version = "v1";

        private const string _apiCriarUsuario = $"/api/{_version}/usuarios/criar";
        private const string _apiAutenticarUsuario = $"/api/{_version}/usuarios/autenticar";

        public UsuariosTest()
        {
            _httpClient = new WebApplicationFactory<Program>().CreateClient();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve criar usuário com sucesso quando os dados forem válidos")]
        public async Task DeveCriarUsuario_QuandoDadosValidos()
        {
            //Arrange
            var request = new
            {
                Nome = _faker.Name.FullName(),
                Email = _faker.Internet.Email(),
                Senha = "@Teste2025",
                SenhaConfirmacao = "@Teste2025"
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(_apiCriarUsuario, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact(DisplayName = "Deve retornar erro de requisição inválida quando email já existe")]
        public async Task DeveRetornarErro_QuandoEmailJaExiste()
        {
            //Arrange
            var request = new
            {
                Nome = _faker.Name.FullName(),
                Email = _faker.Internet.Email(),
                Senha = "@Teste2025",
                SenhaConfirmacao = "@Teste2025"
            };

            //Act
            await _httpClient.PostAsJsonAsync(_apiCriarUsuario, request);

            //Act
            var response = await _httpClient.PostAsJsonAsync(_apiCriarUsuario, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Deve autenticar usuário com sucesso quando as credenciais forem válidas")]
        public async Task DeveAutenticarUsuario_QuandoCredenciaisValidas()
        {
            //Arrange
            var requestCriarUsuario = new
            {
                Nome = _faker.Name.FullName(),
                Email = _faker.Internet.Email(),
                Senha = "@Teste2025",
                SenhaConfirmacao = "@Teste2025"
            };

            //Arrange
            var requestAutenticarUsuario = new
            {
                Email = requestCriarUsuario.Email,
                Senha = requestCriarUsuario.Senha
            };

            //Act
            await _httpClient.PostAsJsonAsync(_apiCriarUsuario, requestCriarUsuario);

            //Act
            var response = await _httpClient.PostAsJsonAsync(_apiAutenticarUsuario, requestAutenticarUsuario);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Deve retornar erro de acesso negado quando as credenciais forem inválidas")]
        public async Task DeveRetornarAcessoNegado_QuandoCredenciaisInvalidas()
        {
            //Arrange
            var request = new
            {
                Email = _faker.Internet.Email(),
                Senha = "@NovaSenha123"
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync(_apiAutenticarUsuario, request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
