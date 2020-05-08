using System;
using System.Data.Common;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using MAVN.Service.PartnerManagement.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories
{
    public class CampaignContext : MsSqlContext
    {
        private const string Schema = "campaign";

        public DbSet<CampaignEntity> Campaigns { get; set; }
        public DbSet<ConditionEntity> ConditionEntities { get; set; }
        public DbSet<BonusTypeEntity> BonusTypeEntities { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<BurnRuleEntity> BurnRules { get; set; }
        public DbSet<BurnRuleContentEntity> BurnRuleContents { get; set; }
        public DbSet<BurnRulePartnerEntity> BurnRulePartners { get; set; }
        public DbSet<EarnRuleContentEntity> EarnRuleContents { get; set; }
        public DbSet<ConditionPartnerEntity> ConditionPartners { get; set; }
        public DbSet<ConditionAttributeEntity> ConditionAttributes { get; set; }

        public CampaignContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        // empty constructor needed for EF migrations
        [UsedImplicitly]
        public CampaignContext() : base(Schema)
        {
        }

        //Needed constructor for using InMemoryDatabase for tests
        public CampaignContext(DbContextOptions options)
            : base(Schema, options)
        {
        }

        public CampaignContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
            //CampaignEntity
            modelBuilder.Entity<CampaignEntity>()
                .HasMany(e => e.ConditionEntities)
                .WithOne(e => e.CampaignEntity)
                .HasForeignKey(e => e.CampaignEntityId);

            modelBuilder.Entity<CampaignEntity>()
                .HasMany(e => e.EarnRuleContents)
                .WithOne(e => e.Campaign)
                .HasForeignKey(e => e.CampaignId);

            modelBuilder.Entity<CampaignEntity>()
                .Property(e => e.RewardType)
                .HasConversion<string>();

            //BonusTypeEntity
            modelBuilder.Entity<BonusTypeEntity>()
                .HasMany(e => e.ConditionEntities)
                .WithOne(e => e.BonusTypeEntity)
                .HasForeignKey(e => e.BonusTypeName);
            
            modelBuilder
                .Entity<BonusTypeEntity>()
                .Property(entity => entity.Vertical)
                .HasConversion(vertical => vertical.ToString(), vertical => Parse(vertical));

            //BurnRuleEntity
            modelBuilder.Entity<BurnRuleEntity>()
                .HasMany(s => s.BurnRuleContents)
                .WithOne(s => s.BurnRule)
                .HasForeignKey(s => s.BurnRuleId);

            modelBuilder.Entity<BurnRuleEntity>()
                .HasMany(s => s.BurnRulePartners)
                .WithOne(s => s.BurnRuleEntity)
                .HasForeignKey(s => s.BurnRuleEntityId);

            modelBuilder
                .Entity<BurnRuleEntity>()
                .Property(entity => entity.Vertical)
                .HasConversion(
                    vertical => vertical.ToString(),
                    vertical => Parse(vertical));

            //Conditions
            modelBuilder.Entity<ConditionEntity>()
                .HasMany(e => e.ConditionPartners)
                .WithOne(e => e.ConditionEntity)
                .HasForeignKey(e => e.ConditionEntityId);

            modelBuilder.Entity<ConditionEntity>()
                .HasMany(e => e.Attributes)
                .WithOne(e => e.Condition)
                .HasForeignKey(e => e.ConditionId);
        }

        private static Vertical? Parse(string value)
        {
            if (Enum.TryParse<Vertical>(value, out var vertical))
                return vertical;

            return null;
        }
    }
}
