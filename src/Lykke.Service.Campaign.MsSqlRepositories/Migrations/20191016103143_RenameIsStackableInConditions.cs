using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class RenameIsStackableInConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_stakeable",
                schema: "campaign",
                table: "condition",
                newName: "has_staking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "has_staking",
                schema: "campaign",
                table: "condition",
                newName: "is_stakeable");
        }
    }
}
