using JetBrains.Annotations;

namespace MAVN.Service.Campaign.Client.Models.Condition
{
    /// <inheritdoc />
    /// <summary>
    /// Represent a Condition Create model
    /// </summary>
    [PublicAPI]
    public class ConditionCreateModel : ConditionBaseModel
    {
        /// <summary>
        /// Represents a condition reward ratio attribute
        /// </summary>
        [CanBeNull]
        public RewardRatioAttribute RewardRatio { get; set; }
    }
}
