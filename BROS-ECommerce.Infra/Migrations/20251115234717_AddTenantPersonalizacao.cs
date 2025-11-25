using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantPersonalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CorFundoPrimaria",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorFundoSecundaria",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorTextoPrimaria",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorTextoSecundaria",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Tenants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "IdFavicon",
                table: "Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdLogo",
                table: "Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TenantId",
                value: "admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CorFundoPrimaria",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CorFundoSecundaria",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CorTextoPrimaria",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CorTextoSecundaria",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IdFavicon",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IdLogo",
                table: "Tenants");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "TenantId",
                value: "bros");
        }
    }
}
