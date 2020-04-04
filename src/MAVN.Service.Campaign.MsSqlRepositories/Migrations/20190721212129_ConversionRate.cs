using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class ConversionRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_in_currency",
                schema: "campaign",
                table: "burn_rules");

            migrationBuilder.DropColumn(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "burn_rules");
        }
    }
}
