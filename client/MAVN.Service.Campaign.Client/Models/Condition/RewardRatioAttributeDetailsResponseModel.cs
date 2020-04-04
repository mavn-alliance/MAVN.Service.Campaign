using System.Collections.Generic;

namespace MAVN.Service.Campaign.Client.Models.Condition
{
    /// <summary>
    /// 
    /// </summary>
    public class RewardRatioAttributeDetailsResponseModel
    {
        /// <summary>
        /// Represents a list of condition's ratios 
        /// </summary>
        public IReadOnlyList<RatioAttributeDetailsModel> Ratios { get; set; }
    }
}
