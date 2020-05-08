using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using MAVN.Numerics;
using MAVN.Service.Campaign.Domain.Enums;

namespace MAVN.Service.Campaign.Domain.Models.EarnRules
{
    /// <summary>
    /// Represents a earn rule that contains content in a specific language.
    /// </summary>
    public class EarnRuleLocalizedModel
    {
        /// <summary>
        /// The earn rule unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The earn rule localized title. 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The earn rule localized description. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The earn rule localized imageUrl. 
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Represents the Reward that is going to be granted once all conditions are met
        /// </summary>
        public Money18 Reward { get; set; }

        /// <summary>
        /// Type of the reward for the Earn Rule
        /// </summary>
        public RewardType RewardType { get; set; }

        /// <summary>
        /// The amount in tokens to calculate rate.
        /// </summary>
        public Money18? AmountInTokens { get; set; }

        /// <summary>
        /// Represents a display value when percentage reward type is selected
        /// </summary>
        public Money18? ApproximateAward { get; set; }

        /// <summary>
        /// Indicates if the reward type is percentage or conversion rate
        /// </summary>
        public bool IsApproximate { get; set; }

        /// <summary>
        /// The amount in currency to calculate rate.
        /// </summary>
        public decimal? AmountInCurrency { get; set; }

        /// <summary>
        /// Indicates that the partner currency rate should be used to convert an amount.
        /// </summary>
        public bool UsePartnerCurrencyRate { get; set; }

        /// <summary>
        /// Represents Start Date of the Earn Rule
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Represents End Date of the Earn Rule
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Represents how many times, Earn Rule can be completed
        /// </summary>
        public int CompletionCount { get; set; }

        /// <summary>
        /// Represents the creation date of the Earn Rule
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Represents identification of User who created Earn Rule
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents status of the Earn Rule
        /// </summary>
        public CampaignStatus Status { get; set; }

        /// <summary>
        /// Represents a list of Conditions' names
        /// </summary>
        public IReadOnlyList<ConditionLocalizedModel> Conditions { get; set; }

        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }
    }
}
