namespace UsuariosApp.API.Models
{
    public class Usuario
    {
        public Guid Id { get; set; } = new Guid();
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public Perfil Perfil { get; set; } = Perfil.Usuario;
    }

    public enum Perfil
    {
        Administrador = 1,
        Usuario = 2
    }
}
