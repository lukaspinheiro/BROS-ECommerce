using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateCategoriaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criar tabela Categorias
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeCategoria = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                });

            // Criar índices para Categorias
            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Ativo",
                table: "Categorias",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_DataCriacao",
                table: "Categorias",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_NomeCategoria",
                table: "Categorias",
                column: "NomeCategoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Categorias");
        }
    }
}