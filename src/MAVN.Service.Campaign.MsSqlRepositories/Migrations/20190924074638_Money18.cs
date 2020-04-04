using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class Money18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "immediate_reward",
                schema: "campaign",
                table: "condition",
                type: "nvarchar(64)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "reward",
                schema: "campaign",
                table: "campaign",
                type: "nvarchar(64)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "campaign",
                type: "nvarchar(64)",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "campaign",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<bool>(
                name: "use_partner_currency_rate",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "burn_rules",
                type: "nvarchar(64)",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "burn_rules",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<bool>(
                name: "use_partner_currency_rate",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "use_partner_currency_rate",
                schema: "campaign",
                table: "campaign");

            migrationBuilder.DropColumn(
                name: "use_partner_currency_rate",
                schema: "campaign",
                table: "burn_rules");

            migrationBuilder.AlterColumn<decimal>(
                name: "immediate_reward",
                schema: "campaign",
                table: "condition",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)");

            migrationBuilder.AlterColumn<decimal>(
                name: "reward",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)");

            migrationBuilder.AlterColumn<long>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "campaign",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "amount_in_tokens",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_in_currency",
                schema: "campaign",
                table: "burn_rules",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
