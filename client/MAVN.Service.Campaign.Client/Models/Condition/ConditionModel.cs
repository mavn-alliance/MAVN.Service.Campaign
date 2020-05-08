using System;
using JetBrains.Annotations;
using MAVN.Service.PartnerManagement.Client.Models;

namespace MAVN.Service.Campaign.Client.Models.Condition
{
    /// <inheritdoc />
    /// <summary>
    /// Represent a Condition Info model
    /// </summary>
    [PublicAPI]
    public class ConditionModel : ConditionBaseModel
    {
        /// <summary>
        /// Represents Identifier of the Condition
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents Bonus Type Display Name
        /// </summary>
        public string TypeDisplayName { get; set; }

        /// <summary>
        /// Represents BonusType's vertical
        /// </summary>
        public Vertical? Vertical { get; set; }

        /// <summary>
        /// Indicates if the bonus type is hidden
        /// </summary>
        public bool IsHiddenType { get; set; }

        /// <summary>
        /// Represents condition's campaignId
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Represents a condition reward ratio attribute
        /// </summary>
        public RewardRatioAttributeDetailsResponseModel RewardRatio { get; set; }
    }
}
