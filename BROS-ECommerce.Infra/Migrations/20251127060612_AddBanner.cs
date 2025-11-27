using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    IdBanner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subtitulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    IdImagem = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CaminhoImagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.IdBanner);
                    table.ForeignKey(
                        name: "FK_Banners_Imagens_IdImagem",
                        column: x => x.IdImagem,
                        principalTable: "Imagens",
                        principalColumn: "IdImagem",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_IdImagem",
                table: "Banners",
                column: "IdImagem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
