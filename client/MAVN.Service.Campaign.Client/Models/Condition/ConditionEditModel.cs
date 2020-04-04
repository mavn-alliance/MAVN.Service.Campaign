using JetBrains.Annotations;

namespace MAVN.Service.Campaign.Client.Models.Condition
{
    /// <inheritdoc />
    /// <summary>
    /// Represent a Condition Edit model
    /// </summary>
    [PublicAPI]
    public class ConditionEditModel : ConditionBaseModel
    {
        /// <summary>
        /// Represents Identifier of the Condition
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents a condition reward ratio attribute
        /// </summary>
        [CanBeNull]
        public RewardRatioAttribute RewardRatio { get; set; }
    }
}
