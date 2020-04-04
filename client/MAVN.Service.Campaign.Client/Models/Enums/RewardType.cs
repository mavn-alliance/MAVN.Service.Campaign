namespace MAVN.Service.Campaign.Client.Models.Enums
{
    /// <summary>
    /// Represents a reward type of a campaign
    /// </summary>
    public enum RewardType
    {
        /// <summary>
        /// Fixed amount in the base currency
        /// </summary>
        Fixed,

        /// <summary>
        /// Percentage of given amount
        /// </summary>
        Percentage,

        /// <summary>
        /// Conversion rate of given amount.
        /// </summary>
        ConversionRate
    }
}
