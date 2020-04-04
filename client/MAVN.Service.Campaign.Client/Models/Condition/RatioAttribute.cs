namespace MAVN.Service.Campaign.Client.Models.Condition
{
    /// <summary>
    /// Represents a ration attribute model
    /// </summary>
    public class RatioAttribute
    {
        /// <summary>
        /// Represents ratio order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Represents reward ratio percent
        /// </summary>
        public decimal RewardRatio { get; set; }

        /// <summary>
        /// Represents payment ratio percent
        /// </summary>
        public decimal PaymentRatio { get; set; }
    }
}
