using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace UsuariosApp.API.Components
{
    public class RabbitMQComponent(RabbitMQSettings rabbitMQSettings, ILogger<RabbitMQComponent> logger)
    {
        //Método para enviar os dados para a fila do RabbitMQ
        public async Task Publish(UsuarioCriadoEvent usuario)
        {
            logger.LogInformation($"Iniciando o envio para a mensageria dos dados de {usuario.NomeUsuario} com email {usuario.EmailUsuario}");

            //criando o objeto para conexão
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.Host,
                Port = rabbitMQSettings.Port,
                UserName = rabbitMQSettings.UserName,
                Password = rabbitMQSettings.Password,
                VirtualHost = rabbitMQSettings.VirtualHost
            };

            try
            {
                //conectando no servidor
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                //criando / acessando a fila
                await channel.QueueDeclareAsync(
                    queue: rabbitMQSettings.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(usuario));

                //gravando a mensagem na fila
                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: rabbitMQSettings.QueueName,
                    body: body
                    );

                logger.LogInformation($"Mensagem enviada com sucesso para a fila do RabbitMQ. Nome: {usuario.NomeUsuario}, email: {usuario.EmailUsuario}");
            }
            catch(Exception e)
            {
                logger.LogError(e, $"Erro ao enviar mensagem para o RabbitMQ do usuário {usuario.NomeUsuario} com email {usuario.EmailUsuario}");
            }
        }
    }

    public record UsuarioCriadoEvent (
        string? NomeUsuario,
        string? EmailUsuario,
        DateTime? DataHoraCriacao
    );

    public class RabbitMQSettings
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
        public string? QueueName { get; set; }
    }
}
