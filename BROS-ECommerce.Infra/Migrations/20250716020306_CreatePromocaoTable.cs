using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreatePromocaoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criar tabela Promocoes
            migrationBuilder.CreateTable(
                name: "Promocoes",
                columns: table => new
                {
                    IdPromocao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    IdProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocoes", x => x.IdPromocao);
                    table.ForeignKey(
                        name: "FK_Promocoes_Produto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            // Criar índices para Promocoes
            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_Ativo",
                table: "Promocoes",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_DataFim",
                table: "Promocoes",
                column: "DataFim");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_DataInicio",
                table: "Promocoes",
                column: "DataInicio");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_IdProduto",
                table: "Promocoes",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_Promocoes_ProdutoVigencia",
                table: "Promocoes",
                columns: new[] { "IdProduto", "DataInicio", "DataFim", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Promocoes");
        }
    }
}