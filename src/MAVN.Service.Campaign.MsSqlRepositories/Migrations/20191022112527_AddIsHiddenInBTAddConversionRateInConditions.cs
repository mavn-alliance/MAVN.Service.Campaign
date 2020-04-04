using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddIsHiddenInBTAddConversionRateInConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "condition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "condition",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reward_type",
                schema: "campaign",
                table: "condition",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "use_partner_currency_rate",
                schema: "campaign",
                table: "condition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_hidden",
                schema: "campaign",
                table: "bonus_type",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_in_currency",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "reward_type",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "use_partner_currency_rate",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "is_hidden",
                schema: "campaign",
                table: "bonus_type");
        }
    }
}
