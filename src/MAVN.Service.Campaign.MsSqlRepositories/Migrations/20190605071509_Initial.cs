using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "campaign");

            migrationBuilder.CreateTable(
                name: "bonus_type",
                schema: "campaign",
                columns: table => new
                {
                    type = table.Column<string>(type: "varchar(64)", nullable: false),
                    display_name = table.Column<string>(nullable: true),
                    is_available = table.Column<bool>(nullable: false),
                    creation_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonus_type", x => x.type);
                });

            migrationBuilder.CreateTable(
                name: "campaign",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    reward = table.Column<decimal>(nullable: false),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    completion_count = table.Column<int>(nullable: false),
                    creation_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    is_enabled = table.Column<bool>(nullable: false),
                    reward_type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "condition",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    bonus_type = table.Column<string>(nullable: true),
                    immediate_reward = table.Column<decimal>(nullable: false),
                    completion_count = table.Column<int>(nullable: false),
                    campaign_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condition", x => x.id);
                    table.ForeignKey(
                        name: "FK_condition_bonus_type_bonus_type",
                        column: x => x.bonus_type,
                        principalSchema: "campaign",
                        principalTable: "bonus_type",
                        principalColumn: "type",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_condition_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalSchema: "campaign",
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_condition_bonus_type",
                schema: "campaign",
                table: "condition",
                column: "bonus_type");

            migrationBuilder.CreateIndex(
                name: "IX_condition_campaign_id",
                schema: "campaign",
                table: "condition",
                column: "campaign_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "condition",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "bonus_type",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "campaign",
                schema: "campaign");
        }
    }
}
