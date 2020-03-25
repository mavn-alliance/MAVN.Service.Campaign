using Lykke.Service.Campaign.Client.Models.Enums;

namespace Lykke.Service.Campaign.Client.Models.Condition
{
    /// <summary>
    /// Represents condition details response
    /// </summary>
    public class ConditionDetailsResponseModel : CampaignServiceErrorResponseModel
    {
        /// <summary>
        /// Condition
        /// </summary>
        public ConditionModel Condition { get; set; }
    }
}