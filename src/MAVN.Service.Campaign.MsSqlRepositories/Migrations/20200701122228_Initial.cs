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
                    type = table.Column<string>(nullable: false),
                    display_name = table.Column<string>(nullable: true),
                    is_available = table.Column<bool>(nullable: false),
                    vertical = table.Column<string>(nullable: true),
                    allow_infinite = table.Column<bool>(nullable: false),
                    allow_percentage = table.Column<bool>(nullable: false),
                    is_stakeable = table.Column<bool>(nullable: false),
                    allow_conversion_rate = table.Column<bool>(nullable: false),
                    creation_date = table.Column<DateTime>(nullable: false),
                    is_hidden = table.Column<bool>(nullable: false),
                    order = table.Column<int>(nullable: false),
                    reward_has_ration = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bonus_type", x => x.type);
                });

            migrationBuilder.CreateTable(
                name: "burn_rules",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    amount_in_tokens = table.Column<string>( nullable: true),
                    amount_in_currency = table.Column<decimal>(nullable: true),
                    use_partner_currency_rate = table.Column<bool>(nullable: false),
                    vertical = table.Column<string>(nullable: true),
                    price = table.Column<decimal>(nullable: true),
                    creation_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burn_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campaign",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    reward = table.Column<string>(nullable: false),
                    approximate_reward = table.Column<string>(nullable: true),
                    reward_type = table.Column<string>(nullable: true),
                    amount_in_tokens = table.Column<string>( nullable: true),
                    amount_in_currency = table.Column<decimal>(nullable: true),
                    use_partner_currency_rate = table.Column<bool>(nullable: false),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: true),
                    completion_count = table.Column<int>(nullable: false),
                    is_deleted = table.Column<bool>(nullable: false),
                    is_enabled = table.Column<bool>(nullable: false),
                    creation_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configuration",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    LastProcessedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "burn_rule_contents",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    burn_rule_id = table.Column<Guid>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    localization = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burn_rule_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_burn_rule_contents_burn_rules_burn_rule_id",
                        column: x => x.burn_rule_id,
                        principalSchema: "campaign",
                        principalTable: "burn_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "burn_rule_partners",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    burn_rule_id = table.Column<Guid>(nullable: false),
                    partner_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_burn_rule_partners", x => x.id);
                    table.ForeignKey(
                        name: "FK_burn_rule_partners_burn_rules_burn_rule_id",
                        column: x => x.burn_rule_id,
                        principalSchema: "campaign",
                        principalTable: "burn_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "condition",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    bonus_type = table.Column<string>(nullable: true),
                    immediate_reward = table.Column<string>( nullable: false),
                    approximate_reward = table.Column<string>(nullable: true),
                    completion_count = table.Column<int>(nullable: false),
                    campaign_id = table.Column<Guid>(nullable: false),
                    has_staking = table.Column<bool>(nullable: false),
                    stake_amount = table.Column<string>(nullable: true),
                    staking_period = table.Column<int>(nullable: true),
                    stake_warning_period = table.Column<int>(nullable: true),
                    staking_rule = table.Column<decimal>(nullable: true),
                    burning_rule = table.Column<decimal>(nullable: true),
                    amount_in_tokens = table.Column<string>(nullable: true),
                    amount_in_currency = table.Column<decimal>(nullable: true),
                    use_partner_currency_rate = table.Column<bool>(nullable: false),
                    reward_type = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "earn_rule_contents",
                schema: "campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    campaign_id = table.Column<Guid>(nullable: false),
                    content_type = table.Column<int>(nullable: false),
                    localization = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_earn_rule_contents", x => x.id);
                    table.ForeignKey(
                        name: "FK_earn_rule_contents_campaign_campaign_id",
                        column: x => x.campaign_id,
                        principalSchema: "campaign",
                        principalTable: "campaign",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_burn_rule_contents_burn_rule_id",
                schema: "campaign",
                table: "burn_rule_contents",
                column: "burn_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_burn_rule_partners_burn_rule_id",
                schema: "campaign",
                table: "burn_rule_partners",
                column: "burn_rule_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_condition_attribute_condition_id",
                schema: "campaign",
                table: "condition_attribute",
                column: "condition_id");

            migrationBuilder.CreateIndex(
                name: "IX_condition_partners_condition_id",
                schema: "campaign",
                table: "condition_partners",
                column: "condition_id");

            migrationBuilder.CreateIndex(
                name: "IX_earn_rule_contents_campaign_id",
                schema: "campaign",
                table: "earn_rule_contents",
                column: "campaign_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "burn_rule_contents",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "burn_rule_partners",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "condition_attribute",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "condition_partners",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "configuration",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "earn_rule_contents",
                schema: "campaign");

            migrationBuilder.DropTable(
                name: "burn_rules",
                schema: "campaign");

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
