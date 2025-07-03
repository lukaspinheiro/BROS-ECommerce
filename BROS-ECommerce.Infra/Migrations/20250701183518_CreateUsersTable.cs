using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nascimento = table.Column<DateTime>(type: "date", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "IdUser", "Ativo", "Cpf", "DataAtualizacao", "DataCriacao", "Email", "Genero", "Nascimento", "Nome", "Senha" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, "12345678901", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@bros.com", "Masculino", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrador Sistema", "rQZK5vNzZGE9K5vNzZGE9K5vNzZGE9K5vNzZGE9K5vNzZGE=" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, "98765432100", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "joao.silva@gmail.com", "Masculino", new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "João Silva Santos", "sRZL6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF=" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, "45678912300", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "maria.oliveira@hotmail.com", "Feminino", new DateTime(1998, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria Oliveira Costa", "tSZM7xPbZIG1M7xPbZIG1M7xPbZIG1M7xPbZIG1M7xPbZIG=" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, "78912345600", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "alex.santos@outlook.com", "Outro", new DateTime(2000, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alex Santos Lima", "uTZN8yQcZJH2N8yQcZJH2N8yQcZJH2N8yQcZJH2N8yQcZJH=" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Ativo",
                table: "Users",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Cpf",
                table: "Users",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DataCriacao",
                table: "Users",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}