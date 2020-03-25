using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuration",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    LastProcessedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuration", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuration",
                schema: "campaign");
        }
    }
}
