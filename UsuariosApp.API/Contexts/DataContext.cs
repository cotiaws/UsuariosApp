using Microsoft.EntityFrameworkCore;
using UsuariosApp.API.Models;

namespace UsuariosApp.API.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity => 
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Nome).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Senha).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Perfil).IsRequired();

                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}
