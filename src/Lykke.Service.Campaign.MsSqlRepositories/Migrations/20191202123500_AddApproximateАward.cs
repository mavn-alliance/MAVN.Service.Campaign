using Microsoft.EntityFrameworkCore.Migrations;

namespace Lykke.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddApproximateАward : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "approximate_reward",
                schema: "campaign",
                table: "condition",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "approximate_reward",
                schema: "campaign",
                table: "campaign",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.Sql(
                @"UPDATE campaign.campaign
                  SET approximate_reward = 0
                  WHERE reward_type is not null
                  and reward_type = 'Percentage' or reward_type = 'ConversionRate'
                  and  approximate_reward is null");

            migrationBuilder.Sql(
                @"UPDATE campaign.condition
                  SET approximate_reward = 0
                  WHERE reward_type is not null
                  and reward_type = 1 or reward_type = 2
                  and  approximate_reward is null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approximate_reward",
                schema: "campaign",
                table: "condition");

            migrationBuilder.DropColumn(
                name: "approximate_reward",
                schema: "campaign",
                table: "campaign");
        }
    }
}
