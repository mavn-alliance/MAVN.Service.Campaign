using System;
using MAVN.Numerics;
using MAVN.Service.Campaign.Client.Models.Enums;

namespace MAVN.Service.Campaign.Client.Models.Campaign.Requests
{
    /// <summary>
    /// Represents Campaign Entity
    /// </summary>
    public class CampaignBaseModel
    {
        /// <summary>
        /// Represents Campaign name
        /// Required field (3-50 characters)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents Description of the Campaign
        /// Here more detailed information about the campaign can be given
        /// This is plain text, and can include links
        /// Required field (3-200 characters)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents flag if Campaign is enabled or disabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Represents the Reward that is going to be granted once all conditions are met
        /// Reward must be greater or equal to 0, and must be whole number if the RewardType is fixed
        /// Reward must be greater than 0 if the RewardType is Percentage
        /// </summary>
        public Money18? Reward { get; set; }

        /// <summary>
        /// Represents the reward type of given campaign
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
        /// Represents Start Date of the Campaign
        /// From Date is required (must be in the future)
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Represents End Date of the Campaign
        /// If To Date is not specified, the campaign is ongoing forever
        /// If To Date has value, it has to be after From Date
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Represents how many times, campaign can be completed
        /// The value should be greater than 0 or null for infinity
        /// </summary>
        public int? CompletionCount { get; set; } = 1;

        /// <summary>
        /// Indicates the order of the burn rule.
        /// </summary>
        public int Order { get; set; }
    }
}
