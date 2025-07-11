using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelasCarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "Carrinhos",
                columns: table => new
                {
                    IdCarrinho = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Aberto")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrinhos", x => x.IdCarrinho);
                    table.ForeignKey(
                        name: "FK_Carrinhos_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.NoAction);
                });

            
            migrationBuilder.CreateTable(
                name: "CarrinhoItens",
                columns: table => new
                {
                    IdCarrinhoItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCarrinho = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhoItens", x => x.IdCarrinhoItem);
                    table.ForeignKey(
                        name: "FK_CarrinhoItens_Carrinho",
                        column: x => x.IdCarrinho,
                        principalTable: "Carrinhos",
                        principalColumn: "IdCarrinho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarrinhoItens_Produto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            
            migrationBuilder.CreateIndex(
                name: "IX_Carrinhos_IdUsuario",
                table: "Carrinhos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Carrinhos_DataCriacao",
                table: "Carrinhos",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_Carrinhos_Status",
                table: "Carrinhos",
                column: "Status");

            
            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoItens_IdCarrinho",
                table: "CarrinhoItens",
                column: "IdCarrinho");

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoItens_IdProduto",
                table: "CarrinhoItens",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhoItens_CarrinhoProduto",
                table: "CarrinhoItens",
                columns: new[] { "IdCarrinho", "IdProduto" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CarrinhoItens");
            migrationBuilder.DropTable(name: "Carrinhos");
        }
    }
}