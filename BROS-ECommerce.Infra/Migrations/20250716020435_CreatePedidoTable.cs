using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreatePedidoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criar tabela Pedidos
            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroPedido = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValue: "Pendente"),
                    ValorSubtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorFrete = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ValorDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ObservacoesPedido = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    ObservacoesInternas = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    DataCancelamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoCancelamento = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            // Criar tabela PedidoItens
            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    IdPedidoItem = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProduto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorDesconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => x.IdPedidoItem);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Pedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Produto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            // Criar índices para Pedidos
            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DataCriacao",
                table: "Pedidos",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DataPedido",
                table: "Pedidos",
                column: "DataPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdUsuario",
                table: "Pedidos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_NumeroPedido",
                table: "Pedidos",
                column: "NumeroPedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Status",
                table: "Pedidos",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioData",
                table: "Pedidos",
                columns: new[] { "IdUsuario", "DataPedido" });

            // Criar índices para PedidoItens
            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_IdPedido",
                table: "PedidoItens",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_IdProduto",
                table: "PedidoItens",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidoProduto",
                table: "PedidoItens",
                columns: new[] { "IdPedido", "IdProduto" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PedidoItens");
            migrationBuilder.DropTable(name: "Pedidos");
        }
    }
}