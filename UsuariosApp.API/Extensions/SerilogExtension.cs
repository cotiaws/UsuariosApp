using Serilog;
using Serilog.Events;

namespace UsuariosApp.API.Extensions
{
    public static class SerilogExtension
    {
        public static ConfigureHostBuilder AddSerilogConfig(this ConfigureHostBuilder host)
        {
            //Criando uma configuração para o Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //Define que os logs da categoria 'Microsoft' (Ex: Asp.NET) só serão registrados a partir de Warning
                //Isso evita poluir os logs com informações de nivel baixo como 'Info' ou 'Debug'
                //Dessa forma o Serilog irá gravar apenas logs de 'Warning' ou 'Error' do .NET
                .Enrich.FromLogContext()
                //Adiciona automaticamente dados do contexto do log, como informações de requisição HTTP
                //útil para rastreamento de logs de aplicações web pegando dados da requisição e da resposta
                .WriteTo.Console()
                //Logs serão escritos em modo console
                .WriteTo.Seq("http://seq:80")
                //Envia os logs gerados para o servidor Seq que está rodando no docker
                .CreateLogger();

            host.UseSerilog(); //ativando o Serilog

            return host;
        }
    }
}
