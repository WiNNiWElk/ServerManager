using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OsName = table.Column<string>(type: "text", nullable: false),
                    RamGb = table.Column<int>(type: "integer", nullable: false),
                    StorageGb = table.Column<int>(type: "integer", nullable: false),
                    CpuCores = table.Column<int>(type: "integer", nullable: false),
                    RentedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ServerStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
