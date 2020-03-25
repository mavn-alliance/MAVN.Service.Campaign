using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class EarnRuleConversionRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "allow_conversion_rate",
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
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "allow_conversion_rate",
                schema: "campaign",
                table: "bonus_type");

            migrationBuilder.AlterColumn<int>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
