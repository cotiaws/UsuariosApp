using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UsuariosApp.API.Components;
using UsuariosApp.API.Entities;

namespace UsuariosApp.API.Repositories
{
    public class UsuarioRepository (string connectionString)
    {
        public async Task<Usuario?> Obter(string email, string senha)
        {
            //Abrindo conexão com o banco de dados
            using var connection = new SqlConnection(connectionString);

            try
            {
                //Escrevendo a consulta que será executada no banco de dados
                var query = @"
                    SELECT 
                        u.ID, u.NOME, u.EMAIL, u.PERFILID,
                        p.ID, p.NOME
                    FROM USUARIO u
                    INNER JOIN PERFIL p
                    ON u.PERFILID = p.ID
                    WHERE u.EMAIL = @Email
                    AND u.Senha = @Senha
                ";

                //Executando a consulta no banco de dados usando o Dapper
                var result = await connection.QueryAsync(query, (Usuario usuario, Perfil perfil) =>
                {
                    usuario.Perfil = perfil; //Capturando o perfil e associando ao usuário
                    return usuario; //retornando o objeto usuário
                },                
                param: new { @Email = email, @Senha = CryptoComponent.Sha256Encrypt(senha) },
                splitOn: "PERFILID"); //campo que representa a chave estrangeira               

                //retornar 1 usuário ou null
                return result.FirstOrDefault();
            }
            catch(Exception)
            {
                throw new ApplicationException($"Falha ao obter usuário.");
            }
        }

        public async Task<Guid?> Inserir(Usuario usuario)
        {
            //Abrindo conexão com o banco de dados
            using var connection = new SqlConnection(connectionString);

            //Capturando o id do perfil 'Operador' no banco de dados
            usuario.PerfilId = await connection.QueryFirstOrDefaultAsync<Guid?>
                                ("SELECT ID FROM PERFIL WHERE NOME = 'Operador'");

            //Definindo os parametros que serão passados para a procedure
            var parameters = new DynamicParameters();
            parameters.Add("@NOME", usuario.Nome);
            parameters.Add("@EMAIL", usuario.Email);
            parameters.Add("@SENHA", CryptoComponent.Sha256Encrypt(usuario.Senha));
            parameters.Add("@PERFILID", usuario.PerfilId);
            parameters.Add("@USUARIOID", dbType: DbType.Guid, direction: ParameterDirection.Output);

            try
            {
                //Executando a procedure no banco de dados passando os parametros
                await connection.ExecuteAsync("SP_CRIAR_USUARIO", parameters, commandType: CommandType.StoredProcedure);
                //Retornat o id do usuário gerado no banco de dados
                return parameters.Get<Guid>("@USUARIOID");
            }
            catch(SqlException e)
            {
                throw new ApplicationException($"Falha ao criar usuário: {e.Message}");
            }
        }
    }
}
