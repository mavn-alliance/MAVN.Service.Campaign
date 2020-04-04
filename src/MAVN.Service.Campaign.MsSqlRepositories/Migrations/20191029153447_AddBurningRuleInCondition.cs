using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddBurningRuleInCondition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "burning_rule",
                schema: "campaign",
                table: "condition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "burning_rule",
                schema: "campaign",
                table: "condition");
        }
    }
}
