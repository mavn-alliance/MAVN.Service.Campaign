using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Numerics;
using MAVN.Service.Campaign.Domain.Enums;

namespace MAVN.Service.Campaign.MsSqlRepositories.Entities
{
    [Table("condition")]
    public class ConditionEntity : BaseEntity
    {
        [Column("bonus_type")]
        public string BonusTypeName { get; set; }
        
        [Column("immediate_reward")]
        public Money18 ImmediateReward { get; set; }

        [Column("approximate_reward")]
        public Money18? ApproximateAward { get; set; }

        [Column("completion_count")]
        public int CompletionCount { get; set; }

        [Column("campaign_id")]
        public Guid CampaignEntityId { get; set; }

        public BonusTypeEntity BonusTypeEntity { get; set; }

        [Column("has_staking")]
        public bool HasStaking { get; set; }

        [Column("stake_amount")]
        public Money18? StakeAmount { get; set; }

        [Column("staking_period")]
        public int? StakingPeriod { get; set; }

        [Column("stake_warning_period")]
        public int? StakeWarningPeriod { get; set; }

        [Column("staking_rule")]
        public decimal? StakingRule { get; set; }

        [Column("burning_rule")]
        public decimal? BurningRule { get; set; }

        [Column("amount_in_tokens")]
        public Money18? AmountInTokens { get; set; }

        [Column("amount_in_currency")]
        public decimal? AmountInCurrency { get; set; }

        [Column("use_partner_currency_rate")]
        public bool UsePartnerCurrencyRate { get; set; }

        [Column("reward_type")]
        public RewardType? RewardType { get; set; }

        // TODO: Rename to EarnRule
        public CampaignEntity CampaignEntity { get; set; }

        public ICollection<ConditionPartnerEntity> ConditionPartners { get; set; }

        public ICollection<ConditionAttributeEntity> Attributes { get; set; }
    }
}
