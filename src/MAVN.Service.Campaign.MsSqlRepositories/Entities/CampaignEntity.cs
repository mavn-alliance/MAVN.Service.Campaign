using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MAVN.Numerics;
using MAVN.Service.Campaign.Domain.Enums;

namespace MAVN.Service.Campaign.MsSqlRepositories.Entities
{
    // TODO: Rename to EarnRuleEntity
    [Table("campaign")]
    public class CampaignEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("reward", TypeName = "nvarchar(64)")]
        public Money18 Reward { get; set; }

        [Column("approximate_reward", TypeName = "nvarchar(64)")]
        public Money18? ApproximateAward { get; set; }

        [Column("reward_type")] 
        public RewardType? RewardType { get; set; }

        [Column("amount_in_tokens", TypeName = "nvarchar(64)")]
        public Money18? AmountInTokens { get; set; }

        [Column("amount_in_currency")]
        public decimal? AmountInCurrency { get; set; }

        [Column("use_partner_currency_rate")]
        public bool UsePartnerCurrencyRate { get; set; }
        
        [Column("from_date")]
        public DateTime FromDate { get; set; }

        [Column("to_date")]
        public DateTime? ToDate { get; set; }

        [Column("completion_count")]
        public int CompletionCount { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("order")]
        public int Order { get; set; }

        public ICollection<ConditionEntity> ConditionEntities { get; set; }

        public ICollection<EarnRuleContentEntity> EarnRuleContents { get; set; }
    }
}
