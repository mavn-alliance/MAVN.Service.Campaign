// <auto-generated />
using System;
using MAVN.Service.Campaign.MsSqlRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MAVN.Service.Campaign.MsSqlRepositories.Migrations
{
    [DbContext(typeof(CampaignContext))]
    [Migration("20190905101404_EarnRuleConversionRate")]
    partial class EarnRuleConversionRate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("campaign")
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.BonusTypeEntity", b =>
                {
                    b.Property<string>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("type")
                        .HasColumnType("varchar(64)");

                    b.Property<bool>("AllowConversionRate")
                        .HasColumnName("allow_conversion_rate");

                    b.Property<bool>("AllowInfinite")
                        .HasColumnName("allow_infinite");

                    b.Property<bool>("AllowPercentage")
                        .HasColumnName("allow_percentage");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnName("creation_date");

                    b.Property<string>("DisplayName")
                        .HasColumnName("display_name");

                    b.Property<bool>("IsAvailable")
                        .HasColumnName("is_available");

                    b.Property<string>("Vertical")
                        .HasColumnName("vertical");

                    b.HasKey("Type");

                    b.ToTable("bonus_type");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.BurnRuleContentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<Guid>("BurnRuleId")
                        .HasColumnName("burn_rule_id");

                    b.Property<int>("Localization")
                        .HasColumnName("localization");

                    b.Property<int>("RuleContentType")
                        .HasColumnName("content_type");

                    b.Property<string>("Value")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("BurnRuleId");

                    b.ToTable("burn_rule_contents");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.BurnRuleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<decimal>("AmountInCurrency")
                        .HasColumnName("amount_in_currency");

                    b.Property<long>("AmountInTokens")
                        .HasColumnName("amount_in_tokens");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnName("creation_date");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Title")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("burn_rules");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.CampaignEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<decimal>("AmountInCurrency")
                        .HasColumnName("amount_in_currency");

                    b.Property<long>("AmountInTokens")
                        .HasColumnName("amount_in_tokens");

                    b.Property<int>("CompletionCount")
                        .HasColumnName("completion_count");

                    b.Property<string>("CreatedBy")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnName("creation_date");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<DateTime>("FromDate")
                        .HasColumnName("from_date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsEnabled")
                        .HasColumnName("is_enabled");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<decimal>("Reward")
                        .HasColumnName("reward");

                    b.Property<string>("RewardType")
                        .HasColumnName("reward_type");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnName("to_date");

                    b.HasKey("Id");

                    b.ToTable("campaign");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.ConditionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("BonusTypeName")
                        .HasColumnName("bonus_type");

                    b.Property<Guid>("CampaignEntityId")
                        .HasColumnName("campaign_id");

                    b.Property<int>("CompletionCount")
                        .HasColumnName("completion_count");

                    b.Property<decimal>("ImmediateReward")
                        .HasColumnName("immediate_reward");

                    b.HasKey("Id");

                    b.HasIndex("BonusTypeName");

                    b.HasIndex("CampaignEntityId");

                    b.ToTable("condition");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.ConditionPartnerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<Guid>("ConditionEntityId")
                        .HasColumnName("condition_id");

                    b.Property<Guid>("PartnerId")
                        .HasColumnName("partner_id");

                    b.HasKey("Id");

                    b.HasIndex("ConditionEntityId");

                    b.ToTable("condition_partners");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.Configuration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("LastProcessedDate");

                    b.HasKey("Id");

                    b.ToTable("configuration");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.EarnRuleContentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<Guid>("CampaignId")
                        .HasColumnName("campaign_id");

                    b.Property<int>("Localization")
                        .HasColumnName("localization");

                    b.Property<int>("RuleContentType")
                        .HasColumnName("content_type");

                    b.Property<string>("Value")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("earn_rule_contents");
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.BurnRuleContentEntity", b =>
                {
                    b.HasOne("MAVN.Service.Campaign.MsSqlRepositories.Entities.BurnRuleEntity", "BurnRule")
                        .WithMany("BurnRuleContents")
                        .HasForeignKey("BurnRuleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.ConditionEntity", b =>
                {
                    b.HasOne("MAVN.Service.Campaign.MsSqlRepositories.Entities.BonusTypeEntity", "BonusTypeEntity")
                        .WithMany("ConditionEntities")
                        .HasForeignKey("BonusTypeName");

                    b.HasOne("MAVN.Service.Campaign.MsSqlRepositories.Entities.CampaignEntity", "CampaignEntity")
                        .WithMany("ConditionEntities")
                        .HasForeignKey("CampaignEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.ConditionPartnerEntity", b =>
                {
                    b.HasOne("MAVN.Service.Campaign.MsSqlRepositories.Entities.ConditionEntity", "ConditionEntity")
                        .WithMany("ConditionPartners")
                        .HasForeignKey("ConditionEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MAVN.Service.Campaign.MsSqlRepositories.Entities.EarnRuleContentEntity", b =>
                {
                    b.HasOne("MAVN.Service.Campaign.MsSqlRepositories.Entities.CampaignEntity", "Campaign")
                        .WithMany("EarnRuleContents")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
