using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BROS_ECommerce.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Senha",
                value: "rQZK5vNzZGE9K5vNzZGE9K5vNzZGE9K5vNzZGE9K5vNzZGE=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Senha",
                value: "sRZL6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF0L6wOaZHF=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Senha",
                value: "tSZM7xPbZIG1M7xPbZIG1M7xPbZIG1M7xPbZIG1M7xPbZIG=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Senha",
                value: "uTZN8yQcZJH2N8yQcZJH2N8yQcZJH2N8yQcZJH2N8yQcZJH=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Senha",
                value: "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Senha",
                value: "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Senha",
                value: "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Senha",
                value: "AQAAAAEAACcQAAAAEJ8D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D8Q9D");
        }
    }
}
