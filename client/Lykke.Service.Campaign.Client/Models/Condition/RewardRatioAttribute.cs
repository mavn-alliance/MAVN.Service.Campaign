using System.Collections.Generic;

namespace Lykke.Service.Campaign.Client.Models.Condition
{
    /// <summary>
    /// Represents condition reward ratio attribute
    /// </summary>
    public class RewardRatioAttribute
    {
        /// <summary>
        /// Represents a list of condition's ratios 
        /// </summary>
        public IReadOnlyList<RatioAttribute> Ratios { get; set; }
        = new List<RatioAttribute>();
    }
}
