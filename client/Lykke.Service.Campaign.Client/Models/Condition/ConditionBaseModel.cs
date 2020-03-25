using System;
using System.Collections.Generic;
using Falcon.Numerics;
using Lykke.Service.Campaign.Client.Models.Enums;

namespace Lykke.Service.Campaign.Client.Models.Condition
{
    /// <summary>
    /// Represents the Condition entity
    /// </summary>
    public class ConditionBaseModel
    {
        /// <summary>
        /// Represents Condition Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Represents the reward that is going to be granted once the condition is met
        /// </summary>
        public Money18 ImmediateReward { get; set; }

        /// <summary>
        /// Represents how many times a condition should be fulfilled before completion.
        /// null for infinity or number greater than 0.
        /// </summary>
        public int? CompletionCount { get; set; } = 1;

        /// <summary>
        /// Represents a list with partners' identifiers of the condition.
        /// </summary>
        public Guid[] PartnerIds { get; set; }

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
        public int? StakingPeriod { get; set; } = 10;

        /// <summary>
        /// Represents stake warning period
        /// </summary>
        public int? StakeWarningPeriod { get; set; } = 2;

        /// <summary>
        /// Represents staking percentage
        /// </summary>
        public decimal? StakingRule { get; set; } = 0;

        /// <summary>
        /// Represents staking burning percent
        /// </summary>
        public decimal? BurningRule { get; set; }

        /// <summary>
        /// Indicates the reward type.
        /// </summary>
        public RewardType RewardType { get; set; }

        /// <summary>
        /// Represents a display amount value when percentage reward type is selected
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
        /// Indicates if the condition can have reward ratio
        /// </summary>
        public bool RewardHasRatio { get; set; }
    }
}
