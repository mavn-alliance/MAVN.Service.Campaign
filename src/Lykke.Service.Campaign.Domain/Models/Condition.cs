using System;
using System.Collections.Generic;
using Falcon.Numerics;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Domain.Enums;

namespace Lykke.Service.Campaign.Domain.Models
{
    /// <summary>
    /// Represents an earn rule condition.
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The earn rule identifier.
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// The bonus type associated with condition.
        /// </summary>
        public BonusType BonusType { get; set; }

        /// <summary>
        /// The amount of immediate reward. 
        /// </summary>
        public Money18 ImmediateReward { get; set; }

        /// <summary>
        /// The number of completion.
        /// </summary>
        public int CompletionCount { get; set; }

        /// <summary>
        /// Identify if the condition has staking
        /// </summary>
        public bool HasStaking { get; set; }

        /// <summary>
        /// Represents stake amount 
        /// </summary>
        public Money18? StakeAmount { get; set; }

        /// <summary>
        /// Represents a staking period
        /// </summary>
        public int? StakingPeriod { get; set; }

        /// <summary>
        /// Represents stake warning period
        /// </summary>
        public int? StakeWarningPeriod { get; set; }

        /// <summary>
        /// Represents staking percentage
        /// </summary>
        public decimal? StakingRule { get; set; }

        /// <summary>
        /// Represents staking burning percent
        /// </summary>
        public decimal? BurningRule { get; set; }

        /// <summary>
        /// Indicates the reward type.
        /// </summary>
        public RewardType RewardType { get; set; }

        /// <summary>
        /// Represents a display value when percentage reward type is selected
        /// </summary>
        public Money18? ApproximateAward { get; set; }

        /// <summary>
        /// The amount in tokens to calculate rate.
        /// </summary>
        public Money18? AmountInTokens { get; set; }

        /// <summary>
        /// The amount in currency to calculate rate.
        /// </summary>
        public decimal? AmountInCurrency { get; set; }

        /// <summary>
        /// Indicates that the partner currency rate should be used to convert an amount.
        /// </summary>
        public bool UsePartnerCurrencyRate { get; set; }

        /// <summary>
        /// The collection of associated partners.
        /// </summary>
        public List<Guid> PartnerIds { get; set; }
            = new List<Guid>();

        /// <summary>
        /// Represents a condition ratio
        /// </summary>
        [CanBeNull]
        public RewardRatioAttributeModel RewardRatio { get; set; }
    }
}
