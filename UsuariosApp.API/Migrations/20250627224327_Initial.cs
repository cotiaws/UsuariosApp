using System;
using Microsoft.EntityFrameworkCore.Migrations;
using UsuariosApp.API.Helpers;

#nullable disable

namespace UsuariosApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Perfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            // Inserção dos dois usuários
            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Nome", "Email", "Senha", "Perfil" },
                values: new object[,]
                {
                        {
                            Guid.NewGuid(),
                            "Usuário Administrador",
                            "admin@coti.com.br",
                            CryptoHelper.GetSHA256("@Admin123"),
                            1
                        },
                        {
                            Guid.NewGuid(),
                            "Usuário Comum",
                            "user@coti.com.br",
                            CryptoHelper.GetSHA256("@User123"),
                            2
                        }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
