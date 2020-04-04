using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    public partial class AddConditionPartnerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "condition_partners",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    condition_id = table.Column<Guid>(nullable: false),
                    partner_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condition_partners", x => x.id);
                    table.ForeignKey(
                        name: "FK_condition_partners_condition_condition_id",
                        column: x => x.condition_id,
                        principalSchema: "campaign",
                        principalTable: "condition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_condition_partners_condition_id",
                schema: "campaign",
                table: "condition_partners",
                column: "condition_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "condition_partners",
                schema: "campaign");
        }
    }
}
