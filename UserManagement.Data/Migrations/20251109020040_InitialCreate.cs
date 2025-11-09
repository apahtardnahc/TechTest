using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Forename = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Forename", "IsActive", "Surname" },
                values: new object[,]
                {
                    { 1L, new DateTime(1999, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ploew@example.com", "Peter", true, "Loew" },
                    { 2L, new DateTime(1998, 11, 2, 0, 0, 0, 0, DateTimeKind.Utc), "bfgates@example.com", "Benjamin Franklin", true, "Gates" },
                    { 3L, new DateTime(1997, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "ctroy@example.com", "Castor", false, "Troy" },
                    { 4L, new DateTime(1996, 11, 4, 0, 0, 0, 0, DateTimeKind.Utc), "mraines@example.com", "Memphis", true, "Raines" },
                    { 5L, new DateTime(1995, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "sgodspeed@example.com", "Stanley", true, "Goodspeed" },
                    { 6L, new DateTime(1994, 11, 6, 0, 0, 0, 0, DateTimeKind.Utc), "himcdunnough@example.com", "H.I.", true, "McDunnough" },
                    { 7L, new DateTime(1993, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), "cpoe@example.com", "Cameron", false, "Poe" },
                    { 8L, new DateTime(1992, 11, 8, 0, 0, 0, 0, DateTimeKind.Utc), "emalus@example.com", "Edward", false, "Malus" },
                    { 9L, new DateTime(1991, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), "dmacready@example.com", "Damon", false, "Macready" },
                    { 10L, new DateTime(1990, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "jblaze@example.com", "Johnny", true, "Blaze" },
                    { 11L, null, "rfeld@example.com", "Robin", true, "Feld" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
