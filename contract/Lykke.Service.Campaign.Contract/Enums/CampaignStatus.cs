namespace Lykke.Service.Campaign.Contract.Enums
{
    // TODO: Rename to earn rule status.
    /// <summary>
    /// Specifies the earn rule status.
    /// </summary>
    public enum CampaignStatus
    {
        /// <summary>
        /// Indicates that the earn rule has been created and not activated.
        /// </summary>
        Pending,

        /// <summary>
        /// Indicates that the earn rule is active.
        /// </summary>
        Active,

        /// <summary>
        /// Indicates that the earn rule is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// Indicates that the earn rule is deleted.
        /// </summary>
        Inactive
    }
}
