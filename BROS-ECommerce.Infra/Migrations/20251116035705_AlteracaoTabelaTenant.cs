using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoTabelaTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorFundoSecundaria",
                table: "Tenants",
                newName: "CorTextoMenuInferior");

            migrationBuilder.RenameColumn(
                name: "CorFundoPrimaria",
                table: "Tenants",
                newName: "CorMenuInferior");

            migrationBuilder.AddColumn<string>(
                name: "CorFundo",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorMenu",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorFundo",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CorMenu",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "CorTextoMenuInferior",
                table: "Tenants",
                newName: "CorFundoSecundaria");

            migrationBuilder.RenameColumn(
                name: "CorMenuInferior",
                table: "Tenants",
                newName: "CorFundoPrimaria");
        }
    }
}
