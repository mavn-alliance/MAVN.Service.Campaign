using System;
using System.Collections.Generic;
using Falcon.Numerics;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.PartnerManagement.Client.Models;

namespace Lykke.Service.Campaign.Client.Models.Condition
{
    /// <summary>
    /// Represents a condition localized response 
    /// </summary>
    public class ConditionLocalizedResponse
    {
        /// <summary>Represents Identifier of the Condition</summary>
        public string Id { get; set; }

        /// <summary>Represents Condition Type</summary>
        public string Type { get; set; }

        /// <summary>Bonus type vertical.</summary>
        public Vertical? Vertical { get; set; }

        /// <summary>Is hidden flag</summary>
        public bool IsHidden { get; set; }

        /// <summary>Represents Bonus Type Display Name</summary>
        public string DisplayName { get; set; }

        /// <summary>Represents the reward that is going to be granted once the condition is met</summary>
        public Money18 ImmediateReward { get; set; }

        /// <summary>
        /// Represents how many times a condition should be fulfilled before completion.
        /// null for infinity or number greater than 0.
        /// </summary>
        public int? CompletionCount { get; set; }

        /// <summary>Represents a list with partners' identifiers of the condition.</summary>
        public List<Guid> PartnerIds { get; set; }

        /// <summary>Identify if the condition has staking</summary>
        public bool HasStaking { get; set; }

        /// <summary>Represents stake amount</summary>
        public Money18? StakeAmount { get; set; }

        /// <summary>Represents a staking period</summary>
        public int? StakingPeriod { get; set; }

        /// <summary>Represents stake warning period</summary>
        public int? StakeWarningPeriod { get; set; }

        /// <summary>Represents staking percentage</summary>
        public decimal? StakingRule { get; set; }

        /// <summary>Represents staking burning percent</summary>
        public decimal? BurningRule { get; set; }

        /// <summary>Indicates the reward type.</summary>
        public RewardType RewardType { get; set; }

        /// <summary>Indicates if the reward type is percentage or conversion rate</summary>
        public bool IsApproximate { get; set; }

        /// <summary>Represents a display amount value when percentage reward type is selected</summary>
        public Money18? ApproximateAward { get; set; }

        /// <summary>The amount in tokens to calculate rate.</summary>
        public Money18? AmountInTokens { get; set; }

        /// <summary>The amount in currency to calculate rate.</summary>
        public decimal? AmountInCurrency { get; set; }

        /// <summary>Indicates that the partner currency rate should be used to convert an amount.</summary>
        public bool UsePartnerCurrencyRate { get; set; }

        /// <summary>Represents a condition reward ratio attribute</summary>
        public RewardRatioAttributeDetailsResponseModel RewardRatio { get; set; }
    }
}
