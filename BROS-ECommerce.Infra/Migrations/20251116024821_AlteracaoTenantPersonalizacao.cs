using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoTenantPersonalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorTextoSecundaria",
                table: "Tenants",
                newName: "CorTextoMenu");

            migrationBuilder.RenameColumn(
                name: "CorTextoPrimaria",
                table: "Tenants",
                newName: "CorTexto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorTextoMenu",
                table: "Tenants",
                newName: "CorTextoSecundaria");

            migrationBuilder.RenameColumn(
                name: "CorTexto",
                table: "Tenants",
                newName: "CorTextoPrimaria");
        }
    }
}
