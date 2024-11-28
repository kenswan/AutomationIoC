#pragma warning disable IDE0005, IDE0022, IDE0073, IDE0161
// IDE0005 : Using directive is unnecessary.
// IDE0022 : Use expression body for method.
// IDE0073 : A source file is missing a required header.
// IDE0161 : Convert to file-scoped namespace.

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePowerShellSample.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}
#pragma warning restore IDE0005, IDE0022, IDE0073, IDE0161
