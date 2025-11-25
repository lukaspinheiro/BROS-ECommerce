using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarColunaTenantIdEmTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Promocoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "ProdutoImagens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PedidoItens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Imagens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Estoque",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "CategoriaProdutos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Carrinhos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "CarrinhoItens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TenantId",
                value: "bros");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "TenantId",
                value: "gamma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Promocoes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProdutoImagens");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PedidoItens");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Imagens");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Estoque");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CategoriaProdutos");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Carrinhos");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CarrinhoItens");
        }
    }
}
