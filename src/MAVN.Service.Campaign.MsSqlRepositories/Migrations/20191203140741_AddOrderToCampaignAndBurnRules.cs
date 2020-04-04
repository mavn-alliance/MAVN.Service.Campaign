using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddOrderToCampaignAndBurnRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "order",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                schema: "campaign",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "order",
                schema: "campaign",
                table: "burn_rules");
        }
    }
}
