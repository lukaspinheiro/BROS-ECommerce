using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateImagemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.CreateTable(
                name: "Imagens",
                columns: table => new
                {
                    IdImagem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeArquivo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    TamanhoArquivo = table.Column<long>(type: "bigint", nullable: false),
                    TipoMime = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AltText = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagens", x => x.IdImagem);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoImagens",
                columns: table => new
                {
                    IdProdutoImagem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdImagem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Principal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataAssociacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoImagens", x => x.IdProdutoImagem);
                    table.ForeignKey(
                        name: "FK_ProdutoImagens_Imagem",
                        column: x => x.IdImagem,
                        principalTable: "Imagens",
                        principalColumn: "IdImagem",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoImagens_Produto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Imagens_Ativo",
                table: "Imagens",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Imagens_DataCriacao",
                table: "Imagens",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_Imagens_NomeArquivo",
                table: "Imagens",
                column: "NomeArquivo");

            migrationBuilder.CreateIndex(
                name: "IX_Imagens_TipoMime",
                table: "Imagens",
                column: "TipoMime");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagens_IdImagem",
                table: "ProdutoImagens",
                column: "IdImagem");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagens_IdProduto",
                table: "ProdutoImagens",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagens_IdProduto_Ordem",
                table: "ProdutoImagens",
                columns: new[] { "IdProduto", "Ordem" });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagens_UnicaPrincipal",
                table: "ProdutoImagens",
                columns: new[] { "IdProduto", "Principal" },
                filter: "Principal = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoImagens");

            migrationBuilder.DropTable(
                name: "Imagens");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "IdUser", "Ativo", "Cpf", "DataAtualizacao", "DataCriacao", "Email", "Genero", "Nascimento", "Nome", "Senha" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, "45678912300", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "maria.oliveira@hotmail.com", "Feminino", new DateTime(1998, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria Oliveira Costa", "tSZM7xPbZIG1M7xPbZIG1M7xPbZIG1M7xPbZIG1M7xPbZIG=" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, "78912345600", null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "alex.santos@outlook.com", "Outro", new DateTime(2000, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alex Santos Lima", "uTZN8yQcZJH2N8yQcZJH2N8yQcZJH2N8yQcZJH2N8yQcZJH=" }
                });
        }
    }
}
