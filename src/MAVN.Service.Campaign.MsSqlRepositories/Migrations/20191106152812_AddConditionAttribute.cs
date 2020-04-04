using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddConditionAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "reward_has_ration",
                schema: "campaign",
                table: "bonus_type",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "condition_attribute",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    condition_id = table.Column<Guid>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    json_value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condition_attribute", x => x.id);
                    table.ForeignKey(
                        name: "FK_condition_attribute_condition_condition_id",
                        column: x => x.condition_id,
                        principalSchema: "campaign",
                        principalTable: "condition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_condition_attribute_condition_id",
                schema: "campaign",
                table: "condition_attribute",
                column: "condition_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "condition_attribute",
                schema: "campaign");

            migrationBuilder.DropColumn(
                name: "reward_has_ration",
                schema: "campaign",
                table: "bonus_type");
        }
    }
}
