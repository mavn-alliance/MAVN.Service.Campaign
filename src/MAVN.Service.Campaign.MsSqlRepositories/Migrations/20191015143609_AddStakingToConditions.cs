using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddStakingToConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_stakeable",
                schema: "campaign",
                table: "condition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "stake_amount",
                schema: "campaign",
                table: "condition",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stake_warning_period",
                schema: "campaign",
                table: "condition",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "staking_period",
                schema: "campaign",
                table: "condition",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "staking_rule",
                schema: "campaign",
                table: "condition",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_stakeable",
                schema: "campaign",
                table: "bonus_type",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_stakeable",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "stake_amount",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "stake_warning_period",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "staking_period",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "staking_rule",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "is_stakeable",
                schema: "campaign",
                table: "bonus_type");
        }
    }
}
