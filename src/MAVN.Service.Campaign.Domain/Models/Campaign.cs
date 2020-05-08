using MAVN.Service.Campaign.Domain.Enums;
using System;
using System.Collections.Generic;
using MAVN.Numerics;

namespace MAVN.Service.Campaign.Domain.Models
{
    // TODO: Rename to earn rule.
    /// <summary>
    /// Represents an earn rule.
    /// </summary>
    public class Campaign
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The additional information.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The amount of reward.
        /// </summary>
        /// <remarks>
        /// The value depends of <see cref="RewardType"/>.
        /// Contains percentage if <see cref="RewardType"/> is <see cref="Enums.RewardType.Percentage"/>.
        /// Contains fixed amount if <see cref="RewardType"/> is <see cref="Enums.RewardType.Fixed"/>.
        /// </remarks>
        public Money18 Reward { get; set; }

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
        /// The valid from date.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// The valid until date.
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// The number of required completions.
        /// </summary>
        public int CompletionCount { get; set; }

        /// <summary>
        /// Indicates that the earn rule deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Indicates that the earn rule enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The date and time of creation.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The unique identifier of creator.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The collection of conditions.
        /// </summary>
        public IReadOnlyList<Condition> Conditions { get; set; }

        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The earn rule status.
        /// </summary>
        public CampaignStatus CampaignStatus
        {
            get
            {
                if (!IsEnabled)
                {
                    return CampaignStatus.Inactive;
                }

                if (DateTime.UtcNow < FromDate)
                {
                    return CampaignStatus.Pending;
                }

                if (!ToDate.HasValue || ToDate.Value > DateTime.UtcNow)
                {
                    return CampaignStatus.Active;
                }

                return CampaignStatus.Completed;
            }
        }
    }
}
