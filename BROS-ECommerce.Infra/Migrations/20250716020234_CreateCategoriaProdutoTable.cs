using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateCategoriaProdutoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criar tabela CategoriaProdutos
            migrationBuilder.CreateTable(
                name: "CategoriaProdutos",
                columns: table => new
                {
                    IdCategoriaProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCategoria = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAssociacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProdutos", x => x.IdCategoriaProduto);
                    table.ForeignKey(
                        name: "FK_CategoriaProdutos_Categoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaProdutos_Produto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            // Criar índices para CategoriaProdutos
            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProdutos_DataAssociacao",
                table: "CategoriaProdutos",
                column: "DataAssociacao");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProdutos_IdCategoria",
                table: "CategoriaProdutos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProdutos_IdProduto",
                table: "CategoriaProdutos",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProdutos_CategoriasProduto",
                table: "CategoriaProdutos",
                columns: new[] { "IdCategoria", "IdProduto" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CategoriaProdutos");
        }
    }
}